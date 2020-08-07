<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductIdSelector.ascx.cs" Inherits="UCommerce.Kentico.CMSFormControls.Selectors.ProductIdSelector" %>

<div style="width: 356px;">
    <form method="POST">
        <asp:TextBox ID="SearchTerm" runat="server" placeholder="Start typing to filter" CssClass="form-control input-width-82" onkeypress="if(window.event.keyCode == 13) { this.parentElement.querySelector('input[type=submit]').click(); window.event.stopPropagation();}" />
        <asp:Button runat="server" ID="SearchForProduct" type="submit" Text="Search" OnClick="SearchForProduct_OnClick" CssClass="btn btn-default" />
    </form>
    <asp:ListBox runat="server" ID="SearchResult" Style="max-width: 100%;margin-top: 20px;" CssClass="form-control" />
</div>
