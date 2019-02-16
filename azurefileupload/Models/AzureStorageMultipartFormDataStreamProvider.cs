using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace azurefileupload.Models
{
    public class AzureStorageMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private readonly CloudBlobContainer _blobContainer;
        private readonly string[] _supportedMimeTypes = { "image/png", "image/jpeg", "image/jpg" };

        public AzureStorageMultipartFormDataStreamProvider(CloudBlobContainer blobContainer) : base("azure")
        {
            _blobContainer = blobContainer;
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            if (!_supportedMimeTypes.Contains(headers.ContentType.ToString().ToLower()))
            {
                throw new NotSupportedException("Only jpeg and png are supported");
            }

            // Generate a new filename for every new blob
            var fileName = Guid.NewGuid().ToString();

            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(fileName);

            if (headers.ContentType != null)
            {
                // Set appropriate content type for your uploaded file
                blob.Properties.ContentType = headers.ContentType.MediaType;
            }

            this.FileData.Add(new MultipartFileData(headers, blob.Name));

            return blob.OpenWrite();
        }

        //    public Stream GetStreamforFileShare(HttpContent parent, HttpContentHeaders headers)
        //    {

        //        if (parent == null) throw new ArgumentNullException(nameof(parent));
        //        if (headers == null) throw new ArgumentNullException(nameof(headers));

        //        if (!_supportedMimeTypes.Contains(headers.ContentType.ToString().ToLower()))
        //        {
        //            throw new NotSupportedException("Only jpeg and png are supported");
        //        }

        //        ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
        //        if (contentDisposition != null)
        //        {
        //            // We will post process this as form data  
        //            _isFormData.Add(String.IsNullOrEmpty(contentDisposition.FileName));

        //            return new MemoryStream();
        //        }
        //}
    }
}