using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EventsApi.Core.Entities;
using EventsApi.Core.Interfaces;
using EventsApi.Web.Models.Talks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EventsApi.Web.Controllers
{
    [ApiController]
    [Route("/api/talks")]
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
            var talks = await _repository.ListAsync<Talk>();
            var talkDtos = _mapper.Map<List<TalkDto>>(talks);
            return Ok(talkDtos);
        }
    }
}
