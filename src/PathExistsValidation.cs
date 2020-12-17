using System.ComponentModel.DataAnnotations;
using System.IO;

namespace AnnotationInConsoleAppFramework
{
    public class PathExists : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string path && File.Exists(path))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"Path not found: {value}");
        }
    }
}
