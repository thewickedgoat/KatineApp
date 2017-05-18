using KantineApp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KantineApp.Pages
{
    public partial class GalleryPage : ContentPage
    {
        private IServiceGateway _serviceGatway = Factory.GetServiceGateway;


        private string chosenImgUrl;

        List<string> _images;


        public GalleryPage()
        {
            InitializeComponent();
            InitGallery();
        }

        public async void InitGallery()
        {
            _images = await _serviceGatway.GetAllImages();
            GridRowDefinitions();
            PopulateGrid();
        }

        public void GridRowDefinitions()
        {
            for (int i = 0; i <= (_images.Count / 3) + 1; i++)
            {
                imageGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
        }

        public void PopulateGrid()
        {
            var count = 0;
            foreach (var image in _images)
            {
                Image galleryImage;
                var row = count / 3;
                var col = count % 3;

                var imgurl = image.Insert(image.IndexOf("/upload/") + 8, "c_scale,h_257,w_325/");
                galleryImage = new Image { Aspect = Aspect.Fill, HorizontalOptions = LayoutOptions.FillAndExpand };
                galleryImage.Source = imgurl;

                imageGrid.Children.Add(galleryImage, col, row);
                count++;
                ImageTapRecognizer(imgurl, galleryImage);
            }
        }

        public void ImageTapRecognizer(string url, Image galleryImage)
        {
            var tap = new TapGestureRecognizer();
            tap.Tapped += (sender, eventArgs) =>
            {
                galleryImage.FadeTo(0.50, 50);
                chosenImgUrl = url;
            };

            galleryImage.GestureRecognizers.Add(tap);
        }

        private async void ChooseImage_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(chosenImgUrl, "ChosenImage");
            await Navigation.PopModalAsync();
        }
    }
} 
