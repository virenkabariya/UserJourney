using System.ComponentModel;

namespace UserJourney.Core.Enums
{
    public enum CookieName
    {
        [Description("USIsRemember")]
        IsRemember,

        [Description("USUserName")]
        UserName,

        [Description("USPassword")]
        Password,

        [Description("LoginUserInfo")]
        LoginUserInfo,
    }
}
