using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.IO.Compression;

namespace SekiroModManager.Manager
{
    class ModManager
    {
        /// <summary>
        /// 导入mod，移动源文件到config/mods目录下，删除源文件后创建配置文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <returns></returns>
        public bool ImportMod(string sourcePath)
        {
            if(Path.GetExtension(sourcePath) != ".zip")
            {
                MessageBox.Show("暂不支持的文件类型", "错误", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
            //获取用户mod命名
            string newName = String.Empty;
            Form_InputModName form_InputModName = new Form_InputModName();
            form_InputModName.textBox.Text = Path.GetFileNameWithoutExtension(sourcePath);
            form_InputModName.textBox.Focus();
            form_InputModName.textBox.SelectAll();
            
            DialogResult result = form_InputModName.ShowDialog();
            if (result != DialogResult.OK)
                return false;
            newName = form_InputModName.textBox.Text;
            form_InputModName.Close();

            //检测重名
            if (GetModInformation(newName, false) != null)
            {

                DialogResult dialogResult = MessageBox.Show("已经存在同名文件，是否进行覆盖？", "提示", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if(dialogResult == DialogResult.Yes)
                {
                    DeleteMod(newName);
                } else
                {
                    return false;
                }
                
            }

            try
            {                
                File.Copy(sourcePath, PathOperation.GetModsStorePath() + "\\" + newName + ".mod");
                InitNewMod(PathOperation.GetModsStorePath() + "\\" + newName + ".mod");
                return true;
            }
            catch(Exception e)
            {
                ExceptionHandler.ShowError(e, "移动文件出现错误");
                return false;
            }
        }

        private void InitNewMod(string sourcePath)
        {
            string name = Path.GetFileNameWithoutExtension(sourcePath);
            XmlDocument xml = new XmlDocument();
            xml.Load(PathOperation.GetModsIndexPath());

            XmlElement basic = xml.CreateElement("mod_" + name);
            XmlElement size = xml.CreateElement("size");
            XmlElement time = xml.CreateElement("time");

            //文件大小
            FileInfo fileInfo = new FileInfo(sourcePath);
            size.InnerText = ((float)fileInfo.Length / 1024 / 1024).ToString("0.##");
            basic.AppendChild(size);

            //文件时间
            time.InnerText = fileInfo.CreationTime.ToString();
            basic.AppendChild(time);

            //文件列表
            XmlElement fileBasic = xml.CreateElement("files");
            List<string> fileList = InitNewModFiles(sourcePath);
            if(fileList.Count == 0)
            {
                MessageBox.Show("这不是只狼Mod文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DeleteMod(name);
                return;
            }
            foreach(string path in fileList)
            {
                XmlElement file = xml.CreateElement("file");
                file.InnerText = path;
                fileBasic.AppendChild(file);
            }
            basic.AppendChild(fileBasic);

            xml.DocumentElement.AppendChild(basic);
            xml.Save(PathOperation.GetModsIndexPath());
        }

        private List<string> InitNewModFiles(string sourcePath)
        {
            List<string> list = new List<string>();
            try
            {
                string tempPath = Path.GetTempPath() + "smm";
                if(Directory.Exists(tempPath))
                    Directory.Delete(tempPath, true);
                ZipFile.ExtractToDirectory(sourcePath, tempPath);
                Console.WriteLine("已经 " + sourcePath + "解压至" + tempPath); 
                string nTempPath = Path.GetTempPath() + @"smm_nFiles";
                Directory.CreateDirectory(nTempPath);
                foreach(string filePath in 
                    Directory.GetFiles(tempPath,"*",SearchOption.AllDirectories))
                {
                    if(PathOperation.IsModFilePath(filePath))
                    {
                        string nPath = PathOperation.NormolizeModFilePath(filePath);
                        Console.WriteLine("标准路径为：" + nPath);
                        list.Add(nPath);
                        Directory.CreateDirectory(Path.GetDirectoryName(nTempPath + "\\" + nPath));
                        File.Copy(filePath, nTempPath + "\\" + nPath,true);
                    }
                }
                File.Delete(sourcePath);
                if(list.Count > 0)
                    ZipFile.CreateFromDirectory(nTempPath,
                        sourcePath, CompressionLevel.Fastest, false);
                Directory.Delete(tempPath,true);
                Directory.Delete(nTempPath,true);
                return list;
            }
            catch(Exception e)
            {
                ExceptionHandler.ShowError(e, "无法初始化Mod信息");
                return list;
            }           
        }



        /// <summary>
        /// 获取全部Mod信息
        /// </summary>
        /// <returns>返回ModInformationList</returns>
        public List<ModInformation> GetAllModsInformation()
        {
            List<ModInformation> list = new List<ModInformation>();
            try
            {
                foreach(string path in 
                    Directory.GetFiles(PathOperation.GetModsStorePath(), "*.mod", SearchOption.TopDirectoryOnly))
                {
                    ModInformation information = GetModInformation(Path.GetFileNameWithoutExtension(path),true);
                    if(information != null)
                    {
                        list.Add(information);
                    }                   
                }
                return list;
            } catch(Exception e)
            {
                ExceptionHandler.ShowError(e, "检索全部Mod信息出错");
                return list;
            }
            

        }

        /// <summary>
        /// 获取特定Mod信息
        /// </summary>
        /// <param name="name">欲获取Mod的名称</param>
        /// <param name="shouldHave">应该存在，若为真则产生报错信息</param>
        /// <returns>返回ModInformation</returns>
        public ModInformation GetModInformation(string name,bool shouldHave)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(PathOperation.GetModsIndexPath());
                string time = xml.SelectSingleNode("mods/mod_" + name + "/time").InnerText;
                string size = xml.SelectSingleNode("mods/mod_" + name + "/size").InnerText;
                List<string> contain = new List<string>();

                XmlNodeList fileNodes = xml.SelectNodes("mods/mod_" + name + "/files/file");
                foreach(XmlNode node in fileNodes)
                {
                    contain.Add(node.InnerText);
                }
                ModInformation information = new ModInformation(name, time, size,contain);
                return information;
            } catch(Exception e)
            {
                if(shouldHave)
                    ExceptionHandler.ShowError(e, "获取 " + name + " 信息出错");
                return null;
            }           
        }

        /// <summary>
        /// 删除Mod
        /// </summary>
        /// <param name="name">欲删除Mod的文件名</param>
        /// <returns>成功则返回真</returns>
        public bool DeleteMod(string name)
        {
            List<ReplacedInformation> informations = GetReplacedFiles();
            List<string> installedFilesPaths = new List<string>();
            foreach(ReplacedInformation i in informations)
            {
                if (i.name == name)
                    installedFilesPaths.Add(i.path);
            }

            if(installedFilesPaths.Count > 0)
            {
                string warningText = "该Mod的部分文件已经被安装" + Environment.NewLine;
                foreach(string path in installedFilesPaths)
                {
                    warningText += path + Environment.NewLine;
                }
                warningText += "是否仍要删除该Mod与已安装文件？";
                DialogResult result = MessageBox.Show(warningText, "警告",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.No)
                    return true;
            }

            try
            {
                File.Delete(PathOperation.GetModsStorePath() + "\\" + name + ".mod");
                //删除Index中的节点
                XmlDocument xml = new XmlDocument();
                xml.Load(PathOperation.GetModsIndexPath());
                XmlNode node = xml.SelectSingleNode("mods/mod_" + name);
                if(node != null)
                    xml.DocumentElement.RemoveChild(node);
                xml.Save(PathOperation.GetModsIndexPath());

                //删除已替换文件
                UnInstallFiles(installedFilesPaths,false);
                return true;
            } catch(Exception e)
            {
                ExceptionHandler.ShowError(e, "无法删除Mod");
                return false;
            }
        }

        /// <summary>
        /// 安装所给文件
        /// </summary>
        /// <param name="name">欲安装文件的Mod名称</param>
        /// <param name="paths">欲安装的文件列表</param>
        /// <returns>若成功安装，返回真</returns>
        public bool InstallFiles(string name,List<string> paths)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(PathOperation.GetReplacedFilePath());
            if (paths.Count == 0)
                return true;
            try
            {
                //检测冲突
                List<ReplacedInformation> replacedFiles = GetReplacedFiles();
                List<ReplacedInformation> conflictFiles = new List<ReplacedInformation>();
                foreach(ReplacedInformation i in replacedFiles)
                {
                    foreach(string path in paths)
                    {
                        if(path == i.path)
                        {
                            conflictFiles.Add(i);
                            break;
                        }
                    }
                }
                //若有冲突，弹出警告
                if(conflictFiles.Count > 0)
                {
                    string warningText = "以下Mod文件有冲突：" + Environment.NewLine;
                    foreach (ReplacedInformation i in conflictFiles)
                    {
                        warningText += i.name + " 中的 " + i.path + Environment.NewLine;
                    }
                    warningText += "是否要覆盖上述文件？";
                    DialogResult result = MessageBox.Show(warningText, "警告", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.No)
                        return true;
                    //若选择继续覆盖，删除原有文件及XML信息
                    foreach(ReplacedInformation i in conflictFiles)
                    {
                        File.Delete(PathOperation.GetGamePath(false) + "\\mods\\" + i.path);
                        foreach(XmlNode node in xml.SelectNodes("replaced/file"))
                        {
                            if(node.InnerText == i.path)
                            {
                                xml.DocumentElement.RemoveChild(node);
                                break;
                            }
                        }
                    }
                }
                

                //保存配置文件
                foreach(string path in paths)
                {
                    XmlElement ele = xml.CreateElement("file");
                    ele.InnerText = path;
                    ele.SetAttribute("mod", name);
                    xml.DocumentElement.AppendChild(ele);
                }
                xml.Save(PathOperation.GetReplacedFilePath());
                //解压文件
                string targetPath = PathOperation.GetGamePath(false) + @"\mods";
                string sourcePath = PathOperation.GetModsStorePath() + "\\" + name + ".mod";
                //解压至临时目录
                string tempPath = Path.GetTempPath() + "smm";
                if (Directory.Exists(tempPath))
                    Directory.Delete(tempPath, true);
                ZipFile.ExtractToDirectory(sourcePath, tempPath);
                Console.WriteLine("已经 " + sourcePath + "解压至" + tempPath);
                foreach(string path in paths)
                {
                    if(!Directory.Exists(targetPath + "\\" + Path.GetDirectoryName(path)))
                        Directory.CreateDirectory(targetPath + "\\" + Path.GetDirectoryName(path));
                    File.Copy(tempPath + "\\" + path, targetPath + "\\" + path);
                }
                Directory.Delete(tempPath, true);
                MessageBox.Show("所选文件已安装成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            } catch(Exception e)
            {
                ExceptionHandler.ShowError(e, "安装文件出现错误");
                return false;
            }
        }

        /// <summary>
        /// 获取已经安装的文件路径及Mod名称
        /// </summary>
        /// <returns></returns>
        public List<ReplacedInformation> GetReplacedFiles()
        {
            List<ReplacedInformation> replacedFiles = new List<ReplacedInformation>();

            XmlDocument xml = new XmlDocument();
            xml.Load(PathOperation.GetReplacedFilePath());

            XmlNodeList nodeList = xml.SelectNodes("replaced/file");
            foreach(XmlNode node in nodeList)
            {
                string path = node.InnerText;
                string name = node.Attributes["mod"].Value;
                ReplacedInformation information = new ReplacedInformation(path, name);
                replacedFiles.Add(information);
            }
            return replacedFiles;
        }


        public bool UnInstallFiles(List<string> paths,bool needWarn)
        {
            if(needWarn)
            {
                string warningText;
                warningText = "确认要卸载以下文件？" + Environment.NewLine;
                foreach (string path in paths)
                {
                    warningText += path + Environment.NewLine;
                }

                DialogResult result = MessageBox.Show(warningText, "警告",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.No)
                    return true;
            }
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(PathOperation.GetReplacedFilePath());

                foreach(string path in paths)
                {
                    foreach (XmlNode node in xml.SelectNodes("replaced/file"))
                    {
                        Console.WriteLine(path);
                        if(path == node.InnerText)
                        {
                            xml.DocumentElement.RemoveChild(node);
                            break;
                        }
                    }
                    File.Delete(PathOperation.GetGamePath(false) + "\\mods\\" + path);
                    
                }
                xml.Save(PathOperation.GetReplacedFilePath());
                return true;
            } catch(Exception e)
            {
                ExceptionHandler.ShowError(e, "卸载所选文件出错");
                return false;
            }
        }
    }
}
