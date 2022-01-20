using Inventory.API.Entities;
using Inventory.API.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Inventory.API.Data.Interfaces;
using Inventory.API.Settings.Interfaces;
using Inventory.Data;
using MongoDB.Bson;

namespace Inventory.API.Repositories.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : IDocument
    {
        private readonly IContext _context;
        private readonly IMongoCollection<TEntity> _collection;

        public Repository(IProductDatabaseSettings settings, IContext context)
        {
            _context = context;
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
        }

        private string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute) documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {

            return  await _collection.Find(p => true)
                .ToListAsync();
        }

        public virtual async Task<(int totalPages, IReadOnlyList<TEntity> data)> GetAll(int page, int pageSize)
        {
            return await _collection.AggregateByPage(
                Builders<TEntity>.Filter.Empty,
                Builders<TEntity>.Sort.Descending("CreatedAt"),
                page: page,
                pageSize: pageSize);
        }
        
        public virtual IEnumerable<TEntity> FilterBy(
            Expression<Func<TEntity, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TEntity FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TEntity> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }


        #region Commands


        public virtual void InsertOne(TEntity document)
    {
        _collection.InsertOne(document);
    }

        public virtual Task InsertOneAsync(TEntity document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TEntity> documents)
        {
            _collection.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<TEntity> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }

        
        #endregion
    }

    public static class MongoCollectionQueryByPageExtensions
    {
        public static async Task<(int totalPages, IReadOnlyList<TEntity> data)> AggregateByPage<TEntity>(
            this IMongoCollection<TEntity> collection,
            FilterDefinition<TEntity> filterDefinition,
            SortDefinition<TEntity> sortDefinition,
            int page,
            int pageSize)
        {
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<TEntity, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<TEntity>()
                }));

            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<TEntity, TEntity>.Create(new[]
                {

                    PipelineStageDefinitionBuilder.Sort(sortDefinition),
                    PipelineStageDefinitionBuilder.Skip<TEntity>((page - 1) * pageSize),
                    PipelineStageDefinitionBuilder.Limit<TEntity>(pageSize),
                }));


            var aggregation = await collection.Aggregate()
                .Match(filterDefinition)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var totalPages = (int)Math.Ceiling((double)count/ pageSize);

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<TEntity>();

            return (totalPages, data);
        }
    }
}
