using AutoMapper;
using WebCore.Dtos;
using Core.Helpers;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Filters;
using WebCore.Helpers;
using Microsoft.AspNetCore.Http;
using Core.Constants;
using Core.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Core;
using WebCore.Attributes;

namespace WebCore.Controllers
{
    [ApiController]
    [ValidateModel]
    [ApiExplorerSettings(IgnoreApi = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(NotOkResultDto), StatusCodes.Status409Conflict)]

    public class BaseController<TController, TApplicationSetting> : ControllerBase where TApplicationSetting : ApplicationSettingModel, new()
    {
        protected readonly ILogger<TController> _logger;
        protected readonly IMapper _mapper;
        protected readonly TApplicationSetting _configuration;

        public BaseController()
        {
            _configuration = ServiceLocator.GetService<TApplicationSetting>();
            _logger = ServiceLocator.GetService<ILogger<TController>>();
            _mapper = ServiceLocator.GetService<IMapper>();

        }


        /// <summary>
        /// Use this method for returning a common message signature in Apis
        /// </summary>
        /// <seealso cref="RestResultBody(string, string[])"/>
        /// <seealso cref="PaymentGatewayMicro.Infrastructure.Dtos.HttpStatusMessageDto"/>
        /// <seealso cref="PaymentGatewayMicro.Infrastructure.Helpers.RestHelper.RestResultBody(string, IEnumerable{string})"/>
        /// <param name="message">message</param>
        /// <param name="parameters">list of parameters (e.g. the invalid parameter)</param>
        /// <returns></returns>
        protected object RestResultBody(string message, IEnumerable<object> parameters = null) =>
                  APIResultHelper.RestResultBody(message, parameters
                                 .Select(e => APIResultHelper.StringifyParameter(nameof(e), e)));

        /// <summary>
        /// Use this method for returning a common message signature in Apis
        /// </summary>
        /// <seealso cref="RestResultBody(string, IEnumerable{string})"/>
        /// <seealso cref="PaymentGatewayMicro.Infrastructure.Dtos.HttpStatusMessageDto"/>
        /// <seealso cref="PaymentGatewayMicro.Infrastructure.Helpers.RestHelper.RestResultBody(string, IEnumerable{string})"/>
        /// <param name="message">message</param>
        /// <param name="parameters">list of parameters (e.g. the invalid parameter)</param>
        /// <returns></returns>
        protected object RestResultBody(string message, params object[] parameters) =>
                  APIResultHelper.RestResultBody(message, new List<object>(parameters)
                                 .Select(e => APIResultHelper.StringifyParameter(nameof(e), e)));
        protected object RestResultBody(string message, params Param[] parameters) =>
                  APIResultHelper.RestResultBody(message, new List<string>(parameters
                                 .Select(e => APIResultHelper.StringifyParameter(e.Key, e.Value))));

        //public override NotFoundObjectResult NotFound(object value) =>
        //       base.NotFound(RestResultBody("Resource not found", _reflectionHelper.GetParamNames(value).ToArray()));

        /// <summary>
        /// Do not use this method. This method has been restricted in order to keep the output in standard format
        /// </summary>
        /// <returns>throw NotImplementedException</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override ConflictResult Conflict() =>
               throw new NotImplementedException("This method is not allowed");
        /// <summary>
        /// Do not use this method. This method has been restricted in order to keep the output in standard format
        /// </summary>
        /// <returns>throw NotImplementedException</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override ConflictObjectResult Conflict([ActionResultObjectValue] ModelStateDictionary modelState) =>
                              throw new NotImplementedException("This method is not allowed");
        public override ConflictObjectResult Conflict([ActionResultObjectValue] object error) =>
               base.Conflict(RestResultBody(error.ToJsonString()));

        /// <summary>
        /// Do not use this method. This method has been restricted in order to keep the output in standard format
        /// </summary>
        /// <returns>throw NotImplementedException</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override NotFoundResult NotFound() =>
               throw new NotImplementedException("This method is not allowed");
        public override NotFoundObjectResult NotFound([ActionResultObjectValue] object value) =>
               base.NotFound(RestResultBody(value.ToJsonString()));
        protected NotFoundObjectResult NotFound(string message, IEnumerable<string> parameters = null) =>
               base.NotFound(RestResultBody(message, parameters));
        protected NotFoundObjectResult NotFound(string message, params object[] parameters) =>
                  base.NotFound(RestResultBody(message, parameters));
        protected NotFoundObjectResult NotFound(string message, params string[] parameters) =>
               base.NotFound(RestResultBody(message, parameters));
        protected NotFoundObjectResult NotFound(string message, params Param[] parameters) =>
               base.NotFound(RestResultBody(message, parameters));

        /// <summary>
        /// Do not use this method. This method has been restricted in order to keep the output in standard format
        /// </summary>
        /// <returns>throw NotImplementedException</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override BadRequestResult BadRequest() =>
               throw new NotImplementedException("This method is not allowed");
        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] ModelStateDictionary modelState) =>
               throw new NotImplementedException("This method is not allowed");

        //public override BadRequestObjectResult BadRequest(object value) =>
        //       base.BadRequest(RestResultBody("Request Parameters are invalid", _reflectionHelper.GetParamNames(value).ToArray()));
        public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object error) =>
               base.BadRequest(RestResultBody(error.ToJsonString()));
        protected BadRequestObjectResult BadRequest(string message, IEnumerable<object> parameters = null) =>
               base.BadRequest(RestResultBody(message, parameters));
        protected BadRequestObjectResult BadRequest(string message, params object[] parameters) =>
             base.BadRequest(RestResultBody(message, parameters));
        protected BadRequestObjectResult BadRequest(string message, params Param[] parameters) =>
             base.BadRequest(RestResultBody(message, parameters));

        //public override UnauthorizedObjectResult Unauthorized(object value) =>
        //       base.Unauthorized(RestResultBody("Request is not authorized", _reflectionHelper.GetParamNames(value).ToArray()));

        /// <summary>
        /// Do not use this method. This method has been restricted in order to keep the output in standard format
        /// </summary>
        /// <returns>throw NotImplementedException</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override UnauthorizedResult Unauthorized() =>
               throw new NotImplementedException("This method is not allowed");
        public override UnauthorizedObjectResult Unauthorized([ActionResultObjectValue] object value) =>
               base.Unauthorized(RestResultBody(value.ToJsonString()));
        protected UnauthorizedObjectResult Unauthorized(string message, IEnumerable<string> parameters = null) =>
               base.Unauthorized(RestResultBody(message, parameters));
        protected UnauthorizedObjectResult Unauthorized(string message, params object[] parameters) =>
                  base.Unauthorized(RestResultBody(message, parameters));
        protected UnauthorizedObjectResult Unauthorized(string message, params string[] parameters) =>
               base.Unauthorized(RestResultBody(message, parameters));
        protected UnauthorizedObjectResult Unauthorized(string message, params Param[] parameters) =>
            base.Unauthorized(RestResultBody(message, parameters));

        //public override ObjectResult StatusCode([ActionResultStatusCode] int statusCode, object value) =>
        //       base.StatusCode(statusCode, RestResultBody("Request is not authorized", _reflectionHelper.GetParamNames(value).ToArray()));

        /// <summary>
        /// Do not use this method. This method has been restricted in order to keep the output in standard format
        /// </summary>
        /// <returns>throw NotImplementedException</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override StatusCodeResult StatusCode([ActionResultStatusCode] int statusCode) =>
               throw new NotImplementedException("This method is not allowed");

        public override ObjectResult StatusCode([ActionResultStatusCode] int statusCode, [ActionResultObjectValue] object value) =>
               base.StatusCode(statusCode, RestResultBody(value.ToJsonString()));
        protected ObjectResult StatusCode([ActionResultStatusCode] int statusCode, string message, IEnumerable<string> parameters = null) =>
               base.StatusCode(statusCode, RestResultBody(message, parameters));
        protected ObjectResult StatusCode([ActionResultStatusCode] int statusCode, string message, params object[] parameters) =>
             base.StatusCode(statusCode, RestResultBody(message, parameters));
        protected ObjectResult StatusCode([ActionResultStatusCode] int statusCode, string message, params string[] parameters) =>
               base.StatusCode(statusCode, RestResultBody(message, parameters));

        public OkObjectResult GetOkResult<T>(T value, int count = 0, int pageNumber = 1, int pageSize = GlobalConstants.DefaultPageSize) where T : class
        {
            var template = new OkResult<T>(value);
            
            return Ok(template); 
        }

        public OkObjectResult GetOkResult<T>(ICollection<T> value, int pageNumber, int pageSize, int totalItems, int totalPages) where T : class
        {

            return GetOkResult(new PaginatedList<T>()
            {
                Items = value,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            });
        }

        public OkObjectResult GetOkResult<T>(PaginatedList<T> paginatedResult) where T : class
        {
            var template = new OkListResult<T>(paginatedResult.Items);
            template.PageNumber = paginatedResult.PageNumber;
            template.PageSize = paginatedResult.PageSize;
            template.TotalPages = paginatedResult.TotalPages;
            template.TotalItems = paginatedResult.TotalItems;
            
            return Ok(template);
        }

        public class Param
        {
            public Param(string key, object value)
            {
                Key = key;
                Value = value;
            }

            public string Key { get; set; }
            public object Value { get; set; }
        }


    }
}
