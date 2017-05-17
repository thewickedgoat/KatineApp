using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace KantineApp
{
    
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            NavigationPage navPage = new NavigationPage(new MainPage())
            {          
                BarBackgroundColor = Color.FromHex("#222"),
                BarTextColor = Color.FromHex("#ededed")                
            };            
            MainPage = navPage;         
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
