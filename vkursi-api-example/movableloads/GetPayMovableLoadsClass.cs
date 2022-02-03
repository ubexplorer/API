﻿using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using vkursi_api_example.token;

namespace vkursi_api_example.movableloads
{
    public class GetPayMovableLoadsClass
    {
        /*
         
        23. ДРОРМ отримання витяга по Id обтяжння
        [POST] api/1.0/MovableLoads/getpaymovableloads
        
        curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getpaymovableloads' \
        --header 'ContentType: application/json' \
        --header 'Authorization: Bearer eyJhbGciOiJIUzI1Ni...' \
        --data-raw '{"Id":1278898}'
             
         */

        public static GetPayMovableLoadsResponseModel GetPayMovableLoads(string token, int movableId)
        {
            if (string.IsNullOrEmpty(token)) { AuthorizeClass _authorize = new AuthorizeClass();token = _authorize.Authorize();}

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetPayMovableLoadsRequestBodyModel GPMLRequestBody = new GetPayMovableLoadsRequestBodyModel
                {
                    Id = movableId                                          // Id обтяжння
                };

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getpaymovableloads");
                RestRequest request = new RestRequest(Method.POST);

                string body = JsonConvert.SerializeObject(GPMLRequestBody); // Example Body: {"Id":1278898}

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

            GetPayMovableLoadsResponseModel GPMLResponseRow = new GetPayMovableLoadsResponseModel();

            GPMLResponseRow = JsonConvert.DeserializeObject<GetPayMovableLoadsResponseModel>(responseString);

            return GPMLResponseRow;
        }
    }


    /*
        // Python - http.client example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("text/plain");
        RequestBody body = RequestBody.create(mediaType, "{\"Id\":1278898}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/MovableLoads/getpaymovableloads")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1Ni...")
          .build();
        Response response = client.newCall(request).execute();

        
        // Java - OkHttp example:

        import http.client
        import mimetypes
        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = "{\"Id\":1278898}"
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1Ni...'
        }
        conn.request("POST", "/api/1.0/MovableLoads/getpaymovableloads", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

    */

    public class GetPayMovableLoadsRequestBodyModel                         // Модель Body запиту
    {
        public int Id { get; set; }                                         // Id обтяжння
    }

    public class GetPayMovableLoadsResponseModel                            // Модель відповіді
    {
        public bool isSuccess { get; set; }                                 // Успішний запит (true - так / false - ні)
        public string status { get; set; }                                  // Статус запиту
        public Data data { get; set; }                                      // Дані
    }


    public class Data                                                       
    {
        public Guid userId { get; set; }                                    // Системний id Vkursi
        public Dictionary<int, Guid> success { get; set; }                  // Перелік успішнио отриманих обтяжень
        public List<int> withError { get; set; }                            // Перелік не успішнио отриманих обтяжень
        public string reportRef { get; set; }                               // Посилання на перегляд витягу на Vkursi
        public Report report { get; set; }                                  // Дані витягу
    }

    public class Report
    {
        public DateTime createdOn { get; set; }                             // Дата отримання
        public string dataObjectOriginal { get; set; }                      // Оригінальні дані відповіді від Nais (1в1).                       Опис полів від ДП Nais https://nais.gov.ua/files/general/2020/01/27/20200127160802-47.docx
        public DataObjectClear dataObject { get; set; }                     // Витяг (Оригінальні дані відповіді Nais перетворені в об'єкт).    Опис полів від ДП Nais https://nais.gov.ua/files/general/2020/01/27/20200127160802-47.docx
    }

    public class DataObjectClear                                            // Витяг
    {
        public int id { get; set; }                             // Ідентифікатор запису (для АПІ не несе інформаційного навантаження)
        public int opOpID { get; set; }                         // Номер обтяження
        public string lmType { get; set; } // тип обтяження / Податкова застава    LM_TYPE=12 / Звернення стягнення LM_TYPE=13 / Заборона на рухоме майно LM_TYPE=7 / Арешт рухомого майна LM_TYPE = 8 / Застава рухомого майна LM_TYPE = 10 / Інше обтяження рухомого майна LM_TYPE=999
        public string opStatus { get; set; } // Стан запису (активний анульований) – в АПІ надходить тільки статус 1 - активний
        public string opType { get; set; } // стан реєстрації обтяжень (тип операції - 1 реєстрація обтяження 2 -припинення обтяження)
        public DateTime actTerm { get; set; } // Термін дії: 
        public string archiveDate { get; set; } // Архівна дата
        public DateTime execTerm { get; set; } // Термін виконання зобов’язання
        public string prevRegistration { get; set; }
        public int reqReqID { get; set; }
        public string bnBnID { get; set; }
        public string regNum { get; set; } // Реєстраційний номер обтяження (зовнішній ключ)
        public string contractSum { get; set; } // Розмір основного зобов’язання: сума
        public string checkSum { get; set; } // Контрольна сума заяви (для АПІ не несе інформаційного навантаження)
        public string archiveNum { get; set; }
        public string penaltyInit { get; set; } // Ознака наявності “Звернення стягнення”
        public string currencyType { get; set; } // Розмір основного зобов’язання: валюта
        public string lmSort { get; set; } // вид обтяження / • публічне обтяження(LM_SORT= 1) / • приватне обтяження(LM_SORT= 2)
        public string alPossible { get; set; } // 1. Дозволено відчужувати / 2. Заборонено відчужувати / 3.	За погодженням з обтяжувачем
        public string lmTypeExtension { get; set; } // Опис у довільному форматі типу обтяження
        public string additional { get; set; } // Додаткові дані до обтяження
        public string archRegName { get; set; }
        public DateTime regDate { get; set; } // Дата обтяження
        public int currentObjID { get; set; }
        public string startDay { get; set; } // 
        public string startMonth { get; set; }
        public string startYear { get; set; }
        public double validContractSum { get; set; }
        public List<Property> properties { get; set; } // Опис майна
        public List<Subject> subjects { get; set; } // Інформація про обтяжувача 
        public List<CauseDocument> causeDocuments { get; set; }
        public DateTime opRegDate { get; set; } // Дата проведення операції формування витягу
        public string registrarInfo { get; set; } // Інформація про реєстратора
        public string lmState { get; set; } // Стан документа
        public List<Document> documents { get; set; }
        public string lmStateName { get; set; } // Назва стану документа
        public string currentObjName { get; set; } // Організація реєстратора, який сформував витяг
        public string currentRegistrar { get; set; } // Реєстратора, який сформував витяг
        public string penalty { get; set; } // Ознака наявності “Звернення стягнення”
    }

    public class PrTypeAttr
    {
        public string xtype { get; set; }
        public string name { get; set; }
        public string fieldLabel { get; set; }
        public string attrTyp { get; set; }
        public int dataType { get; set; }
        public bool enforceMaxLength { get; set; }
        public int daRnNum { get; set; }
        public string width { get; set; }
        public int flex { get; set; }
        public bool allowBlank { get; set; }
        public int maxLength { get; set; }
        public string vtype { get; set; }
    }

    public class Property
    {
        public int id { get; set; }
        public int? prParentID { get; set; }
        public string prPrID { get; set; }
        public int opOpID { get; set; } // Номер обтяження
        public string adAdID { get; set; }
        public string reReID { get; set; }
        public string reLandRegNumHash { get; set; }
        public string reLetterHash { get; set; }
        public string reCadNumHash { get; set; }
        public string mvRegNum { get; set; }
        public string prType { get; set; }
        public string reLandRegNum { get; set; }
        public string reLandType { get; set; }
        public string reLetter { get; set; }
        public string reCadNum { get; set; }
        public string changeType { get; set; }
        public string prCategory { get; set; }
        public string mvSrNum { get; set; }
        public string otherRegNum { get; set; }
        public string mvSrNumHash { get; set; }
        public string mvRegNumHash { get; set; }
        public string otherRegNumHash { get; set; }
        public string additional { get; set; }
        public string prTypeExtension { get; set; }
        public string reLandTypeExtension { get; set; }
        public string otherRegNumNID { get; set; }
        public string isArchive { get; set; }
        public string prRegNum { get; set; } // Реєстраційний номер майна
        public string prRegNumNID { get; set; }
        public string driRegDate { get; set; }
        public string driRegNum { get; set; }
        public string fullExtension { get; set; } // Опис майна
        public List<string> prAttr { get; set; } // Масив переліку динамічних атрибутів та їх значень
        public List<PrTypeAttr> prTypeAttr { get; set; }
        public string addProperties { get; set; } // Додаткові відомості
    }

    public class Subject // Інформація про обтяжувача 
    {
        public int id { get; set; }
        public string sbjType { get; set; } // Тип суб'єкта: / 1 (фіз.особа) / 2 (юр.особа)
        public string subjectParentID { get; set; }
        public string rusNID { get; set; }
        public string atuAtuID { get; set; }
        public string cnCnID { get; set; }
        public int rlRlID { get; set; }
        public int opOpID { get; set; } // Номер обтяження
        public int ukrNID { get; set; }
        public string foreignSubject { get; set; } // Ознака нерезедентності
        public string birthMonth { get; set; }
        public string codeAbsence { get; set; }
        public string code { get; set; }
        public string changeType { get; set; } // Тип змін (для АПІ не несе інформаційного навантаження)
        public string birthYear { get; set; }
        public string birthDay { get; set; }
        public string addressIndex { get; set; }
        public string additional { get; set; } // Додаткові відомості
        public string birthPlace { get; set; }
        public string name { get; set; }
        public string simpleAddress { get; set; }
        public string addressDetails { get; set; }
        public string document { get; set; }
        public string rusName { get; set; }
        public string fullName { get; set; }
        public string dcCountry { get; set; }
        public string room { get; set; }
        public string dcRoomType { get; set; }
        public string dcBuildingType { get; set; }
        public string building { get; set; }
        public string dcObjectNumType { get; set; }
        public string objectNum { get; set; }
        public string dcHouseType { get; set; }
        public string house { get; set; }
        public string streetAtuID { get; set; }
        public string phone { get; set; }
        public string atuAtuStr { get; set; }
        public string rlName { get; set; }
        public string addSubject { get; set; }
    }

    public class CauseDocument
    {
        public int id { get; set; } // Id
        public int opOpID { get; set; } // Номер обтяження
        public DateTime pubDate { get; set; } // Дата видачі документа
        public string cdType { get; set; } // Тип документа
        public string serNum { get; set; } // Номер документа
        public string additional { get; set; } // Додаткові відомості
        public string publisher { get; set; } // Ким виданий
        public string cdTypeExtension { get; set; } // Опис типу документу
        public string cdParentID { get; set; }
        public string changeType { get; set; } // Тип зміни запису про документ (додавання, виправлення тощо)
        public string causeDocumentInfo { get; set; } // Інформація про документ - підставу
    }

    public class Document
    {
        public int docID { get; set; }
        public int regNum { get; set; }
        public int objObjID { get; set; }
        public int empEmpID { get; set; }
        public string docType { get; set; }
        public string generateddocument { get; set; }
        public DateTime regDate { get; set; } // Дата реєстрації
        public string seSeID { get; set; }
        public string docState { get; set; }
        public string docTypeName { get; set; }
        public string empFullName { get; set; }
    }
}
