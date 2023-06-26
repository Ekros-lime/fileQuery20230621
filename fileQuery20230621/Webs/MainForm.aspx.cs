using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using MSW = Microsoft.Office.Interop.Word;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace fileQuery20230621.Webs
{
    public partial class MainForm : System.Web.UI.Page
    {
        //用于存储要搜索的文件类型
        private string[] fileTypes = { "*.txt", "*.doc", "*.docx", "*.pdf" };
        //用于存储搜索到符合类型的文件路径
        private List<string> pathList = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        //浏览按钮
        protected void Button1_Click(object sender, EventArgs e)
        {
            //选择目标文件并获取路径
            Thread t = new Thread((ThreadStart)(() =>
            {
                getFilePath();
            }
            ));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
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

        //获取所选文件路径
        string getFilePath()
        {
            string path = "";

            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = fbd.SelectedPath;
                txtFilePath.Text = path;
            }

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
                        if (line.Contains(txtQueryText.Text))
                        {
                            isGetText = true;
                            boxInfo += $"{lineNum} {line}<br>";
                        }
                    }
                }
                else if(s.Substring(s.Length -4,4) == ".doc" || s.Substring(s.Length - 5, 5) == "docx")
                {

                }

                boxInfo = boxInfo.Replace(txtQueryText.Text, $"<span style=\"color:red\">{txtQueryText.Text}</span>");
                boxInfo = titleTxt + boxInfo;
                //文件中查找到对应的字符串
                if (isGetText)
                {
                    checkBoxInfoList.Add(new QueryInfo(boxInfo));
                }
            }

            return checkBoxInfoList;
        }
        //绑定数据
        void bindCheckListBox(List<QueryInfo> info)
        {
            CheckBoxList1.DataSource = info;
            CheckBoxList1.DataTextField = "Info";
            CheckBoxList1.DataValueField = "Info";
            CheckBoxList1.DataBind();
        }
    }
}