using FluentValidation;
using RentACar.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service.Validations
{
    public class CarTypeDtoValidator : AbstractValidator<CarTypeDto>
    {
        public CarTypeDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
