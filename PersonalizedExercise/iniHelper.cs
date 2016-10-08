using System;
using System.Text;

namespace Test
{
    class iniHelper
    {
        /// <summary>
        /// 设定INI文件中的属性
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filePath">INI文件的绝对地址</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(
            string section, string key, string val, string filePath);

        /// <summary>
        /// 读取INI文件中的属性
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="def">默认值</param>
        /// <param name="retVal">被存储到的StringBuilder</param>
        /// <param name="size">最大字串截取长度</param>
        /// <param name="filePath">INI文件的绝对地址</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
            string section, string key, string def,
            System.Text.StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// INI文件的绝对路径
        /// </summary>
        public string Path;

        /// <summary>
        /// 读取的默认值
        /// </summary>
        public string DefValue;

        /// <summary>
        /// INI读写工具类
        /// </summary>
        /// <param name="path">INI文件绝对地址</param>
        /// <param name="def">读取值的默认值</param>
        public iniHelper(string path, string def = "")
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            if (!fi.Exists)
            {
                throw new Exception("文件不存在");
            }
            this.Path = fi.FullName;
            this.DefValue = def;
        }

        /// <summary>
        /// 设置INI文件的一个值
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void SetValue(string section, string key, object value)
        {
            WritePrivateProfileString(section, key, value.ToString(), this.Path);
        }

        /// <summary>
        /// 获取INI文件的一个值
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string section, string key)
        {
            StringBuilder sb = new StringBuilder(255);
            int i = GetPrivateProfileString(
                section, key, this.DefValue, sb, 255, this.Path);
            return sb.ToString();
        }
    }
}
