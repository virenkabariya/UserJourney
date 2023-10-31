using UserJourney.Core.Constants;
using UserJourney.Core.Contracts;
using UserJourney.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace UserJourney.Core.Services
{
    public class ClaimsManager : IClaimsManager
    {
        private HttpContext _httpContext;
        public ClaimsManager(IHttpContextAccessor contextAccessor)
        {
            _httpContext = contextAccessor.HttpContext;
        }

        public int GetCurrentUserId()
        {
            var userId = _httpContext.User.FindFirst($"{CustomClaimTypes.UserId}");
            return Convert.ToInt32(userId == null ? "0" : userId.Value);
        }

        public string GetCurrentUserName()
        {
            var firstName = _httpContext.User.FindFirst($"{CustomClaimTypes.FirstName}");
            var LastName = _httpContext.User.FindFirst($"{CustomClaimTypes.LastName}");
            return (firstName != null ? firstName.Value : "") + " " + (LastName != null ? LastName.Value : "");
        }

    }
}
