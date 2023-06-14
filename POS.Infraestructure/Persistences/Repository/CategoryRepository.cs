using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infraestructure.Commons.Bases.Request;
using POS.Infraestructure.Commons.Bases.Response;
using POS.Infraestructure.Persistences.Context;
using POS.Infraestructure.Persistences.Interfaces;
using POS.Utilities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Infraestructure.Persistences.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(PuntoDeVentaContext context) : base(context) { }

        public async Task<BaseEntityResponse<Category>> ListCategories(BaseFiltersRequest request)
        {
            var response = new BaseEntityResponse<Category>();

            //var categories = (from c in _context.Categories
            //                  where c.AuditDeleteUser == null && c.AuditDeleteDate == null
            //                  select c).AsNoTracking().AsQueryable();
            var categories = GetEntityQuery(x => x.AuditDeleteUser == null && x.AuditDeleteDate == null);
            
            if(request.NumFilter is not null && !string.IsNullOrEmpty(request.TextFilter))
            {
                switch (request.NumFilter)
                {
                    case 1:
                        categories = categories.Where(x => x.Name!.Contains(request.TextFilter));
                        break;
                    case 2:
                        categories = categories.Where(x => x.Description!.Contains(request.TextFilter));
                        break;
                }
            }

            if(request.StateFilter is not null)
            {
                categories = categories.Where(x => x.State.Equals(request.StateFilter));
            }

            if(string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
            {
                categories = categories.Where(x => x.AuditCreateDate >= Convert.ToDateTime(request.StartDate) &&
                    x.AuditCreateDate <= Convert.ToDateTime(request.EndDate).AddDays(1)); 
            }

            if (request.Sort is null) request.Sort = "Id";

            response.TotalRecords = await categories.CountAsync();
            response.Items = await Ordering(request, categories, !(bool)request.Download).ToListAsync();

            return response;
        }
    }
}
