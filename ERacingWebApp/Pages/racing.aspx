<%@ Page MaintainScrollPositionOnPostback="true" Title="Racing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="racing.aspx.cs" Inherits="ERacingWebApp.Pages.racing1" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       
        <div class="jumbotron text-center">
            <h1>Racing</h1>
            <p>You are currently logged in as:</p>
            <asp:Label ID="Label4" runat="server" Text='<%# Bind("User.Identity.Name") %>'></asp:Label>
        </div>
    
    <div class="row">
        <div class="col-4 mx-auto">
            <asp:Calendar CssClass="table table-hover table-sm table-striped" ID="Calendar1" PrevMonthText="PREVIOUS" NextMonthText="NEXT" OnDayRender="Calendar1_DayRender" OnSelectionChanged="Calendar1_SelectionChanged" VisibleDate="Aug 31, 2019" runat="server"></asp:Calendar> 
        </div>
    
        
        <div class="mx-auto">
            <asp:GridView ID="ScheduleGridView" ItemType="ERacingSystem.VIEWMODELS.ScheduleViewModel" OnRowCommand="ScheduleGridView_RowCommand" CssClass="table table-hover table-sm" AutoGenerateColumns="false" runat="server" DataKeyNames="RaceID">
                <Columns>
                    <asp:TemplateField HeaderText="Time">
                        <EditItemTemplate>                            
                            <asp:TextBox runat="server" Text='<%# Bind("Time") %>' ID="TextBox1"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("Time", "{0:t}") %>' ID="Label1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <HeaderStyle BackColor="WindowFrame" />
                <Columns>
                    <asp:ButtonField  CommandName="Select" Text="View" ControlStyle-CssClass="btn btn-secondary btn-xs" />
                    <asp:BoundField DataField="Time" DataFormatString="{0:t}" HeaderText="Time" />
                    <asp:BoundField DataField="Competition" HeaderText="Competition" />
                    <asp:BoundField DataField="Run" HeaderText="Run" />
                    <asp:BoundField DataField="Drivers" HeaderText="Drivers" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

        <div>
            <uc1:MessageUserControl runat="server" ID="MessageUserControl1" />
            <asp:Label ID="Message" runat="server" /><br />
            <div class="col text-center">
                <asp:LinkButton ID="RecordRaceTimes" OnCommand="RecordRaceTimes_Command"  runat="server" CssClass="btn btn-success btn-xs" Visible="false">Record Race Times</asp:LinkButton>
            </div>
            <asp:GridView ID="GridView1" DataKeyNames="RaceDetailID" runat="server" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="False" CssClass="mx-auto table table-hover table-sm table-striped" DataSourceID="RosterDataSource" AllowPaging="True" ShowFooter="true">
                
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" Text="Update" CommandArgument='<%# Container.DataItemIndex %>' CommandName="CustomUpdate" CausesValidation="True" ID="EditButton"></asp:LinkButton>&nbsp;<asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="False" ID="LinkButton2"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="False" ID="LinkButton1"></asp:LinkButton>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button CssClass="btn btn-secondary btn-xs" runat="server" OnCommand="InsertButton_Command" CommandName="Insert" Text="Insert" ID="InsertButton" />
                            <asp:Button CssClass="btn btn-success btn-xs" runat="server" Text="Save" ID="SaveButton" OnCommand="SaveButton_Command" Visible="false"/>
                            <asp:Button CssClass="btn btn-secondary btn-xs" runat="server" OnCommand="CancelButton_Command" CommandName="Cancel" Text="Clear" ID="CancelButton" Visible="false" />
                        </FooterTemplate>
                        <ControlStyle CssClass="btn btn-secondary btn-xs"></ControlStyle>
                    </asp:TemplateField>

                    <asp:BoundField DataField="RaceDetailID" HeaderText="RaceDetailID" SortExpression="RaceDetailID" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="RaceID" HeaderText="RaceID" SortExpression="RaceID" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="MemberID" HeaderText="MemberID" SortExpression="MemberID" Visible="False"></asp:BoundField>
                    <asp:TemplateField HeaderText="Name" SortExpression="Name">                        
                        <EditItemTemplate>
                            <asp:HiddenField runat="server" ID="RaceID" Value='<%# Bind("RaceID") %>'/>
                            <asp:HiddenField runat="server" ID="RaceDetailID" Value='<%# Bind("RaceDetailID") %>'/>
                            <asp:Label runat="server" Text='<%# Bind("Name") %>' ID="Label1"></asp:Label>
                            <strong><asp:Label runat="server" Text=" - Comment"></asp:Label></strong>&nbsp &nbsp
                            <asp:TextBox runat="server" Text='<%# Bind("RaceDetailComment") %>' ID="RaceDetailComment"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <strong><asp:Label runat="server" Text="Choose a member" Visible="false" ID="MemberDropDownLabel"></asp:Label></strong>&nbsp &nbsp
                            <asp:DropDownList AppendDataBoundItems="true" runat="server" ID="MemberDropDown" DataTextField="MemberName" DataValueField="MemberID" Visible="false">
                                <asp:ListItem Value="0">Select a Member</asp:ListItem>
                            </asp:DropDownList>                            
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="CarID" Value='<%# Bind("CarID") %>'/>
                            <asp:HiddenField runat="server" ID="RaceID" Value='<%# Bind("RaceID") %>'/>
                            <asp:HiddenField runat="server" ID="CarClassID" Value='<%# Bind("CarClassID") %>' />                            
                            <asp:Label runat="server" Text='<%# Bind("Name") %>' ID="Label1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RaceFee" SortExpression="RaceFee">
                        <FooterTemplate>
                            <strong><asp:Label runat="server" Text="Choose a Fee" Visible="false" ID="FeeDropDownLabel"></asp:Label></strong>&nbsp &nbsp
                            <asp:DropDownList AppendDataBoundItems="true" runat="server" ID="FeeDropDown" AutoPostBack="true" OnSelectedIndexChanged="FeeDropDown_SelectedIndexChanged" DataSourceID="ObjectDataSource1" DataTextField="RaceFee" DataValueField="RaceFeeID" Visible="false">
                                <asp:ListItem Value="0">N/A</asp:ListItem>
                                <asp:ListItem Value="1">Enter a Custom Amount</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="FeeTextbox" Visible="false" TextMode="Number" runat="server"></asp:TextBox>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("RaceFee") %>' ID="RaceFeeLabel"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="RentalFee" SortExpression="RentalFee">
                        <EditItemTemplate>
                            <strong><asp:Label runat="server" Text="Select a Class"></asp:Label></strong>&nbsp &nbsp
                            <asp:DropDownList ID="CarClassesDropDown" OnSelectedIndexChanged="CarClassesDropDown_SelectedIndexChanged" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true" DataTextField="CarClassName" DataValueField="CarClassID">                                
                            </asp:DropDownList>
                        </EditItemTemplate>
                            <FooterTemplate>
                                <strong><asp:Label runat="server" Text="Choose a class" Visible="false" ID="ClassDropDownLabel"></asp:Label></strong>&nbsp &nbsp
                                <asp:DropDownList AppendDataBoundItems="true" runat="server" ID="FooterClassDropDown" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="FooterClassDropDown_SelectedIndexChanged" DataTextField="CarClassName" DataValueField="CarClassID">
                                    <asp:ListItem Value="0">Select a Class</asp:ListItem>
                                </asp:DropDownList> 
                            </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("RentalFee") %>' ID="RentalFeeLabel"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Place" SortExpression="Place">
                        <EditItemTemplate>
                            <strong><asp:Label runat="server" Text="Select a Car"></asp:Label></strong>&nbsp &nbsp
                            <asp:DropDownList ID="CarDropDown" runat="server"  CssClass="form-control" AppendDataBoundItems="true" DataTextField="SerialNumber" DataValueField="CarID">
                                <asp:ListItem Value="0">Select a Vehicle</asp:ListItem></asp:DropDownList>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <strong><asp:Label runat="server" Text="Choose a vehicle" Visible="false" ID="VehicleDropDownLabel"></asp:Label></strong>&nbsp &nbsp
                            <asp:DropDownList AppendDataBoundItems="true" runat="server" ID="SerialDropDown" Visible="false" DataTextField="SerialNumber" DataValueField="CarID" >
                                    <asp:ListItem Value="0">Select a Car</asp:ListItem>
                            </asp:DropDownList>
                        </FooterTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("Place") %>' ID="Label3"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Refund" SortExpression="Refund">
                        <EditItemTemplate>
                            <asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="RefundCheckBox_CheckedChanged" Checked='<%# Bind("Refund") %>' ID="RefundCheckBox"></asp:CheckBox>
                            <asp:TextBox runat="server" Text='<%# Bind("RefundReason") %>' ID="RefundTextBox" Visible="false"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" Checked='<%# Bind("Refund") %>' Enabled="false" ID="CheckBox1"></asp:CheckBox>
                        </ItemTemplate>
                    </asp:TemplateField>                   

                    <asp:BoundField DataField="RaceDetailComment" HeaderText="RaceDetailComment" SortExpression="RaceDetailComment" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="RefundReason" HeaderText="RefundReason" SortExpression="RefundReason" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="CarClassID" HeaderText="CarClassID" SortExpression="CarClassID" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="CarClassName" HeaderText="CarClassName" SortExpression="CarClassName" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="SerialNumber" HeaderText="SerialNumber" SortExpression="SerialNumber" Visible="False"></asp:BoundField>

                </Columns>                
            </asp:GridView>
            </div>

            <div>
            <uc1:MessageUserControl runat="server" ID="MessageUserControl2" />
            <asp:Label ID="Label2" runat="server" /><br />
                <div class="col text-center">
                    <asp:LinkButton CssClass="text-center btn btn-success" ID="UpdateResults" Visible="false" OnCommand="UpdateResults_Command" runat="server">Save Results</asp:LinkButton>
                </div>                
            <asp:GridView CssClass="table table-hover table-sm table-striped mx-auto" ID="ResultsGridView" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="Name" SortExpression="Name">
                        <ItemTemplate>
                            <asp:HiddenField ID="PenaltyID" runat="server" Value='<%# Bind("PenaltyID") %>'/>
                            <asp:HiddenField ID="RaceDetailID" runat="server" Value='<%# Bind("RaceDetailID") %>'/>
                            <asp:Label runat="server" Text='<%# Bind("Name") %>' ID="NameLabel"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RunTime" SortExpression="RunTime">
                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Eval("RunTime") %>' ID="RunTimeTextBox"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Runtime must be in the format 00:00:00" ControlToValidate="RunTimeTextBox" 
                                ValidationExpression="^([0-9]{1}|(?:0[0-9]|1[0-9]|2[0-3])+):([0-5]?[0-9])(?::([0-5]?[0-9])(?:\.(\d{1,9}))?)?$" Display="Dynamic"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Penalties" SortExpression="Penalties">
                        <ItemTemplate>
                            <asp:DropDownList ID="PenaltyDropDownList" runat="server"  CssClass="form-control" AppendDataBoundItems="true" DataTextField="Description" DataValueField="PenaltyID">
                                <asp:ListItem Value="0">None</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>                

                    <asp:TemplateField HeaderText="Place" SortExpression="Place">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("Place") %>' ID="PlaceLabel"></asp:Label>
                        </ItemTemplate>                       
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>        
    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Fees_List" TypeName="ERacingSystem.BLL.RacingController"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="RosterDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Roster_List" TypeName="ERacingSystem.BLL.RacingController" DataObjectTypeName="ERacingSystem.VIEWMODELS.RosterViewModel" InsertMethod="Add_Racer" UpdateMethod="Edit_Roster">
            <SelectParameters>
                <asp:ControlParameter ControlID="ScheduleGridView" PropertyName="SelectedValue" DefaultValue="0" Name="raceId" Type="Int32"></asp:ControlParameter>
            </SelectParameters>
        </asp:ObjectDataSource>
    
</asp:Content>
