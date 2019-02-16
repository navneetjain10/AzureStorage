using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace azurefileupload.Models
{
    public class BaseTable
    {
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
    }
}