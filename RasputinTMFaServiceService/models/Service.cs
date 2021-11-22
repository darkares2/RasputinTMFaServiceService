
using System;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace Rasputin.TM{
    public class Service : TableEntity {
        public Service(string name)
        {
            this.PartitionKey = "p1";
            this.RowKey = Guid.NewGuid().ToString();
            this.Name = name;
        }
        public Service() { }
        public string Name { get; set; }
        public Guid ServiceID { get { return Guid.Parse(RowKey); }}
        public static explicit operator Service(TableResult v)
        {
            DynamicTableEntity entity = (DynamicTableEntity)v.Result;
            Service ServiceProfile = new Service();
            ServiceProfile.PartitionKey = entity.PartitionKey;
            ServiceProfile.RowKey = entity.RowKey;
            ServiceProfile.Timestamp = entity.Timestamp;
            ServiceProfile.ETag = entity.ETag;
            ServiceProfile.Name = entity.Properties["Name"].StringValue;

            return ServiceProfile;
        }

    }
}