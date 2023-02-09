using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text;

namespace UnitTestDemo.WebApi.Controllers;

[ApiController]
[Route("api/echo")]
public class EchoController : ControllerBase
{

    [HttpGet, HttpPost, HttpPut, HttpDelete, HttpPatch]
    public async Task<string> Echo([FromServices] EchoHub echoHub)
    {
        var builder = new StringBuilder();

        var method = Request.Method.ToUpper();
        var path = Request.Path.ToString() + Request.QueryString;
        var protocol = Request.Protocol;

        builder.AppendFormat("{0} {1} {2}{3}", method, path, protocol, Environment.NewLine);

        foreach(var header in Request.Headers)
        {
            builder.AppendFormat("{0}: {1}{2}", header.Key, header.Value, Environment.NewLine);
        }

        if(Request.Body.CanRead)
        {
            builder.AppendLine();
            using var sr = new StreamReader(Request.Body);

            builder.Append(await sr.ReadToEndAsync());
        }

        var response = builder.ToString();

        // Send to SignalR Socket
        await echoHub.Echo(response);

        // Return API Response
        return response;
    }
}
