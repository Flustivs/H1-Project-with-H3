using AdminSite.Controller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class FacilityAddNewModel : PageModel
    {
    private readonly FacilityManager _facilityManager;
public FacilityAddNewModel(FacilityManager facilityManager)
{
    _facilityManager = facilityManager;
}

[BindProperty]
public string NewName { get; set; }
[BindProperty]
public string NewURL { get; set; }

[BindProperty]
public DateTime NewDate { get; set; }

        public void OnGet()
        {
        }

         public IActionResult OnPostSaveChanges()
 {
     _facilityManager.AddFacility(NewName, NewURL, NewDate);

     return RedirectToPage("/AdminPanel");
 }

 public IActionResult OnPostCancel()
 {

     return RedirectToPage("/AdminPanel");
 }
    }
}
