using System.ComponentModel.DataAnnotations;

namespace ToDo.UI.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public class NotRequired : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
    }
}
