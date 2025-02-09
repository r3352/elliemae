// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.BatchPrintLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class BatchPrintLog
  {
    private ArrayList badPrintRecords;
    private ArrayList goodPrintRecords;

    public BatchPrintLog()
    {
      this.badPrintRecords = new ArrayList();
      this.goodPrintRecords = new ArrayList();
    }

    public void AddPrintStatusRecord(BatchPrintRecord printRecord, bool printable)
    {
      if (printable)
        this.goodPrintRecords.Add((object) printRecord);
      else
        this.badPrintRecords.Add((object) printRecord);
    }

    public void RemoveISPrintRecord(Hashtable serviceForms, string loanGuid)
    {
      if (this.goodPrintRecords.Count == 0)
        return;
      int num1 = this.goodPrintRecords.Count - 1;
      for (int index = num1; index >= 0; --index)
      {
        BatchPrintRecord goodPrintRecord = (BatchPrintRecord) this.goodPrintRecords[index];
        string key = goodPrintRecord.FormName;
        int num2 = goodPrintRecord.FormName.LastIndexOf("\\");
        if (num2 > -1)
          key = key.Substring(num2 + 1);
        if (key.ToLower().EndsWith(".docx"))
          key = key.Substring(0, key.Length - 5);
        else if (key.ToLower().EndsWith(".doc"))
          key = key.Substring(0, key.Length - 4);
        if (goodPrintRecord.LoanGUID == loanGuid && serviceForms.ContainsKey((object) key))
          this.goodPrintRecords.RemoveAt(index);
      }
    }

    public Hashtable GetAllPrintRecords()
    {
      Hashtable allPrintRecords = new Hashtable();
      List<string> stringList = new List<string>();
      string empty = string.Empty;
      foreach (BatchPrintRecord badPrintRecord in this.badPrintRecords)
      {
        if (!allPrintRecords.ContainsKey((object) badPrintRecord.LoanGUID))
          allPrintRecords.Add((object) badPrintRecord.LoanGUID, (object) new ArrayList());
        ((ArrayList) allPrintRecords[(object) badPrintRecord.LoanGUID]).Add((object) badPrintRecord);
        string str = badPrintRecord.LoanGUID + badPrintRecord.FormName;
        if (!stringList.Contains(str))
          stringList.Add(str);
      }
      foreach (BatchPrintRecord goodPrintRecord in this.goodPrintRecords)
      {
        string str = goodPrintRecord.LoanGUID + goodPrintRecord.FormName;
        if (!stringList.Contains(str))
        {
          if (!allPrintRecords.ContainsKey((object) goodPrintRecord.LoanGUID))
            allPrintRecords.Add((object) goodPrintRecord.LoanGUID, (object) new ArrayList());
          ((ArrayList) allPrintRecords[(object) goodPrintRecord.LoanGUID]).Add((object) goodPrintRecord);
        }
      }
      return allPrintRecords;
    }

    public BatchPrintRecord[] GetPrintRecord(string guid)
    {
      ArrayList arrayList = new ArrayList();
      foreach (BatchPrintRecord badPrintRecord in this.badPrintRecords)
      {
        if (badPrintRecord.LoanGUID == guid)
          arrayList.Add((object) badPrintRecord);
      }
      return (BatchPrintRecord[]) arrayList.ToArray(typeof (BatchPrintRecord));
    }

    public bool HasBadRecord => this.badPrintRecords.Count > 0;
  }
}
