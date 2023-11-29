﻿using System;
using RestSharp;
using Newtonsoft.Json;
using vkursi_api_example.token;
using System.Collections.Generic;

namespace vkursi_api_example.estate
{
    public class EstatePutOnMonitoringClass
    {
        /*

        45. Додати об'єкт до моніторингу нерухомості за номером ОНМ (sms rrp) 
        [POST] /api/1.0/estate/estateputonmonitoring

        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/estate/estateputonmonitoring' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --header 'Content-Type: text/plain' \
        --header 'Cookie: ARRAffinity=60c7763e47a70e864d73874a4687c10eb685afc08af8bda506303f7b37b172b8' \
        --data-raw '{"OnmNumbers":[1260724348000],"CadastrNumbers":null,"DateTimeEnd":"2020-10-31T00:00:00"}'

        */

        public static EstatePutOnMonitoringResponseModel EstatePutOnMonitoring(string token, string dateTimeEnd, List<long> onmNumbers)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                EstatePutOnMonitoringRequestBodyModel EPOMRequestBody = new EstatePutOnMonitoringRequestBodyModel
                {
                    DateTimeEnd = DateTime.Parse(dateTimeEnd),
                    OnmNumbers = onmNumbers
                };

                string body = JsonConvert.SerializeObject(EPOMRequestBody);

                // Example Body: {"OnmNumbers":[1260724348000],"CadastrNumbers":null,"DateTimeEnd":"2020-10-31T00:00:00"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/estate/estateputonmonitoring");
                RestRequest request = new RestRequest(Method.POST);

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

                else if ((int)response.StatusCode != 200 || response.ErrorMessage == "The operation has timed out.")
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            EstatePutOnMonitoringResponseModel EPOMResponseRow = new EstatePutOnMonitoringResponseModel();

            EPOMResponseRow = JsonConvert.DeserializeObject<EstatePutOnMonitoringResponseModel>(responseString);

            return EPOMResponseRow;
        }
    }

    /*
        // Python - http.client example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"OnmNumbers\":[1260724348000],\"CadastrNumbers\":null,\"DateTimeEnd\":\"2020-10-31T00:00:00\"}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...',
          'Content-Type': 'text/plain'
        }
        conn.request("POST", "/api/1.0/estate/estateputonmonitoring", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))


        // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("text/plain");
        RequestBody body = RequestBody.create(mediaType, "{\"OnmNumbers\":[1260724348000],\"CadastrNumbers\":null,\"DateTimeEnd\":\"2020-10-31T00:00:00\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/estate/estateputonmonitoring")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .addHeader("Content-Type", "text/plain")
          .build();
        Response response = client.newCall(request).execute();

     */
    /// <summary>
    /// Модель запиту 
    /// </summary>
    public class EstatePutOnMonitoringRequestBodyModel                                  // 
    {/// <summary>
     /// Перелік номерів ОНМ
     /// </summary>
        public List<long> OnmNumbers { get; set; }                                      // 

        // public List<string> CadastrNumbers { get; set; }                             // ВИКЛЮЧЕНО! Перелік кадастрових номерів
        /// <summary>
        /// Дата до якої діє моніторинг
        /// </summary>
        public DateTime DateTimeEnd { get; set; }                                       // 
    }

    /// <summary>
    /// Модель на відповідь
    /// </summary>
    public class EstatePutOnMonitoringResponseModel                                     // 
    {/// <summary>
     /// Чи успішний запит
     /// </summary>
        public bool IsSuccess { get; set; }                                             // 
        /// <summary>
        /// Статус відповіді по API
        /// </summary>
        public string Status { get; set; }                                              // 
        /// <summary>
        /// Перелік не знайдених кадастрових номерів
        /// </summary>
        public List<string> NotFoundCadastrsOnMonitoring { get; set; }                  // 
        /// <summary>
        /// Перелік не знайдених номерів ОНМ
        /// </summary>
        public List<string> NotFoundOnmOnMonitoring { get; set; }                       // 
    }
}
