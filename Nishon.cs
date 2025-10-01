using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        // --- EmguCV ---
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);

        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;

        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        int ContTrain, NumLabels;
        string name;

        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\UsersDB.mdf;Integrated Security=True;Connect Timeout=30";

        public FrmPrincipal()
        {
            InitializeComponent();

            // HaarCascade yuklash
            try
            {
                face = new HaarCascade("haarcascade_frontalface_default.xml");
            }
            catch
            {
                MessageBox.Show("Haarcascade fayli topilmadi!");
            }
        }

        // --- Kamera ishga tushirish va qidirish ---
        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            try
            {
                if (grabber != null) grabber.Dispose();
                grabber = new Capture();
                grabber.QueryFrame();
                Application.Idle += new EventHandler(FrameGrabber);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kamera xatosi: " + ex.Message);
            }
        }

        // --- Yuzni DB da qidirish ---
        void FrameGrabber(object sender, EventArgs e)
        {
            try
            {
                currentFrame = grabber.QueryFrame().Resize(320, 240, INTER.CV_INTER_CUBIC);
                gray = currentFrame.Convert<Gray, Byte>();

                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                    face, 1.2, 10, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

                if (facesDetected[0].Length > 0)
                {
                    MCvAvgComp f = facesDetected[0][0];

                    result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                    currentFrame.Draw(f.rect, new Bgr(Color.Green), 2);

                    picFace.Image = result.ToBitmap();

                    // Tanish
                    if (trainingImages.Count > 0)
                    {
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                            trainingImages.ToArray(), labels.ToArray(), 3000, ref termCrit);

                        name = recognizer.Recognize(result);
                        txtid.Text = name;
                    }

                    // Agar txtid bo‘sh bo‘lsa yangi foydalanuvchi sifatida qabul qilamiz
                    if (!string.IsNullOrEmpty(txtid.Text))
                    {
                        LoadUserFromDB(txtid.Text);
                    }

                    imageBoxFrameGrabber.Image = currentFrame;
                    Application.Idle -= FrameGrabber;
                }
                else
                {
                    imageBoxFrameGrabber.Image = currentFrame;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FrameGrabber error: " + ex.Message);
            }
        }

        // --- Ma'lumotlarni DB dan olish ---
        private void LoadUserFromDB(string userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Foydalanuvchilar WHERE UserId=@UserId";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtIsm.Text = reader["Ism"].ToString();
                        txtFamiliya.Text = reader["Familiya"].ToString();
                        txtSharif.Text = reader["Sharif"].ToString();
                        txtUnvon.Text = reader["Unvon"].ToString();
                        txtid.Text = reader["UserId"].ToString();

                        byte[] imgBytes = (byte[])reader["Rasm"];
                        picFace.Image = ByteArrayToImage(imgBytes);
                    }
                    else
                    {
                        MessageBox.Show("Bu foydalanuvchi bazada topilmadi. Yangi foydalanuvchi sifatida qo‘shishingiz mumkin.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("DB xatolik: " + ex.Message);
                }
            }
        }

        // --- Qo‘shish/Yangilash ---
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtid.Text) || txtid.Text.Length != 9)
            {
                MessageBox.Show("9 xonali ID kiriting!");
                return;
            }

            byte[] faceImage = picFace.Image != null ? ImageToByteArray(picFace.Image) : null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM Foydalanuvchilar WHERE UserId=@UserId";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@UserId", txtid.Text);
                    int count = (int)checkCmd.ExecuteScalar();

                    string query;
                    if (count > 0) // update
                    {
                        query = @"UPDATE Foydalanuvchilar 
                                  SET Ism=@Ism, Familiya=@Familiya, Sharif=@Sharif, Unvon=@Unvon, Rasm=@Rasm
                                  WHERE UserId=@UserId";
                    }
                    else // insert
                    {
                        query = @"INSERT INTO Foydalanuvchilar (Ism, Familiya, Sharif, Unvon, UserId, Rasm) 
                                  VALUES (@Ism, @Familiya, @Sharif, @Unvon, @UserId, @Rasm)";
                    }

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Ism", txtIsm.Text);
                    cmd.Parameters.AddWithValue("@Familiya", txtFamiliya.Text);
                    cmd.Parameters.AddWithValue("@Sharif", txtSharif.Text);
                    cmd.Parameters.AddWithValue("@Unvon", txtUnvon.Text);
                    cmd.Parameters.AddWithValue("@UserId", txtid.Text);
                    cmd.Parameters.AddWithValue("@Rasm", (object)faceImage ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show(count > 0 ? "Foydalanuvchi yangilandi." : "Yangi foydalanuvchi qo‘shildi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xatolik: " + ex.Message);
                }
            }
        }

        // --- O‘chirish ---
        private void picFace_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtid.Text)) return;

            DialogResult dr = MessageBox.Show("Haqiqatan ham ushbu foydalanuvchini o‘chirasizmi?", "Delete", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "DELETE FROM Foydalanuvchilar WHERE UserId=@UserId";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@UserId", txtid.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Foydalanuvchi o‘chirildi.");

                        txtIsm.Clear();
                        txtFamiliya.Clear();
                        txtSharif.Clear();
                        txtUnvon.Clear();
                        txtid.Clear();
                        picFace.Image = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Xatolik: " + ex.Message);
                    }
                }
            }
        }

        // --- Convert funksiyalar ---
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
