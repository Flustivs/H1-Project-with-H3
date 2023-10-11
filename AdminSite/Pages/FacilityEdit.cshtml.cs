using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class FacilityEditModel : PageModel
    {
        public void OnGet()
        {
            List<Facility> facilities = _facilityManager.GetAllFacilities();
            FacilityToEdit = facilities.FirstOrDefault(f => f.FacilityName == SelectedFacility) ;
        }

        public IActionResult OnPostSaveChanges()
        {
            List<Facility> facilities = _facilityManager.GetAllFacilities();
            FacilityToEdit = facilities.FirstOrDefault(f => f.FacilityName == SelectedFacility);
            int facilityIDtoEdit = FacilityToEdit.FacilityID;

            _facilityManager.EditFacility(facilityIDtoEdit, NewName, NewURL, NewDate);

            return RedirectToPage("/AdminPanel");
        }

        public IActionResult OnPostCancel()
        {

            return RedirectToPage("/AdminPanel");
        }
    }
}
