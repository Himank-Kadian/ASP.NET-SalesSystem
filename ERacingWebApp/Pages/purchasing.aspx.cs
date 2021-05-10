using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.ComponentModel;
using ERacingSystem.BLL;
using ERacingSystem.DAL;
using ERacingSystem.VIEWMODELS;
using ERacingSystem.ENTITIES;
using Microsoft.Ajax.Utilities;

namespace ERacingWebApp.Pages
{
    public partial class purchasing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated && (User.IsInRole("Director") || User.IsInRole("OfficeManager")))
            {
                MessageUserControl.ShowInfo("", "You've successfully logged in");
            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }
        protected void SelectVendor_Click(object sender, EventArgs e)
        {
            if (VendorDropDown.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("", "Error: you must select a vendor");
            }
            else
            {
                
                var controller = new PurchasingController();
                var info = controller.GetVendorByID(int.Parse(VendorDropDown.SelectedValue));
                SetVendor(info.VendorName, info.Contact, info.Phone, info.VendorID);
                var order = controller.GetActiveOrder(int.Parse(VendorDropDown.SelectedValue));
                SetOrder(order.Comment, order.SubTotal, order.Tax, order.Total, order.OrderID);
                List<OrderDetailsViewModel> orderDetail = controller.GetActiveOrderDetails(int.Parse(VendorDropDown.SelectedValue));
                OrderDetailsGrid.DataSource = orderDetail;
                OrderDetailsGrid.DataBind();
            }
        }
        protected void SetVendor(string vendorName,  string contact, string phone, int vendorID)
        {
            VendorName.Text = vendorName;
            Contact.Text = contact;
            Phone.Text = phone;

        }
        protected void SetOrder(string comments, decimal subTotal, decimal tax, decimal total, int orderId)
        {
            Comments.Text = comments;
            SubTotal.Text = subTotal.ToString("0.00");
            Tax.Text = tax.ToString("0.00");
            Total.Text = total.ToString("0.00");
            OrderID.Text = orderId.ToString();

        }
        
        protected void InventoryList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "AddToMyOrder")
            {
                OrderDetailsViewModel product = GetItemFromListToAddToOrderDetails(e.Item);
                List<OrderDetailsViewModel> orderDetails = GetOrderDetailsFromGridview();
                bool itemAlreadyOnOrder = false;
                foreach (OrderDetailsViewModel item in orderDetails)
                {
                    if (item.ProductId == product.ProductId)
                    {
                        itemAlreadyOnOrder = true;
                    }
                }
                if (itemAlreadyOnOrder)
                {
                    MessageUserControl.ShowInfo("", "Error: Cannot have duplicate items in order");
                }
                else
                {
                    orderDetails.Add(product);
                    OrderDetailsGrid.DataSource = orderDetails;
                    OrderDetailsGrid.DataBind();
                    string comment = Comments.Text;
                    int orderId = int.Parse(OrderID.Text);
                    SetOrder(comment, orderDetails.Sum(x => x.ExtendedCost), orderDetails.Sum(x => x.ExtendedCost) * .05M, orderDetails.Sum(x => x.ExtendedCost) * 1.05M, orderId);
                }
                e.Handled = true;
            }
        }
        OrderDetailsViewModel GetItemFromListToAddToOrderDetails(ListViewItem item)
        {
            PurchasingController sysmgr = new PurchasingController();
            decimal unitCost = sysmgr.GetProductUnitCost(item.FindLabel("ProductIdLabel").Text.ToInt());
            var product = new OrderDetailsViewModel
            {
                ProductId = item.FindLabel("ProductIdLabel").Text.ToInt(),
                ItemName = item.FindLabel("ItemNameLabel").Text,
                Quantity = 1,
                OrderUnitSize = item.FindLabel("OrderUnitSizeLabel").Text.ToInt(),
                UnitCost = unitCost,
                ItemCost = unitCost/ item.FindLabel("OrderUnitSizeLabel").Text.ToInt(),
                ExtendedCost = unitCost * 1
            };
            return product;
        }
            
        List<OrderDetailsViewModel> GetOrderDetailsFromGridview()
        {
            var list = new List<OrderDetailsViewModel>();
            foreach (GridViewRow row in OrderDetailsGrid.Rows)
            {
                var item = new OrderDetailsViewModel
                {
                    ProductId = row.FindLabel("ProductId").Text.ToInt(),
                    ItemName = row.FindLabel("ItemName").Text,
                    Quantity = row.FindTextBox("Quantity").Text.ToInt(),
                    OrderUnitSize = row.FindLabel("OrderUnitSize").Text.ToInt(),
                    UnitCost = row.FindTextBox("UnitCost").Text.ToDecimal(),
                    ItemCost = row.FindTextBox("UnitCost").Text.ToDecimal() / row.FindLabel("OrderUnitSize").Text.ToInt(),
                    ExtendedCost = row.FindTextBox("UnitCost").Text.ToDecimal() * row.FindTextBox("Quantity").Text.ToInt(),

                };
                list.Add(item);
            }
            return list;
        }
        protected void Refresh_Command(object sender, EventArgs e)
        {
            List<OrderDetailsViewModel> orderDetails = GetOrderDetailsFromGridview();
            OrderDetailsGrid.DataSource = orderDetails;
            OrderDetailsGrid.DataBind();
            string comment = Comments.Text;
            int orderId = int.Parse(OrderID.Text);
            SetOrder(comment, orderDetails.Sum(x => x.ExtendedCost), orderDetails.Sum(x => x.ExtendedCost) * .05M, orderDetails.Sum(x => x.ExtendedCost) * 1.05M, orderId);

        }
        protected void OrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var controller = new PurchasingController();
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            List<OrderDetailsViewModel> orderDetails = GetOrderDetailsFromGridview();
            OrderDetailsViewModel orderDetail = orderDetails[rowIndex];
            if (e.CommandName == "Remove_Command")
            {
                orderDetails.Remove(orderDetail);
                OrderDetailsGrid.DataSource = orderDetails;
                OrderDetailsGrid.DataBind();
                string comment = Comments.Text;
                int orderId = int.Parse(OrderID.Text);
                SetOrder(comment, orderDetails.Sum(x => x.ExtendedCost), orderDetails.Sum(x => x.ExtendedCost) * .05M, orderDetails.Sum(x => x.ExtendedCost) * 1.05M, orderId);
            } else if(e.CommandName == "Refresh_Command")
            {
                orderDetails = GetOrderDetailsFromGridview();
                OrderDetailsGrid.DataSource = orderDetails;
                OrderDetailsGrid.DataBind();
                foreach (GridViewRow row in OrderDetailsGrid.Rows)
                {
                    if (row.FindTextBox("UnitCost").Text.ToDecimal() / row.FindLabel("OrderUnitSize").Text.ToInt() >= controller.GetProductSellingPrice(row.FindLabel("ProductId").Text.ToInt()))
                    {
                        row.FindLabel("Warning").Visible = true;
                    }
                }
                string comment = Comments.Text;
                int orderId = int.Parse(OrderID.Text);
                SetOrder(comment, orderDetails.Sum(x => x.ExtendedCost), orderDetails.Sum(x => x.ExtendedCost) * .05M, orderDetails.Sum(x => x.ExtendedCost) * 1.05M, orderId);
            }
            
        }

        protected void PlaceOrder_Click(object sender, EventArgs e)
        {
            if (int.Parse(VendorDropDown.SelectedValue) == 0)
            {
                MessageUserControl.ShowInfo("", "Error: you must select a vendor");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    PurchasingController sysmgr = new PurchasingController();
                    List<OrderDetailsViewModel> orderDetails = GetOrderDetailsFromGridview();
                    OrderDetailsGrid.DataSource = orderDetails;
                    OrderDetailsGrid.DataBind();
                    string comment = Comments.Text;
                    int orderId = int.Parse(OrderID.Text);
                    SetOrder(comment, orderDetails.Sum(x => x.ExtendedCost), orderDetails.Sum(x => x.ExtendedCost) * .05M, orderDetails.Sum(x => x.ExtendedCost) * 1.05M, orderId);
                    OrderViewModel order = new OrderViewModel
                    {
                        Comment = Comments.Text,
                        SubTotal = decimal.Parse(SubTotal.Text),
                        Tax = decimal.Parse(Tax.Text),
                        Total = decimal.Parse(Total.Text),
                        OrderID = int.Parse(OrderID.Text)
                    };
                    sysmgr.PlacePurchaseOrder(int.Parse(VendorDropDown.SelectedValue), order, orderDetails);
                }, "", "SUCCESS: Order Placed");
                VendorDropDown.SelectedIndex = 0;
                List<OrderDetailsViewModel> resetOrderDetails = new List<OrderDetailsViewModel>();
                VendorViewModel info = new VendorViewModel();
                SetVendor(info.VendorName, info.Contact, info.Phone, info.VendorID);
                OrderViewModel resetOrder = new OrderViewModel();
                SetOrder(resetOrder.Comment, resetOrder.SubTotal, resetOrder.Tax, resetOrder.Total, resetOrder.OrderID);
                OrderDetailsGrid.DataSource = resetOrderDetails;
                OrderDetailsGrid.DataBind();
            }
        }
        protected void SaveOrder_Click(object sender, EventArgs e)
        {
            PurchasingController sysmgr = new PurchasingController();
            if (int.Parse(VendorDropDown.SelectedValue) == 0)
            {
                MessageUserControl.ShowInfo("", "Error: you must select a vendor");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    
                    List<OrderDetailsViewModel> saveOrderDetails = GetOrderDetailsFromGridview();
                    OrderDetailsGrid.DataSource = saveOrderDetails;
                    OrderDetailsGrid.DataBind();
                    string comment = Comments.Text;
                    int orderId = int.Parse(OrderID.Text);
                    SetOrder(comment, saveOrderDetails.Sum(x => x.ExtendedCost), saveOrderDetails.Sum(x => x.ExtendedCost) * .05M, saveOrderDetails.Sum(x => x.ExtendedCost) * 1.05M, orderId);
                    OrderViewModel saveOrder = new OrderViewModel
                    {
                        Comment = Comments.Text,
                        SubTotal = decimal.Parse(SubTotal.Text),
                        Tax = decimal.Parse(Tax.Text),
                        Total = decimal.Parse(Total.Text),
                        OrderID = int.Parse(OrderID.Text)
                    };
                    sysmgr.SavePurchaseOrder(int.Parse(VendorDropDown.SelectedValue), saveOrder, saveOrderDetails);
                    
                }, "", "SUCCESS: Order Saved");
                var order = sysmgr.GetActiveOrder(int.Parse(VendorDropDown.SelectedValue));
                SetOrder(order.Comment, order.SubTotal, order.Tax, order.Total, order.OrderID);
                List<OrderDetailsViewModel> orderDetail = sysmgr.GetActiveOrderDetails(int.Parse(VendorDropDown.SelectedValue));
                OrderDetailsGrid.DataSource = orderDetail;
                OrderDetailsGrid.DataBind();
            }
        }
        protected void DeleteOrder_Click(object sender, EventArgs e)
        {
            if (int.Parse(VendorDropDown.SelectedValue) == 0)
            {
                MessageUserControl.ShowInfo("", "Error: you must select a vendor");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    PurchasingController sysmgr = new PurchasingController();
                    List<OrderDetailsViewModel> orderDetails = GetOrderDetailsFromGridview();
                    string comment = Comments.Text;
                    int orderId = int.Parse(OrderID.Text);
                    OrderViewModel order = new OrderViewModel
                    {
                        Comment = Comments.Text,
                        SubTotal = decimal.Parse(SubTotal.Text),
                        Tax = decimal.Parse(Tax.Text),
                        Total = decimal.Parse(Total.Text),
                        OrderID = int.Parse(OrderID.Text)
                    };
                    sysmgr.DeletePurchaseOrder(int.Parse(VendorDropDown.SelectedValue), order, orderDetails);
                }, "", "SUCCESS: Order Deleted");
                VendorDropDown.SelectedIndex = 0;
                List<OrderDetailsViewModel> resetOrderDetails = new List<OrderDetailsViewModel>();
                VendorViewModel info = new VendorViewModel();
                SetVendor(info.VendorName, info.Contact, info.Phone, info.VendorID);
                OrderViewModel resetOrder = new OrderViewModel();
                SetOrder(resetOrder.Comment, resetOrder.SubTotal, resetOrder.Tax, resetOrder.Total, resetOrder.OrderID);
                OrderDetailsGrid.DataSource = resetOrderDetails;
                OrderDetailsGrid.DataBind();
            }


        }
        protected void CancelSelection_Click(object sender, EventArgs e)
        {
            
            VendorDropDown.SelectedIndex = 0;
            List<OrderDetailsViewModel> orderDetails = new List<OrderDetailsViewModel>();
            VendorViewModel info = new VendorViewModel();
            SetVendor(info.VendorName, info.Contact, info.Phone, info.VendorID);
            OrderViewModel order = new OrderViewModel();
            SetOrder(order.Comment, order.SubTotal, order.Tax, order.Total, order.OrderID);
            OrderDetailsGrid.DataSource = orderDetails;
            OrderDetailsGrid.DataBind();
        }


    }

}