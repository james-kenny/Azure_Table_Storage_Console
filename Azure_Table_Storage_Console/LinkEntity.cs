using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Azure_Table_Storage_Console
{
  public class LinkEntity : TableEntity
  {
    public LinkEntity(string hostcode, string shortcode)
    {
      this.PartitionKey = hostcode;
      this.RowKey = shortcode;
    }


    public LinkEntity() { }

    public string Short_Code { get; set; }

    public string Raw_URL { get; set; }

  }
}
