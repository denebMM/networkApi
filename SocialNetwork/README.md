Project Name: SocialNetwork
Prerequisites
- Visual Studio 2022
- SQL Server Management Sudio v17.9.1
- Project ASP.NET Core 6 Web Api

** DATA BASE**

### Create Data Base using SQL Server
  
Execute the following scripts in SQL Server Management Studio
to create the database and required tables:

CREATE DATABASE SOCIALDB
USE SOCIALDB

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [Posts] ( 
    [Id] int NOT NULL IDENTITY, 
    [Content] nvarchar(max) NOT NULL , 
    [UserId] int NOT NULL , 
    [IsPublic] bit NOT NULL DEFAULT 1, -- Ahora es NOT NULL y con valor predeterminado true  
    [Likes] int,  
    [CreatedAt] datetime NOT NULL DEFAULT GETDATE(), -- Agregando la fecha de creación
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_USERID] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE 
);

CREATE TABLE Likes (
    Id int IDENTITY(1,1) PRIMARY KEY,
    UserId int NOT NULL,
    PostId int NOT NULL,
    CONSTRAINT FK_Likes_Users FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
    CONSTRAINT FK_Likes_Posts FOREIGN KEY ([PostId]) REFERENCES [Posts]([Id])
);

CREATE TABLE [UserFriends] (
    [UserId] int NOT NULL,
    [FriendId] int NOT NULL,
    CONSTRAINT [PK_UserFriends] PRIMARY KEY ([UserId], [FriendId]),
    CONSTRAINT [FK_UserFriends_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE,
    
);

CREATE TABLE [UserFollowers] (
    [UserId] int NOT NULL,
    [FollowerId] int NOT NULL,
    CONSTRAINT [PK_UserFollowers] PRIMARY KEY ([UserId], [FollowerId]),
    CONSTRAINT [FK_UserFollowers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE,
   
);

### Preloaded Sample Data
The database should contain the following preloaded sample data:

INSERT INTO Users([FullName])VALUES ('Sheyla Perez aportela'),('Maria Zayas Esperon'),
('Abel Suarez Pina'),('Mario Morales Montalvo'),('Ana Perez Acosta')

INSERT INTO Posts([Content],[UserId])
VALUES ('#ropa',1),('#funny',2),('#zapatos #ropa',3),
('#childout',4),('#vacation #rest',5)


### Database Model Creation in the Project
To create database models in the project, use the following steps:

1. Open **Tools** -> **NuGet Package Manager** -> **Package Manager Console**.
2. Execute the following command to scaffold the DbContext:
Scaffold-DbContext "Server=(local); DataBase=SOCIALDB; Integrated Security=true" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models
   

## Unit Tests
### Project Name: SocialNetwork.Tests
The project should include unit tests using Xunit.
The total number of tests conducted is 24


### Running Unit Tests in Visual Studio
To execute and view the results of the unit tests:

1. Go to **Test** in the menu bar.
2. Select **Run All Tests**.

To view the executed tests and their outcomes:

1. Navigate to **View** in the menu.
2. Choose **Test Explorer**.







