namespace ExamSysWinform.Utils
{
    using ExamSysWinform;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    public class WaitFormService
    {
        private static WaitFormService _instance;
        private static readonly object syncLock = new object();
        private waitForm waitFM;
        private Thread waitThread;

        private WaitFormService()
        {
        }

        public void CloseForm()
        {
            if (this.waitThread != null)
            {
                try
                {
                    this.waitFM.SetText("close");
                    this.waitThread = null;
                    this.waitFM = null;
                }
                catch (Exception)
                {
                }
            }
        }

        public static void CloseWaitForm()
        {
            Instance.CloseForm();
        }

        public void CreateForm(string text)
        {
            if (this.waitThread != null)
            {
                try
                {
                    this.waitThread = null;
                    this.waitFM = null;
                }
                catch (Exception)
                {
                }
            }
            this.waitThread = new Thread((ParameterizedThreadStart)delegate {
                this.waitFM = new waitForm(text);
                Application.Run(this.waitFM);
            });
            this.waitThread.Start();
        }

        public static void CreateWaitForm(string text)
        {
            Instance.CreateForm(text);
        }

        public void SetFormCaption(string text)
        {
            if (this.waitFM != null)
            {
                try
                {
                    this.waitFM.SetText(text);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void SetWaitFormCaption(string text)
        {
            Instance.SetFormCaption(text);
        }

        public static WaitFormService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new WaitFormService();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}

