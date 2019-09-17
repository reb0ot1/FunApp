using FunApp.Services.DataServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FunApp.Web.Models
{
    public class ValidCategoryId : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var service = (ICategoryService)validationContext
                .GetService(typeof(ICategoryService));

            if (service.IsCategoryIdValid((int)value))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Category id does not exists!");
        }
    }
}
