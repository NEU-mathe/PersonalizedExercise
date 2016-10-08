using System;
using System.Windows.Forms;
using System.Text;

namespace Test
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
            Application.Run(new Form1());

            //iniHelper ih = new iniHelper("test1.ini", "Default");

            ////设置INI文件中的值
            ////ih.SetValue("Set1", "attr1", "1");
            ////ih.SetValue("Set1", "attr2", 2L);
            ////ih.SetValue("Set1", "attr3", 3f);
            ////ih.SetValue("Set2", "attra", new StringBuilder("a"));
            ////ih.SetValue("Set2", "attrb", 'b');

            ////读取INI文件中的值
            //int count;
            //count =Convert.ToInt32(ih.GetValue("Index", "count"));
            //MessageBox.Show(count.ToString());
            ////读取不存在的键值
            ////Console.WriteLine(ih.GetValue("Setx", "attrx"));

            ////Console.WriteLine("Done.");
            ////Console.ReadLine();
        }
    }
}
