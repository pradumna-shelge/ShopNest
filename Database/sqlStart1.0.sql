--create database ShopNest
--use ShopNest
--USE MASTER
--DROP DATABASE ShopNest


-------------------------------------------------------------User Roles Table -----------------
CREATE TABLE mst_UserRoles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName VARCHAR(50) NOT NULL
);

INSERT INTO mst_UserRoles (RoleName)
VALUES
    ('Customer'),          
    ('PremiumCustomer'),     
    ('Admin');             



------------------------------------------------------------Users Table--------------------------
CREATE TABLE mst_Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) NOT NULL,
    PasswordHash VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    RegistrationDate DATETIME NOT NULL,
	OTP VARCHAR(6),
    OTPDateTime DATETIME,
    LastLogin DATETIME,
    LoginPCNo VARCHAR(50),
	ResetLink VARCHAR(100), 
    ResetLinkExpiration DATETIME
);

------------------------------------------------------------ User Role mapping table using user tabel in role tabel -------------
CREATE TABLE trn_UserRoleMapping (
    MappingID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    RoleID INT,
    FOREIGN KEY (UserID) REFERENCES mst_Users (UserID),
    FOREIGN KEY (RoleID) REFERENCES mst_UserRoles (RoleID)
);




----------------------------------------------------------- Product Table ---------------------------------------
CREATE TABLE mst_Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName VARCHAR(100) NOT NULL,
	ProductImage NVARCHAR(MAX) NOT NULL,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
);

alter table mst_Products
add Mrprice  DECIMAL(10, 2) NOT NULL

---------------------------------------------------------  Orders Table ---------------------------------------------
CREATE TABLE trn_Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    OrderDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (UserID) REFERENCES mst_Users (UserID)
);
alter table trn_Orders
add InvoiceNo bigint not null
------------------------------------------------------  Order Item Table ----------------------------------------------
CREATE TABLE trn_Orders_OrderItems (
    OrderItemID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT,
    ProductID INT,
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES trn_Orders (OrderID),
    FOREIGN KEY (ProductID) REFERENCES mst_Products (ProductID)
);

------------------------------------------------------ user cart Table ---------------------------------------------
CREATE TABLE trn_AddToCart (
    CartID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT, 
    ProductID INT, 
    Quantity INT NOT NULL,
    AddedDateTime DATETIME NOT NULL,
    FOREIGN KEY (UserID) REFERENCES mst_Users (UserID),
    FOREIGN KEY (ProductID) REFERENCES mst_Products (ProductID)
);

alter table trn_AddToCart
add Price  DECIMAL(10, 2) NOT NULL

--------------------------------------------------- location Table ----------------------------
 CREATE TABLE mst_Location (
    LocationID INT PRIMARY KEY,
    ParentLocationID INT,
    Name NVARCHAR(255) NOT NULL
);

-- Add foreign key constraint for ParentLocationID
ALTER TABLE mst_Location
ADD CONSTRAINT FK_Location_ParentLocation
FOREIGN KEY (ParentLocationID)
REFERENCES mst_Location(LocationID);



------------------------------------------------ Order Delivery Addresses Table -------------
CREATE TABLE trn_Users_DeliveryAddresses (
    AddressID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT, 
    Country VARCHAR(200),
    State VARCHAR(200),
    City VARCHAR(200),
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    AddressLine NVARCHAR(Max),
    ZIP VARCHAR(10),
    CONSTRAINT FK_UserID FOREIGN KEY (UserID) REFERENCES mst_Users (UserID)
);


-------------------------------------------- Views ----------------------------------------------------------------------

----------------------------------- users and role info  view -------------------------------------------------------------
go
CREATE VIEW vw_UserInfo AS
SELECT
    u.Username,
    u.Email,
    u.LastLogin,
    u.UserId,
    m.RoleId,
    u.PasswordHash
FROM mst_Users u
JOIN trn_UserRoleMapping m ON u.UserId = m.UserId;
go

select * from vw_UserInfo


-------------------------------- Store Procedures ---------------

--------------------------------------------user cart data ---------------------
go
CREATE PROCEDURE GetCartData
    @UserName NVARCHAR(255)
AS
BEGIN
    DECLARE @UserId INT;
    
    -- Find the user's ID based on the username
    SELECT @UserId = UserId FROM mst_Users WHERE Username = @UserName;

    -- Check if the user was found
    IF @UserId IS NOT NULL
    BEGIN
        SELECT 
            CONVERT(DECIMAL(10, 2), mp.Price) AS Price,
            CONVERT(DECIMAL(10, 2), mp.Mrprice) AS Mrprice,
            ca.Quantity,
            mp.ProductImage,
            mp.ProductName,
            mp.Description,
            ca.CartId,
            ca.UserId
        FROM trn_AddToCart ca
        JOIN mst_Products mp ON ca.ProductId = mp.ProductId
        WHERE ca.UserId = @UserId
        ORDER BY ca.CartId DESC;
    END
    ELSE
    BEGIN
        -- Return an empty result or an error message
        -- You can customize this part based on your requirements
        SELECT NULL AS Price, NULL AS Mrprice, NULL AS Quantity, NULL AS ProductImage, NULL AS ProductName, NULL AS Description, NULL AS CartId, NULL AS UserId;
    END
END;
go

exec GetCartData @UserName='admin'
drop PROCEDURE  GetCartData



