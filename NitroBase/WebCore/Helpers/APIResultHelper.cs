using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebCore.Helpers
{
    public class APIResultHelper
    {
        /// <summary>
        /// Use this method for returning a common message signature in Apis
        /// </summary>
        /// <seealso cref="ControllerBase RestResultBody(string, List{string})"/>
        /// <seealso cref="PaymentGatewayMicro.Infrastructure.Dtos.HttpStatusMessageDto"/>
        /// <param name="message">message</param>
        /// <param name="parameters">list of parameters (e.g. the invalid parameter)</param>
        /// <returns></returns>
        public static object RestResultBody(string message, IEnumerable<string> parameters = null)
        {
            return new Dtos.NotOkResultDto(message, parameters);
        }
        public static object RestResultBody(string message, Dictionary<string, string> parameters = null)
        {
            List<string> parametersList = new List<string>();
            foreach (var parameter in parameters)
                parametersList.Add(StringifyParameter(parameter.Key, parameter.Value));

            return new Dtos.NotOkResultDto(message, parametersList);
        }
        public static object RestResultBody(string message, params string[] parameters)
        {
            return new Dtos.NotOkResultDto(message, parameters);
        }

        public static string StringifyParameter(string key, object value) =>
                       $"{key}: {value}";

        public static string CreateErrorMessage(string url, HttpStatusCode httpStatusCode) =>
            $"An error occurred in api call with url: {url}. HttpStatusCode: {httpStatusCode}";
    }
}
