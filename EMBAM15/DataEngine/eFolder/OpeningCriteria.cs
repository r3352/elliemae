// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.OpeningCriteria
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class OpeningCriteria : IXmlSerializable, IHashableContents
  {
    public string[] AmortTypeValues;
    public string[] LienTypeValues;
    public string[] LoanPurposeValues;
    public string[] LoanTypeValues;
    public string[] OccupancyTypeValues;
    public string[] PropertyStateValues;
    public string[] PlanCodeValues;
    public string[] ChannelTypeValues;
    public string[] EntityTypeValues;
    public string[] PackageTypeValues;
    public string[] LoanLockValues;
    public string[] ChangedCircumstance;
    public string[] FormVersionValues;

    public OpeningCriteria(
      string[] amortTypeValues,
      string[] lienTypeValues,
      string[] loanPurposeValues,
      string[] loanTypeValues,
      string[] occupancyTypeValues,
      string[] propertyStateValues,
      string[] planCodeValues,
      string[] channelTypeValues,
      string[] entityTypeValues,
      string[] packageTypeValues,
      string[] loanLockValues,
      string[] changedCircumstance,
      string[] formVersionValues)
    {
      this.AmortTypeValues = amortTypeValues;
      this.LienTypeValues = lienTypeValues;
      this.LoanPurposeValues = loanPurposeValues;
      this.LoanTypeValues = loanTypeValues;
      this.OccupancyTypeValues = occupancyTypeValues;
      this.PropertyStateValues = propertyStateValues;
      this.PlanCodeValues = planCodeValues;
      this.ChannelTypeValues = channelTypeValues;
      this.EntityTypeValues = entityTypeValues;
      this.PackageTypeValues = packageTypeValues;
      this.LoanLockValues = loanLockValues;
      this.ChangedCircumstance = changedCircumstance;
      this.FormVersionValues = formVersionValues;
    }

    public OpeningCriteria(XmlSerializationInfo info)
    {
      XmlList<string> xmlList1 = (XmlList<string>) info.GetValue(nameof (AmortTypeValues), typeof (XmlList<string>), (object) null);
      if (xmlList1 != null)
        this.AmortTypeValues = xmlList1.ToArray();
      XmlList<string> xmlList2 = (XmlList<string>) info.GetValue(nameof (LienTypeValues), typeof (XmlList<string>), (object) null);
      if (xmlList2 != null)
        this.LienTypeValues = xmlList2.ToArray();
      XmlList<string> xmlList3 = (XmlList<string>) info.GetValue(nameof (LoanPurposeValues), typeof (XmlList<string>), (object) null);
      if (xmlList3 != null)
        this.LoanPurposeValues = xmlList3.ToArray();
      XmlList<string> xmlList4 = (XmlList<string>) info.GetValue(nameof (LoanTypeValues), typeof (XmlList<string>), (object) null);
      if (xmlList4 != null)
        this.LoanTypeValues = xmlList4.ToArray();
      XmlList<string> xmlList5 = (XmlList<string>) info.GetValue(nameof (OccupancyTypeValues), typeof (XmlList<string>), (object) null);
      if (xmlList5 != null)
        this.OccupancyTypeValues = xmlList5.ToArray();
      XmlList<string> xmlList6 = (XmlList<string>) info.GetValue(nameof (PropertyStateValues), typeof (XmlList<string>), (object) null);
      if (xmlList6 != null)
        this.PropertyStateValues = xmlList6.ToArray();
      XmlList<string> xmlList7 = (XmlList<string>) info.GetValue(nameof (PlanCodeValues), typeof (XmlList<string>), (object) null);
      if (xmlList7 != null)
        this.PlanCodeValues = xmlList7.ToArray();
      XmlList<string> xmlList8 = (XmlList<string>) info.GetValue(nameof (ChannelTypeValues), typeof (XmlList<string>), (object) null);
      if (xmlList8 != null)
        this.ChannelTypeValues = xmlList8.ToArray();
      XmlList<string> xmlList9 = (XmlList<string>) info.GetValue(nameof (EntityTypeValues), typeof (XmlList<string>), (object) null);
      if (xmlList9 != null)
        this.EntityTypeValues = xmlList9.ToArray();
      XmlList<string> xmlList10 = (XmlList<string>) info.GetValue(nameof (PackageTypeValues), typeof (XmlList<string>), (object) null);
      if (xmlList10 != null)
        this.PackageTypeValues = xmlList10.ToArray();
      XmlList<string> xmlList11 = (XmlList<string>) info.GetValue(nameof (LoanLockValues), typeof (XmlList<string>), (object) null);
      if (xmlList11 != null)
        this.LoanLockValues = xmlList11.ToArray();
      XmlList<string> xmlList12 = (XmlList<string>) info.GetValue(nameof (ChangedCircumstance), typeof (XmlList<string>), (object) null);
      if (xmlList12 != null)
        this.ChangedCircumstance = this.changeOfCircumstanceMigration(xmlList12.ToArray());
      XmlList<string> xmlList13 = (XmlList<string>) info.GetValue(nameof (FormVersionValues), typeof (XmlList<string>), (object) null);
      if (xmlList13 == null)
        return;
      this.FormVersionValues = xmlList13.ToArray();
    }

    int IHashableContents.GetContentsHashCode()
    {
      return ObjectArrayHelpers.GetAggregateHash((object) this.AmortTypeValues, (object) this.LienTypeValues, (object) this.LoanPurposeValues, (object) this.LoanTypeValues, (object) this.OccupancyTypeValues, (object) this.PropertyStateValues, (object) this.PlanCodeValues, (object) this.ChannelTypeValues, (object) this.EntityTypeValues, (object) this.LoanLockValues, (object) this.ChangedCircumstance, (object) this.FormVersionValues, (object) this.PackageTypeValues);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      if (this.AmortTypeValues != null)
        info.AddValue("AmortTypeValues", (object) new XmlList<string>((IEnumerable<string>) this.AmortTypeValues));
      else
        info.AddValue("AmortTypeValues", (object) null);
      if (this.LienTypeValues != null)
        info.AddValue("LienTypeValues", (object) new XmlList<string>((IEnumerable<string>) this.LienTypeValues));
      else
        info.AddValue("LienTypeValues", (object) null);
      if (this.LoanPurposeValues != null)
        info.AddValue("LoanPurposeValues", (object) new XmlList<string>((IEnumerable<string>) this.LoanPurposeValues));
      else
        info.AddValue("LoanPurposeValues", (object) null);
      if (this.LoanTypeValues != null)
        info.AddValue("LoanTypeValues", (object) new XmlList<string>((IEnumerable<string>) this.LoanTypeValues));
      else
        info.AddValue("LoanTypeValues", (object) null);
      if (this.OccupancyTypeValues != null)
        info.AddValue("OccupancyTypeValues", (object) new XmlList<string>((IEnumerable<string>) this.OccupancyTypeValues));
      else
        info.AddValue("OccupancyTypeValues", (object) null);
      if (this.PropertyStateValues != null)
        info.AddValue("PropertyStateValues", (object) new XmlList<string>((IEnumerable<string>) this.PropertyStateValues));
      else
        info.AddValue("PropertyStateValues", (object) null);
      if (this.PlanCodeValues != null)
        info.AddValue("PlanCodeValues", (object) new XmlList<string>((IEnumerable<string>) this.PlanCodeValues));
      else
        info.AddValue("PlanCodeValues", (object) null);
      if (this.ChannelTypeValues != null)
        info.AddValue("ChannelTypeValues", (object) new XmlList<string>((IEnumerable<string>) this.ChannelTypeValues));
      else
        info.AddValue("ChannelTypeValues", (object) null);
      if (this.EntityTypeValues != null)
        info.AddValue("EntityTypeValues", (object) new XmlList<string>((IEnumerable<string>) this.EntityTypeValues));
      else
        info.AddValue("EntityTypeValues", (object) null);
      if (this.PackageTypeValues != null)
        info.AddValue("PackageTypeValues", (object) new XmlList<string>((IEnumerable<string>) this.PackageTypeValues));
      else
        info.AddValue("PackageTypeValues", (object) null);
      if (this.LoanLockValues != null)
        info.AddValue("LoanLockValues", (object) new XmlList<string>((IEnumerable<string>) this.LoanLockValues));
      else
        info.AddValue("LoanLockValues", (object) null);
      if (this.ChangedCircumstance != null)
        info.AddValue("ChangedCircumstance", (object) new XmlList<string>((IEnumerable<string>) this.ChangedCircumstance));
      else
        info.AddValue("ChangedCircumstance", (object) null);
      if (this.FormVersionValues != null)
        info.AddValue("FormVersionValues", (object) new XmlList<string>((IEnumerable<string>) this.FormVersionValues));
      else
        info.AddValue("FormVersionValues", (object) null);
    }

    public bool EvaluateCriteria(
      LoanData loan,
      string channelType,
      string[] entityTypes,
      string[] packageTypes)
    {
      if (this.AmortTypeValues != null && Array.IndexOf<string>(this.AmortTypeValues, loan.GetSimpleField("608", 0)) == -1 || this.LienTypeValues != null && Array.IndexOf<string>(this.LienTypeValues, loan.GetSimpleField("420", 0)) == -1 || this.LoanPurposeValues != null && Array.IndexOf<string>(this.LoanPurposeValues, loan.GetSimpleField("19", 0)) == -1 || this.LoanTypeValues != null && Array.IndexOf<string>(this.LoanTypeValues, loan.GetSimpleField("1172", 0)) == -1 || this.OccupancyTypeValues != null && Array.IndexOf<string>(this.OccupancyTypeValues, loan.GetSimpleField("1811", 0)) == -1 || this.PropertyStateValues != null && Array.IndexOf<string>(this.PropertyStateValues, loan.GetSimpleField("14", 0)) == -1 || this.PlanCodeValues != null && Array.IndexOf<string>(this.PlanCodeValues, loan.GetSimpleField("DISCLOSUREPLANCODE", 0)) == -1)
        return false;
      if (channelType == "Banked - Retail")
        channelType = "Banker - Retail";
      if (channelType == "Banked - Wholesale")
        channelType = "Banker - Wholesale";
      if (this.ChannelTypeValues != null && Array.IndexOf<string>(this.ChannelTypeValues, channelType) == -1)
        return false;
      if (this.EntityTypeValues != null)
      {
        bool flag = false;
        foreach (string entityType in entityTypes)
        {
          if (Array.IndexOf<string>(this.EntityTypeValues, entityType) >= 0)
            flag = true;
        }
        if (!flag)
          return false;
      }
      if (this.PackageTypeValues != null)
      {
        bool flag = false;
        foreach (string packageType in packageTypes)
        {
          if (Array.IndexOf<string>(this.PackageTypeValues, packageType) >= 0)
            flag = true;
        }
        if (!flag)
          return false;
      }
      if (this.LoanLockValues != null)
      {
        string str = "N";
        if (loan.GetSimpleField("2400", 0).StartsWith("Y"))
          str = "Y";
        if (Array.IndexOf<string>(this.LoanLockValues, "Locked") >= 0 && Array.IndexOf<string>(this.LoanLockValues, "Not Locked") == -1)
        {
          if (str == "N")
            return false;
        }
        else if (Array.IndexOf<string>(this.LoanLockValues, "Locked") == -1 && Array.IndexOf<string>(this.LoanLockValues, "Not Locked") >= 0 && str == "Y")
          return false;
      }
      if (this.ChangedCircumstance != null)
      {
        if (!loan.GetSimpleField("3168", 0).StartsWith("Y"))
          return false;
        string[] strArray = loan.GetField("3627", 0).Split(',');
        bool flag = false;
        foreach (string str in strArray)
        {
          if (Array.IndexOf<string>(this.ChangedCircumstance, str) != -1)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      if (this.FormVersionValues != null)
      {
        if (loan.Use2015RESPA)
        {
          if (Array.IndexOf<string>(this.FormVersionValues, "RESPA-TILA 2015 LE and CD") == -1)
            return false;
        }
        else if (loan.Use2010RESPA)
        {
          if (Array.IndexOf<string>(this.FormVersionValues, "RESPA 2010 GFE and HUD-1") == -1)
            return false;
        }
        else if (Array.IndexOf<string>(this.FormVersionValues, "Old GFE and HUD-1") == -1)
          return false;
      }
      return true;
    }

    private bool isDate(string date)
    {
      try
      {
        return !(DateTime.Parse(date) == DateTime.MinValue);
      }
      catch
      {
        return false;
      }
    }

    private string[] changeOfCircumstanceMigration(string[] oriList)
    {
      List<string> stringList = new List<string>();
      foreach (string ori in oriList)
      {
        switch (ori)
        {
          case "Additional borrower has been added to the loan or borrower has been dropped from the loan":
            stringList.Add("AddiBor");
            break;
          case "Additional service (such as survey) is necessary based on title report":
            stringList.Add("AddiService");
            break;
          case "Appraised value is different than estimated value":
            stringList.Add("ApprasValDiff");
            break;
          case "Borrower income could not verified or was verified at different amount":
            stringList.Add("IncomeNotVeri");
            break;
          case "Borrower taking title to the property has changed":
            stringList.Add("PropertyTitle");
            break;
          case "Change in loan amount":
            stringList.Add("ChangeLoanAmt");
            break;
          case "Loan type or loan program has changed":
            stringList.Add("LoanTypeProgram");
            break;
          case "Other":
            stringList.Add("Other");
            break;
          case "Recording fees are increased based on need to record additional unanticipated documents such as release of prior lien":
            stringList.Add("RecordingFee");
            break;
          default:
            stringList.Add(ori);
            break;
        }
      }
      return stringList.ToArray();
    }
  }
}
