using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KantineApp.DLL;
using KantineApp.Entity;
using KantineApp.Interface;

namespace KantineApp
{
    public class Factory
    {
        public static IData<MenuItem, int> GetRepository()
        {
            return new Repository();
        }
    }
}
