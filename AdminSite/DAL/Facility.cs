namespace AdminSite.DAL
{
    public class Facility
    {
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string FacilityPicURL { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public List<string> MaintenanceStaff { get; set; } = new List<string>();

    }
}
