﻿using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using vkursi_api_example.token;
namespace vkursi_api_example.organizations
{
    public class GetYedynyyPodatokClass
    {
        /*
         
        81. Реєстр платників Єдиного податку
        [POST] /api/1.0/organizations/GetYedynyyPodatok 

        */

        public static GetYedynyyPodatokResponseModel GetYedynyyPodatok(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetYedynyyPodatokRequestBodyModel GYPRBody = new GetYedynyyPodatokRequestBodyModel
                {
                    Code = new List<string> { code }                        // Код ЄДРПОУ або ІПН
                };

                string body = JsonConvert.SerializeObject(GYPRBody);        // Example body: {"code":["21560766"]}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetYedynyyPodatok");
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
                else if ((int)response.StatusCode == 200 && responseString == "\"Not found\"")
                {
                    Console.WriteLine("За вказаним кодом організації не знайдено");
                    return null;
                }
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetYedynyyPodatokResponseModel GYPResponse = new GetYedynyyPodatokResponseModel();

            GYPResponse = JsonConvert.DeserializeObject<GetYedynyyPodatokResponseModel>(responseString);

            return GYPResponse;
        }
    }
    /// <summary>
    /// Модель запиту (Example: {"code":["21560766"]})
    /// </summary>
    public class GetYedynyyPodatokRequestBodyModel                              // 
    {/// <summary>
     /// Код ЄДРПОУ
     /// </summary>
        public List<string> Code { get; set; }                                  // 
    }

    /// <summary>
    /// Модель на відповідь GetYedynyyPodatok
    /// </summary>
    public class GetYedynyyPodatokResponseModel                                 // 
    {/// <summary>
     /// Статус запиту (maxLength:128)
     /// </summary>
        public string Status { get; set; }                                      // 
        /// <summary>
        /// Запит виконано успішно (true - так / false - ні)
        /// </summary>
        public bool IsSuccess { get; set; }                                     // 
        /// <summary>
        /// Дані
        /// </summary>
        public List<OrgYedunuyPodatok> Data { get; set; }                       // 
    }/// <summary>
     /// Дані
     /// </summary>
    public class OrgYedunuyPodatok                                              // 
    {/// <summary>
     /// Код ЄДРПОУ за яким знайдено дані
     /// </summary>
        public string Code { get; set; }                                        // 
        /// <summary>
        /// Ставка ЄП
        /// </summary>
        public decimal? Stavka { get; set; }                                    // 
        /// <summary>
        /// Група ЄП
        /// </summary>
        public int? Grup { get; set; }                                          // 
        /// <summary>
        /// Дата отримання
        /// </summary>
        public DateTime? DateStart { get; set; }                                // 
        /// <summary>
        /// Дата анулювання
        /// </summary>
        public DateTime? DateEnd { get; set; }                                  // 
        /// <summary>
        /// Статус (1 Группа ПДВ / 2 Группа без ПДВ / 3 Группа без ПДВ / 3 Группа ПДВ" / Не платник ЄП)
        /// </summary>
        public string Info { get; set; }                                        // 
    }
}
