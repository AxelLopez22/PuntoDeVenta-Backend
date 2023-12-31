﻿using AutoMapper;
using POS.Application.Commons.Bases;
using POS.Application.Dtos.Request;
using POS.Application.Dtos.Response;
using POS.Application.Interfaces;
using POS.Application.Validators.Category;
using POS.Domain.Entities;
using POS.Infraestructure.Commons.Bases.Request;
using POS.Infraestructure.Commons.Bases.Response;
using POS.Infraestructure.Persistences.Interfaces;
using POS.Utilities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Services
{
    public class CategoryApplication : ICategoryApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CategoryValidator _validationRules;

        public CategoryApplication(IUnitOfWork unitOfWork, IMapper mapper, CategoryValidator validationRules)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validationRules = validationRules;
        }

        public async Task<BaseResponse<CategoryResponseDTO>> CategoryById(int CategoryId)
        {
            var response = new BaseResponse<CategoryResponseDTO>();
            var category = await _unitOfWork.Category.GetByIdAsync(CategoryId);

            if(category is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<CategoryResponseDTO>(category);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            } else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> EditCategory(int CategoryId, CategoryRequestDTO requestDTO)
        {
            var response = new BaseResponse<bool>();
            var categoryEdit = await CategoryById(CategoryId);

            if(categoryEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }

            var category = _mapper.Map<Category>(requestDTO);
            category.Id = CategoryId;
            response.Data = await _unitOfWork.Category.EditAsync(category);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_UPDATE;
            } else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        public async Task<BaseResponse<BaseEntityResponse<CategoryResponseDTO>>> ListCategories(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<CategoryResponseDTO>>();
            var categories = await _unitOfWork.Category.ListCategories(filters);

            if(categories is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<BaseEntityResponse<CategoryResponseDTO>>(categories);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            } else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }

            return response;
        }

        public async Task<BaseResponse<IEnumerable<CategorySelectResponseDTO>>> ListSelectCategories()
        {
            var response = new BaseResponse<IEnumerable<CategorySelectResponseDTO>>();
            var categories = await _unitOfWork.Category.GetAllAsync();

            if(categories is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<CategorySelectResponseDTO>>(categories);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterCategory(CategoryRequestDTO requestDTO)
        {
            var response = new BaseResponse<bool>();
            var validationResult = await _validationRules.ValidateAsync(requestDTO);

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_VALIDATION;
                response.Error = validationResult.Errors;
                return response;
            }

            var category = _mapper.Map<Category>(requestDTO);
            response.Data = await _unitOfWork.Category.RegisterAsync(category);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RemoveCategory(int CategoryId)
        {
            var response = new BaseResponse<bool>();
            var category = await CategoryById(CategoryId);

            if (category.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }

            response.Data = await _unitOfWork.Category.DeleteAsync(CategoryId);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }

            return response;
        }
    }
}
