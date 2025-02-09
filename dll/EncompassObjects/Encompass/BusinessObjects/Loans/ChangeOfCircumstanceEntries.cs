// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ChangeOfCircumstanceEntries
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class ChangeOfCircumstanceEntries : IChangeOfCircumstanceEntries
  {
    private Loan loan;

    internal ChangeOfCircumstanceEntries(Loan loan) => this.loan = loan;

    public int Count => this.loan.LoanData.GetNumberOfGoodFaithChangeOfCircumstance();

    public string[] GetAlertFieldIDs()
    {
      Hashtable varianceAlertDetails = this.loan.LoanData.Calculator.GetGFFVarianceAlertDetails();
      if (varianceAlertDetails == null || varianceAlertDetails.Count == 0)
        return (string[]) null;
      List<string> stringList = new List<string>();
      foreach (DictionaryEntry dictionaryEntry in varianceAlertDetails)
      {
        ArrayList arrayList = (ArrayList) dictionaryEntry.Value;
        if (arrayList != null && arrayList.Count != 0)
        {
          for (int index = 0; index < arrayList.Count; ++index)
          {
            GFFVAlertTriggerField alertTriggerField = (GFFVAlertTriggerField) arrayList[index];
            stringList.Add(alertTriggerField.FieldId);
          }
        }
      }
      return stringList.Count <= 0 ? (string[]) null : stringList.ToArray();
    }

    public int GetChangeOfCircumstanceEntryIndex(string alertFieldID)
    {
      if (string.IsNullOrEmpty(alertFieldID))
        throw new ArgumentOutOfRangeException(nameof (alertFieldID), (object) alertFieldID, "The alertFieldID value cannot be null or blank.");
      return this.loan.LoanData.GetGoodFaithChangeOfCircumstanceRecordIndex(alertFieldID);
    }
  }
}
