using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UploadFileDemo.Core.Models;
using UploadFileDemo.Persistence;
using UploadFileDemo.Shared;

namespace UploadFileDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _hosting;
        private readonly IOptionsSnapshot<PhotoSettings> _photoSettings;
        public PhotosController(AppDbContext context, IHostingEnvironment hosting, IOptionsSnapshot<PhotoSettings> photoSettings)
        {
            _hosting = hosting;
            _context = context;
            _photoSettings = photoSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetPhotos()
        {
            var photos = await _context.Photos.ToListAsync();

            return Ok(photos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhotos(int id)
        {
            var file = await _context.Photos.FindAsync(id);

            if (file is null)
                return BadRequest("Invalid ID.");

            var uploadsFolderPath = Path.Combine(_hosting.ContentRootPath, "contents", "uploads");

            var stream = System.IO.File.OpenRead(Path.Combine(uploadsFolderPath, file.FileName));

            var photo = File(stream, $"image/{Path.GetExtension(file.FileName).Replace(".", string.Empty)}");

            return photo;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var uploadsFolderPath = Path.Combine(_hosting.ContentRootPath, "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            if (_photoSettings.Value.IsFileExtensionInvalid(file.FileName))
                return BadRequest("File type is not supported.");

            if (_photoSettings.Value.IsMaxSizeExceed(file.Length))
                return BadRequest("Photo size must not exceed to 10 mb.");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo
            {
                FileName = fileName
            };

            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("", photo);
        }
    }
}