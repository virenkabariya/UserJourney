﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserJourney.Core.Contracts
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(List<string> to, List<string> cc, string subject, string templateName, Dictionary<string, string> values, byte[] attachment = null, string attachmentName = null);
    }
}
