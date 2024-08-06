using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using InteractivePresentation.Client.Service.Abstract;
using InteractivePresentation.Domain.Entity;
using InteractivePresentation.Domain.Model;
using InteractivePresentation.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("presentations/{presentation_id:guid}/polls/current")]
    [ApiController]
    public class PollsController(IPresentationClientService clientService, IPollService pollService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Poll>> GetCurrentPoll([FromRoute, Required] Guid presentation_id)
        {
            if (presentation_id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(presentation_id));
            }
            var poll = await pollService.GetCurrentPollAsync(presentation_id);
            if (poll == null)
            {
                return Conflict("There are no polls currently displayed");
            }

            return Ok(poll);
        }

        [HttpPut]
        public async Task<ActionResult<Poll>> SetCurrentPoll([FromRoute, Required] Guid presentation_id)
        {
            if (presentation_id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(presentation_id));
            }
            try
            {
                var presentation = await clientService.GetAsync(presentation_id);
                if (presentation == null)
                {
                    return NotFound("No presentation found.");
                }

                var poll = await clientService.GetCurrentAsync(presentation_id);
                if (poll == null)
                {
                    return Conflict("The presentation ran out of polls.");
                }

                var setPoll = new PollRequest
                {
                    Id = poll.PollId,
                    PresentationId = presentation_id,
                    Question = poll.Question
                };

                var setCurrentPoll = await pollService.SetCurrentPollAsync(presentation_id, setPoll);
                if (setCurrentPoll == null)
                {
                    return NotFound("No presentation found.");
                }
                return Ok(setCurrentPoll);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}