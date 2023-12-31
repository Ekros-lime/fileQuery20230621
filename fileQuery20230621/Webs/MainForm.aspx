﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="fileQuery20230621.Webs.MainForm" validateRequest="false" %>

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
            <p>-----------------------</p>
            <table>
                <tr>
                    <td><asp:Label ID="Label2" runat="server" Text="选则目录"></asp:Label></td>
                    <td><asp:TextBox ID="txtFilePath" runat="server" ReadOnly="true"></asp:TextBox></td>
                    <td><asp:Button ID="btnSelect" runat="server" Text="浏览" OnClick="Button1_Click" /></td>
                    <td><asp:Button ID="Button4" runat="server" Text="返回上级目录" OnClick="Button4_Click" /></td>
                </tr>
            </table>  
        </div>
        <asp:DataList ID="DataList1" runat="server" OnItemCommand="DataList1_ItemCommand">
            <ItemTemplate>
                <p>-----------------------</p>
               <table>
                    <tr>
                        <td> <asp:Label ID="Label4" runat="server" Text='<%# Eval("ShowName") %>'></asp:Label></td>
                        <td><asp:Button ID="Button2" runat="server" Text="进入" CommandArgument='<%# Eval("Path") %>' CommandName="enter" /></td>
                        <td><asp:Button ID="Button3" runat="server" Text="选则" CommandArgument='<%# Eval("Path") %>' CommandName="chose" /></td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>
        <div>
            <p>-----------------------</p>
           <table>
                <tr>
                    <td><asp:Label ID="Label3" runat="server" Text="所需查找的内容"></asp:Label></td>
                    <td><asp:TextBox ID="txtQueryText" runat="server"></asp:TextBox></td>
                    <td><asp:Button ID="btnQuery" runat="server" Text="查找" OnClick="btnQuery_Click" /></td>
                </tr>
            </table> 
        </div>
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" Width="500px">
        </asp:CheckBoxList>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="下载" />
    </form>
</body>
</html>
