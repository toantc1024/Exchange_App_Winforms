CREATE DATABASE ExchangeAppDB;
USE ExchangeAppDB;


create table Role(
    RoleID int IDENTITY(1,1) constraint PK_Role PRIMARY KEY,
    Rolename varchar(255),
);

create table Users(
    UserID int IDENTITY(1,1) constraint PK_User PRIMARY KEY,
    Name varchar(255) not null,
    Username varchar(255) not null,
    Password varchar(255) not null,
    Phone varchar(255) not null,
    IsActive bit default 1 not null,
    Address varchar(255) not null,
    Birthdate date not null,
    RoleID int references Role(RoleID),
    constraint CK_Phone check (Phone like '[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]'),
    constraint CK_Birthdate check (Birthdate <= getdate())
);

ALTER TABLE Users ADD Location varchar(255);



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
    constraint CK_Original_price check (Original_price > 0),
    constraint CK_Sell_price check (Sell_price > 0),
    constraint CK_Quantity check (Quantity >= 0),
    constraint CK_UploadedDate check (UploadedDate <= getdate())
);

-- add View_count to Product table
ALTER TABLE Product
ADD View_count int not null default 0;


CREATE TABLE Images(
    ImageID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT not null references Product(ProductID),
    ImageURL VARCHAR(255) not null
);

create table User_Order(
    OrderID int IDENTITY(1,1) constraint PK_User_Order PRIMARY KEY,
    UserID int references Users(UserID),
    Status varchar(255) not null,
    OrderDate date not null
);

create table OrderDetail(
    OrderDetailID int IDENTITY(1,1) constraint PK_OrderDetail PRIMARY KEY,
    OrderID int references User_Order(OrderID),
    ProductID int references Product(ProductID),
    Quantity int check (Quantity > 0) not null,  
);

Create Table WishItem (
	WishItemID int identity(1,1) primary key,
	ProductID int CONSTRAINT FK_CartDetail_ProductID references Product(ProductID), 
	UserID int FOREIGN KEY REFERENCES Users(UserId)
)

INSERT INTO Role(Rolename) VALUES ('Admin');
INSERT INTO Role(Rolename) VALUES ('User');


-- VIEWS

-- VIEW OF ORDERS
CREATE VIEW View_Orders
AS
SELECT OrderID, UserID, Status, OrderDate, ProductID, Quantity, Sell_price * Quantity AS TotalPrice
FROM User_Order
JOIN OrderDetail ON User_Order.OrderID = OrderDetail.OrderID;


-- VIEW OF PRODUCTS
CREATE VIEW View_Products
AS
SELECT ProductID, ProductName, Quantity, Info_des, Status_des, Original_price, Sell_price, UploadedDate, View_count, CatName, Name, Phone, Address, Birthdate, Username, RoleID, Location
FROM Product
JOIN Category ON Product.CatID = Category.CatID
JOIN Users ON Product.UserID = Users.UserID;


-- VIEW OF USERS
CREATE VIEW View_Users
AS
SELECT UserID, Name, Username, Phone, Address, Birthdate, RoleID, Location
FROM Users;

-- VIEW OF CATEGORIES
CREATE VIEW View_Categories
AS
SELECT CatID, CatName
FROM Category;


-- TRIGGERS

-- FUNCTIONS

DROP FUNCTION GetProductByCategory;
DROP FUNCTION GetProductByUser;
DROP FUNCTION FindProductByKeyWord;

CREATE FUNCTION GetProductByCategory(@CatID int)
RETURNS TABLE
AS
RETURN
(  
    SELECT * FROM Product WHERE CatID = @CatID 
);

CREATE FUNCTION GetProductByUser(@UserID int)
RETURNS TABLE
AS
RETURN
(
	SELECT * FROM Product WHERE UserID = @UserID 
);

-- FIND ALL PRODUCTS BY KEYWORD
CREATE FUNCTION FindProductByKeyWord(@Keyword varchar(255))
RETURNS TABLE
AS
RETURN
SELECT * FROM Product WHERE ProductName LIKE '%' + @Keyword + '%' ORDER BY View_count DESC OFFSET 0 ROWS;



DROP FUNCTION Login;
-- CREATE LOGIN FUNCTION THAT RETURN SINGLE ROW OR DEFAULT NULL
CREATE FUNCTION LoginAccount(@Username varchar(255), @Password varchar(255))
RETURNS TABLE
AS
RETURN
(
	SELECT * FROM Users WHERE Username = @Username AND Password = @Password
);


-- EXECUTE FUNCTION
SELECT * FROM LoginAccount('tctoan', 'f90e791f916551c9e5fac06a83a840c8');


SELECT * FROM FindProductByKeyWord('d')


  /* 
  This section joins products and categories based on category_id 
  and filters based on IDs from the temporary table. You can modify it 
  based on your table structure and desired filtering logic.
  */
  INSERT INTO @temp_table (product_id, name)
  SELECT p.product_id, p.name
  FROM products p
  INNER JOIN product_categories pc ON p.product_id = pc.product_id
  INNER JOIN categories c ON pc.category_id = c.category_id
  WHERE pc.category_id IN (SELECT category_id FROM @temp_table);

  /* Select products from the temporary table */
  SELECT * FROM @temp_table;

  /* Drop the temporary table after use */
  DROP TABLE @temp_table;
END;