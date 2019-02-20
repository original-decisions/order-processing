using System;
using Microsoft.EntityFrameworkCore;
using odec.Processing.DAL.Interop;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.User;

namespace odec.Processing.DAL
{
    public class CardProcessing:ICardProcessing<int,DbContext,Card,User>
    {
        public Card GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(Card entity)
        {
            throw new NotImplementedException();
        }

        public void SaveById(Card entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Card entity)
        {
            throw new NotImplementedException();
        }

        public DbContext Db { get; set; }
        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            throw new NotImplementedException();
        }

        public Card GetUserCard(User user)
        {
            throw new NotImplementedException();
        }
    }
}
