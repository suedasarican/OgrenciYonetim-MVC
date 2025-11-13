CREATE DATABASE OkulDB;
GO
USE OkulDB;
GO
CREATE TABLE Ogrenciler (
    ID int PRIMARY KEY IDENTITY(1,1),
    Ad nvarchar(50),
    Soyad nvarchar(50),
    Bolum nvarchar(50)
);
INSERT INTO Ogrenciler (Ad, Soyad, Bolum) VALUES ('Ali', 'Yılmaz', 'Bilgisayar Müh.');