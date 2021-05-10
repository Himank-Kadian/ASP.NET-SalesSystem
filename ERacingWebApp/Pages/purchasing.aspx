<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="purchasing.aspx.cs" Inherits="ERacingWebApp.Pages.purchasing" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1"
    TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"
    runat="server">
    <div class="jumbotron text-center">
        <h1>Purchase Order
            <!-- example icon -->
            <i data-feather="circle"></i></h1>
    </div>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <asp:Label ID="Message" runat="server" /><br />
    <asp:Panel ID="VendorSelectionPanel" runat="server" Visible="true"
        CssClass="row">
        <div class="col">
            <asp:Label ID="Label2" runat="server" Text="Select Artist"></asp:Label>&nbsp;&nbsp;
            <asp:DropDownList ID="VendorDropDown" runat="server"
                DataSourceID="VendorDataSource"
                DataTextField="VendorName"
                DataValueField="VendorID"
                AppendDataBoundItems="true">
                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;
           
            <asp:ObjectDataSource ID="VendorDataSource" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="ListVendors" TypeName="ERacingSystem.BLL.PurchasingController">
            </asp:ObjectDataSource>

            <asp:LinkButton ID="SelectVendor" runat="server" OnClick="SelectVendor_Click"
                CssClass="btn btn-primary">Select</asp:LinkButton>
            <asp:LinkButton ID="PlaceOrder" runat="server" OnClick="PlaceOrder_Click"
                CssClass="btn btn-primary">Place Order</asp:LinkButton>
            <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveOrder_Click"
                CssClass="btn btn-primary">Save</asp:LinkButton>
            <asp:LinkButton ID="Delete" runat="server" OnClick="DeleteOrder_Click"
                CssClass="btn btn-primary">Delete</asp:LinkButton>
            <asp:LinkButton ID="CancelSelection" runat="server" OnClick="CancelSelection_Click"
                CssClass="btn btn-secondary">Cancel</asp:LinkButton>

        </div>

    </asp:Panel>
    <br />
    <asp:Panel ID="VendorInfoPanel" runat="server" CssClass="row">
        <div class="row">
            <div class="col-sm-3">
                <asp:Label ID="VendorNameLabel" runat="server" Text="Vendor Name"
                    AssociatedControlID="VendorName" />
            </div>
            <div class="col-sm-9">
                <asp:TextBox ID="VendorName" Text="Vendor Name" runat="server"
                    Enabled="false" CssClass="form-control" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3">
                <asp:Label ID="ContactLabel" runat="server" Text="Contact" AssociatedControlID="Contact" />
            </div>
            <div class="col-sm-9">
                <asp:TextBox ID="Contact" Text="Contact" runat="server" Enabled="false"
                    CssClass="form-control" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3">
                <asp:Label ID="PhoneLabel" runat="server" Text="Phone" AssociatedControlID="Phone" />
            </div>
            <div class="col-sm-9">
                <asp:TextBox ID="Phone" Text="Phone" runat="server" Enabled="false"
                    CssClass="form-control" />
            </div>
        </div>
    </asp:Panel>



    <br />

    <asp:Panel ID="OrderPanel" runat="server" Visible="true" CssClass="row">

        <div class="col-md-4">
            <asp:Label ID="CommentsLabel" runat="server" AssociatedControlID="Comments">Comments</asp:Label>
            <asp:TextBox ID="Comments" runat="server" CssClass="form-control"
                TextMode="MultiLine" Rows="3" />
            <asp:TextBox ID="OrderID" runat="server" CssClass="form-control" Text ="" Visible="false"/>
        </div>

        <div class="col-md-4">
            <div class="row">
                <div class="col-sm-3">
                    <asp:Label ID="SubTotalLabel" runat="server" Text="Subtotal" 
                        AssociatedControlID="SubTotal" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="SubTotal" Text="" runat="server" Enabled="false" FormatString="" 
                        CssClass="form-control" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <asp:Label ID="TaxLabel" runat="server" Text="Tax" AssociatedControlID="Tax" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="Tax" Text="" runat="server" Enabled="false" 
                        CssClass="form-control" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3">
                    <asp:Label ID="TottalLabel" runat="server" Text="Total" AssociatedControlID="Total" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="Total" Text="" runat="server" Enabled="false" 
                        CssClass="form-control" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="OrderDetailsPanel" runat="server" Visible="true"
        CssClass="row">
        <asp:GridView ID="OrderDetailsGrid" runat="server" CssClass="table table-hover table-sm"
            ItemType="ERacingSystem.VIEWMODELS.OrderDetailsViewModel"
            DataKeyNames="ProductId"
            OnRowCommand="OrderDetails_RowCommand"
            AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="Remove" runat="server"
                            CommandName="Remove_Command"
                            CommandArgument ="<%# Container.DataItemIndex %>"
                            Text="Remove"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="ProductId" runat="server" 
                            Text='<%# Item.ProductId%>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product">
                    <ItemTemplate>
                        <asp:Label ID="ItemName" runat="server" Text='<%# Bind("ItemName") %>'
                            ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Order Qty">
                    <ItemTemplate>
                        <asp:TextBox ID="Quantity" runat="server" Text='<%# Bind("Quantity") %>' 
                            TextMode="Number" step="1" min="0" ></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                            ControlToValidate="Quantity" runat="server"
                            ErrorMessage="Error: Positive Integers only"
                            ValidationExpression="^\+?[1-9]\d*$">

                        </asp:RegularExpressionValidator>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Unit Size" SortExpression="OrderUnitSize">
                    <ItemTemplate>
                        <asp:Label ID="OrderUnitSize" runat="server" Text='<%# Bind("OrderUnitSize") %>'
                            ></asp:Label>
                        <asp:Label ID="OrderUnitSizeEval" runat="server"
                            Text='<%# (int)Eval("OrderUnitSize") > 1 ? " per case" : "each"%>'></asp:Label>
                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Unit Cost">
                    <ItemTemplate>
                        <asp:TextBox ID="UnitCost" runat="server" Text='<%#decimal.Parse(string.Format("{0:0.00 }", Eval("UnitCost"))) %>' 
                            TextMode="Number" step="0.01" min="0" ></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                            ControlToValidate="UnitCost" runat="server"
                            ErrorMessage="Error: must be 0 or greater to max two decimal places"
                            ValidationExpression="^\s*(?=.*[0-9])\d*(?:\.\d{1,2})?\s*$">

                        </asp:RegularExpressionValidator>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Per-Item Cost" SortExpression="ItemCost">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("ItemCost", "{0:c}") %>'
                            ID="ItemCost"></asp:Label>
                        <asp:Label runat="server" Text=!!
                            ID="Warning"  Visible ="false"></asp:Label>
                        <asp:LinkButton ID="Refresh" runat="server"
                            CommandName="Refresh_Command"
                            CommandArgument ="<%# Container.DataItemIndex %>"
                            Text="Refresh"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Extended Cost" SortExpression="extendedCost">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("extendedCost","{0:c}" ) %>'
                            ID="ExtendedCost"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </asp:Panel>



    <div class="col">
        <asp:Repeater ID="Repeater1" runat="server" DataSourceID="ProductsByVendorCategoryDataSource"
            ItemType="ERacingSystem.VIEWMODELS.InventoryByVendorByCatagoryViewModel">
            <HeaderTemplate>
                <h3>Inventory</h3>
            </HeaderTemplate>
            <ItemTemplate>
                <h4><%# Item.Description %> </h4>
                <asp:ListView ID="VendorInventoryItems" runat="server"
                    DataSource="<%# Item.Items %>"
                    ItemType="ERacingSystem.VIEWMODELS.InventoryViewModel"
                    OnItemCommand="InventoryList_ItemCommand">
                    <ItemTemplate>
                        <tr style="background-color: #FFFFFF; color: #284775;">

                            <td>
                                <asp:LinkButton ID="AddtoOrder" runat="server"
                                    CommandName="AddToMyOrder"
                                    CommandArgument='<%# Item.ProductID %>'
                                    Text="add" CssClass="btn">
                               <%--<i class="fa fa-plus" style="color:red;"></i>--%>
                                </asp:LinkButton>
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.ItemName %>" runat="server" ID="ItemNameLabel" />
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.ReOrderLevel %>" runat="server" ID="ReOrderLabel" />
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.QuantityOnHand %>" runat="server" ID="QuantityOnHandLabel" />
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.QuantityOnOrder %>" runat="server" ID="QuantityOnOrderLabel" />
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.OrderUnitType %>" runat="server" ID="OrderUnitTypeLabel" />
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.OrderUnitSize %>" runat="server" ID="OrderUnitSizeLabel" />
                            </td>
                            <td Visible ="false">
                                <asp:Label Text="<%#Item.ProductID %>" runat="server" ID="ProductIdLabel" Visible="false"/>
                            </td>
                        </tr>

                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr style="background-color: #E0FFFF; color: #333333;">

                            <td>
                                <asp:LinkButton ID="AddtoOrder" runat="server"
                                    CommandName="AddToMyOrder"
                                    CommandArgument='<%# Item.ProductID %>'
                                    Text="add" CssClass="btn">
                               
                                </asp:LinkButton>
                            </td>

                            <td>
                                <asp:Label Text="<%#Item.ItemName %>" runat="server" ID="ItemNameLabel" />
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.ReOrderLevel %>" runat="server" ID="ReOrderLabel" />
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.QuantityOnHand %>" runat="server" ID="QuantityOnHandLabel" />
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.QuantityOnOrder %>" runat="server" ID="QuantityOnOrderLabel" />
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.OrderUnitType %>" runat="server" ID="OrderUnitTypeLabel" />
                            </td>
                            <td>
                                <asp:Label Text="<%#Item.OrderUnitSize %>" runat="server" ID="OrderUnitSizeLabel" />
                            </td>
                            <td Visible ="false">
                                <asp:Label Text="<%#Item.ProductID %>" runat="server" ID="ProductIdLabel" Visible="false"/>
                            </td>

                        </tr>

                    </AlternatingItemTemplate>
                    <LayoutTemplate>
                        <table runat="server">
                            <tr runat="server">
                                <td runat="server">
                                    <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF;
                                        border-collapse: collapse; border-color: #999999; border-style: none;
                                        border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;"
                                        border="1">
                                        <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                            <th runat="server"></th>
                                            <th runat="server">Product</th>
                                            <th runat="server">Reorder</th>
                                            <th runat="server">In Stock</th>
                                            <th runat="server">On Order</th>
                                            <th runat="server">Unit Type</th>
                                            <th runat="server">Unit Size</th>
                                            <th runat="server" visible ="false">ProductId</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder"></tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:ListView>
            </ItemTemplate>


        </asp:Repeater>


        <asp:ObjectDataSource ID="ProductsByVendorCategoryDataSource"
            runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="ListCategoryByVendor" TypeName="ERacingSystem.BLL.PurchasingController">
            <SelectParameters>
                <asp:ControlParameter ControlID="VendorDropDown" PropertyName="SelectedValue"
                    Name="vendorID" Type="Int32"></asp:ControlParameter>
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>


</asp:Content>
