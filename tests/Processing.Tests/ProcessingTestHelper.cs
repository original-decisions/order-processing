using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;
using odec.Framework.Logging;
using odec.Server.Model.Attachment;
using odec.Server.Model.Attachment.Extended;
using odec.Server.Model.Contact;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Money;
using odec.Server.Model.OrderProcessing.Money.Withdrawal;
using odec.Server.Model.Store.Blob;
using odec.Server.Model.Store.Clothes;
using odec.Server.Model.User;

namespace Processing.Tests
{
    internal static class ProcessingTestHelper
    {
        internal static Contact GenerateContact()
        {
            return new Contact
            {
                SexId = 1,
                Email = "pirmosk@gmail.com",
                BirthdayDate = DateTime.Today,
                Code = "pirmosk@gmail.com" + DateTime.Now.ToString(),
                SendNews = true,
                FirstName = "Alex",
                LastName = "Pirogov",
                Patronymic = "Leonidovich",
                IsActive = true,
                DateCreated = DateTime.Now,
                AddressDenormolized = "Litovsky Street 1 flat 512",
                PhoneNumberDenormolized = "9686698356",
                Name = "MainAddress",
                SortOrder = 0
            };
        }
        internal static Order GenerateOrder(Contact contact, OrderState state, PaymentMethod payment, DeliveryMethod delivery, DeliveryZone deliveryZone)
        {
            return new Order
            {
                Name = "Test",
                Code = "Test",
                PaymentMethodId = payment.Id,
                ContactId = contact.Id,
                OrderStateId = state.Id,
                DeliveryMethodId = delivery.Id,
                DeliveryZoneId = deliveryZone.Id,
                DeliveryCost = 200,
                Total = 2000,
                Comment = "Good choice",
                OrderDate = DateTime.Now,
                SortOrder = 0
            };
        }
        internal static PaymentMethod GeneratePaymentMethod()
        {
            return new PaymentMethod
            {
                Name = "Test",
                Code = "Test",
                SortOrder = 0
            };
        }

        internal static DeliveryMethod GenerateDeliveryMethod()
        {
            return new DeliveryMethod
            {
                Name = "Test",
                Code = "Test",
                SortOrder = 0
            };
        }

        internal static Attachment GenerateAttachment()
        {
            return new Attachment
            {
                Name = "Test",
                Code = Guid.NewGuid().ToString(),
                IsActive = true,
                DateCreated = DateTime.Now,
                SortOrder = 0,
                Extension = new Extension
                {
                    Name = "Test",
                    Code = "TEST",
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    SortOrder = 0
                },
                PublicUri = string.Empty,
                IsShared = false,
                Content = new byte[] { 1, 1, 1, 1, 1, 1 },
                AttachmentType = new AttachmentType
                {
                    Name = "Test",
                    Code = "TEST",
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    SortOrder = 0
                }
            };
        }

        internal static Good GenerateGood(string code, Size size)
        {
            return new Good
            {
                Name = "My Conversation",
                Code = code,
                IsActive = true,
                DateCreated = DateTime.Now,
                Articul = Guid.NewGuid().ToString(),
                BasePrice = 1000,
                MarkUp = 1,
                Height = 50,
                Width = 50,
                Depth = 50,
                SizeId = size.Id,
                SerialNumber = Guid.NewGuid().ToString(),
                ShortDescription = string.Empty,
                Description = string.Empty,
                SortOrder = 0,
            };
        }
        internal static DeliveryZone GenerateDeliveryZone()
        {
            return new DeliveryZone
            {
                Name = "Test",
                Code = "Test",
                SortOrder = 0
            };
        }
        internal static WithdrawalApplication GenerateWithdrawalApplication(User user, WithdrawalMethod method)
        {
            return new WithdrawalApplication
            {
                WithdrawalBefore = DateTime.Now.AddDays(14),
                Amount = 1000,
                Comment = "Test Test",
                IsApproved = false,
                DateCreated = DateTime.Now,
                UserId = user.Id,
                WithdrawalMethodId = method.Id
            };
        }
        internal static void PopulateDefaultOrderCtx(DbContext context)
        {
            try
            {


                var nosex = new Sex
                {
                    Code = "NOSEX",
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    Name = "No sex selected"
                };
                if (!context.Set<Sex>().Any(it => it.Code.Equals(nosex.Code)))
                    context.Set<Sex>().Add(nosex);
                var networkDelivery = new DeliveryMethod
                {
                    Code = "ELECTRONIC",
                    Name = "Electronic delivery",
                    IsActive = true,
                    Description = "Default",
                    SortOrder = 0,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<DeliveryMethod>().Any(it => it.Code.Equals(networkDelivery.Code)))
                    context.Set<DeliveryMethod>().Add(networkDelivery);
                var courierDelivery = new DeliveryMethod
                {
                    Code = "ByCourier".ToUpper(),
                    Name = "By Courier",
                    IsActive = true,

                    Description = "Default",
                    SortOrder = 0,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<DeliveryMethod>().Any(it => it.Code.Equals(courierDelivery.Code)))
                    context.Set<DeliveryMethod>().Add(courierDelivery);
                var postDelivery = new DeliveryMethod
                {
                    Code = "Post".ToUpper(),
                    Name = "Post",
                    IsActive = true,
                    SortOrder = 0,

                    Description = "Default",
                    DateCreated = DateTime.Now
                };
                if (!context.Set<DeliveryMethod>().Any(it => it.Code.Equals(postDelivery.Code)))
                    context.Set<DeliveryMethod>().Add(postDelivery);
                var storesDelivery = new DeliveryMethod
                {
                    Code = "stores".ToUpper(),
                    Name = "Our Stores",
                    Description = "Default",
                    IsActive = true,
                    SortOrder = 4,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<DeliveryMethod>().Any(it => it.Code.Equals(storesDelivery.Code)))
                    context.Set<DeliveryMethod>().Add(storesDelivery);
                var formedState = new OrderState
                {
                    Code = "FORMED",
                    Name = "Formed",
                    IsActive = true,
                    SortOrder = 0,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OrderState>().Any(it => it.Code.Equals(formedState.Code)))
                    context.Set<OrderState>().Add(formedState);
                var deliveredState = new OrderState
                {
                    Code = "Delivered".ToUpper(),
                    Name = "Delivered",
                    IsActive = true,
                    SortOrder = 1,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OrderState>().Any(it => it.Code.Equals(deliveredState.Code)))
                    context.Set<OrderState>().Add(deliveredState);
                var paidState = new OrderState
                {
                    Code = "paid".ToUpper(),
                    Name = "Paid",
                    IsActive = true,
                    SortOrder = 2,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OrderState>().Any(it => it.Code.Equals(paidState.Code)))
                    context.Set<OrderState>().Add(paidState);
                var awaitingPaymentState = new OrderState
                {
                    Code = "AwaitingPayment".ToUpper(),
                    Name = "Awaiting Payment",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OrderState>().Any(it => it.Code.Equals(awaitingPaymentState.Code)))
                    context.Set<OrderState>().Add(awaitingPaymentState);

                var creditCardPayment = new PaymentMethod
                {
                    Code = "CreditPayment".ToUpper(),
                    Name = "Credit Card Payment",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<PaymentMethod>().Any(it => it.Code.Equals(creditCardPayment.Code)))
                    context.Set<PaymentMethod>().Add(creditCardPayment);
                var cashPayment = new PaymentMethod
                {
                    Code = "PayCash".ToUpper(),
                    Name = "Pay With Cash",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<PaymentMethod>().Any(it => it.Code.Equals(cashPayment.Code)))
                    context.Set<PaymentMethod>().Add(cashPayment);
                var paypalPayment = new PaymentMethod
                {
                    Code = "paypal".ToUpper(),
                    Name = "PayPal",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<PaymentMethod>().Any(it => it.Code.Equals(paypalPayment.Code)))
                    context.Set<PaymentMethod>().Add(paypalPayment);
                var moneyOrder = new OrderType
                {
                    Code = "moneyOrder".ToUpper(),
                    Name = "Money Order",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OrderType>().Any(it => it.Code.Equals(moneyOrder.Code)))
                    context.Set<OrderType>().Add(moneyOrder);

                var auctionOrder = new OrderType
                {
                    Code = "auctionOrder".ToUpper(),
                    Name = "Auction Order",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OrderType>().Any(it => it.Code.Equals(auctionOrder.Code)))
                    context.Set<OrderType>().Add(auctionOrder);
                var goodOrder = new OrderType
                {
                    Code = "goodOrder".ToUpper(),
                    Name = "Good Order",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OrderType>().Any(it => it.Code.Equals(goodOrder.Code)))
                    context.Set<OrderType>().Add(goodOrder);
                var russiaDeliveryZone = new DeliveryZone
                {
                    Code = "russia".ToUpper(),
                    Name = "Russia",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<DeliveryZone>().Any(it => it.Code.Equals(russiaDeliveryZone.Code)))
                    context.Set<DeliveryZone>().Add(russiaDeliveryZone);

                var moscowDeliveryZone = new DeliveryZone
                {
                    Code = "moscow".ToUpper(),
                    Name = "Moscow",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<DeliveryZone>().Any(it => it.Code.Equals(moscowDeliveryZone.Code)))
                    context.Set<DeliveryZone>().Add(moscowDeliveryZone);
                var ysenevoDeliveryZone = new DeliveryZone
                {
                    Code = "ysenevo".ToUpper(),
                    Name = "Ysenevo",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<DeliveryZone>().Any(it => it.Code.Equals(ysenevoDeliveryZone.Code)))
                    context.Set<DeliveryZone>().Add(ysenevoDeliveryZone);
                var uZAODeliveryZone = new DeliveryZone
                {
                    Code = "UZAO".ToUpper(),
                    Name = "UZAO",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<DeliveryZone>().Any(it => it.Code.Equals(uZAODeliveryZone.Code)))
                    context.Set<DeliveryZone>().Add(uZAODeliveryZone);
                var pechatnikiDeliveryZone = new DeliveryZone
                {
                    Code = "pechatniki".ToUpper(),
                    Name = "Pechatniki",
                    IsActive = true,
                    SortOrder = 3,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<DeliveryZone>().Any(it => it.Code.Equals(pechatnikiDeliveryZone.Code)))
                    context.Set<DeliveryZone>().Add(pechatnikiDeliveryZone);
                var deliveryCharge = new DeliveryCharge
                {
                    ChargeValue = 1000,
                    DeliveryMethodId = courierDelivery.Id,
                    ZoneId = pechatnikiDeliveryZone.Id
                };
                if (!context.Set<DeliveryCharge>().Any(it => it.DeliveryMethodId ==deliveryCharge.DeliveryMethodId && it.ZoneId ==deliveryCharge.ZoneId))
                    context.Set<DeliveryCharge>().Add(deliveryCharge);
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        internal static void PopulateDefaultMoneyCtx(DbContext context)
        {
            try
            {
                var user = new User
                {
                    Id = 1,
                    UserName = "Andrew",

                };
                var user2 = new User
                {
                    Id = 2,
                    UserName = "TestUser2",

                };
                if (!context.Set<User>().Any(it => it.UserName.Equals(user.UserName)))
                    context.Set<User>().AddRange(user, user2);

                var userBalance = new UserMAccount
                {
                    UserId = user.Id,
                    BlockedMoney = 0,
                    CurrentMoney = 0
                };
                var userBalance2 = new UserMAccount
                {
                    UserId = user2.Id,
                    BlockedMoney = 0,
                    CurrentMoney = 0
                };
                context.Set<UserMAccount>().AddRange(userBalance,userBalance2);
                PopulateDefaultOrderCtx(context);

                var cashDepositOperationType = new OperationType
                {
                    Code = "CASHDEPOSIT".ToUpper(),
                    Name = "Cash Deposit",
                    IsActive = true,
                    SortOrder = 0,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OperationType>().Any(it => it.Code.Equals(cashDepositOperationType.Code)))
                    context.Set<OperationType>().Add(cashDepositOperationType);
                var u2uTransferOperationType = new OperationType
                {
                    Code = "USERTOUSERTRANSFER".ToUpper(),
                    Name = "User To User Transfer",
                    IsActive = true,
                    SortOrder = 0,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OperationType>().Any(it => it.Code.Equals(u2uTransferOperationType.Code)))
                    context.Set<OperationType>().Add(u2uTransferOperationType);
                var refillOperationType = new OperationType
                {
                    Code = "refill".ToUpper(),
                    Name = "Refill",
                    IsActive = true,
                    SortOrder = 0,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OperationType>().Any(it => it.Code.Equals(refillOperationType.Code)))
                    context.Set<OperationType>().Add(refillOperationType);
                var withdrawalOperationType = new OperationType
                {
                    Code = "Withdrawal".ToUpper(),
                    Name = "Withdrawal Of Funds",
                    IsActive = true,
                    SortOrder = 0,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<OperationType>().Any(it => it.Code.Equals(withdrawalOperationType.Code)))
                    context.Set<OperationType>().Add(withdrawalOperationType);
                var courierMethod = new WithdrawalMethod
                {
                    Code = "courier".ToUpper(),
                    Name = "Withdraw by a Courier",
                    IsActive = true,
                    Description = "Default",
                    SortOrder = 0,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<WithdrawalMethod>().Any(it => it.Code.Equals(courierMethod.Code)))
                    context.Set<WithdrawalMethod>().Add(courierMethod);
                var creditCardMethod = new WithdrawalMethod
                {
                    Code = "creditCard".ToUpper(),
                    Name = "Credit Card",
                    IsActive = true,

                    Description = "Default",
                    SortOrder = 0,
                    DateCreated = DateTime.Now
                };
                if (!context.Set<WithdrawalMethod>().Any(it => it.Code.Equals(creditCardMethod.Code)))
                    context.Set<WithdrawalMethod>().Add(creditCardMethod);
                var testWithdrawalApplication = GenerateWithdrawalApplication(user, creditCardMethod);
                context.Set<WithdrawalApplication>().Add(testWithdrawalApplication);
                context.Set<WithdrawalApplication>().Add(GenerateWithdrawalApplication(user, creditCardMethod));
                context.Set<WithdrawalApplication>().Add(GenerateWithdrawalApplication(user, courierMethod));
                context.Set<WithdrawalApplication>().Add(GenerateWithdrawalApplication(user, courierMethod));
                context.Set<WithdrawalApplication>().Add(GenerateWithdrawalApplication(user, courierMethod));
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public static OrderDetail GenerateOrderDetail(Order order,object good)
        {
            return new OrderDetail
            {
                DiscountedCost = 1000,
                EntityCount = 10,
                OrderId = order.Id,
                JsonEntityDetails = JsonConvert.SerializeObject(good),
                Total = 1000,
                MomentCost = 1400
            };
        }
    }
}
