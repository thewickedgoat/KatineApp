using KantineApp.Entity;
using KantineApp.Interface;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KantineApp.Gateway
{
    public class ServiceGateway : IServiceGateway
    {
        HttpClient client;

        public ServiceGateway()
        {
            client = new HttpClient(new NativeMessageHandler());
            client.MaxResponseContentBufferSize = 256000;
        }

        public async void Create(MenuEntity menu)
        {
            var uri = new Uri("http://cantine-restapi-webapp.azurewebsites.net/api/Menu");


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
            catch(Exception e)
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
            var uri = new Uri("http://cantine-restapi-webapp.azurewebsites.net/api/Menu");
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
