begin tran
if not exists( select top 1 1 from ecash.OperationTypes where code like 'CASHDEPOSIT')
begin
INSERT INTO ecash.OperationTypes (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Cash Deposit','CASHDEPOSIT',1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from ecash.OperationTypes where code like 'USERTOUSERTRANSFER')
begin
INSERT INTO ecash.OperationTypes (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('User To User Transfer','USERTOUSERTRANSFER',1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from ecash.OperationTypes where code like 'refill')
begin
INSERT INTO ecash.OperationTypes (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Refill',upper('refill'),1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from ecash.OperationTypes where code like 'Withdrawal')
begin
INSERT INTO ecash.OperationTypes (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Withdrawal Of Funds',upper('Withdrawal'),1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from ecash.OperationTypes where code like 'Withdrawal')
begin
INSERT INTO ecash.OperationTypes (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Withdrawal Of Funds',upper('Withdrawal'),1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from ecash.WithdrawalMethods where code like 'courier')
begin
INSERT INTO ecash.WithdrawalMethods (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated,[Description])
values ('Withdraw With a Courier',upper('courier'),1,0,GetDate(),GetDate(),'Our Courier will deliver your money to you. It may take about 14 days.')
end
if not exists( select top 1 1 from ecash.WithdrawalMethods where code like 'creditCard')
begin
INSERT INTO ecash.WithdrawalMethods (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated,[Description])
values ('Credit Card',upper('creditCard'),1,0,GetDate(),GetDate(),'Withdrawal the funds to your credit card. It may take about 14 days.')
end
commit tran