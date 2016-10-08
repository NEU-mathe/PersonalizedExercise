namespace ExamSysWinform
{
    using ExamSysWinform._01考试管理;
    using ExamSysWinform.Utils;
    using ExamSysWinform.WebService;
    //    using Sunisoft.IrisSkin;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using System.IO;
    using PersonalizedExercise;
    using System.Diagnostics;
    using Test;
    using System.Collections.Generic;
    using FtpTest;
    using System.Net;
    using System.Text;

    public class StuMain : Form
    {
        private Button btn_ok;
        private Button btnSubmit;
        private IContainer components;
        private int countIsStart;
        private System.Windows.Forms.Timer ExamTime;
        public static bool isExercise = false;
        private bool isFirst;
        private int isSecondCannotSave;
        private Label label1;
        private ToolStripStatusLabel lblLoginType;
        private Label lbShowExamScore;
        private ToolStripMenuItem mnuAbout;
        private ToolStripMenuItem mnuHelp;
        private MenuStrip msStudent;
        private string name;
        private Panel panel1;
        private Random ran;
        private int randtime;
        public Dictionary<string, int> counter=new Dictionary<string, int>(); //counter for the number of Ts
        private ClientExamModel saveModel;
        private System.Windows.Forms.Timer saveStudentAnwserTick;
//        public static ClientExamTemplate selectModel;
        private Label showExamTime;
//        private SkinEngine skinEngine1;
        private TimeSpan span;
        private StatusStrip ssStudent;
        private System.Windows.Forms.Timer timer1;
        private ToolStripStatusLabel tool_NowTime;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripStatusLabel toolStripStatusLabel5;
        private ToolStripStatusLabel toolStripStatusLabel6;
        private ToolStripStatusLabel toolStripStatusLabel8;
        private ToolTip toolTip1;
        private TimeSpan TotalTime;
        private TrackBar trackBar1;
        private ToolStripMenuItem tsmiExam;
        private ToolStripMenuItem tsmiExit;
        private ToolStripMenuItem tsmiSelfInfo;
        private WebBrowser webBrowser1;
        private StudentService ws;
        private ToolStripMenuItem tsmiBaseInfo;
        private ToolStripMenuItem 试卷查询ToolStripMenuItem;
        private ToolStripMenuItem 离线练习在线抽取ToolStripMenuItem;
        private ToolStripMenuItem 离线练习ToolStripMenuItem;
        private ToolStripMenuItem 高等数学ToolStripMenuItem;
        private ToolStripMenuItem 复变函数ToolStripMenuItem;
        private ToolStripMenuItem 概率统计ToolStripMenuItem;
        private ToolStripMenuItem 数学分析ToolStripMenuItem;
        private ToolStripMenuItem 高等数学ToolStripMenuItem1;
        private ToolStripMenuItem 复变函数ToolStripMenuItem1;
        private ToolStripMenuItem 概率统计ToolStripMenuItem1;
        private ToolStripMenuItem 数学分析ToolStripMenuItem1;
        private ToolStripMenuItem 高等数学ToolStripMenuItem2;
        private ToolStripMenuItem 复变函数ToolStripMenuItem2;
        private ToolStripMenuItem 概率统计ToolStripMenuItem2;
        private ToolStripMenuItem 数学分析ToolStripMenuItem2;
        private ToolStripMenuItem 更新题目计数ToolStripMenuItem;
        
        public Label counterLable;
        public static bool saveChoiceOnly = false;


        public StuMain()
        {
            this.span = new TimeSpan(0, 0, 1);
            this.isFirst = true;
            this.ws = new StudentService();
            this.ran = new Random();
            this.saveStudentAnwserTick = new System.Windows.Forms.Timer();
            this.saveModel = new ClientExamModel();
            this.randtime = this.ran.Next(1, 60);
            this.InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            this.saveStudentAnwserTick.Interval = 0x493e0;
            update_offline_status();
            updateCntDic();

        }

        private void update_offline_status()
        {
            bool[] flag = new bool[]{false,false,false,false};
            if (File.Exists("高等数学_GS.ini") && Directory.Exists("ChoiceSource\\高等数学_GS"))
            {
                flag[0] = true;
                高等数学ToolStripMenuItem2.Visible = true;
            }
            if (File.Exists("复变函数_FB.ini") && Directory.Exists("ChoiceSource\\复变函数_FB"))
            {
                flag[1] = true;
                复变函数ToolStripMenuItem2.Visible = true;
            }
            if (File.Exists("概率统计_GL.ini") && Directory.Exists("ChoiceSource\\概率统计_GL"))
            {
                flag[2] = true;
                概率统计ToolStripMenuItem2.Visible = true;
            }
            if (File.Exists("数学分析_SF.ini") && Directory.Exists("ChoiceSource\\数学分析_SF"))
            {
                flag[3] = true;
                数学分析ToolStripMenuItem2.Visible = true;
            }
            if (!(flag[0]||flag[1]||flag[2]||flag[3]))
                离线练习ToolStripMenuItem.Visible = false;
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            HtmlElementCollection elementsByTagName = this.webBrowser1.Document.GetElementsByTagName("input");
            this.webBrowser1.Document.InvokeScript("cancelUse");
            bool flag = true;
            int rightCnt = 0;
            //判断对错和显示对错
            for (int i = 0; i < elementsByTagName.Count; i++)
            {
                if (elementsByTagName[i].GetAttribute("checked") == "True")
                {
                    if (elementsByTagName[i].GetAttribute("Value") == "1")
                    {
                        Debug.WriteLine(elementsByTagName[i].GetAttribute("name"));
                        FileStream aFile = new FileStream("OkayChoice_"+name+".txt", FileMode.Append);
                        ++rightCnt;
                        StreamWriter sw = new StreamWriter(aFile);
                        sw.WriteLine(elementsByTagName[i].GetAttribute("name"));
                        sw.Close();
                        aFile.Close();
                        this.webBrowser1.Document.InvokeScript("Onsubmit", new string[] { "img" + (i / 4), Application.StartupPath + @"\skin\ok", "rowspan" + (i / 4), "anwser" + (i / 4) });
                    }
                    else
                    {
                        this.webBrowser1.Document.InvokeScript("Onsubmit", new string[] { "img" + (i / 4), Application.StartupPath + @"\skin\error", "rowspan" + (i / 4), "anwser" + (i / 4) });
                    }
                    flag = false;
                }
                if ((i % 4) == 3)
                {
                    if (flag)
                    {
                        flag = true;
                        this.webBrowser1.Document.InvokeScript("Onsubmit", new string[] { "img" + (i / 4), Application.StartupPath + @"\skin\error", "rowspan" + (i / 4), "anwser" + (i / 4) });
                    }
                    flag = true;
                }
            }
            counterLable.Text+=" 本次练习共做对"+rightCnt.ToString()+"题";
            counterLable.Left = (msStudent.Width - counterLable.Width) / 2;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExamTime_Tick(object sender, EventArgs e)
        {
            if (this.countIsStart < this.randtime)
            {
                this.countIsStart++;
            }
            this.TotalTime = this.TotalTime.Subtract(this.span);
            if ((this.TotalTime.Hours == 0) && (this.TotalTime.Minutes <= 1))
            {
                this.showExamTime.Text = "试卷正在准备提交";
                this.btnSubmit.Enabled = false;
                if (this.TotalTime.Seconds == this.randtime)
                {
                    this.ExamTime.Stop();
                    //this.submit();
                }
            }
            else
            {
                this.showExamTime.Text = "离考试结束还有" + (((this.TotalTime.Hours * 60) + this.TotalTime.Minutes)).ToString() + "分钟";
            }
        }

        private void initControl()
        {
            this.btnSubmit.Location = new Point(this.webBrowser1.Width - 150, 0x8a);
            this.btn_ok.Location = new Point(this.webBrowser1.Width - 150, 0xc6);
            this.panel1.Location = new Point(Screen.GetWorkingArea(this).Width - 200, Screen.GetWorkingArea(this).Height - 100);
            this.btnSubmit.Hide();
            this.showExamTime.Hide();
            this.btn_ok.Hide();
            this.lbShowExamScore.Hide();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.msStudent = new System.Windows.Forms.MenuStrip();
            this.tsmiSelfInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBaseInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExam = new System.Windows.Forms.ToolStripMenuItem();
            this.试卷查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高等数学ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.复变函数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.概率统计ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数学分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.离线练习在线抽取ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高等数学ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.复变函数ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.概率统计ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.数学分析ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.更新题目计数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.离线练习ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高等数学ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.复变函数ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.概率统计ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.数学分析ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.ssStudent = new System.Windows.Forms.StatusStrip();
            this.lblLoginType = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tool_NowTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.showExamTime = new System.Windows.Forms.Label();
            this.ExamTime = new System.Windows.Forms.Timer(this.components);
            this.lbShowExamScore = new System.Windows.Forms.Label();
            this.btn_ok = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.counterLable = new System.Windows.Forms.Label();
            this.msStudent.SuspendLayout();
            this.ssStudent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // msStudent
            // 
            this.msStudent.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msStudent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSelfInfo,
            this.tsmiExam,
            this.mnuHelp});
            this.msStudent.Location = new System.Drawing.Point(0, 0);
            this.msStudent.Name = "msStudent";
            this.msStudent.Size = new System.Drawing.Size(696, 28);
            this.msStudent.TabIndex = 1;
            this.msStudent.Text = "menuStrip1";
            // 
            // tsmiSelfInfo
            // 
            this.tsmiSelfInfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBaseInfo,
            this.toolStripSeparator1,
            this.tsmiExit});
            this.tsmiSelfInfo.Name = "tsmiSelfInfo";
            this.tsmiSelfInfo.Size = new System.Drawing.Size(81, 24);
            this.tsmiSelfInfo.Text = "个人信息";
            // 
            // tsmiBaseInfo
            // 
            this.tsmiBaseInfo.Name = "tsmiBaseInfo";
            this.tsmiBaseInfo.Size = new System.Drawing.Size(144, 26);
            this.tsmiBaseInfo.Text = "基本资料";
            this.tsmiBaseInfo.Click += new System.EventHandler(this.tsmiBaseInfo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(141, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(144, 26);
            this.tsmiExit.Text = "退出";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tsmiExam
            // 
            this.tsmiExam.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.试卷查询ToolStripMenuItem,
            this.离线练习在线抽取ToolStripMenuItem,
            this.更新题目计数ToolStripMenuItem,
            this.离线练习ToolStripMenuItem});
            this.tsmiExam.Name = "tsmiExam";
            this.tsmiExam.Size = new System.Drawing.Size(81, 24);
            this.tsmiExam.Text = "考试管理";
            // 
            // 试卷查询ToolStripMenuItem
            // 
            this.试卷查询ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.高等数学ToolStripMenuItem,
            this.复变函数ToolStripMenuItem,
            this.概率统计ToolStripMenuItem,
            this.数学分析ToolStripMenuItem});
            this.试卷查询ToolStripMenuItem.Name = "试卷查询ToolStripMenuItem";
            this.试卷查询ToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.试卷查询ToolStripMenuItem.Text = "在线练习";
            // 
            // 高等数学ToolStripMenuItem
            // 
            this.高等数学ToolStripMenuItem.Name = "高等数学ToolStripMenuItem";
            this.高等数学ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.高等数学ToolStripMenuItem.Text = "高等数学";
            this.高等数学ToolStripMenuItem.Click += new System.EventHandler(this.高等数学ToolStripMenuItem_Click);
            // 
            // 复变函数ToolStripMenuItem
            // 
            this.复变函数ToolStripMenuItem.Name = "复变函数ToolStripMenuItem";
            this.复变函数ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.复变函数ToolStripMenuItem.Text = "复变函数";
            this.复变函数ToolStripMenuItem.Click += new System.EventHandler(this.复变函数ToolStripMenuItem_Click);
            // 
            // 概率统计ToolStripMenuItem
            // 
            this.概率统计ToolStripMenuItem.Name = "概率统计ToolStripMenuItem";
            this.概率统计ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.概率统计ToolStripMenuItem.Text = "概率统计";
            this.概率统计ToolStripMenuItem.Click += new System.EventHandler(this.概率统计ToolStripMenuItem_Click);
            // 
            // 数学分析ToolStripMenuItem
            // 
            this.数学分析ToolStripMenuItem.Name = "数学分析ToolStripMenuItem";
            this.数学分析ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.数学分析ToolStripMenuItem.Text = "数学分析";
            this.数学分析ToolStripMenuItem.Click += new System.EventHandler(this.数学分析ToolStripMenuItem_Click);
            // 
            // 离线练习在线抽取ToolStripMenuItem
            // 
            this.离线练习在线抽取ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.高等数学ToolStripMenuItem1,
            this.复变函数ToolStripMenuItem1,
            this.概率统计ToolStripMenuItem1,
            this.数学分析ToolStripMenuItem1});
            this.离线练习在线抽取ToolStripMenuItem.Name = "离线练习在线抽取ToolStripMenuItem";
            this.离线练习在线抽取ToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.离线练习在线抽取ToolStripMenuItem.Text = "更新离线试题库";
            // 
            // 高等数学ToolStripMenuItem1
            // 
            this.高等数学ToolStripMenuItem1.Name = "高等数学ToolStripMenuItem1";
            this.高等数学ToolStripMenuItem1.Size = new System.Drawing.Size(144, 26);
            this.高等数学ToolStripMenuItem1.Text = "高等数学";
            this.高等数学ToolStripMenuItem1.Click += new System.EventHandler(this.高等数学ToolStripMenuItem1_Click);
            // 
            // 复变函数ToolStripMenuItem1
            // 
            this.复变函数ToolStripMenuItem1.Name = "复变函数ToolStripMenuItem1";
            this.复变函数ToolStripMenuItem1.Size = new System.Drawing.Size(144, 26);
            this.复变函数ToolStripMenuItem1.Text = "复变函数";
            this.复变函数ToolStripMenuItem1.Click += new System.EventHandler(this.复变函数ToolStripMenuItem1_Click);
            // 
            // 概率统计ToolStripMenuItem1
            // 
            this.概率统计ToolStripMenuItem1.Name = "概率统计ToolStripMenuItem1";
            this.概率统计ToolStripMenuItem1.Size = new System.Drawing.Size(144, 26);
            this.概率统计ToolStripMenuItem1.Text = "概率统计";
            this.概率统计ToolStripMenuItem1.Click += new System.EventHandler(this.概率统计ToolStripMenuItem1_Click);
            // 
            // 数学分析ToolStripMenuItem1
            // 
            this.数学分析ToolStripMenuItem1.Name = "数学分析ToolStripMenuItem1";
            this.数学分析ToolStripMenuItem1.Size = new System.Drawing.Size(144, 26);
            this.数学分析ToolStripMenuItem1.Text = "数学分析";
            this.数学分析ToolStripMenuItem1.Click += new System.EventHandler(this.数学分析ToolStripMenuItem1_Click);
            // 
            // 更新题目计数ToolStripMenuItem
            // 
            this.更新题目计数ToolStripMenuItem.Name = "更新题目计数ToolStripMenuItem";
            this.更新题目计数ToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.更新题目计数ToolStripMenuItem.Text = "更新题目计数";
            this.更新题目计数ToolStripMenuItem.Click += new System.EventHandler(this.更新题目计数ToolStripMenuItem_Click);
            // 
            // 离线练习ToolStripMenuItem
            // 
            this.离线练习ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.高等数学ToolStripMenuItem2,
            this.复变函数ToolStripMenuItem2,
            this.概率统计ToolStripMenuItem2,
            this.数学分析ToolStripMenuItem2});
            this.离线练习ToolStripMenuItem.Name = "离线练习ToolStripMenuItem";
            this.离线练习ToolStripMenuItem.Size = new System.Drawing.Size(189, 26);
            this.离线练习ToolStripMenuItem.Text = "离线练习";
            // 
            // 高等数学ToolStripMenuItem2
            // 
            this.高等数学ToolStripMenuItem2.Name = "高等数学ToolStripMenuItem2";
            this.高等数学ToolStripMenuItem2.Size = new System.Drawing.Size(181, 26);
            this.高等数学ToolStripMenuItem2.Text = "高等数学";
            this.高等数学ToolStripMenuItem2.Visible = false;
            this.高等数学ToolStripMenuItem2.Click += new System.EventHandler(this.高等数学ToolStripMenuItem2_Click);
            // 
            // 复变函数ToolStripMenuItem2
            // 
            this.复变函数ToolStripMenuItem2.Name = "复变函数ToolStripMenuItem2";
            this.复变函数ToolStripMenuItem2.Size = new System.Drawing.Size(181, 26);
            this.复变函数ToolStripMenuItem2.Text = "复变函数";
            this.复变函数ToolStripMenuItem2.Visible = false;
            this.复变函数ToolStripMenuItem2.Click += new System.EventHandler(this.复变函数ToolStripMenuItem2_Click);
            // 
            // 概率统计ToolStripMenuItem2
            // 
            this.概率统计ToolStripMenuItem2.Name = "概率统计ToolStripMenuItem2";
            this.概率统计ToolStripMenuItem2.Size = new System.Drawing.Size(181, 26);
            this.概率统计ToolStripMenuItem2.Text = "概率统计";
            this.概率统计ToolStripMenuItem2.Visible = false;
            this.概率统计ToolStripMenuItem2.Click += new System.EventHandler(this.概率统计ToolStripMenuItem2_Click);
            // 
            // 数学分析ToolStripMenuItem2
            // 
            this.数学分析ToolStripMenuItem2.Name = "数学分析ToolStripMenuItem2";
            this.数学分析ToolStripMenuItem2.Size = new System.Drawing.Size(181, 26);
            this.数学分析ToolStripMenuItem2.Text = "数学分析";
            this.数学分析ToolStripMenuItem2.Visible = false;
            this.数学分析ToolStripMenuItem2.Click += new System.EventHandler(this.数学分析ToolStripMenuItem2_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(51, 24);
            this.mnuHelp.Text = "帮助";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(114, 26);
            this.mnuAbout.Text = "关于";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // ssStudent
            // 
            this.ssStudent.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ssStudent.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblLoginType,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel8,
            this.tool_NowTime});
            this.ssStudent.Location = new System.Drawing.Point(0, 334);
            this.ssStudent.Name = "ssStudent";
            this.ssStudent.Size = new System.Drawing.Size(696, 25);
            this.ssStudent.TabIndex = 4;
            // 
            // lblLoginType
            // 
            this.lblLoginType.Name = "lblLoginType";
            this.lblLoginType.Size = new System.Drawing.Size(0, 20);
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(93, 20);
            this.toolStripStatusLabel5.Text = "     技术支持:";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(240, 20);
            this.toolStripStatusLabel6.Text = "2015 2645公司  //东北大学理学院";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(105, 20);
            this.toolStripStatusLabel8.Text = "       当前时间: ";
            // 
            // tool_NowTime
            // 
            this.tool_NowTime.Name = "tool_NowTime";
            this.tool_NowTime.Size = new System.Drawing.Size(116, 20);
            this.tool_NowTime.Text = "tool_NowTime";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 28);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(696, 306);
            this.webBrowser1.TabIndex = 6;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(0, 158);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(105, 38);
            this.btnSubmit.TabIndex = 7;
            this.btnSubmit.Text = "提交试卷";
            this.btnSubmit.UseVisualStyleBackColor = true;
//            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // showExamTime
            // 
            this.showExamTime.AutoSize = true;
            this.showExamTime.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.showExamTime.Location = new System.Drawing.Point(12, 70);
            this.showExamTime.Name = "showExamTime";
            this.showExamTime.Size = new System.Drawing.Size(193, 60);
            this.showExamTime.TabIndex = 8;
            this.showExamTime.Text = "离考试结束\r\n还有多长时间";
            // 
            // ExamTime
            // 
            this.ExamTime.Interval = 1000;
            this.ExamTime.Tick += new System.EventHandler(this.ExamTime_Tick);
            // 
            // lbShowExamScore
            // 
            this.lbShowExamScore.AutoSize = true;
            this.lbShowExamScore.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbShowExamScore.Location = new System.Drawing.Point(12, 131);
            this.lbShowExamScore.Name = "lbShowExamScore";
            this.lbShowExamScore.Size = new System.Drawing.Size(103, 30);
            this.lbShowExamScore.TabIndex = 9;
            this.lbShowExamScore.Text = "得分：";
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(0, 212);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(105, 36);
            this.btn_ok.TabIndex = 10;
            this.btn_ok.Text = "完成练习";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.BackColor = System.Drawing.Color.Blue;
            this.trackBar1.Location = new System.Drawing.Point(92, 6);
            this.trackBar1.Maximum = 4;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(104, 32);
            this.trackBar1.SmallChange = 10;
            this.trackBar1.TabIndex = 11;
            this.trackBar1.TickFrequency = 2;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trackBar1.MouseLeave += new System.EventHandler(this.trackBar1_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Location = new System.Drawing.Point(209, 131);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(199, 41);
            this.panel1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "试卷缩放";
            // 
            // counterLable
            // 
            this.counterLable.AutoSize = true;
            this.counterLable.BackColor = System.Drawing.Color.Transparent;
            this.counterLable.Font = new System.Drawing.Font("宋体", 12F);
            this.counterLable.Location = new System.Drawing.Point(268, 5);
            this.counterLable.Name = "counterLable";
            this.counterLable.Size = new System.Drawing.Size(9, 20);
            this.counterLable.TabIndex = 13;
            this.counterLable.Text = "\r\n";
            this.counterLable.Visible = false;
            // 
            // StuMain
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(696, 359);
            this.Controls.Add(this.counterLable);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.lbShowExamScore);
            this.Controls.Add(this.showExamTime);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.ssStudent);
            this.Controls.Add(this.msStudent);
            this.Name = "StuMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "学生窗口 - 大学数学个性化定制练习系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.StuMain_Load);
            this.msStudent.ResumeLayout(false);
            this.msStudent.PerformLayout();
            this.ssStudent.ResumeLayout(false);
            this.ssStudent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("大学数学个性化定制练习系统 v3.0.12910\n\n.新增功能\n1.个性化学习：已经做对过的题在新的练习中会被排除。\n2.离线练习：同步离线题库，随时随地学习。\n.删减功能\n本程序仅提供练习功能，其他功能请使用大学数学过程学习系统。\n.免责声明\n1.本程序仅学习使用，不做商业用途。\n2.用户自愿遵守软件服务协议和共享软件许可协议。\n3.东北大学理学院 保留所有权利\n\n祝您学习愉快\nAM小组 2645公司敬上");
            //new AboutForm().ShowDialog();
        }

        private void StuMain_Load(object sender, EventArgs e)
        {
            this.initControl();
           // base.Icon = Resource.Icon;
           // this.skinEngine1.SkinFile = @"skin\Midsummer.ssk";
            this.timer1.Start();
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.lblLoginType.Text = this.name ;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.ScriptErrorsSuppressed = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.tool_NowTime.Text = DateTime.Now.ToString();
        }

        private void trackBar1_MouseLeave(object sender, EventArgs e)
        {
            this.webBrowser1.Focus();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            string caption = (100 + (this.trackBar1.Value * 10)) + "%";
            this.toolTip1.SetToolTip(this.trackBar1, caption);
            if (this.webBrowser1.Document != null)
            {
                this.webBrowser1.Document.Body.Style = "zoom:" + caption;
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //WaitFormService.CloseWaitForm();
            DirectoryInfo di = new DirectoryInfo("Download");
            this.initControl();
            if (true)//是练习，显示提交按钮
            {
                this.btn_ok.Show();
                isExercise = false;
                try { di.Delete(true); } catch (Exception) { }
            }
            //else if (!selectModel.IsShowStuPaper)//是考试，显示计时器
            //{
            //    TimeSpan span = new TimeSpan((selectModel.ExamTime - selectModel.UsedTime) / 60, (selectModel.ExamTime - selectModel.UsedTime) % 60, 0);
            //    this.TotalTime = (selectModel.EndTime.Subtract(selectModel.NowTime).Duration() > span) ? span : ((TimeSpan) (selectModel.EndTime - selectModel.NowTime));
            //    if ((selectModel.StudentAnwser != "") && selectModel.Enable)
            //    {
            //        string[] strArray = selectModel.StudentAnwser.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //        HtmlElementCollection elementsByTagName = this.webBrowser1.Document.GetElementsByTagName("input");
            //        for (int i = 0; i < (elementsByTagName.Count / 4); i++)
            //        {
            //            for (int j = i * 4; j <= ((i * 4) + 3); j++)
            //            {
            //                if (elementsByTagName[j].GetAttribute("value") == strArray[i])
            //                {
            //                    this.webBrowser1.Document.InvokeScript("func", new string[] { elementsByTagName[j].GetAttribute("id") });
            //                }
            //            }
            //        }
            //    }
            //    this.ExamTime.Start();
            //    this.saveStudentAnwserTick.Start();
            //    this.showExamTime.Text = "离考试结束还有" + (((this.TotalTime.Hours * 60) + this.TotalTime.Minutes)).ToString() + "分钟";
            //    this.btnSubmit.Show();
            //    this.showExamTime.Show();
            //}
            //else//试卷再现
            //{
            //    string[] strArray2 = selectModel.StudentAnwser.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //    HtmlElementCollection elements2 = this.webBrowser1.Document.GetElementsByTagName("input");
            //    for (int k = 0; k < (elements2.Count / 4); k++)
            //    {
            //        bool flag = true;
            //        for (int m = k * 4; m <= ((k * 4) + 3); m++)
            //        {
            //            if (elements2[m].GetAttribute("value") == strArray2[k])
            //            {
            //                flag = false;
            //                this.webBrowser1.Document.InvokeScript("func", new string[] { elements2[m].GetAttribute("id") });
            //                if (strArray2[k] == "1")
            //                {
            //                    string[] args = new string[] { "img" + k, Application.StartupPath + @"\skin\ok", "font" + k, selectModel.EachInfo[elements2[m].GetAttribute("name")].ToString() + "分" };
            //                    this.webBrowser1.Document.InvokeScript("Onsubmit", args);
            //                }
            //                else
            //                {
            //                    this.webBrowser1.Document.InvokeScript("Onsubmit", new string[] { "img" + k, Application.StartupPath + @"\skin\error", "font" + k, "0分" });
            //                }
            //            }
            //        }
            //        if (flag)
            //        {
            //            this.webBrowser1.Document.InvokeScript("Onsubmit", new string[] { "img" + k, Application.StartupPath + @"\skin\error", "font" + k, "0分" });
            //        }
            //    }
            //    this.webBrowser1.Document.InvokeScript("cancelUse");
            //}
        }

        private void tsmiBaseInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此功能已下线，如需此功能请使用大学数学过程学习系统!");
        }


        private void 高等数学ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.initControl();
            name = "高等数学_GS";
            try
            {
                new onlieExercise(this.webBrowser1, false, "高等数学_GS").ShowDialog();
            }
            catch (Exception) { }
        }

        private void 高等数学ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.initControl();
            name = "高等数学_GS";
            updateCntLable(name);
            new onlieExercise(this.webBrowser1, true, "高等数学_GS").ShowDialog();
        }

        private void 复变函数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.initControl();
            name = "复变函数_FB";
            updateCntLable(name);
            try
            {
                new onlieExercise(this.webBrowser1, false, "复变函数_FB").ShowDialog();
            }
            catch (Exception) { }
        }

        private void 复变函数ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.initControl();
            name = "复变函数_FB";
            updateCntLable(name);
            new onlieExercise(this.webBrowser1, true, "复变函数_FB").ShowDialog();
        }

        private void 概率统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.initControl();
            name = "概率统计_GL";
            updateCntLable(name);
            try
            {
                new onlieExercise(this.webBrowser1, false, "概率统计_GL").ShowDialog();
            }
            catch (Exception) { }
        }

        private void 概率统计ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.initControl();
            name = "概率统计_GL";
            updateCntLable(name);
            new onlieExercise(this.webBrowser1, true, "概率统计_GL").ShowDialog();
        }

        private void 数学分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.initControl();
            name = "数学分析_SF";
            updateCntLable(name);
            try
            {
                new onlieExercise(this.webBrowser1, false, "数学分析_SF").ShowDialog();
            }
            catch (Exception) { }
        }

        private void 数学分析ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.initControl();
            name = "数学分析_SF";
            updateCntLable(name);
            new onlieExercise(this.webBrowser1, true, "数学分析_SF").ShowDialog();
        }

        private void updateChoiceData(string subject)
        {
            try
            {
                Generateini get = new Generateini(subject + ".ini");
                get.generate(subject);
                List<choicenode> choices = choicenode.getChoiceList(subject + ".ini");

                bool flag_cfnexist = false;
                //foreach (choicenode cn in choices)
                //{
                //    bool flag_topLevel = true;
                //    foreach(choicenode cn2 in choices)
                //    {
                //        if(cn2.pid == cn.id)
                //            flag_topLevel = false;
                //    }
                //    if (flag_topLevel)
                //        if (!Directory.Exists("ChoiceSource\\" + subject + "\\" + cn.id.ToString()))
                //            flag_cfnexist = true;
                //}
                if (Directory.Exists("ChoiceSource\\" + subject))
                {
                    WaitFormService.CreateWaitForm("请稍候，正在请求ftp…");
                    foreach (string s in DownloadFolder.ftpGetDir("ftp://202.118.26.80/ChoiceSource/" + subject, ""))
                    {//s = ftp://202.118.26.80/ChoiceSource/高等数学_GS//9
                        WaitFormService.SetWaitFormCaption("请稍候，正在核验本地题库…");
                        Debug.WriteLine("i" + s);
                        foreach (string ss in DownloadFolder.ftpGetDir(s, ""))
                        {//ss = ftp://202.118.26.80/ChoiceSource/高等数学_GS//9//Average
                            Debug.WriteLine("ii" + ss);
                            foreach (string sss in DownloadFolder.GetFtpFileList(ss, WebRequestMethods.Ftp.ListDirectory))
                            {
                                string sTail = s.Replace("ftp://202.118.26.80/ChoiceSource/" + subject, "").Replace("/", "");
                                string ssTail = ss.Replace(s, "").Replace("/", "");
                                Debug.WriteLine("iii,stl" + sTail + "sstl" + ssTail + "sss" + sss);
                                if (!File.Exists("ChoiceSource\\" + subject + "\\" + sTail + "\\" + ssTail + "\\" + sss))
                                {
                                    flag_cfnexist = true;
                                    Debug.WriteLine(flag_cfnexist);
                                    break;
                                }
                                Debug.WriteLine(flag_cfnexist);
                            }
                            if (flag_cfnexist)
                                break;
                        }
                        if (flag_cfnexist)
                            break;
                    }
                    WaitFormService.CloseWaitForm();
                }
                else
                {
                    flag_cfnexist = true;
                    Directory.CreateDirectory("ChoiceSource");
                }
                if (flag_cfnexist)
                {
                    if (MessageBox.Show("题库不存在或不是最新的，是否帮助您下载一份？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (Directory.Exists("ChoiceSource\\"+subject))
                        {
                            DirectoryInfo di = new DirectoryInfo("ChoiceSource\\"+subject);
                            di.Delete(true);
                        }
                        WaitFormService.CreateWaitForm("请稍候，题库正在飞奔到您的电脑中呢");
                        DownloadFolder.downftp("ftp://202.118.26.80/ChoiceSource/", subject, Directory.GetCurrentDirectory() + "\\ChoiceSource");
                        WaitFormService.CloseWaitForm();
                        MessageBox.Show("传输完毕");
                    }
                }
                else
                {
                    MessageBox.Show("恭喜你，此科目题库已为最新！");
                }
                update_offline_status();

            }
            catch (Exception)
            {
                MessageBox.Show("与服务器通信失败！请检查网络或联系开发者。");
            }
        }
        private void 高等数学ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            updateChoiceData("高等数学_GS");
        }

        private void 复变函数ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            updateChoiceData("复变函数_FB");
        }

        private void 概率统计ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            updateChoiceData("概率统计_GL");
        }

        private void 数学分析ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            updateChoiceData("数学分析_SF");
        }

        private void 更新题目计数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updateCntDic();
        }
        private int updateCnt(string subject)
        {
            if (Directory.Exists("ChoiceSource\\" + subject))
                return Directory.GetFiles("ChoiceSource\\" + subject+"\\", "*.*", SearchOption.AllDirectories).Length;
            else
                return 0;
        }
        private void updateCntDic()
        {
            counter["高等数学_GS"]=updateCnt("高等数学_GS");
            counter["复变函数_FB"]=updateCnt("复变函数_FB");
            counter["概率统计_GL"]=updateCnt("概率统计_GL");
            counter["数学分析_SF"]=updateCnt("数学分析_SF");
        }
        private void updateCntLable(string subject)
        {
            
            int okayCnt = 0;
            if(File.Exists("OkayChoice_" + subject + ".txt"))
            {
                string[] lines = File.ReadAllLines("OkayChoice_" + subject + ".txt", Encoding.UTF8);
                okayCnt = lines.Length;
            }
            counterLable.Text = "当前科目：" + subject + " 题目总数：" + 
                        counter[name].ToString() + " 已攻破题数：" + okayCnt.ToString()+" 剩余题目数："+(counter[name]-okayCnt).ToString();
            counterLable.Visible = true;
            counterLable.Left = (msStudent.Width - counterLable.Width) / 2;
        }
    }
}

