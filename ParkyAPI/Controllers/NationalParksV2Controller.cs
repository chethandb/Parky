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
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2 : Controller
    {

        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParksV2(INationalParkRepository nationalParkRepository,
                                       IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(List<NationalParkDTO>))]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalParks()
        {           

            var obj = _nationalParkRepository.GetNationalParks()?.FirstOrDefault();

            // changes to differentiate api versions
            //var objList = _nationalParkRepository.GetNationalParks();
            //var objDTO = new List<NationalParkDTO>();
            //foreach (var item in objList)
            //{
            //    objDTO.Add(_mapper.Map<NationalParkDTO>(item));
            //}

            return Ok(_mapper.Map<NationalParkDTO>(obj));
        }
       
    }
}
