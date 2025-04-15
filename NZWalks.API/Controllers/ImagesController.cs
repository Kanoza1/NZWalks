using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageRepository imageRepository;
    public ImagesController(IImageRepository imageRepository)
    {
        this.imageRepository = imageRepository;
    }

    // POST: api/Images/upload
    [HttpPost("Upload")]
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequestDto request)
    {
        ValidateFileUpload(request);
        if (ModelState.IsValid)
        {
            // convert DTO to domain model
            var imageDomainModel = new Image
            {
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.FileName,
                FileDescription = request.FileDescription,
            };



            // user repository to upload the image
             await imageRepository.Upload(imageDomainModel);
            return Ok(imageDomainModel);
        }
        return BadRequest(ModelState);
    }

    private void ValidateFileUpload (ImageUploadRequestDto request)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
        {
            ModelState.AddModelError("File", "unsupported file extension");
        }
        if (request.File.Length > 10 * 1024 * 1024)
        {
            ModelState.AddModelError("File", "File size exceeds 10MB, please upload smaller size file");
        }
    }
}
