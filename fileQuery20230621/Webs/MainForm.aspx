<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="fileQuery20230621.Webs.MainForm" validateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    </head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="Label1" runat="server" Text="文件检索"></asp:Label>
        <div>
            <asp:Label ID="Label2" runat="server" Text="选则目录"></asp:Label>
            <asp:TextBox ID="txtFilePath" runat="server" ReadOnly="true"></asp:TextBox>
            <asp:Button ID="btnSelect" runat="server" Text="浏览" OnClick="Button1_Click" />
            <asp:Button ID="Button4" runat="server" Text="返回上级目录" OnClick="Button4_Click" />
        </div>
        <asp:DataList ID="DataList1" runat="server" OnItemCommand="DataList1_ItemCommand">
            <ItemTemplate>
                <asp:Label ID="Label4" runat="server" Text='<%# Eval("ShowName") %>'></asp:Label>
                <asp:Button ID="Button2" runat="server" Text="进入" CommandArgument='<%# Eval("Path") %>' CommandName="enter" />
                <asp:Button ID="Button3" runat="server" Text="选则" CommandArgument='<%# Eval("Path") %>' CommandName="chose" />
            </ItemTemplate>
        </asp:DataList>
        <div>
            <asp:Label ID="Label3" runat="server" Text="所需查找的内容"></asp:Label>
            <asp:TextBox ID="txtQueryText" runat="server"></asp:TextBox>
            <asp:Button ID="btnQuery" runat="server" Text="查找" OnClick="btnQuery_Click" />
        </div>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" Width="500px">
        </asp:CheckBoxList>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="下载" />
    </form>
</body>
</html>
