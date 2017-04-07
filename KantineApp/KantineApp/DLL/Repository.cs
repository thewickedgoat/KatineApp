using System.Collections.Generic;
using System.Linq;
using KantineApp.Entity;
using KantineApp.Interface;
using Xamarin.Forms;

namespace KantineApp.DLL
{
    public class Repository : IData
    {
        private List<MenuEntity> _items = new List<MenuEntity>();
        public void Create(MenuEntity item)
        {
            _items.Add(item);
        }

        public MenuEntity Read(int id)
        {
            return SelectedItem(id);
        }

        public List<MenuEntity> ReadAll()
        {
            return _items;
        }

        public bool Update(MenuEntity item)
        {
            _items.Remove(SelectedItem(item.Id));
            _items.Add(item);
            return true;
        }

        public bool Delete(int id)
        {
            _items.Remove(SelectedItem(id));
            return true;
        }

        public MenuEntity SelectedItem(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

    }
}