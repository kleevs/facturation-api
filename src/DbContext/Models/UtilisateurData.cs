using FacturationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models
{
    public partial class UtilisateurData : Identifiable, IUser, IUserFilterable
    {
        [Key]
        public override int Id { get; set; }
        public int UserId { get; set; }
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
