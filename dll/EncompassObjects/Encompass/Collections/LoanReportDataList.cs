// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.LoanReportDataList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Reporting;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class LoanReportDataList : ListBase, ILoanReportDataList
  {
    public LoanReportDataList()
      : base(typeof (LoanReportData))
    {
    }

    public LoanReportDataList(IList source)
      : base(typeof (LoanReportData), source)
    {
    }

    public LoanReportData this[int index]
    {
      get => (LoanReportData) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(LoanReportData value) => this.List.Add((object) value);

    public void AddRange(ICollection values)
    {
      foreach (LoanReportData loanReportData in (IEnumerable) values)
        this.Add(loanReportData);
    }

    public bool Contains(LoanReportData value) => this.List.Contains((object) value);

    public int IndexOf(LoanReportData value) => this.List.IndexOf((object) value);

    public void Insert(int index, LoanReportData value) => this.List.Insert(index, (object) value);

    public void Remove(LoanReportData value) => this.List.Remove((object) value);

    public LoanReportData[] ToArray()
    {
      LoanReportData[] array = new LoanReportData[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
