using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Domain.Entities;
using POS.Infraestructure.Persistences.Context;
using POS.Infraestructure.Persistences.Interfaces;
using POS.Infraestructure.Persistences.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Infraestructure.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfraestructure(this IServiceCollection service, IConfiguration configuration)
        {
            var assembly = typeof(PuntoDeVentaContext).Assembly.FullName;

            service.AddDbContext<PuntoDeVentaContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConnection"),
                    b => b.MigrationsAssembly(assembly)),ServiceLifetime.Transient);

            service.AddTransient<IUnitOfWork, UnitOfWork>();
            service.AddScoped(typeof(IGenericReporsitory<>), typeof(GenericRepository<>));

            return service;
        }
    }
}
