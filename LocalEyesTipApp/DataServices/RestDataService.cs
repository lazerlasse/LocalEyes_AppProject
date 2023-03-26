using LocalEyesTipApp.Interfaces;
using LocalEyesTipApp.Models;

namespace LocalEyesTipApp.DataServices
{
    class RestDataService : IRestDataService
    {
        private readonly HttpClient _httpClient;
        private static readonly string _baseAddress = "https://app.localeyes.dk";

        public RestDataService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_baseAddress)
            };
        }

        public async Task<SendTipReturnMessageModel> SendTipAsync(MessageModel message)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                return new() { Succeded = false, Message = "Der er ikke forbindelse til internettet!" };
            }

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, "api/messageAPI/");

                using var content = new MultipartFormDataContent
                {
                    { new StringContent(message.MessageText), nameof(MessageModel.MessageText) },
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
                    content.Add(new StringContent(string.Empty), nameof(MessageModel.ReplyMail));
                }
                else
                {
                    content.Add(new StringContent(message.ReplyMail), nameof(MessageModel.ReplyMail));
                }

                // Check if media file is null.
                if (message.MediaFiles != null)
                {
                    foreach (var file in message.MediaFiles)
                    {
                        var streamContent = new StreamContent(await file.OpenReadAsync());
                        content.Add(streamContent, nameof(MessageModel.MediaFiles), file.FileName);
                    }
                }

                request.Content = content;

                using var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new() { Succeded = true, Message = $"Beskeden blev sendt: {response.Headers}" };
                }
                else
                {
                    return new() { Succeded = false, Message = $"Der opstod en fejl: {response.Headers}" };
                }
            }
            catch (Exception ex)
            {
                return new() { Succeded = false, Message = $"Der opstod en uventet fejl: {ex.Message}" };
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
