﻿using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.estate
{
    public class GetCadastrCoordinatesClass
    {
        /*

        42. Запит на отримання геопросторових даних ПККУ
        [POST] /api/1.0/Estate/GetCadastrCoordinates

        Опис авторизації(отримання token для Authorization): https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/token/AuthorizeClass.cs

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/getcadastrcoordinates' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer token' \
        --header 'Content-Type: application/json' \
        --data-raw '{"CadNumb":["3523681200:02:000:0230"],"Format":"geojson"}'
         
        */
        public static string GetCadastrCoordinates(string token, string cadNumb, string format)
        {
            // Опис авторизації: https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/token/AuthorizeClass.cs

            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetCadastrCoordinatesRequestBodyModel GCCRequestBody = new GetCadastrCoordinatesRequestBodyModel
                {
                    CadNumb = new List<string>                                  // Перелік кадастровых номерів
                    {
                        cadNumb
                    },
                    Format = format                                             // Формат відповіді (geoJson, kml, shp)
                };

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/getcadastrcoordinates");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(GCCRequestBody);      // Example Body: {"CadNumb":["0521685603:01:004:0001"],"Format":"geojson"}

                request.AddHeader("ContentType", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                responseString = response.Content;

                if ((int)response.StatusCode == 401)
                {
                    Console.WriteLine("Не авторизований користувач або закінчився термін дії токену. Отримайте новый token на api/1.0/token/authorize");
                    AuthorizeClass _authorize = new AuthorizeClass();
                    token = _authorize.Authorize();
                }

                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            return responseString;                                               // Дані в вигяляді тексту в залежності від обранного формату (geoJson, kml, shp). shp - повертаеться в вигляді base64 якій можна конвертувати в архів https://base64.guru/converter/decode/file
        }
    }

    /*
     
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"СadNumb\":[\"0521685603:01:004:0001\"],\"Format\":\"geoJson\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "//api/1.0/estate/getcadastrcoordinates", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:
        
        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"СadNumb\":[\"0521685603:01:004:0001\"],\"Format\":\"geoJson\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net//api/1.0/estate/getcadastrcoordinates")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();

    */
    /// <summary>
    /// Модель Body запиту
    /// </summary>
    public class GetCadastrCoordinatesRequestBodyModel                          // 
    {/// <summary>
     /// Масив кадастрових номерів
     /// </summary>
        public List<string> CadNumb { get; set; }                               // 
        /// <summary>
        /// Формат відповіді (geoJson, kml, shp)
        /// </summary>
        public string Format { get; set; }                                      // 
    }
}
