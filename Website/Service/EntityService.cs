using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Website.Database;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Website.Service
{
    public static class EntityService<T> where T : Entities.Entity
    {


        public static T GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Incorrect value. Id should be greater than 0.", nameof(id));
            }

            using (AppDbContext _context = new AppDbContext())
            {
                T entity = _context.Set<T>().FirstOrDefault(x => EF.Property<int>(x, "Id") == id);

                if (entity == null) { throw new ArgumentNullException($"{nameof(entity)}"); }

                return entity;
            }  
        }

        public static List<T> GetAll() 
        {
            using (AppDbContext _context = new AppDbContext())
            {
                return _context.Set<T>().ToList();
            }  
        }

        public static void RemoveById(int _id)
        {
            throw new NotImplementedException();
        }

        public static T GetBy(string _fieldName, string _value)
        {
            PropertyInfo property = typeof(T).GetProperty(_fieldName);

            if (property == null)
            {
                throw new ArgumentException($"Field with name {_fieldName} does not exist in class {typeof(T).Name}.");
            }

            using (AppDbContext _context = new AppDbContext())
            {
                var entities = _context.Set<T>().AsEnumerable();

                var entity = entities.FirstOrDefault(x => property.GetValue(x)?.ToString() == _value);

                if (entity == null)
                {
                    throw new ArgumentNullException($"{nameof(entity)}");
                }

                return entity as T;
            }
        }


    }
}
