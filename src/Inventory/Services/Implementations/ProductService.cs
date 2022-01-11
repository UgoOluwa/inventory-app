using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.API.Entities;
using Inventory.API.Models;
using Inventory.API.Repositories.Interfaces;
using Inventory.API.Settings.Interfaces;
using Inventory.Services.Interfaces;
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

        public ProductService(IRepository<Product> productRepository, IRepository<Image> imageRepository, IUtility utility, IMapper mapper)
        {
            _repository = productRepository;
            _imageRepository = imageRepository;
            _utility = utility;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductViewModel>> GetProducts()
        {
            var products =  _repository.AsQueryable();

            var productsList = _mapper.Map<IEnumerable<ProductViewModel>>(products);

            IEnumerable<ProductViewModel> productViewModels = productsList.ToList();
            foreach (var product in productViewModels)
            {
                product.Image = await GetImageFile(product.ImageId);
            }

            return productViewModels;
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