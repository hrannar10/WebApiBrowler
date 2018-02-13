﻿using FluentValidation.Attributes;
using WebApiBrowler.Dtos.Validations;

namespace WebApiBrowler.Dtos
{
    [Validator(typeof(CredentialsDtoValidator))]
    public class CredentialsDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
