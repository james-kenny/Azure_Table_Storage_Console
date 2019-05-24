using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Table_Storage_Console
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Starting");


      string _accountKey = "ACCOUNTKEY";
      string _accountName = "ACCOUNTNAME";

      TableQueries _tableQueries = new TableQueries
      {
        _accountKey = _accountKey,
        _accountName = _accountName
      };


      string _host = "ABCD1234";
      string _shortCode = "abc2";

      string _rawURL = "https://serversncode.com";

      Task<Boolean> bLinkCreated = _tableQueries.InsertURL(_host, _rawURL, _shortCode);

      bLinkCreated.Wait();

      // Get Link
      Task<string> _task = _tableQueries.GetLink(_host, _shortCode);

      _task.Wait();

      // Get All Links
      Task<List<LinkSummary>> _linkSummaries = _tableQueries.GetLinks(_host);

      _linkSummaries.Wait();


      foreach (var oLinkSummary in _linkSummaries.Result)
      {
        Console.WriteLine(oLinkSummary.Raw_URL + " Short: " + oLinkSummary.Short_Code);
      }



      Console.WriteLine("Complete");

    }
  }
}
