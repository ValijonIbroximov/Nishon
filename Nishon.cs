using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceDBApp
{
    public partial class FrmPrincipal : Form
    {
        // Kamera va OpenCV ob'ektlari
        private VideoCapture grabber;
        private Mat currentFrame;
        private CascadeClassifier face;

        // DB yo'li va connectionstring
        private string connectionString = "";

        public FrmPrincipal()
        {
            InitializeComponent();

            // Form load event
            this.Load += FrmPrincipal_Load;

            // Haarcascade yuklash — faylni loyihaga qo'shing yoki exe papkaga joylang
            try
            {
                string haarcascade_frontalface_default_Path = Path.Combine(Application.StartupPath, "haarcascade_frontalface_default.xml");
                face = new CascadeClassifier(haarcascade_frontalface_default_Path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Haarcascade fayli topilmadi yoki ochishda xato: " + ex.Message, "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // DB yo'lini yuklash yoki yaratish
            connectionString = LoadConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                // default fayl joyi — exe papkada FaceDB.mdf
                string dbFile = Path.Combine(Application.StartupPath, "FaceDB.mdf");
                try
                {
                    CreateDatabase(dbFile);
                    connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbFile};Integrated Security=True;Connect Timeout=30";
                    SaveConnectionString(connectionString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ma'lumotlar bazasini yaratishda xato: " + ex.Message);
                }
            }

            // kerakli jadvallarni yaratish
            try
            {
                EnsureTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Jadvallarni yaratishda xato: " + ex.Message);
            }
        }

        // ---------- FORM LOAD ----------
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            // Agar sizda g6... r10 kabi PictureBoxlar bo'lsa, ularni yumaloq qilish
            // Misol:
            // CirclePic(g6); CirclePic(g7); ...
            // Agar bunday kontrollerlar yo'q bo'lsa, ushbu qatorlarni olib tashlang yoki komment qiling.
            try
            {
                // agar controllar mavjud bo'lsa aktivlashtiring, aks holda xatolik chiqmasligi uchun tekshirish
                if (this.Controls.ContainsKey("g6")) CirclePic((PictureBox)this.Controls["g6"]);
                if (this.Controls.ContainsKey("g7")) CirclePic((PictureBox)this.Controls["g7"]);
                if (this.Controls.ContainsKey("g8")) CirclePic((PictureBox)this.Controls["g8"]);
                if (this.Controls.ContainsKey("g9")) CirclePic((PictureBox)this.Controls["g9"]);
                if (this.Controls.ContainsKey("g10")) CirclePic((PictureBox)this.Controls["g10"]);
                if (this.Controls.ContainsKey("r6")) CirclePic((PictureBox)this.Controls["r6"]);
                if (this.Controls.ContainsKey("r7")) CirclePic((PictureBox)this.Controls["r7"]);
                if (this.Controls.ContainsKey("r8")) CirclePic((PictureBox)this.Controls["r8"]);
                if (this.Controls.ContainsKey("r9")) CirclePic((PictureBox)this.Controls["r9"]);
                if (this.Controls.ContainsKey("r10")) CirclePic((PictureBox)this.Controls["r10"]);
            }
            catch { /* no-op */ }

            // Eventlarni form designer orqali bog'lagan bo'lsangiz yana qo'shmang.
            // btnSearchPerson.Click += btnSearchPerson_Click; va hokazo agar kerak bo'lsa.
        }

        // PictureBox ni doira holatga keltirish
        public void CirclePic(PictureBox pb)
        {
            if (pb == null) return;
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, pb.Width - 1, pb.Height - 1);
            pb.Region = new Region(gp);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        // ---------- DB: Yaratish / Tekshirish ----------
        private void CreateDatabase(string dbFile)
        {
            // Bu kod LocalDB da yangi MDF yaratadi. Bu uchun foydalanuvchida LocalDB o'rnatilgan bo'lishi kerak.
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;";
            string dbName = Path.GetFileNameWithoutExtension(dbFile);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Agar fayl allaqachon mavjud bo'lsa, CREATE DATABASE xato beradi; shuning uchun avval tekshirish mumkin.
                if (!File.Exists(dbFile))
                {
                    string createQuery = $"CREATE DATABASE [{dbName}] ON (NAME = N'{dbName}', FILENAME = '{dbFile}')";
                    using (SqlCommand cmd = new SqlCommand(createQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void EnsureTables()
        {
            if (string.IsNullOrEmpty(connectionString)) throw new Exception("Connection string mavjud emas.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // PersonInfo jadvali
                string q1 = @"
IF OBJECT_ID('dbo.PersonInfo', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PersonInfo
    (
        Id CHAR(9) PRIMARY KEY,
        Familiya NVARCHAR(100) NULL,
        Ism NVARCHAR(100) NULL,
        Sharif NVARCHAR(100) NULL,
        Unvon NVARCHAR(100) NULL
    );
END
";
                using (SqlCommand c = new SqlCommand(q1, conn)) c.ExecuteNonQuery();

                // PersonImages jadvali — bir nechta rasm uchun
                string q2 = @"
IF OBJECT_ID('dbo.PersonImages', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PersonImages
    (
        ImgId INT IDENTITY PRIMARY KEY,
        PersonId CHAR(9) NOT NULL FOREIGN KEY REFERENCES dbo.PersonInfo(Id),
        Img VARBINARY(MAX) NOT NULL
    );
END
";
                using (SqlCommand c2 = new SqlCommand(q2, conn)) c2.ExecuteNonQuery();
            }
        }

        private void SaveConnectionString(string cs) => File.WriteAllText("dbpath.txt", cs);
        private string LoadConnectionString() => File.Exists("dbpath.txt") ? File.ReadAllText("dbpath.txt") : "";

        // ---------- KAMERA & QIDIRUV ----------
        // btnSearchPerson tugmasini formdan bog'lang
        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            try
            {
                // Avvalgi grabber mavjud bo'lsa tozalash
                StopCamera();

                // 0 — default kamera
                grabber = new VideoCapture(0, VideoCapture.API.DShow);
                grabber.ImageGrabbed += FrameGrabber;
                grabber.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kamera xatosi: " + ex.Message);
            }
        }

        // Kameradan kelayotgan frame bilan ishlovchi funksiya
        private void FrameGrabber(object sender, EventArgs e)
        {
            try
            {
                if (grabber == null || !grabber.IsOpened) return;

                currentFrame = new Mat();
                grabber.Retrieve(currentFrame);

                if (currentFrame == null || currentFrame.IsEmpty) return;

                // Mat -> Image<Bgr,byte> kabi ishlash uchun ToImage
                using (var img = currentFrame.ToImage<Bgr, byte>())
                {
                    if (img == null) return;

                    // Gray formatga o'tkazish
                    using (var gray = img.Convert<Gray, byte>())
                    {
                        Rectangle[] faces = face.DetectMultiScale(
                            gray,
                            1.2,
                            8,
                            new Size(80, 80),
                            Size.Empty);

                        if (faces.Length > 0)
                        {
                            // Faqat birinchi yuz
                            Rectangle f = faces[0];

                            // Yuz ROI ni olish va 100x100 ga qayta o'lchash
                            using (var faceMatColor = new Mat(img.Mat, f))
                            using (var faceGray = new Mat())
                            {
                                CvInvoke.CvtColor(faceMatColor, faceGray, ColorConversion.Bgr2Gray);

                                Mat faceResized = new Mat();
                                CvInvoke.Resize(faceGray, faceResized, new Size(100, 100), 0, 0, Inter.Cubic);

                                // Bitmap qilish uchun Imencode
                                byte[] encoded = CvInvoke.Imencode(".png", faceResized);

                                // GUI threadda yangilaymiz
                                this.BeginInvoke(new Action(() =>
                                {
                                    picFace1.Image = ByteArrayToImage(encoded);
                                }));

                                // Endi — DB da yuz bo'yicha qidirish: agar txtid bo'sh bo'lsa
                                string foundId = null;
                                if (string.IsNullOrWhiteSpace(txtid.Text))
                                {
                                    // template matching orqali bazadagi rasmlar bilan o'xshashlik tekshirish
                                    foundId = FindPersonByFace(faceResized);
                                }
                                else
                                {
                                    // Agar txtid to'ldirilgan bo'lsa, shunchaki shu id bo'yicha olingan ma'lumotni yuklaymiz
                                    foundId = txtid.Text;
                                }

                                if (!string.IsNullOrEmpty(foundId))
                                {
                                    // DB dan PersonInfo yuklash va UIni to'ldirish
                                    LoadPersonInfoToUI(foundId);
                                }

                                // Agar faqat bir marta aniqlansin desangiz stop qiling:
                                // stop to prevent continuous event — agar monitoring kerak bo'lsa izohga oling
                                StopCamera();
                            }
                        }
                        else
                        {
                            // yuz topilmadi: GUIda faqat hozirgi frame ni ko'rsatsak ham bo'ladi
                            // (agar kerak bo'lsa video oynada ko'rsatish qo'shishingiz mumkin)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Kamera/processing paytida xatolik — loglash va davom ettirish
                Debug.WriteLine("FrameGrabber xatosi: " + ex.Message);
            }
        }

        // Yuzni bazadagi rasmlar bilan oddiy template-match orqali qidiradi
        // Kichik DBlar uchun ishlaydi. Qaytaradi: topilgan PersonId yoki null
        private string FindPersonByFace(Mat queryFaceGray)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Barcha rasm va PersonIdlarni olamiz (kichik DB uchun maqbul)
                    string q = "SELECT PersonId, Img FROM PersonImages";
                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        double bestScore = 0.0;
                        string bestId = null;

                        while (reader.Read())
                        {
                            string pid = reader["PersonId"].ToString();
                            byte[] imgBytes = (byte[])reader["Img"];

                            // decode to Mat
                            Mat stored = new Mat();
                            CvInvoke.Imdecode(imgBytes, ImreadModes.Grayscale, stored);

                            if (stored == null || stored.IsEmpty) continue;

                            // ensure same size
                            Mat storedResized = new Mat();
                            CvInvoke.Resize(stored, storedResized, queryFaceGray.Size);

                            // Template matching
                            using (Mat result = new Mat())
                            {
                                CvInvoke.MatchTemplate(queryFaceGray, storedResized, result, TemplateMatchingType.CcoeffNormed);
                                double minVal = 0, maxVal = 0;
                                Point minLoc = new Point(), maxLoc = new Point();
                                CvInvoke.MinMaxLoc(result, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

                                if (maxVal > bestScore)
                                {
                                    bestScore = maxVal;
                                    bestId = pid;
                                }
                            }

                            // agar juda yuqori moslik topilsa (tezroq qaytish)
                            if (bestScore >= 0.78) // threshold — tajriba bilan sozlang
                            {
                                return bestId;
                            }
                        }

                        // agar pastroq score bo'lsa ham bestId qaytariladi, yoki null
                        return bestScore >= 0.60 ? bestId : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FindPersonByFace xato: " + ex.Message);
                return null;
            }
        }

        // DB dan PersonInfo ni o'qib UI ga joylash
        private void LoadPersonInfoToUI(string personId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string q = "SELECT TOP 1 * FROM PersonInfo WHERE Id=@id";
                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", personId);
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                this.BeginInvoke(new Action(() =>
                                {
                                    txtid.Text = r["Id"].ToString();
                                    txtfamiliya.Text = r["Familiya"].ToString();
                                    txtism.Text = r["Ism"].ToString();
                                    txtsharif.Text = r["Sharif"].ToString();
                                    txtunvoni.Text = r["Unvon"].ToString();
                                }));

                                // agar PersonImages dan birinchi rasmni olish kerak bo'lsa ko'rsatuvchi:
                                r.Close();
                                string q2 = "SELECT TOP 1 Img FROM PersonImages WHERE PersonId=@id";
                                using (SqlCommand c2 = new SqlCommand(q2, conn))
                                {
                                    c2.Parameters.AddWithValue("@id", personId);
                                    object o = c2.ExecuteScalar();
                                    if (o != null && o != DBNull.Value)
                                    {
                                        byte[] imgBytes = (byte[])o;
                                        var bmp = ByteArrayToImage(imgBytes);
                                        this.BeginInvoke(new Action(() =>
                                        {
                                            picFace1.Image = bmp;
                                        }));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadPersonInfoToUI xato: " + ex.Message);
            }
        }

        // Kamera va eventlarni tozalash
        private void StopCamera()
        {
            try
            {
                if (grabber != null)
                {
                    grabber.ImageGrabbed -= FrameGrabber;
                    if (grabber.IsOpened) grabber.Stop();
                    grabber.Dispose();
                    grabber = null;
                }
            }
            catch { }
        }

        // ---------- QO'SHISH / UPDATE ----------
        // btnAddPerson tugmasiga bog'lang
        private async void btnAddPerson_Click(object sender, EventArgs e)
        {
            // ID tekshirish: 9 xonali raqam
            if (string.IsNullOrWhiteSpace(txtid.Text) || !Regex.IsMatch(txtid.Text.Trim(), @"^\d{9}$"))
            {
                MessageBox.Show("ID 9 xonali raqam bo'lishi kerak (masalan: 000000001).", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (grabber == null || !grabber.IsOpened)
            {
                MessageBox.Show("Avval kamera ishga tushirilishi lozim (Qidirish tugmasi bilan).", "Eslatma", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 10 ta rasmni 100ms intervallar bilan yig'amiz
            List<byte[]> gathered = new List<byte[]>();
            for (int i = 0; i < 10; i++)
            {
                Mat frame = new Mat();
                grabber.Retrieve(frame);
                if (frame == null || frame.IsEmpty)
                {
                    await Task.Delay(100);
                    continue;
                }

                Mat gray = new Mat();
                CvInvoke.CvtColor(frame, gray, ColorConversion.Bgr2Gray);

                Rectangle[] rects = face.DetectMultiScale(gray, 1.2, 8, new Size(80, 80), Size.Empty);
                if (rects.Length > 0)
                {
                    using (Mat faceMat = new Mat(frame, rects[0]))
                    using (Mat faceGray = new Mat())
                    {
                        CvInvoke.CvtColor(faceMat, faceGray, ColorConversion.Bgr2Gray);
                        Mat faceResized = new Mat();
                        CvInvoke.Resize(faceGray, faceResized, new Size(100, 100), 0, 0, Inter.Cubic);
                        byte[] png = CvInvoke.Imencode(".png", faceResized);
                        gathered.Add(png);

                        // ko'rsatish
                        this.BeginInvoke(new Action(() =>
                        {
                            picFace1.Image = ByteArrayToImage(png);
                        }));
                    }
                }

                await Task.Delay(100);
            }

            if (gathered.Count == 0)
            {
                MessageBox.Show("Yuz aniqlanmadi — rasm olinmadi.", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // DB ga insert/update va PersonImages ga rasmlarni saqlash
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // PersonInfo mavjudligini tekshirish
                    string check = "SELECT COUNT(*) FROM PersonInfo WHERE Id=@id";
                    using (SqlCommand c = new SqlCommand(check, conn))
                    {
                        c.Parameters.AddWithValue("@id", txtid.Text.Trim());
                        int exists = (int)c.ExecuteScalar();

                        if (exists == 0)
                        {
                            // insert
                            string ins = "INSERT INTO PersonInfo(Id, Familiya, Ism, Sharif, Unvon) VALUES(@id, @f, @i, @s, @u)";
                            using (SqlCommand ci = new SqlCommand(ins, conn))
                            {
                                ci.Parameters.AddWithValue("@id", txtid.Text.Trim());
                                ci.Parameters.AddWithValue("@f", (object)txtfamiliya.Text ?? DBNull.Value);
                                ci.Parameters.AddWithValue("@i", (object)txtism.Text ?? DBNull.Value);
                                ci.Parameters.AddWithValue("@s", (object)txtsharif.Text ?? DBNull.Value);
                                ci.Parameters.AddWithValue("@u", (object)txtunvoni.Text ?? DBNull.Value);
                                ci.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // update
                            string upd = "UPDATE PersonInfo SET Familiya=@f, Ism=@i, Sharif=@s, Unvon=@u WHERE Id=@id";
                            using (SqlCommand cu = new SqlCommand(upd, conn))
                            {
                                cu.Parameters.AddWithValue("@id", txtid.Text.Trim());
                                cu.Parameters.AddWithValue("@f", (object)txtfamiliya.Text ?? DBNull.Value);
                                cu.Parameters.AddWithValue("@i", (object)txtism.Text ?? DBNull.Value);
                                cu.Parameters.AddWithValue("@s", (object)txtsharif.Text ?? DBNull.Value);
                                cu.Parameters.AddWithValue("@u", (object)txtunvoni.Text ?? DBNull.Value);
                                cu.ExecuteNonQuery();
                            }

                            // eski rasmlarni o'chirish (agar yangilarini tozalash kerak bo'lsa)
                            string del = "DELETE FROM PersonImages WHERE PersonId=@id";
                            using (SqlCommand cd = new SqlCommand(del, conn))
                            {
                                cd.Parameters.AddWithValue("@id", txtid.Text.Trim());
                                cd.ExecuteNonQuery();
                            }
                        }

                        // End: insert/update PersonInfo
                        // Endi barchasini PersonImages ga joylashtiramiz
                        string insImg = "INSERT INTO PersonImages(PersonId, Img) VALUES(@id, @img)";
                        foreach (var b in gathered)
                        {
                            using (SqlCommand ci2 = new SqlCommand(insImg, conn))
                            {
                                ci2.Parameters.AddWithValue("@id", txtid.Text.Trim());
                                ci2.Parameters.AddWithValue("@img", b);
                                ci2.ExecuteNonQuery();
                            }
                        }
                    }
                }

                MessageBox.Show("Ma'lumotlar va rasm(lar) muvaffaqiyatli saqlandi.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Saqlash xatosi: " + ex.Message, "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- O'CHIRISH (picture double click) ----------
        private void picFace1_DoubleClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtid.Text)) return;

            var res = MessageBox.Show("Foydalanuvchini o'chirishni xohlaysizmi?", "Tasdiq", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string delImgs = "DELETE FROM PersonImages WHERE PersonId=@id";
                    using (SqlCommand c = new SqlCommand(delImgs, conn))
                    {
                        c.Parameters.AddWithValue("@id", txtid.Text.Trim());
                        c.ExecuteNonQuery();
                    }

                    string delPerson = "DELETE FROM PersonInfo WHERE Id=@id";
                    using (SqlCommand c2 = new SqlCommand(delPerson, conn))
                    {
                        c2.Parameters.AddWithValue("@id", txtid.Text.Trim());
                        c2.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Foydalanuvchi o'chirildi.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // UI tozalash
                txtid.Clear();
                txtfamiliya.Clear();
                txtism.Clear();
                txtsharif.Clear();
                txtunvoni.Clear();
                picFace1.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("O'chirishda xato: " + ex.Message);
            }
        }

        // ---------- Yordamchi funksiyalar ----------
        // Bitmap -> byte[]
        private byte[] BitmapToByteArray(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        // Image bytes -> System.Drawing.Image
        private Image ByteArrayToImage(byte[] arr)
        {
            using (MemoryStream ms = new MemoryStream(arr))
            {
                return Image.FromStream(ms);
            }
        }

        // Form yopilganda resurslarni tozalash
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            StopCamera();
        }
    }
}
