using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KantineApp.Entity;
using KantineApp.Interface;

namespace KantineApp.DLL
{
    public class MockRepository : IRepository
    {
        private readonly List<MenuEntity> _menus = new List<MenuEntity>()
        #region DUMMY DATA
		 {
            new MenuEntity()
            {
                Id = 1, Date = DateTime.Now, Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                         Id = 1, Name= "Tarteletter", Image = ""
                    }, new Dish()
                    {
                        Id = 2, Name= "Stegt flæsk", Image = ""
                    },new Dish()
                    {
                        Id = 3, Name= "Råkost salat", Image = ""
                    },new Dish()
                    {
                        Id = 4, Name= "Klar suppe med boller", Image = ""
                    }
                }
            },
            new MenuEntity()
            {
                Id = 2, Date = DateTime.Now.AddDays(4), Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                         Id = 5, Name= "Tomat suppe", Image = ""
                    }, new Dish()
                    {
                        Id = 6, Name= "Stegt flæsk", Image = ""
                    },new Dish()
                    {
                        Id = 7, Name= "Salat", Image = ""
                    }
                }
            }
        };
        #endregion

        public void Create(MenuEntity menu)
        {
            _menus.Add(menu);
        }

        public MenuEntity Read(int id)
        {
            return _menus.FirstOrDefault(menu => menu.Id == id);
        }

        public List<MenuEntity> ReadAll()
        {
            return _menus;
        }

        public bool Update(MenuEntity menu)
        {
            _menus.Remove(Read(menu.Id));
            _menus.Add(menu);
            return true;
        }

        public bool Delete(int id)
        {
            _menus.Remove(Read(id));
            return true;
        }
    }
}
