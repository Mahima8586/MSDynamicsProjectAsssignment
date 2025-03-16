using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

/**
 * @file OrderIntegrationFunction.cs
 * @description Azure Function to send Dynamics 365 Order details to an external API.
 */
public static class OrderIntegrationFunction
{
    private static readonly HttpClient httpClient = new HttpClient();

    [FunctionName("OrderIntegration")]
    public static async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("Received a new Order from Dynamics 365.");

        try
        {
            // Read request body from Dynamics 365
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // Extract order details
            string customerName = data?.customername;
            decimal orderTotal = data?.ordertotal;
            DateTime orderDate = data?.orderdate;

            if (customerName == null || orderTotal <= 0)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Invalid order data.")
                };
            }

            // Prepare payload for external API
            var payload = new
            {
                customerName = customerName,
                orderTotal = orderTotal,
                orderDate = orderDate
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Call the mock external API
            string apiUrl = Environment.GetEnvironmentVariable("ExternalApiUrl");
            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                log.LogInformation($"Successfully sent order data for {customerName}.");
            }
            else
            {
                log.LogError($"Failed to send order data: {response.StatusCode}");
            }

            return response;
        }
        catch (Exception ex)
        {
            log.LogError($"Error processing order: {ex.Message}");
            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
            {
                Content = new StringContent($"Server error: {ex.Message}")
            };
        }
    }
}
