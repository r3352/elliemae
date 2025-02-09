// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.ReportResults
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  public class ReportResults : MarshalByRefObject, IDisposable
  {
    private const int MaxBytesPerPartialResultSet = 400000;
    private List<string[]> results;
    private string warningMessage;

    public ReportResults() => this.results = new List<string[]>();

    public ReportResults(List<string[]> results) => this.results = results;

    public string GetWarningMessage() => this.warningMessage;

    public void SetWarningMessage(string message) => this.warningMessage = message;

    public void Add(string[] data) => this.results.Add(data);

    public void Insert(int index, string[] data) => this.results.Insert(index, data);

    public List<string[]> GetAllResults() => this.results;

    public List<string[]> GetNextPartialResultSet(int startAt, int maxBytesToReturn)
    {
      if (startAt >= this.results.Count)
        return (List<string[]>) null;
      List<string[]> partialResultSet = new List<string[]>();
      int num = 0;
      for (int index = startAt; index < this.results.Count; ++index)
      {
        int rowSize = ReportResults.getRowSize(this.results[index]);
        if (partialResultSet.Count <= 0 || num + rowSize <= maxBytesToReturn)
        {
          partialResultSet.Add(this.results[index]);
          num += rowSize;
        }
        else
          break;
      }
      return partialResultSet;
    }

    private static int getRowSize(string[] data)
    {
      int rowSize = 0;
      for (int index = 0; index < data.Length; ++index)
      {
        if (!string.IsNullOrEmpty(data[index]))
          rowSize += data[index].Length;
      }
      return rowSize;
    }

    public void Dispose()
    {
      this.results = (List<string[]>) null;
      try
      {
        RemotingServices.Disconnect((MarshalByRefObject) this);
      }
      catch
      {
      }
    }

    public static List<string[]> Download(ReportResults results)
    {
      if (results == null)
        return (List<string[]>) null;
      if (!RemotingServices.IsTransparentProxy((object) results))
        return results.GetAllResults();
      List<string[]> strArrayList = new List<string[]>();
      List<string[]> partialResultSet;
      while ((partialResultSet = results.GetNextPartialResultSet(strArrayList.Count, 400000)) != null)
        strArrayList.AddRange((IEnumerable<string[]>) partialResultSet);
      return strArrayList;
    }
  }
}
