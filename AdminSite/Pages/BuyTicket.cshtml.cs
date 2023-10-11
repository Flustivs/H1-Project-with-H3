using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AdminSite.Pages
{
    /// <summary>
    /// In this class we get the price of the different types 
    /// </summary>
    public class BuyTicketModel : PageModel
    {

        private readonly PriceManager price;
        private readonly OrderManager order;

        public BuyTicketModel(PriceManager price, OrderManager order)
        {
            this.price = price;
            this.order = order;
        }

        public string Price1()
        {
            string ticketPrice = price.TicketsConnect(1);
            return ticketPrice;
        }
        public string Price2()
        {
            string ticketPrice = price.TicketsConnect(2);
            return ticketPrice;
        }
        public string Price3()
        {
            string ticketPrice = price.TicketsConnect(3);
            return ticketPrice;
        }

        // Here we bind the property to the (TicketAmount get set)
        [BindProperty]
        public int TicketAmount { get; set; }

        // This method is an onpost method meaning it only reacts on the post on the webpage in this example its when the Submit button is pressed
        // it then tells us this in the console
        public IActionResult OnPost()
        {
            Console.WriteLine("This many tickets: " +  TicketAmount + "\nThis is your orderID: " + order.OrderConnect());
            return RedirectToPage();
        }
    }
}
