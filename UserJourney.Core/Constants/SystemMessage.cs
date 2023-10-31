namespace UserJourney.Core.Constants
{
    public class SystemMessage
    {
        public const string InvalidInput = "Invalid Input.";
        public const string PhoneNumberErrorMessage = "Please enter phone number in valid format.";
        public const string EmailErrorMessage = "Please enter email in valid format e.g. email@domain.com.";
        public const string InvalidPassword = "Password must contain at least one Uppercase, one Lowercase, one Numeric and one special character. Password should have minimum 8 characters.";
        public const string InvalidUserNameandPassword = "Invalid login credentials.";
        public const string EmailDuplicateMessage = "Email already exists. Please use another email.";
        public const string EmailValidMessage = "Please enter a valid email.";
        public const string UserRegistrationMessage = "User registration completed successfully.";
        public const string ModelIsNotValid = "There are few fields not submitted correctly in page.";
        public const string PasswordUpdated = "Password updated successfully.";
        public const string OldNewPasswordsSame = "New password cannot be same as old password.";
        public const string IncorrectOldPassword = "Old password is incorrect.";
        public const string LinkExpired = "This link has expired.";
        public const string LinkSent = "The link has been sent successfully.";
        public const string ErrorOnSendingMail = "A problem occurred while sending the email. Please try again later.";
        public const string ErrorOnTakingUserInfo = "A problem occurred while taking user information. Please try again later.";
    }
}
