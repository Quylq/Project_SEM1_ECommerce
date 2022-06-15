create database Ecommerce;

use Ecommerce;

create table Accounts (
	Account_ID int primary key auto_increment,
    Account_Name varchar(50),
    Account_Password varchar(50),
    Role varchar(50),
    constraint Account_chk1 check (Account_Name not like '% %'),
    constraint Account_chk2 check (Account_Password not like '% %')
    );
    
create table Customers (
	Customer_ID int primary key auto_increment,
    Account_ID int,
    Full_Name varchar(50),
    Email varchar(50),
    Birthday datetime,
    Phone_Number varchar(20),
    Address varchar(100),
    constraint Customer_chk1 check (Phone_Number regexp '^[\0-9]+$')
    );

Alter table Customers add constraint fk_Customers_Accounts foreign key (Account_ID)
		references Accounts (Account_ID);

create table Sellers (
	Seller_ID int primary key auto_increment,
    Account_ID int,
    Full_Name varchar(50),
    Email varchar(50),
    Birthday datetime,
    Phone_Number varchar(20),
    Address varchar(100),
    constraint fk_Sellers_Accounts foreign key (Account_ID)
		references Accounts (Account_ID),
    constraint Sellers_chk1 check (Phone_Number regexp '^[\0-9]+$')
    );

create table Products (
	Product_ID int primary key auto_increment,
    Seller_ID int,
    Product_Name varchar(50),
    Description varchar(50),
    Price float,
    Catogory varchar(50)
    );

alter table Products add constraint fk_Products_Sellers foreign key (Seller_ID) references Sellers (Seller_ID);
alter table Products change column Catogory Category varchar(50);

create table Categorys (
	Category_ID int primary key auto_increment,
    Category_Name varchar(50)
    );

create table Sub_Category (
	Product_ID int,
    Category_ID int,
    constraint fk_Sub_Category_Products foreign key (Product_ID)
		references Products (Product_ID),
	constraint fk_Sub_Category_Category foreign key (Category_ID)
		references Categorys (Category_ID),
	primary key (Product_ID, Category_ID)
	);
    
create table Invoices (
	Invoice_ID int primary key auto_increment,
    Customer_ID int,
    Seller_ID int,
    Create_Date datetime,
    constraint fk_Invoices_Customers foreign key (Customer_ID)
		references Customers (Customer_ID),
	constraint fk_Invoices_Sellers foreign key (Seller_ID)
		references Sellers (Seller_ID)
        );

create table ProductDetails (
	Invoice_ID int,
    product_ID int,
    Product_Number int,
    constraint fk_ProDet_Invoices foreign key (Invoice_ID)
		references Invoices (Invoice_ID),
	constraint fk_ProDet_Products foreign key (Product_ID)
		references Products (Product_ID)
        );
        
	