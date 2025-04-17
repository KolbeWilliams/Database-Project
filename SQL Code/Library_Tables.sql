USE Library

--Employee Table
CREATE TABLE Employee(
	EID INT IDENTITY(1,1),
	first_name VARCHAR(15),
	last_name VARCHAR(15),
	phone_number VARCHAR(15) NOT NULL UNIQUE,
	date_of_birth DATE,
	age AS (DATEDIFF(YEAR, date_of_birth, GETDATE()) -
			CASE 
				WHEN MONTH(date_of_birth) > MONTH(GETDATE()) OR 
					 (MONTH(date_of_birth) = MONTH(GETDATE()) AND DAY(date_of_birth) > DAY(GETDATE()))
				THEN 1 ELSE 0 END),	--Subtract 1 from age if birthday hasn't happended yet this year
	SUID INT,
	PRIMARY KEY (EID),
	CONSTRAINT FK_SUID_Employee
	FOREIGN KEY (SUID) REFERENCES Employee(EID)
);

--Section Table
CREATE TABLE Section(
	SID INT IDENTITY(1,1),
	SUID INT,
	genre VARCHAR(30),
	PRIMARY KEY (SID),
	CONSTRAINT FK_EID_Section
	FOREIGN KEY (SUID) REFERENCES Employee(EID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT Check_genre
		CHECK(genre IN('Science Fiction', 'Fantasy', 'Mystery', 'Romance', 'Thriller', 
			'Historical Fiction', 'Horror', 'Non-fiction', 'Young Adult', 'Childrens Literature'))
);

--Customer Table
CREATE TABLE Customer(
	CID INT IDENTITY(1,1),
	EID INT,
	total DECIMAL(15,2),
	PRIMARY KEY (CID),
	CONSTRAINT FK_EID_Customer
	FOREIGN KEY (EID) REFERENCES Employee(EID)
		ON DELETE SET NULL
		ON UPDATE SET NULL,
	CONSTRAINT Check_total
		CHECK(total >= 0)
);

--Author Table
CREATE TABLE Author(
	AID INT Identity(1,1),
	first_name VARCHAR(15),
	last_name VARCHAR(15),
	SID INT,
	PRIMARY KEY (AID),
	CONSTRAINT FK_SID_Author
	FOREIGN KEY (SID) REFERENCES Section(SID)
		ON DELETE SET NULL
		ON UPDATE SET NULL
);

--Book Table
CREATE TABLE Book(
	ISBN VARCHAR(13),
	AID INT,
	title VARCHAR(50),
	price DECIMAL(15,2),
	number_of_copies INT,
	PRIMARY KEY (ISBN),
	CONSTRAINT FK_AID_Book
	FOREIGN KEY (AID) REFERENCES Author(AID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,
	CONSTRAINT Check_price
		CHECK(price >= 0),
	CONSTRAINT Check_number_of_copies
		CHECK(number_of_copies >= 0)
);

--Book_Copy Table
CREATE TABLE Book_Copy(
	ISBN VARCHAR(13),
	library_book_id INT,
	PRIMARY KEY (ISBN, library_book_id),
	CONSTRAINT FK_ISBN_Book_Copy
	FOREIGN KEY (ISBN) REFERENCES Book(ISBN)
		ON DELETE CASCADE
		ON UPDATE CASCADE
);
