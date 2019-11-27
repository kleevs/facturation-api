CREATE TABLE Facture (
    Id int not null primary key auto_increment,
    UserDataId int not null,
    Numero int not null,
    RaisonSociale varchar(150),
    DateCreation DateTime,
    DateEcheance DateTime,
    PaymentOption int,
    LastName varchar(100),
    FirstName varchar(100),
    Street varchar(100),
    Complement varchar(100),
    ZipCode varchar(100),
    Country varchar(100),
    City varchar(100),
    FOREIGN KEY (UserDataId) REFERENCES UtilisateurData(Id)
)