using LocalEyesTipApp.Interfaces;
using LocalEyesTipApp.Models;
using LocalEyesTipApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace LocalEyesTipApp.DataServices
{
    class RestDataService : IRestDataService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private static readonly string _baseAddress = "https://app.localeyes.dk";
        private static readonly string _messageAPIUrl = $"{_baseAddress}/api/messageAPI/";

        public RestDataService()
        {
#if DEBUG
            HttpsClientHandlerService handler = new HttpsClientHandlerService();
            _httpClient = new HttpClient(handler.GetPlatformMessageHandler());
            _httpClient.BaseAddress = new Uri(_baseAddress);
#else
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseAddress);
#endif

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<SendTipReturnMessageModel> SendTipAsync(MessageModel message)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("------> Ingen internet adgang...");
                return new() { Succeded = false, Message = "Der er ikke forbindelse til internettet!" };
            }

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, "api/messageAPI/");

                using var content = new MultipartFormDataContent
                {
                    { new StringContent(message.Title), nameof(MessageModel.Title) },
                    { new StringContent(message.Description), nameof(MessageModel.Description) },
                    { new StringContent(message.Address) , nameof(MessageModel.Address) }
                };


                // Check if phonenumber is null.
                if (message.ReplyPhoneNumber == null)
                {
                    content.Add(new StringContent("00000000"), nameof(MessageModel.ReplyPhoneNumber));
                }
                else
                {
                    content.Add(new StringContent(message.ReplyPhoneNumber.ToString()), nameof(MessageModel.ReplyPhoneNumber));
                }



                // Check if mail is null.
                if (message.ReplyMail == null)
                {
                    content.Add(new StringContent(""), nameof(MessageModel.ReplyMail));
                }
                else
                {
                    content.Add(new StringContent(message.ReplyMail), nameof(MessageModel.ReplyMail));
                }



                // Check if media file is null.
                if (message.MediaFile != null)
                {
                    content.Add(new StreamContent(await message.MediaFile.OpenReadAsync()), nameof(MessageModel.MediaFile), message.MediaFile.FileName);
                }

                request.Content = content;

                var result = await _httpClient.SendAsync(request);

                if (result.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Beskeden blev sendt: {result.Headers}");
                    return new() { Succeded = true, Message = $"Beskeden blev sendt: {result.Headers}" };
                }
                else
                {
                    Debug.WriteLine($"Der opstod en fejl: {result.Headers}");
                    return new() { Succeded = false, Message = $"Der opstod en fejl: {result.Headers}" };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Der opstod en uventet fejl: {ex.Message}");
                return new() { Succeded = false, Message = $"Der opstod en uventet fejl: {ex.Message}" };
            }
        }
    }
}
