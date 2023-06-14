using FluentValidation;
using POS.Application.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Validators.Category
{
    public class CategoryValidator : AbstractValidator<CategoryRequestDTO>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("El campo nombre no debe ser nulo")
                                .NotEmpty().WithMessage("El campo nombre no puede estar vacio");

            RuleFor(x => x.Description).NotNull().WithMessage("El campo descripcion no puede ser nulo")
                                       .NotEmpty().WithMessage("El campo descripcion no puede estar vacio");
        }
    }
}
