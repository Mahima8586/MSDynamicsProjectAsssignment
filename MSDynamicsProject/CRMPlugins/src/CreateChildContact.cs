using System;
using Microsoft.Xrm.Sdk;

/**
 * @file CreateChildContact.cs
 * @description Plugin to create a child Contact when a new Account is created.

 * @version 1.0
 */
public class CreateChildContact : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        // ✅ Obtain execution context
        IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

        // ✅ Ensure the plugin executes only on Account creation
        if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
        {
            Entity account = (Entity)context.InputParameters["Target"];

            // ✅ Get the Account Name
            string accountName = account.Contains("name") ? account["name"].ToString() : "Unknown";

            // ✅ Obtain organization service
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            // ✅ Create new Contact linked to the Account
            Entity contact = new Entity("contact");
            contact["firstname"] = "Default";
            contact["lastname"] = accountName;
            contact["parentcustomerid"] = new EntityReference("account", account.Id); // Link contact to the account

            service.Create(contact);

            // ✅ Log message in Plugin Trace Log
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            tracingService.Trace("Child Contact created successfully for Account: " + accountName);
        }
    }
}
