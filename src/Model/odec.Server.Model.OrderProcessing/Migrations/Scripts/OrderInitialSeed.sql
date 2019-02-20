begin tran
if not exists( select top 1 1 from dbo.Sexes where code like 'NOSEX')
begin
INSERT INTO dbo.Sexes (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('No sex selected','NOSEX',1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from [order].DeliveryMethods where code like 'ELECTRONIC')
begin
INSERT INTO [order].DeliveryMethods (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated,[Description])
values ('Electronic delivery','ELECTRONIC',0,0,GetDate(),GetDate(),'')
end
if not exists( select top 1 1 from [order].DeliveryMethods where code like 'ByCourier')
begin
INSERT INTO [order].DeliveryMethods (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated,[Description])
values ('By Courier',Upper('ByCourier'),1,0,GetDate(),GetDate(),'Your purchased item will be delivered to your address. You should provide neccessary information for us.')
end
if not exists( select top 1 1 from [order].DeliveryMethods where code like 'Post')

begin
INSERT INTO [order].DeliveryMethods (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated,[Description])
values ('Post',Upper('Post'),1,0,GetDate(),GetDate(),'The Delivery by Post of Russia. It is an unsafe delivery method.')
end
if not exists( select top 1 1 from [order].DeliveryMethods where code like 'stores')
begin
INSERT INTO [order].DeliveryMethods (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated,[Description])
values ('Our Stores',Upper('stores'),1,0,GetDate(),GetDate(),'Your good will be delivered to our store. So we could give it to you.')
end
if not exists( select top 1 1 from [order].DeliveryMethods where code like 'stores')
begin
INSERT INTO [order].DeliveryMethods (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated,[Description])
values ('Our Stores',Upper('stores'),1,0,GetDate(),GetDate(),'Your good will be delivered to our store. So we could give it to you.')
end

if not exists( select top 1 1 from [order].OrderStates where code like 'FORMED')
begin
INSERT INTO [order].OrderStates (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Formed',Upper('FORMED'),1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from [order].OrderStates where code like 'Delivered')
begin
INSERT INTO [order].OrderStates (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Delivered',Upper('Delivered'),1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from [order].OrderStates where code like 'Paid')
begin
INSERT INTO [order].OrderStates (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Paid',Upper('paid'),1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from [order].OrderStates where code like 'AwaitingPayment')
begin
INSERT INTO [order].OrderStates (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Awaiting Payment',Upper('AwaitingPayment'),1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from [order].PaymentMethods where code like 'CreditPayment')
begin
INSERT INTO [order].PaymentMethods (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Credit Card Payment',Upper('CreditPayment'),1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from [order].PaymentMethods where code like 'PayCash')
begin
INSERT INTO [order].PaymentMethods (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Pay With Cash',Upper('PayCash'),1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from [order].PaymentMethods where code like 'PayPal')
begin
INSERT INTO [order].PaymentMethods (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('PayPal',Upper('PayPal'),1,0,GetDate(),GetDate())
end

if not exists( select top 1 1 from [order].[Types] where code like 'moneyOrder')
begin
INSERT INTO [order].[Types] (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Money Order',Upper('moneyOrder'),1,0,GetDate(),GetDate())
end
if not exists( select top 1 1 from [order].[Types] where code like 'auctionOrder')
begin
INSERT INTO [order].[Types] (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Auction Order',Upper('auctionOrder'),1,0,GetDate(),GetDate())
end

if not exists( select top 1 1 from [order].[Types] where code like 'goodOrder')
begin
INSERT INTO [order].[Types] (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Good Order',Upper('goodOrder'),1,0,GetDate(),GetDate())
end

if not exists( select top 1 1 from [order].[Types] where code like 'goodOrder')
begin
INSERT INTO [order].[Types] (Name,Code,IsActive,SortOrder,DateCreated,DateUpdated)
values ('Good Order',Upper('goodOrder'),1,0,GetDate(),GetDate())
end
commit tran