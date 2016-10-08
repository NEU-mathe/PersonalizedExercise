using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if(e.Node.Checked==true)
            {
                foreach (TreeNode node in e.Node.Nodes)
                    node.Checked = true;
            }
            else
            {
                foreach (TreeNode node in e.Node.Nodes)
                    node.Checked = false;
            }
        }

        //private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    e.Node.ExpandAll();
        //}

        private List<choicenode> getChoiceList(string iniPath) //param1:ini file path
        {
            iniHelper ih = new iniHelper(iniPath, "Default");
            int count = Convert.ToInt32(ih.GetValue("Index", "count"));
            List<choicenode> choices = new List<choicenode>();
            for(int i = 1; i<= count;++i)
            {
                choicenode tmpNode = new choicenode();
                tmpNode.id= Convert.ToInt32(ih.GetValue("Node"+i.ToString(), "Id"));
                tmpNode.pid = Convert.ToInt32(ih.GetValue("Node" + i.ToString(), "Pid"));
                tmpNode.name = ih.GetValue("Node" + i.ToString(), "Name");
                choices.Add(tmpNode);
            }
            choices.Sort();
            return choices;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            List<choicenode> choices = getChoiceList("test2645.ini");
            //generate tree

            //add root
            foreach(choicenode tmpNode in choices)
            {
                if (tmpNode.pid == 0)
                {
                    treeView1.Nodes.Add(tmpNode.id.ToString(), tmpNode.name);
                }
            }
            //add child
            foreach (choicenode tmpNode in choices)
            {
                if (tmpNode.pid != 0)
                {
                    TreeNode[] finded = treeView1.Nodes.Find(tmpNode.pid.ToString(), true);
                    foreach(TreeNode findedNode in finded)
                    {
                        findedNode.Nodes.Add(tmpNode.id.ToString(), tmpNode.name);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Generateini get = new Generateini("test2645.ini");
            get.generate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            string str = "";
            foreach(TreeNode tn1 in treeView1.Nodes)
            {
                if (tn1.GetNodeCount(false) == 0&&tn1.Checked)
                    str += tn1.Name + ",";
                foreach(TreeNode tn2 in tn1.Nodes)
                {
                    if (tn2.GetNodeCount(false) == 0&&tn2.Checked)
                        str += tn2.Name + ",";
                    foreach (TreeNode tn3 in tn2.Nodes)
                    {
                        if (tn3.Checked)
                            str += tn3.Name.ToString() + ",";
                    }
                }
            }
            textBox1.Text = str;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] a = textBox1.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> T = a.ToList<string>();
            Dictionary<int, string> dic = new Dictionary<int, string>();
            int count = 0;
            foreach(string s in T)
            {
                foreach (string ss in Directory.GetDirectories(@"ChoiceSource\高等数学_GS\" + s))
                {
                    foreach(string sss in Directory.GetFiles(ss))
                    {
                        dic.Add(++count, sss);
                    }
                }
            }
            int[] myRandoms=new int[25];
            for(int i=0;i<(25<count?25:count);++i)
            {
                int myRandom;
                bool flag=false;
                do
                {
                    myRandom = new Random(Guid.NewGuid().GetHashCode()).Next(1, count+1);
                    Debug.WriteLine(myRandom);
                    myRandoms[i] = myRandom;
                    flag = false;
                    for (int j = 0; j < i; ++j)
                    {
                        if (myRandoms[j] == myRandom)  //1 2 3 4 5 6 7  myran=1
                            flag = true;
                    }
                } while (flag);
            }
            string str = "";
            for(int i=0;i< (25 < count ? 25 : count); ++i)
            {
                Debug.WriteLine(dic[myRandoms[i]]);
                str += dic[myRandoms[i]] + ",";
            }
            textBox1.Text = str;
        }
    }
}
