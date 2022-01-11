using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Inventory.API.Settings.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using static System.Drawing.Image;

namespace Inventory.API.Settings.Implementations
{
    public class Utility : IUtility
    {
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

            var x = fs.DownloadAsBytesAsync(id);
            Task.WaitAll(x);

            await using var newFs = new FileStream(fileName, FileMode.Create);
            newFs.Write(bytes, 0, bytes.Length);

            return FromStream(newFs);
        }
    }
}