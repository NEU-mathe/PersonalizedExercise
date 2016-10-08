namespace ExamSysWinform
{
    using ExamSysWinform.Properties;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class waitForm : Form
    {
        private IContainer components;
        public Label label1;
        private Label label2;
        private PictureBox pictureBox1;

        public waitForm(string strWait)
        {
            this.InitializeComponent();
            this.SetText(strWait);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new PictureBox();
            this.label1 = new Label();
            this.label2 = new Label();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.pictureBox1.BackColor = Color.White;
            this.pictureBox1.BackgroundImageLayout = ImageLayout.None;
            //this.pictureBox1.Image = Resources.pictureBox1_Image;
            //this.pictureBox1.InitialImage = Resources.pictureBox1_Image;
            this.pictureBox1.Location = new Point(1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(0x152, 0x3d);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.label1.BackColor = Color.White;
            this.label1.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label1.Location = new Point(1, 0x40);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x152, 0x17);
            this.label1.TabIndex = 1;
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.AutoSize = true;
            this.label2.Font = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.label2.Location = new Point(0x62, 13);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x98, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "等待对话框，调试用";
            this.label2.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0x151, 0x5e);
            base.Controls.Add(this.pictureBox1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            this.Cursor = Cursors.AppStarting;
            base.FormBorderStyle = FormBorderStyle.None;
            base.ImeMode = ImeMode.Off;
            base.Name = "waitForm";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "waitForm";
            ((ISupportInitialize) this.pictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void SetText(string text)
        {
            if (this.label1.InvokeRequired)
            {
                base.Invoke(new SetTextHandler(this.SetText), new object[] { text });
            }
            else if (text == "close")
            {
                base.Close();
            }
            else
            {
                this.label1.Text = text;
            }
        }

        private delegate void SetTextHandler(string text);
    }
}

