using FacturationApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models
{
    public partial class MotDePasse : Identifiable, IAuthenticateLogin
    {
        [Key]
        public override int Id { get; set; }
        public int UserId { get; set; }
        public string Value { get; set; }

        [ForeignKey("UserId")]
        public Utilisateur User { get; set; }

        string IAuthenticateLogin.Password => Value;
        string ILogin.Email => User?.Email;
    }
}
