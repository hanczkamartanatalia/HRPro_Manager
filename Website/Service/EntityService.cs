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
        private static readonly AppDbContext _context = null!;

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

        public static T GetBy(string _fieldName, string _value)
        {
            PropertyInfo property = typeof(T).GetProperty(_fieldName);

            if (property == null)
            {
                throw new ArgumentException($"Field with name {_fieldName} does not exist in class {typeof(T).Name}.");
            }

            var entity = _context.Set<T>().FirstOrDefault(x => property.GetValue(x).ToString() == _value);
            if (entity == null) { throw new ArgumentNullException($"{nameof(entity)}"); }

            return entity as T;

        }

    }
}
