using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace FaceDBApp
{
    public partial class FrmPrincipal : Form
    {
        VideoCapture grabber;
        Mat currentFrame;
        CascadeClassifier face;
        string connectionString = "";

        public FrmPrincipal()
        {
            InitializeComponent();

            // Form load event qo‘shamiz
            this.Load += FrmPrincipal_Load;

            // Haarcascade yuklash
            try
            {
                face = new CascadeClassifier("haarcascade_frontalface_default.xml");
            }
            catch
            {
                MessageBox.Show("Haarcascade fayli topilmadi!");
            }

            // DB ulash
            connectionString = LoadConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                // Default path
                string dbFile = Path.Combine(System.Windows.Forms.Application.StartupPath, "FaceDB.mdf");
                CreateDatabase(dbFile);
                connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbFile};Integrated Security=True;Connect Timeout=30";
                SaveConnectionString(connectionString);
            }

            EnsureTable();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            // Rasmlarni yumaloq qilish
            CirclePic(g6); CirclePic(g7); CirclePic(g8); CirclePic(g9); CirclePic(g10);
            CirclePic(r6); CirclePic(r7); CirclePic(r8); CirclePic(r9); CirclePic(r10);
        }

        public void CirclePic(PictureBox pb)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, pb.Width - 1, pb.Height - 1);
            pb.Region = new Region(gp);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        #region DB Functions
        private void CreateDatabase(string dbFile)
        {
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;";
            string dbName = Path.GetFileNameWithoutExtension(dbFile);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string q = $"CREATE DATABASE [{dbName}] ON PRIMARY (NAME={dbName}, FILENAME='{dbFile}')";
                new SqlCommand(q, conn).ExecuteNonQuery();
            }
        }

        private void EnsureTable()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string q = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PersonInfo' AND xtype='U')
                             CREATE TABLE PersonInfo (
                                Id CHAR(9) PRIMARY KEY,
                                Familiya NVARCHAR(50),
                                Ism NVARCHAR(50),
                                Sharif NVARCHAR(50),
                                Unvon NVARCHAR(50),
                                FaceImage VARBINARY(MAX)
                             )";
                new SqlCommand(q, conn).ExecuteNonQuery();
            }
        }

        private void SaveConnectionString(string cs) => File.WriteAllText("dbpath.txt", cs);
        private string LoadConnectionString() => File.Exists("dbpath.txt") ? File.ReadAllText("dbpath.txt") : "";
        #endregion

        #region Camera & Search
        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            try
            {
                if (grabber != null) grabber.Dispose();
                grabber = new VideoCapture(0);
                grabber.ImageGrabbed += FrameGrabber;
                grabber.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kamera xatosi: " + ex.Message);
            }
        }

        private void FrameGrabber(object sender, EventArgs e)
        {
            currentFrame = new Mat();
            grabber.Retrieve(currentFrame);

            using (var imageFrame = currentFrame.ToImage<Bgr, byte>())
            {
                if (imageFrame == null) return;

                var gray = imageFrame.Convert<Gray, byte>();

                Rectangle[] facesDetected = face.DetectMultiScale(gray, 1.2, 10, Size.Empty);

                if (facesDetected.Length > 0)
                {
                    Rectangle f = facesDetected[0];
                    var result = gray.Copy(f).Resize(100, 100, Inter.Cubic);
                    var arr = result.ToJpegData(95);
                    //get a memory stream out of the byte array
                    var stream = new MemoryStream(arr);
                    //pass the memory stream to the bitmap ctor
                    var bitmap = new Bitmap(stream);
                    // ❌ ToBitmap() emas, ✅ result.Bitmap ishlatiladi
                    picFace1.Image = bitmap;

                    // DB dan qidirish
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string q = "SELECT TOP 1 * FROM PersonInfo WHERE Id=@id";
                        SqlCommand cmd = new SqlCommand(q, conn);
                        cmd.Parameters.AddWithValue("@id", txtid.Text);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            txtfamiliya.Text = reader["Familiya"].ToString();
                            txtism.Text = reader["Ism"].ToString();
                            txtsharif.Text = reader["Sharif"].ToString();
                            txtunvoni.Text = reader["Unvon"].ToString();
                            txtid.Text = reader["Id"].ToString();

                            byte[] bytes = (byte[])reader["FaceImage"];
                            picFace1.Image = ByteArrayToImage(bytes);
                        }
                    }

                    grabber.ImageGrabbed -= FrameGrabber; // faqat 1 marta ishlaydi
                }
            }
        }
        #endregion

        #region Add/Update
        /*private async void btnAddPerson_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtid.Text) || txtid.Text.Length != 9)
            {
                MessageBox.Show("ID 9 xonali son bo‘lishi kerak!");
                return;
            }

            List<byte[]> faces = new List<byte[]>();
            for (int i = 0; i < 10; i++)
            {
                currentFrame = new Mat();
                grabber.Retrieve(currentFrame);

                using (var frame = currentFrame.ToImage<Bgr, byte>())
                {
                    var gray = frame.Convert<Gray, byte>();
                    Rectangle[] detected = face.DetectMultiScale(gray, 1.2, 10, Size.Empty);

                    if (detected.Length > 0)
                    {
                        Rectangle f = detected[0];
                        var result = gray.Copy(f).Resize(100, 100, Inter.Cubic);

                        // ❌ ToBitmap() emas
                        faces.Add(ImageToByteArray(result.Bitmap));
                    }
                }
                await Task.Delay(100);
            }

            if (faces.Count == 0)
            {
                MessageBox.Show("Yuz aniqlanmadi!");
                return;
            }

            byte[] finalImg = faces[faces.Count - 1];

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string check = "SELECT COUNT(*) FROM PersonInfo WHERE Id=@id";
                SqlCommand c1 = new SqlCommand(check, conn);
                c1.Parameters.AddWithValue("@id", txtid.Text);
                int exists = (int)c1.ExecuteScalar();

                string q = exists > 0
                    ? "UPDATE PersonInfo SET Familiya=@f, Ism=@i, Sharif=@s, Unvon=@u, FaceImage=@img WHERE Id=@id"
                    : "INSERT INTO PersonInfo (Id,Familiya,Ism,Sharif,Unvon,FaceImage) VALUES (@id,@f,@i,@s,@u,@img)";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@id", txtid.Text);
                cmd.Parameters.AddWithValue("@f", txtfamiliya.Text);
                cmd.Parameters.AddWithValue("@i", txtism.Text);
                cmd.Parameters.AddWithValue("@s", txtsharif.Text);
                cmd.Parameters.AddWithValue("@u", txtunvoni.Text);
                cmd.Parameters.AddWithValue("@img", finalImg);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Ma'lumot saqlandi!");
        }
        */

        private async void btnAddPerson_Click(object sender, EventArgs e)
        {
            // ID daraxti: 9 xonali raqam bo'lishi kerak
            if (string.IsNullOrWhiteSpace(txtid.Text) || !Regex.IsMatch(txtid.Text, @"^\d{9}$"))
            {
                MessageBox.Show("ID 9 xonali raqam bo'lishi kerak (masalan: 000000001).");
                return;
            }

            if (grabber == null)
            {
                MessageBox.Show("Avval kamera ishga tushirilishi lozim (Qidirish tugmasi bilan).");
                return;
            }

            List<byte[]> gathered = new List<byte[]>();

            for (int i = 0; i < 10; i++)
            {
                Mat frame = new Mat();
                grabber.Retrieve(frame);
                if (frame.IsEmpty)
                {
                    await Task.Delay(100); // kutib keyingi
                    continue;
                }
                Mat gray = new Mat();
                CvInvoke.CvtColor(frame, gray, ColorConversion.Bgr2Gray);
                var rects = face.DetectMultiScale(gray, 1.1, 6, new Size(80, 80), Size.Empty);
                if (rects.Length > 0)
                {
                    Mat faceMat = new Mat(frame, rects[0]);
                    Mat faceGray = new Mat();
                    CvInvoke.CvtColor(faceMat, faceGray, ColorConversion.Bgr2Gray);
                    Mat faceResized = new Mat();
                    CvInvoke.Resize(faceGray, faceResized, new Size(100, 100), 0, 0, Inter.Cubic);
                    byte[] png = CvInvoke.Imencode(".png", faceResized);
                    gathered.Add(png);
                }
                await Task.Delay(100);
            }

            if (gathered.Count == 0)
            {
                MessageBox.Show("Yuz topilmadi — rasm olinmadi.");
                return;
            }

            // DB saqlash (insert yoki update)
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1) PersonInfo mavjudligini tekshirish
                    string check = "SELECT COUNT(*) FROM PersonInfo WHERE Id=@id";
                    using (SqlCommand c = new SqlCommand(check, conn))
                    {
                        c.Parameters.AddWithValue("@id", txtid.Text);
                        int exists = (int)c.ExecuteScalar();

                        if (exists == 0)
                        {
                            // insert PersonInfo
                            string ins = "INSERT INTO PersonInfo(Id, Familiya, Ism, Sharif, Unvon) VALUES(@id,@f,@i,@s,@u)";
                            using (SqlCommand ci = new SqlCommand(ins, conn))
                            {
                                ci.Parameters.AddWithValue("@id", txtid.Text);
                                ci.Parameters.AddWithValue("@f", (object)txtfamiliya.Text ?? DBNull.Value);
                                ci.Parameters.AddWithValue("@i", (object)txtism.Text ?? DBNull.Value);
                                ci.Parameters.AddWithValue("@s", (object)txtsharif.Text ?? DBNull.Value);
                                ci.Parameters.AddWithValue("@u", (object)txtunvoni.Text ?? DBNull.Value);
                                ci.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // update PersonInfo
                            string upd = "UPDATE PersonInfo SET Familiya=@f, Ism=@i, Sharif=@s, Unvon=@u WHERE Id=@id";
                            using (SqlCommand cu = new SqlCommand(upd, conn))
                            {
                                cu.Parameters.AddWithValue("@id", txtid.Text);
                                cu.Parameters.AddWithValue("@f", (object)txtfamiliya.Text ?? DBNull.Value);
                                cu.Parameters.AddWithValue("@i", (object)txtism.Text ?? DBNull.Value);
                                cu.Parameters.AddWithValue("@s", (object)txtsharif.Text ?? DBNull.Value);
                                cu.Parameters.AddWithValue("@u", (object)txtunvoni.Text ?? DBNull.Value);
                                cu.ExecuteNonQuery();
                            }

                            // agar update bo'lsa, eski rasmlarni o'chirib yangilarini qo'yish (siz istasangiz saqlab qolishingiz ham mumkin)
                            string del = "DELETE FROM PersonImages WHERE PersonId=@id";
                            using (SqlCommand cd = new SqlCommand(del, conn))
                            {
                                cd.Parameters.AddWithValue("@id", txtid.Text);
                                cd.ExecuteNonQuery();
                            }
                        }

                        // End — end of existence check
                        // 2) Insert images
                        string insImg = "INSERT INTO PersonImages(PersonId, Img) VALUES(@id, @img)";
                        foreach (var b in gathered)
                        {
                            using (SqlCommand ci2 = new SqlCommand(insImg, conn))
                            {
                                ci2.Parameters.AddWithValue("@id", txtid.Text);
                                ci2.Parameters.AddWithValue("@img", b);
                                ci2.ExecuteNonQuery();
                            }
                        }
                    }
                }

                MessageBox.Show("Ma'lumotlar bazasiga rasm(lar) muvaffaqiyatli qo'shildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Saqlashda xato: " + ex.Message);
            }
        }

        #endregion

        #region Delete
        private void picFace_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtid.Text)) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string q = "DELETE FROM PersonInfo WHERE Id=@id";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@id", txtid.Text);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Foydalanuvchi o‘chirildi!");
        }
        #endregion

        #region Helpers
        private byte[] ImageToByteArray(System.Drawing.Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private System.Drawing.Image ByteArrayToImage(byte[] arr)
        {
            using (MemoryStream ms = new MemoryStream(arr))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }
        #endregion
    }
}
