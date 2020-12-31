using System;

namespace FacturationApi.Models
{
    public class Token
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }
    }
}
