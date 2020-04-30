using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EventsApi.Core.Entities;
using EventsApi.Core.Interfaces;
using EventsApi.Web.Models.Talks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace EventsApi.Web.Controllers
{
    [ApiController]
    [Route("/api/talks")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TalksController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly ILogger<TalksController> _logger;
        private readonly IMapper _mapper;

        public TalksController(IRepository repository,
            ILogger<TalksController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TalkDto>>> Get()
        {
            _logger.LogInformation("Getting all the talks");

            var talks = await _repository.ListAsync<Talk>();

            _logger.LogInformation("{Total} talks loaded", talks.Count);

            var talkDtos = _mapper.Map<List<TalkDto>>(talks);

            return Ok(talkDtos);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TalkDto>> Get(int id)
        {
            var talk = await _repository.GetByIdAsync<Talk>(id);

            if (talk == null)
            {
                _logger.LogInformation("Talk with id {Id} does not exist", id);
                return NotFound();
            }

            return Ok(_mapper.Map<TalkDto>(talk));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<TalkDto>> Post([FromBody] TalkForUpdateDto talk)
        {
            _logger.LogInformation("Creating new talk: {Talk}", talk);

            var talkEntity = _mapper.Map<Talk>(talk);

            var result = await _repository.AddAsync(talkEntity);

            _logger.LogInformation("Talk created with id {Id}", result.Id);

            return CreatedAtAction(
                nameof(Get),
                new { id = result.Id },
                _mapper.Map<TalkDto>(result));
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Put(int id,[FromBody] TalkForUpdateDto talk)
        {
            _logger.LogInformation("Updating talk {Id} with data {Talk}", id, talk);

            var talkEntity = await _repository.GetByIdAsync<Talk>(id);
            if (talkEntity  == null)
            {
                _logger.LogInformation("Talk with id {Id} does not exist", id);
                return NotFound();
            }

            

            talkEntity.Title = talk.Title;
            talkEntity.Description = talk.Description;
            talkEntity.ScheduledDateTime = talk.ScheduledDateTime;

            await _repository.UpdateAsync(talkEntity);

            _logger.LogInformation("Talk with id {Id} successfully updated", id);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Patch(int id,
            [FromBody]JsonPatchDocument<TalkForUpdateDto> patchDocument)
        {
            _logger.LogInformation("Updating talk with id {Id}", id);

            var talkEntity = await _repository.GetByIdAsync<Talk>(id);

            if (talkEntity == null)
            {
                _logger.LogInformation("Talk with id {Id} does not exist", id);
                return NotFound();
            }

            var talkToPatch = _mapper.Map<TalkForUpdateDto>(talkEntity);

            patchDocument.ApplyTo(talkToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            talkEntity.Title = talkToPatch.Title;
            talkEntity.Description = talkToPatch.Description;
            talkEntity.ScheduledDateTime = talkToPatch.ScheduledDateTime;

            await _repository.UpdateAsync(talkEntity);

            _logger.LogInformation("Talk with id {Id} was successfully updated", id);

            return NoContent();
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting talk with id {Id}", id);

            var talkEntity = await _repository.GetByIdAsync<Talk>(id);

            if (talkEntity == null)
            {
                _logger.LogInformation("Talk with id {Id} does not exist", id);
                return NotFound();
            }

            await _repository.DeleteAsync(talkEntity);

            _logger.LogInformation("Talk with id {0} successfully deleted", id);

            return NoContent();
        }
    }
}
