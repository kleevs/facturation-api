using FacturationApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Models
{
    public partial class MotDePasse : IAuthenticateLogin, IUserDb
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Value { get; set; }

        [ForeignKey("UserId")]
        public Utilisateur User { get; set; }

        string IAuthenticateLogin.Password => Value;
        string ILogin.Email => User?.Email;
        string IUserDb.Email { get => User?.Email; set { User = User ?? new Utilisateur(); User.Email = value; } }
        string IUserDb.Password { get => Value; set => Value = value;  }
    }
}
