﻿using BullkyBook.DataAccess.IRepository;
using BullkyBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullkyBook.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository:IRepository<Company>
    {
        void Update(Company company);
    }
}
