using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.XWPF.UserModel;
using iText.Kernel.Pdf;
using System.Text;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;

namespace fileQuery20230621.Webs
{
    public partial class MainForm : System.Web.UI.Page
    {
        //用于存储要搜索的文件类型
        private string[] fileTypes = { "*.txt", "*.docx", "*.pdf" };
        //用于存储搜索到符合类型的文件路径
        private List<string> pathList = new List<string>();
        //用于存储上级目录
        private static Stack<string> lastPath = new Stack<string>();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //浏览按钮
        protected void Button1_Click(object sender, EventArgs e)
        {
            getFilePath();
        }
        //查找按钮
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            //用于存储绑定CheckBoxList的信息
            List<QueryInfo> checkBoxInfoList = new List<QueryInfo>();
            //先绑定空列表用于清空之前的绑定信息
            bindCheckListBox(checkBoxInfoList);
            //如果没有选择文件则退出
            if (txtFilePath.Text == "")
            {
                return;
            }
            pathList = getFileInfo(txtFilePath.Text);
            //没有文件路径则退出
            if(pathList.Count == 0)
            {
                return;
            }
            checkBoxInfoList = getQueryInfoList(pathList);
            if (checkBoxInfoList.Count != 0)
            {
                bindCheckListBox(checkBoxInfoList);
            }
        }
        //下载按钮
        protected void Button1_Click1(object sender, EventArgs e)
        {
            //如果没有选中项则退出
            if(CheckBoxList1.SelectedIndex == -1)
            {
                return;
            }
            
            //用于存储选中项
            List<string> selectedItems = new List<string>();
            foreach(ListItem listItem in CheckBoxList1.Items)
            {
                if (listItem.Selected)
                {
                    selectedItems.Add(listItem.Value);
                }
            }

            //设置默认文件名
            string fileName = DateTime.Now.ToString("yyyyMMddhhmmss");  
            //保存文件
            using (StreamWriter sw = new StreamWriter($"{txtFilePath.Text}//{fileName}.txt"))
            {
                foreach (string s in selectedItems)
                {
                    string temp = s.Replace("<br>", "\n");
                    temp = temp.Replace("</span>", "");
                    temp = temp.Replace("<span style=\"color:red\">", "");
                    sw.WriteLine(temp);
                }
            }

        }

        //获取根目录路径并绑定DataList
        string getFilePath()
        {
            string path = "";
            List<PathInfo> pathBindList = new List<PathInfo>();
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives) {
                PathInfo temp = new PathInfo(drive.Name, drive.RootDirectory.ToString());
                pathBindList.Add(temp);
            };
            bindDataList(pathBindList);
            return path;
        }

        //根据fileTypes中的文件类型在指定目录下查找文件
        List<string> getFileInfo(string path)
        {
            List<string> filePaths = new List<string>();
            foreach (string type in fileTypes)
            {
                string[] files = Directory.GetFiles(path, type, SearchOption.AllDirectories);
                Console.WriteLine($"{type}:");
                foreach (string file in files)
                {
                    filePaths.Add(file);
                }
            }
            return filePaths;
        }

        //在文件查找指定字符串所在行
        List<QueryInfo> getQueryInfoList(List<string> pathList)
        {
            List<QueryInfo> checkBoxInfoList = new List<QueryInfo>();
            //遍历文件
            foreach (string s in pathList)
            {
                string line;
                string titleTxt = s + "<br>----------------------<br>";
                string fileEnd = "======================<br>";
                string boxInfo = "";
                int lineNum = 0;
                bool isGetText = false;
                
                if(s.Substring(s.Length - 4, 4) == ".txt")
                {
                    StreamReader file = new StreamReader(s);
                    //查看每行
                    while ((line = file.ReadLine()) != null)
                    {
                        lineNum++;
                        if (isContain(line, txtQueryText.Text))
                        {
                            isGetText = true;
                            boxInfo += $"{lineNum} {line}<br>";
                        }
                    }
                }
                else if(s.Substring(s.Length - 5, 5) == ".docx")
                {
                    Stream stream = File.OpenRead(s);
                    //如果读取文件的内容为空则跳过
                    if (stream.Length == 0) continue;
                    XWPFDocument doc = new XWPFDocument(stream);
                    foreach (var wLine in doc.Paragraphs)
                    {
                        line = wLine.ParagraphText;
                        lineNum++;
                        if (isContain(line, txtQueryText.Text))
                        {
                            isGetText = true;
                            boxInfo += $"{lineNum} {line}<br>";
                        }
                    }
                }
                else if(s.Substring(s.Length - 4, 4) == ".pdf")
                {
                    //Response.Write(s);
                    Stream stream = File.OpenRead(s);
                    //如果读取文件的内容为空则跳过
                    if (stream.Length == 0) continue;
                    PdfReader pr = new PdfReader(s);
                    PdfDocument pd = new PdfDocument(pr);
                    StringBuilder sb = new StringBuilder();

                    int count = pd.GetNumberOfPages();
                    for(int i = 1; i <= count; i++)
                    {
                        PdfPage pp = pd.GetPage(i);
                        ITextExtractionStrategy tes = new LocationTextExtractionStrategy();
                        string text = PdfTextExtractor.GetTextFromPage(pp, tes);
                        sb.Append(text);
                    }
                    pd.Close();
                    //Response.Write(sb.ToString());
                    string all = sb.ToString();
                    line = "";
                    foreach(var c in all)
                    {
                        if(c != '\n')
                        {
                            line += c;
                        }
                        else
                        {
                            lineNum++;
                            if (isContain(line, txtQueryText.Text))
                            {
                                isGetText = true;
                                boxInfo += $"{lineNum} {line}<br>";
                            }
                            line = "";
                        }
                    }
                    //判断最后一行
                    lineNum++;
                    if (isContain(line, txtQueryText.Text))
                    {
                        isGetText = true;
                        boxInfo += $"{lineNum} {line}<br>";
                    }
                }

                boxInfo = boxInfo.Replace(txtQueryText.Text, $"<span style=\"color:red\">{txtQueryText.Text}</span>");
                boxInfo = titleTxt + boxInfo;
                boxInfo += fileEnd;
                //文件中查找到对应的字符串
                if (isGetText)
                {
                    checkBoxInfoList.Add(new QueryInfo(boxInfo));
                }
            }

            return checkBoxInfoList;
        }
        //绑定CheckBoxList数据
        void bindCheckListBox(List<QueryInfo> info)
        {
            CheckBoxList1.DataSource = info;
            CheckBoxList1.DataTextField = "Info";
            CheckBoxList1.DataValueField = "Info";
            CheckBoxList1.DataBind();
        }
        
        //绑定DataList数据 TODO:
        void bindDataList(List<PathInfo> info)
        {
            DataList1.DataSource = info;
            DataList1.DataBind();
        }

        //判断字符串中是否有子串
        bool isContain(String str, String key)
        {
            if (str == null || key == null)
            {
                return false;
            }
            if (str.Length < key.Length)
            {
                return false;
            }
            if (str.Length == key.Length && !str.Equals(key))
            {
                return false;
            }
            if (str.Equals(key))
            {
                return true;
            }
            char[] strChars = str.ToCharArray();
            char[] keyChars = key.ToCharArray();
            int k = 0;
            
            for (int i = 0; i < strChars.Length - key.Length + 1; i++)
            {
                k = 0;
                for (int j = 0, b = i; j < keyChars.Length && b < strChars.Length; j++, b++)
                {
                    if (strChars[b] == keyChars[j])
                    {
                        k++;
                        if (k == keyChars.Length)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return false;
        }
        //DataList1事件响应
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            //进入按钮
            if(e.CommandName == "enter")
            {
                string nowPath = Convert.ToString(e.CommandArgument);
                Response.Write($"enter {nowPath}");
                List<PathInfo> pathBindList = getNewPathList(nowPath, false);
                bindDataList(pathBindList);
            }
            //选则按钮
            else if(e.CommandName == "chose")
            {
                string nowPath = Convert.ToString(e.CommandArgument);
                //Response.Write($"chose {nowPath}");
                txtFilePath.Text = nowPath;
            }
        }

        //获取指定文件目录下的文件内容
        List<PathInfo> getNewPathList(string path, bool isBack)
        {
            if (!isBack)
            {
                lastPath.Push(path);
            }
            List<PathInfo> pathBindList = new List<PathInfo>();
            DirectoryInfo root = new DirectoryInfo(path);
            
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                string name = d.Name;
                string fullName = d.FullName;
                PathInfo temp = new PathInfo(name, fullName);
                pathBindList.Add(temp);
            }
            return pathBindList;
        }

        //返回上级目录按钮
        protected void Button4_Click(object sender, EventArgs e)
        {
            if(lastPath.Count == 0)
            {
                Response.Write("没有上级目录");
                return;
            }
            List<PathInfo> pathBindList = new List<PathInfo>();
            lastPath.Pop();
            if(lastPath.Count == 0)
            {
                getFilePath();
                return;
            }
            //Response.Write($"num:{lastPath.Count};path:{lastPath.Peek()}");
            pathBindList = getNewPathList(lastPath.Peek(), true);
            bindDataList(pathBindList);
        }
    }
}
