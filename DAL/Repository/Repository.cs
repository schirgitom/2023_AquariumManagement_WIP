using Amazon.Runtime.Internal.Util;
using DAL.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected Serilog.ILogger log = Utils.Logger.ContextLog<Repository<TEntity>>();

        private readonly IMongoCollection<TEntity> mongoCollection;

        public Repository(DBContext context) {
            mongoCollection = context.DataBase.GetCollection<TEntity>(typeof(TEntity).Name.ToString());
        }

        public Task DeleteByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> InsertOneAsync(TEntity document)
        {
            document.ID = document.GenerateID();
            await mongoCollection.InsertOneAsync(document);

            return document;
        }

        public Task<TEntity> InsertOrUpdateOneAsync(TEntity document)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateOneAsync(TEntity document)
        {
            throw new NotImplementedException();
        }
    }
}
