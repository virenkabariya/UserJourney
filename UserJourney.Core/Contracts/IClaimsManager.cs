using UserJourney.Core.Enums;

namespace UserJourney.Core.Contracts
{
    public interface IClaimsManager
    {
        int GetCurrentUserId();

        string GetCurrentUserName();
    }
}
