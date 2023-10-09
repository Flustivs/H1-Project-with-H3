namespace AdminSite.DAL
{
    public class Person
    {
        public int PersonID { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string PersonPassword { get; set; }
        public string PersonName { get; set; }
        //public int RoleID { get; set; }
        public List<int> RoleIDs { get; set; } = new List<int>();    
        public List<Order> Orders { get; set; }

    }
}
