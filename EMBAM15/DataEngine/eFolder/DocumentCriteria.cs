// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.DocumentCriteria
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
  public class DocumentCriteria : IXmlSerializable, IHashableContents
  {
    public string[] AmortTypeValues;
    public string[] LienTypeValues;
    public string[] LoanPurposeValues;
    public string[] LoanTypeValues;
    public string[] OccupancyTypeValues;
    public string[] PropertyStateValues;
    public string[] PlanCodeValues;
    public string[] FormVersionValues;

    public DocumentCriteria(
      string[] amortTypeValues,
      string[] lienTypeValues,
      string[] loanPurposeValues,
      string[] loanTypeValues,
      string[] occupancyTypeValues,
      string[] propertyStateValues,
      string[] planCodeValues,
      string[] formVersionValues)
    {
      this.AmortTypeValues = amortTypeValues;
      this.LienTypeValues = lienTypeValues;
      this.LoanPurposeValues = loanPurposeValues;
      this.LoanTypeValues = loanTypeValues;
      this.OccupancyTypeValues = occupancyTypeValues;
      this.PropertyStateValues = propertyStateValues;
      this.PlanCodeValues = planCodeValues;
      this.FormVersionValues = formVersionValues;
    }

    int IHashableContents.GetContentsHashCode()
    {
      return ObjectArrayHelpers.GetAggregateHash((object) this.AmortTypeValues, (object) this.LienTypeValues, (object) this.LoanPurposeValues, (object) this.LoanTypeValues, (object) this.OccupancyTypeValues, (object) this.PropertyStateValues, (object) this.PlanCodeValues, (object) this.FormVersionValues);
    }

    public DocumentCriteria(XmlSerializationInfo info)
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
      XmlList<string> xmlList8 = (XmlList<string>) info.GetValue(nameof (FormVersionValues), typeof (XmlList<string>), (object) null);
      if (xmlList8 == null)
        return;
      this.FormVersionValues = xmlList8.ToArray();
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
      if (this.FormVersionValues != null)
        info.AddValue("FormVersionValues", (object) new XmlList<string>((IEnumerable<string>) this.FormVersionValues));
      else
        info.AddValue("FormVersionValues", (object) null);
    }

    public bool EvaluateCriteria(LoanData loan)
    {
      if (this.AmortTypeValues != null && Array.IndexOf<string>(this.AmortTypeValues, loan.GetSimpleField("608", 0)) == -1 || this.LienTypeValues != null && Array.IndexOf<string>(this.LienTypeValues, loan.GetSimpleField("420", 0)) == -1 || this.LoanPurposeValues != null && Array.IndexOf<string>(this.LoanPurposeValues, loan.GetSimpleField("19", 0)) == -1 || this.LoanTypeValues != null && Array.IndexOf<string>(this.LoanTypeValues, loan.GetSimpleField("1172", 0)) == -1 || this.OccupancyTypeValues != null && Array.IndexOf<string>(this.OccupancyTypeValues, loan.GetSimpleField("1811", 0)) == -1 || this.PropertyStateValues != null && Array.IndexOf<string>(this.PropertyStateValues, loan.GetSimpleField("14", 0)) == -1 || this.PlanCodeValues != null && Array.IndexOf<string>(this.PlanCodeValues, loan.GetSimpleField("1881", 0)) == -1)
        return false;
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
  }
}
