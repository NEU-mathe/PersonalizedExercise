namespace ExamSysWinform._01考试管理
{
    using ExamSysWinform;
    using ExamSysWinform.Utils;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Windows.Forms;
    using PersonalizedExercise;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using Test;
    using FtpTest;
    using System.Threading;

    [ComVisible(true)]
    public class onlieExercise : Form
    {
        private IContainer components;
        private string exerciseChoice;
        private WebBrowser wb;
        private BackgroundWorker worker = new BackgroundWorker();
        private TreeView treeView1;
        private TextBox textBox1;
        private Button button1;
        private bool ixOffline;
        private string subject;

        public onlieExercise(WebBrowser wb, bool ixOffline, string subject)
        {
            this.InitializeComponent();
            this.treeView1.Size = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width * 4 / 9, Screen.PrimaryScreen.WorkingArea.Height * 4 / 5);
            this.textBox1.Location = new System.Drawing.Point(Screen.PrimaryScreen.WorkingArea.Width * 4 / 9 + 64, 32);
            this.textBox1.Size = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width * 4 / 9, Screen.PrimaryScreen.WorkingArea.Height * 3 / 5);
            this.button1.Location = new System.Drawing.Point(Screen.PrimaryScreen.WorkingArea.Width * 4 / 9 + 64, Screen.PrimaryScreen.WorkingArea.Height * 3 / 5+64);
            this.wb = wb;
            this.subject = subject;
            this.ixOffline = ixOffline;
            try
            {
                if (!ixOffline)
                {
                    Generateini get = new Generateini(subject + ".ini");
                    get.generate(subject);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("与服务器通信失败！请检查网络或使用离线版。");
                base.Close();
                return;
            }
            treeView1.Nodes.Clear();
            List<choicenode> choices = choicenode.getChoiceList(subject+".ini");
            //generate tree

            //add root
            foreach (choicenode tmpNode in choices)
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
                    foreach (TreeNode findedNode in finded)
                    {
                        findedNode.Nodes.Add(tmpNode.id.ToString(), tmpNode.name);
                    }
                }
            }
            this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
            this.worker.ProgressChanged += new ProgressChangedEventHandler(this.worker_ProgressChanged);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);

        }
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void downloadChoice(string templ, bool isOffline)
        {
            try
            {
                if (isOffline)
                {
                    string[] strArray = templ.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] strArray2 = null;
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        strArray2 = strArray[i].Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        if (!System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory() + @"\Download"))
                        {
                            // 目录不存在，建立目录
                            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + @"\Download");
                        }
                        System.IO.File.Copy("ChoiceSource\\" + subject + "\\" + strArray2[0] + "\\" + strArray2[1] + "\\" + strArray[i] + ".zip" , "Download\\" + strArray[i] + ".zip", true);
                        WaitFormService.SetWaitFormCaption(string.Concat(new object[] { "共", strArray.Length, "道选择题，正在下载第", i + 1, "个选择题" }));
                        Process myProcess = new Process();
                        ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("WinRAR.exe", "e -y "
                            + "Download\\" + strArray[i] + ".zip "
                            + "Download\\");
                        myProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        myProcess.StartInfo = myProcessStartInfo;
                        myProcess.Start();
                        while (!myProcess.HasExited)
                        {
                            myProcess.WaitForExit();
                        }

                    }
                }
                else
                {
                    string[] strArray = templ.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] strArray2 = null;
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        strArray2 = strArray[i].Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        if (!System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory() + @"\Download"))
                        {
                            // 目录不存在，建立目录
                            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + @"\Download");
                        }
                        FileStream stream = new FileStream("./" + "Download" + "/" + strArray[i] + ".zip", FileMode.Create);
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://202.118.26.80/ChoiceSource/" + subject + "/" + strArray2[0] + "/" + strArray2[1] + "/" + strArray[i] + ".zip"));
                        request.Method = "RETR";
                        request.UseBinary = true;
                        request.Credentials = new NetworkCredential("LoginName", "Q191KPgC");
                        request.KeepAlive = false;
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        Stream responseStream = response.GetResponseStream();
                        long contentLength = response.ContentLength;
                        int count = 0x800;
                        byte[] buffer = new byte[count];
                        for (int si = responseStream.Read(buffer, 0, count); si > 0; si = responseStream.Read(buffer, 0, count))
                        {
                            stream.Write(buffer, 0, si);
                        }
                        stream.Close();
                        responseStream.Close();
                        response.Close();
                        WaitFormService.SetWaitFormCaption(string.Concat(new object[] { "共", strArray.Length, "道选择题，正在下载第", i + 1, "个选择题" }));
                        Process myProcess = new Process();
                        ProcessStartInfo myProcessStartInfo = new ProcessStartInfo("WinRAR.exe", "e -y "
                            + "Download\\" + strArray[i] + ".zip "
                            + "Download\\");
                        myProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        myProcess.StartInfo = myProcessStartInfo;
                        myProcess.Start();
                        while (!myProcess.HasExited)
                        {
                            myProcess.WaitForExit();
                        }
                    }
                }
                WaitFormService.CloseWaitForm();
                this.Cursor = Cursors.Default;
            }
            catch (Exception)
            {
                MessageBox.Show("下载选择题出错了");
            }
        }

        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(32, 32);
            this.treeView1.Margin = new System.Windows.Forms.Padding(2);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(121, 97);
            this.treeView1.TabIndex = 2;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(188, 32);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(79, 70);
            this.textBox1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(218, 133);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // onlieExercise
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(358, 261);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.treeView1);
            this.Name = "onlieExercise";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "抽取练习";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void initShiJuan(List<string> choice)
        {
            string str = "<!DOCTYPE html PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' 'http://www.w3.org/TR/html4/loose.dtd'>\r\n        <html>\r\n         <head> \r\n          <title> New Document </title> \r\n          <meta name='Generator' content='EditPlus' /> \r\n          <meta name='Author' content='' /> \r\n          <meta name='Keywords' content='' /> \r\n          <meta name='Description' content='' /> \r\n          <style>\r\n        .box {\r\n         border-top-width: 1px;\r\n         border-right-width: 0px;\r\n         border-bottom-width: 0px;\r\n         border-left-width: 1px;\r\n         border-top-style: solid;\r\n         border-right-style: solid;\r\n         border-bottom-style: solid;\r\n         border-left-style: solid;\r\n         border-top-color: #000000;\r\n         border-right-color: #000000;\r\n         border-bottom-color: #000000;\r\n         border-left-color: #000000;\r\n        }\r\n        .box td {\r\n         border-top-width: 0px;\r\n         border-right-width: 1px;\r\n         border-bottom-width: 1px;\r\n         border-left-width: 0px;\r\n         border-top-style: solid;\r\n         border-right-style: solid;\r\n         border-bottom-style: solid;\r\n         border-left-style: solid;\r\n         border-top-color: #000000;\r\n         border-right-color: #000000;\r\n         border-bottom-color: #000000;\r\n         border-left-color: #000000;\r\n         text-align: center;\r\n         font-size: 18px;\r\n         font-weight: bold;\r\n         word-break:   break-all;\r\n        }\r\n        </style> \r\n        \r\n        <script type='text/javascript'>\r\n        function Onsubmit (id,dest,id3,id4)\r\n        {\r\n          var vv= document.getElementById(id);\r\n          vv.setAttribute('src',dest);\r\n          var ee= document.getElementById(id3);\r\n         \r\n          var dd= document.getElementById(id4);\r\n          dd.style.display='inline';\r\n        }\r\n        function func(name)\r\n        {\r\n         var temp=document.getElementById(name);\r\n         temp.setAttribute('checked','checked');\r\n        }\r\n        function cancelUse()\r\n        {\r\n          var temp=document.getElementsByTagName('input');\r\n          for(var i=0;i<temp.length;i++)\r\n        {\r\n          temp[i].setAttribute('disabled','disabled');\r\n        }\r\n        \r\n        }\r\n        </script>\r\n         </head> \r\n         <body>\r\n          <table id='tb1' cellspacing='0' cellpadding='0' class='box' border='1px' width='500' align='center'> \r\n           <tbody>";
            int num = 1;
            int num2 = 0;
            Random random = new Random();
            int[] numArray = new int[4];
            int num3 = 0;
            for (int i = 0; i < choice.Count; i++)
            {
                numArray[0] = 1;
                numArray[1] = 2;
                numArray[2] = 3;
                numArray[3] = 4;
                for (int j = 0; j < 4; j++)
                {
                    int index = random.Next(4);
                    int num7 = 0;
                    num7 = numArray[index];
                    numArray[index] = numArray[j];
                    numArray[j] = num7;
                }
                string str2 = Application.StartupPath + @"\Download\" + choice[i];
                object obj2 = str;
                str = string.Concat(new object[] { 
                    obj2, "\r\n        <tr> <td id='rowspan", num3, "' align='center' font-size:12px rowspan='5'><img align='center' name='xianshi' id='img", num3, "'/></br><label align='center'><font color='#FF0000' size='9' id='font", num3, "'></font></label></td></tr>\r\n            <tr> \r\n             <td align='center' font-size:12px='' colspan='4'><label> 第", num, "题</label></td> \r\n            </tr> \r\n            <tr> \r\n             <td align='center'><label>题目</label></td> \r\n             <td align='center' font-size:12px='' colspan='3'><img src='", str2, "_0' /></td> \r\n            </tr> \r\n            <tr> \r\n             <td align='center'><input type='radio' id='", ++num2, "'name='", choice[i], "' value='", 
                    numArray[0], "' /></td> \r\n             <td align='center'><img src='", str2, "_", numArray[0], "' /></td> \r\n             <td align='center'><input type='radio' id='", ++num2, "' name='", choice[i], "' value='", numArray[1], "'/></td> \r\n             <td align='center'><img src='", str2, "_", numArray[1], "' /></td> \r\n            </tr> \r\n            <tr> \r\n             <td align='center'><input type='radio' id='", 
                    ++num2, "' name='", choice[i], "'  value='", numArray[2], "'/></td> \r\n             <td align='center'><img src='", str2, "_", numArray[2], "' /></td> \r\n             <td align='center'><input type='radio' id='", ++num2, "' name='", choice[i], "'  value='", numArray[3], "'/></td> \r\n             <td align='center'><img src='", 
                    str2, "_", numArray[3], "' /></td> \r\n            </tr> <tr id='anwser", num3, "' style='display:none'> \r\n     <td align='center'><label>解析</label></td> \r\n     <td align='center' colspan='4'><img  src='", str2, "_5'  /></td> \r\n    </tr>"
                 });
                num++;
                num3++;
            }
            str = str + " </tbody></table></body></html>";
            this.wb.DocumentText = str;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            WaitFormService.CreateWaitForm("正在下载选择题");
            if(!this.ixOffline)
                this.downloadChoice(this.exerciseChoice,false);
            else
            {
                this.downloadChoice(this.exerciseChoice, true);
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string[] collection = this.exerciseChoice.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            WaitFormService.CloseWaitForm();
            this.initShiJuan(new List<string>(collection));
        }

        private void treeView1_AfterCheck_1(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked == true)
            {
                foreach (TreeNode node in e.Node.Nodes)
                    node.Checked = true;
                if (e.Node.GetNodeCount(false) == 0 && !textBox1.Text.Contains(e.Node.Text))
                    textBox1.Text += e.Node.Text + "\r\n";
            }
            else
            {
                foreach (TreeNode node in e.Node.Nodes)
                    node.Checked = false;
                if (e.Node.GetNodeCount(false) == 0)
                    textBox1.Text = textBox1.Text.Replace(e.Node.Text+"\r\n", "");
            }
        }

        //确定按钮
        private void button1_Click(object sender, EventArgs e)
        {
            base.Close();
            Thread th = new Thread(delegate ()
            {
                //遍历最底层选中节点
                string str = "";
                foreach (TreeNode tn1 in treeView1.Nodes)
                {
                    if (tn1.GetNodeCount(false) == 0 && tn1.Checked)
                        str += tn1.Name + ",";
                    foreach (TreeNode tn2 in tn1.Nodes)
                    {
                        if (tn2.GetNodeCount(false) == 0 && tn2.Checked)
                            str += tn2.Name + ",";
                        foreach (TreeNode tn3 in tn2.Nodes)
                        {
                            if (tn3.Checked)
                                str += tn3.Name.ToString() + ",";
                        }
                    }
                }
                string[] a = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> T = a.ToList<string>();
                Dictionary<int, string> dic = new Dictionary<int, string>();
                int count = 0;
                if (ixOffline)
                {
                    try
                    {
                        //遍历离线题库
                        foreach (string s in T)
                        {
                            foreach (string ss in Directory.GetDirectories("ChoiceSource\\" + subject + "\\" + s))
                            {
                                foreach (string sss in Directory.GetFiles(ss))
                                {
                                    dic.Add(++count, sss);
                                }
                            }
                        }
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        MessageBox.Show("离线试题库不完整，请更新题库！");
                    }
                }
                else
                {
                    try
                    {
                        //遍历在线题库
                        foreach (string s in T)
                        {
                            foreach (string ss in DownloadFolder.ftpGetDir("ftp://202.118.26.80/ChoiceSource/" + subject + "/" + s, ""))
                            {

                                foreach (string sss in DownloadFolder.GetFtpFileList(ss, WebRequestMethods.Ftp.ListDirectory))
                                {
                                    dic.Add(++count, sss);
                                }

                            }
                        }
                    }
                    catch (Exception) { MessageBox.Show("题库中没有相应的题！"); }
                }

                //随机抽取试题
                int[] myRandoms = new int[25];
                for (int i = 0; i < (25 < count ? 25 : count); ++i)
                {
                    int myRandom;
                    bool flag = false;
                    do
                    {
                        myRandom = new Random(Guid.NewGuid().GetHashCode()).Next(1, count + 1);
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
                string str2 = "";
                for (int i = 0; i < (25 < count ? 25 : count); ++i)
                {
                    Debug.WriteLine(dic[myRandoms[i]]);
                    str2 += dic[myRandoms[i]] + ",";
                }

                if (ixOffline)
                {
                    //从ChoiceSource\...\*.zip到*.zip
                    string[] strArray = str2.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    str2 = "";
                    string[] strArray_2 = new String[5];
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        strArray_2 = strArray[i].Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                        str2 += strArray_2[4] + ",";
                    }
                }
                //从*.zip到*
                str2 = str2.Replace(".zip", "");

                //删除做对过的题
                string[] strList1 = str2.Split(',');
                string done = "";
                if (File.Exists("OkayChoice_" + subject + ".txt"))
                {
                    FileStream aFile = new FileStream("OkayChoice_" + subject + ".txt", FileMode.Open);
                    StreamReader sw = new StreamReader(aFile);
                    done = sw.ReadToEnd();
                    sw.Close();
                    aFile.Close();
                }
                for (int i = 0; i < strList1.Length; ++i)
                {
                    if (done.Contains(strList1[i]))
                        strList1[i] = "";
                }
                string strList2 = string.Join(",", strList1);
                strList2 = strList2.Replace(",,", ",");
                this.exerciseChoice = strList2;

                //完成
                this.worker.WorkerReportsProgress = true;
                this.worker.RunWorkerAsync();
            });
            th.IsBackground = true;
            th.Start();
        }


    }
}

