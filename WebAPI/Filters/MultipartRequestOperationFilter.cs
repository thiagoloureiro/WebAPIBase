using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Description;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Swagger;

#pragma warning disable 1591

namespace WebAPI.Filters
{
    /// <summary>
    ///
    /// </summary>
    public class MultipartRequestOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            SetRequestModelExamples(operation, schemaRegistry, apiDescription);
            SetResponseModelExamples(operation, apiDescription);
        }

        private static void SetRequestModelExamples(Operation operation, SchemaRegistry schemaRegistry,
            ApiDescription apiDescription)
        {
            var requestAttributes = apiDescription.GetControllerAndActionAttributes<SwaggerRequestExample>();
            if (!requestAttributes.Any()) return;

            operation.consumes.Add("multipart/form-data");
            //  var reportProblemSchema = schemaRegistry.GetOrRegister(typeof (ReportProblem));
            operation.parameters = new[]
            {
                new Parameter
                {
                    name = "file",
                    @in = "formData",
                    required = false,
                    type = "file",
                    description = "File to attach"
                },
                new Parameter
                {
                    name = "payload",
                    @in = "formData",
                    required = true,
                    type = "object",
                    //     schema = reportProblemSchema,
                    description = "Other related data"
                }
            };

            foreach (var attr in requestAttributes)
            {
                var schema = schemaRegistry.GetOrRegister(attr.RequestType);

                var parameter = operation.parameters.FirstOrDefault(p => p.type == "object");
                if (parameter == null)
                    continue;

                parameter.schema = schema;

                var provider = (IExamplesProvider)Activator.CreateInstance(attr.ExamplesProviderType);

                var parts = schema.@ref.Split('/');
                var name = parts.Last();

                var definitionToUpdate = schemaRegistry.Definitions[name];

                if (definitionToUpdate != null)
                {
                    definitionToUpdate.example = ((dynamic)FormatAsJson(provider))["application/json"];
                    parameter.@default = ((dynamic)FormatAsJson(provider))["application/json"].ToString();
                }
            }
        }

        private static void SetResponseModelExamples(Operation operation, ApiDescription apiDescription)
        {
            var responseAttributes = apiDescription.GetControllerAndActionAttributes<SwaggerResponseExampleAttribute>();

            foreach (var attr in responseAttributes)
            {
                var statusCode = ((int)attr.StatusCode).ToString();

                var response = operation.responses.FirstOrDefault(r => r.Key == statusCode);

                if (response.Equals(default(KeyValuePair<string, Response>)) == false)
                {
                    if (response.Value != null)
                    {
                        var provider = (IExamplesProvider)Activator.CreateInstance(attr.ExamplesProviderType);
                        response.Value.examples = FormatAsJson(provider);
                    }
                }
            }
        }

        private static object ConvertToCamelCase(Dictionary<string, object> examples)
        {
            var jsonString = JsonConvert.SerializeObject(examples,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return JsonConvert.DeserializeObject(jsonString);
        }

        private static object FormatAsJson(IExamplesProvider provider)
        {
            var examples = new Dictionary<string, object>
            {
                {
                    "application/json", provider.GetExamples()
                }
            };

            return ConvertToCamelCase(examples);
        }

        /// <summary>
        /// Adds example requests to your controller endpoints.
        /// See https://mattfrear.com/2016/01/25/generating-swagger-example-requests-with-swashbuckle/
        /// </summary>
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        public class SwaggerRequestExample : Attribute
        {
            public SwaggerRequestExample(Type requestType, Type examplesProviderType)
            {
                RequestType = requestType;
                ExamplesProviderType = examplesProviderType;
            }

            public Type ExamplesProviderType { get; private set; }

            public Type RequestType { get; private set; }
        }

        /// <summary>
        /// This is used for generating Swagger documentation. Should be used in conjuction with SwaggerResponse - will add examples to SwaggerResponse.
        /// See https://mattfrear.com/2015/04/21/generating-swagger-example-responses-with-swashbuckle/
        /// </summary>
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        public class SwaggerResponseExampleAttribute : Attribute
        {
            public SwaggerResponseExampleAttribute(HttpStatusCode statusCode, Type examplesProviderType)
            {
                StatusCode = statusCode;
                ExamplesProviderType = examplesProviderType;
            }

            public Type ExamplesProviderType { get; }

            public HttpStatusCode StatusCode { get; }
        }

        /// <summary>
        ///
        /// </summary>
        public interface IExamplesProvider
        {
            object GetExamples();
        }
    }
}