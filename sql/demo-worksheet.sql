-------------------------------------------
-- Query 1: see all JSON data
-------------------------------------------
SELECT
     c.json_document.Make, 
     c.json_document.Model, 
     c.json_document.Cylinders
FROM car c;

-------------------------------------------
-- Query 2: See all relational data
-------------------------------------------
SELECT * FROM dealer;

-------------------------------------------
-- Query 3: Join JSON to relational data
-------------------------------------------
SELECT
    d.NAME,
    d.ADDRESS,
    c.json_document.Model, 
    c.json_document.Cylinders
FROM dealer d
INNER JOIN car c ON d.Make = c.json_document.Make
WHERE d.Make = 'Honda';