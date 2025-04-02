namespace NZWalks.API.Repositories;

public class SqlWalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext dbContext;
    public SqlWalkRepository(NZWalksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public async Task<Walk> CreateAsync(Walk walk)
    {
        await dbContext.Walks.AddAsync(walk);
        await dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null , bool isAscending = true , int pageNumber = 1 , int pageSize = 1000)
    {
        // using navigation property
        //return await dbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();

        var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

        // Filtering
        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            if(filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(x => x.Name.Contains(filterQuery));
            }
        }
        // Sorting
        //if (!string.IsNullOrWhiteSpace(filterQuery) && !string.IsNullOrWhiteSpace(sortBy))
        //{
        //    if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
        //    {
        //        walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
        //    }
        //}

        // Sorting 
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
            }
            else if (sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
            }
        }

        // Pagination
        return await walks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }
    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        // using navigation property
        return await dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingWalk == null)
        {
            return null;
        }
        existingWalk.Name = walk.Name;
        existingWalk.Description = walk.Description;
        existingWalk.LengthInKm = walk.LengthInKm;
        existingWalk.WalkImageUrl = walk.WalkImageUrl;
        existingWalk.RegionId = walk.RegionId;
        existingWalk.DifficultyId = walk.DifficultyId;
        await dbContext.SaveChangesAsync();
        return existingWalk;
    }
    public async Task<Walk?> DeleteAsync(Guid id)
    {
        var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingWalk == null)
        {
            return null;
        }
        dbContext.Walks.Remove(existingWalk);
        await dbContext.SaveChangesAsync();
        return existingWalk;
    }

}
