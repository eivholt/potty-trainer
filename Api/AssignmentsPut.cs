using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Data;

namespace Api;

public class AssignmentsPut
{
    private readonly IAssignmentData productData;

    public AssignmentsPut(IAssignmentData productData)
    {
        this.productData = productData;
    }

    [FunctionName("ProductsPut")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products")] HttpRequest req,
        ILogger log)
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var product = JsonSerializer.Deserialize<Assignment>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var updatedProduct = await productData.UpdateAssignment(product);
        return new OkObjectResult(updatedProduct);
    }
}
