
﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.ProductDTO
{
    public class ProductDTO
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name can't be longer than 100 characters")]
        public string NameProduct { get; set; }

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
        public string DescriptionProduct { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [ValidateType(typeof(double), ErrorMessage = "Price must be a valid number")]
        [NonNegative(ErrorMessage = "Price must be a non-negative number")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [NonNegative(ErrorMessage = "Quantity must be a non-negative integer")]
        [ValidateType(typeof(int), ErrorMessage = "Quantity must be a valid number")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [ValidateType(typeof(int), ErrorMessage = "Category ID must be an integer")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Material ID is required")]
        [ValidateType(typeof(int), ErrorMessage = "Material ID must be an integer")]
        public int MaterialId { get; set; }

        [Required(ErrorMessage = "Gender ID is required")]
        [ValidateType(typeof(int), ErrorMessage = "Gender ID must be an integer")]
        public int GenderId { get; set; }
        public List<string> ImageURLs { get; set; }
        public int ZodiacId { get; set; }



    }

    public class NonNegativeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value is int intValue && intValue < 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
                else if (value is double doubleValue && doubleValue < 0)
                {
                    return new ValidationResult(ErrorMessage);
                }
                else if (value is string stringValue)
                {
                    if (!double.TryParse(stringValue, out double parsedValue) || parsedValue < 0)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }

            return ValidationResult.Success;
        }
    }

    public class ValidateTypeAttribute : ValidationAttribute
    {
        private readonly Type _expectedType;

        public ValidateTypeAttribute(Type expectedType)
        {
            _expectedType = expectedType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value.GetType() != _expectedType)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
