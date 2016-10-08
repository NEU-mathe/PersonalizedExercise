using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace Test
{
    class Generateini
    {
        private string _iniPath;
        public Generateini(string iniPath)
        {
            _iniPath = iniPath;
        }

        public void generate()
        {
            DataSet mySet = SqlHelper.getList();
            DataTable myTable = mySet.Tables["Chapter"];
            int count = 0;
            FileStream fs = new FileStream(_iniPath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs,Encoding.GetEncoding("GB2312"));
            foreach (DataRow myRow in myTable.Rows)
            {
                ++count;
                sw.WriteLine("[Node" + count.ToString() + "]");
                sw.WriteLine("Id=" + myRow[0].ToString());
                sw.WriteLine("Pid=" + myRow[1].ToString());
                sw.WriteLine("Name=" + myRow[2]);
            }
            sw.WriteLine("[Index]");
            sw.WriteLine("count=" +count.ToString());
            sw.Close();
            fs.Close();
        }
    }
    
}
