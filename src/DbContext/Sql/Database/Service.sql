CREATE TABLE Service (
    Id int not null primary key auto_increment,
    FactureId int,
    Description varchar(250),
    Price decimal(10,3),
    Quantity decimal(10,3),
    Tva decimal(5,2),
    Unite varchar(100),
    FOREIGN KEY (FactureId) REFERENCES Facture(Id)
)