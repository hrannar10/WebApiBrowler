﻿using FluentValidation;

namespace WebApiBrowler.Dtos.Validations
{
    public class CredentialsDtoValidator : AbstractValidator<Requests.CredentialsDto>
    {
        public CredentialsDtoValidator()
        {
            RuleFor(vm => vm.UserName).NotEmpty().WithMessage("Username cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.Password).Length(6, 12).WithMessage("Password must be between 6 and 12 characters");
        }
    }
}
