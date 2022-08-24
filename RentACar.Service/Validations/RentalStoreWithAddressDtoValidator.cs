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
    public class RentalStoreWithAddressDtoValidator : AbstractValidator<RentalStoreWithAddressDto>
    {
        public RentalStoreWithAddressDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Email).EmailAddress().NotNull().WithMessage("{PropertyName} is required"); RuleFor(x => x.Address.Street).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.phone)
          .NotEmpty()
          .NotNull().WithMessage("Phone Number is required.")
          .MinimumLength(10).WithMessage("PhoneNumber must not be less than 10 characters.")
          .MaximumLength(20).WithMessage("PhoneNumber must not exceed 50 characters.")
          .Matches(new Regex(@"((\(\d{3}?\) )|(\d{3}?-))\d{4}?")).WithMessage("PhoneNumber not valid, Example -> 555-555-5555");

            RuleFor(x => x.Address.Neighbourhood).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Address.Zipcode).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Address.DistrictId).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Address.description).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
