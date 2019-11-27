using FacturationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models
{
    public partial class Account : Identifiable, IAuthenticateLogin
    {
        [Key]
        public override int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
