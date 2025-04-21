Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'DutyDone_DB')
BEGIN
    DROP DATABASE DutyDone_DB;
END
Go
Create Database DutyDone_DB
Go
Use DutyDone_DB
Go

CREATE TABLE Users(
UserId INT PRIMARY KEY Identity,        -- מפתח ראשי
Email NVARCHAR(100),           --איימיל של המשתמש
Username NVARCHAR(100),        --השם משתמש
UserPassword NVARCHAR(100),    --הסיסמה של המשתמש
IsAdmin bit default(0) not null ,--האם המשתמש הוא מנהל המערכת
IsBlocked bit default(0) not null,
);

CREATE TABLE TaskType(
TypeId INT PRIMARY KEY ,        --מפתח ראשי 
TypeName NVARCHAR(100)         --סוג המשימה
);

CREATE TABLE TaskStatus(
StatusId int primary key ,      --מפתח ראשי
TypeName NVARCHAR(100)         --סטטוס המשימה
);

CREATE TABLE GroupType(
GroupTypeId INT PRIMARY KEY ,        --מפתח ראשי
GroupTypeName NVARCHAR(100)         --סוג הקבוצה
);

CREATE TABLE Groups(
GroupId INT PRIMARY KEY Identity,        --מפתח ראשי
GroupAdmin Int, --מפתח זר לטבלת משתמשים
GroupName NVARCHAR(100), --שם הקבוצה
Capacity INT, --כמות האנשים בקבוצה
GroupType int, --מפתח זר לטבלת סוג הקבוצה
FOREIGN KEY (GroupType) REFERENCES GroupType(GroupTypeId),   --
FOREIGN KEY (GroupAdmin) REFERENCES Users(UserId)   --
);

CREATE TABLE Tasks(
TaskId INT PRIMARY KEY Identity,        --מפתח ראשי
TaskType INT,                  --מפתח זר לטבלת סוג המשימה
DueDay Date,                   --תאריך סיום המשימה
UserId INT,  --מפתח זר לטבלת המשתמשים
TaskName NVARCHAR(100), --שם המשימה
GroupId int, --מפתח זר לטבלת הקבוצה
StatusId int,  --מפתח זר לטבלת סטטוס המשימה
TaskDescription NVARCHAR(500),
TaskUpdate NVARCHAR(500),
 FOREIGN KEY (GroupId) REFERENCES Groups(GroupId),   -- 
 FOREIGN KEY (UserId) REFERENCES Users(UserId),   -- 
 FOREIGN KEY (StatusId) REFERENCES TaskStatus(StatusId),   --
 FOREIGN KEY (TaskType) REFERENCES TaskType(TypeId)   --
);

CREATE TABLE UsersInGroup(
UserId int FOREIGN KEY REFERENCES Users(UserId),
GroupId int FOREIGN KEY REFERENCES Groups(GroupId),
Primary key (UserId, GroupId)
);



select * from Users





CREATE LOGIN [TaskAdminUser] WITH PASSWORD = 'kukuPassword';
Go

-- Create a user in the TamiDB database for the login
CREATE USER [TaskAdminUser] FOR LOGIN [TaskAdminUser];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [TaskAdminUser];
Go

insert into TaskType values (1, N'test')
insert into TaskType values (2, N'work')
insert into TaskType values (3, N'house chore')
insert into TaskType values (4, N'friendly')
insert into TaskType values (5,N'other')

insert into TaskStatus values (1, N'need to start')
insert into TaskStatus values (2, N'being done')
insert into TaskStatus values (3, N'done')

insert into GroupType values (1,N'class')
insert into GroupType values (2,N'work')
insert into GroupType values (3,N'friends')
insert into GroupType values (4,N'family')
insert into GroupType values (5,N'other')

insert into Users(Email, Username, UserPassword) values ('o@o.com','o','o1')
insert into Users(Email, Username, UserPassword, IsAdmin) values ('a@a.com','a','a1', 1)
insert into Users(Email, Username, UserPassword) values ('ziv@gmail.com','ziv','ziv1')

insert into Groups(GroupAdmin,GroupName,GroupType) values(1,'bffs',3)
insert into Groups(GroupAdmin,GroupName,GroupType) values(2,'porat family',4)
insert into Groups(GroupAdmin,GroupName,GroupType) values(2,'test',4)

insert into UsersInGroup values (2,1)
insert into UsersInGroup values (1,2)

insert into Tasks(TaskType,DueDay,UserId,TaskName,GroupId,StatusId,TaskDescription) values(1,'03/04/2025',1,'math test',1,1,'vektorim and hakirot')
insert into Tasks(TaskType,DueDay,UserId,TaskName,GroupId,StatusId,TaskDescription) values(4,'02/14/2025',1,'going to the mall',1,1,'to big glilot')


SELECT*From Tasks

SELECT*From Groups

SELECT*From Users
select * from UsersInGroup
--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=DutyDone_DB;User ID=TaskAdminUser;Password=kukuPassword;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context ZivDBContext -DataAnnotations -force


