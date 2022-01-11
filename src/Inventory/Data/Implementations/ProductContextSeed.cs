using System;
using Inventory.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.API.Data.Interfaces;
using Inventory.API.Settings.Interfaces;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace Inventory.API.Data.Implementations
{
    public class ProductContextSeed : IProductContextSeed
    {
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IUtility _utility;
        private List<Image> imageList;
        private List<Product> productList;
        public ProductContextSeed(IHostEnvironment hostingEnvironment, IUtility utility)
        {
            _hostingEnvironment = hostingEnvironment;
            _utility = utility;
            imageList = new List<Image>();
            productList = new List<Product>();
        }
        public async Task SeedData(IMongoCollection<Product> productCollection, IMongoCollection<Image> imageCollection, IMongoDatabase database)
        {
            bool existProduct = await productCollection.Find(p => true).AnyAsync();
            if (!existProduct)
            {
                await GetPreconfiguredData(database);
                await imageCollection.InsertManyAsync(imageList);
                await productCollection.InsertManyAsync(productList);
            }
        }

        private async Task GetPreconfiguredData(IMongoDatabase database)
        {
            var imageFiles = Directory.GetFiles(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "images/product"));
            Random random = new Random();

            foreach (var imageFile in imageFiles)
            {
                var fileName = Path.GetFileName(imageFile);
                var imageId = await _utility.UploadFile(new GridFSBucket(database), imageFile, fileName);
                var imageName = fileName.Split(".")[0];
                var newImage = new Image()
                {
                    Id =  ObjectId.GenerateNewId(),
                    DocId = imageId,
                    Name = fileName
                };
                imageList.Add(newImage);

                var newProduct = new Product()
                {
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit.",
                    Name = imageName,
                    Price = random.Next(100, 90000),
                    ImageId = newImage.Id
                };

                productList.Add(newProduct);
            }

            /* return new List<Product>()
            {
                new Product()
                {
                    Name = "IPhone X",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-1.png",
                    Price = 950.00M,
                    Category = "Smart Phone"
                },
                new Product()
                {
                    Name = "Samsung 10",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-2.png",
                    Price = 840.00M,
                    Category = "Smart Phone"
                },
                new Product()
                {
                    Name = "Huawei Plus",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-3.png",
                    Price = 650.00M,
                    Category = "White Appliances"
                },
                new Product()
                {
                    Name = "Xiaomi Mi 9",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-4.png",
                    Price = 470.00M,
                    Category = "White Appliances"
                },
                new Product()
                {
                    Name = "HTC U11+ Plus",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-5.png",
                    Price = 380.00M,
                    Category = "Smart Phone"
                },
                new Product()
                {
                    Name = "LG G7 ThinQ",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-6.png",
                    Price = 240.00M,
                    Category = "Home Kitchen"
                }
            }; */
        }
    }
}
