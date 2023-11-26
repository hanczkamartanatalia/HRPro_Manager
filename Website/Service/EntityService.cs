using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Website.Database;

namespace Website.Service
{
    public static class EntityService<T> where T : Entities.Entity
    {
        private static readonly AppDbContext _context;

        public static T GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Incorrect value. Id should be greater than 0.", nameof(id));
            }

            T entity = _context.Set<T>().FirstOrDefault(x => EF.Property<int>(x, "Id") == id);

            if (entity == null) { throw new ArgumentNullException($"{nameof(entity)}"); }

            return entity;
        }

        public static List<T> GetAll() 
        {
            return _context.Set<T>().ToList();
        }

        public static void RemoveById(int _id)
        {
            throw new NotImplementedException();
        }



    }
}
