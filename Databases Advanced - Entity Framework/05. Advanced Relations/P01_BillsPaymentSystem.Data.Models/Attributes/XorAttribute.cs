using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Models.Attributes
{
    public class XorAttribute : ValidationAttribute
    {
        private readonly string xorTargetAttribute;

        public XorAttribute(string xorTargetAttribute)
        {
            this.xorTargetAttribute = xorTargetAttribute;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var targetAttribute = validationContext.ObjectType
                .GetProperty(xorTargetAttribute)
                .GetValue(validationContext.ObjectInstance);

            if (targetAttribute == null && value != null || targetAttribute != null && value == null)
            {
                return ValidationResult.Success;                
            }

            string errorMsg = "The two properties must have the opposite value!";
            return new ValidationResult(errorMsg);
        }
    }
}
