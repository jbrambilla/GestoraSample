using GestoraSample.Infra.Dados.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace GestoraSample.Infra.Dados.Repositorios
{
    public class RepositoryBase<TEntity> where TEntity : class
    {
        protected GestoraContexto Db;
        protected DbSet<TEntity> DbSet;

        public RepositoryBase()
        {
            Db = new GestoraContexto();
            DbSet = Db.Set<TEntity>();
        }

        public void Add(TEntity obj)
        {
            DbSet.Add(obj);
            Db.SaveChanges();
        }

        public TEntity GetById(Guid id)
        {
            return DbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()//(int s, int t)
        {
            return DbSet.ToList(); //Take(t).Skip(s).ToList();
        }

        public void Update(TEntity obj)
        {
            var entry = Db.Entry(obj);
            DbSet.Attach(obj);
            entry.State = EntityState.Modified;
            Db.SaveChanges();
        }

        public void Remove(Guid id)
        {
            DbSet.Remove(DbSet.Find(id));
            Db.SaveChanges();
        }

        public IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
