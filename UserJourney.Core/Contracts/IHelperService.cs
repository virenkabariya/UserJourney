namespace UserJourney.Core.Contracts
{
    public interface IHelperService
    {
        void AddCookie(string cookieName, string cookieValue);

        void RemoveCookie(string cookieName);

        string GetCookieValue(string cookieName);
    }
}
