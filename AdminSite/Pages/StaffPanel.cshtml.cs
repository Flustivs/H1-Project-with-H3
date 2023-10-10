using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class StaffPanelModel : PageModel
    {
        private readonly PriceManager priceManager;
        private readonly TicketManager ticketManager;
        public StaffPanelModel(PriceManager price, TicketManager ticket)
        {
            priceManager = price;
            ticketManager = ticket;
        }
        public string Price1()
        {
            string price = priceManager.TicketsConnect(1);
            return price;
        }
        public string Price2()
        {
            string price = priceManager.TicketsConnect(2);
            return price;
        }
        public string Price3()
        {
            string price = priceManager.TicketsConnect(3);
            return price;

        }
        public List<string> LastPurchase()
        {
            List<string> list = ticketManager.LatestPurchases();
            return list;
        }
    }
}
