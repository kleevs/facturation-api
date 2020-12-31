namespace FacturationApi.Tools
{
    public abstract class Error : System.Exception
    {
        public static void ThrowIf<T>(bool b) where T : Error, new() { if (b) throw new T(); }
        public virtual object Content { get; }
        public virtual int? StatusCode { get; }
    }

    class LoginFailedError : Error
    {
        public override object Content => new { message = "Login/Mot de passe incorrect." };
    }

    class UnAuthorizedError : Error
    {
        public override int? StatusCode => 301;
    }

    class DeleteFactureError : Error
    {
        public override object Content => new { message = "Seule la dernière facture peut être supprimer." };
    }
    
    class SiretInvalidError : Error
    {
        public override object Content => new { message = "Le numéro de siret doit être composé de 14 chiffres." };
    }

    class UnAuthorizedToSaveFactureError : Error
    {
        public override object Content => new { message = "La facture possède des règlements et ne peux donc être modifiée." };
    }
    
    class UnAuthorizedToSaveFactureWithoutClientInfoError : Error
    {
        public override object Content => new { message = "Veuillez renseigner les informations client pour enregistrer la facture." };
    }

    class UserExitedError : Error
    {
        public override object Content => new { message = "Utilisateur déjà existant." };
    }

    class IsEmailEmptyError : Error
    {
        public override object Content => new { message = "L'email n'est pas renseigné." };
    }

    class IsPasswordInvalidError : Error
    {
        public override object Content => new { message = "Le mot de passe est invalide." };
    }
}
