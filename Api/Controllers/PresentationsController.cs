using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using InteractivePresentation.Client.Models;
using InteractivePresentation.Client.Service.Abstract;
using InteractivePresentation.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("presentations")]
    [ApiController]
    public class PresentationsController(IPresentationClientService clientService, IPollService pollService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PresentationRequest presentation)
        {
            if (presentation == null)
            {
                return BadRequest("Mandatory body parameters missing or have incorrect type.");
            }

            var response = await clientService.PostAsync(presentation);

            return Created("/presentations/" + response.PresentationId, new { presentation_id = response.PresentationId });
        }

        [HttpGet("{presentation_id:guid}")]
        public async Task<IActionResult> Get([Required] Guid presentation_id)
        {
            if (presentation_id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(presentation_id));
            }
            var response = await clientService.GetAsync(presentation_id);
            return Ok(response);
        }
    }
}
