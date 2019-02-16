using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using azurefileupload.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System.Net.Http.Formatting;

namespace azurefileupload.Controllers
{
    public class TableStorageController : ApiController
    {
        [HttpGet, Route("api/PersonalRecord")]
        public async Task<IHttpActionResult> GetEmployeePersonalDetails()
        {
            //if (!Request.Content.IsMimeMultipartContent("form-data"))
            //{
            //    throw new HttpResponseException(HttpStatusCode.NoContent);
            //}
            string result = string.Empty;

            var accountName = AppConfiguration.StorageAccountName;
            var accountKey = AppConfiguration.StorageAccountKey;

            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);

            try
            {
                CloudTableClient cloundTableClient = storageAccount.CreateCloudTableClient();
                //Get Reference of the table
                CloudTable cloudTable = cloundTableClient.GetTableReference(AppConfiguration.CloudEmployeeTable);

                //Create Table Query
                TableQuery<PersonalEntity> personalPartitionsQuery = new TableQuery<PersonalEntity>().
                    Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "PersonalDetail"));

                var list = cloudTable.ExecuteQuery(personalPartitionsQuery).ToList();

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

        [HttpPost, Route("api/InsertPersonalRecord")]
        public async Task<IHttpActionResult> InsertEmployeePersonalDetails(FormDataCollection formDataCollection)
        {
            if (!Request.Content.IsFormData())
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }
            string result = string.Empty;

            var accountName = AppConfiguration.StorageAccountName;
            var accountKey = AppConfiguration.StorageAccountKey;

            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);

            try
            {
                CloudTableClient cloundTableClient = storageAccount.CreateCloudTableClient();
                //Get Reference of the table
                CloudTable cloudTable = cloundTableClient.GetTableReference(AppConfiguration.CloudEmployeeTable);

                PersonalEntity personalEntity = new PersonalEntity();
                personalEntity.Address = "hello";
                personalEntity.RowKey = "E1002";
                personalEntity.PartitionKey = "PersonalDetail";
                personalEntity.DateOfBirth = Convert.ToDateTime("01/01/1900");

                TableOperation insertTableOperation = TableOperation.Insert(personalEntity);
                cloudTable.Execute(insertTableOperation);

                //Update the data into table
                TableOperation retrieveOperation = TableOperation.Retrieve<PersonalEntity>("PersonalDetail", "E1002");
                TableResult tableResult = cloudTable.Execute(retrieveOperation);

                PersonalEntity personalEntityUpdate = (PersonalEntity)tableResult.Result;
                personalEntityUpdate.Address = "Oxford Street";

                TableOperation tableOperationUpdate = TableOperation.Replace(personalEntityUpdate);
                cloudTable.Execute(tableOperationUpdate);
                //End update
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
