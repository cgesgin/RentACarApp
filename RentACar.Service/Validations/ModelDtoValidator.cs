using FluentValidation;
using RentACar.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service.Validations
{
    public class ModelDtoValidator : AbstractValidator<ModelDto>
    {
        public ModelDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is required hah").NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
