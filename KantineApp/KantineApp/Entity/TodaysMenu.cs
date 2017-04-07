using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantineApp.Entity
{
    public class TodaysMenu
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }
}
