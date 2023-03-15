using DAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System.Linq.Expressions;
using Utils;

namespace DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected ILogger log = Logger.ContextLog<Repository<TEntity>>();

        private readonly IMongoCollection<TEntity> _collection;

        public Repository(DBContext Context)
        {
            _collection = Context.DataBase.GetCollection<TEntity>(typeof(TEntity).Name.ToString());
        }

        public IMongoCollection<TEntity> Collection
        {
            get
            {
                return _collection;
            }
        }



        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual List<TEntity> FilterBy(
            Expression<Func<TEntity, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToList();
        }

        public async virtual Task<List<TEntity>> FilterByAsync(
          Expression<Func<TEntity, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToList();
        }

        public virtual List<TProjected> FilterBy<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToList();
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public async virtual Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public virtual TEntity FindById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, id);
                return _collection.Find(filter).SingleOrDefault();
            }
            return null;
        }

        public async virtual Task<TEntity> FindByIdAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var objectId = ObjectId.Parse(id);
                var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, id);
                return await _collection.Find(filter).SingleOrDefaultAsync();
            }

            return null;

        }

        public async virtual Task<TEntity> InsertOneAsync(TEntity document)
        {
            try
            {
                document.ID = document.GenerateID();

                await _collection.InsertOneAsync(document);

                return document;
            }
            catch (MongoWriteException ex)
            {
                log.Error("Error while saving", ex);
            }
            catch (Exception e)
            {
                log.Error("Error while saving", e);

            }
            return default;
        }


        public virtual async Task<TEntity> UpdateOneAsync(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, document.ID);
            await _collection.FindOneAndReplaceAsync(filter, document);
            // await document.SaveAsync();

            return document;
        }

        public virtual async Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            await _collection.FindOneAndDeleteAsync(filterExpression);
        }


        public virtual async Task DeleteByIdAsync(string id)
        {

            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.ID, id);
            await _collection.FindOneAndDeleteAsync(filter);

        }



        public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            await _collection.DeleteManyAsync(filterExpression);
        }


        public async Task<TEntity> InsertOrUpdateOneAsync(TEntity document)
        {
            if (document != null && document.ID != null)
            {
                if (!string.IsNullOrEmpty(document.ID))
                {
                    TEntity item = FindById(document.ID.ToString());

                    if (item == null)
                    {
                        await InsertOneAsync(document);
                    }
                    else
                    {
                        await UpdateOneAsync(document);
                    }
                }
                else
                {
                    await InsertOneAsync(document);
                }
            }
            else
            {
                await InsertOneAsync(document);
            }

            return document;
        }

    }
}
