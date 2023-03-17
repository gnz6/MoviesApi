using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Validations
{
    public class FileTypeValidation : ValidationAttribute
    {
        private readonly string[] validTypes;
        
        public FileTypeValidation(string[] validTypes)
        {
            this.validTypes = validTypes;
        }

        public FileTypeValidation(GroupTypeFile groupTypeFile)
        {
            if( groupTypeFile == GroupTypeFile.Image)
            {
                validTypes = new[] { "image/jpeg", "image/png" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile != null)
            {
                return ValidationResult.Success;

            }

            if(!validTypes.Contains(formFile.ContentType))
            {
                return new ValidationResult($"The file must be type: {string.Join(",", validTypes)}");
            }

            return ValidationResult.Success;

        }

    }
}
