namespace NZWalks.API.Repositories;

public interface IImageRepository
{
    Task<Image> Upload(Image image);
}
