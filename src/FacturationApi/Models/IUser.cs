namespace FacturationApi.Models
{
    public interface IUser : IIdentifiable
    {
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
}
