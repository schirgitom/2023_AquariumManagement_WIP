using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : Entity
    {

        List<TEntity> FilterBy(
               Expression<Func<TEntity, bool>> filterExpression);

        Task<List<TEntity>> FilterByAsync(
           Expression<Func<TEntity, bool>> filterExpression);


        List<TProjected> FilterBy<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression);


        Task<TEntity> FindByIdAsync(string id);

        Task<TEntity> InsertOneAsync(TEntity document);

        Task<TEntity> UpdateOneAsync(TEntity document);

        Task<TEntity> InsertOrUpdateOneAsync(TEntity document);

        Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression);

        Task DeleteByIdAsync(string id);

        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression);
    }

}
