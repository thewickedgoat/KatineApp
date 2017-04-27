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
        //readonly IRepository _repo = Factory.GetRepository;
        private IServiceGateway _serviceGatway = Factory.GetServiceGateway;
        List<MenuEntity> _menus = new List<MenuEntity>();

        public MenuHistoryPage()
        {
            InitializeComponent();
            AddMenuStacksToPage();
        }


        /// <summary>
        /// Add menu stacklayouts to the wrapper stacklayout.
        /// </summary>
        public async void AddMenuStacksToPage()
        {
            var res = await CreateMenuStack();

            foreach (var menuStack in res)
            {
                MenuHistoryWrapper.Children.Add(menuStack);
            }

            /**var tasks = CreateMenuStack();

            foreach (var task in tasks.)
            {
                tasks.Add(new Task(() =>
                {
                    // do something with the item in this action
                }));
            }

            await Task.WhenAll(tasks);**/
        }

        /// <summary>
        /// Create a list of menuStack's, one for each menu, with the date on a label; 
        /// In every menuStack, create a list of dishStack's ; one for each dish in that menu.
        /// </summary>
        /// <returns></returns>
        public async Task<List<StackLayout>> CreateMenuStack()
        {
            _menus = await _serviceGatway.ReadAll();
            var menusSortedByDate = _menus.OrderByDescending(x => x.Date.Date).ThenByDescending(x => x.Date.Year);

            var menuStacks = new List<StackLayout>();
            foreach (var menu in menusSortedByDate)
            {
                var menuStack = new StackLayout();
                menuStack.Children.Add(new Label()
                {
                    Text = string.Format(menu.Date.Day + "/" + menu.Date.Month + " - " + menu.Date.Year),
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
