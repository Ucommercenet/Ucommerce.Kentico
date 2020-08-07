namespace UCommerce.Kentico.Ems
{
    public interface IGetConversionValue
    {
        double GetConversionValue(string conversionSettingsString, double initialValue);
    }
}
