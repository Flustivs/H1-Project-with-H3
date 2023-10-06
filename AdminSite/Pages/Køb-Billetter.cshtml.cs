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

        public IActionResult OnPost()
        {
            Console.WriteLine("Clicked...");
            return Page();
        }
    }
}
