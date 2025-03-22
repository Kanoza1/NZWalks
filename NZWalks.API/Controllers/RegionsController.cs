using AutoMapper;

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
        public async Task <IActionResult> GetAll()
        {
            #region Old Mapping
            // Get data from database 
            //var regionsDomain = await dbContext.Regions.ToListAsync();
            //var regionsDomain = await regionRepository.GetAllAsync();

            // map Domain Models to DTOs
            //var regionsDto = new List<RegionDTO>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDTO()
            //    {
            //            Id = regionDomain.Id,
            //            Code = regionDomain.Code,
            //            Name = regionDomain.Name,
            //            RegionImageUrl = regionDomain.RegionImageUrl

            //    });
            //}
            // Return DTOs 
            //var regions = dbContext.Regions.ToList();
            //return Ok(regionsDto);
            #endregion

            // using automapper
            var regionsDomain = await regionRepository.GetAllAsync();
            // Map Domain Model to DTO
            //var regionsDto = mapper.Map<List<RegionDTO>>(regionsDomain);
            // return DTO back to client
            return Ok(mapper.Map<List<RegionDTO>>(regionsDomain));
        }

        [HttpGet("{id:guid}")]
        public async Task <IActionResult> GetById ([FromRoute]Guid id)
        {
            #region Old Mapping 
            // Get region Domain Model from Database 

            // Make function await 

            //var regionDomain =  await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            // Map/Convert Region Domain Model to Region DTO 

            //var regionDto = new RegionDTO
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};
            // return Dto back to client 
            //return Ok(regionDto);
            #endregion

            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null )
            {
                return NotFound();
            }


            // using automapper to map Domain Model to DTO
            return Ok(mapper.Map<RegionDTO>(regionDomain));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            #region Old Mapping
            // Map / Convert DTO to Domain Model
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name = addRegionRequestDto.Name,
            //    RegionImageUrl = addRegionRequestDto.RegionImageUrl
            //};
            // Use Domain Model to create Region
            //regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);
            // make it await


            //await dbContext.Regions.AddAsync(regionDomainModel);
            //await dbContext.SaveChangesAsync();

            // Map Domain Model back to DTO 
            //var regionDto = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            //return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

            #endregion
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
        public async Task <IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            #region Old Mapping
            // Get region from database // Check if region exists
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            // map Dto to Domain Model
            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionRequestDto.Code,
            //    Name = updateRegionRequestDto.Name,
            //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            //};
            // make it await
            //regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            // Check if region exists
            //if (regionDomainModel == null)
            //{
            //    return NotFound();
            //}
            // Update region // Map /Convert DTO to Domain Model
            //regionDomainModel.Code = updateRegionRequestDto.Code;
            //regionDomainModel.Name = updateRegionRequestDto.Name;
            //regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            // Save changes 
            //await dbContext.SaveChangesAsync();


            // Map Domain Model back to DTO 
            //var regionDto = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            //return Ok(regionDto);
            #endregion
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
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regionDto);

        }

        // Delete region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            #region Old Mapping
            // Get region from database // Check if region exists
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            //var regionDomainModel = await regionRepository.DeleteAsync(id);
            //if (regionDomainModel == null)
            //{
            //    return NotFound();
            //}
            //// Delete region
            ////dbContext.Regions.Remove(regionDomainModel);
            ////await dbContext.SaveChangesAsync();

            //// return deleted region back 
            //// map Domain Model to DTO
            //var regionDto = new RegionDTO
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            //return Ok(regionDto);
            #endregion
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
