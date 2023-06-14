using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Infraestructure.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Declaracion de interfaces a nivel de repositorio
        ICategoryRepository Category { get; }
        void SavesChanges();
        Task SaveChangesAsync();
    }
}
 