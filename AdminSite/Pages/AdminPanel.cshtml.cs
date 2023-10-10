using AdminSite.Controller;
using AdminSite.DAL;
using Azure.Core;
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
        private readonly PriceManager _priceManager;

        // Property to hold the list of persons
        public List<Person> AllPersons { get; set; }
        public List<Person> Admins { get; set; }
        public List<Person> Staff { get; set; }
        public List<Person> Customers { get; set; }
        public List<Facility> AllFacilities { get; set; }

		private readonly TicketManager _ticketManager;
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
            _ticketManager = ticketManager;
        }

        // define a property SelectedStaffId to hold the selected staff member's email:
        [BindProperty]
        public string SelectedStaffEmail { get; set; }
        [BindProperty]
        public string SelectedFacilityID { get; set; }
        [BindProperty(Name = "TicketAmount")]
        public int TicketAmount { get; set; }

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


        public IActionResult OnPostPriceEdit()
        {
            return RedirectToPage("/PriceChanger");
        }

        public IActionResult OnPostEdit()
        {
            // Redirect to the EditPerson page with the selected email as a query parameter
            return RedirectToPage("/PersonEdit", new { SelectedEmail = SelectedStaffEmail });
        }
        /// <summary>
        /// In C#, methods with a return type, such as IActionResult, 
        /// are expected to return a value of that type after their execution. 
        /// This is essential for the flow of your application, especially in web applications built with ASP.NET Core.
        /// 
        /// In your case, the OnPostDelete method is defined with a return type of IActionResult.
        /// In ASP.NET Core Razor Pages, the methods like OnPostDelete are used 
        /// to handle HTTP POST requests initiated by user interactions (e.g., clicking a button). 
        /// 
        /// After processing the request(in this case, deleting a person), 
        /// you typically want to redirect the user to another page  
        /// or provide some response to indicate the success or failure of the operation.
        /// </summary>
        /// <returns></returns>
        
        public IActionResult OnPostDelete()
        {
            // Redirect to a specific page after deletion (for example, back to the same page)
            return RedirectToPage("/PersonDelete", new { SelectedEmail = SelectedStaffEmail });
        }

        public IActionResult OnPostAddNew()
        {
            // Redirect to a specific page after deletion (for example, back to the same page)
            return RedirectToPage("/PersonAddNew");
        }


        public IActionResult OnPostEditF()
        {
            // Redirect to the EditPerson page with the selected email as a query parameter
            return RedirectToPage("/FacilityEdit", new { SelectedFacility = SelectedFacilityID });
        }

        public IActionResult OnPostDeleteF()
        {
            // Redirect to a specific page after deletion (for example, back to the same page)
            return RedirectToPage("/FacilityDelete", new { SelectedFacility = SelectedFacilityID });
        }

        public IActionResult OnPostAddNewF()
        {
            // Redirect to a specific page after deletion (for example, back to the same page)
            return RedirectToPage("/FacilityAddNew");
        }

    }
}
