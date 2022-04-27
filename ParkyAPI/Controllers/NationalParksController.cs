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
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNationalPark")]
    public class NationalParksController : ControllerBase
    {
        private INationalParkRepository _nPRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nPRepository = nationalParkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<NationalParkDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetNationalParks()
        {
            var objList = _nPRepository.GetNationalPark();
            var objDTO = new List<NationalParkDto>();

            foreach (var obj in objList)
            {
                objDTO.Add(_mapper.Map<NationalParkDto>(obj));
                var message = HttpContext.Request.PathBase;
            }
            return Ok(objDTO);
        }

        [HttpGet("{nationalParkId:Int}", Name = "National")]
        //[HttpGet]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var objNP = _nPRepository.GetNationalPark(nationalParkId);
            var objDTO = new NationalParkDto();
            if (objNP != null)
            {
                objDTO = _mapper.Map<NationalParkDto>(objNP);
            }
            else
            {
                return NotFound();
            }
            return Ok(objDTO);
        }

        [HttpPost()]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            NationalPark nationalPark = new NationalPark();
            if (nationalParkDto != null)
            {
                nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
                _nPRepository.CreateNationalPark(nationalPark);
            }
            //return Ok();
            return CreatedAtRoute("National", new { nationalParkID = nationalPark.ID }, nationalPark);
        }

        [HttpPatch("{nationalParkId:Int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            NationalPark nationalPark = new NationalPark();
            if (nationalParkDto != null && _nPRepository.NationalParkExists(nationalParkId))
            {
                nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
                _nPRepository.UpdateNationalPark(nationalPark);
            }
            return Ok();
        }

        [HttpDelete("{nationalParkId:Int}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (_nPRepository.NationalParkExists(nationalParkId))
            {
                var objNP = _nPRepository.GetNationalPark(nationalParkId);
                _nPRepository.DeleteNationalPark(objNP);
                return Ok();
            }
            return NoContent();
        }
    }
}
