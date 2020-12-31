namespace FacturationApi.Models
{
    public interface IUser : IIdentifiable
    {
        new int Id { get; set; }
        string LastName { get; }
        string FirstName { get; }
        string Street { get; }
        string Complement { get; }
        string ZipCode { get; }
        string Country { get; }
        string City { get; }
        string Phone { get; }
        string NumTva { get; }
        string Siret { get; }
        string Email { get; }
}

    public interface IUserFilterable : IUser
    {
        int UserId { get; }
    }

    public interface ILogin : IIdentifiable
    {
        string Email { get; }
    }

    public interface IAuthenticateLogin : ILogin
    {
        string Password { get; }
    }

    public interface IUserDb
    {
        string Email { get; set; }
        string Password { get; set; }
    }

    public interface IUserInfoDb
    {
        int UserId { get; set; }
        string LastName { get; set; }
        string FirstName { get; set; }
        string Street { get; set; }
        string Complement { get; set; }
        string ZipCode { get; set; }
        string Country { get; set; }
        string City { get; set; }
        string Phone { get; set; }
        string NumTva { get; set; }
        string Siret { get; set; }
        string Email { get; set; }
    }
}
