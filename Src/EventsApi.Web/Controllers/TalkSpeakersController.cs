using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventsApi.Core.Entities;
using EventsApi.Core.Interfaces;
using EventsApi.Core.Specifications;
using EventsApi.Web.Models.Speaker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventsApi.Web.Controllers
{
    [ApiController]
    [Route("/api/talks/{talkId:int}/speakers")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TalkSpeakersController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<TalkSpeakersController> _logger;
        private readonly IMapper _mapper;

        public TalkSpeakersController(IRepository repository,
            ILogger<TalkSpeakersController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<SpeakerDto>>> Get(int talkId)
        {
            _logger.LogInformation("Getting talk with id {TalkId}", talkId);

            var talk = await _repository.GetByIdAsync<Talk>(talkId);

            if (talk == null)
            {
                _logger.LogInformation("Talk with id {TalkId} does not exist", talkId);
                return NotFound();
            }

            _logger.LogInformation("Getting speakers for talk with id {TalkId}", talkId);

            var speakers = await _repository.ListAsync(new SpeakersForTalkSpecification(talkId));

            _logger.LogInformation("{TotalSpeakers} speakers found", speakers.Count);

            var speakerDto = _mapper.Map<IReadOnlyList<SpeakerDto>>(speakers);

            return Ok(speakerDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post(int talkId,
            [FromBody]SpeakerForUpdateDto speakerDto)
        {
            _logger.LogInformation("Getting talk with id {TalkId}", talkId);

            var talks = await _repository.ListAsync(new TalkWithSpeakersSpecification(talkId));

            if (talks.Count == 0)
            {
                _logger.LogInformation("No talk with id {TalkId} exists", talkId);
                return NotFound();
            }

            var talk = talks[0];

            var speaker = _mapper.Map<Speaker>(speakerDto);

            _logger.LogInformation("Adding new speaker {@Speaker}", speaker);

            talk.AddSpeaker(speaker);

            await _repository.SaveChanges();

            _logger.LogInformation("Speaker added");

            return NoContent();
        }

        [HttpDelete("{speakerId:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int talkId, int speakerId)
        {
            _logger.LogInformation("Getting talk with id {TalkId}", talkId);

            var talks = await _repository.ListAsync(new TalkWithSpeakersSpecification(talkId));

            if (talks.Count == 0)
            {
                _logger.LogInformation("No talk with id {TalkId} exists", talkId);
                return NotFound();
            }

            var talk = talks[0];

            _logger.LogInformation("Getting speaker with id {SpeakerId}", speakerId);

            var speaker = talk.Speakers.Where(s => s.Id == speakerId).FirstOrDefault();

            if (speaker == null)
            {
                _logger.LogInformation("Speaker with id {SpeakerId} does not exist", speakerId);
                return NotFound();
            }

            talk.RemoveSpeaker(speaker);

            await _repository.SaveChanges();

            _logger.LogInformation("Speaker with id {SpeakerId} deleted", speakerId);

            return NoContent();
        }
    }
}
