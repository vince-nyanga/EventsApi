using System.Collections.Generic;
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
    [Route("/api/speakers")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class SpeakersController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<SpeakersController> _logger;
        private readonly IMapper _mapper;

        public SpeakersController(IRepository repository,
            ILogger<SpeakersController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<SpeakerDto>> Get(int id)
        {
            _logger.LogInformation("Getting speaker with id {SpeakerId}", id);

            var speaker = await _repository.GetByIdAsync<Speaker>(id);

            if (speaker == null)
            {
                _logger.LogInformation("Speaker with id {SpeakerId} does not exist", id);
                return NotFound();
            }

            _logger.LogDebug("Returning speaker: {@Speaker}", speaker);

            return Ok(_mapper.Map<SpeakerDto>(speaker));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Put(int id,
            [FromBody] SpeakerForUpdateDto speakerDto)
        {
            
            _logger.LogInformation("Getting Speaker with id {SpeakerId}", id);

            var speaker = await _repository.GetByIdAsync<Speaker>(id);

            if (speaker == null)
            {
                _logger.LogInformation("Speaker with id {SpeakerId} not found", id);
                return NotFound();
            }

            speaker.Name = speakerDto.Name;
            speaker.Email = speakerDto.Email;

            await _repository.UpdateAsync(speaker);

            _logger.LogInformation("Speaker with id {SpeakerId} successfully updated", id);

            return NoContent();
        }

    }
}
