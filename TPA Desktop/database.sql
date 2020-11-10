create table [User]
(
    ID           uniqueidentifier not null primary key default newid(),
    FirstName    varchar(50)      not null,
    LastName     varchar(50)      not null,
    Gender       varchar(6)       not null,
    DateOfBirth  date             not null,
    RegisteredAt date             not null             default getdate(),
    DeletedAt    date             null,
    PhoneNumber  varchar(15)      not null
)

create table [EmployeePosition]
(
    ID   uniqueidentifier not null primary key default newid(),
    Name varchar(50)      not null,
)

create table [Employee]
(
    UserID             uniqueidentifier not null foreign key references [User] (ID) on update cascade on delete cascade,
    EmployeePositionID uniqueidentifier not null foreign key references [EmployeePosition] (ID) on update cascade on delete cascade,
    Email              varchar(50)      not null,
    Password           varchar(255)     not null,
    Salary             money            not null
)

create table [Customer]
(
    UserID           uniqueidentifier not null foreign key references [User] (ID) on update cascade on delete cascade,
    IsBusinessOwner  bit              not null,
    MotherMaidenName varchar(50)      not null,
)

insert into [EmployeePosition] (Name)
values ('Teller'),
       ('Customer Service'),
       ('Security and Maintenance'),
       ('Finance'),
       ('Human Resource'),
       ('Manager')

insert into [User] (FirstName, LastName, Gender, DateOfBirth, PhoneNumber)
values ('Brandon', 'Thenaro', 'Male', '2001-01-01', '085155228431'),
       ('Clarissa', 'Chuardi', 'Female', '1970-01-01', '085155228431'),
       ('Johanes', 'Peter', 'Male', '1970-01-01', '085155228431'),
       ('Lionel', 'Ritchie', 'Male', '1970-01-01', '085155228431'),
       ('Skolastika', 'Gabriella', 'Female', '1970-01-01', '085155228431'),
       ('Stanley', 'Teherag', 'Male', '1970-01-01', '085155228431'),
       ('Thaddeus', 'Cleo', 'Male', '1970-01-01', '085155228431'),
       ('Vincent', 'Benedict', 'Male', '1970-01-01', '085155228431')

insert into [Employee] (UserID, EmployeePositionID, Email, Password, Salary)
values ('8A500811-1636-4BA5-B481-0FEA75A9B79C', 'B2939281-76FF-4A52-B41A-2FABC952036F', 'brandon.thenaro@binus.edu',
        'brandon123', 5000000),
       ('3C59F77D-1E1F-4B2C-8F5D-1086A5F9A9D0', '98CDDDFD-1805-4037-99FE-4AF151A8EE87', 'clarissa.chuardi@binus.edu',
        'clarissa123', 5000000),
       ('B1B60840-9979-4E30-8FE6-1BA1E3E7EA31', '4E9BD711-C837-41F9-91B6-70AC951395C0', 'johanes.peter@binus.edu',
        'johanes123', 5000000),
       ('BB6CCA5E-ACF8-4B6E-B78E-31CEB937A82B', '31DC1183-6052-4F73-A96A-969370F59FE3', 'lionel.ritchie@binus.edu',
        'lionel123', 5000000),
       ('3DD31A86-9D21-43CF-97C3-32641C0E7BE5', 'CF2DF8CD-A7B2-4A3F-8898-FB388A259C93',
        'skolastika.gabriella@binus.edu', 'skolastika123', 5000000),
       ('0A4351B9-72E4-4792-8B4C-4430B32E0F01', '3412C77A-D4E2-49EB-8715-FFDB1D2C67A0', 'stanley.teherag@binus.edu',
        'stanley123', 5000000)

insert into [Customer] (UserID, IsBusinessOwner, MotherMaidenName)
values ('549365E9-F3E7-4576-B2EA-4594A3223940', 1, 'Cooking Mama'),
       ('2ADADE88-6C40-4977-963B-4723F6B13032', 0, 'Cooking Mama')

select *
from [User] U
         full join Customer C on U.ID = C.UserID
         full join Employee E on U.ID = E.UserID
         full join EmployeePosition EP on EP.ID = E.EmployeePositionID

drop table [EmployeePosition]
drop table [Customer]
drop table [Employee]
drop table [User]
