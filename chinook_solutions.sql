SELECT FirstName + ' ' + LastName as FullName, CustomerId, Country FROM Customer WHERE Country != 'USA';

-- From Brazil
SELECT FirstName + ' ' + LastName as FullName, CustomerId, Country FROM Customer WHERE Country = 'Brazil';

-- All sales agents
SELECT * FROM Employee WHERE Title = 'Sales Support Agent';

-- Retrieve a list of all countries in billing addresses on invoices
SELECT DISTINCT BillingCountry FROM Invoice;

-- Retrieve how many invoices there were in 2009 (2021), and what was the sales total for that year?
SELECT COUNT(*), SUM(Total) AS SalesTotal FROM Invoice WHERE YEAR(InvoiceDate) = 2021;

-- (challenge: find the invoice count sales total for every year using one query)
SELECT YEAR(InvoiceDate) AS Year, SUM(Total) AS SalesTotal FROM Invoice GROUP BY YEAR(InvoiceDate);

-- how many line items were there for invoice #37
SELECT COUNT(InvoiceLineId) FROM InvoiceLine WHERE InvoiceId = 37;

-- how many invoices per country? BillingCountry # of invoices -
SELECT COUNT(InvoiceId) AS InvoiceCount, BillingCountry FROM Invoice GROUP BY BillingCountry;

-- Retrieve the total sales per country, ordered by the highest total sales first.
SELECT BillingCountry, COUNT(InvoiceId) AS TotalSales FROM Invoice GROUP BY BillingCountry ORDER BY COUNT(InvoiceId) DESC;

-- JOINS CHALLENGES 
-- Every Album by Artist
SELECT al.Title, ar.Name FROM Album al JOIN Artist ar ON al.ArtistId = ar.ArtistId;

-- All songs of the rock genre
SELECT t.*, g.Name FROM Track t JOIN Genre g ON t.GenreId = g.GenreId;

-- Show all invoices of customers from brazil (mailing address not billing)
SELECT i.* FROM Invoice i JOIN Customer c ON i.CustomerId = c.CustomerId WHERE c.Country = 'Brazil';

-- Show all invoices together with the name of the sales agent for each one 
Select i.InvoiceId, i.InvoiceDate, c.CustomerId, c.FirstName + ' ' + c.LastName 'Customer', e.FirstName + ' ' + e.LastName 'Sales Agent' From Invoice i JOIN Customer c ON i.CustomerId = c.CustomerId JOIN Employee e ON c.SupportRepId = e.EmployeeId

-- Which sales agent made the most sales in 2009 (2021)?
SELECT TOP 1 e.FirstName + ' ' + e.LastName AS FullName, COUNT(InvoiceId) 
FROM Invoice i 
JOIN Customer c ON i.CustomerId = c.CustomerId 
JOIN Employee e ON c.SupportRepId = e.EmployeeId 
WHERE YEAR(InvoiceDate) = 2021 
GROUP BY e.EmployeeId, e.FirstName, e.LastName 
ORDER BY COUNT(InvoiceId) DESC;

-- How many customers are assigned to each sales agent?
SELECT e.FirstName + ' ' + e.LastName AS FullName, COUNT(CustomerId) FROM Customer c JOIN Employee e ON c.SupportRepId = e.EmployeeId GROUP BY e.EmployeeId, e.FirstName, e.LastName;

-- Which track was purchased the most in 2010 (2022)?
SELECT TOP 1 t.Name, SUM(il.Quantity) 
FROM Track t 
JOIN InvoiceLine il ON t.TrackId = il.TrackId 
JOIN Invoice i ON il.InvoiceId = i.InvoiceId 
WHERE YEAR(i.InvoiceDate) = 2022 
GROUP BY t.TrackId, t.Name 
ORDER BY SUM(il.Quantity) DESC;

-- Show the top three best selling artists.
SELECT TOP 3 ar.Name, SUM(Quantity) 
FROM Artist ar 
JOIN Album al ON ar.ArtistId = al.ArtistId 
JOIN Track t ON al.AlbumId = t.AlbumId 
JOIN InvoiceLine il ON t.TrackId = il.TrackId 
GROUP BY ar.ArtistId, ar.Name 
ORDER BY SUM(il.Quantity) DESC;

-- Which customers have the same initials as at least one other customer?
SELECT c.FirstName, c.LastName FROM Customer c WHERE SUBSTRING(FirstName, 1, 1) + SUBSTRING(LastName, 1, 1) IN
(SELECT SUBSTRING(FirstName, 1, 1) + SUBSTRING(LastName, 1, 1) FROM Customer GROUP BY SUBSTRING(FirstName, 1, 1) + SUBSTRING(LastName, 1, 1) HAVING COUNT(CustomerId) > 1);

-- ADVACED CHALLENGES -- solve these with a mixture of joins, subqueries, CTE, and set operators. -- solve at least one of them in two different ways, and see if the execution -- plan for them is the same, or different.

-- 1. which artists did not make any albums at all?
SELECT ar.Name FROM Artist ar LEFT JOIN Album al ON ar.ArtistId = al.ArtistId WHERE al.AlbumId IS NULL;
SELECT Name FROM Artist WHERE ArtistId NOT IN (SELECT ArtistId FROM Album);

-- 2. which artists did not record any tracks of the Latin genre?
SELECT DISTINCT ar.Name FROM Artist ar JOIN Album al ON ar.ArtistId = al.ArtistId WHERE al.AlbumId NOT IN 
(SELECT AlbumId FROM Track t JOIN Genre g ON t.GenreId = g.GenreId WHERE g.Name = 'Latin');

-- 3. which video track has the longest length? (use media type table)
SELECT t.Name, t.Milliseconds FROM Track t JOIN MediaType m ON t.MediaTypeId = m.MediaTypeId WHERE m.Name LIKE '%video%' ORDER BY t.Milliseconds DESC;

-- 4. find the names of the customers who live in the same city as the -- boss employee (the one who reports to nobody)
SELECT FirstName, LastName FROM Customer WHERE City IN 
(SELECT City FROM Employee WHERE ReportsTo IS NULL);

-- 5. how many audio tracks were bought by German customers, and what was -- the total price paid for them?
SELECT COUNT(InvoiceLineId) AS AudioTracksPurchased, SUM(Total) AS TotalCost
FROM Customer c 
JOIN Invoice i ON c.CustomerId = i.CustomerId 
JOIN InvoiceLine il ON i.InvoiceId = il.InvoiceId
JOIN Track t ON il.TrackId = t.TrackId
JOIN MediaType m ON t.MediaTypeId = m.MediaTypeId 
WHERE c.Country = 'Germany' AND m.Name LIKE '%audio%';

-- 6. list the names and countries of the customers supported by an employee -- who was hired younger than 35.
SELECT c.FirstName, c.LastName, c.Country FROM Customer c JOIN Employee e ON c.SupportRepId = e.EmployeeId WHERE DATEDIFF(YEAR, BirthDate, HireDate) < 35;

-- DML exercises

-- 1. insert two new records into the employee table.
INSERT INTO Employee (EmployeeId, LastName, FirstName) VALUES
((SELECT MAX(EmployeeId) + 1 FROM Employee), 'Billy', 'Bobby'),
((SELECT MAX(EmployeeId) + 2 FROM Employee), 'John', 'Jimmy');

-- 2. insert two new records into the tracks table.
INSERT INTO Track(TrackId, Name, MediaTypeId, Milliseconds, UnitPrice) VALUES
((SELECT MAX(TrackId) + 1 FROM Track), 'New Track A', 1, 10000, 0.99),
((SELECT MAX(TrackId) + 2 FROM Track), 'New Track B', 1, 10000, 0.99);

-- 3. update customer Aaron Mitchell's name to Robert Walter
UPDATE Customer SET FirstName = 'Robert', LastName = 'Walter' WHERE FirstName = 'Aaron' AND LastName = 'Mitchell';

-- 4. delete one of the employees you inserted.
DELETE FROM Employee WHERE FirstName = 'Jimmy' AND LastName = 'John';

-- 5. delete customer Robert Walter.
DELETE FROM Customer WHERE FirstName = 'Robert' AND LastName = 'Walter';
