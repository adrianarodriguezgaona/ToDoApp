using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace B4.EE.RodriguezA.Domain.Services
{
    public  interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
        Task<string> GetImagePathAsync();
    }

}
