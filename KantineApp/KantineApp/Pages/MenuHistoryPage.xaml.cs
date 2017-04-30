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
                var menuStack = new StackLayout() { Spacing = 0 };
                menuStack.Children.Add(new Label()
                {
                    Text = string.Format(menu.Date.Day + "/" + menu.Date.Month + " - " + menu.Date.Year),
                    HeightRequest = 60,
                    BackgroundColor = Color.Gray,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 20,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                });

                /* The dishstack contains a list of dishes.*/
                var dishStack = new StackLayout() { Orientation = StackOrientation.Vertical };
                foreach (var dish in menu.Dishes)
                {
                    /* The dishwrapper wraps a dish with it's image horizontal in the dishstack.*/
                    var dishWrapper = new StackLayout() { Orientation = StackOrientation.Horizontal };
                    dishWrapper.Children.Add(new Label()
                    {
                        Text = dish.Name,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalTextAlignment = TextAlignment.Center
                    });

                    if (!string.IsNullOrEmpty(dish.Image))
                    {

                        var imgurl = dish.Image.Insert(dish.Image.IndexOf("/upload/") + 8, "c_scale,h_257,w_325/");
                        dishWrapper.Children.Add(new Image()
                        {
                            Source = ImageSource.FromUri(new Uri(imgurl)),
                            HeightRequest = 80,
                            HorizontalOptions = LayoutOptions.End
                        });
                    }
                    dishStack.Children.Add(dishWrapper);
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
