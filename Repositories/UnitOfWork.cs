using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System;
using System.Threading.Tasks;

namespace StudentManagementSystem.Repositories
{
    public class UnitOfWork //: IDisposable
    {
        private readonly ApplicationDbContext _context;
       // private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a generic repository for any entity if needed
        // Example: public Repository<Student> Students => new Repository<Student>(_context);

        // Save changes to the database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Begin a transaction
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        // Commit the transaction
        public async Task CommitAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        // Roll back the transaction
        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        // Dispose the context
       /* protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }*/

        /*public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }*/
    }
}