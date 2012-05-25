<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Templates.ascx.cs"
    Inherits="Components_Oracle_Templates" %>
<table style="width: 100%; border: 1px solid #336699;" class="tb-header">
    <tr style="background-color:#336699;color:White;font-weight:bold;">
        <td style="white-space:nowrap;">
            Project Name</td>
        <td colspan="2">
            Connection String</td>
    </tr>
    <asp:Repeater ID="rptResult" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <b><%#Eval("ProjectName") %></b>
                </td>
                <td>
                    <%#Eval("ConnectionString") %>
                </td>
                <td>
                    </td>
            </tr>
            <tr>
            <td colspan="3"><div class="imgLine"></div></td>
            </tr>
            <tr><td colspan="3" style="text-align:right;"><input type="button" onclick="AjaxMsg('Create New Template','750','560',SerUrl,'Callback/Oracle/Template.aspx?type=2&pn=<%#Eval("ProjectName") %>&get=1','dv-content',null);"
                        value="Edit Template" class="btn3" />&nbsp;<input type="button" onclick="sld('dv-templates');window.location='Edit.aspx?pn=<%#Eval("ProjectName") %>';"
                            value="Generate Code By This Template" class="btn3" /></td></tr>
            <tr>
            <td colspan="3"><div class="line1"></div></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
