using KantineApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantineApp.Interface
{
    public interface IServiceGateway
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
        Task<MenuEntity> Read(int id);

        /// <summary>
        /// Get all menus.
        /// </summary>
        /// <returns></returns>
        Task<List<MenuEntity>> ReadAll();


        void Update(MenuEntity menu);

        /// <summary>
        /// Delete menu by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(int id);

        Task<List<string>> GetAllImages();
    }
}
