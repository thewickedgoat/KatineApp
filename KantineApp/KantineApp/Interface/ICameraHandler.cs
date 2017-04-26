using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantineApp.Interface
{
    public delegate void PhotoTaken(string s);
    public interface ICameraHandler
    {
        void TakePhoto();

        void AddPhotoTakenEventHandler(PhotoTaken pt);
         
    }
}
