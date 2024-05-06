CREATE DATABASE StuffExchangeApp;
USE StuffExchangeApp;


create table Role(
    RoleID int IDENTITY(1,1) constraint PK_Role PRIMARY KEY,
    Rolename varchar(255),
);

create table Users(
    UserID int IDENTITY(1,1) constraint PK_User PRIMARY KEY,
    Name varchar(255) not null,
    Username varchar(255) not null unique,
    Password varchar(255) not null,
    Phone varchar(255) not null,
    IsActive bit default 1 not null,
    Address varchar(255) not null,
    Birthdate date not null,
    RoleID int references Role(RoleID),
	Location varchar(255)
);


CREATE TABLE Category(
    CatID int IDENTITY(1,1) constraint PK_Category PRIMARY KEY,
    CatName varchar(255) NOT NULL
);


create table Product(
    ProductID int IDENTITY(1,1) constraint PK_Product PRIMARY KEY,
    Quantity int not null,
    Info_des varchar(255) not null,
    Status_des varchar(255) not null,
    Original_price float not null,
    Sell_price float not null,
    UploadedDate date not null,
    ProductName varchar(255) not null,
	CatID int references Category(CatID),
    UserID int references Users(UserID),
     View_count int not null default 0,
    constraint CK_Original_price check (Original_price > 0),
    constraint CK_Sell_price check (Sell_price > 0),
    constraint CK_Quantity check (Quantity >= 0),
    constraint CK_UploadedDate check (UploadedDate <= getdate())
);

alter table Product WITH CHECK ADD CONSTRAINT [FK_Product_category] foreign key([CatID]) references Category ON DELETE SET NULL


CREATE TABLE Images(
    ImageID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT not null references Product(ProductID),
    ImageURL VARCHAR(255) not null
);
CREATE TABLE Review (
    ReviewID int IDENTITY(1,1) CONSTRAINT PK_Review PRIMARY KEY,
    Rating int NOT NULL,
    Comment varchar(255) NOT NULL,
    UserID int REFERENCES Users(UserID),
    ReviewDate date NOT NULL
);


create table User_Order(
    OrderID int IDENTITY(1,1) constraint PK_User_Order PRIMARY KEY,
    UserID int references Users(UserID),
    OrderDate date not null,
    ProductID int references Product(ProductID),
    Quantity int check (Quantity > 0) not null,  
    OrderStatus varchar(255) not null,
   );

   
   INSERT INTO User_Order (
   UserID, Status, OrderDate, ProductID, Quantity, OrderStatus
   ) VALUES ( 1, 'Pending', '2021-12-12', 2, 2, 'Pending');


Create Table WishItem (
	WishItemID int identity(1,1) primary key,
	ProductID int CONSTRAINT FK_CartDetail_ProductID references Product(ProductID), 
	UserID int FOREIGN KEY REFERENCES Users(UserId)
)

INSERT INTO Role(Rolename) VALUES ('Admin');
INSERT INTO Role(Rolename) VALUES ('User');
-- VIEWS



    