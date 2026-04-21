USE master;
GO

USE SCITest;
GO

ALTER PROCEDURE get_all_products
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(id) AS TotalRows
    FROM products;

    SELECT *
    FROM products
    ORDER BY id DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO