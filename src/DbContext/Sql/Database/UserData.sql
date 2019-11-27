CREATE TABLE UtilisateurData (
    Id int not null primary key auto_increment,
    UserId int not null,
    LastName varchar(100),
    FirstName varchar(100),
    Street varchar(100),
    Complement varchar(100),
    ZipCode varchar(100),
    Country varchar(100),
    City varchar(100),
    Phone varchar(10),
    NumTva varchar(15),
    Siret varchar(14),
    Email varchar(250),
    FOREIGN KEY (UserId) REFERENCES Utilisateur(Id)
)