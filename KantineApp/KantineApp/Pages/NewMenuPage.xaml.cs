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
        
        readonly IRepository _repo = Factory.GetRepository;
        private List<Dish> _dishes = new List<Dish>();

        private Button takePhotoBtn = new Button();
        private Button pickPhotoBtn = new Button();

        private Dish _tmpDish = null;

        public NewMenuPage()
        {
            InitializeComponent();
            DishWrapperStack.Children.Add(NewDishStack());
        }

        private void PickPhoto()
        {
            DisplayAlert("Info", "Pick a photo is NOT IMPLEMENTED YET!", "OK");
        }

        private void TakePhoto(Dish dish)
        {
            ICameraHandler _cameraHandler; _cameraHandler = DependencyService.Get<ICameraHandler>();
            _cameraHandler.AddPhotoTakenEventHandler(PhotoReceived);
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
            //newImage.Source = null;
            //newImage.Source = fileName;

        }

        /// <summary>
        /// Create a new Dish stacklayout, with text-entry and buttons for image options.
        /// </summary>
        /// <returns></returns>
        public StackLayout NewDishStack()
        {
            takePhotoBtn = new Button();
            pickPhotoBtn = new Button();
            Dish dish = new Dish();
            var dishStack = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
            var dishName = new Entry() { Placeholder = "Rettens navn", HorizontalOptions = LayoutOptions.FillAndExpand };
            dishStack.Children.Add(dishName);
            dish.Name = dishName.Text;
            _dishes.Add(dish);
            takePhotoBtn.Image = "camera.png";
            pickPhotoBtn.Image = "file.png";
            dishStack.Children.Add(takePhotoBtn);
            dishStack.Children.Add(pickPhotoBtn);

            takePhotoBtn.Clicked += (sender, args) => { TakePhoto(dish); };
            dishName.Completed += (sender, args) => { SaveDishName(dish, dishName.Text); };
            //pickPhotoBtn.Clicked += (sender, args) => { PickPhoto(dish); };

            return dishStack;
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
            menu.Dishes = _dishes;
            _repo.Create(menu);
            DishWrapperStack.Children.Clear();
            _dishes = new List<Dish>();
            DishWrapperStack.Children.Add(NewDishStack());
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
