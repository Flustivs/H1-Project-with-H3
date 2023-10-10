using AdminSite.Controller;
using AdminSite.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class PersonEditModel : PageModel
    {
        private readonly PersonManager _personmanager;
        // private readonly FacilityManager _facilityManager;
        public Person PersonToEdit { get; set; }

        // Property to hold the list of persons
        //public List<Person> AllPersons { get; set; }
        //public List<Person> Admins { get; set; }
        //public List<Person> Staff { get; set; }
        //public List<Person> Customers { get; set; }
        //public List<Facility> AllFacilities { get; set; }

        public PersonEditModel(PersonManager personManager)
        {
            //_logger = logger;
            _personmanager = personManager;
            PersonToEdit = new Person();
        }

        /// <summary>
        /// Receive the query parameter from AdminPanelModel
        /// and assign it to a property in EditPersonModel. 
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SelectedEmail { get; set; }

        [BindProperty]
        public string NewName { get; set; }
        [BindProperty]
        public string NewEmail { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }



        public void OnGet()
        {
            /*This code tries to retrieve the person with the selected email. 
             * If it can't find this person (i.e., _personmanager.RetrievePerson(SelectedEmail) returns null), 
             * it initializes PersonToEdit as a new Person object.
             * However, this will only work if the Person class has a parameterless constructor. 
             * If it doesn't, you may need to provide some default values when creating the new Person object.*/
             PersonToEdit = _personmanager.RetrievePerson(SelectedEmail) ?? new Person(); 
        }
        
        public IActionResult OnPostSaveChanges()
        {
            _personmanager.EditPerson(SelectedEmail, NewEmail, NewName, NewPassword);
            return RedirectToPage("/AdminPanel");
        }

        public IActionResult OnPostCancel()
        {
            
            return RedirectToPage("/AdminPanel");
        }
    }
}
