using azurefileupload.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace azurefileupload.Controllers
{
    public class QueueStorageController : ApiController
    {
        [HttpGet, Route("api/GetQueue")]
        public async Task<IHttpActionResult> GetQueueDetails()
        {
            string result = string.Empty;

            var accountName = AppConfiguration.StorageAccountName;
            var accountKey = AppConfiguration.StorageAccountKey;

            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);

            try
            {
                CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(AppConfiguration.StorageQueue);

                var message = await cloudQueue.GetMessageAsync();
                result = message.AsString;
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }

            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("An error has occured while inserting data. Please try again.");
            }
            else
            {
                return Ok($"Message = " + result);
            }



        }

        [HttpPost, Route("api/CreateQueue")]
        public async Task<IHttpActionResult> CreateQueueData(string message)
        {
            var result = string.Empty;

            var accountName = AppConfiguration.StorageAccountName;
            var accountKey = AppConfiguration.StorageAccountKey;

            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);

            try
            {
                CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(AppConfiguration.StorageQueue);

                await cloudQueue.CreateIfNotExistsAsync();

                CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(message);
                 await cloudQueue.AddMessageAsync(cloudQueueMessage);
                return Ok($"Data inserted successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }

            //if (string.IsNullOrEmpty(result))
            //{
            //    return BadRequest("An error has occured while inserting data. Please try again.");
            //}
        }

        [HttpPost, Route("api/UpdateQueue")]
        public async Task<IHttpActionResult> UpdateQueueData(string message)
        {
            string result = string.Empty;

            var accountName = AppConfiguration.StorageAccountName;
            var accountKey = AppConfiguration.StorageAccountKey;

            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            QueueRequestOptions queueRequestOptions = new QueueRequestOptions();
            try
            {
                CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(AppConfiguration.StorageQueue);

                var msg =  cloudQueue.GetMessage(new TimeSpan(0, 1, 0));
                result = msg.AsString;
                msg.SetMessageContent(message);
                await cloudQueue.UpdateMessageAsync(msg, new TimeSpan(0, 1, 0), MessageUpdateFields.Content | MessageUpdateFields.Visibility);

            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }

            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("An error has occured while inserting data. Please try again.");
            }

            return Ok($"Data inserted successfully!");
        }
    }
}
