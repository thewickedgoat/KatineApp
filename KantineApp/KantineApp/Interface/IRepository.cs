using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KantineApp.Entity;

namespace KantineApp.Interface
{
    public interface IRepository
    {
        /// <summary>
        /// Create a menu with an id, date and a list of dishes whith properties; name and image path.
        /// </summary>
        /// <param name="menu"></param>
        void Create(MenuEntity menu);

        /// <summary>
        /// Get a menu by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MenuEntity Read(int id);

        /// <summary>
        /// Get all menus.
        /// </summary>
        /// <returns></returns>
        List<MenuEntity> ReadAll();
        bool Update(MenuEntity menu);

        /// <summary>
        /// Delete menu by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
    }
}
