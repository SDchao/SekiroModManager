using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace SekiroModManager.IO
{
    class Settings
    {
        private static string path = Environment.CurrentDirectory + "\\config";
        private static string configFilePath = path + "\\config.xml";
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="name">保存节点名称</param>
        /// <param name="value">需要保存的内容</param>
        /// <returns>真为保存正常</returns>
        public static bool SaveConfig(string name, string value)
        {
            try
            {
                // 若文件不存在则新建配置xml
                if (!File.Exists(configFilePath))
                {
                    Directory.CreateDirectory(path);
                    FileStream fileStream = File.Create(configFilePath);
                    fileStream.Close();
                    XmlDocument newXml = new XmlDocument();
                    XmlElement newRoot = newXml.CreateElement("config");
                    newXml.AppendChild(newRoot);
                    newXml.Save(configFilePath);
                }                    
                XmlDocument xml = new XmlDocument();
                xml.Load(configFilePath);
                
                //判断节点是否已经存在
                if(xml.SelectSingleNode("config/" + name) == null)
                {
                    //节点不存在，创建节点
                    XmlNode node = xml.CreateElement(name);
                    node.InnerText = value;
                    xml.DocumentElement.AppendChild(node);
                } else
                {
                    XmlNode node = xml.SelectSingleNode("config/" + name);
                    node.InnerText = value;
                }
                xml.Save(configFilePath);
                return true;
            } catch(Exception e)
            {
                ExceptionHandler.ShowError(e, "保存配置文件错误");
                return false;
            }            
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="nodeName">欲读取的节点名</param>
        /// <returns>节点的值</returns>
        public static string ReadConfig(string name)
        {
            try
            {
                if (!File.Exists(configFilePath))
                    return null;

                XmlDocument xml = new XmlDocument();
                xml.Load(configFilePath);
                XmlNode node = xml.SelectSingleNode("config/" + name);
                if (node == null)
                    return null;
                return node.InnerText;
            } catch (Exception e)
            {
                ExceptionHandler.ShowError(e,"读取配置文件错误");
                return null;
            }
        }
    }
}
