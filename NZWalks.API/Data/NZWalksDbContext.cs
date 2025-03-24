namespace NZWalks.API.Data;

public class NZWalksDbContext : DbContext
{
    public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {

    }

    public DbSet<Difficulty> Difficulties { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Walk> Walks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data for Difficulties 
        // Easy, Medium, Hard

        var difficulties = new List<Difficulty>
        {
            new Difficulty
            {
                Id = Guid.Parse("8b7d211b-5c41-4b96-9027-ee52804d6229"),
                Name = "Easy"
            },
            new Difficulty
            {
                Id = Guid.Parse("ff743fac-e3fa-4702-a99f-c8c7350a8dbd"),
                Name = "Medium"
            },
            new Difficulty
            {
                Id = Guid.Parse("094baaf9-a45f-432b-934e-9979c1646a42"),
                Name = "Hard"
            }
        };
        //  seed difficulties to the database 
        modelBuilder.Entity<Difficulty>().HasData(difficulties);

        // Seed data for Regions

        var regions = new List<Region>
        {
            new Region
            {
                Id = Guid.Parse("98bd81ee-e61f-475c-8dda-9715a8503d27"),
                Code = "A",
                Name = "Ahmed",
                RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Region
            {
                Id = Guid.Parse("212cfad5-6d23-4c05-ba93-85d6158f9aa1"),
                Code = "B",
                Name = "Bassem",
                RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Region
            {
                Id = Guid.Parse("aa55bab7-02c5-43da-b393-677476bf6b6e"),
                Code = "D",
                Name = "Diaa",
                RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            },
            new Region
            {
                Id = Guid.Parse("90fdaac3-074f-4442-8cd8-bd8d9322ba99"),
                Code = "E",
                Name = "Ehab",
                RegionImageUrl = null
            },


        };
        // seed regions to the database
        modelBuilder.Entity<Region>().HasData(regions);
    }
}
