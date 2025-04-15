using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using NZWalks.API.CustomActionFilters;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet("")]
        [Authorize(Roles = "Reader")]
        public async Task <IActionResult> GetAll()
        {
            // using automapper
            var regionsDomain = await regionRepository.GetAllAsync();
            // Map Domain Model to DTO
            //var regionsDto = mapper.Map<List<RegionDTO>>(regionsDomain);
            // return DTO back to client
            return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles ="Reader")]

        public async Task <IActionResult> GetById ([FromRoute]Guid id)
        {

            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null )
            {
                return NotFound();
            }


            // using automapper to map Domain Model to DTO
            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpPost("")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // using automapper
            // Map / Convert DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
            // Use Domain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
            // Map Domain Model back to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // Update region 
        // PUT: https://localhost:portnumber/api/regions/{id} 
        [HttpPut("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task <IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
                // using automapper
                // Get region from database // Check if region exists
                var regionDomainModel = await regionRepository.GetByIdAsync(id);
                // map Dto to Domain Model
                regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
                // make it await
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                // Check if region exists
                if (regionDomainModel == null)
                {
                    return NotFound();
                }
                // Map Domain Model back to DTO
                return Ok(mapper.Map<RegionDTO>(regionDomainModel));
        }

        // Delete region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete("{id:guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // using automapper
            // Get region from database // Check if region exists
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}
