using FluentValidation;
using RentACar.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RentACar.Service.Validations
{
    public class RentalWithCostumerDtoValidator : AbstractValidator<RentalWithCostumerDto>
    {
        public RentalWithCostumerDtoValidator()
        {

            RuleFor(x => x.DropStoreId).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.RentalDate).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.ReturnDate).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Costumer.Name).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Costumer.LastName).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Costumer.Email).EmailAddress().NotNull().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Costumer.LisanceNo).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(p => p.Costumer.Phone)
            .NotEmpty()
            .NotNull().WithMessage("Phone Number is required.")
            .MinimumLength(10).WithMessage("PhoneNumber must not be less than 10 characters.")
            .MaximumLength(20).WithMessage("PhoneNumber must not exceed 50 characters.")
            .Matches(new Regex(@"((\(\d{3}?\) )|(\d{3}?-))\d{4}?")).WithMessage("PhoneNumber not valid, Example -> 555-555-5555");
        }
    }
}
