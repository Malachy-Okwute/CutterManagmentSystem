namespace CutterManagement.Core
{
    public class UserValidationPolicy : DataValidationBase<UserDataModel>
    {
        public override ValidationResult Validate(UserDataModel data)
        {
            ValidationResult result = CreateValidationInstance();

            if (data is null)
               return ErrorReport(result, "A valid user is required");

            if (string.IsNullOrEmpty(data.FirstName))
                return ErrorReport(result, "User's first name is required");
            
            if (data.FirstName.Length < 2)
                return ErrorReport(result, "Use a minimum of 3 letters for first name");

            if (string.IsNullOrEmpty(data.LastName))
                return ErrorReport(result, "User's last name is required");

            if (data.LastName.Length < 2)
                return ErrorReport(result, "Use a minimum of 3 letters for last name");

            if (data.Shift is UserShift.None)
                return ErrorReport(result, "Please select a shift for the user");

            return result;
        }
    }
}
