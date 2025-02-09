// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.BusinessCategoryEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class BusinessCategoryEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;

    static BusinessCategoryEnumUtil()
    {
      BusinessCategoryEnumUtil._NameToValue.Add((object) "", (object) BusinessCategory.NoCategory);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "All", (object) BusinessCategory.Any);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Appraiser", (object) BusinessCategory.Appraiser);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Attorney", (object) BusinessCategory.Attorney);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Builder", (object) BusinessCategory.Builder);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Credit Company", (object) BusinessCategory.Credit);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Doc Signing", (object) BusinessCategory.DocSigning);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Escrow Company", (object) BusinessCategory.Escrow);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Flood Insurance", (object) BusinessCategory.FloodInsurer);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Hazard Insurance", (object) BusinessCategory.HazardInsurer);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Lender", (object) BusinessCategory.Lender);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Mortgage Insurance", (object) BusinessCategory.MortgageInsurer);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Organization", (object) BusinessCategory.Organization);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Real Estate Agent", (object) BusinessCategory.RealEstateAgent);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Servicing", (object) BusinessCategory.Servicing);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Surveyor", (object) BusinessCategory.Surveyor);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Title Insurance", (object) BusinessCategory.Title);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Underwriter", (object) BusinessCategory.Underwriter);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Verification", (object) BusinessCategory.Verification);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Investor", (object) BusinessCategory.Investor);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Warehouse Bank", (object) BusinessCategory.WarehouseBank);
      BusinessCategoryEnumUtil._NameToValue.Add((object) "Dealer", (object) BusinessCategory.Dealer);
      BusinessCategoryEnumUtil._ValueToName = new Hashtable();
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.NoCategory, (object) "No Category");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Any, (object) "All");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Appraiser, (object) "Appraiser");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Attorney, (object) "Attorney");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Builder, (object) "Builder");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Credit, (object) "Credit Company");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.DocSigning, (object) "Doc Signing");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Escrow, (object) "Escrow Company");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.FloodInsurer, (object) "Flood Insurance");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.HazardInsurer, (object) "Hazard Insurance");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Lender, (object) "Lender");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.MortgageInsurer, (object) "Mortgage Insurance");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Organization, (object) "Organization");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.RealEstateAgent, (object) "Real Estate Agent");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Servicing, (object) "Servicing");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Surveyor, (object) "Surveyor");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Title, (object) "Title Insurance");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Underwriter, (object) "Underwriter");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Verification, (object) "Verification");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Investor, (object) "Investor");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.WarehouseBank, (object) "Warehouse Bank");
      BusinessCategoryEnumUtil._ValueToName.Add((object) BusinessCategory.Dealer, (object) "Dealer");
    }

    public static IDictionary ValueToNameMap => (IDictionary) BusinessCategoryEnumUtil._ValueToName;

    public static string[] GetDisplayNames()
    {
      BusinessCategory[] values = (BusinessCategory[]) Enum.GetValues(typeof (BusinessCategory));
      string[] displayNames = new string[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = BusinessCategoryEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(BusinessCategory val)
    {
      return (string) BusinessCategoryEnumUtil._ValueToName[(object) val];
    }

    public static BusinessCategory NameToValue(string name)
    {
      return BusinessCategoryEnumUtil._NameToValue.Contains((object) name) ? (BusinessCategory) BusinessCategoryEnumUtil._NameToValue[(object) name] : BusinessCategory.NoCategory;
    }
  }
}
