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
    class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            _db.Update(company);
        }
    }
}
