using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Contracts.Repositories;

namespace ToDo.Persistence.Contexts.Repositories
{
    public class UnitOfWork<T> : IUnitOfWork<T>, IDisposable
        where T : DbContext
    {
        private readonly T _dbContext;
        private readonly IDictionary<string, object> _repositories;

        private IDbContextTransaction _transaction;

        private bool _isDisposed = false;

        public UnitOfWork(T dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<string, object>();
        }

        public T Context
        {
            get { return _dbContext; }
        }

        public void BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException();
            }

            _transaction = _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new NullReferenceException();
            }

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            if (_transaction == null)
            {
                throw new NullReferenceException();
            }

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public IRepository<E> Repository<E>() where E : class
        {
            var repository = _dbContext.GetService<IRepository<E>>();
            if (repository != null)
            {
                return repository;
            }

            var type = typeof(E);
            var typeName = type.Name;

            if (!_repositories.ContainsKey(typeName))
            {
                var instance = new Repository<E>(_dbContext);
                _repositories.Add(typeName, instance);
            }

            return (IRepository<E>)_repositories[typeName];
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        { 
            if (disposing) 
            { 
                if (!_isDisposed) 
                {
                    if (_transaction != null)
                    { 
                        _transaction.Dispose();
                    }

                    _dbContext.Dispose();
                }
            }

            _isDisposed = true;
        }
    }
}
