
/****** Object:  Database [TodoDb]    Script Date: 5/7/2024 4:11:21 PM ******/
CREATE DATABASE [TodoDb]
GO
USE [TodoDb]
GO
/****** Object:  Table [dbo].[Todos]    Script Date: 5/7/2024 4:11:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Todos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Task] [nvarchar](50) NOT NULL,
	[AssignedTo] [int] NOT NULL,
	[IsComplete] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Todos] ADD  DEFAULT ((0)) FOR [IsComplete]
GO
/****** Object:  StoredProcedure [dbo].[spTodos_CompleteTodo]    Script Date: 5/7/2024 4:11:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[spTodos_CompleteTodo]
	@AssignedTo int,
	@TodoId int
as
begin
	update dbo.Todos
	set IsComplete = 1
	where Id = @TodoId and AssignedTo = @AssignedTo
end
GO
/****** Object:  StoredProcedure [dbo].[spTodos_Create]    Script Date: 5/7/2024 4:11:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spTodos_Create]
	@Task nvarchar(50),
	@AssignedTo int
as
begin
	insert into dbo.Todos (Task, AssignedTo)
	values (@Task, @AssignedTo)

	select Id, Task, AssignedTo, IsComplete
	from dbo.Todos
	where Id = scope_identity();  -- we can use @@Identity to get the last created identity value
								-- @@Identity will give you the last identity created in the entire DB
								--  Where scope_identity will give the last created in a given scope

end
GO
/****** Object:  StoredProcedure [dbo].[spTodos_Delete]    Script Date: 5/7/2024 4:11:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[spTodos_Delete]
	@AssignedTo int,
	@TodoId int
as
begin
	delete from dbo.Todos
	where Id = @TodoId and AssignedTo = @AssignedTo
end
GO
/****** Object:  StoredProcedure [dbo].[spTodos_GetAllAssigned]    Script Date: 5/7/2024 4:11:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[spTodos_GetAllAssigned]
	@AssignedTo int
as
begin
	select Id, Task, AssignedTo, IsComplete
	from dbo.Todos
	where AssignedTo = @AssignedTo
end
GO
/****** Object:  StoredProcedure [dbo].[spTodos_GetOneAssigned]    Script Date: 5/7/2024 4:11:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[spTodos_GetOneAssigned]
	@AssignedTo int,
	@TodoId int
as
begin
	select Id, Task, AssignedTo, IsComplete
	from dbo.Todos
	where AssignedTo = @AssignedTo 
			  and Id = @TodoId
end
GO
/****** Object:  StoredProcedure [dbo].[spTodos_UpdateTask]    Script Date: 5/7/2024 4:11:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[spTodos_UpdateTask]
	@Task nvarchar(50),
	@AssignedTo int,
	@TodoId int
as
begin
	update dbo.Todos
	set Task = @Task
	where Id = @TodoId and AssignedTo = @AssignedTo
end
GO

