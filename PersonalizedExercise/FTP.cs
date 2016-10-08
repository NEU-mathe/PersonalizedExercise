using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

namespace FtpTest
{
    class DownloadFolder
    {
        #region 单个文件下载
        /// <summary>
        /// 单个文件下载方法
        /// </summary>
        /// <param name="adss">保存文件的本地路径</param>
        /// <param name="remotePaths">下载文件的FTP路径</param>
        public static void download(string adss, string remotePaths)
        {
            //FileMode常数确定如何打开或创建文件,指定操作系统应创建新文件。
            //FileMode.Create如果文件已存在，它将被改写
            FileStream outputStream = new FileStream(adss, FileMode.Create);
            FtpWebRequest downRequest = (FtpWebRequest)WebRequest.Create(new Uri(remotePaths));
            downRequest.UseBinary = true;
            downRequest.Credentials = new NetworkCredential("LoginName", "Q191KPgC");
            //设置要发送到 FTP 服务器的命令
            downRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            FtpWebResponse response = (FtpWebResponse)downRequest.GetResponse();
            Stream ftpStream = response.GetResponseStream();
            long cl = response.ContentLength;
            int bufferSize = 2048;
            int readCount;
            byte[] buffer = new byte[bufferSize];
            readCount = ftpStream.Read(buffer, 0, bufferSize);
            while (readCount > 0)
            {
                outputStream.Write(buffer, 0, readCount);
                readCount = ftpStream.Read(buffer, 0, bufferSize);
            }
            ftpStream.Close();
            outputStream.Close();
            response.Close();
        }
        #endregion




        #region 得到ftp服务器端指定文件夹下的目录
        /// </summary>
        /// <param name="remotePath">FTP地址路径</param>
        /// <param name="name">我们所选择的文件或者文件夹名字</param>
        /// <param name="type">要发送到FTP服务器的命令</param>
        /// <returns></returns>
        public static string[] ftp(string remotePath,string name,string type)
        {
            WebResponse webresp = null;
            StreamReader ftpremoteFileListReader = null;
            FtpWebRequest ftpRequest=null;
            try
            {
                 ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri(remotePath +"/" + name));
                 ftpRequest.Method = type;
                 ftpRequest.Credentials = new NetworkCredential("LoginName", "Q191KPgC");
                 webresp = ftpRequest.GetResponse();
                 ftpremoteFileListReader = new StreamReader(webresp.GetResponseStream(), Encoding.UTF8);
            }
            catch(Exception ex)
            {
                ex.ToString();
                
            }
            StringBuilder str = new StringBuilder();
            string line=ftpremoteFileListReader.ReadLine();
            while (line != null)
            {
                str.Append(line);
                str.Append("\n");
                line = ftpremoteFileListReader.ReadLine();
            }
            string[] fen = str.ToString().Split('\n');
            return fen;
        }
        #endregion

        #region ftp文件列表
        /// <summary>
        /// 获得ftp文件列表
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<string> GetFtpFileList(string ftpPath, string type)
        {
            try
            {
                List<string> fileInfiList = new List<string>();
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
                ftpRequest.Method = type;
                ftpRequest.Credentials = new NetworkCredential("LoginName", "Q191KPgC");
                WebResponse webresp = ftpRequest.GetResponse();
                StreamReader ftpFileListReader = new StreamReader(webresp.GetResponseStream(), Encoding.UTF8);

                StringBuilder result = new StringBuilder();
                string line = ftpFileListReader.ReadLine();
                while (line != null)
                {
                    fileInfiList.Add(line);
                    line = ftpFileListReader.ReadLine();
                }
                ftpFileListReader.Close();
                webresp.Close();
                return fileInfiList;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        #endregion

        #region FTP文件列表
        public static List<string> ftpGetDir(string remotePath, string name)
        {
            string downloadRemotePath = remotePath + "/" + name;
            string[] remoteDetails = ftp(remotePath, name, WebRequestMethods.Ftp.ListDirectoryDetails);
            List<string> gotDir = new List<string>();
            //foreach (string s in remoteDetails)
            //{
            //    string sp_size = "";
            //    string sp_fname = "";
            //    if (s != "" && s != null)
            //    {
            //        sp_size = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[4];
            //        sp_fname = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[5];
            //        MessageBox.Show(sp_size + sp_fname);
            //    }
            //}

            //判断是否为单个文件 
            if (remoteDetails.Length <= 2)
            {
                //if (remoteDetails[remoteDetails.Length - 1] == "")
                //{
                //    download(downloadLocalPath, downloadRemotePath);
                //}
            }
            else
            {
                string[] remoteFiles = ftp(remotePath, name, WebRequestMethods.Ftp.ListDirectory);
                Hashtable remoteFileList = new Hashtable();
                foreach (string fileName in remoteFiles)
                {
                    if (fileName == "")
                        continue;
                    remoteFileList.Add(fileName, fileName);
                }
                foreach (string remoteFileOrFolder in remoteDetails)
                {
                    if (remoteFileOrFolder == "")
                        continue;
                    string[] ss = remoteFileOrFolder.Split(' ');
                    List<string> info = new List<string>();
                    foreach (string a in ss)
                    {
                        if (a != "")
                            info.Add(a);
                    }
                    string shortName = info[info.Count - 1];
                    if (shortName == "." || shortName == "..")
                        continue;
                    //判断是否具有文件夹标识<DIR>
                    if (!isNotFolder(shortName, remoteFileList))
                    {
                        gotDir.Add(downloadRemotePath+"/"+shortName);
                        //downftp(downloadRemotePath, shortName, downloadLocalPath);
                    }
                    else
                    {
                        //download(downloadLocalPath + "/" + shortName, downloadRemotePath + "/" +
                        //shortName);
                    }
                }
            }

            return gotDir;
        }
        #endregion

        #region 下载文件夹
        /// <summary>
        /// 下载方法
        /// </summary>
        /// <param name="remotePath">FTP路径</param>
        /// <param name="name">需要下载文件路径</param>
        /// <param name="localPath">保存的本地路径</param>
        public static void downftp(string remotePath, string name, string localPath)
        {
            string downloadLocalPath = localPath + "/" + name;
            string downloadRemotePath = remotePath + "/" + name;
            string[] remoteDetails = ftp(remotePath, name, WebRequestMethods.Ftp.ListDirectoryDetails);

            if (remoteDetails.Length <= 2)
            {
                if (remoteDetails[remoteDetails.Length - 1] == "")
                {
                    download(downloadLocalPath, downloadRemotePath);
                }
            }
            else
            {
                string[] remoteFiles = ftp(remotePath, name, WebRequestMethods.Ftp.ListDirectory);
                Hashtable remoteFileList = new Hashtable();
                foreach (string fileName in remoteFiles)
                {
                    if (fileName == "")
                        continue;
                    remoteFileList.Add(fileName, fileName);
                }
                DirectoryInfo d = new DirectoryInfo(downloadLocalPath);
                if (!d.Exists)
                {
                    d.Create();
                }
                foreach (string remoteFileOrFolder in remoteDetails)
                {
                    if (remoteFileOrFolder == "")
                        continue;
                    string[] ss = remoteFileOrFolder.Split(' ');
                    List<string> info = new List<string>();
                    foreach (string a in ss)
                    {
                        if (a != "")
                            info.Add(a);
                    }
                    string shortName = info[info.Count - 1];
                    if (shortName == "." || shortName == "..")
                        continue;
                    //判断是否具有文件夹标识<DIR>
                    if (!isNotFolder(shortName, remoteFileList))
                    {
                        downftp(downloadRemotePath, shortName, downloadLocalPath);
                    }
                    else
                    {
                        download(downloadLocalPath + "/" + shortName, downloadRemotePath + "/" +
                        shortName);  
                    }
                }
            }
        }
        #endregion




        #region 判断fileName是一个文件
        /// <summary>
        /// 判断fileName是一个文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool isNotFolder(string fileName,Hashtable table)
        {
            bool flag = false;
            foreach (DictionaryEntry each in table)
            {
                if(fileName == each.Value.ToString())
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }
        #endregion
    }
}
