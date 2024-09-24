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
);

CREATE TABLE TaskType(
TypeId INT PRIMARY KEY ,        --מפתח ראשי 
TypeName NVARCHAR(100)         --סוג המשימה
);

CREATE TABLE TaskStatus(
StatusId int primary key ,      --מפתח ראשי
TypeName NVARCHAR(100)         --סטטוס המשימה
);

CREATE TABLE Tasks(
TaskId INT PRIMARY KEY Identity,        --מפתח ראשי
TaskType INT,                  --מפתח זר לטבלת סוג המשימה
DueDay Date,                   --תאריך סיום המשימה
UserId INT,  --מפתח זר לטבלת המשתמשים
TaskName NVARCHAR(100), --שם המשימה
GroupId int, --מפתח זר לטבלת הקבוצה
StatusId int  --מפתח זר לטבלת סטטוס המשימה
 FOREIGN KEY (GroupId) REFERENCES Groups(GroupId)   -- 
 FOREIGN KEY (UserId) REFERENCES Users(UserId)   -- 
 FOREIGN KEY (StatusId) REFERENCES TaskStatus(StatusId)   --
 FOREIGN KEY (TaskType) REFERENCES TaskType(TypeId)   --
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

CREATE TABLE GroupType(
GroupTypeId INT PRIMARY KEY ,        --מפתח ראשי
GroupTypeName NVARCHAR(100)         --סוג הקבוצה
);

insert into TaskType values (1, N'מבחן')
insert into TaskType values (1, N'עבודה')
insert into TaskType values (1, N'מטלת בית')
insert into TaskType values (1, N'חברית')


