CREATE TABLE MotDePasse (
    Id int not null primary key auto_increment,
    UserId int not null,
    Value varchar(50000),
    FOREIGN KEY (UserId) REFERENCES Utilisateur(Id)
)