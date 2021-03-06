﻿using Android.Graphics;
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

        public ServiceGateway()
        {
            client = new HttpClient(new NativeMessageHandler());
            client.MaxResponseContentBufferSize = 256000;
        }

        /*CREATE*/
        public async void Create(MenuEntity menu)
        {
            ImageHandler(menu);
            var uri = new Uri(API + "/menu");
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

        /*READ BY ID*/
        public async Task<MenuEntity> Read(int id)
        {
            var uri = new Uri(API + $"/Menu/{id}");
            MenuEntity menu = null;

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    menu = JsonConvert.DeserializeObject<MenuEntity>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return menu;
        }

        /*READ ALL*/
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

        /*UPDATE*/
        public async Task<bool> Update(MenuEntity menu)
        {
            ImageHandler(menu);
            var uri = new Uri(API + $"/menu/{menu.Id}");
            var json = JsonConvert.SerializeObject(menu);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Debug.WriteLine(json);

            try
            {
                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@" Menu Succesfully saved");
                    return true;
                }
                else
                {
                    Debug.WriteLine(response.StatusCode + " ");
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR 404 - " + e);
                return false;
            }
        }

        /*DELETE*/
        public async Task<bool> Delete(int id)
        {
            var uri = new Uri(API + $"/Menu/{id}");
            bool isDeleted = false;
            try
            {
                var response = await client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    isDeleted = JsonConvert.DeserializeObject<bool>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isDeleted;
        }

        /*GET ALL IMAGES*/
        public async Task<List<string>> GetAllImages()
        {
            var uri = new Uri(API + "/Menu/GetAllImages");
            var images = new List<string>();
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    images = JsonConvert.DeserializeObject<List<string>>(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return images;
        }
 
        private Byte[] ConvertToByteArray(string input)
        {
            var image = BitmapFactory.DecodeFile(input);
            var ms = new MemoryStream();

            image.Compress(Bitmap.CompressFormat.Jpeg, 80, ms);
            var imageBytes = ms.ToArray();
            return imageBytes;
        }
      
        private void ImageHandler(MenuEntity menu)
        {
            var uri = new Uri(API + "/menu/UploadImage");
            foreach (var dish in menu.Dishes)
            {
                if (!dish.Image.Contains("http"))
                {
                    var bytes = ConvertToByteArray(dish.Image);
                    var imageContent = new StringContent
                        (JsonConvert.SerializeObject(new
                        {
                            dishName = dish.Name,
                            imageBytes = bytes
                        }), Encoding.UTF8, "application/json");

                    HttpResponseMessage imageResponse = client.PostAsync(uri, imageContent).Result;

                    if (imageResponse.IsSuccessStatusCode)
                    {
                        dish.Image = imageResponse.Content.ReadAsStringAsync().Result.Replace("\"", "");
                    }
                }
            }
        }
    }
}
