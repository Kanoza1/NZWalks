namespace NZWalks.API.Repositories;

public class LocalImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment webHostEnvironment;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly NZWalksDbContext dbContext;

    public LocalImageRepository(IWebHostEnvironment webHostEnvironment , IHttpContextAccessor httpContextAccessor , NZWalksDbContext dbContext)
    {
        this.webHostEnvironment = webHostEnvironment;
        this.httpContextAccessor = httpContextAccessor;
        this.dbContext = dbContext;
    }
    public async Task<Image> Upload(Image image)
    {
        var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",$"{image.FileName}{image.FileExtension}");
        //Upload the image to the local file system
        using var fileStream = new FileStream(localFilePath, FileMode.Create);
        await image.File.CopyToAsync(fileStream);

        // https://localhost:5000/Images/filename.extension
        var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

        image.FilePath = urlFilePath;

        // add image to the images table
        await dbContext.Images.AddAsync(image);
        await dbContext.SaveChangesAsync();
        return image;



    }
}
