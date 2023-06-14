using POS.Application.Commons.Bases;
using POS.Application.Dtos.Request;
using POS.Application.Dtos.Response;
using POS.Infraestructure.Commons.Bases.Request;
using POS.Infraestructure.Commons.Bases.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Interfaces
{
    public interface ICategoryApplication
    {
        Task<BaseResponse<BaseEntityResponse<CategoryResponseDTO>>> ListCategories(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<CategorySelectResponseDTO>>> ListSelectCategories();
        Task<BaseResponse<CategoryResponseDTO>> CategoryById(int CategoryId);
        Task<BaseResponse<bool>> RegisterCategory(CategoryRequestDTO requestDTO);
        Task<BaseResponse<bool>> EditCategory(int CategoryId, CategoryRequestDTO requestDTO);
        Task<BaseResponse<bool>> RemoveCategory(int CategoryId);
    }
}
