using System.Drawing;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace Inventory.API.Settings.Interfaces
{
    public interface IUtility
    {
        Task<ObjectId> UploadFile(GridFSBucket fs, string path, string fileName);
        Task<Image> DownloadFile(GridFSBucket fs, ObjectId id, string fileName);
    }
}