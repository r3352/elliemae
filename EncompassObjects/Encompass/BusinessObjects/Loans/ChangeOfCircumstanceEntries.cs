// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.ChangeOfCircumstanceEntries
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Change of Circumstance Entries class</summary>
  public class ChangeOfCircumstanceEntries : IChangeOfCircumstanceEntries
  {
    private Loan loan;

    internal ChangeOfCircumstanceEntries(Loan loan) => this.loan = loan;

    /// <summary>Gets the number of Change Of Circumstance Entries</summary>
    public int Count => this.loan.LoanData.GetNumberOfGoodFaithChangeOfCircumstance();

    /// <summary>
    /// Get all field IDs from the Good Faith Fee Variance Alert Worksheet.
    /// </summary>
    /// <returns>A string array with all available fields in the Good Faith Fee Variance Alert Worksheet. Return null if alert is not available.
    /// </returns>
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

    /// <summary>
    /// Get Change Of Circumstance Record Index Number (1-based)
    /// </summary>
    /// <returns>A Change Of Circumstance Record Index Number will be retured (1-based).
    /// </returns>
    /// <param name="alertFieldID">Use an Alert Trigger Field to get the Change Of Circumstance Record Index Number</param>
    public int GetChangeOfCircumstanceEntryIndex(string alertFieldID)
    {
      if (string.IsNullOrEmpty(alertFieldID))
        throw new ArgumentOutOfRangeException(nameof (alertFieldID), (object) alertFieldID, "The alertFieldID value cannot be null or blank.");
      return this.loan.LoanData.GetGoodFaithChangeOfCircumstanceRecordIndex(alertFieldID);
    }
  }
}
