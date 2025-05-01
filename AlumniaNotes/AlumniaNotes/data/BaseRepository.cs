using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace AlumniaNotes.data
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly DatabaseContext _context;
        protected readonly string _tableName;

        protected BaseRepository(DatabaseContext context, string tableName)
        {
            _context = context;
            _tableName = tableName;
        }

        public virtual IEnumerable<T> GetAll()
        {
            var query = $"SELECT * FROM {_tableName}";
            var entities = new List<T>();

            using (var reader = _context.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    entities.Add(MapReaderToEntity(reader));
                }
            }

            return entities;
        }

        public virtual T GetById(int id)
        {
            var query = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", id) };

            using (var reader = _context.ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    return MapReaderToEntity(reader);
                }
            }

            return null;
        }

        public virtual void Add(T entity)
        {
            var properties = typeof(T).GetProperties();
            var columns = new List<string>();
            var values = new List<string>();
            var parameters = new List<SqlParameter>();

            foreach (var property in properties)
            {
                if (property.Name != "Id")
                {
                    columns.Add(property.Name);
                    values.Add($"@{property.Name}");
                    parameters.Add(new SqlParameter($"@{property.Name}", property.GetValue(entity) ?? DBNull.Value));
                }
            }

            var query = $"INSERT INTO {_tableName} ({string.Join(", ", columns)}) VALUES ({string.Join(", ", values)})";
            _context.ExecuteNonQuery(query, parameters.ToArray());
        }

        public virtual void Update(T entity)
        {
            var properties = typeof(T).GetProperties();
            var updates = new List<string>();
            var parameters = new List<SqlParameter>();

            foreach (var property in properties)
            {
                if (property.Name != "Id")
                {
                    updates.Add($"{property.Name} = @{property.Name}");
                    parameters.Add(new SqlParameter($"@{property.Name}", property.GetValue(entity) ?? DBNull.Value));
                }
            }

            parameters.Add(new SqlParameter("@Id", typeof(T).GetProperty("Id").GetValue(entity)));

            var query = $"UPDATE {_tableName} SET {string.Join(", ", updates)} WHERE Id = @Id";
            _context.ExecuteNonQuery(query, parameters.ToArray());
        }

        public virtual void Delete(int id)
        {
            var query = $"DELETE FROM {_tableName} WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", id) };
            _context.ExecuteNonQuery(query, parameters);
        }

        protected abstract T MapReaderToEntity(SqlDataReader reader);
    }
} 