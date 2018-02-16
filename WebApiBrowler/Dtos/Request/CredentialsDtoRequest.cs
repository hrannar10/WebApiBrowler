using FluentValidation.Attributes;
using WebApiBrowler.Dtos.Validations;

namespace WebApiBrowler.Dtos.Request
{
    [Validator(typeof(CredentialsDtoValidator))]
    public class CredentialsDtoRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
