using System.Collections.Generic;
using KantineApp.Entity;

namespace KantineApp.Interface
{
    public interface IData
    {
        void Create(MenuEntity item);
        MenuEntity Read(int id);
        List<MenuEntity> ReadAll();
        bool Update(MenuEntity item);
        bool Delete(int id);
    }
}
