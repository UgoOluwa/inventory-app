using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Inventory.API.Settings.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using static System.Drawing.Image;

namespace Inventory.API.Settings.Implementations
{
    public class Utility : IUtility
    {
        private readonly IProductDatabaseSettings _settings;

        public Utility(IProductDatabaseSettings settings)
        {
            _settings = settings;
        }

        public async Task<ObjectId> UploadFile(GridFSBucket fs, string path, string fileName)
        {
            await using var stream = File.OpenRead(path);
            var t =  await Task.Run<ObjectId>(() => fs.UploadFromStreamAsync(fileName, stream));

            return t;
        }

        public async Task<Image> DownloadFile(GridFSBucket fs, ObjectId id, string fileName)
        {
            var t = fs.DownloadAsBytesByNameAsync(fileName);
            Task.WaitAll(t);
            var bytes =  await t;
            
            await using var newFs = new FileStream(fileName, FileMode.Create);
            newFs.Write(bytes, 0, bytes.Length);

            return FromStream(newFs);
        }

        public IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);

            return database;
        }
    }
}