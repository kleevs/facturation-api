using FacturationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models
{
    public partial class Utilisateur : ILogin
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
