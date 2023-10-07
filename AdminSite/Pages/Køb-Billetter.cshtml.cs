using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace AdminSite.Pages
{
    public class Køb_BilletterModel : PageModel
    {
        PriceManager ticket = new PriceManager();
        public string Price1()
        {
            string price = ticket.TicketsConnect(1);
            return price;
        }
        public string Price2()
        {
            string price = ticket.TicketsConnect(2);
            return price;
        }
        public string Price3()
        {
            string price = ticket.TicketsConnect(3);
            return price;
        }

        [BindProperty(Name = "TicketAmount")]
        public int TicketAmount { get; set; }

        public IActionResult OnPost()
        {
            // You can access TicketAmount here and process it as needed
            // For demonstration, let's just write it to the console
            System.Console.WriteLine("Ticket Amount: " + TicketAmount);
            return RedirectToPage();
        }
    }
}
