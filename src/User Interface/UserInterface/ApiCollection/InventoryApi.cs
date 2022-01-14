using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserInterface.ApiCollection.Infrastructure;
using UserInterface.ApiCollection.Interfaces;
using UserInterface.Models;
using UserInterface.Settings;

namespace UserInterface.ApiCollection
{
    public class InventoryApi : BaseHttpClientWithFactory, IInventoryApi
    {
        private readonly IApiSettings _settings;

        public InventoryApi(IHttpClientFactory factory, IApiSettings settings) : base(factory)
        {
            _settings = settings;
        }

        public async Task<MultipleProductViewModel> GetProducts()
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                               .SetPath(_settings.InventoryPath)
                               .HttpMethod(HttpMethod.Get)
                               .GetHttpMessage();

            return await SendRequest<MultipleProductViewModel>(message);
        }

        public async Task<SingleProductViewModel> GetProduct(string id)
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                               .SetPath(_settings.InventoryPath)
                               .AddToPath(id)
                               .HttpMethod(HttpMethod.Get)
                               .GetHttpMessage();

            return await SendRequest<SingleProductViewModel>(message);
        }
        
        public async Task<SingleProductViewModel> CreateProduct(CreateProductDto productModel)
        {
            var message = new HttpRequestBuilder(_settings.BaseAddress)
                                .SetPath(_settings.InventoryPath)
                                .HttpMethod(HttpMethod.Post)
                                .GetHttpMessage();

            var json = JsonConvert.SerializeObject(productModel);
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return await SendRequest<SingleProductViewModel>(message);
        }
    }
}
