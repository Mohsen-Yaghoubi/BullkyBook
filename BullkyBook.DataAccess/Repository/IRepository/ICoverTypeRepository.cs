using BullkyBook.DataAccess.IRepository;
using BullkyBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullkyBook.DataAccess.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update(CoverType coverType);
    }
}
