using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using InteractivePresentation.Client.Service.Abstract;
using InteractivePresentation.Domain.Entity;
using InteractivePresentation.Domain.Model;
using InteractivePresentation.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("presentations/{presentation_id:guid}/polls")]
    [ApiController]
    public class VotesController(IPresentationClientService clientService, IPollService pollService) : ControllerBase
    {
        [HttpPost("current/votes")]
        public async Task<IActionResult> CreateVote([FromRoute, Required] Guid presentation_id, [FromBody] VoteRequest vote)
        {
            ArgumentNullException.ThrowIfNull(vote);
            if (presentation_id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(presentation_id));
            }
            try
            {
                var poll = await pollService.GetCurrentPollAsync(presentation_id);
                if (poll == null)
                {
                    return NotFound("Either `presentation_id` or `poll_id` not found");
                }
                if (poll.Id != vote.PollId)
                {
                    return Conflict("In case of `poll_id` not matching currently displayed poll");
                }

                await pollService.CreateVoteAsync(presentation_id, vote.PollId, vote);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{poll_id:guid}/votes")]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotes([FromRoute, Required] Guid presentation_id, [FromRoute, Required] Guid poll_id)
        {
            if (presentation_id == Guid.Empty || poll_id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(presentation_id));
            }
            var poll = await pollService.GetCurrentPollAsync(presentation_id);
            if (poll == null)
            {
                return NotFound("Either `presentation_id` or `poll_id` not found");
            }
            if (poll.Id != poll_id)
            {
                return Conflict("In case of `poll_id` not matching currently displayed poll");
            }

            var votes = await pollService.GetVotesAsync(presentation_id, poll_id);

            if (votes == null)
            {
                return Conflict("There are no polls currently displayed");
            }

            return Ok(votes);
        }
    }
}
