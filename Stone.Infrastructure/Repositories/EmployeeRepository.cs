﻿using Stone.Domain.Entities;
using Stone.Domain.Interface.Repositories;
using Stone.Infrastructure.DataContextLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stone.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployee
    {

        private readonly EFDataContext _db;

        public EmployeeRepository()
        {
            _db = new EFDataContext();
        }

        public Employee Create(Employee entity)
        {            
            try
            {
                _db.Add(entity);
                _db.SaveChanges();
                return entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public Employee Read(int id)
        {
            Console.WriteLine("---");
            return _db.Employees.Find(id);
        }
    }
}
