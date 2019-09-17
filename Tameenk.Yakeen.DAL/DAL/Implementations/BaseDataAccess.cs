using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Tameenk.Yakeen.DAL
{   
    public abstract class BaseDataAccess<TEntity, TKey> where TEntity : class
    {
        protected readonly DbSet<TEntity> entity;
        protected readonly YakeenContext context;

        public BaseDataAccess()
        {
            this.context = new YakeenContext();
            entity = context.Set<TEntity>();
        }
        
        public virtual int Count()
        {
            return entity.Count();
        }
        
        public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate,
               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
               string includeProperties = "")
        {
            var query = entity.Where(predicate);

            foreach (var includeProperty in includeProperties.Split
               (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetSingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return entity.SingleOrDefault(predicate);
        }

        public virtual TEntity Get(TKey id)
        {
            return entity.Find(id);
        }

        public virtual List<TEntity> GetAll()
        {
            return entity.ToList();
        }
        public virtual int Add(TEntity entity)
        {
            this.entity.Add(entity);
            return context.SaveChanges();
        }

        public virtual int AddRange(List<TEntity> entities) {
            this.entity.AddRange(entities);
                return context.SaveChanges();
        }


        public virtual int Update(TEntity entity)
        {
            this.entity.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public virtual int Remove(TEntity entity)
        {
            this.entity.Remove(entity);
            return context.SaveChanges();
        }

        public virtual int RemoveRange(List<TEntity> entities)
        {
            this.entity.RemoveRange(entities);
            return context.SaveChanges();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }
    }
}
