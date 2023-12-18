using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using Website.Database;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Website.Service
{
    public static class EntityService<T> where T : Entities.Entity
    {
        private static readonly AppDbContext _context = new AppDbContext();

        public static T GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Incorrect value. Id should be greater than 0.", nameof(id));
            }
            T entity = EntityService<T>.GetBy("Id", id.ToString());
            return entity;
        }

        public static List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public static T GetBy(string _fieldName, string _value)
        {
            PropertyInfo property = typeof(T).GetProperty(_fieldName);

            if (property == null)
            {
                throw new ArgumentException($"Field with name {_fieldName} does not exist in class {typeof(T).Name}.");
            }

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
