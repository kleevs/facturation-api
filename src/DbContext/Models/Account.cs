using FacturationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models
{
    public partial class Account : IAuthenticateLogin
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
