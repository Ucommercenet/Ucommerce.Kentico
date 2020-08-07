<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../../../CMSMasterPages/ui/simplePage.master" CodeBehind="DashboardWrapper.aspx.cs" theme="default" Inherits="UCommerce.Kentico.CMSPages.DashboardWrapper" %>
<%@ Register TagPrefix="uc" Namespace="UCommerce.Web.Shell.Web.Controls" Assembly="UCommerce.Web.Shell" %>

<asp:Content ID="uCommercePlaceHolder" ContentPlaceHolderID="plcBeforeBody" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>	
</asp:Content>
<asp:Content ID="ucommerceContent" ContentPlaceHolderID="plcContent" runat="server">
    <uc:ClientDependencyShell runat="server" id="ClientDependency" />
    <asp:PlaceHolder runat="server" ID="JavaScriptPlaceHolder"></asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="CssPlaceHolder"></asp:PlaceHolder>
    <script src="App/json3.js"></script>
    <script src="Scripts/yepnope/yepnope.1.5.4-min.js"></script>
    <script src="App/loader.js"></script>
    
    <iframe id="ucommerceIFrame" src="../Vue/dashboard.html" style="width: 100%; height: 100%; border: 0"></iframe>
</asp:Content>