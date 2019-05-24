using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Azure_Table_Storage_Console
{
  public class TableQueries
  {
    public string _accountKey { get; set; }
    public string _accountName { get; set; }

    public async Task<Boolean> InsertURL(string _hostCode, string sURL, string sShortCode)
    {
      Boolean bSuccess = false;

      CloudStorageAccount storageAccount =
        new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(_accountName, _accountKey),
          true);
      CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

      CloudTable _linkTable = tableClient.GetTableReference("TABLENAME");

      _linkTable.CreateIfNotExists();

      // Create a new customer entity.
      LinkEntity _link = new LinkEntity(_hostCode, sShortCode);

      _link.Raw_URL = sURL;
      _link.Short_Code = sShortCode;

      // Create the TableOperation that inserts the customer entity.
      TableOperation insertOperation = TableOperation.InsertOrMerge(_link);

      try
      {

        await _linkTable.ExecuteAsync(insertOperation);

        bSuccess = true;
      }
      catch (Exception e)
      {
        bSuccess = false;
      }
      return bSuccess;
    }


    public async Task<string> GetLink(string _hostCode, string sShortCode)
    {
      string sResult = "";

      CloudStorageAccount storageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(_accountName, _accountKey), true);
      CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

      CloudTable _linkTable = tableClient.GetTableReference("TABLENAME");

      _linkTable.CreateIfNotExists();

      // Create a retrieve operation that takes a customer entity.
      TableOperation retrieveOperation = TableOperation.Retrieve<LinkEntity>(_hostCode, sShortCode);

      // Execute the retrieve operation.
      TableResult retrievedResult = await _linkTable.ExecuteAsync(retrieveOperation);


      try
      {
        // Print the phone number of the result.
        if (retrievedResult.Result != null)
          sResult = (((LinkEntity)retrievedResult.Result).Raw_URL);
        else
          sResult = "";


      }
      catch (Exception e)
      {
        sResult = "";
      }

      return sResult;
    }

    public async Task<List<LinkSummary>> GetLinks(string _hostCode)
    {
      List<LinkSummary> _records = new List<LinkSummary>();

      CloudStorageAccount storageAccount = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(_accountName, _accountKey), true);
      CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

      CloudTable _linkTable = tableClient.GetTableReference("TABLENAME");

      _linkTable.CreateIfNotExists();

      // Construct the query operation for all customer entities where PartitionKey="Smith".
      TableQuery<LinkEntity> query = new TableQuery<LinkEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, _hostCode));

      // Print the fields for each customer.
      TableContinuationToken token = null;
      do
      {
        TableQuerySegment<LinkEntity> resultSegment = await _linkTable.ExecuteQuerySegmentedAsync(query, token);
        token = resultSegment.ContinuationToken;

        foreach (var entity in resultSegment.Results)
        {
          LinkSummary _summary = new LinkSummary
          {
            Raw_URL = entity.Raw_URL,
            Short_Code = entity.Short_Code
          };

          _records.Add(_summary);
        }
      } while (token != null);


      return _records;
    }
  }
}
