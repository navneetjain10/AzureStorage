using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace azurefileupload.Models
{
    public static class AppConfiguration
    {
        public static string StorageAccountName
        {
            get {
                return ConfigurationManager.AppSettings["storage:account:name"];
            }
        }

        public static string StorageAccountKey
        {
            get
            {
                return ConfigurationManager.AppSettings["storage:account:key"];
            }
        }

        public static string StorageAccountFileShare
        {
            get
            {
                return ConfigurationManager.AppSettings["storage:account:fileshare"];
            }
        }

        public static string LocalDownLoadPath
        {
            get
            {
                return ConfigurationManager.AppSettings["downLoadpath"];
            }
        }

        public static string RootDictionary
        {
            get
            {
                return ConfigurationManager.AppSettings["storage:account:fileshare:directory"];
            }
        }
        public static string CloudEmployeeTable {
            get
            {
                return ConfigurationManager.AppSettings["storage:account:tableName:cloudEmployeeTable"];
            }
        }
        public static string PersonalDetailPartitionName
        {
            get
            {
                return ConfigurationManager.AppSettings["storage:account:tableName:cloudEmployeeTable:PersonalDetails"];
            }
        }

        public static string StorageQueue
        {
            get
            {
                return ConfigurationManager.AppSettings["storage:account:QueueName"];
            }
        }

        

    }
}