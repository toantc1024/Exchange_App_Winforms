

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
    constraint CK_Original_price check (Original_price > 0),
    constraint CK_Sell_price check (Sell_price > 0),
    constraint CK_Quantity check (Quantity >= 0),
    constraint CK_UploadedDate check (UploadedDate <= getdate())
);

-- add View_count to Product table
ALTER TABLE Product
ADD View_count int not null default 0;

alter table Product WITH CHECK ADD CONSTRAINT [FK_Product_category] foreign key([CatID]) references Category ON DELETE SET NULL


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

-- VIEWS
go

CREATE VIEW View_products
AS
  SELECT *
  FROM  Product;

go

CREATE VIEW View_users
AS
  SELECT *
  FROM   users;

go

CREATE VIEW View_category
AS
  SELECT *
  FROM   category;

go

CREATE VIEW View_product_images
AS
  SELECT *
  FROM   images
  WHERE  productid IS NOT NULL;

go

CREATE VIEW View_user_order
AS
  SELECT *
  FROM   user_order;

go

CREATE VIEW View_orderdetail
AS
  SELECT *
  FROM   orderdetail;

go

CREATE VIEW View_wishitem
AS
  SELECT *
  FROM   wishitem;

go

-- TRIGGERS
GO
CREATE TRIGGER TRG_ValidateUser
ON Users
AFTER INSERT, UPDATE
AS
BEGIN
    -- VALIDATE PHONE NUMBER
IF EXISTS (SELECT * FROM inserted WHERE NOT Phone LIKE '[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]')
    BEGIN
        RAISERROR('Invalid Phone Number', 16, 1);
        ROLLBACK TRANSACTION;
    END
    -- VALIDATE BIRTHDATE GREATER THAN 16
IF EXISTS (SELECT * FROM inserted WHERE DATEDIFF(YEAR, Birthdate, GETDATE()) < 16)
    BEGIN
        RAISERROR('Invalid Birthdate', 16, 1);
        ROLLBACK TRANSACTION;
    END




    -- VALIDATE ROLE
IF EXISTS (SELECT * FROM inserted WHERE RoleID NOT IN (SELECT RoleID FROM Role))
    BEGIN
        RAISERROR('Invalid Role', 16, 1);
        ROLLBACK TRANSACTION;
    END
    -- VALIDATE ADDRESS
IF EXISTS (SELECT * FROM inserted WHERE Address = '')
    BEGIN
        RAISERROR('Address is not empty', 16, 1);
        ROLLBACK TRANSACTION;
    END




    -- VALIDATE NAME
IF EXISTS (SELECT * FROM inserted WHERE Name = '')
    BEGIN
        RAISERROR('Name is not empty', 16, 1);
        ROLLBACK TRANSACTION;
    END




    -- VALIDATE USERNAME




IF EXISTS (SELECT * FROM inserted WHERE Username = '')




    BEGIN
        RAISERROR('Username is not empty', 16, 1);
        ROLLBACK TRANSACTION;
    END




    -- VALIDATE PASSWORD




IF EXISTS (SELECT * FROM inserted WHERE Password = '')




    BEGIN
        RAISERROR('Password is not empty', 16, 1);
        ROLLBACK TRANSACTION;
    END




    -- IS PASSWORD STRONG
IF EXISTS (SELECT * FROM inserted WHERE LEN(Password) < 8)
    BEGIN
        RAISERROR('Password is not strong', 16, 1);
        ROLLBACK TRANSACTION;
    END

END

GO


CREATE TRIGGER TRG_ProductInsert
ON Product
FOR INSERT, UPDATE
AS
BEGIN
    -- Info_des không được để trống
IF EXISTS (SELECT 1 FROM inserted WHERE Info_des = '')
BEGIN
    RAISERROR ('Info_des không được để trống.', 16, 1);
    ROLLBACK TRANSACTION;
    RETURN;
END;
-- Status_des không được để trống
IF EXISTS (SELECT 1 FROM inserted WHERE Status_des = '')
BEGIN
    RAISERROR ('Status_des không được để trống.', 16, 1);
ROLLBACK TRANSACTION;
RETURN;
END;
-- Product Name không được để trống
IF EXISTS (SELECT 1 FROM inserted WHERE ProductName = '')
BEGIN
    RAISERROR ('Product Name không được để trống.', 16, 1);
ROLLBACK TRANSACTION;
RETURN;
END;
  -- Kiểm tra giá trị Original_price và Sell_price
  IF EXISTS (SELECT 1 FROM inserted WHERE Original_price <= 0 OR Sell_price <= 0)
  BEGIN
      RAISERROR ('Original_price và Sell_price phải lớn hơn 0.', 16, 1);
      ROLLBACK TRANSACTION;
      RETURN;
  END;


  -- Kiểm tra giá trị Quantity
  IF EXISTS (SELECT 1 FROM inserted WHERE Quantity < 0)
  BEGIN
      RAISERROR ('Quantity phải lớn hơn hoặc bằng 0.', 16, 1);
      ROLLBACK TRANSACTION;
      RETURN;
  END;
END;

GO
CREATE TRIGGER TRG_DeleteProduct
ON Product
FOR DELETE
AS
BEGIN
  -- Kiểm tra xem sản phẩm bị xóa có nằm trong đơn hàng khác hay không
  IF EXISTS (
      SELECT 1
      FROM Deleted d
      INNER JOIN OrderDetail od ON d.ProductID = od.ProductID
  )
  BEGIN
      -- Nếu sản phẩm bị xóa có nằm trong đơn hàng khác
      -- Thực hiện hành động tương ứng ở đây (ví dụ: ROLLBACK TRANSACTION hoặc DELETE các đơn hàng liên quan)
      ROLLBACK TRANSACTION; -- Ví dụ: Rollback giao dịch để không cho xóa sản phẩm
      RAISERROR ('Cannot delete the product. It is associated with other orders.', 16, 1);
  END
END;
GO

-- TRIGGER Category
CREATE TRIGGER TRG_AddCategory
ON Category
AFTER INSERT, UPDATE
AS
BEGIN
   SET NOCOUNT ON;
  
   DECLARE @DuplicateCount INT;


   SELECT @DuplicateCount = COUNT(*)
   FROM Category c
   INNER JOIN INSERTED i ON c.CatName = i.CatName;


   IF @DuplicateCount > 1
   BEGIN
       RAISERROR('Category name already exists', 16, 1);
       RETURN;
   END;


   IF EXISTS (
       SELECT *
       FROM INSERTED
       WHERE CatName = ''
   )
   BEGIN
       RAISERROR('Category name must not be empty', 16, 1);
       RETURN;
   END;
END;

GO
CREATE TRIGGER TRG_DEL_CATEGORY
ON Category
AFTER DELETE
AS
BEGIN
   DECLARE @CatName VARCHAR(50)
   SELECT @CatName = CatName
   FROM DELETED








   IF EXISTS (
       SELECT *
       FROM Product
       WHERE CatID = (
           SELECT CatID
           FROM Category
           WHERE CatName = @CatName
       )
   )
   BEGIN
       RAISERROR ('The category being referenced by the product', 16, 1)
   END
END


GO
CREATE TRIGGER TRG_OrderProduct
ON OrderDetail
FOR INSERT
AS
BEGIN
    DECLARE @ProductID INT;
    DECLARE @Quantity INT;
    DECLARE @CurrentQuantity INT;
    DECLARE @OrderID INT 

    
    -- Lấy ProductID và Quantity của dòng dữ liệu vừa được thêm vào OrderDetail
    SELECT @ProductID = ProductID, @Quantity = Quantity, @OrderID = OrderID
    FROM INSERTED;



    -- Lấy số lượng hiện tại của sản phẩm
    SELECT @CurrentQuantity = Quantity
    FROM Product
    WHERE ProductID = @ProductID;

    -- Kiểm tra nếu Quantity mới đặt lớn hơn Quantity hiện tại của sản phẩm
    IF @Quantity > @CurrentQuantity
    BEGIN
        RAISERROR('Số lượng đặt hàng vượt quá số lượng hiện có của sản phẩm', 16, 1);
    END
END;

GO
CREATE TRIGGER TRG_AddImage
ON Images
AFTER INSERT
AS
BEGIN
    DECLARE @ProductID INT;
	DECLARE @ImageURL VARCHAR(255);
	
	-- Lấy ProductID và ImageURL của dòng dữ liệu vừa được thêm vào Product
	SELECT @ProductID = ProductID, @ImageURL = ImageURL
	FROM INSERTED;


	-- Kiểm tra nếu ImageURL rỗng
	IF @ImageURL = ''
	BEGIN
		RAISERROR('ImageURL không được để trống', 16, 1);
	END
END;



-- PROCEDURES


EXEC PROC_AddProduct 10, 'This is a product', 'New', 100000, 120000, 'Product 1', 13, 7;



GO
CREATE PROCEDURE PROC_AddProduct
(
  @Quantity int,
  @Info_des varchar(255),
  @Status_des varchar(255),
  @Original_price float,
  @Sell_price float,
  @ProductName varchar(255),
  @CatID int,
  @UserID int
)
AS
BEGIN TRY
  BEGIN TRAN

  -- Upload date
  DECLARE @UploadDate date;
  SET @UploadDate = GETDATE();

  INSERT Product
  (
    Quantity, Info_des, Status_des, Original_price, Sell_price, UploadedDate, ProductName, CatID, UserID, View_count
  )
  VALUES
  (
    @Quantity, @Info_des, @Status_des, @Original_price, @Sell_price, @UploadDate, @ProductName, @CatID, @UserID, 0
  )

  -- Set output parameter with SCOPE_IDENTITY()
  DECLARE @ProductID int
  SET @ProductID = SCOPE_IDENTITY();
  SELECT @ProductID;
  COMMIT TRAN

  -- No need for return statement, output parameter handles it
END TRY

BEGIN CATCH
  ROLLBACK TRAN

  -- Declare error
  DECLARE @ErrorMessage nvarchar(4000);
  SET @ErrorMessage = N'Lỗi ' + ERROR_MESSAGE();
  RAISERROR(@ErrorMessage, 16, 1);

  -- Declare error state
END CATCH

    go
        --Sua san pham
create procedure PROC_UpdateProduct
	   @ProductID int,
	   @Quantity int,
	   @Info_des varchar(255),
	   @Status_des varchar(255),
	   @Original_price float,
	   @Sell_price float,
	   @ProductName varchar(255),
	   @CatID int
as 
begin
begin try
    begin tran
update Product
set
	Quantity = @Quantity,   
Info_des = @Info_des,
Status_des = @Status_des,
Original_price = @Original_price,
Sell_price = @Sell_price,
ProductName = @ProductName,
CatID = @CatID
where ProductID = @ProductID
commit tran
end try
begin catch
rollback tran
    DECLARE @err NVARCHAR(max)
    SET @err = N'Lỗi '+ ERROR_MESSAGE()
    RAISERROR(@err, 16, 1)
end catch
end
go


create procedure PROC_DeleteProduct
   @ProductID int
as
set nocount on;
begin try
   begin tran
   delete from Product
   where Product.ProductID = @ProductID
   commit tran
end try


   begin catch
       rollback tran


       declare @ErrorNumber_INT INT;
       declare @ErrorSeverity_INT INT;
       declare @ErrorProcedure_VC VARCHAR(200);
       declare @ErrorLine_INT INT;
       declare @ErrorMessage_NVC NVARCHAR(4000);


       select
           @ErrorMessage_NVC = ERROR_MESSAGE(),
           @ErrorLine_INT = ERROR_LINE(),
           @ErrorNumber_INT = ERROR_NUMBER(),
           @ErrorProcedure_VC = ERROR_PROCEDURE(),
           @ErrorSeverity_INT = ERROR_SEVERITY()


       raiserror(@ErrorMessage_NVC, @ErrorSeverity_INT,1);


   end catch
go
go


create procedure PROC_AddWishItem
   @ProductID int,
   @UserID int
as
set nocount on;
   begin
   begin tran

begin try

  IF EXISTS (SELECT * FROM WishItem WHERE ProductID = @ProductID and UserID = @UserID)         BEGIN
			RAISERROR('Product đã nằm trong wish list', 16, 1)
			RETURN
		END


   insert WishItem
   (
       ProductID, UserID
   )


   values
   (
       @ProductID, @UserID
   )


   commit tran
end try


   begin catch
       rollback tran


       declare @ErrorNumber_INT INT;
       declare @ErrorSeverity_INT INT;
       declare @ErrorProcedure_VC VARCHAR(200);
       declare @ErrorLine_INT INT;
       declare @ErrorMessage_NVC NVARCHAR(4000);


       select
           @ErrorMessage_NVC = ERROR_MESSAGE(),
           @ErrorLine_INT = ERROR_LINE(),
           @ErrorNumber_INT = ERROR_NUMBER(),
           @ErrorProcedure_VC = ERROR_PROCEDURE(),
           @ErrorSeverity_INT = ERROR_SEVERITY()


       raiserror(@ErrorMessage_NVC, @ErrorSeverity_INT,1);


   end catch

   end


GO


create procedure PROC_DeleteWishItem
   @WishItemID int
as
begin
set nocount on;
begin try
   begin tran
   delete from WishItem
   where WishItem.WishItemID = @WishItemID
  
   commit tran
end try




   begin catch
       rollback tran




       declare @ErrorNumber_INT INT;
       declare @ErrorSeverity_INT INT;
       declare @ErrorProcedure_VC VARCHAR(200);
       declare @ErrorLine_INT INT;
       declare @ErrorMessage_NVC NVARCHAR(4000);




       select
           @ErrorMessage_NVC = ERROR_MESSAGE(),
           @ErrorLine_INT = ERROR_LINE(),
           @ErrorNumber_INT = ERROR_NUMBER(),
           @ErrorProcedure_VC = ERROR_PROCEDURE(),
           @ErrorSeverity_INT = ERROR_SEVERITY()




       raiserror(@ErrorMessage_NVC, @ErrorSeverity_INT,1);




   end catch
end

GO

CREATE PROCEDURE PROC_AddCat
@CatName VARCHAR(255)
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
		INSERT INTO Category(CatName)
	    VALUES(@CatName)
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
		ROLLBACK TRANSACTION
		    DECLARE @err NVARCHAR(MAX)
            SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
            RAISERROR(@err, 16, 1)
        RETURN
	END CATCH
END

Go
CREATE PROCEDURE PROC_DeleteCat 
@CatId INT
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
		DELETE FROM Category
		WHERE CatID = @CatID

		UPDATE Product SET CatID = null
		WHERE CatID = @CatID
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
		ROLLBACK TRANSACTION
		    DECLARE @err NVARCHAR(MAX)
            SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
            RAISERROR(@err, 16, 1)
        RETURN
	END CATCH
END
Go
CREATE PROCEDURE PROC_UpdateCat
@CatId INT,
@CatName VARCHAR(255)
AS
BEGIN
	BEGIN TRANSACTION
	BEGIN TRY
		UPDATE Category SET CatName = @CatName
		WHERE CatID = @CatID
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
		ROLLBACK TRANSACTION
		    DECLARE @err NVARCHAR(MAX)
            SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
            RAISERROR(@err, 16, 1)
        RETURN
	END CATCH
END

GO


CREATE PROCEDURE PROC_UpdateUserInformation
@UserId int,
@Name varchar(255),
@Username varchar(255),
@Password varchar(255),
@Phone varchar(255),
@Address varchar(255),
@Birthdate date
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        UPDATE Users
        SET Name = @Name, Username = @Username, Password = @Password, Phone = @Phone, Address = @Address, Birthdate = @Birthdate
        WHERE UserID = @UserId
        COMMIT TRANSACTION
    END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    DECLARE @err NVARCHAR(max)
    SET @err = ERROR_MESSAGE()
    RAISERROR(@err, 16, 1)
END CATCH
END


GO


CREATE PROC PROC_CreateAccount
@Name varchar(255),
@Username varchar(255),
@Password varchar(255),
@Phone varchar(255),
@Address varchar(255),
@Birthdate date,
@RoleID int
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION
        INSERT INTO Users(Name, Username, Password, Phone, Address, Birthdate, RoleID, Location, IsActive)
        VALUES (@Name, @Username, @Password, @Phone, @Address, @Birthdate, @RoleID, Null, 1)
        COMMIT TRANSACTION
    END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    DECLARE @err NVARCHAR(max)
    SET @err = N'Lỗi '+ ERROR_MESSAGE()
    RAISERROR(@err, 16, 1)
END CATCH
END

GO
CREATE PROC PROC_SetAccountStatus
@UserID int,
@IsActive bit
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
		UPDATE Users
		SET IsActive = @IsActive
		WHERE UserID = @UserID
		COMMIT TRANSACTION
	END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	DECLARE @err NVARCHAR(max)
	SET @err = N'Lỗi '+ ERROR_MESSAGE()
	RAISERROR(@err, 16, 1)
END CATCH
END


--2034
--User = 7

--EXEC PROC_AddUserOrder 7, 2034, 1


GO
CREATE PROC PROC_AddUserOrder
@UserID int,
@ProductID int,
@Quantity int
AS
BEGIN
	BEGIN TRANSACTION
    	BEGIN TRY
        -- if ProductID have UserID == User -> raise error.
        DECLARE @ProductOwnerID int

        SET @ProductOwnerID = (SELECT UserID FROM Product WHERE ProductID = @ProductID)
        IF (@ProductOwnerID = @UserID)
            BEGIN
			RAISERROR('Không thể mua sản phẩm của chính mình', 16, 1)
			RETURN
		END

		INSERT INTO User_Order(UserID, Status, OrderDate)
		VALUES (@UserID, 'Success', GETDATE())
		DECLARE @OrderID int
		SET @OrderID = SCOPE_IDENTITY()
		INSERT INTO OrderDetail(OrderID, ProductID, Quantity)
		VALUES (@OrderID, @ProductID, @Quantity)
		COMMIT TRANSACTION
	END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	DECLARE @err NVARCHAR(max)
	SET @err = N'Lỗi '+ ERROR_MESSAGE()
	RAISERROR(@err, 16, 1)
END CATCH
END
Go



CREATE PROC PROC_AddImageByProductID
@ImageURL varchar(255),
@ProductID int
AS
BEGIN
       BEGIN TRANSACTION
           BEGIN TRY
               INSERT INTO Images(ImageURL, ProductID) VALUES (@ImageURL, @ProductID);
               COMMIT TRANSACTION
           END TRY
   BEGIN CATCH
       ROLLBACK TRANSACTION
       DECLARE @err NVARCHAR(max)
       SET @err = N'Lỗi '+ ERROR_MESSAGE()
       RAISERROR(@err, 16, 1)
   END CATCH
END

GO


CREATE PROC PROC_RemoveImagesByProductID
@ProductID int
AS
BEGIN
           BEGIN TRANSACTION
            BEGIN TRY
                DELETE FROM Images Where ProductID = @ProductID
                COMMIT TRANSACTION
            END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        DECLARE @err NVARCHAR(max)
        SET @err = N'Lỗi '+ ERROR_MESSAGE()
        RAISERROR(@err, 16, 1)
    END CATCH
END
--- FUNCTIONS
GO
CREATE FUNCTION FUNC_Login
(
  @Username VARCHAR(255),
  @Password VARCHAR(255)
)
RETURNS TABLE
AS
RETURN
(
  SELECT *
  FROM Users
  WHERE Username = @Username AND Password = @Password
     AND EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
);
GO


CREATE
FUNCTION FUNC_SearchProductByName
(
   @ProductName varchar(255)
)
RETURNS TABLE
AS
RETURN
(
   SELECT *
   FROM Product
   WHERE ProductName LIKE '%' + @ProductName + '%'
);
Go
CREATE 
FUNCTION FUNC_SearchProductByUserID
(
  @UserID int
)
RETURNS TABLE
AS
RETURN
(
  SELECT *
  FROM Product
  WHERE UserID = @UserID
);
GO
CREATE FUNCTION FUNC_getAllOrders()
RETURNS TABLE
AS
RETURN
(
	SELECT * FROM User_Order
)
GO
CREATE  
FUNCTION FUNC_GetOrdersByUserID
(
  @UserID int
)
RETURNS TABLE
AS
RETURN
(
  SELECT *
  FROM User_Order UO
  WHERE UO.UserID = @UserID
);
GO


CREATE 
FUNCTION FUNC_GetImagesByProductID
(
  @ProductID int
)
RETURNS TABLE
AS
RETURN
(
  SELECT *
  FROM Images
  WHERE ProductID = @ProductID
);
Go


CREATE FUNCTION FUNC_CheckUserRole
(
 @UserID int
)
RETURNS VARCHAR(20)
AS
BEGIN
 DECLARE @RoleName VARCHAR(20);




 SELECT @RoleName = R.Rolename
 FROM Users U
 INNER JOIN Role R ON U.RoleID = R.RoleID
 WHERE U.UserID = @UserID;




 RETURN @RoleName;
END;
GO
CREATE FUNCTION FUNC_getAllUsers()
RETURNS TABLE
AS
RETURN
(
   SELECT * 
   FROM Users
);
GO
CREATE FUNCTION FUNC_GetUserInformationById (@UserID int)
RETURNS TABLE
AS
    RETURN (
        SELECT * FROM Users WHERE UserID = @UserID  
    )

	GO
CREATE FUNCTION FUNC_GetProductByName (@Keyword varchar(255))
RETURNS TABLE
AS
RETURN 
    SELECT * FROM Product WHERE ProductName LIKE '%' + @Keyword + '%';
Go

CREATE FUNCTION FUNC_GetProductByCategories (@CatID int)
RETURNS TABLE
AS
RETURN 
    SELECT * FROM Product WHERE CatID = @CatID

	GO



GO
--lay orderdetail theo orderid
create function FUNC_GetOrderDetail (@OrderID int)
RETURNS TABLE
AS
RETURN 
SELECT * FROM OrderDetail WHERE OrderID = @OrderID
GO



-- ROLES
CREATE ROLE NormalUser;

GRANT SELECT, INSERT, UPDATE, DELETE ON Product TO NormalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON Category TO NormalUser;
GRANT UPDATE ON Users TO NormalUser;
GRANT SELECT ON Role TO NormalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON Images TO NormalUser;
GRANT SELECT, INSERt, UPDATE ON User_Order TO NormalUser;
GRANT SELECT, INSERT, UPDATE ON OrderDetail TO NormalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON WishItem TO NormalUser;

DENY SELECT ON FUNC_getAllOrders TO NormalUser;
DENY SELECT ON FUNC_getAllUsers TO NormalUser;
DENY EXECUTE ON PROC_SetAccountStatus TO NormalUser;
REVOKE DELETE ON dbo.Users TO NormalUser;

GO
CREATE TRIGGER TRG_AddUserRole
ON Users
AFTER INSERT
AS
BEGIN
	DECLARE @Username VARCHAR(255);
    DECLARE @RoleID INT;
    DECLARE @Password VARCHAR(255);
    DECLARE @sqlCmd NVARCHAR(MAX);

    SELECT @Username = Username, @RoleID = RoleID, @Password = Password FROM INSERTED;

	SET @sqlCmd= 'CREATE LOGIN [' + @Username +'] WITH PASSWORD='''+ @Password+ ''', DEFAULT_DATABASE=[ExchangeBee], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF'
	EXEC (@sqlCmd)

	SET @sqlCmd= 'CREATE USER ' + @Username +' FOR LOGIN '+ @Username
	EXEC (@sqlCmd)

	IF(@RoleID = 2) -- 2 la role cua user
		SET @sqlCmd = 'ALTER ROLE NormalUser ADD MEMBER ' + @Username;
	ELSE
		SET @sqlCmd = 'ALTER SERVER ROLE sysadmin' + ' ADD MEMBER ' + @Username;
	EXEC (@sqlCmd)
END;

GO
CREATE PROCEDURE PROC_DeleteUser
@UserID INT
AS
BEGIN
    DECLARE @Username VARCHAR(255);
DECLARE @sqlCmd NVARCHAR(MAX);
    
    SELECT @Username = Users.Username FROM Users WHERE UserID = @UserID;

    BEGIN TRANSACTION
        BEGIN TRY
            UPDATE Users
            SET RoleID = NULL
			WHERE UserID = @UserID;

            SET @sqlCmd = 'DROP USER ' + @Username;
            EXEC (@sqlCmd);
            SET @sqlCmd = 'DROP LOGIN ' + @Username;
            EXEC (@sqlCmd); 

            DELETE Users
            WHERE UserID = @UserID;
            COMMIT TRANSACTION
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
	DECLARE @err NVARCHAR(MAX)
	SELECT @err = N'Lỗi' + ERROR_MESSAGE()
	RAISERROR(@err, 16, 1)
END CATCH
END;


--      @Quantity int,
--        @Info_des varchar(255),
--        @Status_des varchar(255),
--        @Original_price float,
--        @Sell_price float,
--        @ProductName varchar(255),
--        @CatID int,
--        @UserID int

DECLARE @data INT;
EXEC @data = PROC_AddProduct 10, 'This is a product', 'New', 100000, 120000, 'Product 1', 13, 7;
PRINT( @data)

INSERT INTO Product(Quantity, Info_des, Status_des, Original_price, Sell_price, UploadedDate, ProductName, CatID, UserID, View_count)
VALUES(10, 'This is a product', 'New', 100000, 120000, GETDATE(), 'Product 1', 1, 1, 0);



-- TRY LOGIN WITH Users

