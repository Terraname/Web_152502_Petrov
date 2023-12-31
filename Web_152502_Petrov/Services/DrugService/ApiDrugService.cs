using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Domain.Models;
using System.Net.Http;

namespace Web_152502_Petrov.Services.DrugService
{

    public class ApiDrugService : IDrugService
    {
        private readonly HttpClient _httpClient;
        private readonly int _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiDrugService> _logger;
        private readonly HttpContext _httpContext;

        public ApiDrugService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ApiDrugService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetValue<int>("ItemsPerPage");
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<ResponseData<Drug>> CreateDrugAsync(Drug drug, IFormFile? formFile)
        {
            var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Drugs");

            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await _httpClient.PostAsJsonAsync(uri, drug, _serializerOptions);

            if (response.IsSuccessStatusCode)
            {
                var data = await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<Drug>>
                    (_serializerOptions);

                if (formFile != null)
                {
                    await SaveImageAsync(data.Data.Id, formFile);
                }

                return data; // Drug;
            }

            _logger.LogError($"-----> object not created. Error:{response.StatusCode}");

            return new ResponseData<Drug>
            {
                Success = false,
                ErrorMessage = $"Объект не добавлен. Error:{response.StatusCode}"
            };
        }

        public async Task<ResponseData<ListModel<Drug>>> GetDrugListAsync(string? genreNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Drugs/");

            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            if (pageNo > 1)
                urlString.Append($"page{pageNo}");

            if (genreNormalizedName != null)
                urlString.Append($"?cathegory={genreNormalizedName}");

            if (!_pageSize.Equals(3))
                urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response
                        .Content
                        .ReadFromJsonAsync<ResponseData<ListModel<Drug>>>
                        (_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<ListModel<Drug>>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return new ResponseData<ListModel<Drug>>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task DeleteDrugAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Drugs/{id}");

            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await _httpClient.DeleteAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            }
        }

        public async Task<ResponseData<Drug>> GetDrugByIdAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Drugs/{id}");

            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response
                        .Content
                        .ReadFromJsonAsync<ResponseData<Drug>>
                        (_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return new ResponseData<Drug>
                    {
                        Success = false,
                        ErrorMessage = $"Ошибка: {ex.Message}"
                    };
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            return new ResponseData<Drug>
            {
                Success = false,
                ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
            };
        }

        public async Task UpdateDrugAsync(int id, Drug Drug, IFormFile? formFile)
        {
            var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Drugs/{id}");

            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var response = await _httpClient.PutAsync(uri, new StringContent(JsonSerializer.Serialize(Drug), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                if (formFile != null)
                    await SaveImageAsync(id, formFile);
            }
            else
            {
                _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
            }
        }
        private async Task SaveImageAsync(int id, IFormFile image)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Dishes/{id}")
            };

            var token = await _httpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            content.Add(streamContent, "formFile", image.FileName);
            request.Content = content;

            await _httpClient.SendAsync(request);
        }
    }
    //public class ApiDrugService : IDrugService
    //{
    //    private readonly HttpClient _httpClient;
    //    private readonly int _pageSize;
    //    private readonly JsonSerializerOptions _serializerOptions;
    //    private readonly ILogger<ApiDrugService> _logger;

    //    public ApiDrugService(
    //        HttpClient httpClient,
    //        IConfiguration configuration,
    //        ILogger<ApiDrugService> logger)
    //    {
    //        _httpClient = httpClient;
    //        _pageSize = configuration.GetValue<int>("ItemsPerPage");
    //        _serializerOptions = new JsonSerializerOptions()
    //        {
    //            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    //        };
    //        _logger = logger;
    //    }

    //    public async Task<ResponseData<Drug>> CreateDrugAsync(Drug drug, IFormFile? formFile)
    //    {
    //        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Drugs");
    //        var response = await _httpClient.PostAsJsonAsync(uri, drug, _serializerOptions);
    //        if (response.IsSuccessStatusCode)
    //        {
    //            var data = await response
    //            .Content
    //            .ReadFromJsonAsync<ResponseData<Drug>>
    //            (_serializerOptions);
    //            return data; // Drug;
    //        }
    //        _logger.LogError($"-----> object not created. Error:{response.StatusCode}");

    //        return new ResponseData<Drug>
    //        {
    //            Success = false,
    //            ErrorMessage = $"Объект не добавлен. Error:{response.StatusCode}"
    //        };
    //    }

    //    public async Task DeleteDrugAsync(int id)
    //    {
    //        var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Drugs/{id}");

    //        var response = await _httpClient.DeleteAsync(uri);

    //        if (!response.IsSuccessStatusCode)
    //        {
    //            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
    //        }
    //    }

    //    public async Task<ResponseData<Drug>> GetDrugByIdAsync(int id)
    //    {
    //        var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Drugs/{id}");

    //        var response = await _httpClient.GetAsync(uri);

    //        if (response.IsSuccessStatusCode)
    //        {
    //            try
    //            {
    //                return await response
    //                    .Content
    //                    .ReadFromJsonAsync<ResponseData<Drug>>
    //                    (_serializerOptions);
    //            }
    //            catch (JsonException ex)
    //            {
    //                _logger.LogError($"-----> Ошибка: {ex.Message}");
    //                return new ResponseData<Drug>
    //                {
    //                    Success = false,
    //                    ErrorMessage = $"Ошибка: {ex.Message}"
    //                };
    //            }
    //        }

    //        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
    //        return new ResponseData<Drug>
    //        {
    //            Success = false,
    //            ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
    //        };
    //    }
        

    //    public async Task<ResponseData<ListModel<Drug>>> GetDrugListAsync(string? genreNormalizedName, int pageNo = 1)
    //    {
    //    // подготовка URL запроса
    //        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}drugs/");
    //        // добавить категорию в маршрут
    //        if (genreNormalizedName != null)
    //        {
    //            urlString.Append($"{genreNormalizedName}/");
    //        };
    //        // добавить номер страницы в маршрут
    //        if (pageNo > 1)
    //        {
    //            urlString.Append($"page{pageNo}");
    //        };
    //        // добавить размер страницы в строку запроса
    //        if (!_pageSize.Equals("3"))
    //        {
    //            urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
    //        }
    //        // отправить запрос к API
    //        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

    //        if (response.IsSuccessStatusCode)
    //        {
    //            try
    //            {
    //                return await response
    //                .Content
    //                .ReadFromJsonAsync<ResponseData<ListModel<Drug>>>
    //                (_serializerOptions);
    //            }
    //            catch (JsonException ex)
    //            {
    //                _logger.LogError($"-----> Ошибка: {ex.Message}");
    //                return new ResponseData<ListModel<Drug>>
    //                {
    //                    Success = false,
    //                    ErrorMessage = $"Ошибка: {ex.Message}"
    //                };
    //            }
    //        }

    //        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
    //        return new ResponseData<ListModel<Drug>>
    //        {
    //            Success = false,
    //            ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
    //        };
    //    }

    //    public async Task UpdateDrugAsync(int id, Drug drug, IFormFile? formFile)
    //    {
    //        var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Drugs/{id}");

    //        var response = await _httpClient.PutAsync(uri, new StringContent(JsonSerializer.Serialize(drug), Encoding.UTF8, "application/json"));

    //        if (response.IsSuccessStatusCode)
    //        {
    //            if (formFile != null)
    //                await SaveImageAsync(id, formFile);
    //        }
    //        else
    //        {
    //            _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
    //        }
    //    }

    //    private async Task SaveImageAsync(int id, IFormFile image)
    //    {
    //        var request = new HttpRequestMessage
    //        {
    //            Method = HttpMethod.Post,
    //            RequestUri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Drugs/{id}")
    //        };

    //        var content = new MultipartFormDataContent();
    //        var streamContent = new StreamContent(image.OpenReadStream());
    //        content.Add(streamContent, "formFile", image.FileName);
    //        request.Content = content;

    //        await _httpClient.SendAsync(request);
    //    }
    //}
}
