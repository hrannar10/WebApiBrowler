using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiBrowler.Entities
{
    public class AssetType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public bool UserCreatedType { get; set; }
        [RequireWhenUserCreatedType]
        public Guid? UserId { get; set; }
    }

    /// <inheritdoc />
    public class RequireWhenUserCreatedTypeAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        /// <summary>Validates the specified value with respect to the current validation attribute.</summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult"></see> class.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((bool)validationContext.ObjectInstance)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("UserId is required.");
        }
    }
}
