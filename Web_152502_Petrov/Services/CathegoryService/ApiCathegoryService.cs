//﻿using System.Text.Json;
//using Web_152502_Petrov.Domain.Entities;
//using Web_152502_Petrov.Domain.Models;
//using Web_152502_Petrov.Services.CathegoryService;
using System.Text.Json;
using Web_152502_Petrov.Domain.Entities;
using Web_152502_Petrov.Domain.Models;
using Web_152502_Petrov.Services.DrugService;

//namespace Web_152502_Petrov.Services.CathegoryService;

//public class ApiCathegoryService : ICathegoryService
//{
//    private readonly HttpClient _httpClient;
//    private readonly JsonSerializerOptions _serializerOptions;
//    private readonly ILogger<ApiCathegoryService> _logger;

//    public ApiCathegoryService(
//        HttpClient httpClient,
//        IConfiguration configuration,
//        ILogger<ApiCathegoryService> logger)
//    {
//        _httpClient = httpClient;
//        _serializerOptions = new JsonSerializerOptions()
//        {
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//        };
//        _logger = logger;
//    }
//    public Task<ResponseData<List<Cathegory>>> GetCathegoryListAsync()
//    {
//        throw new NotImplementedException();
//    }
//}

namespace Web_152502_Petrov.Services.CathegoryService;

public class ApiCathegoryService :  ICathegoryService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiDrugService> _logger;

    public ApiCathegoryService(
        HttpClient httpClient,
        ILogger<ApiDrugService> logger)
    {
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }
    public async Task<ResponseData<List<Cathegory>>> GetCathegoryListAsync()
    {
        var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}Cathegories/";

        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var resp = await response
                .Content
                .ReadFromJsonAsync<ResponseData<List<Cathegory>>>
                (_serializerOptions);
                return resp;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return new ResponseData<List<Cathegory>>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка: {ex.Message}"
                };
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
        return new ResponseData<List<Cathegory>>
        {
            Success = false,
            ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
        };
    }
}