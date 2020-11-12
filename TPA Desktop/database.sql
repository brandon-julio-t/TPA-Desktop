create table User
(
    ID           uniqueidentifier not null primary key default newid(),
    FirstName    varchar(50)      not null,
    LastName     varchar(50)      not null,
    Gender       varchar(6)       not null,
    DateOfBirth  date             not null,
    RegisteredAt date             not null             default getdate(),
    DeletedAt    date             null,
    PhoneNumber  varchar(15)      not null,
)

create table EmployeePosition
(
    ID   uniqueidentifier not null primary key default newid(),
    Name varchar(50)      not null,
)

create table Employee
(
    ID                 uniqueidentifier not null primary key foreign key references [User] (ID) on update cascade on delete cascade,
    EmployeePositionID uniqueidentifier not null foreign key references [EmployeePosition] (ID) on update cascade on delete cascade,
    Email              varchar(50)      not null,
    Password           varchar(255)     not null,
    Salary             money            not null,
)

create table EmployeeViolation
(
    ID         uniqueidentifier not null primary key default newid(),
    EmployeeID uniqueidentifier not null foreign key references [Employee] (ID) on update cascade on delete cascade,
    Title      varchar(50)      not null,
    Comment    text             not null,
    ViolatedAt datetime2        not null,
    DeletedAt  datetime2        not null,
)

create table Customer
(
    ID               uniqueidentifier not null primary key foreign key references [User] (ID) on update cascade on delete cascade,
    IsBusinessOwner  bit              not null,
    MotherMaidenName varchar(50)      not null,
)

create table Account
(
    CustomerID              uniqueidentifier not null foreign key references [Customer] (ID) on update cascade on delete cascade,
    AccountNumber           char(16)         not null unique,
    Balance                 money            not null,
    Interest                real             not null,
    MaximumWithdrawalAmount money            not null,
    MaximumTransferAmount   money            not null,
    GuardianAccountNumber   char(16)         null,
    SupportForeignCurrency  bit              not null,
    Name                    varchar(50)      not null,
    BlockedAt               datetime2        null,
    CreatedAt               datetime2        not null,
    ClosedAt                datetime2        null,
    AdministrationFee       money            not null,
    MinimumSavingAmount     money            not null,
    UseAutomaticRollOver    bit              not null,

    primary key (CustomerID, AccountNumber)
)

create table Queue
(
    ID             uniqueidentifier not null primary key default newid(),
    QueuedAt       datetime2        not null             default getdate(),
    ServiceStartAt datetime2        null,
    ServedAt       datetime2        null,
)

create table TellerQueue
(
    ID     uniqueidentifier primary key not null foreign key references Queue (ID),
    Number bigint                       not null identity,
)

create table CustomerServiceQueue
(
    ID     uniqueidentifier not null primary key foreign key references Queue (ID),
    Number bigint           not null identity,
)

create table QRCode
(
    ID        uniqueidentifier not null primary key default newid(),
    URL       text             not null,
    CreatedAt datetime2        not null             default getdate(),
)

create table CustomerSatisfaction
(
    ID          uniqueidentifier not null primary key default newid(),
    QRCodeID    uniqueidentifier not null foreign key references QRCode (ID) on update cascade on delete cascade,
    Rating      tinyint          not null,
    Description text             not null,
    SubmittedAt datetime2        not null             default getdate(),
)

create table VirtualAccount
(
    
)

insert into [EmployeePosition] (Name)
values ('Teller'),
       ('Customer Service'),
       ('Security and Maintenance'),
       ('Finance'),
       ('Human Resource'),
       ('Manager')

select *
from Account A join Customer C on A.CustomerID = C.ID join [User] U on U.ID = C.ID
where AccountNumber = '8766153557599758'
