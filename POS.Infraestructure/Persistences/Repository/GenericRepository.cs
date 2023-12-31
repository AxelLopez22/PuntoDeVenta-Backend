﻿using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infraestructure.Commons.Bases.Request;
using POS.Infraestructure.Helpers;
using POS.Infraestructure.Persistences.Context;
using POS.Infraestructure.Persistences.Interfaces;
using POS.Utilities.Static;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace POS.Infraestructure.Persistences.Repository
{
    public class GenericRepository<T> : IGenericReporsitory<T> where T : BaseEntity
    {
        private readonly PuntoDeVentaContext _context;
        private readonly DbSet<T> _entity;

        public GenericRepository(PuntoDeVentaContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            T Entity = await GetByIdAsync(id);

            Entity!.AuditDeleteUser = 1;
            Entity.AuditDeleteDate = DateTime.Now;

            _context.Update(Entity);

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public async Task<bool> EditAsync(T entity)
        {
            entity.AuditUpdateUser = 1;
            entity.AuditUpdateDate = DateTime.Now;
            _context.Update(entity);

            _context.Entry(entity).Property(x => x.AuditCreateUser).IsModified = false;
            _context.Entry(entity).Property(x => x.AuditCreateDate).IsModified = false;

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var getAll = await _entity.Where(x => x.State.Equals((int)StateTypes.Active) && x.AuditDeleteUser == null
                && x.AuditDeleteDate == null).AsNoTracking().ToListAsync();

            return getAll;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var getById = await _entity!.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return getById!;
        }

        public IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _entity;

            if(filter != null) query = query.Where(filter);

            return query;
        }

        public async Task<bool> RegisterAsync(T entity)
        {
            entity.AuditCreateUser = 1;
            entity.AuditCreateDate = DateTime.Now;

            await _context.AddAsync(entity);

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public IQueryable<TDTO> Ordering<TDTO>(BasePaginationRequest request, IQueryable<TDTO> queryable, 
        bool pagination = false) where TDTO : class
        {
            IQueryable<TDTO> queryDto = request.Order == "desc" ? queryable.OrderBy($"{request.Sort} descending") :
                queryable.OrderBy($"{request.Sort} ascending");

            if (pagination) queryDto = queryDto.Paginate(request);

            return queryDto;
        }
    }
}
