using System.Net;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Kubernetes.EventBridge.Host.Controllers
{
    [Route("api/v1/health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        ///     Get Health
        /// </summary>
        /// <remarks>Provides an indication about the health of the host</remarks>
        [HttpGet]
        [SwaggerOperation(OperationId = "Health_Get")]
        [SwaggerResponse((int) HttpStatusCode.OK, Description = "Host is healthy")]
        [SwaggerResponse((int) HttpStatusCode.ServiceUnavailable, Description = "Host is not healthy")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}