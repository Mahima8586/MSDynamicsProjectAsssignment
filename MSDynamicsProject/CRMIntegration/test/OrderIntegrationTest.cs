
---

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;

public class OrderIntegrationTest
{
    [Fact]
    public async Task Test_Valid_Order_Should_Return_Success()
    {
        var client = new HttpClient();
        var orderPayload = new
        {
            customername = "Mahima Verma ",
            ordertotal = 200.50,
            orderdate = DateTime.UtcNow
        };

        string jsonPayload = JsonConvert.SerializeObject(orderPayload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("http://localhost:3000/receiveOrder", content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Test_Invalid_Order_Should_Return_BadRequest()
    {
        var client = new HttpClient();
        var invalidPayload = new
        {
            customername = "",
            ordertotal = -10
        };

        string jsonPayload = JsonConvert.SerializeObject(invalidPayload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("http://localhost:3000/receiveOrder", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
