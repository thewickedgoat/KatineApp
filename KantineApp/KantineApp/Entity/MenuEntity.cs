﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantineApp.Entity
{
    public class MenuEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}