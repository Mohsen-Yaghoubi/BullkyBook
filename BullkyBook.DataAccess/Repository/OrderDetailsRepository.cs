using BulkyBook.Data;
using BullkyBook.DataAccess.IRepository.Repository;
using BullkyBook.DataAccess.Repository.IRepository;
using BullkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BullkyBook.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails obj)
        {
            _db.Update(obj);
        }
    }
}
