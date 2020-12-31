using FacturationApi.Models;
using System.Linq;

namespace FacturationApi.Spi
{
    public interface IFactureProvider
    {
        IQueryable<IFacture> Facture { get; }
        IQueryable<IFacturePdf> FacturePdf { get; }
        IQueryable<IFactureOutput> FactureOutput { get; }
        IQueryable<IFactureFull> FactureFull { get; }
    }

    public interface IFactureDbProvider
    {
        IQueryable<IFactureDb> Facture { get; }
        IFactureDb New();
        void Remove(IFactureDb item);
    }

    public interface IPaiementProvider
    {
        IQueryable<IPaiementDb> Paiement { get; }
        IPaiementDb New();
        void Remove(IPaiementDb item);
    }

    public interface IUserProvider
    {
        IQueryable<IUser> User { get; }
        IQueryable<ILogin> Login { get; }
        IQueryable<IAuthenticateLogin> AuthenticateLogin { get; }
        IQueryable<IUserFilterable> IUserFilterable { get; }
        IUserDb NewUser();
        IUserInfoDb NewUserInfo();
    }

    public interface IAuthenticationProvider
    {
        ILogin Current { get; }
    }

    public interface IPieceJointeProvider
    {
        IQueryable<IFileDb> Files(string path);
        IFileDb NewFile();
        void RemoveFile(string filename);
    }
}