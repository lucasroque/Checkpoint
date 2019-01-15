using System.Globalization;
using System.Windows.Controls;

namespace Checkpoint.Tools
{
    class NotSelectedValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (value is int && (int)value >= 0)
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Campo Obrigatório");
        }
    }
}
