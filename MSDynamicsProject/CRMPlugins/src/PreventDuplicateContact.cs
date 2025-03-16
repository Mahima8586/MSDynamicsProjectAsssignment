using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

/**
 * @file PreventDuplicateContact.cs
 * @description Plugin to prevent duplicate contacts with the same email.

 * @version 1.0
 */
public class PreventDuplicateContact : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        // ✅ Obtain context for execution
        IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

        // ✅ Ensure this runs only on Contact entity Create event
        if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
        {
            Entity contact = (Entity)context.InputParameters["Target"];

            // ✅ Check if Email exists in the form submission
            if (!contact.Attributes.Contains("emailaddress1") || contact["emailaddress1"] == null)
                return;

            string email = contact["emailaddress1"].ToString();

            // ✅ Obtain organization service
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            // ✅ Query Dataverse for existing Contacts with the same email
            QueryExpression query = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet("contactid"),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("emailaddress1", ConditionOperator.Equal, email)
                    }
                }
            };

            EntityCollection existingContacts = service.RetrieveMultiple(query);

            // ✅ If a contact with the same email exists, throw an error
            if (existingContacts.Entities.Count > 0)
            {
                throw new InvalidPluginExecutionException("A contact with this email address already exists.");
            }
        }
    }
}
