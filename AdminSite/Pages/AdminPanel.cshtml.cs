using AdminSite.Controller;
using AdminSite.DAL;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using static System.Net.WebRequestMethods;
using System.Diagnostics.Metrics;
using System;

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

        private PriceManager ticketManager { get; set; }
        // Gets the data from each row of the database.
        public string Price1()
        {
            string price = ticketManager.TicketsConnect(1);
            return price;
        }
        public string Price2()
        {
            string price = ticketManager.TicketsConnect(2);
            return price;
        }
        public string Price3()
        {
            string price = ticketManager.TicketsConnect(3);
            return price;

        }

        public AdminPanelModel(ILogger<IndexModel> logger, PersonManager personManager, LogInManager logInManager, FacilityManager facilityManager, PriceManager priceManager)
        {
            _logger = logger;
            _personmanager = personManager;
            _logInManager = logInManager;
            _facilityManager = facilityManager;
            ticketManager = priceManager;
        }

        // define a property SelectedStaffId to hold the selected staff member's email:
        [BindProperty]
        public string SelectedStaffEmail { get; set; }

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
            // Use _personmanager to delete the selected staff member
            _personmanager.DeletePerson(SelectedStaffEmail);

            // Redirect to a specific page after deletion (for example, back to the same page)
            return RedirectToPage("/PersonDelete", new { selectedEmail = SelectedStaffEmail });
        }
        public IActionResult OnPostAddNew()
        {
            // Render a form to add a new staff member



            // Redirect to a specific page after deletion (for example, back to the same page)
            return RedirectToPage("/PersonAddNew");
        }

    }
}
