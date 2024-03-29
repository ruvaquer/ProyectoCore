USE [CursosOnline]
GO
/****** Object:  StoredProcedure [dbo].[usp_Obtener_Instructores]    Script Date: 16/05/2023 17:28:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Ruben>
-- Create date: <Create Date,02032023,>
-- Description:	<Description,Recuperamos la lista de instructores,>
-- =============================================
create procedure [dbo].[usp_intructor_nuevo]
@IntructorId uniqueidentifier,
@Nombre nvarchar(500),
@Apellidos nvarchar(500),
@Grado nvarchar(100)
AS
BEGIN
SET NOCOUNT ON --ASI LE INDICO PARA QUE NO ME DEVUELVA VALORES DE OPERACIONES 

insert into Instructor(InstructorId,Nombre,Apellidos,Grado)
values(@IntructorId,@Nombre, @Apellidos, @Grado);

END