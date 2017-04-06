using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KantineApp.Interface
{
    public interface IData
    {
        void Create(MenuItem item);
        MenuItem Read(int id);
        List<MenuItem> ReadAll();
        MenuItem Update(MenuItem item);
        void Delete(int id);
    }
}
