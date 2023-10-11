using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    /// <summary>
    /// This class is for the menu where you can change your prices of the passes
    /// </summary>
    public class PriceChangerModel : PageModel
    {
        private readonly PriceManager _priceManager;
        [BindProperty]
        public int dayPass { get; set; }
        [BindProperty]
        public int monthPass { get; set; }
        [BindProperty]
        public int yearPass { get; set; }
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
        // This is an onpost method meaning that it will only be runned when for example our submit button
        public IActionResult OnPost()
        {
            _priceManager.UpdatePrice(dayPass, monthPass, yearPass);
            return RedirectToPage("/AdminPanel");
        }
    }
}
