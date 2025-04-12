go
CREATE VIEW Section_Authors AS
	SELECT s.genre, a.first_name, a.last_name
	FROM Section s
	JOIN Author a ON s.SID = a.SID;
go

SELECT * FROM Section_Authors ORDER BY genre;

go
CREATE VIEW Average_Price_For_Author AS
	SELECT a.first_name + ' ' + a.last_name AS Author_Name,
	AVG(b.price) AS Average_Book_Price
	FROM Author a
	JOIN Book B ON a.AID = b.AID
	GROUP BY a.first_name, a.last_name;
go

SELECT * FROM Average_Price_For_Author;

go
CREATE FUNCTION dbo.Author_Price (@first VARCHAR(15), @last VARCHAR(15))
RETURNS Decimal(15,2)
AS
BEGIN
DECLARE @avg_price DECIMAL(15,2);
	SELECT @avg_price = Average_Book_Price
	FROM Average_Price_For_Author
	WHERE Author_Name = @first + ' ' + @last
	RETURN @avg_price;
END;
go

SELECT dbo.Author_Price('Terry', 'Pratchett') AS Average_Price;

go
CREATE PROCEDURE Offboarding
	@EID INT
AS
BEGIN
	--If Employee Exists
	IF EXISTS (SELECT 1 FROM Employee WHERE EID = @EID)
	BEGIN
		DELETE FROM Employee WHERE EID = @EID
	END
	ELSE
		PRINT 'Employee with specified EID does not exist';
END;
go

EXEC Offboarding 33;
SELECT * FROM Employee WHERE EID = 30;

go
CREATE TRIGGER updateBookCopies
ON Book_Copy
AFTER INSERT, DELETE, UPDATE
AS 
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Book_Copy WHERE ISBN IN 
        (SELECT DISTINCT ISBN FROM inserted
        UNION
        SELECT DISTINCT ISBN FROM deleted))
    BEGIN
        RAISERROR('ERROR: The library book ID doesnt exist for the given ISBN', 16, 1)
        ROLLBACK TRANSACTION;
    END
    ELSE
	    UPDATE b
	    SET b.number_of_copies = bc.copy_count
		    FROM Book b
		    JOIN (
            SELECT ISBN, COUNT(*) AS copy_count
            FROM Book_Copy
            GROUP BY ISBN
        ) bc ON b.ISBN = bc.ISBN
        WHERE b.ISBN IN (SELECT DISTINCT ISBN FROM inserted
                          UNION
                          SELECT DISTINCT ISBN FROM deleted);
END;
go

SELECT number_of_copies FROM Book WHERE ISBN = 9780199650726;
SELECT * FROM Book_Copy WHERE ISBN = 9780199650726;
DELETE FROM Book_Copy WHERE ISBN = 9780199650726 AND library_book_id = 10;