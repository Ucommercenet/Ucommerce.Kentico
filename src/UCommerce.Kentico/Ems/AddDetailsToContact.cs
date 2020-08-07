using CMS.ContactManagement;
using UCommerce.Pipelines;
using UCommerce.Pipelines.AddAddress;

namespace UCommerce.Kentico.Ems
{
    public class AddDetailsToContact : IPipelineTask<IPipelineArgs<AddAddressRequest, AddAddressResult>>
    {
        public PipelineExecutionResult Execute(IPipelineArgs<AddAddressRequest, AddAddressResult> subject)
        {
            if (subject.Request.AddressName != "Billing") { return PipelineExecutionResult.Success; }

            var currentContact = ContactManagementContext.GetCurrentContact();
            if (currentContact == null) { return PipelineExecutionResult.Success; }

            currentContact.ContactEmail = subject.Request.EmailAddress;

            currentContact.ContactFirstName = subject.Request.FirstName;
            currentContact.ContactLastName = subject.Request.LastName;
            currentContact.ContactMobilePhone = subject.Request.MobilePhoneNumber;
            currentContact.ContactCompanyName = subject.Request.Company;
            currentContact.ContactAddress1 = subject.Request.Line1;
            currentContact.ContactCity = subject.Request.City;
            currentContact.ContactZIP = subject.Request.PostalCode;

            ContactInfoProvider.SetContactInfo(currentContact); // Remember to save the updated data to the database.

            return PipelineExecutionResult.Success;
        }
    }
}
