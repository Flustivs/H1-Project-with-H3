using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class LoginModel : PageModel
    {
        private readonly LogInManager _logInManager;
        public LoginModel(LogInManager logInManager)
        {
            _logInManager = logInManager;
        }

        //BindProperty is used to bind the InputEmail and InputPassword properties to the input fields in the for
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
                int roleID = _logInManager.GetRole(personIdToRetrieveRoleID);


                if (roleID == 1)
                {
                    return RedirectToPage("/Customer");
                }
                else if (roleID == 2)
                {
                    // Redirect to staff page
                    return RedirectToPage("/StaffPanel");
                }
                else if (roleID == 3)
                {
                    // Redirect to admin page
                    return RedirectToPage("/AdminPanel");
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
