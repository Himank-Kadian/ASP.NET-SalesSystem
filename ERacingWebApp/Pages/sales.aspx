<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="sales.aspx.cs" Inherits="ERacingWebApp.Pages.sales" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <h1>In Store Purchases</h1><br />
    </div>
    <div class="row">
        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    </div>
    <div class="row">
        <asp:DropDownList ID="CategoryDropDown" runat="server" DataSourceID="CategoryODS" DataTextField="Description" DataValueField="CategoryId" AppendDataBoundItems="true" AutoPostBack="true" Width="150" OnSelectedIndexChanged="CategoryDropDown_SelectedIndexChanged">
            <asp:ListItem Value="0">Select a Category</asp:ListItem>
        </asp:DropDownList>&nbsp;&nbsp;
        <asp:DropDownList ID="ProductDropDown" runat="server" DataSourceID="ProductODS" DataTextField="ItemName" DataValueField="ProductID" AppendDataBoundItems="true" Width="150" AutoPostBack="true">
            <asp:ListItem Value="-1">Select a Product</asp:ListItem>
        </asp:DropDownList>&nbsp;&nbsp;
        <asp:TextBox ID="QuantityTextBox" runat="server" Width="50" TextMode="Number"></asp:TextBox>&nbsp;&nbsp;
        <asp:LinkButton ID="AddButton" runat="server" CssClass="btn btn-primary" OnClick="AddButton_Click">
            Add
        </asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
        <asp:Label runat="server" ID="InvoiceId"></asp:Label>
    </div>
    <br />
    <br />
    <div class="row">
        <asp:GridView ID="InvoiceItems" runat="server" BorderStyle="Solid" GridLines="Vertical" CssClass="table table-striped" ItemType="ERacingSystem.VIEWMODELS.InvoiceDetailsViewModel" AutoGenerateColumns="False" EmptyDataText="No items selected" Width="600px" OnRowCommand="InvoiceItems_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Product">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="ProductId" Value='<%# Item.ProductID%>'></asp:HiddenField>
                        <asp:Label runat="server" ID="ItemName" Text='<%# Item.ItemName%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="Quantity" Text='<%# Item.Quantity%>' OnTextChanged="Quantity_TextChanged" AutoPostBack="true" TextMode="Number"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Price">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="ItemPrice" Text='<%# Item.Price%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="Amount" Text='<%# Item.Amount%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField CommandName="DeleteFromInvoice" ControlStyle-CssClass="btn btn-danger btn-sm" Text="X"/>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="col-md-4">
            <asp:LinkButton ID="PurchaseButton" runat="server" CssClass="btn btn-success" OnClick="PurchaseButton_Click">Purchase</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="ClearCartButton" runat="server" CssClass="btn btn-info" OnClick="ClearCartButton_Click">Clear Cart</asp:LinkButton>
        </div>
        <div class="col-md-8">
            <asp:Label ID="Label4" runat="server" Text="Subtotal: "></asp:Label>
            <asp:TextBox ID="SubtotalAmount" runat="server" Enabled="false"></asp:TextBox><br />
            <asp:Label ID="Label5" runat="server" Text="Tax: "></asp:Label>
            <asp:TextBox ID="TaxAmount" runat="server" Enabled="false"></asp:TextBox><br />
            <asp:Label ID="Label6" runat="server" Text="Total: "></asp:Label>
            <asp:TextBox ID="TotalAmount" runat="server" Enabled="false"></asp:TextBox>
        </div>
        <br />
        <br />
    </div>
    <div class="col-sm-10">
        <asp:Panel ID="QueryPanel" runat="server" EnableViewState="true">
            <asp:GridView ID="SelectedProductGridView" runat="server" BorderStyle="Solid" GridLines="Horizontal" CssClass="table table-striped" ItemType="ERacingSystem.VIEWMODELS.InvoiceDetailsViewModel" AutoGenerateColumns="False" DataSourceID="ProductListItem">
            <Columns>
                <asp:TemplateField HeaderText="ProductId">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="ProductId" Text='<%# Item.ProductID%>' Visible="true"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ItemName" ItemStyle-Width="300">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="ItemName" Text='<%# Item.ItemName%>' Visible="true"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ItemPrice">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="ItemPrice" Text='<%# Item.Price%>' Visible="true"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="QuantityOnHand">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="QuantityOnHand" Text='<%# Item.QuantityOnHand%>' Visible="true"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RestockCharge">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="RestockCharge" Text='<%# Item.RestockCharge%>' Visible="true"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </asp:Panel>
    </div>

    <asp:ObjectDataSource ID="CategoryODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Category_List" TypeName="ERacingSystem.BLL.SalesController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ProductODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Product_List" TypeName="ERacingSystem.BLL.SalesController">
        <SelectParameters>
            <asp:ControlParameter ControlID="CategoryDropDown" PropertyName="SelectedValue" Name="categoryId" Type="Int32" DefaultValue="0"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ProductListItem" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Product_Selected" TypeName="ERacingSystem.BLL.SalesController">
        <SelectParameters>
            <asp:ControlParameter ControlID="ProductDropDown" PropertyName="SelectedValue" Name="productId" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

