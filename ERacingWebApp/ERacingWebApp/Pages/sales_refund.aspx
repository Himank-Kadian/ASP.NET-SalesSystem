<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="sales_refund.aspx.cs" Inherits="ERacingWebApp.Pages.sales_refund" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row">
        <h1>Refunds</h1>
    </div>
    <div class="row">
        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    </div>
    <div class="row">
        <asp:TextBox ID="OriginalInvoiceValue" runat="server" TextMode="Number" Text="Original Invoice #" ></asp:TextBox>&nbsp;&nbsp;
        <asp:LinkButton ID="LookupInvoiceButton" runat="server" CssClass="btn btn-primary" OnClick="LookupInvoiceButton_Click">
            Lookup Invoice
            </asp:LinkButton>&nbsp;&nbsp;
        <asp:LinkButton ID="ClearButton" runat="server" CssClass="btn btn-info" OnClick="ClearButton_Click">Clear</asp:LinkButton>
    </div>
    <br /> <br />
    <div class="row">
        <asp:GridView ID="RefundInvoiceItems" runat="server" BorderStyle="Solid" GridLines="Vertical" CssClass="table table-striped" ItemType="ERacingSystem.VIEWMODELS.InvoiceDetailsViewModel" AutoGenerateColumns="False" EmptyDataText="No items selected" Width="600px" DataSourceID="InvoiceODS1">
            <Columns>
                <asp:TemplateField HeaderText="Product">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="ProductId" Value='<%# Item.ProductID%>'/>
                        <asp:HiddenField runat="server" ID="Category" Value='<%# Item.Category%>' />
                        <asp:Label runat="server" ID="ItemName" Text='<%# Item.ItemName%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="Quantity" Text='<%# Item.Quantity%>'></asp:Label>
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
                <asp:TemplateField HeaderText="Restock Chg">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="RestockCharge" Text='<%# Item.RestockCharge%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reason">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="ReasonProvided" OnCheckedChanged="ReasonProvided_CheckedChanged" AutoPostBack="true"/>
                        <asp:TextBox runat="server" ID="Reason"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br /> <br />
    <div class="row">
        <div class="col-md-4">
            <asp:LinkButton ID="RefundButton" runat="server" CssClass="btn btn-success" OnCommand="RefundButton_Command" CommandName="RefundClick">Refund</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
           <asp:TextBox ID="RefundInvoiceNumber" runat="server" Enabled="false"></asp:TextBox>
        </div> 
        <div class="col-md-8">
            <asp:Label ID="Label4" runat="server" Text="Subtotal: "></asp:Label>
            <asp:TextBox ID="SubtotalAmount" runat="server" Enabled="false"></asp:TextBox><br />
            <asp:Label ID="Label5" runat="server" Text="Tax: "></asp:Label>
            <asp:TextBox ID="TaxAmount" runat="server" Enabled="false"></asp:TextBox><br />
            <asp:Label ID="Label6" runat="server" Text="Total: "></asp:Label>
            <asp:TextBox ID="TotalAmount" runat="server" Enabled="false"></asp:TextBox>
        </div>  
        <br /> <br />
    </div>
        <div class="col-sm-10">
        <asp:Panel ID="QueryPanel" runat="server" EnableViewState="true">
            <asp:Label runat="server" ID="SearchArg"></asp:Label>
        </asp:Panel>
    </div>
    <div class="row">
        <asp:ObjectDataSource ID="InvoiceODS1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Invoice_List" TypeName="ERacingSystem.BLL.SalesController">
            <SelectParameters>
                <asp:ControlParameter ControlID="SearchArg" PropertyName="Text" Name="invoiceId" Type="Int32"></asp:ControlParameter>
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
