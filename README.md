# SalesSystem-ERacing

This is a project I worked on in my 3rd semester while studying at NAIT. I collaborated with 3 other developers to build a working environment for a Racing company (ERace). 
I was in charge of building the Sales Subsystem, which includes the pages Sales - In Store Purchase and Sales - Refund. Here's how they work:

## In Store Purchase
- A person brings their order to the cashier who manually adds item to the cart.
- Cashier browses through the category drop down then the item drop down. They they select the quantity of item and click add.
- The item gets added to the list and all the totals are calculated. Adding the same item multiple times simply increases its quantity in the list.
- To remove an item from the list, cashier can simply click the red X button.
- Once all the items are added, the cashier clicks the Purchase button. If the transaction is successful, a message is displayed and the Invoice ID is generated.
- Clicking the Clear Cart button clears all the fields and resets the page for a new purchase.

![image](https://user-images.githubusercontent.com/60160747/119239728-c6394180-baff-11eb-9e92-4537428b6cab.png)

## Refund
- For a refund, the customer has to bring their original receipt and item(s) to the cashier.
- The cashier enters the Invoice ID and clicked the Lookup Invoice button to display the items in that order.
- To refund an item, the customer must provide a reason which the cashier enters in the textbox.
- The cashier then clicks the checkbox on top of the reason to update the totals.
- Any restocking charges may apply. All confectionery items are non-refundable. (business rules)
- Finally, the cashier clicks the Refund button which recalcualtes the totals, generates a new Invoice ID for the refund and displays a success message.
- Clicking the Clear button clears all the fields and resets the page for a new refund.

![image](https://user-images.githubusercontent.com/60160747/119240032-11ecea80-bb02-11eb-9326-43a4f381dbae.png)

Both the purchase and refund transactions are recorded appropriately in the database.
![image](https://user-images.githubusercontent.com/60160747/119240265-575de780-bb03-11eb-9a57-251e19b492a3.png)


Here is the navigation to the source code for [Sales](https://github.com/Himank-Kadian/SalesSystem-ERacing/blob/master/ERacingWebApp/ERacingWebApp/Pages/sales.aspx.cs) page and the [Refund](https://github.com/Himank-Kadian/SalesSystem-ERacing/blob/master/ERacingWebApp/ERacingWebApp/Pages/sales_refund.aspx.cs) page.

