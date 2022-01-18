using System;
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


        public async Task<SingleProductViewModel> CreateProduct(CreateProductDto product)
        {
            try
            {
                var oldProduct = await _repository.FindOneAsync(x => x.Name == product.Name);
                if (oldProduct != null)
                {
                    _logger.LogWarning($"Product with name: {product.Name}, already exists.");
                    return new SingleProductViewModel(){IsSuccessful = true, Message = $"Product with name: {product.Name}, already exists."};
                }

                var newProduct = _mapper.Map<Product>(product);
                newProduct.Id = ObjectId.GenerateNewId();

                if (product.Image != null)
                {
                    var oldImage = await _imageRepository.FindOneAsync(x => x.Name == product.Name);
                    if (oldImage != null)
                    {
                        _logger.LogWarning($"Image with name: {product.Name}, already exists.");
                        return new SingleProductViewModel(){IsSuccessful = true, Message = $"Image with name: {product.Name}, already exists."};
                    }
                    var database =  _utility.GetDatabase();
                    var imageId = await _utility.UploadNewFile(new GridFSBucket(database), product.Image, product.Name);
                    var newImage = new Image()
                    {
                        Id =  ObjectId.GenerateNewId(),
                        DocId = imageId,
                        Name = product.Name
                    };
                    newProduct.ImageId = newImage.Id;
                    await _imageRepository.InsertOneAsync(newImage);
                }
                await _repository.InsertOneAsync(newProduct);
                var productViewModel= await GetProduct(newProduct.Id.ToString());


                return new SingleProductViewModel(){Data = productViewModel.Data, IsSuccessful = true, Message = "successful"};
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return new SingleProductViewModel(){Message = $"{e.Message}"};
            }
        }

        public async Task<SingleProductViewModel> UpdateProduct(UpdateProductDto product)
        {
            try
            {
                var oldProduct = await  _repository.FindByIdAsync(product.Id);
                if (oldProduct == null)
                {
                    _logger.LogError($"Product with id: {product.Id}, hasn't been found in database.");
                    return new SingleProductViewModel(){IsSuccessful = true, Message = $"Product with id: {product.Id}, hasn't been found in database."};
                }

                var newProduct = _mapper.Map<Product>(product);
                var oldImageBytes = await GetImageFile(oldProduct.ImageId);
                newProduct.ImageId = oldProduct.ImageId;

                //check if user is changing image
                if (product.Image != null && product.Image != oldImageBytes)
                {
                    //delete old image
                    await _imageRepository.DeleteByIdAsync(oldProduct.ImageId.ToString());

                    // add new image
                    var database =  _utility.GetDatabase();
                    var imageId = await _utility.UploadNewFile(new GridFSBucket(database), product.Image, product.Name);
                    var newImage = new Image()
                    {
                        Id =  ObjectId.GenerateNewId(),
                        DocId = imageId,
                        Name = product.Name
                    };
                    newProduct.ImageId = newImage.Id;
                    await _imageRepository.InsertOneAsync(newImage);
                }

                await _repository.ReplaceOneAsync(newProduct);
                var productViewModel= await GetProduct(newProduct.Id.ToString());


                return new SingleProductViewModel(){Data = productViewModel.Data, IsSuccessful = true, Message = "successful"};
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return new SingleProductViewModel(){Message = $"{e.Message}"};
            }
        }

        public async Task<MultipleProductViewModel> GetProducts()
        {
            var products = await  _repository.GetAll();

            var productsList = _mapper.Map<IEnumerable<ProductViewModel>>(products);

            IEnumerable<ProductViewModel> productViewModels = productsList.ToList();
            foreach (var product in productViewModels)
            {
                product.Image = await GetImageFile(new ObjectId(product.ImageId));
            }

            return new MultipleProductViewModel(){Data = productViewModels, IsSuccessful = true, Message = "successful"};
        }
        
        public async Task<SingleProductViewModel> GetProduct(string id)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return new SingleProductViewModel(){Message = $"{e.Message}"};
            }
        }

        public async Task<BaseResponse> DeleteProduct(string id)
        {
            try
            {
                var oldProduct = await  _repository.FindByIdAsync(id);
                if (oldProduct == null)
                {
                    _logger.LogError($"Product with id: {id}, hasn't been found in database.");
                    return new BaseResponse(){IsSuccessful = false, Message = $"Product with id: {id}, hasn't been found in database."};
                } 
                await _imageRepository.DeleteByIdAsync(oldProduct.ImageId.ToString());
                await _repository.DeleteByIdAsync(id);

                return new BaseResponse(){IsSuccessful = true, Message = "Operation successful"};
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return new BaseResponse(){IsSuccessful = false, Message = $"{e.Message}"};
            }
        }
        
        public async Task<BaseResponse> DeleteProducts()
        {
            try
            {
                var products = await _repository.GetAll();
                foreach (var product in products)
                {
                    var oldProduct = await  _repository.FindByIdAsync(product.Id.ToString());
                    await _imageRepository.DeleteByIdAsync(oldProduct.ImageId.ToString());
                    await _repository.DeleteByIdAsync(product.Id.ToString());
                }

                await _repository.DeleteManyAsync(x => true);
                await _imageRepository.DeleteManyAsync(x => true);

                return new BaseResponse(){IsSuccessful = true, Message = "Operation successful"};

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return new BaseResponse(){IsSuccessful = false, Message = $"{e.Message}"};
            }
        }

        private async Task<byte[]> GetImageFile(ObjectId docId)
        {
            var imageData = await _imageRepository.FindByIdAsync(docId.ToString());
            if (imageData == null)
            {
                return null;
            }
            var database = _utility.GetDatabase();
            var imageBytes = await _utility.DownloadFile(new GridFSBucket(database), imageData.Name);
            return imageBytes;
        }
    }
}