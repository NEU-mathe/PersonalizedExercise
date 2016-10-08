using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ExamSysWinform._01考试管理;
using ExamSysWinform.WebService;
using ExamSysWinform;
using System.IO;
using System.Net;
using FtpTest;

namespace PersonalizedExercise
{
    public partial class Form1 : Form
    {
        //private WebBrowser webBrowser1;
        public static ClientStudentModel studentModel;
        public static string StuNumber;
        public static string name;
        public Form1()
        {
            InitializeComponent();
        }

        private void LoginMethod()
        {
            try
            {
                //if (this.chkInput())
                {
                    StudentService service = new StudentService();
                    ClientStudentModel selectModel = new ClientStudentModel
                    {
                        Key = "_3[#$%wd*",
                        Version = "2.0.0",
                        StudentNumber = this.textBox1.Text.Trim(),
                        Pwd = this.textBox2.Text.Trim(),
                        DataSource = name
                    };
                    studentModel = service.Login(selectModel);
                    MessageBox.Show( studentModel.ClassName.ToString()+studentModel.StudentName.ToString()+studentModel.DataSource.ToString()+studentModel.TeacherId.ToString()+studentModel.TeacherName.ToString());
                    if (studentModel == null)
                    {
                        MessageBox.Show("用户不存在");
                        base.Close();
                    }
                    else
                    {
                        if (studentModel.IsSameVersion)
                        {
                            MessageBox.Show(studentModel.MessageInfo1);
                        }
                        if (studentModel.EnableLogin)
                        {
                            base.DialogResult = DialogResult.Yes;
                        }
                        else
                        {
                            base.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("服务器无法处理请求"))
                {
                    MessageBox.Show("服务器正在维护");
                    base.Close();
                }
                else
                {
                    MessageBox.Show(exception.Message);
                    base.Close();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            name = comboBox1.Text;
            FileStream aFile = new FileStream("StuNum.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(aFile);
            sw.WriteLine(textBox1.Text);
            sw.Close();
            aFile = new FileStream("Password.txt", FileMode.Create);
            sw = new StreamWriter(aFile);
            sw.WriteLine(textBox2.Text);
            sw.Close();
            StuNumber = textBox1.Text.Trim();
            if (textBox1.Text != "学号" && textBox2.Text != "密码")
            {
                this.button1.Text = "正在登录";
                new Thread(new ThreadStart(this.LoginMethod)) { IsBackground = true }.Start();
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            name = comboBox1.Text;
            FileStream aFile = new FileStream("StuNum.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(aFile);
            sw.WriteLine(textBox1.Text);
            sw.Close();
            aFile = new FileStream("Password.txt", FileMode.Create);
            sw = new StreamWriter(aFile);
            sw.WriteLine(textBox2.Text);
            sw.Close();
            StuNumber = textBox1.Text.Trim();
            if (button2.Text == "激活离线练习")
            {
                if (MessageBox.Show("没有找到题库，是否帮助您下载一份？（需要联网）", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    button2.Text = "正在下载";
                    DownloadFolder.downftp("ftp://202.118.26.80", "ChoiceSource", Directory.GetCurrentDirectory());
                    button2.Text = "离线练习";
                    MessageBox.Show("传输完毕");
                }
                else
                {
                    if (Directory.Exists("ChoiceSource"))
                        button2.Text = "离线练习";
                }
            }
            else
            {
                StuNumber = textBox1.Text;
                base.DialogResult = DialogResult.No;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 2;
            if (File.Exists("StuNum.txt"))
            {
                FileStream aFile = new FileStream("StuNum.txt", FileMode.Open);
                StreamReader sw = new StreamReader(aFile);
                textBox1.Text = sw.ReadToEnd();
                sw.Close();
            }
            if (File.Exists("Password.txt"))
            {
                FileStream aFile = new FileStream("Password.txt", FileMode.Open);
                StreamReader sw = new StreamReader(aFile);
                textBox2.Text = sw.ReadToEnd();
                sw.Close();
            }
            if(Directory.Exists("ChoiceSource"))
            {
                button2.Text = "离线练习";
            }
        }

    }
}
