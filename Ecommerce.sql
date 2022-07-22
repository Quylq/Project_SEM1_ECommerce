drop database Ecommerce;
create database Ecommerce;

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
    Birthday date,
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
       ('User2', SHA2('123456', 256), 'Customer', 'Full Name 2', '1993-7-2', 'Email2@vtc.edu.vn', '0987654322', 2),
       ('User3', SHA2('123456', 256), 'Customer', 'Full Name 3', '1993-8-3', 'Email3@vtc.edu.vn', '0987654323', 3),
       ('User4', SHA2('123456', 256), 'Customer', 'Full Name 4', '1994-9-4', 'Email4@vtc.edu.vn', '0987654324', 4),
       ('User5', SHA2('123456', 256), 'Customer', 'Full Name 5', '1995-10-5', 'Email5@vtc.edu.vn', '0987654325', 5),
       ('User6', SHA2('123456', 256), 'Customer', 'Full Name 6', '1996-11-6', 'Email6@vtc.edu.vn', '0987654326', 6);

update Users
set password = '123456';

insert into Shops (UserID, AddressID, ShopName)
values 	(1, 2, 'Shop 1'),
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

-- SELECT user, host FROM mysql.user; 
-- SHOW GRANTS FOR 'guest'@'localhost';
DROP USER 'guest'@'localhost';
CREATE user 'guest'@'localhost' identified by '123456';

grant SELECT, INSERT, UPDATE on ecommerce.address to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.users to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.products to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE, delete on ecommerce.categories to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.orders to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.orderdetails to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE, delete on ecommerce.product_categories to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.shops to 'guest'@'localhost';

select max(AddressID) from address;