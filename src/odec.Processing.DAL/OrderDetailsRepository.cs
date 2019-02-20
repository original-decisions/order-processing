using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using odec.Framework.Logging;
using odec.Processing.DAL.Interop;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Filters;
using odec.Framework.Extensions;
using System.Linq;
namespace odec.Processing.DAL
{
    public class OrderDetailsRepository: IOrderDetailsRepository<int,DbContext,OrderDetail,OrderDetailsFilter<int>>
    {
        public OrderDetailsRepository() { }
        public OrderDetailsRepository(DbContext db)
        {
            Db = db;
        }
        public IEnumerable<OrderDetail> Get(OrderDetailsFilter<int> filter)
        {
            try
            {
               
                    var query = Db.Set<OrderDetail>().Where(it => it.OrderId == filter.OrderId);
                    if (filter.TotalInterval!=null)
                    {
                        if (filter.TotalInterval.Start!=null)
                            query= query.Where(it => it.Total >= filter.TotalInterval.Start);
                        if (filter.TotalInterval.End !=null)
                            query = query.Where(it => it.Total <= filter.TotalInterval.End);
                    }
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

        public IEnumerable<OrderDetail> GetByOrder(int orderId)
        {
            try
            {
                return from orderDetail in Db.Set<OrderDetail>()
                    join order in Db.Set<Order>() on orderDetail.OrderId equals order.Id
                    where orderDetail.OrderId == orderId
                    select new OrderDetail
                    {
                        Order = order,
                        DiscountedCost = orderDetail.DiscountedCost,
                        MomentCost = orderDetail.MomentCost,
                        EntityCount = orderDetail.EntityCount,
                        Id = orderDetail.Id,
                        OrderId = orderDetail.OrderId,
                        JsonEntityDetails = orderDetail.JsonEntityDetails,
                        Total = orderDetail.Total
                    };





                        Db.Set<OrderDetail>().Include(it => it.Order).Where(it => it.OrderId == orderId);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public OrderDetail GetById(int id)
        {
            try
            {
                return Db.Set<OrderDetail>().SingleOrDefault(it => it.Id == id);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
            
        }

        public void Save(OrderDetail entity)
        {
            try
            {
                var oDetail = GetById(entity.Id);
                if (oDetail==null || entity.Id ==0)
                    Db.Set<OrderDetail>().Add(entity);
                else
                    Db.Entry(entity).State =EntityState.Modified;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void SaveById(OrderDetail entity)
        {
            try
            {
                Save(entity);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var oDetail = GetById(id);
                if (oDetail == null )
                    return;
                Db.Set<OrderDetail>().Remove(oDetail);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void Delete(OrderDetail entity)
        {
            try
            {
                Delete(entity.Id);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public DbContext Db { get; set; }
        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            Db = db;
        }

        
    }
}
