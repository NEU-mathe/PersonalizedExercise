using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Test
{
    class choicenode : IComparable<choicenode>
    {
        public int id;
        public int pid;
        public string name;

        public choicenode()
        {
           
        }

        public int CompareTo(choicenode p)
        {
                return this.id - p.id;
        }
        public static List<choicenode> getChoiceList(string iniPath) //param1:ini file path
        {
            try
            {
                iniHelper ih = new iniHelper(iniPath, "Default");
                int count = Convert.ToInt32(ih.GetValue("Index", "count"));
                List<choicenode> choices = new List<choicenode>();
                for (int i = 1; i <= count; ++i)
                {
                    choicenode tmpNode = new choicenode();
                    tmpNode.id = Convert.ToInt32(ih.GetValue("Node" + i.ToString(), "Id"));
                    tmpNode.pid = Convert.ToInt32(ih.GetValue("Node" + i.ToString(), "Pid"));
                    tmpNode.name = ih.GetValue("Node" + i.ToString(), "Name");
                    choices.Add(tmpNode);
                }
                choices.Sort();
                return choices;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return new List<choicenode>();
            }
        }

        }
}

