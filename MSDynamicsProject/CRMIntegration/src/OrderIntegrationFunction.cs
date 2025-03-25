using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

class OrderIntegrationFunction
{
    static async Task Main()
    {
        Console.WriteLine($"[{DateTime.Now}]  Starting fetching process...");

        string token = await GetAccessToken();
        Console.WriteLine($"[{DateTime.Now}] Access token acquired.");

        var crmUrl = "https://org98da8e1f.api.crm.dynamics.com/api/data/v9.2";
        var todayStart = DateTime.UtcNow.Date.ToString("yyyy-MM-ddTHH:mm:ssZ");
        var apiUrl = $"{crmUrl}/api/data/v9.2/salesorders?$filter=createdon ge {todayStart}";


        Console.WriteLine($"[{DateTime.Now}]  Fetching orders created on {todayStart}");

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
        httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.GetAsync(apiUrl);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"[{DateTime.Now}] failed to fetch orders: {response.StatusCode}");
            Console.WriteLine($"[{DateTime.Now}] Response: {responseBody}");
            return;
        }


        //string json = JsonConvert.SerializeObject(responseBody, Formatting.Indented);
        //Console.WriteLine(json);


        responseBody = JsonConvert.SerializeObject(responseBody);


        dynamic orders = JsonConvert.DeserializeObject(responseBody);
        Console.WriteLine($"[{DateTime.Now}] todays orders");

        var mockApiUrl = "https://demo8845675.mockable.io/order";

        foreach (var order in orders)
        {
            var payload = new
            {
                OrderId = order["id"],
                CustomerName = order["name"],
                OrderTotal = order["totalamount"],
                OrderDate = order["createdon"]

            };

            // Create a handler that ignores SSL certificate validation
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            // Create the HttpClient with the custom handler
            HttpClient httpClient2 = new HttpClient(handler);

            var jsonPayload = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            Console.WriteLine($"[{DateTime.Now}] sending order: {payload.CustomerName}");

            var sendResult = await httpClient2.PostAsync(mockApiUrl, jsonPayload);

            if (sendResult.IsSuccessStatusCode)
                Console.WriteLine($"[{DateTime.Now}] order sent: {payload.CustomerName}");
            else
                Console.WriteLine($"[{DateTime.Now}] failed to send order {payload.OrderId} Status Code: {sendResult.StatusCode}");
        }

        Console.WriteLine($"[{DateTime.Now}] process completed.");
    }

    static async Task<string> GetAccessToken()
    {
        Console.WriteLine($"[{DateTime.Now}] Requesting access token");
        var tenantId = "fda5d2d7-78d7-4a66-a78b-f35f92ff86df";
        var clientId = "n1X8Q~aN3Apr7M7lWUAqHVsap5tI8aqs6Bfbxau0";
        var clientSecret = "c4436d8c-d638-4a33-bf90-f0f66b83098f4";
        var resource = "https://org98da8e1f.api.crm.dynamics.com";

        using var client = new HttpClient();
        var body = new StringContent($"client_id={clientId}&client_secret={clientSecret}&resource={resource}&grant_type=client_credentials",
            Encoding.UTF8, "application/x-www-form-urlencoded");

        var tokenResponse = await client.PostAsync($"https://login.microsoftonline.com/{tenantId}/oauth2/token", body);
        var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

        if (!tokenResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($"[{DateTime.Now}] Token request failed: {tokenResponse.StatusCode}");
            Console.WriteLine($"[{DateTime.Now}] Token response: {tokenContent}");
            //throw new Exception("Failed to acquire token");
        }

        dynamic tokenData = JsonConvert.DeserializeObject(tokenContent);
        return tokenData.access_token;
    }
}
