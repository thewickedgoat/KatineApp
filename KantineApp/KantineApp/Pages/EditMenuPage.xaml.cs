using KantineApp.Entity;
using KantineApp.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KantineApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditMenuPage : ContentPage
    {
        //readonly IRepository _repo = Factory.GetRepository;
        private IServiceGateway _serviceGateway = Factory.GetServiceGateway;
        private MenuEntity menuToUpdate;
        private Button takePhotoBtn = new Button();
        private Button pickPhotoBtn = new Button();
        private Button removeDishBtn = new Button();
        private Dish _tmpDish = null;
        ICameraHandler _cameraHandler;

        public EditMenuPage(MenuEntity menu)
        {
            InitializeComponent();
            menuToUpdate = menu;
            _cameraHandler = DependencyService.Get<ICameraHandler>();
            _cameraHandler.AddPhotoTakenEventHandler(PhotoReceived); // Callback

            var dishes = new List<StackLayout>();
            foreach (var dish in menuToUpdate.Dishes)
            {
                DishWrapperStack.Children.Add(NewDishStack(dish));
            }
            //DishWrapperStack.Children.Add(NewDishStack(new Dish()));
        }
        private void GallerySubscriber(Dish dish)
        {
            MessagingCenter.Subscribe<string>(this, "ChosenImage", (chosenImgUrl) =>
            {
                dish.Image = chosenImgUrl;
            });
        }

        private async void PickPhoto(Dish dish)
        {
            await Navigation.PushModalAsync(new GalleryPage());
            GallerySubscriber(dish);
        }

        private void TakePhoto(Dish dish)
        {
            if (_tmpDish != null)
                Debug.WriteLine("one try");
            _tmpDish = dish;
            _cameraHandler.TakePhoto();
        }

        public void PhotoReceived(string fileName)
        {
            _tmpDish.Image = fileName;
            _tmpDish = null;
            Debug.WriteLine(fileName);
        }


        /// <summary>
        /// Create a new Dish stacklayout, with text-entry and buttons for image options.
        /// </summary>
        /// <returns></returns>
        public StackLayout NewDishStack(Dish dish)
        {
            takePhotoBtn = new Button()
            {
                BackgroundColor = Color.FromHex("#313030"),
                WidthRequest = 50
            };
            pickPhotoBtn = new Button()
            {
                BackgroundColor = Color.FromHex("#313030"),
                WidthRequest = 50
            };
            removeDishBtn = new Button()
            {
                BackgroundColor = Color.FromHex("#313030"),
                WidthRequest = 50
            };

            var dishStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            var dishName = new Entry()
            {
                Placeholder = "Rettens navn",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            dishStack.Children.Add(dishName);

            if (dish.Name != null)
            {
                dishName.Text = dish.Name;
            }
            else
            {
                dish.Name = dishName.Text;
                menuToUpdate.Dishes.Add(dish);
            }

            takePhotoBtn.Image = "camera.png";
            pickPhotoBtn.Image = "file.png";
            removeDishBtn.Image = "remove.png";
            dishStack.Children.Add(takePhotoBtn);
            dishStack.Children.Add(pickPhotoBtn);
            dishStack.Children.Add(removeDishBtn);


            takePhotoBtn.Clicked += (sender, args) => { TakePhoto(dish); };
            removeDishBtn.Clicked += (sender, args) => { RemoveDish(dish, dishStack); };
            dishName.TextChanged += (sender, args) => { SaveDishName(dish, dishName.Text); };
            pickPhotoBtn.Clicked += (sender, args) => { PickPhoto(dish); };

            return dishStack;
        }
        private void RemoveDish(Dish dish, StackLayout dishStack)
        {
            DishWrapperStack.Children.Remove(dishStack);
            menuToUpdate.Dishes.Remove(dish);
        }

        private void SaveDishName(Dish dish, string dishName)
        {
            dish.Name = dishName;
        }

        /// <summary>
        /// When add new dish is clicked, add new Dish stacklayout to parent.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDishStak(object sender, EventArgs e)
        {
            DishWrapperStack.Children.Add(NewDishStack(new Dish()));
        }

        /// <summary>        
        /// create a new menu with a list of dishes and a given date.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CreateNewMenu_Button_OnClicked(object sender, EventArgs e)
        {

            var result = await _serviceGateway.Update(menuToUpdate);
            //DishWrapperStack.Children.Clear();
            //menuToUpdate = new MenuEntity();
            //menuToUpdate.Dishes = new List<Dish>();
            //DishWrapperStack.Children.Add(NewDishStack(new Dish()));
            if (result)
            {
                NavigateBack();
            }
        }

        public async void NavigateBack()
        {
            await Navigation.PopModalAsync();
        }

        /**protected override void OnAppearing()
        {
            Debug.WriteLine("On appering");
            base.OnAppearing();
            DishWrapperStack.Children.Clear();
            _dishes.Clear();
            DishWrapperStack.Children.Add(NewDishStack());
        }**/


    }
}
