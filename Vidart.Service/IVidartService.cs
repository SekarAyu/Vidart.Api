using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vidart.Service
{
    public interface IVidartService
    {
        Task<bool> trimVideo(int startTime, int endTime);
        Task<bool> trimVideoToAudio(int startTime, int endTime);
    }
}
