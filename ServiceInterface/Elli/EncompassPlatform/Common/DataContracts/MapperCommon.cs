// Decompiled with JetBrains decompiler
// Type: Elli.EncompassPlatform.Common.DataContracts.MapperCommon
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.EncompassPlatform.Common.DataContracts
{
  public class MapperCommon
  {
    public static string GetBorrowerPairName(string pairId, LoanData loanData)
    {
      BorrowerPair borrowerPair = loanData.GetBorrowerPair(pairId);
      string str = (string) null;
      if (borrowerPair != null)
        str = borrowerPair.ToString();
      return !(pairId != BorrowerPair.All.Id) ? BorrowerPair.All.ToString() : str;
    }

    public static void UpdateStatusInfo(
      bool? sourceStatusFlag,
      DateTime? sourceStatusDate,
      string sourceStatusName,
      DateTime targetStatusDate,
      string targetStatusName,
      MapperCommon.MarkStatusDelegate markMethod,
      MapperCommon.UnMarkStatusDelegate unmarkMethod)
    {
      if (!sourceStatusFlag.HasValue)
        return;
      if (sourceStatusFlag.Value)
      {
        DateTime statusDate = sourceStatusDate ?? targetStatusDate;
        string user = !string.IsNullOrEmpty(sourceStatusName) ? sourceStatusName : targetStatusName;
        markMethod(statusDate, user);
      }
      else
        unmarkMethod();
    }

    public static Dictionary<DocumentLog, string> GetDocumentsWithTitleIndex(
      LogList logList,
      bool checkAccess,
      bool includeDeleted)
    {
      Dictionary<DocumentLog, string> documentsWithTitleIndex = new Dictionary<DocumentLog, string>();
      List<string> stringList = new List<string>();
      DocumentLog[] allDocuments = logList.GetAllDocuments(checkAccess, includeDeleted);
      Array.Sort<DocumentLog>(allDocuments, (Comparison<DocumentLog>) ((x, y) => x.DateAdded.CompareTo(y.DateAdded)));
      foreach (DocumentLog key in allDocuments)
      {
        string indexedStringFromList = MapperCommon.getIndexedStringFromList(stringList, key.Title);
        stringList.Add(indexedStringFromList);
        if (!documentsWithTitleIndex.ContainsKey(key))
          documentsWithTitleIndex.Add(key, indexedStringFromList);
      }
      return documentsWithTitleIndex;
    }

    private static string getIndexedStringFromList(List<string> stringList, string baseString)
    {
      int num = 1;
      string indexedStringFromList;
      for (indexedStringFromList = baseString; stringList.Contains(indexedStringFromList); indexedStringFromList = baseString + " (" + num.ToString() + ")")
        ++num;
      return indexedStringFromList;
    }

    public delegate void MarkStatusDelegate(DateTime statusDate, string user);

    public delegate void UnMarkStatusDelegate();
  }
}
