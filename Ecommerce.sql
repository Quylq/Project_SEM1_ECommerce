drop database if EXISTS Ecommerce;
create database if NOT EXISTS Ecommerce;

use Ecommerce;

create table Address
(
    AddressID int not null auto_increment,
    City      varchar(30),
    District  varchar(30),
    Commune   varchar(30),
    Address   varchar(110),
    constraint pk_Address primary key (AddressID)
);

create table Users
(
    UserID    int not null auto_increment,
    AddressID int,
    UserName  varchar(50) not null unique,
    Password  varchar(200) not null,
    FullName  varchar(100) not null,
    Birthday  date,
    Email     varchar(100),
    Phone     varchar(20),
    Role      enum ('Customer', 'Admin'),
    constraint fk_Users_Address foreign key (AddressID)
        references Address (AddressID),
    constraint Users_chk1 check (UserName not like '% %'),
    constraint Users_chk2 check (Password not like '% %'),
    constraint pk_Users primary key (UserID)
);

create table Shops
(
    ShopID    int not null auto_increment,
    UserID    int,
    AddressID int,
    ShopName  varchar(50),
    constraint fk_Shops_Users foreign key (UserID)
        references Users (UserID),
    constraint fk_Shops_Address foreign key (AddressID)
        references Address (AddressID),
    constraint pk_Shops primary key (ShopID)
);

create table Products
(
    ProductID   int not null auto_increment,
    ShopID      int,
    ProductName varchar(500) not null,
    Description varchar(1000),
    Price       int not null,
    Amount    int not null,
    constraint fk_Products_Shops foreign key (ShopID)
        references Shops (ShopID),
    constraint pk_Products primary key (ProductID)
);

create table Categories
(
    CategoryID   int not null auto_increment,
    ShopID       int,
    CategoryName varchar(50),
    constraint fk_Category_Shops foreign key (ShopID)
        references Shops (ShopID),
    constraint pk_Categories primary key (CategoryID)
);

create table Product_Categories
(
    ProductID  int,
    CategoryID int,
    constraint fk_Sub_Products foreign key (ProductID)
        references Products (ProductID),
    constraint fk_Sub_Categories foreign key (CategoryID)
        references Categories (CategoryID),
    constraint pk_Product_Categories primary key (ProductID, CategoryID)
);

create table Orders
(
    OrderID    int not null auto_increment,
    UserID     int,
    ShopID     int,
    CreateDate datetime,
    Status     enum ('Shopping', 'Processing', 'Shipping', 'ToReceive', 'Finished', 'Failed'),
    constraint fk_Orders_Users foreign key (UserID)
        references Users (UserID),
    constraint fk_Orders_Shops foreign key (ShopID)
        references Shops (ShopID),
    constraint pk_Orders primary key (OrderID)
);
create table OrderDetails
(
    OrderID int,
    ProductID int,
    Quantity int,
    constraint fk_OD_Orders foreign key (OrderID)
        references Orders (OrderID),
    constraint fk_OD_Products foreign key (ProductID)
        references Products (ProductID),
    constraint pk_OrderDetails primary key (OrderID, ProductID)
);

insert into Address (City, District, Commune, Address)
values ('Hà Nội', 'Quận Đống Đa', 'Phường Trung Liệt', 'Phố Chùa Bộc'),
       ('Hà Nội', 'Quận Hà Đông', 'Phường Phú Lãm', 'Đường Thanh Lãm'),
       ('Hà Nội', 'Quận Nam Từ Liêm', 'Phường Mỹ Đình', 'Đường Mỹ Đình'),
       ('Hà Nội', 'Quận Long Biên', 'Phường Đức Giang', 'Đường Ngô Gia Tự'),
       ('Hà Nội', 'Quận Thanh Xuân', 'Phường Thanh Xuân Bắc', 'Đường Khuất Duy Tiến'),
       ('Hà Nội', 'Quận Hai Bà Trưng', 'Phường Đồng Tâm', 'Đường Giải Phóng'),
       ('Hà Nội', 'Quận Hoàng Mai', 'Phường Giáp Bát', 'Đường Giáp Bát'),
       ('Hà Nội', 'Quận Cầu Giấy', 'Phường Trung Hòa', 'Phố Trung Kính'),
       ('Hà Nội', 'Quận Ba Đình', 'Phường Kim Mã', 'Đường Kim Mã');

INSERT INTO Users (AddressID, UserName, Password, FullName, Birthday, Email, Phone, Role)
VALUES (1, 'User1', SHA2('123456', 256), 'Văn Đức',
        '1992-06-02', 'VanDuc110@vtc.edu.vn', '0986532764', 'Customer'),
       (2, 'User2', SHA2('123456', 256), 'Việt Anh',
        '2002-02-23', 'Vietanh1140@vtc.edu.vn', '0987123643', 'Customer'),
       (3, 'User3', SHA2('123456', 256), 'Quang Quý',
        '1993-07-25', 'Quangquy1140@vtc.edu.vn', '0981238143', 'Customer'),
       (4, 'User4', SHA2('123456', 256), 'Tuấn Anh',
        '2003-04-12', 'Tuananh1140@vtc.edu.vn', '0983152643', 'Customer'),
       (5, 'User5', SHA2('123456', 256), 'Cảnh Toàn',
        '2002-10-28', 'Canhtoan1140@vtc.edu.vn', '0986157443', 'Customer'),
       (6, 'User6', SHA2('123456', 256), 'Tất Đạt',
        '2000-07-19', 'Tatdat1140@vtc.edu.vn', '0981236432', 'Customer'),
       (7, 'User7', SHA2('123456', 256), 'Cao Bắc',
        '2001-01-20', 'Caobac1140@vtc.edu.vn', '0986735242', 'Customer'),
       (8, 'User8', SHA2('123456', 256), 'Văn Tâm',
        '2002-11-14', 'Vantam1140@vtc.edu.vn', '0986135742', 'Customer'),
       (9, 'User9', SHA2('123456', 256), 'Long Tân',
        '2001-04-28', 'Longtan1140@vtc.edu.vn', '0985663254', 'Customer'),
       (10, 'User10', '123456', 'Lê Văn Tèo',
        '2001-04-26', 'Levanteo@vtc.edu.vn', '0985663251', 'Customer');

INSERT INTO Shops (UserID, AddressID, ShopName)
VALUES (1, 1, 'Shop Đồ Gia Dụng'),
       (2, 2, 'Shop Đồ Dùng Học Sinh'),
       (3, 3, 'Shop Phụ Kiện Điện Tử'),
       (4, 4, 'Shop Thời Trang');

INSERT INTO ecommerce.products (ShopID, ProductName, Description, Price, Amount)
VALUES (1, 'Kệ để đồ, kệ đa năng chia tầng Inochi Nhựa Dùng Để Đựng Gia Vị Nhà Bếp Đựng Đồ Thực Phẩm, Phòng ngủ KET4T', '+ Sản phẩm xuất nhật chất lượng cao
+ Dùng trong nhà bếp, phòng khách, phòng ngủ.
+ Nhựa nguyên sinh cao cấp. chắc chắn, chịu được trọng tải lớn
+Đạt tiêu chuẩn khắt khe cho các sản phẩm gia dụng của Nhật Bản
+ Kiểu dáng sang trọng, tận dụng không gian dọc (chồng tầng) tiết kiệm diện tích
+ Thiết kế thông minh lắp ráp dễ dàng và đa chức năng.
+ Sử dụng đồ dùng đa năng, linh hoạt tuỳ biến trong nhiều không gian.
THÔNG SÔ KỆ ĐỂ ĐỒ 2 TẦNG
+ Kích thước: 370 x 230 x 410 mm
+ Khối lượng: 500 gr
+ Màu sắc: Trắng, Be, Nâu', 165000, 25),
       (1, 'Kệ đa năng chia 2/3/4 tầng Inochi đựng mỹ phẩm thông minh. đựng đồ dùng nhà bếp -4 tầng KET4T', '+ Sản phẩm xuất nhật chất lượng cao
+ Dùng trong nhà bếp, phòng khách, phòng ngủ.
+ Nhựa nguyên sinh cao cấp. chắc chắn, chịu được trọng tải lớn
+Đạt tiêu chuẩn khắt khe cho các sản phẩm gia dụng của Nhật Bản
+ Kiểu dáng sang trọng, tận dụng không gian dọc (chồng tầng) tiết kiệm diện tích
+ Thiết kế thông minh lắp ráp dễ dàng và đa chức năng.
+ Sử dụng đồ dùng đa năng, linh hoạt tuỳ biến trong nhiều không gian.
+ Kích thước: 37 x 23 x 100 cm
+ Màu sắc: Trắng', 200000, 25),
       (1, 'Kệ đựng mỹ thẩm - Kệ Đa Năng - kệ đẩy di động - Phòng ngủ 4 tầng có bánh xe di động', '+ Nguyên liệu nhựa nguyên sinh cao cấp, an toàn cho sức khoẻ.
+ Đạt tiêu chuẩn khắt khe cho các sản phẩm gia dụng, thực phẩm, y tế của Nhật Bản.
+ Chất lượng cao, tích hợp các tính năng vượt trội, kháng khuẩn, khử mùi…
+ Kiểu dáng gọn nhẹ, Tận dụng không gian dọc (chồng tầng) tiết kiệm diện tích
+ Sử dụng đồ dùng đa năng, linh hoạt tuỳ biến trong nhiều không gian.
+ Kích thước: 480 x 210 x 1000 mm
+ Khối lượng: 1600 gr
+ Màu sắc: Trắng, Be, Nâu', 230000, 25),
       (1, 'Kệ trượt TOKYO INOCHI Thông Minh Dùng Đựng Gia Vị Nhà Bếp, Giá Để Đồ Nhà Tắm, Chắc Chắn, Tiện lợi KE009', 'Mã SP: HIN.KETR.TOKY
Kích thước: 455x260x447 mm
Khối lượng: 1160 g
Chất liệu: Nhựa PP (Polypropylen), PS nguyên sinh, hạt màu, inox cao cấp
Màu sắc: Trắng ngọc,
+ Sản phẩm xuất khẩu thị trường Nhật Bản chính hãng 100% do chúng tôi sản xuất.
+ Bảo hành 3 tháng 1 đổi 1 với lỗi của nhà sản xuất
+ Chúng tôi bán sản phẩm chứ không bán lương tâm.
+ Đảm bảo chất lượng, dịch vụ tốt nhất, hàng được giao từ 1-5 ngày kể từ ngày đặt hàng.', 150000, 25),
       (1, 'Kệ gầm bếp , đa năng thông minh Tokyo INOCHI dành cho nhà bếp nhà tắm', 'Mã SP: HIN.KEGB.TOKY
SẢn Phẩm: Kệ bếp đa năng,kệ thông minh bếp Tokyo INOCHI tiện lợi dành cho nhà bếp nhà tắm nhà bếp KE008
Kích thước: 457x300x390 mm
Khối lượng: 1370 g
Chất liệu: Nhựa PP (Polypropylen), PS nguyên sinh, hạt màu, inox cao cấp
Màu sắc: Trắng ngọc, Ghi sữa
ƯU ĐIỂM:
Kệ gầm bếp đa năng thông minh Tokyo INOCHI tiện lợi dành cho nhà bếp nhà tắm nhà bếp KE008
– Thiết kế hiện đại, dễ dàng tháo lắp.
– Dễ dàng tăng giảm kích thước.
– Cấu tạo chắc chắn chịu lực tốt.
– Sản phẩm được sản xuất theo dây chuyền công nghệ và kiểm soát chất lượng theo tiêu chuẩn Nhật Bản
Kệ gầm bếp,kệ bếp,kệ nhà tắm,kệ đa năng,inochi,giá để bát,kệ bếp thông minh,', 140000, 25),
       (1, 'Kệ đựng chén -Bát Nhựa Inox 2 Tầng Inochi Có Khay Thoát Nước Thông Minh Tiện dụng hàng chính hãng', '+ Nguyên liệu nhựa nguyên sinh cao cấp, an toàn cho sức khoẻ.
+ Đạt tiêu chuẩn khắt khe cho các sản phẩm gia dụng, thực phẩm, y tế của Nhật Bản.
+ Gồm nhiều ngăn đựng riêng biệt. Giúp tận dụng tối đa không gian bếp của bạn.
+ Thiết kế sang trọng, đẹp mắt, màu sắc trang nhã
+ Thiết kế thoát nước thông minh
+ Kích thước: 420 x 325 x 362 mm
+ Khối lượng: 1900 gr
+ Màu sắc: Trắng', 360000, 25),
       (1,
        'Kệ nhà tắm - trượt TOTYO INOCHI Thông Minh Dùng Đựng Gia Vị Nhà Bếp Giá Để Đồ Nhà Tắm Chắc Chắn Tiện lợi KE009', '+ Nguyên liệu nhựa nguyên sinh cao cấp, an toàn cho sức khoẻ.
+ Đạt tiêu chuẩn khắt khe cho các sản phẩm gia dụng, thực phẩm, y tế của Nhật Bản.
+ Chất lượng cao, tích hợp các tính năng vượt trội, kháng khuẩn, khử mùi…
Kích thước: 455x260x447 mm
Khối lượng: 1160 g
Chất liệu: Nhựa PP (Polypropylen), PS nguyên sinh, hạt màu, inox cao cấp
Màu sắc: Trắng ngọc,', 150000, 25),
       (1, 'Kệ Nhà Bếp dạng gấp Tokyo INOCHI tiện nghi đa năng gấp gọn nhà bếp, phòng ăn Bàn gấp KE007', 'Thương hiệu: inochi
Inochi Sản xuất tại Việt Nam (Hàng xuất Nhật)
Kích thước: 405x239x216 mm
Khối lượng: 464 g
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, hạt màu, inox cao cấp
(Sản phẩm gồm các màu: Trắng kệ, Be, Nâu kệ - SHOP giao màu ngẫu nhiên)', 70000, 25),
       (1,
        'Kệ treo quần áo Tokyo INOCHI thiết kế thông minh linh hoạt, tiện xếp gọn reo quần áo ngăn nắp gọn gàng KE003Kệ treo quần', 'Thương hiệu: Inochi.
Kích thước: 700 x 450 x 1550 mm
Màu sắc: Trắng, Be, Nâu
Khối lượng: 6480g.
Chất liệu: Nhựa PP (Polypropylen), PA nguyên sinh, hạt màu, inox cao cấp.', 910000, 25),
       (1,
        'Thùng rác có lõi bên trong 20l- INOCHI đựng Rác Văn Phòng, Khách sạn, Gia Đình, trong nhà và ngoài hành lang', '+ Nguyên liệu nhựa nguyên sinh cao cấp, an toàn cho sức khoẻ.
+ Đạt tiêu chuẩn khắt khe cho các sản phẩm gia dụng, thực phẩm, y tế của Nhật Bản.
+ Chất lượng cao, tích hợp các tính năng vượt trội, kháng khuẩn, khử mùi…
+ Nắp đậy kín không để lọt mùi ra ngoài môi trường
+ Đa dạng cách mở, có quai xách, dễ dàng sử dụng
+ Thiết kế chân đạp âm tránh gãy vỡ', 290000, 25),
       (1, 'Thùng Rác -Nhựa Tròn Inochi 5 Lít Màu Rất Đẹp Làm Sọt Rác Văn Phòng, Khách Sạn, Đựng Rác Gia Đình SR05L', 'Mã SP: HIN.SRTR.0005
Kích thước: 184x184x234 mm
Khối lượng: 148 g
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, hạt màu, và phụ gia kháng khuẩn Ag+
Màu sắc: Be sữa, Ghi sữa, Xanh nhạt, Nâu café
', 65000, 25),
       (1, 'Thùng rác nắp lật 5L, 10L, 15L INOCHI cao cấp siêu bền đẹp', 'Mã SP: HIN.RANL.0010
Kích thước: 261 x 170 x 425 mm
Khối lượng: 716 g
Chất liệu: Nhựa PP (Polypropylen), LDPE nguyên sinh, hạt màu, inox và phụ gia kháng khuẩn Ag+
Màu sắc: Thân Trắng kem + Nắp Ghi sữa/ Xanh chàm/ Nâu café/ Be sữa', 160000, 25),
       (1, 'Thùng Rác -Nhựa Đạp Chân Nắp Tròn INOCHI 6L Lít Làm Sọt Rác Văn Phòng,Đựng Rác Gia Đình, Trong Nhà THRT6L', 'Kích thước: 243x233x281 mm
Khối lượng: 729 g
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, HIPS, hạt màu, inox và phụ gia kháng khuẩn Ag+
Màu sắc: Thân Trắng kem + Nắp Ghi sữa/ Xanh chàm, Thân Ghi sữa + Nắp Ghi sữa,', 115000, 25),
       (1,
        'Thùng Rác Thông Minh,Nhựa Nắp Lật Inochi 5Lít Rất Đẹp Làm Sọt Rác Văn Phòng, Khách Sạn, Đựng Rác Gia Đình THRNL05', 'Mã SP: HIN.RANL.0005
Kích thước: 226 x 167 x 300 mm
Khối lượng: 460 g
Chất liệu: Nhựa PP (Polypropylen), LDPE nguyên sinh, hạt màu, inox và phụ gia kháng khuẩn Ag+
Màu sắc: Thân Trắng kem + Nắp Ghi sữa/ Xanh chàm/ Nâu café/ Be sữa', 100000, 25),
       (1,
        'Thùng rác thông minh HIRO INOCHI 2 Ngăn bấm nút tự động mở nắp phân loại rác cho phòng khách, phòng ngủ,vệ sinh RAPL.002', 'Kích thước: 358x336x530 mm
Khối lượng: 2445 g
Chất liệu: Nhựa PP, PA nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+), lò xo
Màu sắc: Xanh nhạt + Ghi sữa
- Thùng rác nhựa nắp lật
- Thùng rác thông minh
- Thùng rác tiện ích', 300000, 25),
       (1,
        'Thùng rác thông minh HIRO INOCHI 3 Ngăn bấm nút tự động mở nắp phân loại rác cho phòng khách, phòng ngủ,vệ sinh RAPL.003', 'Mã SP: HIN.RAPL.003N
Kích thước: 537x336x530 mm
Khối lượng: 3620 g
Chất liệu: Nhựa PP, PA nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+), lò xo
Màu sắc: Xanh nhạt + Ghi sữa + Nâu Café
MỘT SỐ SẢN PHẨM LIÊN QUAN
- Thùng rác nhựa nắp lật
- Thùng rác thông minh
- Thùng rác tiện ích', 600000, 25),
       (1, 'Bát nhựa, chén nhựa cao cấp Inochi Amori chịu nhiệt chịu nhiệt 415ml BAN415 (1 chiếc)', 'THÔNG TIN SẢN PHẨM
Mã SP: HIN.BAAM.0415
Kích thước: 121x121x55 mm
Khối lượng: 52 g
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+)
Màu sắc: Hồng nhạt, xanh nhạt, cam sữa', 10000, 25),
       (1, 'Thau rửa mặt - cho trê em nhựa nguyên sinh kháng khuẩn Ag+ cao cấp Notoro inochi 32 cm TRM01', 'Thau nhựa cao cấp Notoro 38cm
Kích thước : 380x380x120 mm
Khối lượng: 356 g
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+)
Màu sắc: Xanh bạc hà, hồng nhạt, xanh nhạt, xanh trà', 50000, 25),
       (1,
        'Bát ăn dặm cho bé ,dùng ăn cơm, ăn dặm cho bé cỡ lớn inochi amori nhựa nguyên sinh kháng khuẩn Ag+ cao cấp BAL830', 'Mã SP: BAL830
Kích thước: 161x161x60 mm
Khối lượng: 80 g
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+)
Màu sắc: Hồng nhạt, xanh nhạt, cam sữa', 17000, 25),
       (1,
        'Ghế Nhà Tắm Notoro INOCHI Dành cho Mẹ Và Bé - Nhựa Cao Cấp Chắc Chắn - Chiều Cao Phù Hợp Cho Mẹ Và Bé Ngồi Thoải Mái', 'Mã SP: HIN.GHNT.0323
Kích thước: 323x261x198 mm
Khối lượng: 606 g
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+)
Màu sắc: Trắng ngọc, hồng nhạt, xanh nhạt, xanh bạc hà', 65000, 25),
       (1, 'Thìa tập ăn INOCHI muỗng tập ăn dặm cho bé Amori', 'Mã sản phẩm: HIN.THTA.AM01
Kích thước: 99x42x39 mm/ 100x40x200 mm
Khối lượng tịnh: 15 g
Thành phần: Nhựa PP nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+)', 30000, 25),
       (1, 'Gáo Nhựa -Múc Nước Tắm Cho Bé Notoro INOCHI Nhật Bản Với Nhựa Cao Cấp Chắc Chắn CANUOC', 'Mã SP: HIN.GANO.0273
Kích thước: 273x142x130 mm
Khối lượng: 120 g
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+)
Màu sắc: Trắng ngọc, hồng nhạt, xanh nhạt, xanh bạc hà', 30000, 25),
       (1, 'Thìa ăn dặm muỗng cho bé inochi cho trẻ nhỏ tự ăn thông minh loại dài', 'Mã sản phẩm: HIN.TAAM.BO02
Kích thước: 137x33x19 mm / 135x40x200 mm
Khối lượng tịnh: 7 g
Thành phần: Nhựa PP nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+)', 30000, 25),
       (1, 'Thùng đa năng trẻ em Inochi - Notoro 30L màu Xanh/Đỏ/Vàng/Hồng', 'Mã SP: HIN.THDN.0030
Kích thước: 472x353x249 mm
Khối lượng: 1293 g
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, ABS, Roon silicon, hạt màu và phụ gia kháng khuẩn ion Ag+
Màu sắc: Thân Trắng ngọc + Nắp trắng ngọc, Thân Xanh đậm + Nắp xanh nhạt', 190000, 25),
       (1,
        'Chậu tắm -Tựa Tắm Cho Bé Notoro inochi Nhật Bản Thiết Kế Chắc Chắn Chống Trượt Giúp Bé sơ sinh Thoải Mái TUATAM', 'Mã SP: HIN.TTTU.NOPL
Kích thước: 611x294x225 mm
Khối lượng: 392 gram
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+)
Màu sắc: Thân trắng ngọc + nút logo trắng ngọc, Thân xanh nhạt + Nút logo xanh đậm', 245000, 25),
       (1, 'Chậu Tắm -Cho Bé Notoro Plus INOCHI Thiết Kế Rộng Có Tay Nắm Giúp Bé Thoải Mái tắm THAUTAM', 'Mã SP: HIN.TTBE.NOPL
Kích thước: 863x524x238 mm
Khối lượng: 1531 g
Chất liệu: Nhựa PP (Polypropylen) nguyên sinh, hạt màu, phụ gia kháng khuẩn (Ag+)
Màu sắc: Thân trắng ngọc + tay nắm/tựa tắm hồng nhạt/xanh nhạt/xanh bạc hà', 250000, 25),
       (2,
        'Bút bi nước viết êm tay không lem mực ra tay, bút bi giá rẻ sẵn 3 màu đen xanh đỏ, đồ dùng văn phòng phẩm cho học sinh',
        'Bút bi nước văn phòng 0.5mm mực đều, nét chữ đẹp ĐỦ MÀU XANH, ĐEN, ĐỎ
Số lượng: 1 chiếc
Bút bi nước luôn là bạn đồng hành cho mọi người
- bút viết không lem mực mực ra đều
- hàng bán chạy nhất nhiều lăm qua
- có 3 mầu cho các bạn lựa chọn xanh đen đỏ', 2000, 500),
       (2, 'BÚT BI MỰC NƯỚC ĐỒ DÙNG HỌC SINH', 'bút bi là do con người hiện đại mới phát minh ra,thay thế cho bút mực,bút lông gà bút nghiên
ngày nay hầu hết các bạn học sinh,sinh viên trong túi đều có vài ba cậy bút bi,bút nước
bút rất tiện sử dụng và vô cùng dễ viết. mực xuống đều cho và nét chữ rất xinh
bút có 3 mầu. xanh+đen+đỏ
', 2000, 500),
       (2, 'Bút gel nhám đơn giản đồ dùng học tập quà tặng cho học sinh', 'Thông tin sản phẩm: bút gel
Kích thước: 14,5 x 1 cm
Sử dụng chính: Văn phòng, văn phòng phẩm
Có xóa được không: Không
Các dịp sử dụng: học ở trường, kế hoạch hàng ngày, quà tặng, văn phòng
Khối lượng tịnh: 14 g
Ngòi bút: 0,38mm', 3000, 500),
       (2, 'Sáng bóng Bướm hoạt hình Bút gel 0,38mm Bút gel Bút mực đen Quà tặng Học sinh Đồ dùng trẻ em Dụng cụ viết', 'Bút Gel hoạt hình bướm
Kích thước: 20 * 3CM
Trọng lượng: 11g
Màu sắc: Xanh lá cây / Trắng / Hồng / Tím
Chất liệu: Nhựa ', 5000, 500),
       (2, 'Bút gel mực đen 0.5mm dành cho học sinh, sinh viên, đồ dùng văn phòng', 'Sẽ có một chút khác biệt về màu sắc do màn hình màu khác nhau, vui lòng tham khảo sản phẩm thực tế.
Chất liệu vỏ: nhựa
Kích thước: khoảng 14,5cm
Xóa được: không
Màu ruột: Đen
Đầu ngòi: 0,5mm', 7000, 500),
       (2, 'Bút bi mực nước giáng sinh văn phòng phẩm đồ dùng học tập làm quà noel N03', '- Loại sản phẩm: Bút bi mực nước
- Chất liệu: Nhựa.
- Ngòi viết: Ngòi 0.5mm mực đen.
- Màu sắc: Nhiều màu.
- Kích thước: 18cm.
- Trọng lượng: 10g.
- Qui cách đóng gói: lẻ 1 cây', 4000, 500),
       (2, 'Bút Chì Lò Xo Giáng Sinh, Đồ Dùng Làm Quà Tặng Trong Học Tập Mùa Noel Cho Học Sinh - BEYOU', 'Kích thước: 22.2cm
Trọng lượng: 8g
Chất liệu: gỗ tự nhiên
Phong cách : giáng sinh, noel
Mẫu: 6 mẫu ngẫu nhiên
Đặc tính: bút chì có lò xo kèm icon phong cách giáng sinh ngộ nghĩnh dễ thương', 4000, 500),
       (2, 'Hộp 10 bút chì gỗ hoạt hình con vật siêu cute nhiều màu sắc đồ dùng học tập- quà tặng học sinh', 'Bút chì được ứng dụng trong nhiều mục đích khác nhau trong lĩnh vực hội họa, nhà trường, kỳ khảo sát,...
Thân bút chì hình lục giác, thuận tiện cho việc cầm nắm và sử dụng.
Đầu chì than mềm, nét bút mượt mà.
Thân bút sử dụng sơn không độc.
Xuất xứ: Trung Quốc', 25000, 500),
       (2,
        '6 CÁI / Bộ Macaron Bút chì đôi Bút đánh dấu Nghệ thuật Bút vẽ Kawaii Kẹo Màu Manga Bút Gel Màu Bút lông Đầu mềm Văn phòng phẩm', 'Xuất xứ: CN (Xuất xứ)
Đầu cọ: Đầu tròn + đầu xiên
Bao bì: Bộ
Kích thước: Bình thường
Số lượng gói: 6 màu / hộp
Có thể xóa hoặc không: Không
Loại sản phẩm: Bút đánh dấu
Nhiều màu: Có
Sử dụng: Điểm đánh dấu văn phòng & trường học
Viết hai mặt: Có
Số mô hình: A558', 35000, 500),
       (2,
        'Bút xóa có thể thu vào dễ thương Bút xóa nhỏ dễ thương Cao su sáng tạo Văn phòng phẩm Học sinh Đồ dùng học tập', 'Phấn Nước có thể thu vào
Kích thước: 9.5 * 2.5cm
Màu: Hồng / Cam / Trắng / Xanh
Cao su công suất lớn, bền
Cao su có thể thu vào, tiện lợi và gọn gàng  ', 20000, 500),
       (2, '6 cái / 1 hộp Dễ thương Airman Phi công Anime Màu đen Báo chí Bút 0,5mm Bút Gel Văn phòng phẩm', 'Thông tin sản phẩm: bút đen
Chất liệu vỏ: nhựa
Thông số kỹ thuật: 14,3cm
Sử dụng cho: Văn phòng, văn phòng phẩm
Có xóa được không: không
Dành cho: học ở trường, kế hoạch hàng ngày, quà tặng, văn phòng
Thân bút: 14,3cm
Khối lượng tịnh: 6,7g
Refill màu: đen', 30000, 500),
       (2, 'Bút viết hoạt hình dễ thương kèm hộp siêu xinh tặng hai ngòi bút đồ dùng học sinh, quà lưu niệm', 'Bút viết hoạt hình dễ thương kèm hộp siêu xinh tặng hai ngòi bút đồ dùng học sinh, quà lưu niệm
Một set bao gồm hộp ngoài siêu dễ thương, bút mực viết êm tay tặng kèm 2 ngòi bút và một trang sticker cho các bạn thoải mái sáng tạo
Kích thước:15*2.4cm
Xuất xứ: Trung Quốc', 20000, 500),
       (2,
        'Flashcard trắng hình giáng sinh, thẻ học từ vựng tiếng anh dùng để ghi chú, ghi chép, đồ dùng học tập làm quà noel N019', '-Loại sản phẩm: Thẻ Flashcard
-Chất liệu: Giấy
- Số trang: 80 trang
- Màu Sắc : Nhiều màu
- Kích thước: 7 x 7cm', 10000, 500),
       (2, 'Giấy Note Ghi Chú Trendy, Đồ Dùng Học Sinh Quà Tặng Sáng Tạo Giá Rẻ Bukao', 'Các loại giấy note mẫu cute đáng yêu vô vàn
Sản phẩm giá được sale giảm rẻ hơn từng sản phẩm bán riêng nên bạn chọn mẫu đúng theo phân loại, màu ngẫu nhiên không chọn nhé.
Tùy note sẽ có kích thước và số tờ khác nhau. Mẫu giá càng cao số tờ càng nhiều. Bạn có thể tham khảo thêm mô tả sp ở từng sản phẩm cụ thể đăng riêng.',
        10000, 500),
       (2, 'A5 bìa Kraft Sổ ghi chép Nhật ký Cổ điển Trống / lưới / lót Văn phòng phẩm Lập kế hoạch Văn phòng phẩm', 'Thương hiệu: Dot Ding
Xuất xứ: CN (Xuất xứ)
Số mô hình: A431
Màu: Kaki, đen
Chất liệu: Giấy + bìa giấy Kraft
Trọng lượng: 100g
Giấy: 80 GSM
Kích thước: A5: 14 * 21cm', 20000, 500),
       (2, '(combo 10 Quyển) Sổ Ghi Nhanh Mix Mẫu Không Trùng Đồ Dùng Học Sinh Quà Tặng Dễ Thương Bukao', 'combo 10 sổ tay mix mẫu các loại sổ ghi nhanh không trùng
Giấy trắng, kẻ ngang
Khoảng 40 trang', 30000, 500),
       (2,
        'Giấy note hình chữ nhật hoạt hình cute, giấy ghi chú dễ thương đồ dùng học tập làm quà tặng học sinh giá rẻ GN02', '- Loại sản phẩm: Giấy ghi chú, giấy note.
- Chất liệu: Giấy cao cấp
- Số trang: 20 tờ
- Kích thước: 7,4cm * 5cm
- Màu Sắc: Nhiều màu
- Trọng lượng: 20g', 3000, 500),
       (2, 'Sổ lò xo, sổ tay ghi chép cute chủ đề giáng sinh văn phòng phẩm đồ dùng học tập làm quà noel N020', '- Loại sản phẩm: Sổ lò xo, sổ ghi chép, sổ tay cute.
- Chất liệu: Giấy cao cấp có dòng kẻ
- Kích thước: 10,5 x 8,5cm
- Số trang: 80 trang.
- Màu Sắc : Nhiều màu
- Trọng lượng: 50g
', 9000, 500),
       (2, 'Giấy note hình động vật cute, giấy ghi chú dễ thương 20 tờ đồ dùng học tập làm quà tặng học sinh giá rẻ',
        'Gôm các mẫu được mix với nhau làm giấy ghi chú cho các con or các cô được nhiều hơn. ', 2000, 500),
       (2, 'Sổ Tay#Đồ Dùng Học Sinh#Cuốn Sách phe đối lập', 'Vải bên trong: giấy chất lượng cao
Trọng lượng giấy bên trong: 70 (g)
Kích thước giấy bên trong: 206 * 140
Các mặt hàng bao gồm: Không có
Áp dụng các dịp tặng quà: đám cưới, sinh nhật, rằm, kỷ niệm du lịch, tốt nghiệp, tân gia, họp mặt dự tiệc, viếng thăm và chia buồn
In LOGO: Có
Xử lý tùy chỉnh: Có
Đặc điểm kỹ thuật: A5', 50000, 500),
       (2, 'Sổ tay học ghi chú từ vựng đồ dùng học tập cho học sinh sổ ghi chép GN19', 'Sổ tay học ghi chú từ vựng đồ dùng học tập cho học sinh sổ ghi chép
Thời gian giao hàng 2-4 ngày
Số tờ 50 tờ
Kích thước :7-8.5x9 cm', 10000, 500),
       (2, 'Túi đựng bút chất liệu trong suốt phong cách cá tính', 'Túi đựng bút
Kích thước: Hình chữ nhật: 20.5 * 12CM, Hình tam giác: 20.5 * 9CM
Màu sắc: Đen, Trắng, Xanh, Hồng, Xanh lá, Xám
Chất liệu: Nylon
Màu sắc & Kích thước có thể có sự khác biệt nhỏ do độ phân giải, độ sáng, độ tương phản của màn hình máy tính, các phương pháp đo lường, v.v. Chúng tôi hy vọng bạn có thể thông cảm.',
        15000, 500),
       (2, 'Túi Đựng Bút Phong Cách Hàn Quốc H54', 'Sử dụng: Quà tặng
Mới lạ: Có
Model số: H54
Loại sản phẩm: Túi đựng bút chì
Chất liệu: Vải', 10000, 500),
       (2, 'Túi Đựng Bút/ Mỹ Phẩm Bằng Vải Canvas Trơn Siêu Bền Phong Cách Vintage', 'Có thể chứa khoảng 30 bút
Kích thước: 20,5 * 8 * 5cm', 45000, 500),
       (2, 'Túi đựng bút canvas màu trơn kích thước lớn tiện lợi', 'Màu sắc: hồng, xanh lục, vàng, be, xám đậm, xanh nước biển
Khối lượng tịnh: 60 gram
Đặc điểm chi tiết: 20 * 8cm
Dành cho: Mọi người
Độ cứng: mềm mại
Chất liệu: Vải canvas
Kiểu dáng: hình vuông', 25000, 500),
       (2, 'Túi đựng bút vải canvas sức chứa lớn in họa tiết gấu hoạt hình đáng yêu tiện lợi', 'Chất liệu: Vải canvas
Kích thước: 20 x 7 x 7 m
Phong cách: Đóng kín
Phạm vi sử dụng: Thông thường
Khối lượng sản phẩm: xấp xỉ 30g', 20000, 500),
       (2, 'Túi Đựng Bút Trong Suốt Họa Tiết Dễ Thương', 'Chất liệu: PVC
Trọng lượng: 72g
Chiều dài: 20 * 12 * 8cm
Họa tiết: Dễ Thương
Màu sắc: Trong suốt
Gói hàng bao gồm: 1 chiếc', 25000, 500),
       (2, 'Túi đựng bút bằng vải canvas cỡ lớn in hình hoạt hình xinh xắn đáng yêu', 'Chất liệu: Vải canvas
Mã số sản phẩm: YT2481
Khối lượng:
Chiều dài * Chiều rộng * Chiều cao: 17 * 6.5 * 5cm / 6.7 * 2.5 * 1.9in
Vui lòng cho phép sai số 0.5-1 inch về số đo do cách đo lường thủ công. ', 17000, 500),
       (2, 'Túi Đựng Bút CARO Phong Cách HÀN QUỐC Siêu Đẹp', 'Kích thước(Dài x Rộng x Cao):: 18.5*4*5.5cm
Chất liệu: DAPU
Màu sắc: CARO viền trắng, nền đen, xám.
Chất liêu: DAPU, không thấm nước, khóa kéo,đường may đẹp,chắc chắn.
ĐẶC ĐIỂM NỔI BẬT :
Màu sắc cực đẹp, thích hợp cho những bạn thích phong cách tối giản.
Chất liệu không thấm nước,
Khóa kéo trắng, đường may đẹp,chắc chắn.
Dùng để bỏ bút , viết, tùy theo nhu cầu của người dùng.', 15000, 500),
       (2, 'Túi đựng bút trong suốt có khóa kéo họa tiết xinh xắn đáng yêu tiện lợi', 'Chất liệu: PVC
Mã số sản phẩm: YT2478
Kích thước:
Chiều dài * Chiều rộng * Chiều cao: 20*3.5*8 cm/ 7.9*1.4*3.1in
Vui lòng cho phép sai số kích thước 0.5-1inch do đo lường thủ công khác nhau.', 20000, 500),
       (3, 'Dây sạc Foxcom 8 lõi chân USB-A', '- Cáp lightning hàng CHÍNH HÃNG Foxconn
- Cáp Foxconn cao cáp: 8ic ( lõi 8 sợi cực kì bền bỉ)
- Dài tiêu chuẩn 1m
- Sử dụng chíp chính hãng, có mã hàng của Foxconn trên dây
-  Bao xài, restore70', 165000, 100)
        ,
       (3, 'Dây Cáp Sạc X14 Cho ĐT', '- Tên sản phẩm: Cáp  X14
- Model:  X14
- Chiều dài cáp: 1 Mét & 2 Mét
- Dòng sạc tối đa: 5V-2.4A
- Chất liệu: Dây dù, lõi đồng, hợp kim nhôm
- Cáp Sạc X14 Chính Hãng  Bảo Hành 3 Tháng
-Tương thích được cho các sản phẩm ip từ 5-13promax', 30000, 100)
        ,
       (3, 'Dây sạc 3 đầu sạc nhanh nguồn điện 3A JUYUPU TS30 vải dù chống rối cao cấp chính hãng', 'Model : TS-30
Chiều dài : 1m
Chứng nhận chất lượng: ce , RoHS, FCC, MSDS
Dòng điện : 3A
Chức năng: Sạc nhanh 3A
Tốc độ truyền dữ liệu: 480MbpA
Lõi đồng nguyên chất 100% không pha tạp chất
Với 3 lớp bọc : bọc thép, bọc lưới, nhựa bảo vệ
Chất liệu ngoài : bộc dù cao cấp
Tương thích với các thiết bị mới hiện nay như: iPhone Samsung OPPO Vivo HUAWEI XIAOMi
Cổng cấm : USB
Chuôi sạc : 3 in 1 gồm có iPhone , Micro, Type C', 90000, 100)
        ,
       (3, 'Dây sạc micro samsung bọc dù, cáp sạc cho các dòng xiaomi, oppo, vivo dài 1m chất lượng cao - KLH Shop', '- Cáp sạc chân micro
- Toàn thân bọc dù tăng cường độ bền
- Độ dài dây 1m
- dùng chung các loại củ sạc tới 3A
- Màu sắc nhiều màu
- Hỗ trợ các dòng máy samsung, oppo, vivo, xiaomi ...
- Sản phẩm được nhập khẩu từ Trung Quốc', 15000, 100)
        ,
       (3, 'Dây Sạc Cáp Sạc nhanh Hoco X14 Cho Điện Thoại Sạc Dự Phòng Tai nghe Bluetooth cho 6/6s->13pm', '- Tên sản phẩm: Cáp Hoco X14_x000D_
- Model: Hoco X14_x000D_
- Chiều dài cáp: 1 Mét _x000D_
- Dòng sạc tối đa: 5V-2.4A_x000D_
- Chất liệu: Dây dù, lõi đồng, hợp kim nhôm_x000D_
- Dòng máy tương thích: dòng 6s trở lên
-  Cáp Sạc X14 Chính Hãng Hoco Bảo Hành 3 Tháng_x000D_', 50000, 100)
        ,
       (3, 'Dây cáp sạc nhanh Baseus 2.4A cho IPhone 13 pro 12 Xs Max Xr X 8 7 Plus', 'Chất liệu: Nhôm + Dây nylon bện dày
Màu sắc: Đen, Đỏ
Chức năng: Cáp Lightning truyền dữ liệu & Sạc nhanh cho iPhone
Loại sản phẩm: 8Pin
Bao bì bán lẻ: Có
Thương hiệu tương thích: iPhone của Apple
Đặc điểm: Có thể đảo ngược
Chiều dài/Đầu ra: 0,5m / 1m: Tối đa 2,4A, 2m: Tối đa 1.5A ', 120000, 100)
        ,
       (3, 'Dây cáp sạc IPTOPK AP15 20W dạng bện sợi Nylon chuyển đổi USB C sang cổng cho IP12 xs 11 X 8 7 6 5', 'Hỗ trợ sạc PD: Sạc thiết bị của bạn nhanh hơn và an toàn hơn. Bạn có thể sử dụng cáp Lightning này kết nối với sạc dự phòng USB-C hoặc ổ sạc USB-C để sạc thiết bị iOS của bạn
Sạc và đồng bộ hóa: Kết nối iPhone, iPad hoặc iPod của bạn bằng đầu nối với máy Mac hỗ trợ USB-C hoặc Thunderbolt 3 (USB-C) để đồng bộ hóa và sạc, tốc độ truyền dữ liệu lên đến 480Mbps.',
        70000, 100)
        ,
       (3, 'DÂY SẠC DÀI 3M TIỆN ÍCH', 'Nhà e cos đủ dây chân ip và chân samsung , oppp
Thấy dân tình sốt dây sạc iphone dài 3m thế này
Sạc cực nhanh, dây cực dài lại chống rồi nữa ý. Thik cứ gọi là mê
Em quẩy 1 ít về bán luôn ', 30000, 100)
        ,
       (3, 'Dây cáp sạc ĐT ngắn gọn cổng micro usb cho IP và android dài 20cm', ' Dây Cáp Sạc cao su nhám mịn, chân tiếp súc đồng vàng .
+ Không gây loạn cảm ứng.
+ Không làm chậm thời gian sạc pin.
+ Lý do nên mua: Sạc đúng dung lượng pin, không làm lệch cảm ứng như sạc thường. Đáp ứng tiêu chuẩn an toàn sản xuất nên không bị rò rỉ điện gây giật điện.
+ Tăng tuổi thọ pin và cảm ứng, an toàn cho người sử dụng.  ', 25000, 100)
        ,
       (3, 'Dây sạc điện thoại Đa Năng 3 Đầu Sạc nhanh Dây Rút Siêu Bền Gọn Nhẹ Tiện Lợi', 'Cáp Đa Năng 3 Đầu Sạc Dây Rút YT 3 In 1 Sạc Nhanh 3A Micro USB, Type-C và lightning Siêu Bền Gọn Nhẹ Tiện Lợi
Sạc nhanh. tiện lợi.
nhiều khách hàng tinh tưởng.', 20000, 100)
        ,
       (3, 'Ốp lưng iphone trong lỗ camera bear.bri.ck', '- Ốp lưng được đóng gói bằng túi nilon thiết kế đẹp.
- Mực in chất lượng cao,sắc nét, không phai màu, không gây hại cho da,
- Hình ảnh thiết kế đẹp, phong cách, trẻ trung.
- Công dụng: thay đổi màu sắc cho điện thoại, giữ điện thoại chắc chắn trên tay, an toàn chống trầy xước,  bảo vệ chiếc ốp khỏi va đập.
Hướng dẫn sử dụng sản phẩm Ốp lưng iphone 5/5s/6/6s/6plus/6splus/7/8/7plus/8plus/x/xs/xs max/11/11pro max.
- Không nên để ốp lưng, bao da dưới sàn nhà.
- Để nơi thoáng mát sẽ giúp bảo quản.
- Để xa tầm tay trẻ em.
- Xuất xứ: Việt Nam', 10000, 100)
        ,
       (3, 'Ốp lưng iphone TPU cạnh vuông tim bóng', '- Chất liệu:  Viền nhựa dẻo , mặt nhám
- Màu sắc: Nhiều Màu.
- Ốp lưng được đóng gói bằng túi nilon thiết kế đẹp.
- Hình ảnh chất lượng cao,sắc nét, không phai màu.
- Hình ảnh thiết kế đẹp, phong cách, trẻ trung.
- Công dụng: Là phụ kiện thời trang, thay đổi màu sắc cho điện thoại, giữ điện thoại chắc chắn trên tay, an toàn chống trầy xước,  bảo vệ chiếc điện thoại khỏi va đập.',
        10000, 100)
        ,
       (3, 'Ốp dẻo silicon hình điện thoại Nokia cổ', 'Tên sản phẩm: Ốp lưng iphone silicon
•    Mô tả sản phẩm:
•     Thiết kế: Bo khít máy, bảo vệ tối đa
•    Chất liệu: nhựa dẻo TPU dày dặn ít bám bẩn', 10000, 100)
        ,
       (3, 'Ốp lưng iphone cạnh vuông lỗ camera gấu cute', '- Ốp lưng được đóng gói bằng túi nilon thiết kế đẹp.
- Mực in chất lượng cao,sắc nét, không phai màu, không gây hại cho da,
- Hình ảnh thiết kế đẹp, phong cách, trẻ trung.
- Công dụng: thay đổi màu sắc cho điện thoại, giữ điện thoại chắc chắn trên tay, an toàn chống trầy xước,  bảo vệ chiếc ốp khỏi va đập.
Hướng dẫn sử dụng sản phẩm Ốp lưng iphone 5/5s/6/6s/6plus/6splus/7/8/7plus/8plus/x/xs/xs max/11/11pro max.
- Không nên để ốp lưng, bao da dưới sàn nhà.
- Để nơi thoáng mát sẽ giúp bảo quản.
- Để xa tầm tay trẻ em.
- Xuất xứ: Việt Nam', 26000, 100)
        ,
       (3, 'Ốp lưng vuông cạnh in hình', '- Chất liệu: Nhựa dẻo
- Màu sắc: Nhiều Màu.
- Hình ảnh chất lượng cao,sắc nét, không phai màu.
- Hình ảnh thiết kế đẹp, phong cách, trẻ trung.', 10000, 100)
        ,
       (3, 'Ốp điện thoại dẻo nắp trượt bảo vệ camera màu kẹo ngọt dễ thương', '  Đặc điểm: Ốp điện thoại bảo vệ ống kính
  Kích thước: 5.8 Kích thước: 6.5 Kích thước: 5.5 Kích thước: 6.1 Kích thước: 4.7
  Thiết kế: Thiết kế mặt nhám Thiết kế: Màu trơn: Thiết kế: Trong suốt Thiết kế: Hoạt hình
  Đặc điểm: Chống bụi bẩn 、 Chống va đập 、 Bảo vệ chắc chắn
  Model 1: Ốp điện thoại bảo vệ ống kính máy ảnh
  Model 2: Ốp điện thoại mềm màu kẹo', 18000, 100)
        ,
       (3, 'Ốp Lưng Iphone Basic Camera In Hình New Hot', '- Chất liệu:  Viền nhựa dẻo , mặt nhám
- Màu sắc: Nhiều Màu.
- Ốp lưng được đóng gói bằng túi nilon thiết kế đẹp.
- Hình ảnh chất lượng cao,sắc nét, không phai màu.
- Hình ảnh thiết kế đẹp, phong cách, trẻ trung.
- Công dụng: Là phụ kiện thời trang, thay đổi màu sắc cho điện thoại, giữ điện thoại chắc chắn trên tay, an toàn chống trầy xước,  bảo vệ chiếc điện thoại khỏi va đập.',
        15000, 100)
        ,
       (3, 'Ốp lưng iphone trong lượn sóng tim mây', '- Ốp lưng được đóng gói bằng túi nilon thiết kế đẹp.
- Mực in chất lượng cao,sắc nét, không phai màu, không gây hại cho da,
- Hình ảnh thiết kế đẹp, phong cách, trẻ trung.
- Công dụng: thay đổi màu sắc cho điện thoại, giữ điện thoại chắc chắn trên tay, an toàn chống trầy xước,  bảo vệ chiếc ốp khỏi va đập.
Hướng dẫn sử dụng sản phẩm Ốp lưng iphone 5/5s/6/6s/6plus/6splus/7/8/7plus/8plus/x/xs/xs max/11/11pro max.
- Không nên để ốp lưng, bao da dưới sàn nhà.
- Để nơi thoáng mát sẽ giúp bảo quản.
- Để xa tầm tay trẻ em.
- Xuất xứ: Việt Nam', 15000, 100)
        ,
       (3, 'Ốp lưng iphone Water Color Mojito Cạnh Vuông BVC', '- Chất liệu: Nhựa dẻo
- Màu sắc: Nhiều Màu.
- Ốp lưng được đóng gói bằng túi nilon thiết kế đẹp.
- Hình ảnh chất lượng cao,sắc nét, không phai màu.
- Hình ảnh thiết kế đẹp, phong cách, trẻ trung.
- Công dụng: Là phụ kiện thời trang, thay đổi màu sắc cho điện thoại, giữ điện thoại chắc chắn trên tay, an toàn chống trầy xước,  bảo vệ chiếc điện thoại khỏi va đập.',
        15000, 100)
        ,
       (3, 'Ốp Lưng Iphone Basic Camera In Hình Siêu Cute Lưng Nhám', '- Chất liệu:  Viền nhựa dẻo , mặt nhám  chống vân tay
- Màu sắc: Nhiều Màu.
- Ốp lưng được đóng gói bằng túi nilon thiết kế đẹp.
- Hình ảnh chất lượng cao,sắc nét, không phai màu.
- Hình ảnh thiết kế đẹp, phong cách, trẻ trung.
- Công dụng: Là phụ kiện thời trang, thay đổi màu sắc cho điện thoại, giữ điện thoại chắc chắn trên tay, an toàn chống trầy xước,  bảo vệ chiếc điện thoại khỏi va đập.',
        15000, 100)
        ,
       (3, 'Ốp lưng iphone cạnh vuông lỗ camera gấu hoa', '- Ốp lưng được đóng gói bằng túi nilon thiết kế đẹp.
- Mực in chất lượng cao,sắc nét, không phai màu, không gây hại cho da,
- Hình ảnh thiết kế đẹp, phong cách, trẻ trung.
- Công dụng: thay đổi màu sắc cho điện thoại, giữ điện thoại chắc chắn trên tay, an toàn chống trầy xước,  bảo vệ chiếc ốp khỏi va đập.
Hướng dẫn sử dụng sản phẩm Ốp lưng iphone 5/5s/6/6s/6plus/6splus/7/8/7plus/8plus/x/xs/xs max/11/11pro max.
- Không nên để ốp lưng, bao da dưới sàn nhà.
- Để nơi thoáng mát sẽ giúp bảo quản.
- Để xa tầm tay trẻ em.
- Xuất xứ: Việt Nam', 15000, 100)
        ,
       (3, 'Ốp điện thoại gương chiếc lá chống sốc 4 góc tpu', '- Ốp lưng được đóng gói bằng túi nilon thiết kế đẹp.
- Mực in chất lượng cao,sắc nét, không phai màu, không gây hại cho da,
- Hình ảnh thiết kế đẹp, phong cách, trẻ trung.
- Công dụng: thay đổi màu sắc cho điện thoại, giữ điện thoại chắc chắn trên tay, an toàn chống trầy xước,  bảo vệ chiếc ốp khỏi va đập.
Hướng dẫn sử dụng sản phẩm Ốp lưng iphone 5/5s/6/6s/6plus/6splus/7/8/7plus/8plus/x/xs/xs max/11/11pro max.
- Không nên để ốp lưng, bao da dưới sàn nhà.
- Để nơi thoáng mát sẽ giúp bảo quản.
- Để xa tầm tay trẻ em.
- Xuất xứ: Việt Nam', 20000, 100)
        ,
       (3, 'Giá đỡ điện thoại bằng kim loại HÌNH NGƯỜI cho điện thoại cao cấp', 'Material: Iron
Color: White, Pink
Car Size: 7.5*7*5 cm', 25000, 100)
        ,
       (3, 'Giá đỡ điện thoại hình ghế đẩu có thể gấp gọn để bàn đa chức năng nhiều màu', '- Chất liệu : Nhựa cứng
- Xuất xứ : Trung Quốc
- Màu sắc : Nhiều màu sắc
- Kiểu dáng : Thiết kế độc lạ
- Tính năng : Đỡ điện thoại để xem phim, nghe nhạc ...
- Kích thước : 6.5 x 5.5 x 11cm', 10000, 100)
        ,
       (3, 'Giá đỡ điện thoại Livestream, kẹp chống lưng 360 ,đế chân tròn để bàn, gia do tiện lợi đa năng siêu bền', '- Chất liệu: Kim Loại + Nhựa
- Kích thước đế: 12 x 2.5cm
- Kích thước giá đỡ: co giãn linh hoạt từ 6,5cm đến 9,5cm
- Thân giá đỡ: Cao tối thiểu 24cm, cao tối đa 38cm
- Màu sắc: Đen', 50000, 100)
        ,
       (3, 'Giá đỡ điện thoại ô tô đa năng treo điện thoại gương chiếu hậu xoay 360', 'Màu sản phẩm: xám, xanh lá
Kích thước sản phẩm: 185 * 120 * 75mm
Trọng lượng sản phẩm: 134g
Kích thước hộp: 62 * 36 * 42cm', 160000, 100)
        ,
       (3,
        'Giá Đỡ Điện Thoại Để Bàn Có Tăng Giảm Chiều Cao, Dễ Dàng Gấp Gọn, Chất Nhựa Chắc Bền, Để Được Cả Máy Tính Bảng',
        'Nếu không có máy tính để bàn hay laptop, ĐỪNG LO! Sản phẩm sẽ giúp bạn thoải mái học tập và làm việc mà không lo bị mỏi vai gáy khi phải cầm điện thoại, ipad. ',
        20000, 100)
        ,
       (3, 'Giá Đỡ Điện Thoại Hình Thú Bằng Gỗ Dễ Thương', '- Chất liệu: gỗ phun sơn tạo hình và chống mối mọt.
-Trọng lượng: 34g
-Xuất xứ: Trung Quốc', 10000, 100),
       (4, 'Set Đồ Nữ, Set Bộ Áo Thun Mặt Cười Mix Quần Kaki Ống Suông Hàng Loại 1', 'Chất liệu:
Màu Sắc : Đen, Xanh
Size: Free Size < 57kg', 100000, 20),
       (4, 'Bộ Thun Cộc Tay Vico Nữ', 'Bộ cộc tay in chữ Vico form rộng nữ
Set đồ mùa hè chất thun mỏng mát, mặc thoải mái , Freesize <65kg
Nhanh tay đặt hàng Set Thun In Chữ Vico để nhận nhiều ưu đãi của các nàng ơiii <3
Quần áo thun mỏng là item không thể thiếu của mỗi cô nàng dịp mùa hè', 90000, 20),
       (4, 'Bộ quần áo bóng đá CLB PSG mùa giải 2021 2022 - Quần áo đá banh mới nhất', 'Bộ quần áo bóng đá CLB PSG mùa giải 2021 2022 - Quần áo đá banh mới nhất
•    Chất liệu THUN LẠNH (Hàng Việt) & POLYESTER (Hàng Thái) cao cấp
•    KHÔNG NHĂN – KHÔNG XÙ – KHÔNG PHAI
•    Thấm hút mồ hôi cực tốt
•    Thiết kế mạnh mẽ,  hiện đại', 100000, 20),
       (4, 'Bộ quần áo bóng đá, đá bóng đồ đá banh VIỆT NAM ĐỎ & TRẮNG mới nhất', 'Chất lượng sản phẩm Top 1 thị trường.
phiên bản mới nhất
Size: M, L, XL
vải áo mềm mịn, thoáng mát.
cực kì thấm mồ hôi.
logo thêu chắc chắn.', 100000, 20),
       (4, 'Bộ quần áo Bóng đá CLB PSG 2021-2022 DÀI TAY', 'Siêu đẹp và rẻ nhé các bạn !
Hàng có sẵn tại shop: Hàng thun lạnh, Co giãn 4 chiều,thấm mồ hôi cho anh em thoải mái vận động
Size Chiều cao.    Cân nặng
S.  1m50 - 1m64. 48kg - 53kg
M. 1m65 - 1m72. 54kg -  59kg
L.  1m70 - 1m75. 60kg -  70kg
XL. 1m75 trở lên - 70kg - 80kg', 100000, 20),
       (4, 'Quần Dài Thể Thao hip hop In Họa Tiết graffiti Mẫu Mới Dành Cho Nam Size M-5XL 2022', 'Độ dày: Chung
Kích thước: M -5XL
Thành phần nguyên liệu: Polyester
Chiều dài tay áo: Tay ngắn
Loại cổ áo: cổ tròn
Chi tiết phong cách: in
Loại tay áo: tay ngắn
Phiên bản: Dáng rộng
Mùa thích hợp: mùa hè
Thời gian đưa ra thị trường: 2022
Kịch bản này: Hàng ngày
Đối tượng: Thanh niên
Phong cách cơ bản: thời trang trẻ
Phong cách phân khu: nhỏ và tươi', 100000, 20),
       (4, 'Quần Áo Thom Viền Sọc Trắng - Bộ Quần Áo Thome Nam Siêu Đẹp', 'Bề mặt mềm mịn, thông thoáng, co dãn giúp giảm nhiệt cực nhanh.
Độ dày vừa phải cùng đường may tỉ mỉ đảm bảo giữ phom dáng, bền màu sau nhiều lần giặt
Họa tiết, logo in nhiệt dễ giặt, mau khô, không lo bong tróc
Kiểu dáng thời trang phù hợp nhiều hoàn cảnh: mặc nhà, đi học, đi chơi, du lịch, thể thao, làm quà tặng...
Đủ size, Đủ ảnh, Video thực tế, Đảm bảo sản phẩm y hình 100%', 220000, 20),
       (4, 'Bộ Quần Áo Mặc Nhà Thể Thao Nam Mùa Hè Phong Cách Cao Cấp ZERO', 'Chất liệu: Poly cá sấu
Màu sắc: Đen - Xanh than - Xám - Be - Xanh dương
Thương hiệu: ZERO
Xuất xứ: Việt Nam
Size: M - L- XL - XXL', 200000, 20),
       (4, 'Đồ bộ nam STYLE MARVEN áo thun in hình Stitch và quần shorts đùi kẻ ngang - TOP 182 + SHORTS 16 B', 'Mã sản phẩm: TOP 182 + SHORTS 16 B
Xuất xứ: Việt Nam
Chất liệu: Set quần áo nam sử dụng vải thun trơn mềm mại, co giãn nhẹ thấm hút mồ hôi, mặc thoáng mát
Thiết kế: Áo thun form suông, cổ tròn, tay ngắn basic in hình Stitch và quần đùi sọc ngang dễ mặc dễ phối đồ
Kiểu dáng: Trẻ trung, năng động, cá tính, mẫu mã độc đáo hot hit
Phù hợp nhiều hoàn cảnh: mặc đi học, đi chơi, đi du lịch, mặc hội nhóm siêu đẹp', 100000, 20),
       (4, 'SET VÁY YẾM KAKI BE + ÁO THUN TRẮNG - BỘ ĐẦM 2 DÂY BẢN TO MÀU BE DÁNG DÀI ULZZANG', 'Kiểu dáng: yếm 2 dây bản to chắc chắn, form dài . Mặc cả set này là độ cute tăng lên 1000% luôn ạ ^^
Chất liệu: yếm kaki dày dặn  (shop có bán thêm áo trắng tay cộc chất thun mềm làm set nha)
Form : yếm dài 107cm, v1, v2 từ 60- 90cm, v3 từ 70- 120cm. Tầm dưới 60-65kg đổ lại mặc xinh nha ', 115000, 20),
       (4, 'Sét áo SILENCE +quần ống rộng caro LN12', 'Áo chất cotton su màu trắng in silence và quần ống rộng caro
Sản xuất tại: phuongmyt 0989401395
Freesize
nữ nam 40-60 kg 1m67 về mặc được ', 80000, 20),
       (4,
        'Sét Bộ Đồ Quần Áo Nữ Đón Hè In 3D Cartoon Hoạt Hình Khủng Long Cánh Mặc Ở Nhà - Đi Chơi Phối Quần Thun Bo Gấu Siêu Xinh', 'Kích thước: S , L, M
- Size S: 38kg-44kg
- Size M: 45kg-50kg
- Size L: 51kg-54kg
Chất liệu: Áo Cotton Organic, Quần Thun Sọc Co Giãn', 85000, 20),
       (4, 'Sét áo phông Khủng long good + quần bom, Đồ UNISEX mặc ở nhà và đi chơi, chất đẹp, mặc không xù - FANI', '- Áo chất liệu: vải thun chất tici
- Quần suông kết hợp với áo tạo sét trang phục cá tính
- Thiết kế thời trang, phù hợp mặc được nhiều hoàn cảnh như: đi học, đi chơi,..
- Có thể kết hợp được với nhiều loại quần khác nhau như: quần jeam, quần suông ống rộng, quần bo gấu...tạo ra những sét trang phục cá tính, đặc sắc tùy người mặc.
- Mẫu free size có thể mặc được cho cả nam và nữa từ 35 - 55kg, cao 1m68 đổ về ', 120000, 20),
       (4, 'Sét đồ nữ 3 Món Gồm Áo Gile + Áo Thun Form Rộng Tay Lỡ + Quần Short Xám Siêu Hot Hit Unisex', '-  Chất liệu: áo thun cotton co dãn 4 chiều, unisex tay lỡ nhé.
- Còn về quần: lưng thun, có may 2 túi, chất liệu vải thun thể thao không pha.
Sét đồ nữ 3 Món Gồm Áo Gile + Áo Thun Form Rộng Tay Lỡ + Quần Short Xám Siêu Hot Hit Unisex
- Bảng Size: Quần form châu Âu rộng rộng nhé các cậu.
 + Quần: Freesize 40 đến 65kg vừa
  + Rộng v2 55cm - 90cm
 + Rộng vòng 3 < 102cm đổ lại
 + Tầm 40-65kg đổ lại mặc quá hợp lí luôn.', 110000, 20),
       (4, 'Giày Đạp Gót NY Thể Thao Độn 4cm Siêu Hot', ' -Chất liệu: vải canvas mềm
 -Màu sắc: Đen/kem
 -Kích thước: 35,36,37,38,39
 -Chất liệu đế: Cao su lưu hóa
 -Hình dạng của mũi giày: Mũi tròn
 -Chất keo: ép nhiệt quanh giày (có thể sử dụng đi trời mưa, không lo bong, hở keo)', 85000, 20),
       (4, 'Dép nữ hở mũi dây buộc nhiều màu JP0721 quai vải canvas siêu bền', '- Giày đạp gót mũi thủng cao cấp từ Taobao
- Đế chắc chắn dày dặn như mẫu CV
- Quai canvas 3 màu bắt mắt
- Dây chỉnh phía trên quai
- Tăng 4cm chiều cao, tiện dụng vô cùng
- Thoải mái và dễ thương cho bạn nữ', 97000, 20),
       (4, 'Giày Sandal Giày Nữ Quai Ngang Nữ Giày Quai Hậu Đế Bằng Nữ Phong Cách Nữ Sinh Ngọt Ngào',
        'Shop cam kết cả về chất liệu cũng như hình dáng (đúng với những gì được nêu bật trong phẩn mô tả sản phẩm). Nếu khách hàng có nhu cầu, Shop sẽ yêu cầu đơn vị vận chuyển cho khách kiểm tra hàng : về mẫu mã, size trước khi lấy để tạo sự thoải mái nhất cho khách hàn',
        55000, 20),
       (4, 'Giày (Dép) Sandal nữ xỏ ngón LCS40 chiến binh Y đế bệt, quai chéo', '• Kiểu dáng: basic
• Chất liệu: PU
• Màu sắc: Kem, Đen
• Size: 35, 36, 37, 38 & 39 (chat với shop để tư vấn chuẩn size)', 70000, 20),
       (4, 'Dép guốc xỏ ngón quai mảnh đan chéo đế trụ 4p cực đẹp', 'Hàng đẹp có đóng kèm hộp đựng chắc chăn
Chất liệu da loại 1 bao bền đẹp
Size 35-39
( 225 = size 35, 230=Size 36, 235=size 37, 240=size 38, 245 =sz39,250=size 40)', 100000, 20),
       (4, 'Giày MC Queen LA Nam Nữ, Giày MLB LA Hàng Đẹp Full Box Bill', 'Thương Hiệu : No Brand
Sản Xuất Tại : Việt Nam
Thông Số Kích Thước : 36-37-38-39-40-41-42-43
Chất Liệu : Cao Su ...
Nguồn Gốc Xuất Xứ : Việt Nam', 370000, 20),
       (4, 'Giày thể thao nữ UT22 đế cao 5cm - Giày sneaker nữ ulzzang độn đế Hàn Quốc 2022', 'Tên sản phẩm: Giày thể thao thời trang nữ
Khối lượng: 450gr/đôi
Nhập khẩu: CÔNG TY TNHH MTV THƯƠNG MẠI - DỊCH VỤ - XNK DƯƠNG TOÀN PHÁT
Địa chỉ nhập khẩu: TP Móng Cái – Quảng Ninh – Việt Nam
Xuất xứ: Nội địa Trung Quốc', 180000, 20),
       (4, 'Giày Sneaker Xanh Ngọc JD1 Màu Dễ Phối Đồ Giày Hàng Cao Cấp Full Box Bill', 'Thương Hiệu : No Brand
Sản Xuất Tại : Việt Nam
Thông Số Kích Thước : 36-37-38-39-40-41-42-43
Chất Liệu : Da, Cao Su ...
Nguồn Gốc Xuất Xứ : Việt Nam', 250000, 20),
       (4, 'Giày quai hậu ĐINH DA LỘN', 'Hàng đẹp có đóng kèm hộp đựng chắc chăn
Chất liệu da loại 1 bao bền đẹp
Size 35-39
( 225 = size 35, 230=Size 36, 235=size 37, 240=size 38, 245 =sz39,250=size 40)', 70000, 20),
       (4, 'Giày thể thao đế dày tăng chiều cao sành điệu năm 2021 năng động cho nữ', 'Tên sản phẩm: giày thể thao thông thường
Chất liệu mặt trên: lưới + da PU
Chiều cao gót: gót bằng, tăng chiều cao bên trong', 250000, 20),
       (4, 'Giày Thể Thao Nữ Thời Trang JUNO 5cm Sneaker Love Shack TT05009', 'Mã sản phẩm: TT05009
Kiểu dáng: Giày sneaker
Chất liệu: PU - Polyester
Độ cao: 5cm
Màu sắc: Kem-Trắng-Hồng-Xanh bạc hà
Kích cỡ: 35-36-37-38-39
Xuất xứ: Việt Nam', 999000, 20),
       (4, 'Giày Trắng Đế Dày Phong Cách Thời Trang Mùa Xuân Kiểu Mới Cho Nữ Sinh Dễ Phối Đồ', 'Chất liệu trên: da tổng hợp
Chất liệu đế: cao su
Họa tiết: màu trơn
Phương thức đóng: ren
Phong cách: Hàn Quốc
Mùa niêm yết: Mùa xuân năm 2022
Quy trình sản xuất: ép phun
Chiều cao ống cao: đỉnh thấp
Chất liệu bên trong: gai dầu
Chất liệu đế: PU
Kiểu gót: đáy dày
Tác dụng: chống trượt
Các yếu tố phổ biến: dây đai chéo
Đối tượng áp dụng: Thanh niên (18-40 tuổi)
', 200000, 20),
       (4, 'Dép quai ngang nam nữ bánh mì đế đúc cực đẹp mẫu mới hót tredd 2021',
        'Dép nữ đế cao 4cm, dép quai ngang chất liệu Eva cao cấp không thấm nước, size từ 36-41 hottrend 2022', 120000,
        20),
       (4, 'DÉP SU TRÀ SỮA ĐẾ BÁNH MÌ ĐI SIÊU ÊM / dép đi trong nhà', 'Dép su gấu đế cao 3cm
chất su xịn đi cực êm chân ạ
DÉP FORM NHỎ NÊN LẤY LÊN 1 SIZE ', 60000, 20),
       (4, 'Dép Nơ Xoắn Thời Trang Nữ', 'Dép đế độn với thiết kế dây chéo thêm nơ xoắn nhỏ tạo sự bắt mắt cho sản phẩm.
Dép 34-39.
Dép có 3 màu đen, bee, xanh. ', 180000, 20),
       (4, 'Giày Sneaker Basic Phong Cách Năng Động', 'Hàng order, KO CÓ SẴN
Hàng order taobao khoảng 20 ngày, ko cọc nên mọi người suy nghĩ kĩ trước khi đặt, đừng hủy đơn giúp em
Đảm bảo hành order taobao 100%, ko phải VNXK
Size từ 35 tới 40', 190000, 20),
       (4, '(HÀNG ORD, KHÔNG SẴN )Giày 03', 'GOM ORDER 2-3 tuần
Size: 35-40', 230000, 20),
       (4, 'Thắt lưng nam cao cấp Vicenzo', 'Chất liệu da :
- Da mềm mại, sang trọng và có thời gian sử dụng 5 - 7 năm
- Dây mai viền đẹp tỉ mỉ không lo bong tróc
Thiết kế tinh tế, nam tính:
- Mặt khoá hợp kim, hoạ tiết cát nhám mịn
- Màu sắc tinh tế và sang trọng, dễ dàng phối đồ
- Kiểu dáng khóa tự động, dễ dàng tháo ra cắt ngắn theo vòng eo, kích thước 3.6x120 cm', 130000, 20),
       (4, 'Thắt lưng nam da bò thật TRITA RTL034', '- Kích thước:  Dài 126cm, Rộng 3.5cm
- Kiểu khóa: Khóa cài tự động.
- Chất liệu: Da bò thật cao cấp.
- Màu sắc: Nâu, Đen
- Loại vân: Vân 7, Vân 15.
- Trọng lượng: 97g
- Xuất xứ: Việt Nam
- Bảo hành: 365 ngày ', 500000, 20),
       (4, 'Thắt lưng da nam khóa kim loại IVY moda MS 35E3121', 'Kích thước: Bản dây 4cm (thông dụng) Chiều dài khoảng 125 cm
Màu sắc: Đen
Dòng sản phẩm	Accessories
Nhóm sản phẩm	Thắt lưng
Kiểu dáng	Có mặt
Chất liệu	Da thật', 900000, 20),
       (4, 'Thắt lưng da nam HIGHWAY MENSWEAR TL0064', '- Chất liệu: Da bò
- Màu sắc: Đen
Sản phẩm được sản xuất và thiết kế độc quyền từ thương hiệu thời trang nam Highway Menswear', 330000, 20),
       (4, 'Thắt lưng da nam IVY moda MS 35E3122', 'Kích thước: Bản dây 4cm (thông dụng) Chiều dài khoảng 125 cm
Màu sắc: Đen
Dòng sản phẩm	Accessories
Nhóm sản phẩm	Thắt lưng
Kiểu dáng	Có mặt
Chất liệu	Da thật', 900000, 20),
       (4, 'THẮT LƯNG 2 MẶT PED.RO',
        'Set Pedro cao cấp: Bộ thắt lưng nam tặng kèm box hãng xịn, túi xách giấy sang trọng', 150000, 20),
       (4, 'Thắt Lưng Da Bò Cao Cấp SMARTMEN DLM-01', 'Khóa kim loại sáng mịn,bắt mắt
Khóa dây lưng thiết kế tinh tế,màu sắc nổi bật kết hợp cùng chất da bò bền bỉ tạo nên sự khỏe khắn và nam tính
Kiểu dáng đơn giản dễ dàng kết hợp cùng các set đồ công sở hàng ngày
Màu sắc : Đen
Kích thước:  3.4cm x 105cm ~ 115cm
Xuất xứ: Việt Nam', 280000, 20),
       (4, 'Thắt Lưng Nam Da Bò Đơn Giản Y2010 D01 18501 |YaMe|', 'Chất liệu : Da Bò Thật 100%
Dây nịt YaMe đều làm từ DA BÒ THẬT 100%
- Gia công sản xuất 100% tại Việt Nam
- Được kiểm tra chất lượng tỉ mỉ
- Bảo hành trọn đời các lỗi kỷ thuật
- Bền bỉ
- Sang trọng và tinh tế
(*) Được tặng kèm hộp đựng ', 310000, 20);


INSERT INTO ecommerce.categories (ShopID, CategoryName)
VALUES (1, 'Kệ Để Đồ'),
       (1, 'Thùng Rác'),
       (1, 'Mẹ Và Bé'),
       (2, 'Bút Viết'),
       (2, 'Sổ Và Giấy Các Loại'),
       (2, 'Túi Đựng Bút'),
       (3, 'Dây Sạc Điện Thoại'),
       (3, 'Ốp Lưng'),
       (3, 'Giá Đỡ Điện Thoại'),
       (4, 'Quần Áo'),
       (4, 'Giày Dép'),
       (4, 'Thắt Lưng');

INSERT INTO ecommerce.product_categories (ProductID, CategoryID)
VALUES (1, 1), (2, 1), (3, 1), (4, 1), (5, 1), (6, 1), (7, 1), (8, 1), (9, 1), (10, 2),
       (11, 2), (12, 2), (13, 2), (14, 2), (15, 2), (16, 2), (17, 3), (18, 3), (19, 3), (20, 3),
       (21, 3), (22, 3), (23, 3), (24, 3), (25, 3), (26, 3), (27, 4), (28, 4), (29, 4), (30, 4),
       (31, 4), (32, 4), (33, 4), (34, 4), (35, 4), (36, 4), (37, 4), (38, 4), (39, 5), (40, 5),
       (41, 5), (42, 5), (43, 5), (44, 5), (45, 5), (46, 5), (47, 5), (48, 6), (49, 6), (50, 6),
       (51, 6), (52, 6), (53, 6), (54, 6), (55, 6), (56, 6), (57, 7), (58, 7), (59, 7), (60, 7),
       (61, 7), (62, 7), (63, 7), (64, 7), (65, 7), (66, 7), (67, 8), (68, 8), (69, 8), (70, 8),
       (71, 8), (72, 8), (73, 8), (74, 8), (75, 8), (76, 8), (77, 8), (78, 8), (79, 9), (80, 9),
       (81, 9), (82, 9), (83, 9), (84, 9),(85, 10), (86, 10), (87, 10), (88, 10), (89, 10), (90, 10),
       (91, 10), (92, 10), (93, 10), (94, 10), (95, 10), (96, 10), (97, 10), (98, 10), (99, 11), (100, 11),
       (101, 11), (102, 11), (103, 11), (104, 11), (105, 11), (106, 11), (107, 11), (108, 11), (109, 11), (110, 11),
       (111, 11), (112, 11), (113, 11), (114, 11), (115, 11), (116, 12), (117, 12), (118, 12), (119, 12), (120, 12),
       (121, 12), (122, 12), (123, 12);

-- SELECT user, host FROM mysql.user;
DROP USER if EXISTS 'guest'@'localhost';
CREATE user if not exists 'guest'@'localhost' identified by '123456';

grant SELECT, INSERT, UPDATE on ecommerce.address to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.users to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.products to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE, DELETE on ecommerce.categories to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.orders to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE, DELETE on ecommerce.orderdetails to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE, DELETE on ecommerce.product_categories to 'guest'@'localhost';
grant SELECT, INSERT, UPDATE on ecommerce.shops to 'guest'@'localhost';