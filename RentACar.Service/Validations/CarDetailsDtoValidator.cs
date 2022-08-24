using FluentValidation;
using RentACar.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentACar.Service.Validations
{
    public class CarDetailsDtoValidator : AbstractValidator<CarDetailsDto>
    {
        public CarDetailsDtoValidator()
        {
            RuleFor(x => x.CarYear).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Color).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
