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
    public class ShoppingCardRepository : Repository<ShoppingCard>, IShoppingCardRepository
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCardRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCard obj)
        {
            _db.Update(obj);
        }
    }
}
