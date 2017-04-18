using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KantineApp.Entity;
using KantineApp.Interface;
using Xamarin.Forms;

namespace KantineApp.Pages
{
    public partial class NewMenuPage : ContentPage
    {
        readonly IRepository _repo = Factory.GetRepository;
        readonly List<Entry> _dishes = new List<Entry>();

        public NewMenuPage()
        {
            InitializeComponent();
            DishWrapperStack.Children.Add(NewDishStack());
        }

        /// <summary>
        /// Create a new Dish stacklayout, with text-entry and buttons for image options.
        /// </summary>
        /// <returns></returns>
        public StackLayout NewDishStack()
        {
            var dishStack = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
            var dish = new Entry() { Placeholder = "Rettens navn", HorizontalOptions = LayoutOptions.FillAndExpand };
            dishStack.Children.Add(dish);
            _dishes.Add(dish);
            dishStack.Children.Add(new Button() { Image = "camera.png" });
            dishStack.Children.Add(new Button() { Image = "file.png" });

            return dishStack;
        }

        /// <summary>
        /// When add new dish is clicked, add new Dish stacklayout to parent.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDishStak(object sender, EventArgs e)
        {
            DishWrapperStack.Children.Add(NewDishStack());
        }

        /// <summary>        
        /// create a new menu with a list of dishes and a given date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewMenu_Button_OnClicked(object sender, EventArgs e)
        {
            var menu = new MenuEntity()
            {
                Date = Picker.Date,
                Dishes = new List<Dish>()
            };
            foreach (var dish in _dishes)
            {
                menu.Dishes.Add(new Dish()
                {
                    Name = dish.Text
                });
            }
            _repo.Create(menu);
            DishWrapperStack.Children.Clear();
            _dishes.Clear();
            DishWrapperStack.Children.Add(NewDishStack());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DishWrapperStack.Children.Clear();
            _dishes.Clear();
            DishWrapperStack.Children.Add(NewDishStack());
        }

    }
}
