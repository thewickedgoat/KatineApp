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
    public partial class MenuHistoryPage : ContentPage
    {
        readonly IRepository _repo = Factory.GetRepository;
        List<MenuEntity> _menus = new List<MenuEntity>();

        public MenuHistoryPage()
        {
            InitializeComponent();
            AddMenuStacksToPage();
        }


        /// <summary>
        /// Add menu stacklayouts to the wrapper stacklayout.
        /// </summary>
        public void AddMenuStacksToPage()
        {
            foreach (var menuStack in CreateMenuStack())
            {
                MenuHistoryWrapper.Children.Add(menuStack);
            }
        }

        /// <summary>
        /// Create a list of menuStack's, one for each menu, with the date on a label; 
        /// In every menuStack, create a list of dishStack's ; one for each dish in that menu.
        /// </summary>
        /// <returns></returns>
        public List<StackLayout> CreateMenuStack()
        {
            _menus = _repo.ReadAll();
            var menusSortedByDate = _menus.OrderByDescending(x => x.Date.Date).ThenByDescending(x => x.Date.Year);

            var menuStacks = new List<StackLayout>();
            foreach (var menu in menusSortedByDate)
            {
                var menuStack = new StackLayout();
                menuStack.Children.Add(new Label()
                {
                    Text = menu.Date.ToString(),
                    HeightRequest = 60,
                    BackgroundColor = Color.Gray,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 20,
                    VerticalTextAlignment = TextAlignment.Center
                });

                var dishStack = new StackLayout();
                foreach (var dish in menu.Dishes)
                {
                    dishStack.Children.Add(new Label() { Text = dish.Name });
                }
                menuStack.Children.Add(dishStack);
                menuStacks.Add(menuStack);
            }
            return menuStacks;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MenuHistoryWrapper.Children.Clear();
            AddMenuStacksToPage();
        }

    }
}
