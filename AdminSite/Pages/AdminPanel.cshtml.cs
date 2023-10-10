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

<<<<<<< Updated upstream
        private PriceManager ticketManager { get; set; }
=======
        private readonly TicketManager _ticketManager;
        private readonly PriceManager _priceManager;

>>>>>>> Stashed changes
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
        public List<string> lastpurchase()
        {
            List<string> list = _ticketManager.LatestPurchases();
            return list;
		}

        public AdminPanelModel(ILogger<IndexModel> logger, PersonManager personManager, LogInManager logInManager, FacilityManager facilityManager, PriceManager priceManager, TicketManager ticketManager)
        {
            _logger = logger;
            _personmanager = personManager;
            _logInManager = logInManager;
            _facilityManager = facilityManager;
            _priceManager = priceManager;
<<<<<<< Updated upstream
=======
            _ticketManager = ticketManager;
>>>>>>> Stashed changes
        }

        // define a property SelectedStaffId to hold the selected staff member's email:
        [BindProperty]
        public string SelectedStaffEmail { get; set; }

        public void OnGet()
        {
            // Get staff and customer lists
            AllPersons = _personmanager.GetAllPersons();
            Admins = new List<Person>();
            Staff = new List<Person>();
            Customers = new List<Person>();
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

        public IActionResult OnPostEdit()
        {
            // Use _personmanager to retrieve the details of the selected staff member
            // And render an edit form
        }
        public IActionResult OnPostDelete()
        {
            // Use _personmanager to delete the selected staff member
            _personmanager.DeletePerson(SelectedStaffEmail);
        }
        public IActionResult OnPostAddNew()
        {
            // Render a form to add a new staff member
        }
    }
}
