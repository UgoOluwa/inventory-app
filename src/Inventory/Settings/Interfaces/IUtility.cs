using System.Drawing;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Inventory.API.Settings.Interfaces
{
    public interface IUtility
    {
        Task<ObjectId> UploadFile(GridFSBucket fs, string path, string fileName);
        Task<byte[]> DownloadFile(GridFSBucket fs, string fileName);
        IMongoDatabase GetDatabase();
        Task<ObjectId> UploadNewFile(GridFSBucket fs, byte[] image, string fileName);
    }
}