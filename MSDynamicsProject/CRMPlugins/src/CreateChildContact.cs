using System;
using System.Security.Principal;
using System.Web.UI.WebControls;
using Microsoft.Xrm.Sdk;

namespace ContactPlugins
{
    public class CreateContactOnAccountCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            //Obtain execution context
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            
            // Initializing logger to  Trace Log
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            
            // Obtain organization service
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                // the plugin executes only on Account creation----
                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity entity && entity.LogicalName == "account")
                {
                    
                    // get the Account Name 
                    string accountName = entity.GetAttributeValue<string>("name");

                    // Creating new Contact 
                    Entity contact = new Entity("contact");
                    contact["firstname"] = "Default";
                    contact["lastname"] = accountName;
                    contact["parentcustomerid"] = new EntityReference("account", entity.Id); // Link contact to account

                    service.Create(contact);
                    // Logging message in Plugin Trace Log-
                    tracingService.Trace("Child contact created for account: " + accountName);
                }
            }
            catch (Exception ex)
            {
                tracingService.Trace("Plugin Exception: " + ex.Message);
                throw new InvalidPluginExecutionException("An error occurred in CreateContactOnAccountCreate plugin.", ex);
            }
        }
    }
}
