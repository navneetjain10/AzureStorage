using azurefileupload.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure;
using System.IO;

namespace azurefileupload.Controllers
{
    
    public class FileShareUploadController : ApiController
    {
        [HttpGet,Route("api/GetfileShare/{fileName}/{extention}")]
        public async Task<IHttpActionResult> GetFileFromFilesStorage(string fileName,string extention)
        {
            string _fileName = fileName + "." + extention;
            string fileresult = string.Empty;

            var accountName = AppConfiguration.StorageAccountName;
            var accountKey = AppConfiguration.StorageAccountKey;
            var shareFile = AppConfiguration.StorageAccountFileShare;
            var filePath = AppConfiguration.LocalDownLoadPath;
            var rootDirectory = AppConfiguration.RootDictionary;

            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            try
            {
                CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
                CloudFileShare share = fileClient.GetShareReference(shareFile);

                if (share.Exists())
                {
                    CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                    CloudFileDirectory sampleDir = rootDir.GetDirectoryReference(rootDirectory);
                    CloudFile file = sampleDir.GetFileReference(_fileName);
                                       
                    if (file.Exists())
                    {
                         file.BeginDownloadToFile(filePath, System.IO.FileMode.Create, null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }

            if (string.IsNullOrEmpty(fileresult))
            {
                return BadRequest("An error has occured while downloading your file content. Please try again.");
            }

            return Ok($"File Content: {fileresult}");

        }

        [HttpGet, Route("api/GetDirectoryData/{dictoryName}")]
        public async Task<IHttpActionResult> GetFileFromFilesStorageDirectory(string dictoryName)
        {
            string fileresult = string.Empty;
            var accountName = AppConfiguration.StorageAccountName;
            var accountKey = AppConfiguration.StorageAccountKey;
            var shareFile = AppConfiguration.StorageAccountFileShare;
            

            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);

            try
            {
                CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
                CloudFileShare share = fileClient.GetShareReference(shareFile);

                if (share.Exists())
                {
                    CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                    CloudFileDirectory sampleDir = rootDir.GetDirectoryReference(dictoryName);

                    if (sampleDir.Exists())
                    {
                        var listofFileandDirectories = sampleDir.ListFilesAndDirectories(null, null);

                        foreach (var clouditem in listofFileandDirectories)
                        {
                            if (clouditem.GetType().Name == "CloudFile")
                            {
                                CloudFile item = (CloudFile)clouditem;
                                fileresult = "Files Found";
                                var filePath = AppConfiguration.LocalDownLoadPath;
                                filePath = filePath + "\\" + item.Name;
                                item.BeginDownloadToFile(filePath, System.IO.FileMode.OpenOrCreate, null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }

            if (string.IsNullOrEmpty(fileresult))
            {
                return BadRequest("An error has occured while downloading your file content. Please try again.");
            }

            return Ok($"Files found: {fileresult}");

        }

        [HttpPost, Route("api/uploadfileShare")]
        public async Task<IHttpActionResult> UploadInSharedFilesStorage()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var accountName = AppConfiguration.StorageAccountName;
            var accountKey = AppConfiguration.StorageAccountKey;
            var shareFile = AppConfiguration.StorageAccountFileShare;
            var rootDirectory = AppConfiguration.RootDictionary;
            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            IList<HttpContent> files = provider.Files;
            HttpContent file1 = files[0];
            Stream stream = await file1.ReadAsStreamAsync();

            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);

            try
            {
                CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
                CloudFileShare share = fileClient.GetShareReference(shareFile);

                if (share.Exists())
                {
                    var fileName = Guid.NewGuid().ToString();
                    // Generate a SAS for a file in the share
                    CloudFileDirectory rootDir = share.GetRootDirectoryReference();
                    CloudFileDirectory sampleDir = rootDir.GetDirectoryReference(rootDirectory);

                    CloudFile cloudFile = sampleDir.GetFileReference(fileName);

                    // Stream fileStream = FileUpload1.PostedFile.InputStream;
                    await cloudFile.UploadFromStreamAsync(stream);
                    //file.UploadFromStream(fileStream);
                   // fileStream.Dispose();

                    //if (file.Exists())
                    //{
                    //    // Write the contents of the file to the console window.
                    //    string result = file.DownloadTextAsync().Result;
                    //}

                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }

            // Retrieve the filename of the file you have uploaded
            // var filename = provider.FileData.FirstOrDefault()?.LocalFileName;
            if (string.IsNullOrEmpty("test.txt"))
            {
                return BadRequest("An error has occured while uploading your file. Please try again.");
            }

            return Ok($"File: {"test.txt"} has successfully uploaded");

        }
    }
}
