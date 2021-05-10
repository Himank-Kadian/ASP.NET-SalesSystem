using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#region Additional Namespaces
using System.ComponentModel;
using ERacingSystem.BLL;
using ERacingSystem.DAL;
using ERacingSystem.VIEWMODELS;
using ERacingSystem.ENTITIES;
using Microsoft.Ajax.Utilities;
#endregion

namespace ERacingWebApp.Pages
{
    public partial class sales_refund : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated && User.IsInRole("Clerk") || User.IsInRole("Administrators"))
            {
                MessageUserControl.ShowInfo("", "Login Successful");
            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            QueryPanel.Style.Add("display", "none");
        }

        protected void LookupInvoiceButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(OriginalInvoiceValue.Text))
                MessageUserControl.ShowInfo("", "Error: Must enter original invoice number");
            else
                SearchArg.Text = OriginalInvoiceValue.Text;
        }

        protected void ReasonProvided_CheckedChanged(object sender, EventArgs e)
        {
            SubtotalAmount.Text = "0";
            foreach (GridViewRow row in RefundInvoiceItems.Rows)
            {
                var item = new InvoiceDetailsViewModel
                {
                    ProductID = row.FindHiddenField("ProductId").Value.ToInt(),
                    Category = row.FindHiddenField("Category").Value,
                    ItemName = row.FindLabel("ItemName").Text,
                    Price = row.FindLabel("ItemPrice").Text.ToDecimal(),
                    Quantity = row.FindLabel("Quantity").Text.ToInt(),
                    Amount = row.FindLabel("Amount").Text.ToDecimal(),
                    RestockCharge = row.FindLabel("RestockCharge").Text.ToDecimal(),
                    RefundReason = row.FindTextBox("Reason").Text
                };
                if (item.Category.Equals("Confectionary"))
                {
                    MessageUserControl.ShowInfo("", "Confectionary items are non refundable");
                    row.FindCheckBox("ReasonProvided").Visible = false;
                    row.FindTextBox("Reason").Visible = false;
                    row.FindLabel("RestockCharge").Visible = false;
                }
                else
                {
                    if (row.FindCheckBox("ReasonProvided").Checked && !row.FindTextBox("Reason").Text.IsNullOrWhiteSpace())
                    {
                        SubtotalAmount.Text = (SubtotalAmount.Text.ToDecimal() + item.Amount - item.RestockCharge).ToString();
                    }
                }
            }
            TaxAmount.Text = (SubtotalAmount.Text.ToDecimal() * (decimal)0.05).ToString();
            TotalAmount.Text = (SubtotalAmount.Text.ToDecimal() + TaxAmount.Text.ToDecimal()).ToString();
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
        protected void RefundButton_Command(object sender, CommandEventArgs e)
        {
            var list = new List<InvoiceDetailsViewModel>();
            SubtotalAmount.Text = 0.ToString();
            foreach (GridViewRow row in RefundInvoiceItems.Rows)
            {
                var item = new InvoiceDetailsViewModel
                {
                    ProductID = row.FindHiddenField("ProductId").Value.ToInt(),
                    Category = row.FindHiddenField("Category").Value,
                    ItemName = row.FindLabel("ItemName").Text,
                    Price = row.FindLabel("ItemPrice").Text.ToDecimal(),
                    Quantity = row.FindLabel("Quantity").Text.ToInt(),
                    Amount = row.FindLabel("Amount").Text.ToDecimal(),
                    RestockCharge = row.FindLabel("RestockCharge").Text.ToDecimal(),
                    RefundReason = row.FindTextBox("Reason").Text
                };
                if (row.FindCheckBox("ReasonProvided").Checked && !row.FindTextBox("Reason").Text.IsNullOrWhiteSpace())
                {
                    SubtotalAmount.Text = (SubtotalAmount.Text.ToDecimal() + item.Amount - item.RestockCharge).ToString();
                    list.Add(item);
                }
            }
            if (list.Count < 1)
            {
                MessageUserControl.ShowInfo("", "Please enter the reason and check the check box for items you want to return.");
            }
            else
            {
                TaxAmount.Text = (SubtotalAmount.Text.ToDecimal() * (decimal)0.05).ToString();
                TotalAmount.Text = (SubtotalAmount.Text.ToDecimal() + TaxAmount.Text.ToDecimal()).ToString();
                InvoiceViewModel currentInvoice = new InvoiceViewModel
                {
                    InvoiceID = OriginalInvoiceValue.Text.ToInt(),
                    InvoiceDate = DateTime.Now,
                    Subtotal = SubtotalAmount.Text.ToDecimal() * -1,
                    GST = TaxAmount.Text.ToDecimal() * -1,
                    Total = TotalAmount.Text.ToDecimal() * -1,
                    Products = list
                };
                MessageUserControl.TryRun(() =>
                {
                    SalesController sysmgr = new SalesController();
                    int refundInvoiceId = sysmgr.Refund(currentInvoice, User.Identity.Name);
                    RefundInvoiceNumber.Text = refundInvoiceId.ToString();
                }, "", "Success: Refund complete. Press Clear to start a new refund."
                );

                RefundInvoiceItems.Enabled = false;
                OriginalInvoiceValue.Enabled = false;
                LookupInvoiceButton.Enabled = false;
                RefundButton.Enabled = false;
            }
        }
    }
}