using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fakepack;
using ExamSysWinform._01考试管理;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PersonalizedExercise
{
    public partial class ExamQueryForm : Form
    {
        string subject;
        WebBrowser wb;
        public ExamQueryForm(WebBrowser wb, string subject)
        {
            this.subject = subject;
            this.wb = wb;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!FakePack.Login(subject, textBox1.Text, textBox2.Text))
                return;
            string[] str = FakePack.ExamTemplate(subject, textBox1.Text);
            foreach (string s in str)
                if (s != null && s != "")
                    comboBox1.Items.Add(s);
            if(comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] str = FakePack.ExamTemplate(subject, textBox1.Text, comboBox1.SelectedIndex);
            string[] str2 = FakePack.ExamTemplate(str);
            string str_l = "";
            foreach (string s in str2)
                if (s != null && s != "")
                    str_l += s + ',';

            //从*.zip到*
            str_l = str_l.Replace(".zip", "");

            try
            {
                onlieExercise oe = new onlieExercise(this.wb, false, subject);
                oe.exerciseChoice = str_l;
                oe.worker.WorkerReportsProgress = true;
                oe.worker.RunWorkerAsync();
                oe.Close();
            }
            catch (Exception) { }

            this.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] str = FakePack.ExamTemplate(subject, textBox1.Text, comboBox1.SelectedIndex);
            string[] str2 = FakePack.ExamTemplate(str);
            string str_l = "";
            int cnt = 0;
            foreach (string s in str2)
                if (s != null && s != "")
                    str_l += s + ',';

            //从*.zip到*
            str_l = str_l.Replace(".zip", "");

            string gu = Guid.NewGuid().ToString();
            string tempdir = Environment.GetEnvironmentVariable("TEMP");
            Directory.CreateDirectory(tempdir + "\\2645\\AMCalTor\\ans\\" + gu);

            try
            {
                onlieExercise oe = new onlieExercise(this.wb, false, subject);
                oe.exerciseChoice = str_l;
                oe.worker_DoWork(sender, new DoWorkEventArgs(e));
                Thread th = new Thread(delegate ()
                {
                    CreateAnswerSheet.cas(gu, str_l);
                    Process.Start(tempdir + "\\2645\\AMCalTor\\ans\\" + gu + "\\Ans.doc");
                    oe.worker_RunWorkerCompleted(sender, new RunWorkerCompletedEventArgs(e, new Exception(), false));
                });
                th.Start();
            }
            catch (Exception) { }

            this.Close();
        }
    }
}
