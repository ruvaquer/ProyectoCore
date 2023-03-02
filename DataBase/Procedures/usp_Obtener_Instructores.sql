SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Ruben>
-- Create date: <Create Date,02032023,>
-- Description:	<Description,Recuperamos la lista de instructores,>
-- =============================================
create procedure usp_Obtener_Instructores
AS
BEGIN
SET NOCOUNT ON --ASI LE INDICO PARA QUE NO ME DEVUELVA VALORES DE OPERACIONES 
SELECT 
X.InstructorId
,X.Nombre
,X.Apellidos
,X.Grado
--,X.FotoPerfil
FROM Instructor AS X
END