namespace Web.Models.Input
{
    public class ContactModel
    {
        public string RaisonSociale { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public AddressModel Address { get; set; }
    }
}
