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
    ID                 uniqueidentifier not null primary key foreign key references [User] (ID) on update cascade on delete cascade,
    EmployeePositionID uniqueidentifier not null foreign key references [EmployeePosition] (ID) on update cascade on delete cascade,
    Email              varchar(50)      not null,
    Password           varchar(255)     not null,
    Salary             money            not null
)

create table [EmployeeViolation]
(
    ID         uniqueidentifier not null primary key default newid(),
    EmployeeID uniqueidentifier not null foreign key references [Employee] (ID) on update cascade on delete cascade,
    Title      varchar(50)      not null,
    Comment    text             not null,
    ViolatedAt datetime2        not null,
    DeletedAt  datetime2        not null
)

create table [Customer]
(
    ID               uniqueidentifier not null primary key foreign key references [User] (ID) on update cascade on delete cascade,
    IsBusinessOwner  bit              not null,
    MotherMaidenName varchar(50)      not null,
)
alter table [Account]
    add UseAutomaticRollOver bit
create table [Account]
(
    CustomerID              uniqueidentifier not null foreign key references [Customer] (ID) on update cascade on delete cascade,
    AccountNumber           bigint           not null unique,
    Balance                 money            not null,
    Interest                real             not null,
    MaximumWithdrawalAmount money            not null,
    MaximumTransferAmount   money            not null,
    GuardianAccountNumber   bigint           null,
    SupportForeignCurrency  bit              not null,
    Name                    varchar(50)      not null,
    BlockedAt               datetime2        not null,
    CreatedAt               datetime2        not null,
    ClosedAt                datetime2        not null,
    AdministrationFee       money            not null,
    MinimumSavingAmount     money            not null,
    UseAutomaticRollOver    bit              not null,
    
    primary key (CustomerID, AccountNumber)
)

insert into [EmployeePosition] (Name)
values ('Teller'),
       ('Customer Service'),
       ('Security and Maintenance'),
       ('Finance'),
       ('Human Resource'),
       ('Manager')

drop table [EmployeePosition]
drop table [Customer]
drop table [Employee]
drop table [User]

select U.ID,
       FirstName,
       LastName,
       Gender,
       DateOfBirth,
       RegisteredAt,
       DeletedAt,
       PhoneNumber,
       IsBusinessOwner,
       MotherMaidenName
from [User] U
         join Customer C on U.ID = C.ID
