using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AdminSite.Pages
{
    /// <summary>
    /// In this class we get the price of the different types 
    /// </summary>
    public class Køb_BilletterModel : PageModel
    {

        private readonly PriceManager price;
        private readonly OrderManager order;

        public Køb_BilletterModel(PriceManager price, OrderManager order)
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

        [BindProperty(Name = "TicketAmount")]
        public int TicketAmount { get; set; }

        public IActionResult OnPost()
        {
            Console.WriteLine("This many tickets: " +  TicketAmount + "\nThis is your orderID: " + order.OrderConnect());
            return RedirectToPage("/BuyTicket");
        }
    }
}
