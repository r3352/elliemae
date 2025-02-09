// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.StackingOrderComparer
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  public class StackingOrderComparer : IComparer
  {
    private StackingOrderSetTemplate template;
    private ArrayList pairList = new ArrayList();
    private ArrayList vodList = new ArrayList();
    private ArrayList voeList = new ArrayList();
    private ArrayList volList = new ArrayList();
    private ArrayList vomList = new ArrayList();
    private ArrayList vorList = new ArrayList();

    public StackingOrderComparer(LoanData loanData, StackingOrderSetTemplate template)
    {
      this.template = template;
      BorrowerPair currentBorrowerPair = loanData.CurrentBorrowerPair;
      foreach (BorrowerPair borrowerPair in loanData.GetBorrowerPairs())
      {
        loanData.SetBorrowerPair(borrowerPair);
        this.pairList.Add((object) loanData.PairId);
        for (int index = 1; index <= loanData.GetNumberOfDeposits(); ++index)
          this.vodList.Add((object) loanData.GetSimpleField("DD" + index.ToString("00") + "35"));
        ArrayList arrayList = new ArrayList();
        for (int index = 1; index <= loanData.GetNumberOfEmployer(true); ++index)
        {
          string simpleField = loanData.GetSimpleField("BE" + index.ToString("00") + "99");
          if (loanData.GetSimpleField("BE" + index.ToString("00") + "09") == "Y")
            this.voeList.Add((object) simpleField);
          else
            arrayList.Add((object) simpleField);
        }
        foreach (string str in arrayList)
          this.voeList.Add((object) str);
        arrayList.Clear();
        for (int index = 1; index <= loanData.GetNumberOfEmployer(false); ++index)
        {
          string simpleField = loanData.GetSimpleField("CE" + index.ToString("00") + "99");
          if (loanData.GetSimpleField("CE" + index.ToString("00") + "09") == "Y")
            this.voeList.Add((object) simpleField);
          else
            arrayList.Add((object) simpleField);
        }
        foreach (string str in arrayList)
          this.voeList.Add((object) str);
        for (int index = 1; index <= loanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp(); ++index)
          this.volList.Add((object) loanData.GetSimpleField("FL" + index.ToString("00") + "99"));
        for (int index = 1; index <= loanData.GetNumberOfMortgages(); ++index)
          this.vomList.Add((object) loanData.GetSimpleField("FM" + index.ToString("00") + "43"));
        arrayList.Clear();
        for (int index = 1; index <= loanData.GetNumberOfResidence(true); ++index)
        {
          string simpleField = loanData.GetSimpleField("BR" + index.ToString("00") + "99");
          if (loanData.GetSimpleField("BR" + index.ToString("00") + "23") == "Current")
            this.vorList.Add((object) simpleField);
          else
            arrayList.Add((object) simpleField);
        }
        foreach (string str in arrayList)
          this.vorList.Add((object) str);
        arrayList.Clear();
        for (int index = 1; index <= loanData.GetNumberOfResidence(false); ++index)
        {
          string simpleField = loanData.GetSimpleField("CR" + index.ToString("00") + "99");
          if (loanData.GetSimpleField("CR" + index.ToString("00") + "23") == "Current")
            this.vorList.Add((object) simpleField);
          else
            arrayList.Add((object) simpleField);
        }
        foreach (string str in arrayList)
          this.vorList.Add((object) str);
      }
      loanData.SetBorrowerPair(currentBorrowerPair);
    }

    public int Compare(object x, object y)
    {
      DocumentLog documentLog1 = (DocumentLog) x;
      DocumentLog documentLog2 = (DocumentLog) y;
      if (documentLog1 == null || documentLog2 == null)
        return 0;
      string lower1 = documentLog1.Title.ToLower();
      string lower2 = documentLog1.GroupName.ToLower();
      string lower3 = documentLog2.Title.ToLower();
      string lower4 = documentLog2.GroupName.ToLower();
      int num1 = -1;
      int num2 = -1;
      for (int index = 0; index < this.template.DocNames.Count; ++index)
      {
        if (this.template.DocNames[index] is string docName)
        {
          if (this.template.NDEDocGroups.Contains((object) docName))
          {
            string lower5 = docName.ToLower();
            if (lower2 == lower5)
              num1 = index;
            if (lower4 == lower5)
              num2 = index;
          }
          else
          {
            string lower6 = docName.ToLower();
            if (lower1 == lower6)
              num1 = index;
            if (lower3 == lower6)
              num2 = index;
          }
          if (num1 >= 0 && num2 >= 0)
            break;
        }
      }
      int num3 = num1.CompareTo(num2);
      if (num3 != 0)
        return num1 == -1 || num2 == -1 ? num3 * -1 : num3;
      int num4 = documentLog1.Title.CompareTo(documentLog2.Title);
      if (num4 != 0)
        return num4;
      int num5 = this.pairList.IndexOf((object) documentLog1.PairId);
      int num6 = this.pairList.IndexOf((object) documentLog2.PairId);
      int num7 = num5.CompareTo(num6);
      if (num7 != 0)
        return num7;
      if (documentLog1 is VerifLog && documentLog2 is VerifLog)
      {
        VerifLog verifLog1 = (VerifLog) documentLog1;
        VerifLog verifLog2 = (VerifLog) documentLog2;
        switch (documentLog1.Title)
        {
          case "VOD":
            num5 = this.vodList.IndexOf((object) verifLog1.Id);
            num6 = this.vodList.IndexOf((object) verifLog2.Id);
            break;
          case "VOE":
            num5 = this.voeList.IndexOf((object) verifLog1.Id);
            num6 = this.voeList.IndexOf((object) verifLog2.Id);
            break;
          case "VOL":
            num5 = this.volList.IndexOf((object) verifLog1.Id);
            num6 = this.volList.IndexOf((object) verifLog2.Id);
            break;
          case "VOM":
            num5 = this.vomList.IndexOf((object) verifLog1.Id);
            num6 = this.vomList.IndexOf((object) verifLog2.Id);
            break;
          case "VOR":
            num5 = this.vorList.IndexOf((object) verifLog1.Id);
            num6 = this.vorList.IndexOf((object) verifLog2.Id);
            break;
        }
        int num8 = num5.CompareTo(num6);
        if (num8 != 0)
          return num8;
      }
      return documentLog1.Date.CompareTo(documentLog2.Date);
    }
  }
}
