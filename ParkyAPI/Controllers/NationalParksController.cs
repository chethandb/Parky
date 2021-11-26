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
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {

        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository nationalParkRepository,
                                       IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var objList = _nationalParkRepository.GetNationalParks();

            var objDTO = new List<NationalParkDTO>();

            foreach (var item in objList)
            {
                objDTO.Add(_mapper.Map<NationalParkDTO>(item));
            }

            return Ok(objDTO);
        }

        [HttpGet("{nationalParkId:int}")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _nationalParkRepository.GetNationalPark(nationalParkId);

            if (obj == null)
            {
                return NotFound();
            }

            var objDTO = _mapper.Map<NationalParkDTO>(obj);

            return Ok(objDTO);
        }


        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDTO nationalParkDTO)
        {
            if (nationalParkDTO == null) 
            {
                return BadRequest(ModelState);
            }

            if (_nationalParkRepository.NationalParkExists(nationalParkDTO.Name))
            {
                ModelState.AddModelError("", "National Park is already present");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDTO);

            if (!_nationalParkRepository.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}
