drop database if EXISTS Ecommerce;
create database if NOT EXISTS Ecommerce;

use Ecommerce;

create table Address (
	AddressID int not null auto_increment,
    City varchar(50),
    District varchar(50),
    Commune varchar(50),
    Address varchar(200),
    constraint pk_Address primary key (AddressID)
	);

create table Users (
	UserID int not null auto_increment,
	AddressID int,
    UserName varchar(50) not null unique,
    Password varchar(200),
    FullName varchar(100),
    Birthday datetime,
    Email varchar(100),
    Phone varchar(20),
    Role enum('Customer', 'Admin'),
    constraint fk_Users_Address foreign key (AddressID)
    references Address(AddressID),
    constraint Users_chk1 check (UserName not like '% %'),
    constraint Users_chk2 check (Password not like '% %'),
    constraint pk_Users primary key (UserID)
    );

create table Shops (
	ShopID int not null auto_increment,
    UserID int,
    AddressID int,
    ShopName varchar(50),
    constraint fk_Shops_Users foreign key (UserID)
    references Users (UserID),
    constraint fk_Shops_Address foreign key (AddressID)
    references Address (AddressID),
    constraint pk_Shops primary key (ShopID)
	);

create table Products (
	ProductID int not null auto_increment,
    ShopID int,
    ProductName varchar(50),
    Description varchar(1000),
    Price int not null,
    Quantity int not null,
    constraint fk_Products_Shops foreign key (ShopID)
    references Shops (ShopID),
    constraint pk_Products primary key (ProductID)
    );

create table Categories (
	CategoryID int not null auto_increment,
    ShopID int,
    CategoryName varchar(50),
    constraint fk_Category_Shops foreign key (ShopID)
    references Shops(ShopID),
    constraint pk_Categories primary key (CategoryID)
    );

create table Product_Categories (
	ProductID int,
    CategoryID int,
    constraint fk_Sub_Products foreign key (ProductID)
		references Products (ProductID),
	constraint fk_Sub_Categories foreign key (CategoryID)
		references Categories (CategoryID),
	constraint pk_Product_Categories primary key (ProductID, CategoryID)
	);

create table Orders (
	OrderID int not null auto_increment,
    UserID int,
    ShopID int,
    CreateDate datetime,
    Status enum ('Shopping', 'Processing', 'Shipping', 'ToReceive', 'Finished', 'Failed'),
	constraint fk_Orders_Users foreign key (UserID)
		references Users (UserID),
	constraint fk_Orders_Shops foreign key (ShopID)
		references Shops (ShopID),
	constraint pk_Orders primary key(OrderID)
        );
create table OrderDetails (
	OrderID int,
    ProductID int,
    ProductNumber int,
    constraint fk_OD_Orders foreign key (OrderID)
		references Orders (OrderID),
	constraint fk_OD_Products foreign key (ProductID)
		references Products (ProductID),
	constraint pk_OrderDetails primary key (OrderID, ProductID)
        );

insert into Address (City, District, Commune, Address)
values 	('Thành Phố 1', 'Quận 1', 'Phường 1', 'Số nhà 1, đường 1'),
		('Thành Phố 1', 'Quận 1', 'Phường 1', 'Số nhà 2, đường 2'),
        ('Thành Phố 1', 'Quận 1', 'Phường 2', 'Số nhà 3, đường 3'),
        ('Thành Phố 1', 'Quận 2', 'Phường 3', 'Số nhà 4, đường 4'),
        ('Thành Phố 2', 'Quận 3', 'Phường 4', 'Số nhà 5, đường 5'),
        ('Thành Phố 3', 'Quận 4', 'Phường 5', 'Số nhà 6, đường 6');

insert into Users (UserName, Password, Role, FullName, Birthday, Email, Phone, AddressID)
VALUES ('User1', SHA2('123456', 256), 'Customer', 'Full Name 1', '1993-7-1', 'Email1@vtc.edu.vn', '0987654321', 1),
       ('User2', SHA2('123456', 256), 'Customer', 'Full Name 2', '1993-7-2', 'Email2@vtc.edu.vn', '0987654321', 2),
       ('User3', SHA2('123456', 256), 'Customer', 'Full Name 3', '1993-8-3', 'Email3@vtc.edu.vn', '0987654321', 3),
       ('User4', SHA2('123456', 256), 'Customer', 'Full Name 4', '1994-9-4', 'Email4@vtc.edu.vn', '0987654321', 4),
       ('User5', SHA2('123456', 256), 'Customer', 'Full Name 5', '1995-10-5', 'Email5@vtc.edu.vn', '0987654321', 5),
       ('User6', SHA2('123456', 256), 'Customer', 'Full Name 6', '1996-11-6', 'Email6@vtc.edu.vn', '0987654321', 6);

insert into Shops (UserID, AddressID, ShopName)
values 	(1, 1, 'Shop 1'),
		(2, 2, 'Shop 2');

insert into Products (ProductName, ShopID, Price, Quantity, Description)
VALUES 	('Sản Phẩm 1', 1, '123000', 11, 'Mô tả 1'),
		('Sản Phẩm 2', 1, '124000', 11, 'Mô tả 2'),
        ('Sản Phẩm 3', 1, '133000', 11, 'Mô tả 3'),
		('Sản Phẩm 4', 1, '143000', 11, 'Mô tả 4'),
        ('Sản Phẩm 5', 1, '153000', 11, 'Mô tả 5'),
		('Sản Phẩm 6', 1, '163000', 11, 'Mô tả 6'),
        ('Sản Phẩm 7', 1, '223000', 11, 'Mô tả 7'),
		('Sản Phẩm 8', 1, '323000', 11, 'Mô tả 8'),
        ('Sản Phẩm 9', 1, '423000', 11, 'Mô tả 9'),
		('Sản Phẩm 10', 2, '13000', 11, 'Mô tả 10'),
        ('Sản Phẩm 11', 2, '15000', 11, 'Mô tả 11'),
        ('Sản Phẩm 12', 2, '17000', 11, 'Mô tả 12'),
        ('Sản Phẩm 13', 2, '23000', 11, 'Mô tả 13'),
        ('Sản Phẩm 14', 2, '33000', 11, 'Mô tả 14'),
        ('Sản Phẩm 15', 2, '43000', 11, 'Mô tả 15'),
        ('Sản Phẩm 16', 2, '53000', 11, 'Mô tả 16'),
        ('Sản Phẩm 17', 2, '63000', 11, 'Mô tả 17');

INSERT INTO Categories (CategoryID, ShopID, CategoryName)
VALUES (1, 2, 'Đồ Chơi'),
       (2, 2, 'Thiết Bị Trường Học'),
       (3, 2, 'Bút Các Loại'),
       (4, 1, 'Thiết Bị Gia Dụng'),
       (5, 1, 'Thiết Bị Điện'),
       (6, 1, 'Phụ Kiện Điện Thoại'),
       (7, 1, 'Dụng Cụ Nhà Bếp');

INSERT INTO Orders (OrderID, UserID, ShopID, CreateDate, Status)
VALUES (1, 4, 1, '2022-07-22 11:20:34', 'Processing'),
       (2, 5, 1, '2022-07-22 11:21:13', 'Processing'),
       (3, 3, 2, '2022-07-22 11:21:55', 'Processing'),
       (4, 1, 2, '2022-07-22 11:22:18', 'Processing'),
       (5, 3, 1, '2022-07-22 11:22:41', 'Processing'),
       (6, 2, 1, '2022-07-22 11:22:59', 'Processing'),
       (7, 4, 2, '2022-07-22 11:23:14', 'Processing'),
       (8, 6, 1, '2022-07-22 11:23:34', 'Processing'),
       (9, 6, 2, '2022-07-22 11:23:58', 'Processing'),
       (10, 5, 2, '2022-07-22 11:24:19', 'Processing');

insert into OrderDetails (OrderID, ProductID, ProductNumber)
values (1, 1, 1),
       (2, 5, 1),
       (3, 13, 2),
       (4, 11, 1),
       (5, 8, 3),
       (6, 1, 4),
       (1, 3, 3),
       (1, 4, 2),
       (2, 2, 4),
       (3, 15, 1),
       (6, 3, 2),
       (7, 15, 3),
       (7, 12, 2),
       (8, 6, 3),
       (8, 7, 2),
       (8, 9, 1),
       (9, 10, 4),
       (9, 14, 2),
       (9, 17, 1),
       (10, 16, 3),
       (10, 11, 2);

INSERT INTO Product_Categories (ProductID, CategoryID)
VALUES (10, 1),
       (11, 1),
       (12, 1),
       (13, 1),
       (14, 2),
       (15, 2),
       (16, 2),
       (17, 2),
       (16, 3),
       (17, 3),
       (1, 4),
       (2, 4),
       (3, 4),
       (4, 4),
       (5, 5),
       (6, 5),
       (7, 5),
       (8, 6),
       (9, 6),
       (1, 7),
       (3, 7);

-- SELECT user, host FROM mysql.user;
DROP USER if EXISTS 'guest'@'localhost';
CREATE user if not exists 'guest'@'localhost' identified by '123456';

grant SELECT, INSERT, UPDATE on ecommerce.address to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.users to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.products to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.categories to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.orders to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.orderdetails to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.product_categories to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.shops to 'guest'@'localhost';