using LocalEyesTipApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalEyesTipApp.Interfaces
{
    public interface IRestDataService
    {
        Task<SendTipReturnMessageModel> SendTipAsync(MessageModel messageModel);
    }
}
