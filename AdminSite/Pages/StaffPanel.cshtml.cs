using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class StaffPanelModel : PageModel
    {
        private readonly PriceManager ticketPrice;
        public StaffPanelModel(PriceManager price)
        {
            this.ticketPrice = price;
        }
        public string Price1()
        {
            string price = ticketPrice.TicketsConnect(1);
            return price;
        }
        public string Price2()
        {
            string price = ticketPrice.TicketsConnect(2);
            return price;
        }
        public string Price3()
        {
            string price = ticketPrice.TicketsConnect(3);
            return price;

        }
    }
}
