using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class AboutUsModel : PageModel
    {
        Controller.AboutController About = new Controller.AboutController();
        public string GetDanish1()
        {
            string value = About.connect(1);
            return value;
        }
        public string GetDanish2()
        {
            string value = About.connect(2);
            return value;
        }
        public string GetDanish3()
        {
            string value = About.connect(3);
            return value;
        }
        public string GetEnglish1()
        {
            string value = About.connect(4);
            return value;
        }
        public string GetEnglish2()
        {
            string value = About.connect(5);
            return value;
        }
        public string GetEnglish3()
        {
            string value = About.connect(6);
            return value;
        }
    }
}
