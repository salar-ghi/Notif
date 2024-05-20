using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.ThirdParties
{
    internal class PayamSms : ISmsProvider
    {


        public Task<bool> SendAsync(string ProviderName, Notif message)
        {
            throw new NotImplementedException();
        }
    }
}
