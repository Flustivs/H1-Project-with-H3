using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class AboutUsModel : PageModel
    {
        Controller.AboutController About = new Controller.AboutController();
        public string GetDanish1()
        {
            string value = About.Connect(1);
            return value;
        }
        public string GetDanish2()
        {
            string value = About.Connect(2);
            return value;
        }
        public string GetDanish3()
        {
            string value = About.Connect(3);
            return value;
        }
        public string GetEnglish1()
        {
            string value = About.Connect(4);
            return value;
        }
        public string GetEnglish2()
        {
            string value = About.Connect(5);
            return value;
        }
        public string GetEnglish3()
        {
            string value = About.Connect(6);
            return value;
        }
    }
}
