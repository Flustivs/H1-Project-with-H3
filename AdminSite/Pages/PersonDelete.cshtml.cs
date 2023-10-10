using AdminSite.Controller;
using AdminSite.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class PersonDeleteModel : PageModel
    {
        private readonly PersonManager _personmanager;

        public Person PersonToDelete { get; set; }

        public PersonDeleteModel(PersonManager personManager) 
        {
            _personmanager = personManager;
            PersonToDelete = new Person();
        }
        [BindProperty(SupportsGet = true)]
        public string SelectedEmail { get; set; }

        public void OnGet()
        {
            PersonToDelete = _personmanager.RetrievePerson(SelectedEmail) ?? new Person();
        }

        public IActionResult OnPostConfirm()
        {
            _personmanager.DeletePerson(SelectedEmail);
            return RedirectToPage("/AdminPanel");
        }

        public IActionResult OnPostCancel()
        {

            return RedirectToPage("/AdminPanel");
        }
    }
}
