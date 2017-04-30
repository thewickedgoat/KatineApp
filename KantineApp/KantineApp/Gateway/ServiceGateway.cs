using Android.Graphics;
using KantineApp.Entity;
using KantineApp.Interface;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KantineApp.Gateway
{
    public class ServiceGateway : IServiceGateway
    {
        HttpClient client;
        public string API = "http://cantine-restapi-webapp.azurewebsites.net/api";
        //public string API = "http://169.254.80.80:7874/api";

        public ServiceGateway()
        {
            client = new HttpClient(new NativeMessageHandler());
            client.MaxResponseContentBufferSize = 256000;
        }

        private Byte[] ConvertToByteArray(string input)
        {
            var icon = BitmapFactory.DecodeFile(input);
            var ms = new MemoryStream();

            icon.Compress(Bitmap.CompressFormat.Jpeg, 80, ms);
            var iconBytes = ms.ToArray();
            return iconBytes;
        }


        public async void Create(MenuEntity menu)
        {
            var uri = new Uri(API + "/menu");

            foreach (var dish in menu.Dishes)
            {
                var bytes = ConvertToByteArray(dish.Image);
                var imageContent = new StringContent(JsonConvert.SerializeObject(new { dishName = dish.Name, imageBytes = bytes }), Encoding.UTF8, "application/json");

                HttpResponseMessage imageResponse = await client.PostAsync(new Uri(uri + "/UploadImage"), imageContent);

                if (imageResponse.IsSuccessStatusCode)
                {
                    dish.Image = imageResponse.Content.ReadAsStringAsync().Result.Replace("\"", "");
                }
            }

            var json = JsonConvert.SerializeObject(menu);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Debug.WriteLine(json);

            try
            {
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@" Menu Succesfully saved");
                }
                else
                {
                    Debug.WriteLine(response.StatusCode + " ");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR 404 - " + e);
            }

        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public MenuEntity Read(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MenuEntity>> ReadAll()
        {
            var menus = new List<MenuEntity>();
            var uri = new Uri(API + "/Menu");
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    menus = JsonConvert.DeserializeObject<List<MenuEntity>>(content);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return menus;
        }

        public bool Update(MenuEntity menu)
        {
            throw new NotImplementedException();
        }
    }
}
