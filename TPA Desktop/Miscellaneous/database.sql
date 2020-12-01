/*--------------------------------------------------------------------------------------------------------------------*
 |                                                   Setup Database                                                   |
 *--------------------------------------------------------------------------------------------------------------------*/

use master
go

drop database [TPA Desktop]
go

create database [TPA Desktop]
use [TPA Desktop]
go

/*--------------------------------------------------------------------------------------------------------------------*
 |                                                         DDL                                                        |
 *--------------------------------------------------------------------------------------------------------------------*/

create table [User]
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
    Salary             money            not null,
)

create table [EmployeeViolation]
(
    ID         uniqueidentifier not null primary key default newid(),
    EmployeeID uniqueidentifier not null foreign key references [Employee] (ID) on update cascade on delete cascade,
    Title      varchar(50)      not null,
    Comment    text             not null,
    ViolatedAt datetime2        not null,
    DeletedAt  datetime2        not null,
)

create table [Customer]
(
    ID               uniqueidentifier not null primary key foreign key references [User] (ID) on update cascade on delete cascade,
    IsBusinessOwner  bit              not null,
    MotherMaidenName varchar(50)      not null,
)

create table [Account]
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
    IsBusiness              bit              not null default 0,
    RegularAccountNumber    char(16)         null,

    primary key (CustomerID, AccountNumber)
)

create table [QRCode]
(
    ID        uniqueidentifier not null primary key default newid(),
    URL       text             not null,
    CreatedAt datetime2        not null             default getdate()
)

create table [Queue]
(
    ID             uniqueidentifier not null primary key default newid(),
    QueuedAt       datetime2        not null             default getdate(),
    ServiceStartAt datetime2        null,
    ServedAt       datetime2        null,
    QRCodeID       uniqueidentifier null foreign key references [QRCode] (ID),
)

create table [TellerQueue]
(
    ID     uniqueidentifier primary key not null foreign key references [Queue] (ID),
    Number bigint                       not null identity,
)

create table [CustomerServiceQueue]
(
    ID     uniqueidentifier not null primary key foreign key references [Queue] (ID),
    Number bigint           not null identity,
)

create table [CustomerSatisfaction]
(
    ID          uniqueidentifier not null primary key default newid(),
    QRCodeID    uniqueidentifier not null foreign key references [QRCode] (ID) on update cascade on delete cascade,
    Rating      tinyint          not null,
    Description text             not null,
    SubmittedAt datetime2        not null             default getdate(),
)

create table [VirtualAccount]
(
    ID                       uniqueidentifier not null primary key default newid(),
    SourceAccountNumber      char(16)         not null,
    DestinationAccountNumber char(16)         not null,
    VirtualAccountNumber     char(16)         not null unique,
    Amount                   money            not null,
    CreatedAt                datetime2        not null             default getdate(),
    ExpiredAt                datetime2        not null             default dateadd(day, 3, getdate()),
    PaidAt                   datetime2        null,
)

alter table [VirtualAccount]
    add constraint fk_source
        foreign key (SourceAccountNumber) references [Account] (AccountNumber)

alter table [VirtualAccount]
    add constraint fk_destination
        foreign key (DestinationAccountNumber) references [Account] (AccountNumber)

create table [TransactionType]
(
    ID   uniqueidentifier not null primary key default newid(),
    Name varchar(25)      not null unique,
)

create table [PaymentType]
(
    ID   uniqueidentifier not null primary key default newid(),
    Name varchar(25)      not null unique,
)

create table [Transaction]
(
    ID                uniqueidentifier not null default newid(),
    AccountNumber     char(16)         not null foreign key references [Account] (AccountNumber),
    CustomerID        uniqueidentifier not null foreign key references [Customer] (ID),
    PaymentTypeID     uniqueidentifier null foreign key references [PaymentType] (ID),
    TransactionTypeID uniqueidentifier not null foreign key references [TransactionType] (ID),
    Date              datetime2        not null,
    Amount            money            not null,

    primary key (ID, CustomerID)
)

create table [DebitCard]
(
    ID            uniqueidentifier not null primary key default newid(),
    AccountNumber char(16)         not null foreign key references [Account] (AccountNumber),
)

create table [RequestStatus]
(
    ID   uniqueidentifier not null primary key default newid(),
    Name varchar(25)      not null unique,
)

create table [Request]
(
    ID              uniqueidentifier not null primary key default newid(),
    CreatedAt       datetime2        not null             default getdate(),
    UpdatedAt       datetime2        null,
    RequestStatusID uniqueidentifier not null foreign key references [RequestStatus] (ID)
)

create table [ExpenseRequestType]
(
    ID   uniqueidentifier primary key not null default newid(),
    Name varchar(25)                  not null unique,
)

create table [ExpenseRequest]
(
    ID                   uniqueidentifier not null primary key foreign key references [Request] (ID) default newid(),
    ExpenseRequestTypeID uniqueidentifier not null foreign key references [ExpenseRequestType] (ID),
    EntityID             uniqueidentifier not null unique,
)

create table [CreditCardCompany]
(
    ID          uniqueidentifier not null primary key default newid(),
    Email       varchar(50)      not null unique,
    Name        varchar(25)      not null unique,
    PhoneNumber char(12)         not null unique,
    Address     varchar(255)     not null
)

create table [CreditCard]
(
    ID                  uniqueidentifier not null primary key default newid(),
    CreditCardCompanyID uniqueidentifier not null foreign key references [CreditCardCompany] (ID),
    AccountNumber       char(16)         not null foreign key references [Account] (AccountNumber),
)

create table [DocumentType]
(
    ID   uniqueidentifier not null primary key default newid(),
    Name varchar(50)      not null unique,
)

create table [Document]
(
    ID         uniqueidentifier not null primary key default newid(),
    Value      money            not null,
    Comment    varchar(255)     not null,
    DocumentId varchar(255)     not null unique,
)

create table [Notification]
(
    ID                 uniqueidentifier not null primary key default newid(),
    Title              varchar(100)     not null,
    CreatedAt          datetime2        not null             default getdate(),
    ReadAt             datetime2        null,
    EmployeePositionID uniqueidentifier not null foreign key references [EmployeePosition] (ID),
)

create table [Charge]
(
    ID            uniqueidentifier not null primary key default newid(),
    Amount        money            not null,
    Description   varchar(100)     not null,
    DueAt         datetime2        not null,
    PaidAt        datetime2        null,
    AccountNumber char(16)         not null foreign key references [Account] (AccountNumber),
)

create table [BusinessCard]
(
    ID                       uniqueidentifier not null primary key default newid(),
    Name                     varchar(50)      not null,
    CanWithdraw              bit              not null,
    MaximumTransactionAmount money            not null,
    SupportsForeignCurrency  bit              not null,
)

create table [EquipmentCondition]
(
    ID   uniqueidentifier not null primary key default newid(),
    Name varchar(50)      not null,
)

create table [Equipment]
(
    ID                   uniqueidentifier not null primary key default newid(),
    Name                 varchar(50)      not null,
    EquipmentConditionID uniqueidentifier not null foreign key references EquipmentCondition (ID),
    Floor                int              not null,
    DeletedAt            datetime2        null,
)

create table [BrokenEquipmentReport]
(
    ID          uniqueidentifier not null primary key default newid(),
    EquipmentId uniqueidentifier not null foreign key references Equipment (ID),
    ReportedAt  datetime2        not null,
    Description varchar(255)     not null             default '',
    EmployeeId  uniqueidentifier not null foreign key references Employee (ID)
)

/*--------------------------------------------------------------------------------------------------------------------*
 |                                                         DML                                                        |
 *--------------------------------------------------------------------------------------------------------------------*/

select *
from CreditCardCompany
insert into EmployeePosition (Name)
values ('Teller'),
       ('Customer Service'),
       ('Security and Maintenance'),
       ('Finance'),
       ('Human Resource'),
       ('Manager')

insert into TransactionType (Name)
values ('Payment'),
       ('Withdraw'),
       ('Deposit'),
       ('Transfer'),
       ('Transfer Virtual Account')

insert into PaymentType (Name)
values ('Pulse'),
       ('Electric Pulse'),
       ('Insurance')

insert into RequestStatus (Name)
values ('Approved'),
       ('Rejected'),
       ('Pending')

insert into ExpenseRequestType (Name)
values ('Credit Card')

select *
from CreditCardCompany
insert into CreditCardCompany (Email, Name, PhoneNumber, Address)
values ('support@visa.com', 'Visa', '081212341234', 'Visa Street'),
       ('support@mastercard.com', 'Master Card', '081257294812', 'Master Card Street'),
       ('support@citibank.com', 'Citibank', '081746285943', 'Citibank Street'),
       ('support@americanexpress.com', 'American Express', '081763458903', 'American Express Street'),
       ('support@bankofamerica.com', 'Bank of America', '081827634551', 'Bank of America Street')

insert into DocumentType (Name)
values ('Guarantee'),
       ('Gross income per month')

insert into EquipmentCondition (Name)
values ('Good'),
       ('Pending'),
       ('Broken')

insert into Equipment (Name, EquipmentConditionID, Floor)
values ('Papan Tulis', (select ID from EquipmentCondition where name = 'Good'), 1),
       ('Spidol', (select ID from EquipmentCondition where name = 'Good'), 1),
       ('PC 1', (select ID from EquipmentCondition where name = 'Good'), 1),
       ('PC 2', (select ID from EquipmentCondition where name = 'Good'), 1),
       ('PC 3', (select ID from EquipmentCondition where name = 'Good'), 1),
       ('PC 4', (select ID from EquipmentCondition where name = 'Good'), 1),
       ('PC 5', (select ID from EquipmentCondition where name = 'Good'), 1),
       ('PC 6', (select ID from EquipmentCondition where name = 'Good'), 1),
       ('PC 7', (select ID from EquipmentCondition where name = 'Good'), 1),
       ('PC 8', (select ID from EquipmentCondition where name = 'Good'), 1)

/*--------------------------------------------------------------------------------------------------------------------*
 |                                                         DQL                                                        |
 *--------------------------------------------------------------------------------------------------------------------*/
select *
from [EquipmentCondition]
-- Employee login

select Email, Password, EP.Name, U.Gender
from Employee E
         join EmployeePosition EP on EP.ID = E.EmployeePositionID
         join [User] U on E.ID = u.ID

select AccountNumber, Name, Date, Amount
from [Transaction] T
         join TransactionType TT on TT.ID = T.TransactionTypeID

select T.AccountNumber, T.CustomerID, T.Date, T.Amount, PT.Name, TT.Name
from [Transaction] T
         join PaymentType PT on T.PaymentTypeID = PT.ID
         join TransactionType TT on TT.ID = T.TransactionTypeID

select FirstName,
       LastName,
       DateOfBirth,
       MotherMaidenName,
       AccountNumber,
       Balance,
       IsBusinessOwner,
       Gender,
       BlockedAt,
       ClosedAt
from Account A
         join Customer C on C.ID = A.CustomerID
         join [User] U on C.ID = U.ID

select SourceAccountNumber,
       DestinationAccountNumber,
       VirtualAccountNumber,
       Amount,
       PaidAt,
       A.Balance,
       A2.Balance,
       FirstName,
       LastName,
       DateOfBirth,
       MotherMaidenName
from VirtualAccount VA
         join Account A on VA.SourceAccountNumber = A.AccountNumber
         join Account A2 on VA.DestinationAccountNumber = A2.AccountNumber
         join Customer C on C.ID = A.CustomerID
         join [User] U on C.ID = U.ID
where PaidAt is null
order by VA.CreatedAt desc

select U.FirstName, U.LastName, BlockedAt, AccountNumber, Balance
from Account A
         join Customer C on A.CustomerID = C.ID
         join [User] U on C.ID = U.ID

select FirstName, LastName, Email, Gender, DateOfBirth, Salary, PhoneNumber
from Employee E
         join [User] U on U.ID = E.ID

select count(T.PaymentTypeID), count(T.TransactionTypeID)
from [Transaction] T

select TT.Name, count(T.TransactionTypeID)
from [Transaction] T
         join TransactionType TT on T.TransactionTypeID = TT.ID
group by TT.Name

select TT.Name, count(T.PaymentTypeID)
from [Transaction] T
         join PaymentType TT on T.PaymentTypeID = TT.ID
group by TT.Name

select *
from Customer C
         JOIN [User] U on U.ID = C.ID

select *
from DebitCard DC
         full join Account A
                   on DC.AccountNumber = A.AccountNumber

select ERT.Name,
       RS.Name,
       A.AccountNumber,
       FirstName,
       LastName,
       CCC.Name,
       R.CreatedAt,
       UpdatedAt
from [ExpenseRequest] ER
         join [Request] R
              on ER.ID = R.ID
         join [RequestStatus] RS
              on R.RequestStatusID = RS.ID
         join [ExpenseRequestType] ERT
              on ER.ExpenseRequestTypeID = ERT.ID
         join [CreditCard] CC
              on ER.EntityID = CC.ID
         join [CreditCardCompany] CCC
              on CC.CreditCardCompanyID = CCC.ID
         join [Account] A
              on A.AccountNumber = CC.AccountNumber
         join [Customer] C
              on C.ID = A.CustomerID
         join [User] U
              on U.ID = C.ID
select *
from ExpenseRequest Er
         join Request R on Er.ID = R.ID

select *
from [Document]

select *
from [Notification]

select *
from Account

update Request
set RequestStatusId = (select ID from RequestStatus where Name = 'Approved')
where ID = 'E745590D-D420-41EB-B6FA-2B365EFF5A6A'

select R.id, AccountNumber, RS.Name
from Request R
         join RequestStatus RS on R.RequestStatusID = RS.ID
         join ExpenseRequest ER on R.ID = ER.ID
         join CreditCard CC on ER.EntityID = CC.ID

-- CC Request

select RS.Name
from CreditCard CC
         join ExpenseRequest ER on CC.ID = ER.EntityID
         join Request R on ER.ID = R.ID
         join RequestStatus RS on R.RequestStatusID = RS.ID

-- CC Charge

select *
from Charge