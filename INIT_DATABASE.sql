CREATE DATABASE TESTDB;
USE TESTDB;
go

--1. ROLE
CREATE TABLE role
  (
     roleid   INT IDENTITY(1, 1) CONSTRAINT pk_role PRIMARY KEY,
     rolename VARCHAR(255),
  );

--2. USER
CREATE TABLE users
  (
     userid     INT IDENTITY(1, 1) CONSTRAINT pk_user PRIMARY KEY,
     NAME       VARCHAR(255) NOT NULL,
     username   VARCHAR(255) NOT NULL UNIQUE,
     password   VARCHAR(255) NOT NULL,
     phone      VARCHAR(255) NOT NULL,
     isactive   BIT DEFAULT 1 NOT NULL,
     address    VARCHAR(255) NOT NULL,
     birthdate  DATE NOT NULL,
     roleid     INT REFERENCES role(roleid),
     location   VARCHAR(255),
     view_count INT DEFAULT 0,
     CONSTRAINT ck_phone CHECK ( phone LIKE
     '[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]' ),
     CONSTRAINT ck_birthdate CHECK ( birthdate <= Getdate() )
  );

--3. CATEGORY
CREATE TABLE category
  (
     catid   INT IDENTITY(1, 1) CONSTRAINT pk_category PRIMARY KEY,
     catname VARCHAR(255) NOT NULL
  );

--4. PRODUCT
CREATE TABLE product
  (
     productid      INT IDENTITY(1, 1) CONSTRAINT pk_product PRIMARY KEY,
     quantity       INT NOT NULL,
     info_des       VARCHAR(255) NOT NULL,
     status_des     VARCHAR(255) NOT NULL,
     original_price FLOAT NOT NULL,
     sell_price     FLOAT NOT NULL,
     uploadeddate   DATE NOT NULL,
     productname    VARCHAR(255) NOT NULL,
     catid          INT REFERENCES category(catid),
     userid         INT REFERENCES users(userid),
     CONSTRAINT ck_original_price CHECK (original_price > 0),
     CONSTRAINT ck_sell_price CHECK (sell_price > 0),
     CONSTRAINT ck_quantity CHECK (quantity >= 0),
     CONSTRAINT ck_uploadeddate CHECK ( uploadeddate <= Getdate() )
  );

-- add View_count to Product table
ALTER TABLE product
  ADD view_count INT NOT NULL DEFAULT 0;

--5. IMAGES
CREATE TABLE images
  (
     imageid   INT IDENTITY(1, 1) PRIMARY KEY,
     productid INT NOT NULL REFERENCES product(productid),
     imageurl  VARCHAR(255) NOT NULL
  );

--6. USER_ORDER
CREATE TABLE user_order
  (
     orderid   INT IDENTITY(1, 1) CONSTRAINT pk_user_order PRIMARY KEY,
     userid    INT REFERENCES users(userid),
     status    VARCHAR(255) NOT NULL,
     orderdate DATE NOT NULL
  );

--7. ORDERDETAIL
CREATE TABLE orderdetail
  (
     orderdetailid INT IDENTITY(1, 1) CONSTRAINT pk_orderdetail PRIMARY KEY,
     orderid       INT REFERENCES user_order(orderid),
     productid     INT REFERENCES product(productid),
     quantity      INT CHECK (quantity > 0) NOT NULL,
  );

--8. WISHITEM
CREATE TABLE wishitem
  (
     wishitemid INT IDENTITY(1, 1) PRIMARY KEY,
     productid  INT CONSTRAINT fk_cartdetail_productid REFERENCES product(
     productid),
     userid     INT FOREIGN KEY REFERENCES users(userid)
  )

INSERT INTO role
            (rolename)
VALUES      ('Admin');

INSERT INTO role
            (rolename)
VALUES      ('User');

-- VIEWS
go

CREATE VIEW view_products
AS
  SELECT *
  FROM   product;

go

CREATE VIEW view_users
AS
  SELECT *
  FROM   users;

go

CREATE VIEW view_category
AS
  SELECT *
  FROM   category;

go

CREATE VIEW view_product_images
AS
  SELECT *
  FROM   images
  WHERE  productid IS NOT NULL;

go

CREATE VIEW view_user_order
AS
  SELECT *
  FROM   user_order;

go

CREATE VIEW view_orderdetail
AS
  SELECT *
  FROM   orderdetail;

go

CREATE VIEW view_wishitem
AS
  SELECT *
  FROM   wishitem;

go

-- TRIGGERS
CREATE TRIGGER trg_validateuser
ON users
after INSERT, UPDATE
AS
  BEGIN
      -- VALIDATE PHONE NUMBER
      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  NOT
            phone LIKE '[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]')
        BEGIN
            RAISERROR('Invalid Phone Number',16,1);

            ROLLBACK TRANSACTION;
        END

      -- VALIDATE BIRTHDATE GREATER THAN 16
      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  Datediff(year, birthdate, Getdate()) < 16)
        BEGIN
            RAISERROR('Invalid Birthdate',16,1);

            ROLLBACK TRANSACTION;
        END

      -- VALIDATE ROLE
      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  roleid NOT IN (SELECT roleid
                                       FROM   role))
        BEGIN
            RAISERROR('Invalid Role',16,1);

            ROLLBACK TRANSACTION;
        END

      -- VALIDATE ADDRESS
      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  address = '')
        BEGIN
            RAISERROR('Address is not empty',16,1);

            ROLLBACK TRANSACTION;
        END

      -- VALIDATE NAME
      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  NAME = '')
        BEGIN
            RAISERROR('Name is not empty',16,1);

            ROLLBACK TRANSACTION;
        END

      -- VALIDATE USERNAME
      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  username = '')
        BEGIN
            RAISERROR('Username is not empty',16,1);

            ROLLBACK TRANSACTION;
        END

      -- VALIDATE PASSWORD
      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  password = '')
        BEGIN
            RAISERROR('Password is not empty',16,1);

            ROLLBACK TRANSACTION;
        END

      -- IS PASSWORD STRONG
      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  Len(password) < 8)
        BEGIN
            RAISERROR('Password is not strong',16,1);

            ROLLBACK TRANSACTION;
        END

      -- IS PASSWORD ONLY CONTAIN ALPHABET AND NUMBER
      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  password NOT LIKE '%[^a-zA-Z0-9]%')
        BEGIN
            RAISERROR('Password only contain alphabet and number',16,1);

            ROLLBACK TRANSACTION;
        END

      -- IS PASSWORD CONTAIN AT LEAST 1 UPPERCASE
      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  password NOT LIKE '%[A-Z]%')
        BEGIN
            RAISERROR('Password must contain at least 1 uppercase',16,1);

            ROLLBACK TRANSACTION;
        END
  END

go

CREATE TRIGGER trg_productinsert
ON product
FOR INSERT, UPDATE
AS
  BEGIN
      -- Info_des không được để trống
      IF EXISTS (SELECT 1
                 FROM   inserted
                 WHERE  info_des = '')
        BEGIN
            RAISERROR ('Info_des không được để trống.',16,1);

            ROLLBACK TRANSACTION;

            RETURN;
        END;

      -- Status_des không được để trống
      IF EXISTS (SELECT 1
                 FROM   inserted
                 WHERE  status_des = '')
        BEGIN
            RAISERROR ('Status_des không được để trống.',16,1);

            ROLLBACK TRANSACTION;

            RETURN;
        END;

      -- Product Name không được để trống
      IF EXISTS (SELECT 1
                 FROM   inserted
                 WHERE  productname = '')
        BEGIN
            RAISERROR ('Product Name không được để trống.',16,1);

            ROLLBACK TRANSACTION;

            RETURN;
        END;

      -- Kiểm tra giá trị Original_price và Sell_price
      IF EXISTS (SELECT 1
                 FROM   inserted
                 WHERE  original_price <= 0
                         OR sell_price <= 0)
        BEGIN
            RAISERROR ('Original_price và Sell_price phải lớn hơn 0.',16,1
            );

            ROLLBACK TRANSACTION;

            RETURN;
        END;

      -- Kiểm tra giá trị Quantity
      IF EXISTS (SELECT 1
                 FROM   inserted
                 WHERE  quantity < 0)
        BEGIN
            RAISERROR ('Quantity phải lớn hơn hoặc bằng 0.',16,1);

            ROLLBACK TRANSACTION;

            RETURN;
        END;

      -- Kiểm tra giá trị UploadedDate
      IF EXISTS (SELECT 1
                 FROM   inserted
                 WHERE  uploadeddate > Getdate())
        BEGIN
            RAISERROR (
            'UploadedDate không thể lớn hơn ngày hiện tại.',
            16,1
            );

            ROLLBACK TRANSACTION;

            RETURN;
        END;
  END;

go

CREATE TRIGGER trg_deleteproduct
ON product
FOR DELETE
AS
  BEGIN
      -- Kiểm tra xem sản phẩm bị xóa có nằm trong đơn hàng khác hay không
      IF EXISTS (SELECT 1
                 FROM   deleted d
                        INNER JOIN orderdetail od
                                ON d.productid = od.productid)
        BEGIN
            -- Nếu sản phẩm bị xóa có nằm trong đơn hàng khác
            -- Thực hiện hành động tương ứng ở đây (ví dụ: ROLLBACK TRANSACTION hoặc DELETE các đơn hàng liên quan)
            ROLLBACK TRANSACTION;
            -- Ví dụ: Rollback giao dịch để không cho xóa sản phẩm

            RAISERROR (
            'Cannot delete the product. It is associated with other orders.',
            16,1);
        END
  END;

-- TRIGGER Category
go

CREATE TRIGGER trg_addcategory
ON category
after INSERT, UPDATE
AS
  BEGIN
      SET nocount ON;

      DECLARE @DuplicateCount INT;

      SELECT @DuplicateCount = Count(*)
      FROM   category c
             INNER JOIN inserted i
                     ON c.catname = i.catname;

      IF @DuplicateCount > 1
        BEGIN
            RAISERROR('Category name already exists',16,1);

            ROLLBACK TRANSACTION;

            RETURN;
        END;

      IF EXISTS (SELECT *
                 FROM   inserted
                 WHERE  catname = '')
        BEGIN
            RAISERROR('Category name must not be empty',16,1);

            ROLLBACK TRANSACTION;

            RETURN;
        END;
  END;

go

CREATE TRIGGER trg_del_category
ON category
after DELETE
AS
  BEGIN
      DECLARE @CatName VARCHAR(50)

      SELECT @CatName = catname
      FROM   deleted

      IF EXISTS (SELECT *
                 FROM   product
                 WHERE  catid = (SELECT catid
                                 FROM   category
                                 WHERE  catname = @CatName))
        BEGIN
            RAISERROR ('The category being referenced by the product',16,1)

            ROLLBACK
        END
  END

go

CREATE TRIGGER trg_orderproduct
ON orderdetail
after INSERT
AS
  BEGIN
      DECLARE @ProductID INT;
      DECLARE @Quantity INT;
      DECLARE @CurrentQuantity INT;

      -- Lấy ProductID và Quantity của dòng dữ liệu vừa được thêm vào OrderDetail
      SELECT @ProductID = productid,
             @Quantity = quantity
      FROM   inserted;

      -- Lấy số lượng hiện tại của sản phẩm
      SELECT @CurrentQuantity = quantity
      FROM   product
      WHERE  productid = @ProductID;

      -- Kiểm tra nếu Quantity mới đặt lớn hơn Quantity hiện tại của sản phẩm
      IF @Quantity > @CurrentQuantity
        BEGIN
            RAISERROR(
'Số lượng đặt hàng vượt quá số lượng hiện có của sản phẩm'
,16,1);

    ROLLBACK TRANSACTION;
END
ELSE
  BEGIN
      UPDATE product
      SET    quantity = @CurrentQuantity - @Quantity
      WHERE  productid = @ProductID;
  END
END;

go

CREATE TRIGGER trg_wishlist
ON wishitem
after INSERT
AS
  BEGIN
      BEGIN TRAN

      IF EXISTS (SELECT *
                 FROM   inserted i
                        JOIN wishitem w
                          ON i.productid = w.productid)
        BEGIN
            ROLLBACK TRAN

            RAISERROR('Product is already in the wish list',16,1)
        END
  END

go

CREATE TRIGGER trg_addimage
ON images
after INSERT
AS
  BEGIN
      DECLARE @ProductID INT;
      DECLARE @ImageURL VARCHAR(255);

      -- Lấy ProductID và ImageURL của dòng dữ liệu vừa được thêm vào Product
      SELECT @ProductID = productid,
             @ImageURL = imageurl
      FROM   inserted;

      -- Kiểm tra nếu ImageURL rỗng
      IF @ImageURL = ''
        BEGIN
            RAISERROR('ImageURL không được để trống',16,1);

            ROLLBACK TRANSACTION;
        END
      ELSE
        BEGIN
            INSERT INTO images
                        (productid,
                         imageurl)
            VALUES      (@ProductID,
                         @ImageURL);
        END;
  END;

-- PROCEDURES
go

CREATE PROCEDURE Proc_addproduct @Quantity       INT,
                                 @Info_des       VARCHAR(255),
                                 @Status_des     VARCHAR(255),
                                 @Original_price FLOAT,
                                 @Sell_price     FLOAT,
                                 @ProductName    VARCHAR(255),
                                 @CatID          INT,
                                 @UserID         INT
AS
    SET nocount ON;

  BEGIN try
      BEGIN TRAN

      -- upload date
      DECLARE @UploadDate DATE;

      SET @UploadDate = Getdate();

      INSERT product
             (quantity,
              info_des,
              status_des,
              original_price,
              sell_price,
              uploadeddate,
              productname,
              catid,
              userid,
              view_count)
      VALUES ( @Quantity,
               @Info_des,
               @Status_des,
               @Original_price,
               @Sell_price,
               @UploadDate,
               @ProductName,
               @CatID,
               @UserID,
               0 )

      COMMIT TRAN
  END try

  BEGIN catch
      ROLLBACK TRAN

      -- declare error
      DECLARE @ErrorMessage NVARCHAR(4000);

      SET @ErrorMessage = N'Lỗi ' + Error_message();

      RAISERROR(@ErrorMessage,16,1);
  -- declare error state
  END catch

go

--Sua san pham
CREATE PROC Proc_editproduct @ProductID      INT,
                             @Quantity       INT,
                             @Info_des       VARCHAR(255),
                             @Status_des     VARCHAR(255),
                             @Original_price FLOAT,
                             @Sell_price     FLOAT,
                             @UploadedDate   DATE,
                             @ProductName    VARCHAR(255),
                             @CatID          INT,
                             @UserID         INT,
                             @View_count     INT
AS
  BEGIN
      SET nocount ON;

      BEGIN try
          BEGIN TRAN

          UPDATE product
          SET    quantity = @Quantity,
                 info_des = @Info_des,
                 status_des = @Status_des,
                 original_price = @Original_price,
                 sell_price = @Sell_price,
                 uploadeddate = @UploadedDate,
                 productname = @ProductName,
                 catid = @CatID,
                 view_count = @View_count
          WHERE  productid = @ProductID

          COMMIT TRAN
      END try

      BEGIN catch
          ROLLBACK TRAN

          DECLARE @ErrorNumber_INT INT;
          DECLARE @ErrorSeverity_INT INT;
          DECLARE @ErrorProcedure_VC VARCHAR(200);
          DECLARE @ErrorLine_INT INT;
          DECLARE @ErrorMessage_NVC NVARCHAR(4000);

          SELECT @ErrorMessage_NVC = Error_message(),
                 @ErrorLine_INT = Error_line(),
                 @ErrorNumber_INT = Error_number(),
                 @ErrorProcedure_VC = Error_procedure(),
                 @ErrorSeverity_INT = Error_severity()

          RAISERROR(@ErrorMessage_NVC,@ErrorSeverity_INT,1);
      END catch
  END

go

CREATE PROCEDURE Proc_deleteproduct @ProductID INT
AS
    SET nocount ON;

  BEGIN try
      BEGIN TRAN

      DELETE FROM product
      WHERE  product.productid = @ProductID

      COMMIT TRAN
  END try

  BEGIN catch
      ROLLBACK TRAN

      DECLARE @ErrorNumber_INT INT;
      DECLARE @ErrorSeverity_INT INT;
      DECLARE @ErrorProcedure_VC VARCHAR(200);
      DECLARE @ErrorLine_INT INT;
      DECLARE @ErrorMessage_NVC NVARCHAR(4000);

      SELECT @ErrorMessage_NVC = Error_message(),
             @ErrorLine_INT = Error_line(),
             @ErrorNumber_INT = Error_number(),
             @ErrorProcedure_VC = Error_procedure(),
             @ErrorSeverity_INT = Error_severity()

      RAISERROR(@ErrorMessage_NVC,@ErrorSeverity_INT,1);
  END catch

go

go

CREATE PROCEDURE Proc_addwishitem @ProductID INT,
                                  @UserID    INT
AS
    SET nocount ON;

  BEGIN
      BEGIN TRAN

      BEGIN try
          INSERT wishitem
                 (productid,
                  userid)
          VALUES ( @ProductID,
                   @UserID )

          COMMIT TRAN
      END try

      BEGIN catch
          ROLLBACK TRAN

          DECLARE @ErrorNumber_INT INT;
          DECLARE @ErrorSeverity_INT INT;
          DECLARE @ErrorProcedure_VC VARCHAR(200);
          DECLARE @ErrorLine_INT INT;
          DECLARE @ErrorMessage_NVC NVARCHAR(4000);

          SELECT @ErrorMessage_NVC = Error_message(),
                 @ErrorLine_INT = Error_line(),
                 @ErrorNumber_INT = Error_number(),
                 @ErrorProcedure_VC = Error_procedure(),
                 @ErrorSeverity_INT = Error_severity()

          RAISERROR(@ErrorMessage_NVC,@ErrorSeverity_INT,1);
      END catch
  END

go

CREATE PROCEDURE Proc_deletewishitem @WishItemID INT
AS
    SET nocount ON;

  BEGIN try
      BEGIN TRAN

      DELETE FROM wishitem
      WHERE  wishitem.wishitemid = @WishItemID

      COMMIT TRAN
  END try

  BEGIN catch
      ROLLBACK TRAN

      DECLARE @ErrorNumber_INT INT;
      DECLARE @ErrorSeverity_INT INT;
      DECLARE @ErrorProcedure_VC VARCHAR(200);
      DECLARE @ErrorLine_INT INT;
      DECLARE @ErrorMessage_NVC NVARCHAR(4000);

      SELECT @ErrorMessage_NVC = Error_message(),
             @ErrorLine_INT = Error_line(),
             @ErrorNumber_INT = Error_number(),
             @ErrorProcedure_VC = Error_procedure(),
             @ErrorSeverity_INT = Error_severity()

      RAISERROR(@ErrorMessage_NVC,@ErrorSeverity_INT,1);
  END catch

go

CREATE PROCEDURE Proc_addcat @CatName VARCHAR(255)
AS
  BEGIN
      BEGIN TRANSACTION

      BEGIN try
          INSERT INTO category
                      (catname)
          VALUES     (@CatName)

          COMMIT TRANSACTION
      END try

      BEGIN catch
          ROLLBACK TRANSACTION

          DECLARE @err NVARCHAR(max)

          SELECT @err = N'Lỗi ' + Error_message()

          RAISERROR(@err,16,1)

          RETURN
      END catch
  END

go

CREATE PROCEDURE Proc_deletecat @CatId INT
AS
  BEGIN
      BEGIN TRANSACTION

      BEGIN try
          DELETE FROM category
          WHERE  catid = @CatID

          UPDATE product
          SET    catid = NULL
          WHERE  catid = @CatID

          COMMIT TRANSACTION
      END try

      BEGIN catch
          ROLLBACK TRANSACTION

          DECLARE @err NVARCHAR(max)

          SELECT @err = N'Lỗi ' + Error_message()

          RAISERROR(@err,16,1)

          RETURN
      END catch
  END

go

CREATE PROCEDURE Proc_updatecat @CatId   INT,
                                @CatName VARCHAR(255)
AS
  BEGIN
      BEGIN TRANSACTION

      BEGIN try
          UPDATE category
          SET    catname = @CatName
          WHERE  catid = @CatID

          COMMIT TRANSACTION
      END try

      BEGIN catch
          ROLLBACK TRANSACTION

          DECLARE @err NVARCHAR(max)

          SELECT @err = N'Lỗi ' + Error_message()

          RAISERROR(@err,16,1)

          RETURN
      END catch
  END

go

CREATE PROCEDURE Proc_updateuserinformation @UserId    INT,
                                            @Name      VARCHAR(255),
                                            @Username  VARCHAR(255),
                                            @Password  VARCHAR(255),
                                            @Phone     VARCHAR(255),
                                            @Address   VARCHAR(255),
                                            @Birthdate DATE
AS
  BEGIN
      BEGIN try
          BEGIN TRANSACTION

          UPDATE users
          SET    NAME = @Name,
                 username = @Username,
                 password = @Password,
                 phone = @Phone,
                 address = @Address,
                 birthdate = @Birthdate
          WHERE  userid = @UserId

          COMMIT TRANSACTION
      END try

      BEGIN catch
          ROLLBACK TRANSACTION

          DECLARE @err NVARCHAR(max)

          SET @err = Error_message()

          RAISERROR(@err,16,1)
      END catch
  END

go

CREATE PROC Proc_createaccount @Name      VARCHAR(255),
                               @Username  VARCHAR(255),
                               @Password  VARCHAR(255),
                               @Phone     VARCHAR(255),
                               @Address   VARCHAR(255),
                               @Birthdate DATE,
                               @RoleID    INT
AS
  BEGIN
      BEGIN try
          BEGIN TRANSACTION

          INSERT INTO users
                      (NAME,
                       username,
                       password,
                       phone,
                       address,
                       birthdate,
                       roleid,
                       location,
                       isactive)
          VALUES      (@Name,
                       @Username,
                       @Password,
                       @Phone,
                       @Address,
                       @Birthdate,
                       @RoleID,
                       NULL,
                       1)

          COMMIT TRANSACTION
      END try

      BEGIN catch
          ROLLBACK TRANSACTION

          DECLARE @err NVARCHAR(max)

          SET @err = N'Lỗi ' + Error_message()

          RAISERROR(@err,16,1)
      END catch
  END

go

CREATE PROC Proc_setaccountstatus @UserID   INT,
                                  @IsActive BIT
AS
  BEGIN
      BEGIN try
          BEGIN TRANSACTION

          UPDATE users
          SET    isactive = @IsActive
          WHERE  userid = @UserID

          COMMIT TRANSACTION
      END try

      BEGIN catch
          ROLLBACK TRANSACTION

          DECLARE @err NVARCHAR(max)

          SET @err = N'Lỗi ' + Error_message()

          RAISERROR(@err,16,1)
      END catch
  END

go

go

CREATE PROC Proc_adduserorder @UserID    INT,
                              @ProductID INT,
                              @Quantity  INT
AS
  BEGIN
      BEGIN TRANSACTION

      BEGIN try
          INSERT INTO user_order
                      (userid,
                       status,
                       orderdate)
          VALUES      (@UserID,
                       'Success',
                       Getdate())

          DECLARE @OrderID INT

          SET @OrderID = Scope_identity()

          -- PRINT @OrderID
          PRINT @OrderID

          INSERT INTO orderdetail
                      (orderid,
                       productid,
                       quantity)
          VALUES      (@OrderID,
                       @ProductID,
                       @Quantity)

          COMMIT TRANSACTION
      END try

      BEGIN catch
          ROLLBACK TRANSACTION

          DECLARE @err NVARCHAR(max)

          SET @err = N'Lỗi ' + Error_message()

          RAISERROR(@err,16,1)
      END catch
  END

go

CREATE PROC Proc_addimagebyproductid @ImageURL  VARCHAR(255),
                                     @ProductID INT
AS
  BEGIN
      BEGIN TRANSACTION

      BEGIN try
          INSERT INTO images
                      (imageurl,
                       productid)
          VALUES      (@ImageURL,
                       @ProductID);

          COMMIT TRANSACTION
      END try

      BEGIN catch
          ROLLBACK TRANSACTION

          DECLARE @err NVARCHAR(max)

          SET @err = N'Lỗi ' + Error_message()

          RAISERROR(@err,16,1)
      END catch
  END

go

CREATE PROC Proc_removeimagesbyproductid @ProductID INT
AS
  BEGIN
      BEGIN TRANSACTION

      BEGIN try
          DELETE FROM images
          WHERE  productid = @ProductID

          COMMIT TRANSACTION
      END try

      BEGIN catch
          ROLLBACK TRANSACTION

          DECLARE @err NVARCHAR(max)

          SET @err = N'Lỗi ' + Error_message()

          RAISERROR(@err,16,1)
      END catch
  END

-- FUNCTIONS
go

CREATE FUNCTION Func_login (@Username VARCHAR(255),
                            @Password VARCHAR(255))
returns TABLE
AS
    RETURN
      (SELECT *
       FROM   users
       WHERE  username = @Username
              AND password = @Password
              AND EXISTS (SELECT 1
                          FROM   users
                          WHERE  username = @Username));

go

CREATE FUNCTION Func_searchproductbyname (@ProductName VARCHAR(255))
returns TABLE
AS
    RETURN
      (SELECT *
       FROM   product
       WHERE  productname LIKE '%' + @ProductName + '%');

go

CREATE FUNCTION Func_searchproductbyuserid (@UserID INT)
returns TABLE
AS
    RETURN
      (SELECT *
       FROM   product
       WHERE  userid = @UserID);

go
GO
CREATE FUNCTION Func_getordersbyuserid (@UserID INT)
returns TABLE
AS
    RETURN
      (SELECT UO.orderid,
              UO.status,
              UO.orderdate
       FROM   user_order UO
       WHERE  UO.userid = @UserID);

go

CREATE FUNCTION Func_getimagesbyproductid (@ProductID INT)
returns TABLE
AS
    RETURN
      (SELECT imageid,
              imageurl
       FROM   images
       WHERE  productid = @ProductID);

go

CREATE FUNCTION Func_getallusers()
returns TABLE
AS
    RETURN
      (SELECT *
       FROM   users);

go

CREATE FUNCTION Func_getuserinformationbyid (@UserID INT)
returns TABLE
AS
    RETURN
      (SELECT *
       FROM   users
       WHERE  userid = @UserID)

go

CREATE FUNCTION Func_getproductbyname (@Keyword VARCHAR(255))
returns TABLE
AS
    RETURN
      SELECT *
      FROM   product
      WHERE  productname LIKE '%' + @Keyword + '%';

go

CREATE FUNCTION Func_getproductbycategories (@CatID INT)
returns TABLE
AS
    RETURN
      SELECT *
      FROM   product
      WHERE  catid = @CatID

go
CREATE FUNCTION Func_getallorders ()
returns TABLE
AS
    RETURN
      (SELECT *
       FROM   user_order);

--lay orderdetail theo orderid
go

CREATE FUNCTION Func_getorderdetail (@OrderID INT)
returns TABLE
AS
    RETURN
      SELECT *
      FROM   orderdetail
      WHERE  orderid = @OrderID 