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
                var stack = new StackLayout()
                { Orientation = StackOrientation.Horizontal, BackgroundColor = Color.FromHex("#313030"), };

                var editBtn = new Button()
                { TextColor = Color.FromHex("#ededed"), BackgroundColor = Color.FromHex("#313030"), WidthRequest = 50, Image = "pencil.png" };

                var deleteBtn = new Button()
                { TextColor = Color.FromHex("#ededed"), BackgroundColor = Color.FromHex("#313030"), WidthRequest = 50, Image = "delete.png" };

                deleteBtn.Clicked += (sender, EventArgs) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var answer = await DisplayAlert("Slet menu", "Er du sikker på du vil slette denne menu?", "Ja", "Nej");
                        if (answer)
                        {
                            // Await is called in Delete()!
                            _serviceGatway.Delete(menu.Id);
                            OnAppearing();
                        }
                    });
                };

                editBtn.Clicked += (sender, EventArgs) =>
                {
                    NavigateToEditMenuPage(menu);
                };

                var dateLbl = new Label()
                {
                    Text = string.Format(menu.Date.Day + "/" + menu.Date.Month + " - " + menu.Date.Year),
                    HeightRequest = 45,
                    BackgroundColor = Color.FromHex("#313030"),
                    TextColor = Color.FromHex("#ededed"),
                    FontSize = 18,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.StartAndExpand
                };

                stack.Children.Add(dateLbl);
                stack.Children.Add(editBtn);
                stack.Children.Add(deleteBtn);
                menuStack.Children.Add(stack);

                /* The dishstack contains a list of dishes.*/
                var dishStack = new StackLayout() { Orientation = StackOrientation.Vertical };
                foreach (var dish in menu.Dishes)
                {
                    var horizontalLine = new BoxView()
                    { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 1, Color = Color.FromHex("#222") };

                    /* The dishwrapper wraps a dish with it's image horizontal in the dishstack.*/
                    var dishWrapper = new StackLayout() { Orientation = StackOrientation.Horizontal };

                    dishWrapper.Children.Add(new Label()
                    { Text = dish.Name, HorizontalOptions = LayoutOptions.StartAndExpand, VerticalTextAlignment = TextAlignment.Center });

                    if (!string.IsNullOrEmpty(dish.Image))
                    {
                        var imgurl = dish.Image.Insert(dish.Image.IndexOf("/upload/") + 8, "c_scale,h_257,w_325/");
                        dishWrapper.Children.Add(new Image()
                        { Source = ImageSource.FromUri(new Uri(imgurl)), HeightRequest = 80, HorizontalOptions = LayoutOptions.End });
                    }
                    dishStack.Children.Add(dishWrapper);
                    dishStack.Children.Add(horizontalLine);
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

        private async void NavigateToEditMenuPage(MenuEntity menu)
        {
            await Navigation.PushModalAsync(new EditMenuPage(menu));
        }
    }
}
