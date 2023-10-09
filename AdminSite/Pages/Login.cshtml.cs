using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace AdminSite.Pages
{
    public class LoginModel : PageModel
    {
        private readonly LogInManager _logInManager;
        public LoginModel(LogInManager logInManager)
        {
            _logInManager = logInManager;
        }

        /* [BindProperty]
         * is an attribute in ASP.NET Core Razor Pages that simplifies the process of binding form inputs to properties of the Razor Page model. 
         * It's particularly useful when dealing with form submissions, 
         * as it eliminates the need to manually retrieve values from the Request object or other sources.
         * When you decorate a property with [BindProperty],
         * it tells ASP.NET Core to automatically bind the property to form fields with matching names in the Razor Page.
         *
         * BindProperty is used to bind the InputEmail and InputPassword properties to the input fields in the form 
         */
        [BindProperty]
        public string InputEmail { get; set; }

        [BindProperty]
        public string InputPassword { get; set; }


        public void OnGet()
        {
        }
        /// <summary>
        /// OnPost is the method that's invoked when the form is submitted. 
        /// It retrieves the user from the database based on the email, 
        /// hashes the concatenated user input password and salt, 
        /// and compares it with the hashed password in the database through _logInManager,
        /// then redirect the user based on the roleID returned.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            string personID = _logInManager.HashAndRetrieveInputPassword(InputEmail, InputPassword);

            if (int.TryParse(personID, out int personIdToRetrieveRoleID))
            {
                List<int> roleIDs = _logInManager.GetRole(personIdToRetrieveRoleID);

                /*Given that a person may have multiple roles, 
                 * we want to prioritize the admin and staff roles over the customer role, 
                 * OnPost() method to check for multiple roles and redirect to the appropriate page based on the highest priority role. 
                 */
                if (roleIDs.Contains(3))
                {
                    return RedirectToPage("/AdminPanel");
                }
                else if (roleIDs.Contains(2))
                {
                    // Redirect to staff page
                    return RedirectToPage("/StaffPanel");
                }
                else if (roleIDs.Contains(1))
                {
                    // Redirect to Customer page
                    return RedirectToPage("/Customer");
                }
                else
                {
                    return RedirectToPage("/Error");
                }
            }
            else
            {
                // Handle other roles or errors as needed
                // For example, redirect to an error page
                return RedirectToPage("/Error");
            }
        }
    }
}
