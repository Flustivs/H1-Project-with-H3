using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class PriceChangerModel : PageModel
    {
        private readonly PriceManager _priceManager;
        public PriceChangerModel(PriceManager priceManager)
        {
            _priceManager = priceManager;
        }
        public string Price1()
        {
            string price = _priceManager.TicketsConnect(1);
            return price;
        }
        public string Price2()
        {
            string price = _priceManager.TicketsConnect(2);
            return price;
        }
        public string Price3()
        {
            string price = _priceManager.TicketsConnect(3);
            return price;

        }
        public IActionResult OnPost()
        {
            return Page();
        }
    }
}
