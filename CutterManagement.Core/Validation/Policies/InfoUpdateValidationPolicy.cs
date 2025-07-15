namespace CutterManagement.Core
{
    public class InfoUpdatesValidationPolicy : DataValidationBase<InfoUpdateDataModel>
    {
        public override ValidationResult Validate(InfoUpdateDataModel data)
        {
            ValidationResult result = CreateValidationInstance();

            if (string.IsNullOrEmpty(data.Title))
                return ErrorReport(result, "Please enter a title to continue");

            if (string.IsNullOrEmpty(data.Information))
                return ErrorReport(result, "Information content is required");
            
            return result;
        }
    }
}
