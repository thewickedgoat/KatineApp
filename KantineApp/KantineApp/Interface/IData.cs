using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KantineApp.Interface
{
    public interface IData<T>
    {
        void Create();
        T Read(int id);
        List<T> ReadAll();
        T Update(T t);
        void Delete(int id);
    }
}
