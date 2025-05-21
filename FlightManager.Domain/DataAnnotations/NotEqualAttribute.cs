using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FlightManager.Domain.DataAnnotations
{
    public class NotEqualAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _otherProperty;

        public NotEqualAttribute(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherProperty = validationContext.ObjectType.GetProperty(_otherProperty);
            if (otherProperty == null)
                return new ValidationResult($"Unknown property: {_otherProperty}");

            var otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            if (value != null && value.Equals(otherValue))
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be different from {_otherProperty}");

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-notequal", ErrorMessage ?? "Fields must not be equal.");
            MergeAttribute(context.Attributes, "data-val-notequal-other", _otherProperty);
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
                return false;

            attributes.Add(key, value);
            return true;
        }
    }
}

