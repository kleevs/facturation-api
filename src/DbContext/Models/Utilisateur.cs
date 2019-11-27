using FacturationApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models
{
    public partial class Utilisateur : Identifiable, ILogin
    {
        [Key]
        public override int Id { get; set; }
        public string Email { get; set; }
    }
}
