using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.DTO;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Route("api/Trails")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {

        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepository,
                                       IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(List<TrailDTO>))]
        [ProducesDefaultResponseType]
        public IActionResult GetTrails()
        {
            var objList = _trailRepository.GetTrails();

            var objDTO = new List<TrailDTO>();

            foreach (var item in objList)
            {
                objDTO.Add(_mapper.Map<TrailDTO>(item));
            }

            return Ok(objDTO);
        }

        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="trailId">The Id of trail</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepository.GetTrail(trailId);

            if (obj == null)
            {
                return NotFound();
            }

            var objDTO = _mapper.Map<TrailDTO>(obj);

            return Ok(objDTO);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailDTO trailDTO)
        {
            if (trailDTO == null) 
            {
                return BadRequest(ModelState);
            }

            if (_trailRepository.TrailExists(trailDTO.Name))
            {
                ModelState.AddModelError("", "Trail exists!");
                return StatusCode(404, ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDTO);

            if (!_trailRepository.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
        }

        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]       
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailDTO trailDTO)
        {
            if(trailDTO == null || trailId != trailDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDTO);
            if (!_trailRepository.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong while updating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId))
            {
                return NotFound();
            }            

            var trailObj = _trailRepository.GetTrail(trailId);
            if (!_trailRepository.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
