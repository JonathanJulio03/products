CREATE DATABASE SCITest;
GO

USE SCITest;
GO

CREATE TABLE products (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(150) NOT NULL,
    description NVARCHAR(500) NULL,
    price DECIMAL(18,2) NOT NULL,
    created_date DATETIME DEFAULT GETDATE()
);
GO

CREATE PROCEDURE get_all_products
AS
BEGIN
    SELECT * 
    FROM products;
END;
GO

CREATE PROCEDURE get_product_by_id
    @id INT
AS
BEGIN
    SELECT * 
    FROM products 
    WHERE id = @id;
END;
GO

CREATE PROCEDURE create_product
    @name NVARCHAR(150),
    @description NVARCHAR(500),
    @price DECIMAL(18,2),
    @new_id INT OUTPUT
AS
BEGIN
    INSERT INTO products (name, description, price)
    VALUES (@name, @description, @price);

    SET @new_id = SCOPE_IDENTITY();
END;
GO

CREATE PROCEDURE update_product
    @id INT,
    @name NVARCHAR(150),
    @description NVARCHAR(500),
    @price DECIMAL(18,2)
AS
BEGIN
    UPDATE products
    SET name = @name,
        description = @description,
        price = @price
    WHERE id = @id;
END;
GO

CREATE PROCEDURE delete_product
    @id INT
AS
BEGIN
    DELETE FROM products 
    WHERE id = @id;
END;
GO