using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

#region Additional Namespaces
using ERacingSystem.BLL;
using ERacingSystem.VIEWMODELS;
#endregion

namespace ERacingWebApp.Pages
{
    public partial class racing1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated && (User.IsInRole("RaceCoordinator") || User.IsInRole("Administrators")))
            {
                MessageUserControl1.ShowInfo(User.Identity.Name, "You've successfully logged in");
            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            Label4.Text = User.Identity.Name;


        }

        #region Calendar Selection Changed
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            var controller = new RacingController();
            var schedule = controller.Schedules_List(Calendar1.SelectedDate);
            ScheduleGridView.DataSource = schedule;
            ScheduleGridView.DataBind();

            Close_Results();

            RecordRaceTimes.Visible = false;
            GridView1.Visible = false;
        }
        #endregion

        #region Calendar Render
        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            var controller = new RacingController();
            List<DateTime> dates = controller.DateList();
            foreach (DateTime date in dates)
            {
                if (e.Day.Date == date.Date)
                {
                    e.Cell.BorderWidth = 2;
                    e.Cell.Font.Size = 14;
                    e.Cell.Font.Bold = true;
                    e.Cell.BorderColor = System.Drawing.Color.RoyalBlue;
                    e.Cell.BackColor = System.Drawing.Color.LightGreen;
                }
            }
        }
        #endregion

        #region ScheduleGridView RowCommand
        protected void ScheduleGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                int index = Convert.ToInt32(e.CommandArgument);                
                                
                var controller = new RacingController();
                var schedule = controller.Schedules_List(Calendar1.SelectedDate);

                var roster = controller.Roster_List(schedule[index].RaceID);
                var raceId = roster[0].RaceID.ToString();

                RosterDataSource.SelectParameters.Clear();
                RosterDataSource.SelectParameters.Add(new Parameter("raceId", TypeCode.Int32, raceId));
                RosterDataSource.DataBind();

                Close_Results();

                RecordRaceTimes.Visible = true;
                GridView1.EditIndex = -1;
                GridView1.Visible = true;              

            }
        }
        #endregion

        #region DropDowns
        protected void CarClassesDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenField raceId = null;
            foreach (GridViewRow row in GridView1.Rows)
            {
                raceId = (HiddenField)row.FindControl("RaceID");
            }

            DropDownList CarClassDropDown = sender as DropDownList;
            ListItem select = new ListItem("Select a car", "0");

            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList carList = (DropDownList)gvr.FindControl("CarDropDown");

            var controller = new RacingController();
            var cars = controller.Cars_List(int.Parse(CarClassDropDown.SelectedValue), int.Parse(raceId.Value), 0);

            if (carList != null)
            {
                carList.Items.Clear();
                carList.DataSource = cars;
                carList.Items.Add(select);
                carList.DataBind();
            }
        }

        protected void FooterClassDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenField raceId = null;
            foreach (GridViewRow row in GridView1.Rows)
            {
                raceId = (HiddenField)row.FindControl("RaceID");
            }

            DropDownList CarClassDropDown = sender as DropDownList;
            var controller = new RacingController();
            var cars = controller.Cars_List(int.Parse(CarClassDropDown.SelectedValue), int.Parse(raceId.Value), 0);

            DropDownList carDropDown = (DropDownList)GridView1.FooterRow.FindControl("SerialDropDown");
            carDropDown.DataSource = cars;
            carDropDown.Visible = true;
            carDropDown.DataBind();
        }

        protected void FeeDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox feeTextBox = (TextBox)GridView1.FooterRow.FindControl("FeeTextBox");
            DropDownList feeDropDownList = (DropDownList)GridView1.FooterRow.FindControl("FeeDropDown");
            if (feeDropDownList.SelectedIndex == 1)
            {
                feeTextBox.Visible = true;
            }
        }
        #endregion

        #region Roster Grid RowCommands

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CustomUpdate")
            {
                string error = string.Empty;
                var controller = new RacingController();
                int index = Convert.ToInt32(e.CommandArgument);                
                HiddenField raceId = (HiddenField)GridView1.Rows[index].FindControl("RaceID");
                TextBox comment = (TextBox)GridView1.Rows[index].FindControl("RaceDetailComment");
                DropDownList car = (DropDownList)GridView1.Rows[index].FindControl("CarDropDown");
                HiddenField raceDetailID = (HiddenField)GridView1.Rows[index].FindControl("RaceDetailID");
                CheckBox refund = (CheckBox)GridView1.Rows[index].FindControl("RefundCheckBox");
                TextBox refundReason = (TextBox)GridView1.Rows[index].FindControl("RefundTextBox");

                RosterViewModel roster = new RosterViewModel()
                {
                    CarID = int.Parse(car.SelectedValue),
                    RaceDetailComment = comment.Text,
                    RefundReason = refundReason.Text,
                    Refund = refund.Checked,
                    RaceDetailID = int.Parse(raceDetailID.Value)
                };

                if (refund.Checked && refundReason.Text.Trim().Length == 0)
                {
                    error = error + " A reason must be provided for all refunds";
                }

                if (error.Trim().Length == 0)
                {
                    try
                    {
                        controller.Edit_Roster(roster);
                    } catch (Exception ex)
                    {
                        MessageUserControl1.ShowInfo(ex.Message, "Failed to update details");
                    }
                    MessageUserControl1.ShowInfo("Success", "RaceDetails Updated");
                    GridView1.EditIndex = -1;
                    GridView1.DataBind();
                } else
                {
                    MessageUserControl1.ShowInfo("Error", error);
                }

            }
        }

        #endregion

        #region Roster GridView RowDataBound

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RosterViewModel raceData = (RosterViewModel)e.Row.DataItem;
            var controller = new RacingController();
            if (raceData != null)
            {
                if (raceData.RaceDetailID == 0)
                {
                    LinkButton editButton = (LinkButton)e.Row.FindControl("LinkButton1");
                    CheckBox refund = (CheckBox)e.Row.FindControl("CheckBox1");
                    Label raceFee = (Label)e.Row.FindControl("RaceFeeLabel");
                    Label rentalFee = (Label)e.Row.FindControl("RentalFeeLabel");
                    refund.Visible = false;
                    raceFee.Visible = false;
                    rentalFee.Visible = false;
                    editButton.Visible = false;
                }
            }

            #region RowDataBound Edit
            if (!IsPostBack)
            {
                GridView1.Visible = false;
            }
            if ((e.Row.RowState & DataControlRowState.Edit) > 0 )
            {            
                var classes = controller.CarClasses_List(raceData.RaceID);

                DropDownList carClassesDropDown = (DropDownList)e.Row.FindControl("CarClassesDropDown");
                carClassesDropDown.DataSource = classes;
                if (raceData.CarID != null)
                {
                    carClassesDropDown.SelectedValue = raceData.CarClassID.ToString();
                } else
                {
                    string select = "Select a car class";
                    carClassesDropDown.Items.Insert(0, select);
                }
                try
                {
                    carClassesDropDown.DataBind();
                }
                catch (Exception ex)
                {
                    string output = string.Empty;
                    foreach (var singleClass in classes)
                    {
                        output = output + singleClass.CarClassName;
                    }

                    MessageUserControl1.ShowInfo(ex.Message, 
                    ($"Currently Registered vehicle - '{raceData.CarClassName}' does not meet the class requirements of this race - '{output}'"));
                }

                if (raceData.CarID != null)
                {
                    List<CarViewModel> cars = new List<CarViewModel>();
                    DropDownList carDropDown = (DropDownList)e.Row.FindControl("CarDropDown");
                    cars = controller.Cars_List(int.Parse(carClassesDropDown.SelectedValue), raceData.RaceID, (int)raceData.CarID);
                    carDropDown.DataSource = cars;
                    if (raceData.CarID != null)
                    {
                        carDropDown.SelectedValue = raceData.CarID.ToString();
                    }
                    carDropDown.DataBind();
                }

                TextBox refundReason = (TextBox)e.Row.FindControl("RefundTextBox") as TextBox;
                if (refundReason.Text.Trim().Length > 0 )
                {
                    refundReason.Visible = true;
                }
                Close_Results();                
            }
            #endregion
        }
        #endregion

        #region Close Results GridView
        public void Close_Results()
        {
            UpdateResults.Visible = false;
            ResultsGridView.DataSource = null;
            ResultsGridView.DataBind();
        }
        #endregion

        #region Refund Checkbox
        protected void RefundCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TextBox refundReason = null;
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (refundReason == null)
                    {
                        refundReason = row.FindControl("RefundTextBox") as TextBox;
                        if (refundReason != null)
                        {
                            refundReason.Visible = !refundReason.Visible;
                            if (refundReason.Visible == false)
                            {
                                refundReason.Text = null;
                            }
                        }
                    }
                }
            }            
        }
        #endregion

        #region Insert Button Command
        protected void InsertButton_Command(object sender, CommandEventArgs e)
        {
            var controller = new RacingController();          
            HiddenField raceId = null;
            foreach (GridViewRow row in GridView1.Rows)
            {                
                    raceId = (HiddenField)row.FindControl("RaceID");              
            }

            DropDownList memberDropDown = (DropDownList)GridView1.FooterRow.FindControl("MemberDropDown");
            DropDownList feeDropDown = (DropDownList)GridView1.FooterRow.FindControl("FeeDropDown");
            DropDownList classDropDown = (DropDownList)GridView1.FooterRow.FindControl("FooterClassDropDown");

            HideFooter();

            var classes = controller.CarClasses_List(int.Parse(raceId.Value));            
            var members = controller.Members_List(int.Parse(raceId.Value));

            classDropDown.DataSource = classes;
            memberDropDown.DataSource = members;

            feeDropDown.SelectedIndex = 0;

            memberDropDown.Visible = true;
            classDropDown.Visible = true;
            feeDropDown.Visible = true;

            Close_Results();

            classDropDown.DataBind();
            memberDropDown.DataBind();
            GridView1.EditIndex = -1;
        }
        #endregion

        #region Footer Management
        protected void HideFooter()
        {
            Button insertButton = (Button)GridView1.FooterRow.FindControl("InsertButton");
            Button cancelButton = (Button)GridView1.FooterRow.FindControl("CancelButton");
            Button saveButton = (Button)GridView1.FooterRow.FindControl("SaveButton");

            saveButton.Visible = !saveButton.Visible;
            insertButton.Visible = !insertButton.Visible;
            cancelButton.Visible = !cancelButton.Visible;

            Label memberDropDownLabel = (Label)GridView1.FooterRow.FindControl("MemberDropDownLabel");
            Label feeDropDownLabel = (Label)GridView1.FooterRow.FindControl("FeeDropDownLabel");
            Label classDropDownLabel = (Label)GridView1.FooterRow.FindControl("ClassDropDownLabel");
            Label vehicleDropDownLabel = (Label)GridView1.FooterRow.FindControl("VehicleDropDownLabel");

            memberDropDownLabel.Visible = !memberDropDownLabel.Visible;
            feeDropDownLabel.Visible = !feeDropDownLabel.Visible;
            classDropDownLabel.Visible = !classDropDownLabel.Visible;
            vehicleDropDownLabel.Visible = !vehicleDropDownLabel.Visible;
        }

        protected void CloseFooter()
        {
            DropDownList memberDropDown = (DropDownList)GridView1.FooterRow.FindControl("MemberDropDown");
            DropDownList feeDropDown = (DropDownList)GridView1.FooterRow.FindControl("FeeDropDown");
            DropDownList classDropDown = (DropDownList)GridView1.FooterRow.FindControl("FooterClassDropDown");
            TextBox feeTextBox = (TextBox)GridView1.FooterRow.FindControl("FeeTextBox");

            HideFooter();

            memberDropDown.Visible = false;
            feeDropDown.Visible = false;
            classDropDown.Visible = false;
            feeTextBox.Visible = false;

            memberDropDown.DataSource = null;
            classDropDown.DataSource = null;
            feeDropDown.DataSource = null;

            feeDropDown.DataBind();
            classDropDown.DataBind();
            memberDropDown.DataBind();
        }

        protected void CancelButton_Command(object sender, CommandEventArgs e)
        {
            CloseFooter();
        }

        #endregion

        #region Add Racer
        protected void SaveButton_Command(object sender, CommandEventArgs e)
        {
            string error = string.Empty;
            var controller = new RacingController();
            HiddenField raceId = null;
            foreach (GridViewRow row in GridView1.Rows)
            {
                raceId = (HiddenField)row.FindControl("RaceID");
            }
            DropDownList memberDropDown = (DropDownList)GridView1.FooterRow.FindControl("MemberDropDown");
            DropDownList feeDropDown = (DropDownList)GridView1.FooterRow.FindControl("FeeDropDown");
            DropDownList carDropDown = (DropDownList)GridView1.FooterRow.FindControl("SerialDropDown");

            decimal raceFee;

            if (feeDropDown.SelectedIndex == 1)
            {
                TextBox feeTextBox = (TextBox)GridView1.FooterRow.FindControl("FeeTextBox");
                raceFee = int.Parse(feeTextBox.Text);
            } else if (feeDropDown.SelectedIndex == 0)
            {
                raceFee = 0;
            } else
            {
                raceFee = decimal.Parse(feeDropDown.SelectedItem.Text);
            }

            RosterViewModel roster = new RosterViewModel()
            {
                MemberID = int.Parse(memberDropDown.SelectedValue),
                CarID = int.Parse(carDropDown.SelectedValue),
                RaceID = int.Parse(raceId.Value),
                RaceFee = raceFee               
            };
            var racerCount = controller.Racer_Count(roster);
            InvoiceViewModel invoice = null;

            if (memberDropDown.SelectedIndex == 0)
            {
                error = error + " You must select a member when adding a new racer.";
            }

            if (raceFee == 0)
            {
                error = error + " You must enter a race fee in order to add a new racer.";
            }

            if (racerCount.Current > racerCount.Max)
            {
                error = error + " Race is already full.  Cannot register additional racers.";
            }

            if (error.Trim().Length == 0)
            {
                try
                {
                   invoice =  controller.Add_Racer(roster, User.Identity.Name);
                } catch (Exception ex)
                {
                    MessageUserControl1.ShowInfo(ex.Message, "Unable to save racer");
                }

                var updateSchedule = controller.Schedules_List(Calendar1.SelectedDate);

                RosterDataSource.SelectParameters.Clear();
                RosterDataSource.SelectParameters.Add(new Parameter("raceId", TypeCode.Int32, raceId.Value));
                RosterDataSource.DataBind();

                ScheduleGridView.DataSource = updateSchedule;
                ScheduleGridView.DataBind();

                MessageUserControl1.ShowInfo("Success", ($"Racer Added to roster.  Invoice #{invoice.InvoiceID} created by {User.Identity.Name}.  Subtotal - {invoice.Subtotal:C2}, GST - {invoice.GST:C2}, Total - {invoice.Total:C2}"));
            } else 
            {
                MessageUserControl1.ShowInfo("Failure", error);
            }
            CloseFooter();
        }
        #endregion

        #region Load Race Times

        protected void RecordRaceTimes_Command(object sender, CommandEventArgs e)
        {
            LoadRaceTime();
        }

        public void LoadRaceTime()
        {
            var controller = new RacingController();
            var raceID = ScheduleGridView.SelectedValue;
            var results = controller.Results_List(Convert.ToInt32(raceID));

            var penalties = controller.Penalties_List();
            UpdateResults.Visible = true;


            ResultsGridView.DataSource = results;
            ResultsGridView.DataBind();
            foreach (GridViewRow row in ResultsGridView.Rows)
            {
                DropDownList penaltiesDropDown = (DropDownList)row.FindControl("PenaltyDropDownList");
                HiddenField penaltyID = (HiddenField)row.FindControl("PenaltyID");
                Label placeLabel = (Label)row.FindControl("PlaceLabel");

                if (placeLabel.Text != "")
                {
                    string newLabel = ConvertIntsToStrings(int.Parse(placeLabel.Text));
                    placeLabel.Text = newLabel;
                }

                penaltiesDropDown.DataSource = penalties;

                if (penaltyID.Value != "")
                {
                    penaltiesDropDown.SelectedValue = penaltyID.Value;
                }
                penaltiesDropDown.DataBind();
            }
        }

        #endregion

        #region Convert Place ints to String Values
        public string ConvertIntsToStrings(int number)
        {
            string textNumber = string.Empty;
            switch (number)
            {
                case 1:
                    textNumber = "First";
                    break;
                case 2:
                    textNumber = "Second";
                    break;
                case 3:
                    textNumber = "Third";
                    break;
                case 4:
                    textNumber = "Fourth";
                    break;
                case 5:
                    textNumber = "Fifth";
                    break;
                case 6:
                    textNumber = "Sixth";
                    break;
                case 7:
                    textNumber = "Seventh";
                    break;
                case 8:
                    textNumber = "Eighth";
                    break;
                case 9:
                    textNumber = "Ninth";
                    break;
                case 10:
                    textNumber = "Tenth";
                    break;
                default:
                    textNumber = "";
                    break;
            }
            return textNumber;
        }
        #endregion

        #region UpdateRaceResults
        protected void UpdateResults_Command(object sender, CommandEventArgs e)
        {
            var raceID = ScheduleGridView.SelectedValue;
            string error = string.Empty;
            List<ResultsViewModel> results = new List<ResultsViewModel>();
            ResultsViewModel singleResult = null;
            foreach (GridViewRow row in ResultsGridView.Rows)
            {


                DropDownList penalties = (DropDownList)row.FindControl("PenaltyDropDownList");
                TextBox runTimeText = (TextBox)row.FindControl("RunTimeTextBox");
                Label name = (Label)row.FindControl("NameLabel");
                if (runTimeText.Text.Trim().Length < 1 && int.Parse(penalties.SelectedValue) != 4)
                {
                    error = error + $"Cannot enter null values for race times unless vehicle has been scratched - {name.Text}";
                }

                TimeSpan? runTime = null;
                if (runTimeText.Text == "")
                {
                    runTime = null;
                } else
                {
                    runTime = TimeSpan.Parse(runTimeText.Text);
                }

                HiddenField raceDetailID = (HiddenField)row.FindControl("RaceDetailID");
                singleResult = new ResultsViewModel()
                {
                    Name = name.Text,
                    RaceDetailID = int.Parse(raceDetailID.Value),
                    RaceID = Convert.ToInt32(raceID),
                    RunTime = runTime,
                    PenaltyID = int.Parse(penalties.SelectedValue)
                };
                results.Add(singleResult);
            }
            var controller = new RacingController();
            if (error.Trim().Length > 0)
            {
                MessageUserControl2.ShowInfo("Error", error);
            } else
            {
                controller.Edit_Results(results);
                LoadRaceTime();
            }
        }
        #endregion
    }
}