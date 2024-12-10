using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create(AddWalkRequestDto addWalkRequestDto)
        {
           
           
                var walk = mapper.Map<Walk>(addWalkRequestDto);
            
                await walkRepository.CreateAsync(walk);
            
                var walkDto = mapper.Map<WalkDto>(walk);
            
                return Ok(walkDto);
         
        }

        [HttpGet]
        public async Task<IActionResult> GetAll( [FromQuery] string? filterOn,[FromQuery] string? filterQuery
          ,[FromQuery] string? sortBy ,[FromQuery] bool? isAscending
          ,[FromQuery] int pageNumber=1 ,[FromQuery] int pageSize=1000)
        {
         var walks = await walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending??true,pageNumber,pageSize);
          return Ok(mapper.Map<List<WalkDto>>(walks));
        
       }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var walk = await walkRepository.GetByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walk));
        }

        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            
                var walk = mapper.Map<Walk>(updateWalkRequestDto);
                await walkRepository.UpdateAsync(id, walk);
                if (walk == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<WalkDto>(walk));
            
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedWalk = await walkRepository.DeleteAsync(id);
            if (deletedWalk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(deletedWalk));
        }
    }
}
