using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SySDEAProject.Models
{
    public class ValidateHoraAgendamento : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (DateTime.Parse(value.ToString()).Date >= DateTime.Now.Date)
                {
                    if (DateTime.Parse(value.ToString()).TimeOfDay >= DateTime.Now.TimeOfDay)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult("A hora não deve ser anterior à atual");
                    }
                }
                else
                {
                    return new ValidationResult("A data não não deve ser anterior à atual.");
                }
            }
            return new ValidationResult("O campo Data/Hora não pode ser nulo");
        }
    }
    public class ValidateDataAgendamento : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // your validation logic

            if (value != null)
            {

                if (DateTime.Parse(value.ToString()).Date >= DateTime.Now.Date)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("A data não não deve ser anterior à atual.");
                }
            }
            else
            {
                return new ValidationResult("O campo data é obrigatório");
            }
        }
    }
    public class ValidateDataHoraAgendamento : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // your validation logic

            if (DateTime.Parse(value.ToString()).TimeOfDay != null)
            {

                if (DateTime.Parse(value.ToString()).Date > DateTime.Now.Date)
                {
                    return ValidationResult.Success;
                }
                else
                    if (DateTime.Parse(value.ToString()).Date == DateTime.Now.Date && DateTime.Parse(value.ToString()).TimeOfDay > DateTime.Now.TimeOfDay)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("A data não não deve ser anterior à atual.");
                }
            }
            else
            {
                return new ValidationResult("O campo data é obrigatório");
            }
        }
    }
    public class ValidateDataHoraAgendamento2 : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // your validation logic

            if (DateTime.Parse(value.ToString()).TimeOfDay != null)
            {

                if (DateTime.Parse(value.ToString()).Date > DateTime.Now.Date)
                {
                    return ValidationResult.Success;
                }
                else
                    if (DateTime.Parse(value.ToString()).Date == DateTime.Now.Date && DateTime.Parse(value.ToString()).TimeOfDay > DateTime.Now.TimeOfDay)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("A data não não deve ser anterior à atual.");
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

