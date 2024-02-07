using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace Vidart.Service.Impl
{
    public class VidartServiceImpl : IVidartService
    {
        private readonly IMapper mapper;

        public VidartServiceImpl(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<bool> trimVideo(int startTime, int endTime)
        {
            try
            {
                string _dir = $"D:{Path.DirectorySeparatorChar}QSI Doc{Path.DirectorySeparatorChar}ffmpeg-5.1.2-essentials_build{Path.DirectorySeparatorChar}bin";
                var input = Path.Combine(_dir, "file.mp4");
                var output = Path.Combine(_dir, "converted.mp4");
                FFmpeg.SetExecutablesPath(_dir, ffmpegExeutableName: "FFmpeg");

                var startSpan = TimeSpan.FromSeconds(startTime);
                var endSpan = TimeSpan.FromSeconds(endTime);
                var duration = endSpan - startSpan;

                var info = await FFmpeg.GetMediaInfo(input);

                var videoStream = info.VideoStreams.First()
                .SetCodec(VideoCodec.h264)
                .SetSize(VideoSize.Hd480)
                .Split(startSpan, duration);

                await FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .SetOutput(output)
                .Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }

        public async Task<bool> trimVideoToAudio(int startTime, int endTime)
        {
            try
            {
                string _dir = $"D:{Path.DirectorySeparatorChar}QSI Doc{Path.DirectorySeparatorChar}ffmpeg-5.1.2-essentials_build{Path.DirectorySeparatorChar}bin";
                var input = Path.Combine(_dir, "file.mp4");
                var output = Path.Combine(_dir, "converted.mp4");
                var outputAudio = Path.Combine(_dir, "convertedAudio.aac");
                FFmpeg.SetExecutablesPath(_dir, ffmpegExeutableName: "FFmpeg");

                var startSpan = TimeSpan.FromSeconds(startTime);
                var endSpan = TimeSpan.FromSeconds(endTime);
                var duration = endSpan - startSpan;

                var info = await FFmpeg.GetMediaInfo(input);
                //IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(input.MkvWithAudio);

                var videoStream = info.VideoStreams.First()
                .SetCodec(VideoCodec.h264)
                .SetSize(VideoSize.Hd480)
                .Split(startSpan, duration);

                var audioStream = info.AudioStreams.First()
                    .SetCodec(AudioCodec.aac);

                await FFmpeg.Conversions.New()
                .AddStream(audioStream)
                .SetOutput(outputAudio)
                .Start();

                await FFmpeg.Conversions.New()
                .AddStream(videoStream)
                .SetOutput(output)
                .Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return true;
        }
    }
}
