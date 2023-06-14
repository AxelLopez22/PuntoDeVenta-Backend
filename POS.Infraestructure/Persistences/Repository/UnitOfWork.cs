using POS.Infraestructure.Persistences.Context;
using POS.Infraestructure.Persistences.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Infraestructure.Persistences.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PuntoDeVentaContext _context;

        public ICategoryRepository Category { get; private set; }

        public UnitOfWork(PuntoDeVentaContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
        }

        public void Dispose()
        { 
            _context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SavesChanges()
        {
            _context.SaveChanges();
        }
    }
}
