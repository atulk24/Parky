using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/Trails")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    public class TrailController : ControllerBase
    {
        private ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTrails()
        {
            var objList = _trailRepository.GetTrail();
            var objDTO = new List<TrailDto>();

            foreach (var obj in objList)
            {
                objDTO.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDTO);
        }

        [HttpGet("{trailId:Int}", Name = "GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var objNP = _trailRepository.GetTrail(trailId);
            var objDTO = new TrailDto();
            if (objNP != null)
            {
                objDTO = _mapper.Map<TrailDto>(objNP);
            }
            else
            {
                return NotFound();
            }
            return Ok(objDTO);
        }

        [HttpPost()]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            Trail trail = new Trail();
            if (trailDto != null)
            {
                trail = _mapper.Map<Trail>(trailDto);
                _trailRepository.CreateTrail(trail);
            }
            return CreatedAtRoute("GetTrail", new { trailId = trail.Id }, trail);
        }

        [HttpPatch("{trailId:Int}", Name = "UpdateTrail")]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpsertDto trailDto)
        {
            Trail trail = new Trail();
            if (trailDto != null && _trailRepository.TrailExists(trailId))
            {
                trail = _mapper.Map<Trail>(trailDto);
                _trailRepository.UpdateTrail(trail);
            }
            return Ok();
        }

        [HttpDelete("{trailId:Int}", Name = "DeleteTrail")]
        public IActionResult DeleteTrail(int trailId)
        {
            if (_trailRepository.TrailExists(trailId))
            {
                var objNP = _trailRepository.GetTrail(trailId);
                _trailRepository.DeleteTrail(objNP);
            }
            return NoContent();
        }

        [HttpGet("nationlParkID")]
        public IActionResult GetTrailInNationalPark(string nationlParkID)
        {
            var objList = _trailRepository.GetTrailsInNationalPark(Convert.ToInt32(nationlParkID));
            var objDTO = new List<TrailDto>();

            foreach (var obj in objList)
            {
                objDTO.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDTO);
        }

    }
}
