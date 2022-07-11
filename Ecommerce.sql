drop database Ecommerce;
create database Ecommerce;

use Ecommerce;

CREATE user 'customer'@'localhost' identified by '123456';
CREATE user 'seller'@'localhost' identified by '123456';

grant select, update on ecommerce.users to 'customer'@'localhost';
grant select on ecommerce.products to 'customer'@'localhost';
grant select on ecommerce.category to 'customer'@'localhost';
grant select, insert on ecommerce.orders to 'customer'@'localhost';
grant select, insert on ecommerce.orderdetails to 'customer'@'localhost';
grant select on ecommerce.product_categories to 'customer'@'localhost';

grant SELECT, INSERT, UPDATE, DELETE on ecommerce.users to 'seller'@'localhost';
grant SELECT, INSERT, UPDATE, DELETE on ecommerce.products to 'seller'@'localhost';
grant SELECT, INSERT, UPDATE, DELETE on ecommerce.categories to 'seller'@'localhost';
grant SELECT, INSERT, UPDATE, DELETE on ecommerce.orders to 'seller'@'localhost';
grant SELECT, INSERT, UPDATE, DELETE on ecommerce.orderdetails to 'seller'@'localhost';
grant SELECT, INSERT, UPDATE, DELETE on ecommerce.product_categories to 'seller'@'localhost';
grant SELECT, INSERT, UPDATE, DELETE on ecommerce.users_product to 'seller'@'localhost';

create table Users (
	UserID int not null auto_increment,
    UserName varchar(50) not null unique,
    Password varchar(200),
    FullName varchar(100),
    Birthday datetime,
    Email varchar(100),
    Phone varchar(20),
    Address varchar(200),
    Role enum('Customer', 'Seller'),
    constraint Users_chk1 check (UserName not like '% %'),
    constraint Users_chk2 check (Password not like '% %'),
    constraint pk_Users primary key (UserID)
    );

create table Products (
	ProductID int not null auto_increment,
    ProductName varchar(50),
    Description varchar(500),
    Price int not null,
    Quantity int not null,
    constraint pk_Products primary key (ProductID)
    );

create table Users_Product (
	UserID int,
    ProductID int,
    constraint fk_Users foreign key (UserID)
    references Users (UserID),
    constraint fk_Products foreign key (ProductID)
    references Products (ProductID),
    constraint pk_Users_Product primary key (UserID, ProductID)
    );

create table Categories (
	CategoryID int not null auto_increment,
    UserID int,
    CategoryName varchar(50),
    constraint fk_Category_Users foreign key (UserID)
    references Users(UserID),
    constraint pk_Categories primary key (CategoryID)
    );

create table Product_Categories (
	ProductID int,
    CategoryID int,
    constraint fk_ProCat_Products foreign key (ProductID)
		references Products (ProductID),
	constraint fk_ProCat_Category foreign key (CategoryID)
		references Categories (CategoryID),
	primary key (ProductID, CategoryID)
	);
    
create table Orders (
	OrderID int not null auto_increment,
    SellerID int,
    CustomerID int,
    CreateDate datetime,
    Status enum ('shopping', 'Processing', 'Confirm', 'Failed', 'Shipping', 'Finished'),
    constraint fk1_Orders_Users foreign key (SellerID)
		references Users (UserID),
	constraint fk2_Orders_Users foreign key (CustomerID)
		references Users (UserID),
	constraint pk_Orders primary key(OrderID)
        );

create table OrderDetails (
	OrderID int,
    ProductID int,
    ProductNumber int,
    constraint fk_OrdDet_Orders foreign key (OrderID)
		references Orders (OrderID),
	constraint fk_OrdDet_Products foreign key (ProductID)
		references Products (ProductID),
	constraint pk_OrderDetails primary key (OrderID, ProductID)
        );
        
select * from Users;

insert into Users (UserName, Password, Role, FullName, Birthday, Email, Phone, Address)
VALUES ('QuangQuy', SHA2('123456', 256), 'Seller', 'Lê Quang Quý', '1993-7-1', 'QuangQuy@vtc.edu.vn', '0987654321', 'Thanh Hóa'),
       ('TuanAnh', SHA2('123456', 256), 'Customer', 'Ngô Tuấn Anh', '2003-7-3','TuanAnh@vtc.edu.vn', '0987654322', 'Bắc Giang'),
       ('NguyenDat', SHA2('123456', 256), 'Customer', 'Nguyễn Tất Đạt', '2000-12-5','NguyenDat@vtc.edu.vn', '0987654323', 'Hà Nội'),
       ('LongTan', SHA2('123456', 256), 'Customer', 'Đào Long Tân', '1996-4-3','LongTan@vtc.edu.vn', '0987654324', 'Thái Bình'),
       ('CaoBac', SHA2('123456', 256), 'Customer', 'Cao Việt Bắc', '1996-2-14', 'CaoBac@vtc.edu.vn', '0987654325', 'Nghệ An'),
       ('VietAnh', SHA2('123456', 256), 'Seller', 'Nguyễn Duyên Việt Anh', '2000-4-6', 'VietAnh@vtc.edu.vn', '0987654326', 'Tiền Giang'),
       ('CanhToan', SHA2('123456', 256), 'Customer', 'Nguyễn Cảnh Toàn', '2002-3-4', 'CanhToan@vtc.edu.vn', '0987654327', 'Bắc Ninh'),
       ('VanTam', SHA2('123456', 256), 'Customer', 'Lê Văn Tâm', '1999-3-12', 'VanTam@vtc.edu.vn', '0987654328', 'Ninh Bình');

update Users
set password = '123456';

select * from Products;

insert into Products (ProductName, Price, Quantity)
VALUES ('Toyota Raize 1.0 Turbo', '527000000', 10),
	   ('Kia Seltos', '719000000', 10),
       ('Ford Territory', '780000000', 10),
	   ('Toyota Venzao', '1100000000', 10),
       ('Samsung Galaxy Z Fold3 5G', '36990000', 10),
	   ('iPhone 13 Pro Max', '31190000', 10),
       ('Samsung Galaxy S22 Ultra 5G', '30990000', 10),
	   ('OPPO Find X5 Pro 5G', '30990', 10),
       ('Đồng Hồ ORIENT Cơ 41 mm Nam', '37458000', 10),
       ('Đồng Hồ ORIENT Cơ 38.7 mm Nam RE-AW0004S00B', '17640000', 10),
       ('Đồng Hồ TITONI Cơ kính sapphire 27 mm Nữ 729 G-306', '24720000', 10),
       ('Máy giặt Samsung Inverter 9 Kg', '11590000', 10),
       ('Máy giặt Aqua Inverter 10 Kg AQD', '8990000', 10),
       ('Máy giặt LG Inverter 10 Kg', '14390', 10);
       
update Products 
set Description = ' ';

select * from Orders;

insert into Orders (SellerID, CustomerID, CreateDate, Status)
values (1, 3, '2020-01-01 15:10:10', 'Processing'),
	   (1, 4, '2020-11-01 12:10:10', 'Processing'),
       (1, 5, '2020-02-01 5:10:10', 'Processing'),
	   (6, 8, '2020-01-01 10:18:10', 'Processing'),
       (6, 8, '2020-01-09 11:19:10', 'Processing'),
	   (1, 7, '2020-01-08 16:10:10', 'Processing');

select * from OrderDetails;
insert into OrderDetails (OrderID, ProductID, ProductNumber)
values (1, 1, 1),
       (3, 5, 1),
       (3, 6, 2),
       (3, 7, 1),
       (3, 8, 3),
       (2, 1, 4);
       
insert into Users_Product (UserID, ProductID)
values  (1, 1),
		(1, 2),
        (1, 3),
        (1, 4),
        (1, 5),
        (1, 6),
        (1, 7),
        (1, 8),
        (6, 9),
        (6, 10),
        (6, 11),
        (6, 12),
        (6, 13),
        (6, 14);
