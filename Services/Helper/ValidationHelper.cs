using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Services.Helper
{
    public class ValidationHelper
    {

        internal static void ModelValidation(object obj)
        {
            //Model Validation

            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResult = new List<ValidationResult>();

            bool isValid=Validator.TryValidateObject(obj,validationContext,validationResult,true);

            if (!isValid)
                throw new ArgumentException(validationResult.FirstOrDefault()?.ErrorMessage);
        }
    }
}
