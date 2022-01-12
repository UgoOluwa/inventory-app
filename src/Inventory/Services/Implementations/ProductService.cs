using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.API.Entities;
using Inventory.API.Models;
using Inventory.API.Repositories.Interfaces;
using Inventory.API.Settings.Interfaces;
using Inventory.Services.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace Inventory.API.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly IRepository<Image> _imageRepository;
        private readonly IUtility _utility;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IRepository<Product> productRepository, IRepository<Image> imageRepository, IUtility utility, IMapper mapper,
            ILogger<ProductService> logger)
        {
            _repository = productRepository;
            _imageRepository = imageRepository;
            _utility = utility;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<SingleProductViewModel> CreateProduct(ProductViewModel product)
        {
            var oldProduct = await _repository.FindOneAsync(x => x.Name == product.Name);
            if (oldProduct != null)
            {
                _logger.LogError($"Product with name: {product.Name}, already exists.");
                return new SingleProductViewModel(){IsSuccessful = true, Message = $"Product with name: {product.Name}, already exists."};
            }

            var newProduct = _mapper.Map<Product>(product);
            newProduct.Id = ObjectId.GenerateNewId();
            await _repository.InsertOneAsync(newProduct);
            var productViewModel= await GetProduct(newProduct.Id.ToString());


            return new SingleProductViewModel(){Data = productViewModel.Data, IsSuccessful = true, Message = "successful"};
        }


        public async Task<SingleProductViewModel> UpdateProduct(ProductViewModel product)
        {
            var oldProduct = await  _repository.FindByIdAsync(product.Id);
            if (oldProduct == null)
            {
                _logger.LogError($"Product with id: {product.Id}, hasn't been found in database.");
                return new SingleProductViewModel(){IsSuccessful = true, Message = $"Product with id: {product.Id}, hasn't been found in database."};
            }

            var newProduct = _mapper.Map<Product>(product);

            await _repository.ReplaceOneAsync(newProduct);

            return new SingleProductViewModel(){Data = product, IsSuccessful = true, Message = "successful"};
        }

        public async Task<MultipleProductViewModel> GetProducts()
        {
            var products = await  _repository.GetAll();

            var productsList = _mapper.Map<IEnumerable<ProductViewModel>>(products);

            IEnumerable<ProductViewModel> productViewModels = productsList.ToList();
            foreach (var product in productViewModels)
            {
                product.Image = await GetImageFile(product.ImageId);
            }

            return new MultipleProductViewModel(){Data = productViewModels, IsSuccessful = true, Message = "successful"};
        }
        
        public async Task<SingleProductViewModel> GetProduct(string id)
        {
            var product = await  _repository.FindByIdAsync(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, hasn't been found in database.");
                return new SingleProductViewModel(){IsSuccessful = true, Message = $"Product with id: {id}, hasn't been found in database."};
            }

            var productViewModel = _mapper.Map<ProductViewModel>(product);
            productViewModel.Image = await GetImageFile(product.ImageId);
            

            return new SingleProductViewModel(){Data = productViewModel, IsSuccessful = true, Message = "successful"};
        }

        public async Task DeleteProduct(string id)
        {
           await _repository.DeleteByIdAsync(id);
        }
        
        public async Task DeleteProducts()
        {
           await _repository.DeleteManyAsync(x => true);
        }

        private async Task<System.Drawing.Image> GetImageFile(ObjectId docId)
        {
            var imageData = await _imageRepository.FindByIdAsync(docId.ToString());
            if (imageData == null)
            {
                return null;
            }
            var database = _utility.GetDatabase();
            var imageFile = await _utility.DownloadFile(new GridFSBucket(database), docId, imageData.Name);
            return imageFile;
        }
    }
}