using System;
using System.Collections.Generic;
using System.Text;

namespace BullkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        void Save();
    }

}
