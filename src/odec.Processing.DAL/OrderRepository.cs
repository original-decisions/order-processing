using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL;
using odec.Processing.DAL.Interop;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Filters;

using odec.Framework.Extensions;
using odec.Framework.Logging;
using odec.Server.Model.Contact;
using odec.Server.Model.User;

namespace odec.Processing.DAL
{
    public class OrderRepository: OrmEntityOperationsRepository<int, Order, DbContext>, IOrderRepository<int, DbContext,Order,OrderDetail, OrderState, OrderFilter<int?>, OrderDetailsFilter<int>>
    {
        private OrderDetailsRepository repo= new OrderDetailsRepository();
        public OrderRepository() { }

        public OrderRepository(DbContext db)
        {
            Db = db;
            repo.SetContext(db);
        }
        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            Db = db;
            repo.SetContext(db);
        }

        public IEnumerable<Order> Get(OrderFilter<int?> filter)
        {
            try
            {

                var query = Db.Set<Order>().AsQueryable();//.Where(it => it. == filter.OrderId);
                if (filter.UserId.HasValue)
                    query = (from uc in Db.Set<UserContact>() 
                        join contact in Db.Set<Contact>() on uc.ContactId equals contact.Id
                            join order in query on contact.Id equals order.ContactId
                            where uc.UserId == filter.UserId
                            select order).Distinct();
                if (filter.OrderTypeId.HasValue)
                    query = (from oType in Db.Set<OrderOrderType>()
                             join order in query on oType.OrderId equals order.Id
                             where oType.OrderTypeId == filter.OrderTypeId
                             select order).Distinct();
                //date created
                if (filter.DateCreatedInterval != null)
                {
                    if (filter.DateCreatedInterval.Start != null)
                        query = query.Where(it => it.DateCreated >= filter.DateCreatedInterval.Start);
                    if (filter.DateCreatedInterval.End != null)
                        query = query.Where(it => it.DateCreated <= filter.DateCreatedInterval.End);
                }
                //date delivered
                if (filter.DateDeliveredInterval != null)
                {
                    if (filter.DateDeliveredInterval.Start != null)
                        query = query.Where(it => it.DateDelivery >= filter.DateDeliveredInterval.Start);
                    if (filter.DateDeliveredInterval.End != null)
                        query = query.Where(it => it.DateDelivery <= filter.DateDeliveredInterval.End);
                }
                //total
                if (filter.TotalInterval != null)
                {
                    if (filter.TotalInterval.Start != null)
                        query = query.Where(it => it.Total >= filter.TotalInterval.Start);
                    if (filter.TotalInterval.End != null)
                        query = query.Where(it => it.Total <= filter.TotalInterval.End);
                }
                //order Number 
                if (!string.IsNullOrEmpty(filter.Code))
                    query = query.Where(it => it.Code == filter.Code);
                //order state
                if (filter.OrderStateId.HasValue)
                    query = query.Where(it => it.OrderStateId == filter.OrderStateId);
                //order delivery method
                if (filter.DeliveryMethodId.HasValue)
                    query = query.Where(it => it.DeliveryMethodId == filter.DeliveryMethodId);
                //order payment method
                if (filter.PaymentMethodId.HasValue)
                    query = query.Where(it => it.PaymentMethodId == filter.PaymentMethodId);

                if (!string.IsNullOrEmpty(filter.Sidx))
                    query = filter.Sord.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(filter.Sidx)
                    : query.OrderBy(filter.Sidx);
                if (filter.Page != 0 && filter.Rows != 0)
                    return query.Skip(filter.Rows * (filter.Page - 1)).Take(filter.Rows);
                return query;

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void AddOrderDetails(IEnumerable<OrderDetail> orderDetails, Order order)
        {
            foreach (var orderDetail in orderDetails)
            {
                orderDetail.OrderId = order.Id;
                repo.Save(orderDetail);
            }
        }

        public IEnumerable<OrderDetail> GetOrderDetails(OrderDetailsFilter<int> filter)
        {
            return repo.Get(filter);
        }

        public IEnumerable<OrderDetail> GetOrderDetails(Order orderId)
        {
            return repo.Get(new OrderDetailsFilter<int>
            {
                OrderId = orderId.Id
            });
        }

        public void ChangeState(int orderId, OrderState state)
        {
            try
            {
                var order = GetById(orderId);
                order.OrderStateId = state.Id;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
