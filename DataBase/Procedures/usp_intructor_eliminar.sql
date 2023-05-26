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
-- Description:	<Description,Eliminar la lista de instructores,>
-- =============================================
CREATE procedure [dbo].[usp_intructor_eliminar]
@IntructorId uniqueidentifier
AS
BEGIN
SET NOCOUNT ON --ASI LE INDICO PARA QUE NO ME DEVUELVA VALORES DE OPERACIONES 

delete from CursoInstructor where InstructorId = @IntructorId
delete from Instructor where InstructorId = @IntructorId


END