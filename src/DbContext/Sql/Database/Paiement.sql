CREATE TABLE Paiement (
    Id int not null primary key auto_increment,
    FactureId int,
    DateCreation DateTime,
    Value decimal(10,3),
    FOREIGN KEY (FactureId) REFERENCES Facture(Id)
)