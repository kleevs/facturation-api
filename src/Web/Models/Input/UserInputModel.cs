using FacturationApi.Models;

namespace Web.Models.Input
{
    public class UserInputModel : IUser
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Street { get; set; }
        public string Complement { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string NumTva { get; set; }
        public string Siret { get; set; }
        public string Email { get; set; }
    }
}