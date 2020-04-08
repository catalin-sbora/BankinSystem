using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Abstractions
{
    public interface IRepository<T> where T: DataEntity
    {
        IEnumerable<T> GetAll();

        T GetById(Guid id);

        T Add(T entity);

        T Update(T entity);

        bool Remove(Guid id);
    }
}
