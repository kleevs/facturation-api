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
        IFactureDb NewFacture();
    }

    public interface IPaiementProvider
    {
        IQueryable<IPaiementDb> Paiement { get; }
        IPaiementDb NewPaiement();
        void RemovePaiement(IPaiementDb item);
    }

    public interface IUserProvider
    {
        IQueryable<IUser> User { get; }
        IQueryable<ILogin> Login { get; }
        IQueryable<IAuthenticateLogin> AuthenticateLogin { get; }
        IQueryable<IUserFilterable> IUserFilterable { get; }
    }

    public interface IAuthenticationProvider
    {
        ILogin Current { get; }
    }

    public interface IPieceJointeProvider
    {
        IQueryable<IFileDb> Files(string path);
        IFileDb NewFile();
        void RemoveFile(IFileDb item);
    }
}