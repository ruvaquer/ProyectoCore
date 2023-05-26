USE [CursosOnline]
GO
/****** Object:  StoredProcedure [dbo].[usp_intructor_nuevo]    Script Date: 26/05/2023 18:39:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Ruben>
-- Create date: <Create Date,02032026,>
-- Description:	<Description,Actualizar la lista de instructores,>
-- =============================================
CREATE procedure [dbo].[usp_intructor_editar]
@IntructorId uniqueidentifier,
@Nombre nvarchar(500),
@Apellidos nvarchar(500),
@Grado nvarchar(100)
AS
BEGIN
SET NOCOUNT ON --ASI LE INDICO PARA QUE NO ME DEVUELVA VALORES DE OPERACIONES 

update Instructor
set
Nombre = @Nombre,
Apellidos = @Apellidos,
Grado = @Grado
where 
InstructorId = @IntructorId

END