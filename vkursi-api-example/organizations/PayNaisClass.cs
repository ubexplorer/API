﻿using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace vkursi_api_example.organizations
{
    public class PayNaisClass
    {
        /*

        Метод:
            59. Оригінальний метод Nais, на отримання повних данних по ЮО або ФОП за кодом NaisId (який ми отримуємо з [POST] /api/1.0/organizations/freenais)
            [GET] /api/1.0/organizations/paynais

        cURL запиту:curl --location --request GET 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/paynais?id=811202' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...' \
            --header 'Content-Type: application/json' \
            

        Приклад відповіді: 
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/PayNaisResponse.json

        */

        public static OrganizationaisElModel PayNais(ref string token, long? naisId, string code)
        {
            if (string.IsNullOrEmpty(token)) 
            { 
                AuthorizeClass _authorize = new AuthorizeClass(); 
                token = _authorize.Authorize(); 
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/paynais");
                RestRequest request = new RestRequest(Method.GET);

                request.AddParameter("id", naisId);                             // Код Nais (який ми отримуємо з [POST] /api/1.0/organizations/freenais)
                request.AddParameter("code", code);
                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
                }
                else if(!string.IsNullOrEmpty(responseString))
                {
                    // Перевіряємо на вміст помилки
                    PaynaisErrorResponseModel PaynaisErrorResponse = new PaynaisErrorResponseModel();

                    PaynaisErrorResponse = JsonConvert.DeserializeObject<PaynaisErrorResponseModel>(responseString);

                    if (PaynaisErrorResponse != null)
                    {
                        Console.WriteLine(PaynaisErrorResponse.Errors[0].Message);
                        return null;
                    }
                }
            }

            // responseString - Оригінальна відповід Nais

            // Приклад відповіді: https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/PayNaisResponse.json

            OrganizationaisElModel OrganizationaisElastic = new OrganizationaisElModel();

            OrganizationaisElastic = JsonConvert.DeserializeObject<OrganizationaisElModel>(responseString);

            return OrganizationaisElastic;
        }
    }

    /*
        // Python - http.client example:

            import http.client

            conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
            payload = ''
            headers = {
              'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsIn...',
              'Content-Type': 'application/json'
            }
            conn.request("GET", "/api/1.0/organizations/paynais?id=811202", payload, headers)
            res = conn.getresponse()
            data = res.read()
            print(data.decode("utf-8"))

        // Java - OkHttp example:

            OkHttpClient client = new OkHttpClient().newBuilder()
              .build();
            Request request = new Request.Builder()
              .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/paynais?id=811202")
              .method("GET", null)
              .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiI...")
              .addHeader("Content-Type", "application/json")
              .build();
            Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель відповіді з помилкою Paynais
    /// </summary>
    public partial class PaynaisErrorResponseModel                              // 
    {/// <summary>
    /// ???
    /// </summary>
        [JsonProperty("errors")]
        public List<Error> Errors { get; set; }
    }
    /// <summary>
    /// ???
    /// </summary>
    public partial class Error
    {/// <summary>
    /// ???
    /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        [JsonProperty("code")]
        public long Code { get; set; }
    }
}
