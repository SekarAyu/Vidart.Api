using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Vidart.Service;

namespace Vidart.Api.AspNetCore.Controller
{
    [Authorize]
    [ApiController]
    [AllowAnonymous]
    [Route("Vidart")]
    public class VidartController : ControllerBase
    {
        private readonly IVidartService vidartService;

        public VidartController(IVidartService vidartService)
        {
            this.vidartService = vidartService;
        }

        [HttpPost]
        [Route("trim/video")]
        [AllowAnonymous]
        public async Task<IActionResult> Trim(IFormFile file, [FromQuery] int startTime, [FromQuery] int endTime)
        {
            string _dir = $"D:{Path.DirectorySeparatorChar}QSI Doc{Path.DirectorySeparatorChar}ffmpeg-5.1.2-essentials_build{Path.DirectorySeparatorChar}bin";
            using (var fileStream = new FileStream(Path.Combine(_dir, "file.mp4"), FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream);
            }
            await vidartService.trimVideo(startTime, endTime);
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [Route("trim/video/toaudio")]
        [AllowAnonymous]
        public async Task<IActionResult> TrimToAudio(IFormFile file, [FromQuery] int startTime, [FromQuery] int endTime)
        {
            string _dir = $"D:{Path.DirectorySeparatorChar}QSI Doc{Path.DirectorySeparatorChar}ffmpeg-5.1.2-essentials_build{Path.DirectorySeparatorChar}bin";
            using (var fileStream = new FileStream(Path.Combine(_dir, "file.mp4"), FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream);
            }
            await vidartService.trimVideoToAudio(startTime, endTime);
            return RedirectToAction("Index");
        }
    }
}
