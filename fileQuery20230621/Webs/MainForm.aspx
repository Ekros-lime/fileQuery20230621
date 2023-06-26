<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="fileQuery20230621.Webs.MainForm" validateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtFilePath" runat="server"></asp:TextBox>
            <asp:Button ID="btnSelect" runat="server" Text="浏览" OnClick="Button1_Click" />
        </div>
        <div>
            <asp:TextBox ID="txtQueryText" runat="server"></asp:TextBox>
            <asp:Button ID="btnQuery" runat="server" Text="查找" OnClick="btnQuery_Click" />
        </div>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" Width="500px">
        </asp:CheckBoxList>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="下载" />
    </form>
</body>
</html>
