using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using System.Net;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IMapper mapper , IWalkRepository walkRepository )
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }


        // Create Walk 
        // Post: /api/walks
        [HttpPost("")]
        [ValidateModel] 
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
            // Create Walk
            await walkRepository.CreateAsync(walkDomainModel);
            // Map Domain Model to DTO

            //return Ok(mapper.Map<WalkDto>(walkDomainModel));
            // return CreatedAtAction(nameof(GetById), new { id = walkDomainModel.Id }, walkDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = walkDomainModel.Id }, mapper.Map<WalkDto>(walkDomainModel));



        }

        // Get Walks
        // Get: /api/walks
        // Get: /api/walks?filterOn=Name & filterQuery=TrackssodtBy & sortBy=NameisAscending=true
        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending , [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000 )
        {
                // Get all walks from database
                var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            // create an exception
            throw new Exception("This is a custom exception");
            // Map Domain Model to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
            
        }

        // Get Walk by Id
        // Get: /api/walks/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // Update Walk
        // Put: /api/walks/{id}
        [HttpPut("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            
        }

        // Delete Walk
        // Delete: /api/walks/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
