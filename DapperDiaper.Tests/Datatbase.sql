CREATE PROCEDURE [dbo].Contact_Get
	@Id [int]
AS
BEGIN
	SELECT [Id], [FirstName], [LastName], [Email], [Company], [Title]
	FROM [dbo].[Contacts]
	WHERE ([Id] = @Id)
END
