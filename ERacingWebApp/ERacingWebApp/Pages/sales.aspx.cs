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

//Sales employee credentials
//ID: JCalder 
//PW: Pa$$w0rd

namespace ERacingWebApp.Pages
{
    public partial class sales : System.Web.UI.Page
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

        protected void CategoryDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductDropDown.Items.Clear();
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (QuantityTextBox.Text.Length < 1 || CategoryDropDown.SelectedIndex < 1)
            {
                MessageUserControl.ShowInfo("", "Please fill the necessary product details");
            }
            else
            {
                if (QuantityTextBox.Text.ToInt() < 1)
                {
                    MessageUserControl.ShowInfo("", "Please enter a minimum of 1 quantity for the item.");
                }

                else 
                {
                    InvoiceDetailsViewModel item = GetProductFromHiddenGridView();
                    List<InvoiceDetailsViewModel> invoiceItems = GetInvoiceItemsFromGridView();
                    bool existingItem = false;
                    foreach (InvoiceDetailsViewModel member in invoiceItems)
                    {
                        if (item.ProductID == member.ProductID)
                        {
                            member.Quantity = member.Quantity + item.Quantity;
                            member.Amount = member.Amount + item.Amount;
                            existingItem = true;
                        }
                    }
                    if (!existingItem)
                        invoiceItems.Insert(0, item);

                    InvoiceItems.DataSource = invoiceItems;
                    InvoiceItems.DataBind();
                    CalculateTotals();
                }
            }
        }

        protected void InvoiceItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            List<InvoiceDetailsViewModel> invoiceProducts = GetInvoiceItemsFromGridView();
            InvoiceDetailsViewModel invoiceItem = invoiceProducts[rowIndex];
            if (e.CommandName == "DeleteFromInvoice")
            {
                invoiceProducts.Remove(invoiceItem);
            }
            InvoiceItems.DataSource = invoiceProducts;
            InvoiceItems.DataBind();
            CalculateTotals();
        }

        protected void Quantity_TextChanged(object sender, EventArgs e)
        {
            List<InvoiceDetailsViewModel> invoiceProducts = GetInvoiceItemsFromGridView();
            foreach (var row in invoiceProducts)
            {
                if (row.Quantity < 1)
                {
                    row.Quantity = 1;
                    MessageUserControl.ShowInfo("","Quantity must be at least 1.");
                }
                if (row.Quantity * row.Price != row.Amount)
                {
                    row.Amount = (decimal)(row.Quantity * row.Price);
                }
            }
            InvoiceItems.DataSource = invoiceProducts;
            InvoiceItems.DataBind();
            CalculateTotals();
        }

        protected void PurchaseButton_Click(object sender, EventArgs e)
        {
            if (InvoiceItems.Rows.Count < 1)
            {
                MessageUserControl.ShowInfo("", "Please enter products to purchase");
            }
            else
            {
                InvoiceViewModel currentInvoice = new InvoiceViewModel
                {
                    InvoiceDate = DateTime.Now,
                    Subtotal = SubtotalAmount.Text.ToDecimal(),
                    GST = TaxAmount.Text.ToDecimal(),
                    Total = TotalAmount.Text.ToDecimal(),
                    Products = GetInvoiceItemsFromGridView()
                };
                MessageUserControl.TryRun(() =>
                {
                    SalesController sysmgr = new SalesController();
                    int inoivceId = sysmgr.Purchase(currentInvoice, User.Identity.Name);
                    InvoiceId.Text = "Invoice ID: " +inoivceId.ToString();
                }, "", "Success: Purchase complete. Press Clear Cart to start a new purchase."
                );

                CategoryDropDown.Enabled = false;
                ProductDropDown.Enabled = false;
                QuantityTextBox.Enabled = false;
                InvoiceItems.Enabled = false;
                AddButton.Enabled = false;
                PurchaseButton.Enabled = false;
            }
        }

        protected void ClearCartButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        InvoiceDetailsViewModel GetProductFromHiddenGridView()
        {
            GridViewRow row = SelectedProductGridView.Rows[0];
            InvoiceDetailsViewModel item = new InvoiceDetailsViewModel
            {
                ProductID = row.FindLabel("ProductId").Text.ToInt(),
                ItemName = row.FindLabel("ItemName").Text,
                Price = row.FindLabel("ItemPrice").Text.ToDecimal(),
                Quantity = QuantityTextBox.Text.ToInt(),
                Amount = row.FindLabel("ItemPrice").Text.ToDecimal() * QuantityTextBox.Text.ToInt()
            };
            return item;
        }
        List<InvoiceDetailsViewModel> GetInvoiceItemsFromGridView()
        {
            var list = new List<InvoiceDetailsViewModel>();
            foreach (GridViewRow row in InvoiceItems.Rows)
            {
                if (row.FindTextBox("Quantity").Text.Length < 1)
                {
                    MessageUserControl.ShowInfo("","Quantity can't be empty.");
                    row.FindTextBox("Quantity").Text = "1";
                }
                var item = new InvoiceDetailsViewModel
                {
                    ProductID = row.FindHiddenField("ProductId").Value.ToInt(),
                    ItemName = row.FindLabel("ItemName").Text,
                    Price = row.FindLabel("ItemPrice").Text.ToDecimal(),
                    Quantity = row.FindTextBox("Quantity").Text.ToInt(),
                    Amount = row.FindLabel("Amount").Text.ToDecimal()
                };
                list.Add(item);
            }
            return list;
        }

        void CalculateTotals()
        {
            decimal amount = 0;
            foreach (GridViewRow row in InvoiceItems.Rows)
            {
                amount += row.FindLabel("Amount").Text.ToDecimal();
            }
            SubtotalAmount.Text = amount.ToString();
            TaxAmount.Text = (SubtotalAmount.Text.ToDecimal() * (decimal)0.05).ToString();
            TotalAmount.Text = (SubtotalAmount.Text.ToDecimal() + TaxAmount.Text.ToDecimal()).ToString();
        }

        protected void Unnamed_Click()
        {

        }
    }
    #region Web Extensions
    public static class WebControlExtensions
    {
        public static Label FindLabel(this Control self, string id)
            => self.FindControl(id) as Label;
        public static TextBox FindTextBox(this Control self, string id)
            => self.FindControl(id) as TextBox;
        public static HiddenField FindHiddenField(this Control self, string id)
            => self.FindControl(id) as HiddenField;
        public static CheckBox FindCheckBox(this Control self, string id)
            => self.FindControl(id) as CheckBox;
        public static int ToInt(this string self) => int.Parse(self);
        public static decimal ToDecimal(this string self) => decimal.Parse(self);
    }
    #endregion
}
