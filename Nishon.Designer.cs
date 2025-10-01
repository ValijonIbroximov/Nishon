namespace FaceDBApp
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnSearchPerson = new System.Windows.Forms.Button();
            this.txtid = new System.Windows.Forms.TextBox();
            this.r10 = new System.Windows.Forms.PictureBox();
            this.g10 = new System.Windows.Forms.PictureBox();
            this.r9 = new System.Windows.Forms.PictureBox();
            this.g9 = new System.Windows.Forms.PictureBox();
            this.r8 = new System.Windows.Forms.PictureBox();
            this.g8 = new System.Windows.Forms.PictureBox();
            this.r7 = new System.Windows.Forms.PictureBox();
            this.g7 = new System.Windows.Forms.PictureBox();
            this.txtunvoni = new System.Windows.Forms.TextBox();
            this.txtsharif = new System.Windows.Forms.TextBox();
            this.txtism = new System.Windows.Forms.TextBox();
            this.txtfamiliya = new System.Windows.Forms.TextBox();
            this.picFace = new Emgu.CV.UI.ImageBox();
            this.r6 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.g5 = new System.Windows.Forms.PictureBox();
            this.btnAddPerson = new System.Windows.Forms.Button();
            this.g6 = new System.Windows.Forms.PictureBox();
            this.imageBoxFrameGrabber = new Emgu.CV.UI.ImageBox();
            this.target = new System.Windows.Forms.Panel();
            this.r5 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.r10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.r9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.r8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.r7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.r6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.g6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).BeginInit();
            this.target.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.r5)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearchPerson
            // 
            this.btnSearchPerson.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchPerson.BackgroundImage = global::MultiFaceRec.Properties.Resources.personSearch;
            this.btnSearchPerson.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearchPerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchPerson.Location = new System.Drawing.Point(988, 123);
            this.btnSearchPerson.Name = "btnSearchPerson";
            this.btnSearchPerson.Size = new System.Drawing.Size(60, 60);
            this.btnSearchPerson.TabIndex = 24;
            this.btnSearchPerson.UseVisualStyleBackColor = false;
            this.btnSearchPerson.Click += new System.EventHandler(this.btnSearchPerson_Click);
            // 
            // txtid
            // 
            this.txtid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtid.Font = new System.Drawing.Font("Agency FB", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtid.ForeColor = System.Drawing.Color.White;
            this.txtid.Location = new System.Drawing.Point(871, 445);
            this.txtid.Name = "txtid";
            this.txtid.Size = new System.Drawing.Size(156, 28);
            this.txtid.TabIndex = 22;
            // 
            // r10
            // 
            this.r10.BackColor = System.Drawing.Color.Transparent;
            this.r10.Image = global::MultiFaceRec.Properties.Resources.r10;
            this.r10.Location = new System.Drawing.Point(160, 160);
            this.r10.Name = "r10";
            this.r10.Padding = new System.Windows.Forms.Padding(3);
            this.r10.Size = new System.Drawing.Size(80, 80);
            this.r10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.r10.TabIndex = 5;
            this.r10.TabStop = false;
            this.r10.Visible = false;
            // 
            // g10
            // 
            this.g10.BackColor = System.Drawing.Color.Transparent;
            this.g10.Image = global::MultiFaceRec.Properties.Resources.g10;
            this.g10.Location = new System.Drawing.Point(160, 160);
            this.g10.Name = "g10";
            this.g10.Padding = new System.Windows.Forms.Padding(3);
            this.g10.Size = new System.Drawing.Size(80, 80);
            this.g10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.g10.TabIndex = 5;
            this.g10.TabStop = false;
            // 
            // r9
            // 
            this.r9.BackColor = System.Drawing.Color.Transparent;
            this.r9.Image = global::MultiFaceRec.Properties.Resources.r9;
            this.r9.Location = new System.Drawing.Point(120, 120);
            this.r9.Name = "r9";
            this.r9.Padding = new System.Windows.Forms.Padding(3);
            this.r9.Size = new System.Drawing.Size(160, 160);
            this.r9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.r9.TabIndex = 4;
            this.r9.TabStop = false;
            this.r9.Visible = false;
            // 
            // g9
            // 
            this.g9.BackColor = System.Drawing.Color.Transparent;
            this.g9.Image = global::MultiFaceRec.Properties.Resources.g9;
            this.g9.Location = new System.Drawing.Point(120, 120);
            this.g9.Name = "g9";
            this.g9.Padding = new System.Windows.Forms.Padding(3);
            this.g9.Size = new System.Drawing.Size(160, 160);
            this.g9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.g9.TabIndex = 4;
            this.g9.TabStop = false;
            // 
            // r8
            // 
            this.r8.BackColor = System.Drawing.Color.Transparent;
            this.r8.Image = global::MultiFaceRec.Properties.Resources.r8;
            this.r8.Location = new System.Drawing.Point(80, 80);
            this.r8.Name = "r8";
            this.r8.Padding = new System.Windows.Forms.Padding(3);
            this.r8.Size = new System.Drawing.Size(240, 240);
            this.r8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.r8.TabIndex = 3;
            this.r8.TabStop = false;
            this.r8.Visible = false;
            // 
            // g8
            // 
            this.g8.BackColor = System.Drawing.Color.Transparent;
            this.g8.Image = global::MultiFaceRec.Properties.Resources.g8;
            this.g8.Location = new System.Drawing.Point(80, 80);
            this.g8.Name = "g8";
            this.g8.Padding = new System.Windows.Forms.Padding(3);
            this.g8.Size = new System.Drawing.Size(240, 240);
            this.g8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.g8.TabIndex = 3;
            this.g8.TabStop = false;
            // 
            // r7
            // 
            this.r7.BackColor = System.Drawing.Color.Transparent;
            this.r7.Image = global::MultiFaceRec.Properties.Resources.r7;
            this.r7.Location = new System.Drawing.Point(40, 40);
            this.r7.Name = "r7";
            this.r7.Padding = new System.Windows.Forms.Padding(3);
            this.r7.Size = new System.Drawing.Size(320, 320);
            this.r7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.r7.TabIndex = 2;
            this.r7.TabStop = false;
            this.r7.Visible = false;
            // 
            // g7
            // 
            this.g7.BackColor = System.Drawing.Color.Transparent;
            this.g7.Image = global::MultiFaceRec.Properties.Resources.g7;
            this.g7.Location = new System.Drawing.Point(40, 40);
            this.g7.Name = "g7";
            this.g7.Padding = new System.Windows.Forms.Padding(3);
            this.g7.Size = new System.Drawing.Size(320, 320);
            this.g7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.g7.TabIndex = 2;
            this.g7.TabStop = false;
            // 
            // txtunvoni
            // 
            this.txtunvoni.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtunvoni.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtunvoni.Font = new System.Drawing.Font("High Tower Text", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtunvoni.ForeColor = System.Drawing.Color.White;
            this.txtunvoni.Location = new System.Drawing.Point(858, 391);
            this.txtunvoni.Name = "txtunvoni";
            this.txtunvoni.Size = new System.Drawing.Size(181, 27);
            this.txtunvoni.TabIndex = 21;
            // 
            // txtsharif
            // 
            this.txtsharif.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtsharif.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtsharif.Font = new System.Drawing.Font("High Tower Text", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsharif.ForeColor = System.Drawing.Color.White;
            this.txtsharif.Location = new System.Drawing.Point(858, 337);
            this.txtsharif.Name = "txtsharif";
            this.txtsharif.Size = new System.Drawing.Size(181, 27);
            this.txtsharif.TabIndex = 20;
            // 
            // txtism
            // 
            this.txtism.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtism.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtism.Font = new System.Drawing.Font("High Tower Text", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtism.ForeColor = System.Drawing.Color.White;
            this.txtism.Location = new System.Drawing.Point(858, 284);
            this.txtism.Name = "txtism";
            this.txtism.Size = new System.Drawing.Size(181, 27);
            this.txtism.TabIndex = 19;
            // 
            // txtfamiliya
            // 
            this.txtfamiliya.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtfamiliya.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtfamiliya.Font = new System.Drawing.Font("High Tower Text", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtfamiliya.ForeColor = System.Drawing.Color.White;
            this.txtfamiliya.Location = new System.Drawing.Point(858, 231);
            this.txtfamiliya.Name = "txtfamiliya";
            this.txtfamiliya.Size = new System.Drawing.Size(181, 27);
            this.txtfamiliya.TabIndex = 18;
            // 
            // picFace
            // 
            this.picFace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picFace.Location = new System.Drawing.Point(861, 70);
            this.picFace.Name = "picFace";
            this.picFace.Size = new System.Drawing.Size(82, 105);
            this.picFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picFace.TabIndex = 17;
            this.picFace.TabStop = false;
            // 
            // r6
            // 
            this.r6.BackColor = System.Drawing.Color.Transparent;
            this.r6.Image = global::MultiFaceRec.Properties.Resources.r6;
            this.r6.Location = new System.Drawing.Point(0, 0);
            this.r6.Name = "r6";
            this.r6.Padding = new System.Windows.Forms.Padding(3);
            this.r6.Size = new System.Drawing.Size(400, 400);
            this.r6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.r6.TabIndex = 1;
            this.r6.TabStop = false;
            this.r6.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::MultiFaceRec.Properties.Resources.Info_user2;
            this.pictureBox1.Location = new System.Drawing.Point(823, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(253, 479);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // g5
            // 
            this.g5.BackColor = System.Drawing.Color.Transparent;
            this.g5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g5.Image = global::MultiFaceRec.Properties.Resources.g5;
            this.g5.Location = new System.Drawing.Point(0, 0);
            this.g5.Name = "g5";
            this.g5.Size = new System.Drawing.Size(400, 400);
            this.g5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.g5.TabIndex = 0;
            this.g5.TabStop = false;
            // 
            // btnAddPerson
            // 
            this.btnAddPerson.BackColor = System.Drawing.Color.Transparent;
            this.btnAddPerson.BackgroundImage = global::MultiFaceRec.Properties.Resources.personAdd;
            this.btnAddPerson.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddPerson.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPerson.Location = new System.Drawing.Point(988, 57);
            this.btnAddPerson.Name = "btnAddPerson";
            this.btnAddPerson.Size = new System.Drawing.Size(60, 60);
            this.btnAddPerson.TabIndex = 23;
            this.btnAddPerson.UseVisualStyleBackColor = false;
            this.btnAddPerson.Click += new System.EventHandler(this.btnAddPerson_Click);
            // 
            // g6
            // 
            this.g6.BackColor = System.Drawing.Color.Transparent;
            this.g6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.g6.Image = global::MultiFaceRec.Properties.Resources.g6_;
            this.g6.Location = new System.Drawing.Point(0, 0);
            this.g6.Name = "g6";
            this.g6.Padding = new System.Windows.Forms.Padding(3);
            this.g6.Size = new System.Drawing.Size(400, 400);
            this.g6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.g6.TabIndex = 1;
            this.g6.TabStop = false;
            // 
            // imageBoxFrameGrabber
            // 
            this.imageBoxFrameGrabber.BackColor = System.Drawing.Color.White;
            this.imageBoxFrameGrabber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBoxFrameGrabber.Location = new System.Drawing.Point(861, 70);
            this.imageBoxFrameGrabber.Name = "imageBoxFrameGrabber";
            this.imageBoxFrameGrabber.Size = new System.Drawing.Size(82, 105);
            this.imageBoxFrameGrabber.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.imageBoxFrameGrabber.TabIndex = 16;
            this.imageBoxFrameGrabber.TabStop = false;
            // 
            // target
            // 
            this.target.BackColor = System.Drawing.Color.Transparent;
            this.target.Controls.Add(this.r10);
            this.target.Controls.Add(this.g10);
            this.target.Controls.Add(this.r9);
            this.target.Controls.Add(this.g9);
            this.target.Controls.Add(this.r8);
            this.target.Controls.Add(this.g8);
            this.target.Controls.Add(this.r7);
            this.target.Controls.Add(this.g7);
            this.target.Controls.Add(this.r6);
            this.target.Controls.Add(this.g6);
            this.target.Controls.Add(this.r5);
            this.target.Controls.Add(this.g5);
            this.target.Location = new System.Drawing.Point(87, 26);
            this.target.Name = "target";
            this.target.Size = new System.Drawing.Size(400, 400);
            this.target.TabIndex = 14;
            // 
            // r5
            // 
            this.r5.BackColor = System.Drawing.Color.Transparent;
            this.r5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.r5.Image = global::MultiFaceRec.Properties.Resources.r5;
            this.r5.Location = new System.Drawing.Point(0, 0);
            this.r5.Name = "r5";
            this.r5.Size = new System.Drawing.Size(400, 400);
            this.r5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.r5.TabIndex = 0;
            this.r5.TabStop = false;
            this.r5.Visible = false;
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MultiFaceRec.Properties.Resources.Background_nishon1;
            this.ClientSize = new System.Drawing.Size(1089, 530);
            this.Controls.Add(this.btnSearchPerson);
            this.Controls.Add(this.btnAddPerson);
            this.Controls.Add(this.txtid);
            this.Controls.Add(this.txtunvoni);
            this.Controls.Add(this.txtsharif);
            this.Controls.Add(this.txtism);
            this.Controls.Add(this.txtfamiliya);
            this.Controls.Add(this.picFace);
            this.Controls.Add(this.imageBoxFrameGrabber);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.target);
            this.Name = "FrmPrincipal";
            this.Text = "Camera Input & Face Detection";
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.r10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.r9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.r8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.r7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.r6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.g6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).EndInit();
            this.target.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.r5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSearchPerson;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.PictureBox r10;
        private System.Windows.Forms.PictureBox g10;
        private System.Windows.Forms.PictureBox r9;
        private System.Windows.Forms.PictureBox g9;
        private System.Windows.Forms.PictureBox r8;
        private System.Windows.Forms.PictureBox g8;
        private System.Windows.Forms.PictureBox r7;
        private System.Windows.Forms.PictureBox g7;
        private System.Windows.Forms.TextBox txtunvoni;
        private System.Windows.Forms.TextBox txtsharif;
        private System.Windows.Forms.TextBox txtism;
        private System.Windows.Forms.TextBox txtfamiliya;
        private Emgu.CV.UI.ImageBox picFace;
        private System.Windows.Forms.PictureBox r6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox g5;
        private System.Windows.Forms.Button btnAddPerson;
        private System.Windows.Forms.PictureBox g6;
        private Emgu.CV.UI.ImageBox imageBoxFrameGrabber;
        private System.Windows.Forms.Panel target;
        private System.Windows.Forms.PictureBox r5;
    }
}

