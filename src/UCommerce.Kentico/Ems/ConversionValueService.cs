namespace UCommerce.Kentico.Ems
{
    public class ConversionValueService : IGetConversionValue
    {
        public double GetConversionValue(string conversionSettingsString, double initialValue)
        {
            if (IsPercentage(conversionSettingsString))
            {
                double percentage = GetDoubleValue(conversionSettingsString.Substring(0, conversionSettingsString.Length - 1));
                double productValue = initialValue;

                return productValue * percentage / 100;
            }

            // Direct value.
            double v = GetDoubleValue(conversionSettingsString);
            return v;
        }

        private double GetDoubleValue(string val)
        {
            if (double.TryParse(val, out var v))
            {
                return v;
            }

            // Default value, if all else fails.
            return 1;
        }

        private bool IsPercentage(string val)
        {
            val = val.Trim();
            return val.EndsWith("%");
        }
    }
}
