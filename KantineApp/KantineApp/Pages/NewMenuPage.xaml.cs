using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace KantineApp.Pages
{
    public partial class NewMenuPage : ContentPage
    {
        public NewMenuPage()
        {
            InitializeComponent();
            WrapperStack.Children.Add(NewMenuItemStack());
        }

        /// <summary>
        /// Create menu item stack text line and buttons.
        /// </summary>
        /// <returns></returns>
        public StackLayout NewMenuItemStack()
        {
            var menuItemStack = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.StartAndExpand };
            menuItemStack.Children.Add(new Entry() { Placeholder = "Rettens navn", HorizontalOptions = LayoutOptions.FillAndExpand });
            menuItemStack.Children.Add(new Button() { Image = "camera.png" });
            menuItemStack.Children.Add(new Button() { Image = "file.png" });
            return menuItemStack;
        }

        /// <summary>
        /// When clicked add new menu item stack to parent.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMenuItemStack_OnClicked(object sender, EventArgs e)
        {
            WrapperStack.Children.Add(NewMenuItemStack());
        }
    }
}
