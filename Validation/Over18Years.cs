using System.ComponentModel.DataAnnotations;

namespace TUQA_Shop.Validation
{
    public class Over18Years : ValidationAttribute

    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                if (DateTime.Now.Year - date.Year > 18)
                    return true;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be at least 18 years old !";
        }
    }
}
