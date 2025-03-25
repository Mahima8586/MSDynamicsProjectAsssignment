using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ContactPlugins
{
    public class PreventDuplicateContactPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Obtain context for execution
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            //  this runs only on Contact entity Create event
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity entity && entity.LogicalName == "contact")
            {
                // Check if Email exists in the form 
                if (!entity.Attributes.Contains("emailaddress1"))
                    return;

                string email = entity.GetAttributeValue<string>("emailaddress1");
                if (string.IsNullOrEmpty(email))
                    return;

                // Obtain organization service
                IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                // checking email is unique ---
                QueryExpression query = new QueryExpression("contact")
                {
                    ColumnSet = new ColumnSet("emailaddress1"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("emailaddress1", ConditionOperator.Equal, email)
                        }
                    },
                    TopCount = 1
                };

                //  Throwing  error if email is duplicate -----
                var results = service.RetrieveMultiple(query);
                if (results.Entities.Any())
                {
                    throw new InvalidPluginExecutionException("A contact with this email address already exists.");
                }
            }
        }
    }
}
