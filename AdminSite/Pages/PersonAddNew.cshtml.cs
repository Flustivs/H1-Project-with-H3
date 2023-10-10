using AdminSite.Controller;
using AdminSite.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class PersonAddNewModel : PageModel
    {
        private readonly PersonManager _personmanager;
        public PersonAddNewModel(PersonManager personManager)
        {
            _personmanager = personManager;
        }

        [BindProperty]
        public string NewName { get; set; }
        [BindProperty]
        public string NewEmail { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostSaveChanges()
        {
            _personmanager.AddPerson(NewEmail, NewName, NewPassword);
            return RedirectToPage("/AdminPanel");
        }

        public IActionResult OnPostCancel()
        {

            return RedirectToPage("/AdminPanel");
        }

    }
}
