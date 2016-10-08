using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExamSysWinform;

namespace PersonalizedExercise
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //try
            {
                //Form1 win = new Form1();
                //DialogResult a = win.ShowDialog();
                ////if (a == DialogResult.Yes)
                //{
                //    Application.Run(new StuMain(Form1.name, Form1.StuNumber, false));
                //}
                //else if(a == DialogResult.No)
                //{
                Application.Run(new StuMain());
                //}
            }
            //catch (Exception exception)
            //{
            //    MessageBox.Show(exception.Message);
            //}

        }
    }
}
