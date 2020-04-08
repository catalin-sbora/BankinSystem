using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class BaseRepository<T> : IRepository<T> where T : DataEntity
    {
        protected readonly BankingDbContext dbContext;
        public BaseRepository(BankingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public T Add(T itemToAdd)
        {
            var entity = dbContext.Add<T>(itemToAdd);
            dbContext.SaveChanges();
            return entity.Entity;
        }
      

        public IEnumerable<T> GetAll()
        {
            return dbContext.Set<T>()
                            .AsEnumerable();
        }

        public T GetById(Guid id)
        {
            return dbContext.Set<T>()
                            .Where(entity => entity.Id.Equals(id))
                            .SingleOrDefault();
                            
        }

        public bool Remove(Guid id)
        {
            var entityToRemove = GetById(id);
            if (entityToRemove != null)
            {
                dbContext.Remove<T>(entityToRemove);
                return true;
            }
            return false;
        }

        public T Update(T itemToUpdate)
        {
            var entity = dbContext.Update<T>(itemToUpdate);
            dbContext.SaveChanges();
            return entity.Entity;
        }
    }
}
