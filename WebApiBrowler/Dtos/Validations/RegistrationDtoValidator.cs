﻿using FluentValidation;
using WebApiBrowler.Dtos.Request;

namespace WebApiBrowler.Dtos.Validations
{
    public class RegistrationDtoValidator : AbstractValidator<RegistrationDtoRequest>
    {
        public RegistrationDtoValidator()
        {
            RuleFor(vm => vm.Email).NotEmpty().WithMessage("Email cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.FirstName).NotEmpty().WithMessage("FirstName cannot be empty");
            RuleFor(vm => vm.LastName).NotEmpty().WithMessage("LastName cannot be empty");
            RuleFor(vm => vm.Location).NotEmpty().WithMessage("Location cannot be empty");
        }
    }
}
