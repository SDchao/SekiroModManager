using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using SekiroModManager.IO;


namespace SekiroModManager
{
    class PathOperation
    {
        private static readonly string[] keywords = { "action" ,"chr","cutscene","event","facegen",
                "font" ,"map","menu","msg","mtd","obj","other","param","parts",
                "script","sfx","shader","sound"};
        /// <summary>
        /// 获取游戏目录，若用户从未设置过则弹出对话框要求用户选择
        /// </summary>
        /// <param name="reselect">若参数为真，则强制要求用户重新选择</param>
        /// <returns>游戏路径</returns>
        public static string GetGamePath(bool reselect)
        {
            string gamePath = Settings.ReadConfig("gamePath");
            Console.WriteLine("读取的目录为：" + gamePath);
            if (gamePath != null && !reselect)
                return gamePath;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择游戏目录";
            bool isCorrectPath = false;
            do
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    gamePath = dialog.SelectedPath;
                    Console.WriteLine("用户选择的游戏路径为：" + gamePath);
                    if (File.Exists(gamePath + "\\sekiro.exe"))
                        isCorrectPath = true;
                }
                Settings.SaveConfig("gamePath", gamePath);
            } while (!isCorrectPath);
            return gamePath;
        }

        /// <summary>
        /// 获取储存Mod路径
        /// </summary>
        /// <returns>返回mod路径，若不存在会自动创建</returns>
        public static string GetModsStorePath()
        {
            string path = Environment.CurrentDirectory + "\\config\\mods";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// 获取Mod索引路径
        /// </summary>
        /// <returns>返回Mod索引路径，若不存在会自动创建</returns>
        public static string GetModsIndexPath()
        {
            string path = GetModsStorePath() + "\\index.xml";
            if(!File.Exists(path))
            {
                FileStream fileStream = File.Create(path);
                fileStream.Close();
                XmlDocument newXml = new XmlDocument();
                XmlElement newRoot = newXml.CreateElement("mods");
                newXml.AppendChild(newRoot);
                newXml.Save(path);
            }
            return path;
        }

        /// <summary>
        /// 判断文件是否为正确的mod文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static bool IsModFilePath(string path)
        {

            foreach(string keyword in keywords)
            {
                if (path.Contains(keyword + "\\"))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取指定路径的标准化Mod内相对文件路径
        /// </summary>
        /// <param name="path">需要标准化的路径</param>
        /// <returns>标准化后的Mod内文件相对路径</returns>
        public static string NormolizeModFilePath(string path)
        {
            string startWord = string.Empty;
            foreach (string keyword in keywords)
            {
                if (path.Contains(keyword + "\\"))
                {
                    startWord = keyword;
                    break;
                }              
            }

            int startIndex = path.IndexOf(startWord);
            string newPath = path.Substring(startIndex);
            return newPath;
        }

        /// <summary>
        /// 获取替换信息文件路径
        /// </summary>
        /// <returns>返回替换信息文件路径，若不存在则创建</returns>
        public static string GetReplacedFilePath()
        {
            string path = GetModsStorePath() + "\\replaced.xml";
            if (!File.Exists(path))
            {
                FileStream fileStream = File.Create(path);
                fileStream.Close();
                XmlDocument newXml = new XmlDocument();
                XmlElement newRoot = newXml.CreateElement("replaced");
                newXml.AppendChild(newRoot);
                newXml.Save(path);
            }
            return path;
        }
    }
}
