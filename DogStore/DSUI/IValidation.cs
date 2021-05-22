namespace DSUI
{
    /// <summary>
    /// Validation interface, see implementation for comments
    /// </summary>
    public interface IValidation
    {
        string ValidateString(string message);

        int ValidateInt(string message);

        double ValidateDouble(string message);

        string ValidateAddress(string message);

        long ValidatePhone(string message);

        string ValidateName(string message);

        char ValidateGender(string message);

        int ValidateOrderSearchOptions(string message);
    }
}