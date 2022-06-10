
using Microsoft.AspNetCore.Mvc;
using SimpleApi.Data;
using SimpleApi.Models;

namespace SimpleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly ApplicationDBContext _ctx;

        public AdvertisementController(ApplicationDBContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost]
        public async Task<IActionResult> PostAd([FromForm] AdvertisementModel ad)
        {
            var mFiles = ad.Files.Select(GetImageEntity).ToList();

            var adEntity = new Advertisement()
            {
                Id = Guid.NewGuid(),
                Title = ad.Title,
                Description = ad.Description,
                Tags = string.Join(',', ad.Tags),
                Medias = mFiles
            };

            await _ctx.Advertisements.AddAsync(adEntity);
            await _ctx.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAds()
        {
            var ads = await _ctx.Advertisements
                .AsNoTracking()
                .Include(ad => ad.Medias)
                .Select(ad => new
                {
                    Id = ad.Id,
                    Title = ad.Title,
                    Description = ad.Description,
                    Tags = ad.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries),
                    Files = ad.Medias.Select(m => new
                    {
                        Id = m.Id,
                        ContentType = m.ContentType,
                        SizeInMb = m.SizeInMb
                    })
                }).ToListAsync();

            return Ok(ads);
        }

        [HttpGet]
        [Route("/media/{id}")]
        public async Task<IActionResult> GetMedia(Guid id)
        {
            var file = await _ctx.Medias.FirstOrDefaultAsync(m => m.Id == id);

            var stream = new MemoryStream(file.Data);

            return File(stream, file.ContentType);
        }

        private List<(Guid Id, string ContentType, double Size)> GetFiles(Advertisement ad)
            => ad.Medias.Select(m => (m.Id, m.ContentType, m.SizeInMb)).ToList();

        private Media GetImageEntity(IFormFile file)
        {
            using var stream = new MemoryStream();

            file.CopyTo(stream);

            return new Media()
            {
                Id = Guid.NewGuid(),
                ContentType = file.ContentType,
                Data = stream.ToArray()
            };
        }
    }
}
