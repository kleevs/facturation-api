CREATE TABLE Utilisateur (
    Id int not null primary key auto_increment,
    Email varchar(250) unique
)