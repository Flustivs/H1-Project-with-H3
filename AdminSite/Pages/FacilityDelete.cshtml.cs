using AdminSite.Controller;
using AdminSite.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class FacilityDeleteModel : PageModel
    {

        private readonly FacilityManager _facilityManager;

        public Facility FacilityToDelete { get; set; }

        public FacilityDeleteModel(FacilityManager facilityManager)
        {
            _facilityManager = facilityManager;
            FacilityToDelete = new Facility();
        }
        
        [BindProperty(SupportsGet = true)]
        public string SelectedFacility { get; set; }


        public void OnGet()
        {
            List<Facility> facilities = _facilityManager.GetAllFacilities();
            FacilityToDelete = facilities.FirstOrDefault(f => f.FacilityName == SelectedFacility);
        }
        public IActionResult OnPostDeleteFacility()
        {
            List<Facility> facilities = _facilityManager.GetAllFacilities();
            FacilityToDelete = facilities.FirstOrDefault(f => f.FacilityName == SelectedFacility);
            int facilityIDtoDelete = FacilityToDelete.FacilityID;
            _facilityManager.DeleteFacility(facilityIDtoDelete);

            return RedirectToPage("/AdminPanel");
        }

        public IActionResult OnPostCancel()
        {

            return RedirectToPage("/AdminPanel");
        }
    }
}
