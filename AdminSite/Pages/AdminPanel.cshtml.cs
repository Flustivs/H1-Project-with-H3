using AdminSite.Controller;

using AdminSite.DAL;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class AdminPanelModel : PageModel
    {
 
        private readonly ILogger<IndexModel> _logger;
        private readonly PersonManager _personmanager;
        private readonly LogInManager _logInManager;
        private readonly FacilityManager _facilityManager;


        // Property to hold the list of persons
        public List<Person> AllPersons { get; set; }
        public List<Person> Admins { get; set; } 
        public List<Person> Staff { get; set; }
        public List<Person> Customers { get; set; }
        public List<Facility> AllFacilities { get; set; }

        public AdminPanelModel(ILogger<IndexModel> logger, PersonManager personManager, LogInManager logInManager, FacilityManager facilityManager)
        {
            _logger = logger;
            _personmanager = personManager;
            _logInManager = logInManager;
            _facilityManager = facilityManager;
        }

        public void OnGet()
        {
            // Get staff and customer lists
            AllPersons = _personmanager.GetAllPersons();
            Admins = new List<Person>();
            Staff = new List<Person>();
            Customers= new List<Person>();
            foreach (Person person in AllPersons)
            {
                List<int> roleIDs = _logInManager.GetRole(person.PersonID);
                if (roleIDs.Contains(3))
                {
                    Admins.Add(person);
                }
                else if (roleIDs.Contains(2))
                {
                    Staff.Add(person);
                }
                else
                {
                    Customers.Add(person);
                }
            }

            // Get AllFacilities
            AllFacilities = _facilityManager.GetAllFacilities();

        PriceManager ticket = new PriceManager();
        public string Price1()
        {
            string price = ticket.TicketsConnect(1);
            return price;
        }
        public string Price2()
        {
            string price = ticket.TicketsConnect(2);
            return price;
        }
        public string Price3()
        {
            string price = ticket.TicketsConnect(3);
            return price;

        }
    }
}
