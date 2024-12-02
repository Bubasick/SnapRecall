using Azure.Storage.Blobs;
namespace SnapRecall.Infrastructure
{
    public class StorageService(BlobServiceClient client)
    {
        internal List<string> GetBlobsInContainerAsync(string containerName)
        {
            var containerClient = client.GetBlobContainerClient(containerName);
            var blobs = containerClient.GetBlobs();
            List<string> result = [];
            foreach (var blob in blobs)
            {
                result.Add(blob.Name);
            }
            return result;
        }

        public async Task UploadBlobAsync(string containerName, string key, Stream stream)
        {
            var containerClient = client.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(key);

            await using (stream)
            {
                await blobClient.UploadAsync(stream, true);
            }
        }

        public async Task DeleteBlobAsync(string containerName, string key)
        {
            var containerClient = client.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(key);
            
            await blobClient.DeleteAsync();
        }
    }
}
