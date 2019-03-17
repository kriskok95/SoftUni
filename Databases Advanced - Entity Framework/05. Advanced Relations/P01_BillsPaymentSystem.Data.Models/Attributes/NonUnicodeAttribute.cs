using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Models.Attributes
{
    class NonUnicodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string nullMsg = "The value can't be null!";

            if (value == null)
            {
                return new ValidationResult(nullMsg);
            }

            string text = (string)value;

            string nonUnicodeMsg = "The value must be non unicode!";

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 255)
                {
                    return new ValidationResult(nonUnicodeMsg);
                }
            }

            return ValidationResult.Success;
        }          
    }
}
