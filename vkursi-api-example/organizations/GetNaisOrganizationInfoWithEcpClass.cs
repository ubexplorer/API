﻿using Newtonsoft.Json;
using RestSharp;
using System;
using vkursi_api_example.token;

namespace vkursi_api_example.organizations.GetNaisOrganizationInfoWithEcp
{
    public class GetNaisOrganizationInfoWithEcpClass
    {
        /*

        Метод:
            150. Відомості (витяг) з ЄДР з електронною печаткою (КЕП) Державного підприємства “НАІС”
            [POST] /api/1.0/organizations/GetNaisOrganizationInfoWithEcp

        cURL запиту:
            curl --location --request POST 'https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetNaisOrganizationInfoWithEcp' \
            --header 'ContentType: application/json' \
            --header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6Ik...' \
            --header 'Content-Type: application/json' \
            --data-raw '{"Code":"00131305"}'

        Приклад відповіді:
            https://github.com/vkursi-pro/API/blob/master/vkursi-api-example/responseExample/GetNaisOrganizationInfoWithEcpResponse.json

        */

        public static GetNaisOrganizationInfoWithEcpResponseModel GetNaisOrganizationInfoWithEcp(ref string token, string code)
        {
            if (string.IsNullOrEmpty(token))
            {
                AuthorizeClass _authorize = new AuthorizeClass();
                token = _authorize.Authorize();
            }

            string responseString = string.Empty;

            while (string.IsNullOrEmpty(responseString))
            {
                GetNaisOrganizationInfoWithEcpRequestBodyModel GAOONDRequestBody = new GetNaisOrganizationInfoWithEcpRequestBodyModel
                {
                    Code = code                                                        // Код ЄДРПОУ аба ІПН
                };

                string body = JsonConvert.SerializeObject(GAOONDRequestBody);           // Example body: {"Code":"00131305"}

                RestClient client = new RestClient("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetNaisOrganizationInfoWithEcp");
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
                else if ((int)response.StatusCode != 200)
                {
                    Console.WriteLine("Запит не успішний");
                    return null;
                }
            }

            GetNaisOrganizationInfoWithEcpResponseModel GNWEResponse = new GetNaisOrganizationInfoWithEcpResponseModel();

            GNWEResponse = JsonConvert.DeserializeObject<GetNaisOrganizationInfoWithEcpResponseModel>(responseString);

            return GNWEResponse;
        }
    }

    /*

    // Java - OkHttp example:

        OkHttpClient client = new OkHttpClient().newBuilder()
          .build();
        MediaType mediaType = MediaType.parse("application/json");
        RequestBody body = RequestBody.create(mediaType, "{\"Code\":\"00131305\"}");
        Request request = new Request.Builder()
          .url("https://vkursi-api.azurewebsites.net/api/1.0/organizations/GetNaisOrganizationInfoWithEcp")
          .method("POST", body)
          .addHeader("ContentType", "application/json")
          .addHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiIs...")
          .addHeader("Content-Type", "application/json")
          .build();
        Response response = client.newCall(request).execute();


    // Python - http.client example:

        import http.client
        import json

        conn = http.client.HTTPSConnection("vkursi-api.azurewebsites.net")
        payload = json.dumps({
          "Code": "00131305"
        })
        headers = {
          'ContentType': 'application/json',
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIs...',
          'Content-Type': 'application/json'
        }
        conn.request("POST", "/api/1.0/organizations/GetNaisOrganizationInfoWithEcp", payload, headers)
        res = conn.getresponse()
        data = res.read()
        print(data.decode("utf-8"))

    */

    /// <summary>
    /// Body запиту на метод GetNaisOrganizationInfoWithEcp
    /// </summary>
    public class GetNaisOrganizationInfoWithEcpRequestBodyModel
    {
        /// <summary>
        /// Код ЄДРПОУ або ІПН
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// Відповідь на метод GetNaisOrganizationInfoWithEcp
    /// </summary>
    public class GetNaisOrganizationInfoWithEcpResponseModel
    {
        /// <summary>
        /// Чи успішний запит
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Статус відповіді
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Повна інформація про організацію з НАІС
        /// </summary>
        public GetAdvancedOrganizationOnlyNaisDataResponseData Data { get; set; }

        /// <summary>
        /// Посилання вебсервіс vkursi.pro з повною інформацією про організацію з НАІС
        /// </summary>
        [JsonProperty("href")]
        public string Href { get; set; }
        /// <summary>
        /// Посилання на скачування архіву з повною інформацією про організацію з НАІС
        /// </summary>
        [JsonProperty("hrefAllDataZip")]
        public string HrefAllDataZip { get; set; }

        /// <summary>
        /// Посилання на скачування Pdf з повною інформацією про організацію
        /// </summary>
        [JsonProperty("hrefEdrDataPdf")]
        public string HrefEdrDataPdf { get; set; }
    }

    /// <summary>
    /// Повна інформація про організацію з НАІС
    /// </summary>
    public class GetNaisOrganizationInfoWithEcpResponseData
    {
        public int id { get; set; }                                             // Унікальний ідентифікатор суб’єкта
        public int? state { get; set; }                                         // Таблица ###. Стан суб’єкта
        public string state_text { get; set; }                                  // Текстове відображення стану суб’єкта (maxLength:64)
        public string code { get; set; }                                        // ЄДРПОУ (maxLength:10)
        /// <summary>
        /// (Тільки для ФОП) Назва країни громадянства ФОП
        /// </summary>
        public string country { get; set; }
        public OrgNamesModel names { get; set; }                                // Назва суб’єкта
        public string olf_code { get; set; }                                    // Код організаційно-правової форми суб’єкта, якщо суб’єкт – юридична особа (maxLength:256)
        public string olf_name { get; set; }                                    // Назва організаційно-правової форми суб’єкта, якщо суб’єкт – юридична особа (maxLength:256)
        /// <summary>
        /// Тип ОПФ
        /// </summary>
        public string olf_subtype { get; set; }
        public string founding_document { get; set; }                           // Назва установчого документа, якщо суб’єкт – юридична особа (maxLength:128)
        /// <summary>
        /// Діяльність на підставі: 
        /// «1» - власного установчого документа
        /// «2» - модельного статуту (якщо суб’єкт юридична особа)
        /// </summary>
        public string founding_document_type { get; set; }
        /// <summary>
        /// Код модельного статуту (якщо суб’єкт юридична особа)
        /// </summary>
        public string founding_document_code { get; set; }
        /// <summary>
        /// Назва установчого документа (якщо суб’єкт юридична особа)
        /// </summary>
        public string founding_document_name { get; set; }
        public OrgExecutivePower executive_power { get; set; }                  // Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить державне підприємство або частка держави у статутному капіталі юридичної особи, якщо ця частка становить не менше 25 відсотків
        public string object_name { get; set; }                                 // Місцезнаходження реєстраційної справи (maxLength:256)
        public OrgFounders[] founders { get; set; }                             // Array[Founder]. Перелік засновників (учасників) юридичної особи, у тому числі прізвище, ім’я, по батькові, якщо засновник – фізична особа; найменування, місцезнаходження та ідентифікаційний код юридичної особи, якщо засновник – юридична особа
        public OrgFounders[] beneficiaries { get; set; }                        // Перелік кінцевих бенефіціарних власників(КБВ) юридичної особи
        public OrgBranches[] branches { get; set; }                             // Array[Branch]. Перелік відокремлених підрозділів юридичної особи
        public OrgAuthorisedCapital authorised_capital { get; set; }            // Дані про розмір статутного капіталу (статутного або складеного капіталу) та про дату закінчення його формування, якщо суб’єкт – юридична особа
        public string management { get; set; }                                  // Відомості про органи управління юридичної особи (maxLength:256)
        public string managing_paper { get; set; }                              // Найменування розпорядчого акта, якщо суб’єкт – юридична особа (maxLength:256)
        public bool? is_modal_statute { get; set; }                             // Дані про наявність відмітки про те, що юридична особа створюється та діє на підставі модельного статуту
        /// <summary>
        /// Відомості про структуру власності юридичної особи
        /// </summary>
        public PropertyStruct property_struct { get; set; }
        public OrgActivityKinds[] activity_kinds { get; set; }                  // Перелік видів економічної діяльності
        public OrgNaisHeads[] heads { get; set; }                               // Array[Head]. Прізвище, ім’я, по батькові, дата обрання (призначення) осіб, які обираються (призначаються) до органу управління юридичної особи, уповноважених представляти юридичну особу у правовідносинах з третіми особами, або осіб, які мають право вчиняти дії від імені юридичної особи без довіреності, у тому числі підписувати договори та дані про наявність обмежень щодо представництва від імені юридичної особи
        public OrgNaisAddress address { get; set; }                             // Адреса
        public OrgNaisRegistration registration { get; set; }                   // inline_model_2. Дата державної реєстрації, дата та номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу
        public OrgNaisBankruptcy bankruptcy { get; set; }                       // inline_model_3. Дані про перебування юридичної особи в процесі провадження у справі про банкрутство, санації
        public OrgNaisTermination termination { get; set; }                     // inline_model_4. Дата та номер запису про державну реєстрацію припинення юридичної особи, підстава для його внесення
        public OrgNaisTerminationCancel termination_cancel { get; set; }        // inline_model_5. Дата та номер запису про відміну державної реєстрації припинення юридичної особи, підстава для його внесення
        public OrgNaisAssignees[] assignees { get; set; }                       // RelatedSubject. Дані про юридичних осіб-правонаступників: повне найменування та місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
        public OrgNaisAssignees[] predecessors { get; set; }                    // RelatedSubject. Дані про юридичних осіб-правонаступників: повне найменування та місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
        public OrgNaisRegistrations[] registrations { get; set; }               // inline_model_6. Відомості, отримані в порядку взаємного обміну інформацією з відомчих реєстрів органів статистики,  Міндоходів, Пенсійного фонду України
        public OrgNaisPrimaryActivityKind primary_activity_kind { get; set; }   // inline_model_7. Дані органів статистики про основний вид економічної діяльності юридичної особи, визначений на підставі даних державних статистичних спостережень відповідно до статистичної методології за підсумками діяльності за рік
        public string prev_registration_end_term { get; set; }                  // Термін, до якого юридична особа перебуває на обліку в органі Міндоходів за місцем попередньої реєстрації, у разі зміни місцезнаходження юридичної особи
        public OrgNaisContacts contacts { get; set; }                           // Контактні дані
        public string[] open_enforcements { get; set; }                         // Дата відкриття виконавчого провадження щодо юридичної особи (для незавершених виконавчих проваджень)
    }

    /// <summary>
    /// Назва суб’єкта
    /// </summary>
    public class OrgNamesModel
    {
        public string name { get; set; }                                        // Повна назва суб’єкта (maxLength:512)
        public bool? include_olf { get; set; }                                  // Вказує, чи треба додавати організаційно-правову форму до назви, якщо суб’єкт – юридична особа
        public string display { get; set; }                                     // Назва для відображення (з ОПФ чи без, в залежності від параметру include_olf), якщо суб’єкт – юридична особа (maxLength:512)

        [JsonProperty("short")]
        public string shortName { get; set; }                                   // Коротка назва, якщо суб’єкт – юридична особа (maxLength:512)
        public string name_en { get; set; }                                     // Повна назва суб’єкта англійською мовою, якщо суб’єкт – юридична особа (maxLength:512)
        public string short_en { get; set; }                                    // Коротка назва англійською мовою, якщо суб’єкт – юридична особа (maxLength:512)
    }

    public class OrgExecutivePower                                              // Центральний чи місцевий орган виконавчої влади, до сфери управління якого належить державне підприємство або частка держави у статутному капіталі юридичної особи, якщо ця частка становить не менше 25 відсотків
    {
        public string name { get; set; }                                        // Назва (maxLength:512)
        public string code { get; set; }                                        // ЄДРПОУ (maxLength:10)
    }

    /// <summary>
    /// Array[Founder]. Перелік засновників (учасників) або бенефіціарів юридичної особи, у тому числі прізвище, ім’я, по батькові, 
    /// якщо засновник – фізична особа; найменування, місцезнаходження та ідентифікаційний код юридичної особи, якщо засновник – юридична особа
    /// </summary>
    public class OrgFounders
    {
        /// <summary>
        /// Повна назва суб’єкта (maxLength:512)
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// ЄДРПОУ код, якщо суб’єкт – юридична особа (maxLength:10)
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// Адреса
        /// </summary>
        public OrganizationaisAddress address { get; set; }
        /// <summary>
        /// Назва країни громадянства КБВ (якщо громадянств декілька, то назви країн відображені через розділовий знак «;»)
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// Прізвище (якщо суб’єкт - приватна особа); (maxLength:256)
        /// </summary>
        public string last_name { get; set; }
        /// <summary>
        /// Ім’я та по-батькові (якщо суб’єкт – приватна особа) (maxLength:256)
        /// </summary>
        public string first_middle_name { get; set; }
        /// <summary>
        /// Таблица ###. Роль по відношенню до пов’язаного суб’єкта
        /// </summary>
        public int? role { get; set; }
        /// <summary>
        /// Тип бенефіцарного володіння: 
        /// «5» - Прямий вирішальний вплив;
        /// «6» - Не прямий вирішальний вплив;
        /// «7» - Прямий та непрямий вирішальний вплив;
        /// </summary>
        public string beneficiaries_type { get; set; }
        /// <summary>
        /// Відсоток частки статутного капіталу або відсоток права голосу
        /// </summary>
        public string interest { get; set; }
        /// <summary>
        /// Відсоток частки статутного капіталу або відсоток права голосу (непрямий вплив)
        /// </summary>
        public decimal? indirect_interest { get; set; }
        /// <summary>
        /// Інший характер та міра впливу
        /// </summary>
        public string other_impact { get; set; }
        /// <summary>
        /// Ознака, що можлива недостовірність інформації про КБВ
        /// </summary>
        public bool? beneficiary_false { get; set; }
        /// <summary>
        /// Текстове відображення ролі (maxLength:64)
        /// </summary>
        public string role_text { get; set; }
        /// <summary>
        /// Ідентифікатор суб'єкта
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// Посилання на сторінку з детальною інформацією про суб'єкт (maxLength:64)
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// Розмір частки у статутному капіталі пов’язаного суб’єкта (лише для засновників) (maxLength:128)
        /// </summary>
        public string capital { get; set; }
        /// <summary>
        /// Причина відсутності КБВ (якщо у юридичної особи відсутні КБВ)
        /// </summary>
        public int? reason { get; set; }
    }

    /// <summary>
    /// Адреса
    /// </summary>
    public class OrgNaisAddress
    {
        public string zip { get; set; }                                         // Поштовий індекс (maxLength:16)
        public string country { get; set; }                                     // Назва країни (maxLength:64)
        public string address { get; set; }                                     // Адреса (maxLength:256)
        public OrgNaisParts parts { get; set; }                                 // Детальна адреса
    }

    /// <summary>
    /// Детальна адреса
    /// </summary>
    public class OrgNaisParts
    {
        public string atu { get; set; }                                         // Адміністративна територіальна одиниця (maxLength:32)
        public string street { get; set; }                                      // Вулиця (maxLength:256)
        public string house_type { get; set; }                                  // Тип будівлі ('буд.', 'інше') (maxLength:64)
        public string house { get; set; }                                       // Номер будинку, якщо тип - 'буд.' (maxLength:64)
        public string building_type { get; set; }                               // Тип будівлі (maxLength:64)
        public string building { get; set; }                                    // Номер будівлі (maxLength:64)
        public string num_type { get; set; }                                    // Тип приміщення (maxLength:64)
        public string num { get; set; }                                         // Номер приміщення (maxLength:32)
    }

    /// <summary>
    /// Array[Branch]. Перелік відокремлених підрозділів юридичної особи
    /// </summary>
    public class OrgBranches
    {
        public string name { get; set; }                                        // Повна назва суб’єкта (maxLength:512)
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт - юр.особа (maxLength:10)
        public int? role { get; set; }                                          // Таблица ###. Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі (maxLength:64)
        public int? type { get; set; }                                          // Тип відокремленого підрозділу
        public string type_text { get; set; }                                   // Текстове відображення типу відокремленого підрозділу (maxLength:512)
        public OrgNaisAddress address { get; set; }                             // Адреса
    }

    /// <summary>
    /// Дані про розмір статутного капіталу (статутного або складеного капіталу) 
    /// та про дату закінчення його формування, якщо суб’єкт – юридична особа
    /// </summary>
    public class OrgAuthorisedCapital
    {
        public string value { get; set; }                                       // Сумма (maxLength:128)
        public DateTime? date { get; set; }                                     // Дата формування
    }

    /// <summary>
    /// Перелік видів економічної діяльності
    /// </summary>
    public class OrgActivityKinds
    {
        public string code { get; set; }                                        // Код згідно КВЕД (maxLength:64)
        public string name { get; set; }                                        // Найменування виду діяльності (maxLength:256)
        public bool? is_primary { get; set; }                                   // Вказує, чи є вид діяльності основним (згідно даних органів статистики про основний вид економічної діяльності юридичної особи)
    }

    /// <summary>
    /// Array[Head]. Прізвище, ім’я, по батькові, дата обрання (призначення) осіб, які обираються (призначаються) 
    /// до органу управління юридичної особи, уповноважених представляти юридичну особу у правовідносинах з третіми особами, 
    /// або осіб, які мають право вчиняти дії від імені юридичної особи без довіреності, у тому числі підписувати договори 
    /// та дані про наявність обмежень щодо представництва від імені юридичної особи
    /// </summary>
    public class OrgNaisHeads                                                   
    {
        public string name { get; set; }                                        // Повна назва суб’єкта (maxLength:256)
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт - юр.особа (maxLength:10)
        public OrgNaisAddress address { get; set; }                             // Адреса
        public string last_name { get; set; }                                   // Прізвище (якщо суб’єкт - приватна особа) (maxLength:128)
        public string first_middle_name { get; set; }                           // Ім’я та по-батькові (якщо суб’єкт - приватна особа) (maxLength:128)
        public int? role { get; set; }                                          // Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі (maxLength:64)
        public int? id { get; set; }                                            // Ідентифікатор суб'єкта
        public string url { get; set; }                                         // Посилання на сторінку з детальною інформацією про суб'єкт (maxLength:64)
        public DateTime? appointment_date { get; set; }                         // Дата призначення
        public string restriction { get; set; }                                 // Обмеження (maxLength:512)
    }

    /// <summary>
    /// inline_model_2. Дата державної реєстрації, дата та номер запису в Єдиному державному реєстрі 
    /// про включення до Єдиного державного реєстру відомостей про юридичну особу
    /// </summary>
    public class OrgNaisRegistration
    {
        public DateTime? date { get; set; }                                     // Дата державної реєстрації
        public string record_number { get; set; }                               // Номер запису в Єдиному державному реєстрі про включення до Єдиного державного реєстру відомостей про юридичну особу (maxLength:32)
        public DateTime? record_date { get; set; }                              // Дата запису в Єдиному державному реєстрі
        public bool? is_separation { get; set; }                                // Державна реєстрація юридичної особи шляхом виділу
        public bool? is_division { get; set; }                                  // Державна реєстрація юридичної особи шляхом поділу
        public bool? is_merge { get; set; }                                     // Державна реєстрація юридичної особи шляхом злиття
        public bool? is_transformation { get; set; }                            // Державна реєстрація юридичної особи шляхом перетворення
    }

    /// <summary>
    /// inline_model_3. Дані про перебування юридичної особи 
    /// в процесі провадження у справі про банкрутство, санації
    /// </summary>
    public class OrgNaisBankruptcy
    {
        public DateTime? date { get; set; }                                     // Дата запису про державну реєстрацію провадження у справі про банкрутство
        public int? state { get; set; }                                         // Таблица ###. Стан суб’єкта
        public string state_text { get; set; }                                  // Текстове відображення стану суб’єкта (maxLength:128)
        public string doc_number { get; set; }                                  // Номер провадження про банкрутство (maxLength:64)
        public DateTime? doc_date { get; set; }                                 // Дата провадження про банкрутство
        public DateTime? date_judge { get; set; }                               // Дата набуття чинності
        public string court_name { get; set; }                                  // Найменування суду (maxLength:256)
    }

    /// <summary>
    /// inline_model_4. Дата та номер запису про державну реєстрацію припинення юридичної особи, 
    /// підстава для його внесення
    /// </summary>
    public class OrgNaisTermination
    {
        public DateTime? date { get; set; }                                     // Дата запису про державну реєстрацію припинення юридичної особи, або початку процесу ліквідації в залежності від поточного стану («в стані припинення», «припинено»)
        public int? state { get; set; }                                         // Таблица ###. Стан суб’єкта
        public string state_text { get; set; }                                  // Текстове відображення стану суб’єкта (maxLength:64)
        public string record_number { get; set; }                               // Номер запису про державну реєстрацію припинення юридичної особи (якщо в стані «припинено»); (maxLength:128)
        public string requirement_end_date { get; set; }                        // Відомості про строк, визначений засновниками (учасниками) юридичної особи, судом або органом, що прийняв рішення про припинення юридичної особи, для заявлення кредиторами своїх вимог (maxLength:128)
        public string cause { get; set; }                                       // Підстава для внесення запису про державну реєстрацію припинення юридичної особи (maxLength:512)
    }

    public class OrgNaisTerminationCancel                                       // inline_model_5. Дата та номер запису про відміну державної реєстрації припинення юридичної особи, підстава для його внесення
    {
        public DateTime? date { get; set; }                                     // Дата запису про відміну державної реєстрації припинення юридичної особи
        public string record_number { get; set; }                               // Номер запису про відміну державної реєстрації припинення юридичної особи (maxLength:64)
        public string doc_number { get; set; }                                  // Номер провадження про банкрутство (maxLength:64)
        public DateTime? doc_date { get; set; }                                 // Дата провадження про банкрутство
        public DateTime? date_judge { get; set; }                               // Дата набуття чинності
        public string court_name { get; set; }                                  // Найменування суду (maxLength:256)
    }

    /// <summary>
    /// Дані про юридичних осіб-правонаступників: повне найменування та 
    /// місцезнаходження юридичних осіб-правонаступників, їх ідентифікаційні коди
    /// </summary>
    public class OrgNaisAssignees                                      
    {
        public string name { get; set; }                                        // Повна назва суб’єкта (maxLength:512)
        public string code { get; set; }                                        // ЄДРПОУ код, якщо суб’єкт – юридична особа (maxLength:10)
        public OrgNaisAddress address { get; set; }                     // Адреса
        public string last_name { get; set; }                                   // Прізвище (якщо суб’єкт - приватна особа) (maxLength:256)
        public string first_middle_name { get; set; }                           // Ім’я та по-батькові (якщо суб’єкт - приватна особа) (maxLength:256)
        public int? role { get; set; }                                          // Роль по відношенню до пов’язаного суб’єкта
        public string role_text { get; set; }                                   // Текстове відображення ролі (maxLength:64)
        public int? id { get; set; }                                            // Ідентифікатор суб'єкта
        public string url { get; set; }                                         // Посилання на сторінку з детальною інформацією про суб'єкт (maxLength:64)
    }

    /// <summary>
    /// inline_model_6. Відомості, отримані в порядку взаємного обміну інформацією 
    /// з відомчих реєстрів органів статистики,  Міндоходів, Пенсійного фонду України
    /// </summary>
    public class OrgNaisRegistrations                                   
    {
        public string name { get; set; }                                        // Назва органу (maxLength:512)
        public string code { get; set; }                                        // Ідентифікаційний код органу (maxLength:10)
        public string type { get; set; }                                        // Тип відомчого реєстру (maxLength:64)
        public string description { get; set; }                                 // Назва відомчого реєстру (maxLength:512)
        public DateTime? start_date { get; set; }                               // Дата взяття на облік
        public string start_num { get; set; }                                   // Номер взяття на облік (maxLength:64)
        public DateTime? end_date { get; set; }                                 // Дата зняття з обліку
        public string end_num { get; set; }                                     // Номер зняття з обліку (maxLength:64)
    }

    /// <summary>
    /// inline_model_7. Дані органів статистики про основний вид економічної діяльності юридичної особи, 
    /// визначений на підставі даних державних статистичних спостережень відповідно до статистичної методології за підсумками діяльності за рік
    /// </summary>
    public class OrgNaisPrimaryActivityKind                             
    {
        public string name { get; set; }                                        // Назва КВЕД (maxLength:128)
        public string code { get; set; }                                        // Код КВЕД (maxLength:64)
        public string reg_number { get; set; }                                  // Дані про реєстраційний номер платника єдиного внеску (maxLength:128)
        [JsonProperty("class")]
        public string classProp { get; set; }                                   // Дані про клас ризику (maxLength:32)
    }

    /// <summary>
    /// Контактні дані
    /// </summary>
    public class OrganizationaisContacts
    {
        /// <summary>
        /// Електронна адреса (maxLength:128)
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// Перелік контактних телефонів (maxLength:128)
        /// </summary>
        public string[] tel { get; set; }
        /// <summary>
        /// Номер факсимільного апарату (maxLength:128)
        /// </summary>
        public string fax { get; set; }
        /// <summary>
        /// Інтернет сайт (maxLength:128)
        /// </summary>
        public string web_page { get; set; }
        /// <summary>
        /// Інші відомості (maxLength:128)
        /// </summary>
        public string anotherInfo { get; set; }
    }

    /// <summary>
    /// Відомості про структуру власності юридичної особи
    /// </summary>
    public class PropertyStruct
    {
        /// <summary>
        /// Відмітка, що структуру власності підписано
        /// </summary>
        public bool? struct_signed { get; set; }
        /// <summary>
        /// Дата структури власності
        /// </summary>
        public DateTime? date_struct { get; set; }
        /// <summary>
        /// Номер структури власності
        /// </summary>
        public string num_struct { get; set; }
        /// <summary>
        /// Прізвище особи ким підписано структуру власності
        /// </summary>
        public string last_name_sign { get; set; }
        /// <summary>
        /// Ім’я та по-батькові особи ким підписано структуру власності
        /// </summary>
        public string first_middle_name_sign { get; set; }
        /// <summary>
        /// Тип особи ким підписано структуру власності:
        /// «1» – Керівник 
        /// «2» – Представник
        /// «3» – Засновник(учасник)
        /// «4» – Інша уповноважена особа
        /// </summary>
        public int? type_sign { get; set; }
        /// <summary>
        /// Ознака, що можлива недостовірність структури власності
        /// </summary>
        public bool? struct_false { get; set; }
        /// <summary>
        /// Ознака, що структура власності визнана Національним банком України непрозорою
        /// </summary>
        public bool? struct_opaque { get; set; }
    }
}


/*
   Можливі значення станів суб’єкта:

   -1 - «скасовано»
   1 - «зареєстровано»
   2 - «в стані припинення»
   3 - «припинено»
   4 - «порушено справу про банкрутство»
   5 - «порушено справу про банкрутство (санація)»
   6 - «зареєстровано, свідоцтво про державну реєстрацію недійсне»

*/


/*
   Можливі значення ролей:

   1 - «суб’єкт підприємницької діяльності»
   2 - «представник»
   3 - «керівник»
   4 - «засновник»
   5 - «відокремлений підрозділ»
   6 - «особа - управитель майна»
   7 - «комісія з припинення (комісія з реорганізації, ліквідаційна комісія)»
   8 - «голова комісії з припинення або ліквідатор»
   9 - «правонаступник»
   10 - «попередник»
   11 - «керівник комісії з виділу»
   12 - «член комісії з виділу»
   13 - «ліквідатор»
   14 - «керуючий санацією»
   15 - «Розпорядник майна»
   16 - «Заявник»
   17 - «Керівний орган»
   18 - «Уповноважена особа Фонду гарантування вкладів фізичних осіб»
   19 - «Кінцевий бенефіціарний власник»
*/