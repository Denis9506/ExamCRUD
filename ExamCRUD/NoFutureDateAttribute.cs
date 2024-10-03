using System;
using System.ComponentModel.DataAnnotations;

public class NoFutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime dateValue)
        {
            return dateValue <= DateTime.Now;
        }
        return true;
    }
}
