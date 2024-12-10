using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using System.Collections.Generic;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles="Reader")]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();
            //var regionsDto = new List<RegionDto>();
            //foreach (var region in regions)
            //{
               // regionsDto.Add(new RegionDto()
                //{
                   // Id = region.Id,
                    //Name = region.Name,
                    //Code = region.Code,
                    //RegionImageUrl = region.RegionImageUrl

               // });
            //}
            var regionsDto = mapper.Map< List < RegionDto >>(regions);
                return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById(Guid id) {
                var region = await regionRepository.GetByIdAsync(id);
                if (region == null)
                {
                    return NotFound();
                }
            //var regionDto = new RegionDto
            //{
                //Id = region.Id,
                //Name = region.Name,
               // Code = region.Code,
                //RegionImageUrl = region.RegionImageUrl
            //};
            var regionDto = mapper.Map< RegionDto >(region);
                return Ok(regionDto);
            }


        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create(AddRegionRequestDto addRegionRequestDto)
        {
            
                var region = mapper.Map<Region>(addRegionRequestDto);
                // var region = new Region
                //{
                // Code = addRegionRequestDto.Code,
                // Name = addRegionRequestDto.Name,
                // RegionImageUrl= addRegionRequestDto.RegionImageUrl
                //};
                region = await regionRepository.CreateAsync(region);

                var regionDto = mapper.Map<RegionDto>(region);
                //var regionDto = new Region
                //{
                // Id = region.Id,
                //Name = region.Name,
                //Code = region.Code,
                // RegionImageUrl = region.RegionImageUrl
                //};

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            
        }

        [HttpPut]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update(Guid id, UpdateRegionRequestDto updateRegionRequestDto)
        {
            
                var region = mapper.Map<Region>(updateRegionRequestDto);
           // var region = new Region
            //{
               // Code = updateRegionRequestDto.Code,
                //Name = updateRegionRequestDto.Name,
                //RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            //};
            region = await regionRepository.UpdateAsync(id, region);
            if (region == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDto>(region) ;
           // var regionDto = new RegionDto
            //{
                //Id = region.Id,
                //Name = region.Name,
                //Code = region.Code,
                //RegionImageUrl = region.RegionImageUrl
            //};
            return Ok(regionDto);
         

        }
        [HttpDelete]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var region = await regionRepository.DeleteAsync(id);
            if (region == null)
            { return NotFound(); }
           var regionDto = mapper.Map<RegionDto>(region);
           // var regionDto = new RegionDto
            //{
               // Id = region.Id,
                //Name = region.Name,
                //Code = region.Code,
                //RegionImageUrl = region.RegionImageUrl
           // };

            return Ok(regionDto);
        }
    }
} 
