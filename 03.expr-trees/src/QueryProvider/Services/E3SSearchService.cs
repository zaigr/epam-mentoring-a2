using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Expressions.Task3.E3SQueryProvider.Models.Entitites;
using Expressions.Task3.E3SQueryProvider.Models.Response;
using Newtonsoft.Json;
using QueryProvider.Processing.Generator;

namespace QueryProvider.Services
{
    public class E3SSearchService : IE3SSearchService
    {
        private readonly HttpClient _httpClient;

        private readonly IRequestGenerator _requestGenerator;

        public E3SSearchService(HttpClient httpClient, IRequestGenerator requestGenerator)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _requestGenerator = requestGenerator ?? throw new ArgumentNullException(nameof(requestGenerator));
        }

        public IEnumerable<T> SearchFts<T>(string query, int start = 0, int limit = 0) where T : BaseE3SEntity
        {
            Uri request = _requestGenerator.GenerateRequestUrl<T>(query, start, limit);
            string resultString = _httpClient.GetStringAsync(request).Result;

            return JsonConvert.DeserializeObject<FTSResponse<T>>(resultString).Items.Select(t => t.Data);
        }
        
        public IEnumerable SearchFts(Type type, string query, int start = 0, int limit = 0)
        {
            Uri request = _requestGenerator.GenerateRequestUrl(type, query, start, limit);

            string resultString = _httpClient.GetStringAsync(request).Result;
            Type endType = typeof(FTSResponse<>).MakeGenericType(type);
            object result = JsonConvert.DeserializeObject(resultString, endType);

            var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(type)) as IList;

            foreach (object item in (IEnumerable)endType.GetProperty("items").GetValue(result))
            {
                list.Add(item.GetType().GetProperty("data").GetValue(item));
            }

            return list;
        }
    }
}
