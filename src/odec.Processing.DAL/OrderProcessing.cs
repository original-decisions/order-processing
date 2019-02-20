using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using odec.Framework.Logging;
using odec.Processing.DAL.Interop;
using odec.Server.Model.OrderProcessing;

namespace odec.Processing.DAL
{
    public class OrderProcessing:IOrderProcessing<int,DbContext,Order,OrderState>
    {
        private OrderRepository repo = new OrderRepository();
        public OrderProcessing()
        {
            
        }
        public OrderProcessing(DbContext db)
        {
            Db = db;
            repo.SetContext(db);
        }
        public DbContext Db { get; set; }
        public void SetConnection(string connection)
        {
           throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            Db = db;
            repo.SetContext(db);
        }

        public void ChangeOrderState(Order order, OrderState newState)
        {
            
            ChangeOrderState(order.Id, newState);
            order.OrderStateId = newState.Id;
        }

        public void ChangeOrderState(int orderId, OrderState newState)
        {
            repo.ChangeState(orderId,newState);
        }

        public IEnumerable<OrderState> GetAvailableStates(Order oder)
        {
            try
            {
                //todo: configurable order flow
                return Db.Set<OrderState>();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
