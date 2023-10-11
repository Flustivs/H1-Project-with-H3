using AdminSite.Controller;
using AdminSite.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminSite.Pages
{
    public class FacilityEditModel : PageModel
    {
        private readonly FacilityManager _facilityManager;

        public Facility FacilityToEdit { get; set; }

        public FacilityEditModel(FacilityManager facilityManager)
        {
            _facilityManager = facilityManager;
            FacilityToEdit = new Facility();
        }

        [BindProperty(SupportsGet = true)]
        public string SelectedFacility { get; set; }

        [BindProperty]
        public string NewName { get; set; }
        [BindProperty]
        public string NewURL { get; set; }

        [BindProperty]
        public DateTime NewDate { get; set; }

        public void OnGet()
        {
            List<Facility> facilities = _facilityManager.GetAllFacilities();
            FacilityToEdit = facilities.FirstOrDefault(f => f.FacilityName == SelectedFacility) ?? new Facility();
        }

        public IActionResult OnPostSaveChanges()
        {
            int facilityIDtoEdit = FacilityToEdit.FacilityID;
            _facilityManager.DeleteFacility(facilityIDtoEdit);

            _facilityManager.EditFacility(facilityIDtoEdit, NewName, NewURL, NewDate);

            return RedirectToPage("/AdminPanel");
        }

        public IActionResult OnPostCancel()
        {

            return RedirectToPage("/AdminPanel");
        }
    }
}
