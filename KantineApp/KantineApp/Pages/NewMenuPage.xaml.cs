using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KantineApp.Entity;
using KantineApp.Interface;
using Xamarin.Forms;
using System.Diagnostics;

namespace KantineApp.Pages
{
    public partial class NewMenuPage : ContentPage
    {
        private IServiceGateway _serviceGateway = Factory.GetServiceGateway;
        ICameraHandler _cameraHandler;
        private MenuEntity menuToCreate;
        private Button takePhotoBtn = new Button();
        private Button pickPhotoBtn = new Button();
        private Button removeDishBtn = new Button();
        private Dish _tmpDish = null;

        public NewMenuPage()
        {
            InitializeComponent();
            menuToCreate = new MenuEntity();
            menuToCreate.Dishes = new List<Dish>();
            _cameraHandler = DependencyService.Get<ICameraHandler>();
            _cameraHandler.AddPhotoTakenEventHandler(PhotoReceived); // Callback
            DishWrapperStack.Children.Add(NewDishStack(new Dish()));
        }

        private void GallerySubscriber(Dish dish)
        {
            MessagingCenter.Subscribe<string>(this, "ChosenImage", (chosenImgUrl) =>
            {
                MessagingCenter.Unsubscribe<string>(this, "ChosenImage");
                dish.Image = chosenImgUrl;
            });
        }

        private async void NavigateToPhotoPicker(Dish dish)
        {
            await Navigation.PushModalAsync(new GalleryPage());
            GallerySubscriber(dish);
        }

        private void TakeNewPhoto(Dish dish)
        {
            if (_tmpDish != null)
                Debug.WriteLine("one try");
            _tmpDish = dish;
            _cameraHandler.TakePhoto();
        }

        public void PhotoReceived(string fileName)
        {
            _tmpDish.Image = fileName;
            if (_tmpDish != null)
            {
                _tmpDish = null;
            }
            Debug.WriteLine(fileName);
        }

        /// <summary>
        /// Create a new Dish stacklayout, with text-entry and buttons for image options.
        /// </summary>
        /// <returns></returns>
        public StackLayout NewDishStack(Dish dish)
        {
            takePhotoBtn = new Button()
            { BackgroundColor = Color.FromHex("#313030"), WidthRequest = 50, Image = "camera.png" };

            pickPhotoBtn = new Button()
            { BackgroundColor = Color.FromHex("#313030"), WidthRequest = 50, Image = "file.png" };

            removeDishBtn = new Button()
            { BackgroundColor = Color.FromHex("#313030"), WidthRequest = 50, Image = "remove.png" };

            var dishStack = new StackLayout()
            { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };

            var dishName = new Entry()
            { Placeholder = "Rettens navn", HorizontalOptions = LayoutOptions.FillAndExpand };

            dishStack.Children.Add(dishName);
            dish.Name = dishName.Text;

            dishStack.Children.Add(takePhotoBtn);
            dishStack.Children.Add(pickPhotoBtn);
            dishStack.Children.Add(removeDishBtn);

            pickPhotoBtn.Clicked += (sender, args) => { NavigateToPhotoPicker(dish); };
            takePhotoBtn.Clicked += (sender, args) => { TakeNewPhoto(dish); };
            removeDishBtn.Clicked += (sender, args) => { RemoveDish(dish, dishStack); };
            dishName.TextChanged += (sender, args) => { SaveDishName(dish, dishName.Text); };

            menuToCreate.Dishes.Add(dish);
            return dishStack;
        }
        private void RemoveDish(Dish dish, StackLayout dishStack)
        {
            DishWrapperStack.Children.Remove(dishStack);
            menuToCreate.Dishes.Remove(dish);
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
        private void CreateNewMenu_Button_OnClicked(object sender, EventArgs e)
        {
            menuToCreate.Date = dPicker.Date;
            _serviceGateway.Create(menuToCreate);
            DishWrapperStack.Children.Clear();
            menuToCreate = new MenuEntity();
            menuToCreate.Dishes = new List<Dish>();
            DishWrapperStack.Children.Add(NewDishStack(new Dish()));
        }

        protected override void OnAppearing()
        {
            _cameraHandler.AddPhotoTakenEventHandler(PhotoReceived); // Callback
        }
    }
}
