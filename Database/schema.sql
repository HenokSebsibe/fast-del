CREATE TABLE IF NOT EXISTS Roles (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT UNIQUE NOT NULL,
    PasswordHash TEXT NOT NULL,
    RoleId INTEGER NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

INSERT OR IGNORE INTO Roles (Name) VALUES ('Admin'), ('Customer'), ('Delivery');

CREATE TABLE IF NOT EXISTS Customers (
    CustomerID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER,
    CustomerName TEXT NOT NULL,
    Phone TEXT,
    Email TEXT,
    Address TEXT,
    IsActive INTEGER DEFAULT 1,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE IF NOT EXISTS Restaurants (
    RestaurantID INTEGER PRIMARY KEY AUTOINCREMENT,
    RestaurantName TEXT NOT NULL,
    Phone TEXT,
    Address TEXT,
    IsActive INTEGER DEFAULT 1
);

CREATE TABLE IF NOT EXISTS MenuItems (
    MenuItemID INTEGER PRIMARY KEY AUTOINCREMENT,
    RestaurantID INTEGER,
    ItemName TEXT NOT NULL,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
    IsAvailable INTEGER DEFAULT 1,
    FOREIGN KEY (RestaurantID) REFERENCES Restaurants(RestaurantID)
);

CREATE TABLE IF NOT EXISTS DeliveryPersonnel (
    DeliveryPersonID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER,
    Name TEXT NOT NULL,
    Phone TEXT,
    IsActive INTEGER DEFAULT 1,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE IF NOT EXISTS Vehicles (
    VehicleID INTEGER PRIMARY KEY AUTOINCREMENT,
    DeliveryPersonID INTEGER,
    PlateNumber TEXT NOT NULL,
    Model TEXT,
    IsActive INTEGER DEFAULT 1,
    FOREIGN KEY (DeliveryPersonID) REFERENCES DeliveryPersonnel(DeliveryPersonID)
);

CREATE TABLE IF NOT EXISTS Orders (
    OrderID INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerID INTEGER,
    DeliveryPersonID INTEGER,
    OrderDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    Status TEXT NOT NULL, -- 'Pending', 'Preparing', 'Out for Delivery', 'Delivered', 'Cancelled'
    TotalAmount DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (DeliveryPersonID) REFERENCES DeliveryPersonnel(DeliveryPersonID)
);

CREATE TABLE IF NOT EXISTS OrderItems (
    OrderItemID INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderID INTEGER,
    MenuItemID INTEGER,
    Quantity INTEGER NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (MenuItemID) REFERENCES MenuItems(MenuItemID)
);

-- Seed Data
INSERT OR IGNORE INTO Users (UserID, Username, PasswordHash, RoleId) VALUES 
(1, 'admin', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 1),
(2, 'customer', '8c75ddc2bf4dcd8e1e75a6875b28d022b7a976c7c68ba0238e219ba420dfde94', 2),
(3, 'delivery', 'b152d12e61df3f707f185f4039dfcdceea4d6a8b191ec4d5eb6452e85ab89d38', 3);

INSERT OR IGNORE INTO Customers (CustomerID, UserID, CustomerName, Phone, Email, Address) VALUES 
(1, 2, 'Alice Smith', '555-0101', 'alice@example.com', '123 Main St');

INSERT OR IGNORE INTO Restaurants (RestaurantID, RestaurantName, Phone, Address) VALUES 
(1, 'Burger King', '555-0202', '456 Food Ave');

INSERT OR IGNORE INTO MenuItems (MenuItemID, RestaurantID, ItemName, Description, Price) VALUES 
(1, 1, 'Cheeseburger', 'Classic cheese burger', 5.99),
(2, 1, 'Fries', 'Medium fries', 2.99);

INSERT OR IGNORE INTO DeliveryPersonnel (DeliveryPersonID, UserID, Name, Phone) VALUES 
(1, 3, 'Bob Driver', '555-0303');

INSERT OR IGNORE INTO Vehicles (VehicleID, DeliveryPersonID, PlateNumber, Model) VALUES 
(1, 1, 'XYZ-123', 'Honda Civic');
