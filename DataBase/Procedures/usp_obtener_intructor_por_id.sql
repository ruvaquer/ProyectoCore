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
-- Description:	<Description,Recuperar instructor,>
-- =============================================
CREATE procedure [dbo].[usp_obtener_intructor_por_id]
@Id uniqueidentifier
AS
BEGIN
SET NOCOUNT ON --ASI LE INDICO PARA QUE NO ME DEVUELVA VALORES DE OPERACIONES 

select 
InstructorId,
Nombre,
Apellidos,
Grado
from Instructor 
where InstructorId = @Id 


END