using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.DTO.ViewModels;

namespace CefClient.WebServices.Models
{
    public interface IBrowserDataCollector
    {

    }
    public class ClientDataCollector
    {
        private List<object> _collection { get; set; } = new List<object>();
        private IApiService _apiService { get; set; }
        public ClientDataCollector(IApiService service)
        {
            _apiService = service;
        }
        public void AddMany(string inputStr)
        {
            if (!string.IsNullOrEmpty(inputStr))
            {
                var result = _apiService.TransformResponseString<IEnumerable<object>>(inputStr);
                if (result != null)
                    _collection.AddRange(result);

            }
        }
        public void AddOne(string inputStr)
        {
            var result = _apiService.TransformResponseString<object>(inputStr);
            if (result != null)
                _collection.Add(result);
        }
        public void ReplaceMany(string inputStr)
        {
            _collection.Clear();
            AddMany(inputStr);
        }
        public void ReplaceOne(string inputStr)
        {
            _collection.Clear();
            AddOne(inputStr);
        }
        public IEnumerable<TEntity> GetCollection<TEntity>()
        {
            return _collection.Cast<TEntity>();
        }
    }
}
