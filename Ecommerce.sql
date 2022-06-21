drop database Ecommerce;
create database Ecommerce;

use Ecommerce;

create table Users (
	UserID int not null auto_increment,
    UserName varchar(50) not null unique,
    Password varchar(200),
    Role varchar(50),
    constraint Users_chk1 check (UserName not like '% %'),
    constraint Users_chk2 check (Password not like '% %'),
    constraint pk_Users primary key (UserID)
    );
    
create table Customers (
	CustomerID int not null auto_increment,
    UserID int,
    Name varchar(100),
    Email varchar(100),
    Phone varchar(20),
    Address varchar(200),
    constraint Customers_chk1 check (Phone regexp '^[\0-9]+$'),
    constraint fk_Customers_Users foreign key (UserID)
		references Users (UserID),
    constraint pk_Customers primary key (CustomerID)
    );

create table Sellers (
	SellerID int not null auto_increment,
    UserID int,
    Name varchar(100),
    Email varchar(100),
    Phone varchar(20),
    Address varchar(200),
    constraint Sellers_chk1 check (Phone regexp '^[\0-9]+$'),
    constraint fk_Sellers_Users foreign key (UserID)
		references Users (UserID),
    constraint pk_Sellers primary key (SellerID)
    );

create table Products (
	ProductID int not null auto_increment,
    SellerID int,
    ProductName varchar(50),
    Description varchar(500),
    Price int,
    constraint fk_Products_Sellers foreign key (SellerID) references Sellers (SellerID),
    constraint pk_Products primary key (ProductID)
    );

create table Category (
	CategoryID int not null auto_increment,
    CategoryName varchar(50),
    constraint pk_Category primary key (CategoryID)
    );

create table Sub_Category (
	ProductID int,
    CategoryID int,
    constraint fk_SubCategory_Products foreign key (ProductID)
		references Products (ProductID),
	constraint fk_SubCategory_Category foreign key (CategoryID)
		references Category (CategoryID),
	primary key (ProductID, CategoryID)
	);
    
create table Oders (
	OderID int not null auto_increment,
    CustomerID int,
    SellerID int,
    CreateDate datetime,
    constraint fk_Oders_Customers foreign key (CustomerID)
		references Customers (CustomerID),
	constraint fk_Oders_Sellers foreign key (SellerID)
		references Sellers (SellerID),
	constraint pk_Users primary key(OderID)
        );

create table ProductDetails (
	OderID int,
    ProductID int,
    ProductNumber int,
    constraint fk_ProDet_Oders foreign key (OderID)
		references Oders (OderID),
	constraint fk_ProDet_Products foreign key (ProductID)
		references Products (ProductID),
	constraint pk_ProductDetails primary key (OderID, ProductID)
        );
        
select * from Users;


insert into Users (UserName, Password, Role)
VALUES ('QuangQuy', SHA2('123456', 256), 'Seller'),
       ('TuanAnh', SHA2('123456', 256), 'Customer'),
       ('NguyenDat', SHA2('123456', 256), 'Customer'),
       ('LongTan', SHA2('123456', 256), 'Customer'),
       ('CaoBac', SHA2('123456', 256), 'Customer'),
       ('VietAnh', SHA2('123456', 256), 'Seller'),
       ('CanhToan', SHA2('123456', 256), 'Customer'),
       ('VanTam', SHA2('123456', 256), 'Customer');

select * from Customers;

insert into Customers (UserID, Name, Email, Phone, Address)
VALUES (1, 'Quang Quy', 'QuangQuy@vtc.edu.vn', '0987654321', 'Thanh Hóa'),
       (2, 'Tuan Anh', 'TuanAnh@vtc.edu.vn', '0987654322', 'Bắc Giang'),
       (3, 'Nguyen Dat', 'NguyenDat@vtc.edu.vn', '0987654323', 'Hà Nội'),
       (4, 'Long Tan', 'LongTan@vtc.edu.vn', '0987654324', 'Thái Bình'),
       (5, 'Cao Bac', 'CaoBac@vtc.edu.vn', '0987654325', 'Nghệ An'),
       (6, 'Viet Anh', 'VietAnh@vtc.edu.vn', '0987654326', 'Tiền Giang'),
       (7, 'Canh Toan', 'CanhToan@vtc.edu.vn', '0987654327', 'Bắc Ninh'),
       (8, 'Van Tam', 'VanTam@vtc.edu.vn', '0987654328', 'Ninh Bình');
       
select * from Sellers;

insert into Sellers (UserID, Name, Email, Phone, Address)
VALUES (1, 'Quang Quy', 'QuangQuy@vtc.edu.vn', '0987654321', 'Thanh Hóa'),
       (6, 'Viet Anh', 'VietAnh@vtc.edu.vn', '0987654326', 'Thái Bình');
 
select * from Category;
insert into Category (CategoryName)
values ('Thời Trang Nam'),
	   ('Thời Trang Nữ'),
       ('Thời Trang Trẻ Em'),
	   ('Túi Ví Nữ'),
       ('Áo'),
	   ('Giày Dép Nam'),
       ('Xe'),
	   ('Thiết Bị Điện Tử'),
       ('Máy Tính, LapTop'),
	   ('Máy Ảnh'),
       ('Đồng Hồ'),
	   ('Giày'),
       ('Xe'),
	   ('Thiết Bị Điện Dân Dụng'),
       ('Sách'),
       ('Điện Thoại');

select * from Products;

insert into Products (SellerID, ProductName, Price)
VALUES (2, 'Toyota Raize 1.0 Turbo', '527000000'),
	   (2, 'Kia Seltos', '719000000'),
       (2, 'Ford Territory', '780000000'),
	   (2, 'Toyota Venzao', '1100000000'),
       (2, 'Samsung Galaxy Z Fold3 5G', '36990000'),
	   (2, 'iPhone 13 Pro Max', '31190000'),
       (2, 'Samsung Galaxy S22 Ultra 5G', '30990000'),
	   (2, 'OPPO Find X5 Pro 5G', '30990'),
       (1, 'Đồng Hồ ORIENT Cơ 41 mm Nam', '37458000'),
       (1, 'Đồng Hồ ORIENT Cơ 38.7 mm Nam RE-AW0004S00B', '17640000'),
       (1, 'Đồng Hồ TITONI Cơ kính sapphire 27 mm Nữ 729 G-306', '24720000'),
       (1, 'Máy giặt Samsung Inverter 9 Kg', '11590000'),
       (1, 'Máy giặt Aqua Inverter 10 Kg AQD', '8990000'),
       (1, 'Máy giặt LG Inverter 10 Kg', '14390');
       
select * from Sub_Category;
insert into Sub_Category (CategoryID, ProductID)
VALUES (7, 1),
       (7, 2),
       (7, 3),
       (7, 4),
       (16, 5),
       (16, 6),
       (16, 7),
       (16, 8),
       (8, 5),
       (8, 6),
       (8, 7),
       (8, 8),  
       (11, 9),
       (11, 10),
       (11, 11),
       (14, 12),
       (14, 13),
       (14, 14);

select * from Oders;

insert into Oders (CustomerID, SellerID, CreateDate)
values (1, 2, '2020-01-01 15:10:10'),
	   (1, 1, '2020-11-01 12:10:10'),
       (2, 2, '2020-02-01 5:10:10'),
	   (3, 2, '2020-01-01 10:18:10'),
       (4, 2, '2020-01-09 11:19:10'),
	   (4, 1, '2020-01-08 16:10:10');
       
select * from ProductDetails;
insert into ProductDetails (OderID, ProductID, ProductNumber)
values (1, 1, 1),
       (3, 5, 1),
       (3, 6, 2),
       (3, 7, 1),
       (3, 8, 3),
       (2, 10, 4);