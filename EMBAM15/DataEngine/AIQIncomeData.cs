// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AIQIncomeData
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class AIQIncomeData
  {
    private const string className = "AIQIncomeData";
    private static string sw = Tracing.SwOutsideLoan;
    private Dictionary<string, IList<AIQEmploymentData>> incomeData = new Dictionary<string, IList<AIQEmploymentData>>();
    private Dictionary<string, IList<AIQPropertyRentalData>> propertyRentalData = new Dictionary<string, IList<AIQPropertyRentalData>>();
    private Dictionary<string, IList<AIQOtherIncomeData>> otherIncomeData = new Dictionary<string, IList<AIQOtherIncomeData>>();
    private LoanData loan;

    public Dictionary<string, IList<AIQEmploymentData>> IncomeData => this.incomeData;

    public Dictionary<string, IList<AIQPropertyRentalData>> PropertyRentalData
    {
      get => this.propertyRentalData;
    }

    public Dictionary<string, IList<AIQOtherIncomeData>> OtherIncomeData => this.otherIncomeData;

    public AIQIncomeData(LoanData encompassLoan, JAIQIncome jAIQData)
    {
      if (jAIQData == null)
        throw new Exception("Income Analyzer payload is empty.");
      this.loan = encompassLoan;
      this.parseJsonData(jAIQData);
    }

    private void parseJsonData(JAIQIncome jData)
    {
      if (jData.applicants == null || jData.applicants.Count == 0 || jData.results == null || jData.results.applicants == null)
        return;
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        string field1 = this.loan.GetField("4002", borrowerPairs[index]);
        string field2 = this.loan.GetField("4000", borrowerPairs[index]);
        string field3 = this.loan.GetField("65", borrowerPairs[index]);
        string str1 = field3.Length < 4 ? field3 : field3.Substring(field3.Length - 4);
        string field4 = this.loan.GetField("4006", borrowerPairs[index]);
        string field5 = this.loan.GetField("4004", borrowerPairs[index]);
        string field6 = this.loan.GetField("97", borrowerPairs[index]);
        string str2 = field6.Length < 4 ? field6 : field6.Substring(field6.Length - 4);
        string key1 = field1 + "|" + field2 + "|" + str1;
        if (key1 != "||")
          dictionary1.Add(key1, (index + 1).ToString() + "B");
        string key2 = field4 + "|" + field5 + "|" + str2;
        if (key2 != "||")
          dictionary1.Add(key2, (index + 1).ToString() + "C");
      }
      Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
      for (int index = 0; index < jData.applicants.Count; ++index)
      {
        JAIQApplicants applicant = jData.applicants[index];
        dictionary2.Add(applicant.id, applicant.name.lastName + "|" + applicant.name.firstName + "|" + applicant.last4TaxIdentifier);
      }
      for (int index1 = 0; index1 < jData.results.applicants.Count; ++index1)
      {
        JAIQApplicantsInResults applicant = jData.results.applicants[index1];
        string key = dictionary2[applicant.id];
        if (applicant.selected && dictionary1.ContainsKey(key))
        {
          IList<AIQEmploymentData> empDataForEncompass = (IList<AIQEmploymentData>) new List<AIQEmploymentData>();
          IList<JAIQEmployers> employers = applicant.employers;
          for (int index2 = 0; index2 < employers.Count; ++index2)
          {
            JAIQEmployers aiqData = employers[index2];
            if (aiqData.selected && (aiqData.employment != null || aiqData.currentIncome != null))
              empDataForEncompass.Add(new AIQEmploymentData(aiqData));
          }
          if (empDataForEncompass.Count > 0)
          {
            this.populateDefaultLinkedVoeNumber(dictionary1[key], empDataForEncompass);
            this.incomeData.Add(dictionary1[key], empDataForEncompass);
          }
          IList<JAIQPropertyRental> propertyRental = applicant.propertyRental;
          IList<AIQPropertyRentalData> propRentalDataForEncompass = (IList<AIQPropertyRentalData>) new List<AIQPropertyRentalData>();
          if (propertyRental != null)
          {
            for (int index3 = 0; index3 < propertyRental.Count; ++index3)
            {
              JAIQPropertyRental data = propertyRental[index3];
              if (data.selected && data.currentIncome != null)
                propRentalDataForEncompass.Add(new AIQPropertyRentalData(data));
            }
            if (propRentalDataForEncompass.Count > 0)
            {
              if (!this.propertyRentalData.ContainsKey(dictionary1[key]))
              {
                this.propertyRentalData.Add(dictionary1[key], propRentalDataForEncompass);
              }
              else
              {
                foreach (AIQPropertyRentalData propertyRentalData in (IEnumerable<AIQPropertyRentalData>) propRentalDataForEncompass)
                  this.propertyRentalData[dictionary1[key]].Add(propertyRentalData);
              }
              this.populateBelongsTo(dictionary1[key], propRentalDataForEncompass);
            }
          }
          IList<JAIQOtherIncomes> otherIncomes = applicant.otherIncomes;
          IList<AIQOtherIncomeData> otherIncomeDataForEncompass = (IList<AIQOtherIncomeData>) new List<AIQOtherIncomeData>();
          if (otherIncomes != null)
          {
            for (int index4 = 0; index4 < otherIncomes.Count; ++index4)
            {
              JAIQOtherIncomes data = otherIncomes[index4];
              if (data.selected && data.currentIncome != null)
                otherIncomeDataForEncompass.Add(new AIQOtherIncomeData(data));
            }
            if (otherIncomeDataForEncompass.Count > 0)
            {
              this.otherIncomeData.Add(dictionary1[key], otherIncomeDataForEncompass);
              this.populateBelongsTo(dictionary1[key], otherIncomeDataForEncompass);
            }
          }
        }
      }
    }

    private void populateBelongsTo(
      string ssnKey,
      IList<AIQOtherIncomeData> otherIncomeDataForEncompass)
    {
      string str1 = ssnKey;
      int result = 0;
      string str2 = "URLAROIS";
      if (str1.Length > 1)
        int.TryParse(str1.Substring(0, 1), out result);
      if (result > 0)
        --result;
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      this.loan.SetBorrowerPair(result);
      for (int index = 1; index <= this.loan.GetNumberOfOtherIncomeSources(); ++index)
      {
        string field = this.loan.GetField(str2 + index.ToString("00") + "01");
        foreach (AIQOtherIncomeData aiqOtherIncomeData in (IEnumerable<AIQOtherIncomeData>) otherIncomeDataForEncompass)
        {
          if (aiqOtherIncomeData.losId == field)
            aiqOtherIncomeData.linkedEncompassOISBlockNumber = index;
        }
      }
      this.loan.SetBorrowerPair(currentBorrowerPair);
    }

    private void populateBelongsTo(
      string ssnKey,
      IList<AIQPropertyRentalData> propRentalDataForEncompass)
    {
      string str1 = ssnKey;
      int result = 0;
      string str2 = "FM";
      bool flag = true;
      if (str1.Length > 1)
      {
        int.TryParse(str1.Substring(0, 1), out result);
        if (str1.Substring(1) == "C")
          flag = false;
      }
      if (result > 0)
        --result;
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      this.loan.SetBorrowerPair(result);
      for (int index = 1; index <= this.loan.GetNumberOfMortgages(); ++index)
      {
        string field1 = this.loan.GetField(str2 + index.ToString("00") + "43");
        string field2 = this.loan.GetField(str2 + index.ToString("00") + "46");
        string field3 = this.loan.GetField(str2 + index.ToString("00") + "28");
        foreach (AIQPropertyRentalData aiqProp in (IEnumerable<AIQPropertyRentalData>) propRentalDataForEncompass)
        {
          if (aiqProp.losId == field1)
          {
            aiqProp.linkedEncompassVOMBlockNumber = index;
            if (field2 == "Both")
            {
              aiqProp.belongsToBoth = true;
              this.createDummyRecordInBorrower(aiqProp, !flag, result + 1);
            }
            aiqProp.isSubjectProperty = field3 == "Y";
          }
        }
      }
      this.loan.SetBorrowerPair(currentBorrowerPair);
    }

    private void createDummyRecordInBorrower(
      AIQPropertyRentalData aiqProp,
      bool createForBorrower,
      int borIndex)
    {
      string key = borIndex.ToString() + (createForBorrower ? (object) "B" : (object) "C");
      if (!this.propertyRentalData.ContainsKey(key))
        this.propertyRentalData.Add(key, (IList<AIQPropertyRentalData>) new List<AIQPropertyRentalData>());
      this.propertyRentalData[key].Add(new AIQPropertyRentalData(aiqProp)
      {
        belongsToBoth = true
      });
    }

    private void populateDefaultLinkedVoeNumber(
      string ssnKey,
      IList<AIQEmploymentData> empDataForEncompass)
    {
      string str1 = ssnKey;
      int result = 0;
      bool borrower = true;
      string str2 = "BE";
      if (str1.Length > 1)
      {
        int.TryParse(str1.Substring(0, 1), out result);
        if (str1.Substring(1) == "C")
        {
          borrower = false;
          str2 = "CE";
        }
      }
      if (result > 0)
        --result;
      BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      this.loan.SetBorrowerPair(result);
      for (int index = 1; index <= this.loan.GetNumberOfEmployer(borrower); ++index)
      {
        string field = this.loan.GetField(str2 + index.ToString("00") + "99");
        foreach (AIQEmploymentData aiqEmploymentData in (IEnumerable<AIQEmploymentData>) empDataForEncompass)
        {
          if (aiqEmploymentData.losId == field && aiqEmploymentData.linkedEncompassVOEBlockNumber == -1)
          {
            aiqEmploymentData.linkedEncompassVOEBlockNumber = index;
            break;
          }
        }
      }
      this.loan.SetBorrowerPair(currentBorrowerPair);
    }

    public IList<AIQEmploymentData> getEmploymentList(string borrowerKey)
    {
      if (!this.incomeData.ContainsKey(borrowerKey))
        return (IList<AIQEmploymentData>) null;
      List<AIQEmploymentData> employmentList = (List<AIQEmploymentData>) this.incomeData[borrowerKey];
      employmentList.Sort();
      return (IList<AIQEmploymentData>) employmentList;
    }

    public IList<AIQPropertyRentalData> getPropertyRentalList(string borrowerKey)
    {
      return this.propertyRentalData.ContainsKey(borrowerKey) ? this.propertyRentalData[borrowerKey] : (IList<AIQPropertyRentalData>) null;
    }

    public IList<AIQOtherIncomeData> getOtherIncomeList(string borrowerKey)
    {
      return this.otherIncomeData.ContainsKey(borrowerKey) ? this.otherIncomeData[borrowerKey] : (IList<AIQOtherIncomeData>) null;
    }

    public void import()
    {
      try
      {
        BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
        BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
        for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
        {
          this.loan.SetBorrowerPair(borrowerPairs[index1]);
          for (int index2 = 0; index2 <= 1; ++index2)
          {
            string key = (index1 + 1).ToString() + (index2 == 0 ? (object) "B" : (object) "C");
            if (this.incomeData.ContainsKey(key))
            {
              foreach (AIQEmploymentData aiqEmploymentData in (IEnumerable<AIQEmploymentData>) this.incomeData[key])
              {
                if (aiqEmploymentData.isSelected)
                  aiqEmploymentData.import(this.loan, index2 == 0);
              }
            }
            if (this.propertyRentalData.ContainsKey(key))
            {
              foreach (AIQPropertyRentalData propertyRentalData in (IEnumerable<AIQPropertyRentalData>) this.propertyRentalData[key])
              {
                if (propertyRentalData.isSelected && !propertyRentalData.ignoreForImport)
                  propertyRentalData.import(this.loan, index2 == 0);
              }
            }
            if (this.otherIncomeData.ContainsKey(key))
            {
              foreach (AIQOtherIncomeData aiqOtherIncomeData in (IEnumerable<AIQOtherIncomeData>) this.otherIncomeData[key])
              {
                if (aiqOtherIncomeData.isSelected)
                  aiqOtherIncomeData.import(this.loan, index2 == 0);
              }
            }
          }
        }
        this.loan.SetField("5022", System.TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).ToString("u"));
        this.loan.SetBorrowerPair(currentBorrowerPair);
      }
      catch (Exception ex)
      {
        Tracing.Log(AIQIncomeData.sw, TraceLevel.Verbose, nameof (AIQIncomeData), "import Error: " + ex.Message);
      }
    }
  }
}
