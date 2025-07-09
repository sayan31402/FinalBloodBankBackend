use BloodBankManagementSystem_010;

-- query area BEGIN

-- query area END
------------------
-- Initial

insert into  Stocks values 
('AB+',0,GETDATE(),GETDATE()),
('A+',0,GETDATE(),GETDATE()),
('B+',0,GETDATE(),GETDATE()),
('O+',0,GETDATE(),GETDATE()),
('AB-',0,GETDATE(),GETDATE()),
('A-',0,GETDATE(),GETDATE()),
('B-',0,GETDATE(),GETDATE()),
('O-',0,GETDATE(),GETDATE());

insert into  Persons values 
('P2','Female','B+','CITY2',1111111112, 'p2@email.com', GETDATE(),GETDATE()),
('P3','Male','AB+','CITY3',1111111113, 'p3@email.com', GETDATE(),GETDATE()),
('P4','Female','O+','CITY4',1111111114, 'p4@email.com', GETDATE(),GETDATE()),
('P5','Male','A-','CITY5',1111111115, 'p5@email.com', GETDATE(),GETDATE()),
('P6','Female','B-','CITY6',1111111116, 'p6@email.com', GETDATE(),GETDATE()),
('P7','Male','AB-','CITY7',1111111117, 'p7@email.com', GETDATE(),GETDATE()),
('P8','Female','O+','CITY8',1111111118, 'p8@email.com', GETDATE(),GETDATE());

/*

*/

-- Checking
select * from Persons;
select * from Donors;
select * from Receivers;
select * from Stocks;
select * from Users;
