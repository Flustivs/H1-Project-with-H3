using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class AboutUsModel : PageModel
    {
        private readonly AboutController _aboutController;
        public AboutUsModel(AboutController aboutController)
        {
            _aboutController = aboutController;
        }

        public string GetDanish1()
        {
            string value = _aboutController.Connect(1);
            return value;
        }
        public string GetDanish2()
        {
            string value = _aboutController.Connect(2);
            return value;
        }
        public string GetDanish3()
        {
            string value = _aboutController.Connect(3);
            return value;
        }
        public string GetEnglish1()
        {
            string value = _aboutController.Connect(4);
            return value;
        }
        public string GetEnglish2()
        {
            string value = _aboutController.Connect(5);
            return value;
        }
        public string GetEnglish3()
        {
            string value = _aboutController.Connect(6);
            return value;
        }
    }
}
