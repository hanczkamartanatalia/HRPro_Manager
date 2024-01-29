using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Website.Database;

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
            PropertyInfo? property = typeof(T).GetProperty(_fieldName);

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

            _context.Entry(entity).Reload();

            return entity as T;
        }

        public static T? GetLastRecord(string? property = null, string? value = null)
        {
            T? result = null;

            if (string.IsNullOrEmpty(property))
            {
                result = _context.Set<T>().OrderByDescending(x => x.Id)
                              .FirstOrDefault();
                _context.Entry(result).Reload();
                return result;
            }

            PropertyInfo? propertyInfo = typeof(T).GetProperty(property);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Type {typeof(T)} does not contain a property named {property}.");
            }

            result = _context.Set<T>()
              .Where(x => EF.Property<string>(x, property) == value)
              .OrderByDescending(x => x.Id)
              .FirstOrDefault();

            _context.Entry(result).Reload();

            return result;
        }



    }
}
