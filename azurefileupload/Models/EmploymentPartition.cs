using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace azurefileupload.Models
{
    public class EmploymentPartition:BaseTable
    {
        public string EmployeeCode { get; set; }
        public string Skill { get; set; }
        public int Experience { get; set; }
        public DateTime DateOfJoining { get; set; }
    }

    public class EmploymentEntity : TableEntity
    {
        public string EmployeeCode { get; set; }
        public string Skill { get; set; }
        public int Experience { get; set; }
        public DateTime DateOfJoining { get; set; }

        public EmploymentEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public EmploymentEntity() { }
    }
}