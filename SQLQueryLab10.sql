SELECT ProductName, UnitPrice, CategoryName FROM Products
JOIN Categories ON Categories.CategoryID = Products.CategoryID
ORDER BY CategoryName, ProductName
GO

SELECT CompanyName, COUNT(Orders.CustomerID) AS TotalOrders FROM Customers
JOIN Orders ON Customers.CustomerID = Orders.CustomerID
GROUP BY CompanyName
ORDER BY TotalOrders DESC
GO

SELECT Employees.EmployeeID, FirstName, LastName, EmployeeTerritories.TerritoryID, Territories.TerritoryDescription FROM Employees
JOIN EmployeeTerritories ON Employees.EmployeeID = EmployeeTerritories.EmployeeID
JOIN Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID
GO