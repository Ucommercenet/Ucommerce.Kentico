using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using CMS.EmailEngine;
using CMS.MacroEngine;
using CMS.SiteProvider;
using UCommerce.Content;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure.Globalization;
using UCommerce.Infrastructure.Logging;

namespace UCommerce.Kentico.Email
{
    public class EmailService : Transactions.EmailService
    {
        public EmailService(IContentService contentService, IRepository<EmailType> emailTypeRepository,
            ILoggingService loggingService) : base(contentService, emailTypeRepository, loggingService)
        {
        }

        public override string Send(ILocalizationContext localizationContext, EmailProfile profile,
            string emailTypeName, MailAddress to, IDictionary<string, string> templateParameters)
        {
            var emailContent = GetEmailContentInternal(localizationContext, profile, emailTypeName, to, templateParameters);

            int emailProfileId;

            if (!int.TryParse(emailContent.ContentId, out emailProfileId))
            {
                throw new InvalidOperationException("content configured on email content could not be parsed");
            };

            var templateInfo = CMS.EmailEngine.EmailTemplateProvider.GetEmailTemplate(emailProfileId);

            MacroResolver macroResolver = MacroResolver.GetInstance();
            
            EmailMessage msg = new EmailMessage();
            msg.EmailFormat = EmailFormatEnum.Both;
            msg.From = templateInfo.TemplateFrom; //make sure this is specified in the template settings
            msg.Recipients = to.Address;
            msg.Subject = templateInfo.TemplateSubject;

            EmailSender.SendEmailWithTemplateText(SiteContext.CurrentSiteName, msg, templateInfo, macroResolver, true);

            return string.Empty;
        }
    }
}
