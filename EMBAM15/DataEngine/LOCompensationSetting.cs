// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LOCompensationSetting
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class LOCompensationSetting
  {
    private List<string> paidByFields;
    private List<string> amountFields;
    private List<string> paidToFields;
    private List<string> lineItems;
    public static string SellerFields = ",559,L229,1622,569,572,NEWHUD.X226,200,1626,1840,1843,NEWHUD.X779,NEWHUD.X1238,NEWHUD.X1246,NEWHUD.X1254,NEWHUD.X1262,NEWHUD.X1270,NEWHUD.X1278,NEWHUD.X1286,NEWHUD.X788,";
    private LOCompensationSetting.LOActions loAction = LOCompensationSetting.LOActions.NoAction;
    private bool useSellerForBorrower;

    public LOCompensationSetting()
    {
      this.paidByFields = new List<string>();
      this.paidToFields = new List<string>();
      this.amountFields = new List<string>();
      this.lineItems = new List<string>();
    }

    public LOCompensationSetting(string settings)
      : this()
    {
      if ((settings ?? "") == string.Empty)
        settings = "1,a,b,c,d,g,h,i,j,k,l,m,n,o,p,q,r,s";
      if (!((settings ?? "") != string.Empty))
        return;
      string[] strArray = settings.Split(',');
      if (strArray == null)
        return;
      if (strArray.Length == 0)
        return;
      try
      {
        switch (int.Parse(strArray[0], NumberStyles.Any))
        {
          case 0:
            this.loAction = LOCompensationSetting.LOActions.NoAction;
            break;
          case 1:
            this.loAction = LOCompensationSetting.LOActions.Fix;
            break;
          case 2:
            this.loAction = LOCompensationSetting.LOActions.WarningOnly;
            break;
        }
      }
      catch (Exception ex)
      {
        this.loAction = LOCompensationSetting.LOActions.NoAction;
      }
      if (this.loAction == LOCompensationSetting.LOActions.NoAction)
        return;
      this.paidByFields.Add("SYS.X265");
      this.paidToFields.Add("SYS.X266");
      this.amountFields.Add("439");
      this.paidByFields.Add("SYS.X265");
      this.paidToFields.Add("SYS.X266");
      this.amountFields.Add("389");
      this.paidByFields.Add("SYS.X265");
      this.paidToFields.Add("SYS.X266");
      this.amountFields.Add("1620");
      this.paidByFields.Add("SYS.X265");
      this.paidToFields.Add("SYS.X266");
      this.amountFields.Add("572");
      this.lineItems.Add("e");
      this.paidByFields.Add("NEWHUD.X227");
      this.paidToFields.Add("NEWHUD.X230");
      this.amountFields.Add("NEWHUD.X225");
      this.paidByFields.Add("NEWHUD.X227");
      this.paidToFields.Add("NEWHUD.X230");
      this.amountFields.Add("NEWHUD.X223");
      this.paidByFields.Add("NEWHUD.X227");
      this.paidToFields.Add("NEWHUD.X230");
      this.amountFields.Add("NEWHUD.X224");
      this.paidByFields.Add("NEWHUD.X227");
      this.paidToFields.Add("NEWHUD.X230");
      this.amountFields.Add("NEWHUD.X226");
      this.lineItems.Add("f");
      for (int index = 1; index < strArray.Length; ++index)
      {
        if (!(strArray[index] == "s"))
        {
          this.lineItems.Add(strArray[index]);
          switch (strArray[index])
          {
            case "3b":
              this.useSellerForBorrower = true;
              continue;
            case "a":
              this.paidByFields.Add("SYS.X251");
              this.paidToFields.Add("SYS.X252");
              this.amountFields.Add("454");
              this.paidByFields.Add("SYS.X251");
              this.paidToFields.Add("SYS.X252");
              this.amountFields.Add("388");
              this.paidByFields.Add("SYS.X251");
              this.paidToFields.Add("SYS.X252");
              this.amountFields.Add("559");
              continue;
            case "b":
              this.paidByFields.Add("SYS.X261");
              this.paidToFields.Add("SYS.X262");
              this.amountFields.Add("L228");
              this.paidByFields.Add("SYS.X261");
              this.paidToFields.Add("SYS.X262");
              this.amountFields.Add("L229");
              continue;
            case "c":
              this.paidByFields.Add("SYS.X269");
              this.paidToFields.Add("SYS.X270");
              this.amountFields.Add("1621");
              this.paidByFields.Add("SYS.X269");
              this.paidToFields.Add("SYS.X270");
              this.amountFields.Add("1622");
              continue;
            case "d":
              this.paidByFields.Add("SYS.X271");
              this.paidToFields.Add("SYS.X272");
              this.amountFields.Add("367");
              this.paidByFields.Add("SYS.X271");
              this.paidToFields.Add("SYS.X272");
              this.amountFields.Add("569");
              continue;
            case "g":
              this.paidByFields.Add("SYS.X289");
              this.paidToFields.Add("SYS.X290");
              this.amountFields.Add("155");
              this.paidByFields.Add("SYS.X289");
              this.paidToFields.Add("SYS.X290");
              this.amountFields.Add("200");
              continue;
            case "h":
              this.paidByFields.Add("SYS.X291");
              this.paidToFields.Add("SYS.X292");
              this.amountFields.Add("1625");
              this.paidByFields.Add("SYS.X291");
              this.paidToFields.Add("SYS.X292");
              this.amountFields.Add("1626");
              continue;
            case "i":
              this.paidByFields.Add("SYS.X296");
              this.paidToFields.Add("SYS.X297");
              this.amountFields.Add("1839");
              this.paidByFields.Add("SYS.X296");
              this.paidToFields.Add("SYS.X297");
              this.amountFields.Add("1840");
              continue;
            case "j":
              this.paidByFields.Add("SYS.X301");
              this.paidToFields.Add("SYS.X302");
              this.amountFields.Add("1842");
              this.paidByFields.Add("SYS.X301");
              this.paidToFields.Add("SYS.X302");
              this.amountFields.Add("1843");
              continue;
            case "k":
              this.paidByFields.Add("NEWHUD.X748");
              this.paidToFields.Add("NEWHUD.X690");
              this.amountFields.Add("NEWHUD.X733");
              this.paidByFields.Add("NEWHUD.X748");
              this.paidToFields.Add("NEWHUD.X690");
              this.amountFields.Add("NEWHUD.X779");
              continue;
            case "l":
              this.paidByFields.Add("NEWHUD.X1239");
              this.paidToFields.Add("NEWHUD.X1242");
              this.amountFields.Add("NEWHUD.X1237");
              this.paidByFields.Add("NEWHUD.X1239");
              this.paidToFields.Add("NEWHUD.X1242");
              this.amountFields.Add("NEWHUD.X1238");
              continue;
            case "m":
              this.paidByFields.Add("NEWHUD.X1247");
              this.paidToFields.Add("NEWHUD.X1250");
              this.amountFields.Add("NEWHUD.X1245");
              this.paidByFields.Add("NEWHUD.X1247");
              this.paidToFields.Add("NEWHUD.X1250");
              this.amountFields.Add("NEWHUD.X1246");
              continue;
            case "n":
              this.paidByFields.Add("NEWHUD.X1255");
              this.paidToFields.Add("NEWHUD.X1258");
              this.amountFields.Add("NEWHUD.X1253");
              this.paidByFields.Add("NEWHUD.X1255");
              this.paidToFields.Add("NEWHUD.X1258");
              this.amountFields.Add("NEWHUD.X1254");
              continue;
            case "o":
              this.paidByFields.Add("NEWHUD.X1263");
              this.paidToFields.Add("NEWHUD.X1266");
              this.amountFields.Add("NEWHUD.X1261");
              this.paidByFields.Add("NEWHUD.X1263");
              this.paidToFields.Add("NEWHUD.X1266");
              this.amountFields.Add("NEWHUD.X1262");
              continue;
            case "p":
              this.paidByFields.Add("NEWHUD.X1271");
              this.paidToFields.Add("NEWHUD.X1274");
              this.amountFields.Add("NEWHUD.X1269");
              this.paidByFields.Add("NEWHUD.X1271");
              this.paidToFields.Add("NEWHUD.X1274");
              this.amountFields.Add("NEWHUD.X1270");
              continue;
            case "q":
              this.paidByFields.Add("NEWHUD.X1279");
              this.paidToFields.Add("NEWHUD.X1282");
              this.amountFields.Add("NEWHUD.X1277");
              this.paidByFields.Add("NEWHUD.X1279");
              this.paidToFields.Add("NEWHUD.X1282");
              this.amountFields.Add("NEWHUD.X1278");
              continue;
            case "r":
              this.paidByFields.Add("NEWHUD.X1287");
              this.paidToFields.Add("NEWHUD.X1290");
              this.amountFields.Add("NEWHUD.X1285");
              this.paidByFields.Add("NEWHUD.X1287");
              this.paidToFields.Add("NEWHUD.X1290");
              this.amountFields.Add("NEWHUD.X1286");
              continue;
            case "s":
              this.paidByFields.Add("NEWHUD.X749");
              this.paidToFields.Add("NEWHUD.X627");
              this.amountFields.Add("1061");
              this.paidByFields.Add("NEWHUD.X749");
              this.paidToFields.Add("NEWHUD.X627");
              this.amountFields.Add("436");
              continue;
            default:
              continue;
          }
        }
      }
    }

    public bool IsLineItemEnabled(string line) => this.IsLineItemEnabled(line, (IHtmlInput) null);

    public bool IsLineItemEnabled(string line, IHtmlInput dataObject)
    {
      if (this.loAction == LOCompensationSetting.LOActions.NoAction || !this.lineItems.Contains(line))
        return false;
      if (dataObject == null)
        return true;
      switch (line)
      {
        case "a":
          return dataObject.GetField("SYS.X252") == "Broker";
        case "b":
          return dataObject.GetField("SYS.X262") == "Broker";
        case "c":
          return dataObject.GetField("SYS.X270") == "Broker";
        case "d":
          return dataObject.GetField("SYS.X272") == "Broker";
        case "e":
          return dataObject.GetField("SYS.X266") == "Broker";
        case "f":
          return dataObject.GetField("NEWHUD.X230") == "Broker";
        case "g":
          return dataObject.GetField("SYS.X290") == "Broker";
        case "h":
          return dataObject.GetField("SYS.X292") == "Broker";
        case "i":
          return dataObject.GetField("SYS.X297") == "Broker";
        case "j":
          return dataObject.GetField("SYS.X302") == "Broker";
        case "k":
          return dataObject.GetField("NEWHUD.X690") == "Broker";
        case "l":
          return dataObject.GetField("NEWHUD.X1242") == "Broker";
        case "m":
          return dataObject.GetField("NEWHUD.X1250") == "Broker";
        case "n":
          return dataObject.GetField("NEWHUD.X1258") == "Broker";
        case "o":
          return dataObject.GetField("NEWHUD.X1266") == "Broker";
        case "p":
          return dataObject.GetField("NEWHUD.X1274") == "Broker";
        case "q":
          return dataObject.GetField("NEWHUD.X1282") == "Broker";
        case "r":
          return dataObject.GetField("NEWHUD.X1290") == "Broker";
        case "s":
          return dataObject.GetField("NEWHUD.X627") == "Broker" && dataObject.GetField("NEWHUD.X715") == "Include Origination Points" && dataObject.GetField("NEWHUD.X713") != "Origination Charge" && dataObject.GetField("3119") == string.Empty;
        default:
          return false;
      }
    }

    public bool IsPaidByFieldCompensationEnabled(string paidByFieldID, IHtmlInput dataObject)
    {
      if (this.loAction == LOCompensationSetting.LOActions.NoAction)
        return false;
      switch (paidByFieldID)
      {
        case "NEWHUD.X1239":
          return this.IsLineItemEnabled("l", dataObject);
        case "NEWHUD.X1247":
          return this.IsLineItemEnabled("m", dataObject);
        case "NEWHUD.X1255":
          return this.IsLineItemEnabled("n", dataObject);
        case "NEWHUD.X1263":
          return this.IsLineItemEnabled("o", dataObject);
        case "NEWHUD.X1271":
          return this.IsLineItemEnabled("p", dataObject);
        case "NEWHUD.X1279":
          return this.IsLineItemEnabled("q", dataObject);
        case "NEWHUD.X1287":
          return this.IsLineItemEnabled("r", dataObject);
        case "NEWHUD.X227":
          return this.IsLineItemEnabled("f", dataObject);
        case "NEWHUD.X748":
          return this.IsLineItemEnabled("k", dataObject);
        case "SYS.X251":
          return this.IsLineItemEnabled("a", dataObject);
        case "SYS.X261":
          return this.IsLineItemEnabled("b", dataObject);
        case "SYS.X265":
          return this.IsLineItemEnabled("e", dataObject);
        case "SYS.X269":
          return this.IsLineItemEnabled("c", dataObject);
        case "SYS.X271":
          return this.IsLineItemEnabled("d", dataObject);
        case "SYS.X289":
          return this.IsLineItemEnabled("g", dataObject);
        case "SYS.X291":
          return this.IsLineItemEnabled("h", dataObject);
        case "SYS.X296":
          return this.IsLineItemEnabled("i", dataObject);
        case "SYS.X301":
          return this.IsLineItemEnabled("j", dataObject);
        default:
          return false;
      }
    }

    public bool PaidByFieldSyncIsRequired(string paidByFieldID, IHtmlInput dataObject)
    {
      if (this.loAction == LOCompensationSetting.LOActions.NoAction || !(dataObject.GetField(paidByFieldID) == ""))
        return false;
      switch (paidByFieldID)
      {
        case "NEWHUD.X1239":
          return this.IsLineItemEnabled("l", dataObject);
        case "NEWHUD.X1247":
          return this.IsLineItemEnabled("m", dataObject);
        case "NEWHUD.X1255":
          return this.IsLineItemEnabled("n", dataObject);
        case "NEWHUD.X1263":
          return this.IsLineItemEnabled("o", dataObject);
        case "NEWHUD.X1271":
          return this.IsLineItemEnabled("p", dataObject);
        case "NEWHUD.X1279":
          return this.IsLineItemEnabled("q", dataObject);
        case "NEWHUD.X1287":
          return this.IsLineItemEnabled("r", dataObject);
        case "NEWHUD.X227":
          return this.IsLineItemEnabled("f", dataObject);
        case "NEWHUD.X748":
          return this.IsLineItemEnabled("k", dataObject);
        case "SYS.X251":
          return this.IsLineItemEnabled("a", dataObject);
        case "SYS.X261":
          return this.IsLineItemEnabled("b", dataObject);
        case "SYS.X265":
          return this.IsLineItemEnabled("e", dataObject);
        case "SYS.X269":
          return this.IsLineItemEnabled("c", dataObject);
        case "SYS.X271":
          return this.IsLineItemEnabled("d", dataObject);
        case "SYS.X289":
          return this.IsLineItemEnabled("g", dataObject);
        case "SYS.X291":
          return this.IsLineItemEnabled("h", dataObject);
        case "SYS.X296":
          return this.IsLineItemEnabled("i", dataObject);
        case "SYS.X301":
          return this.IsLineItemEnabled("j", dataObject);
        default:
          return false;
      }
    }

    public bool EnableLOCompensationRule(IHtmlInput dataObject)
    {
      switch (dataObject)
      {
        case null:
        case ClosingCost _:
          return true;
        default:
          string field1 = dataObject.GetField("3292");
          string field2 = dataObject.GetField("745");
          DateTime date = Utils.ParseDate((object) "04/01/2011");
          DateTime dateTime = DateTime.MinValue;
          if (field1 != "" && field1 != "//")
            dateTime = Utils.ParseDate((object) field1);
          if (dateTime == DateTime.MinValue && field2 != "" && field2 != "//")
            dateTime = Utils.ParseDate((object) field2);
          return dateTime == DateTime.MinValue || dateTime != DateTime.MinValue && date.Date.CompareTo(dateTime.Date) <= 0;
      }
    }

    public void SetDefaultPaidBy(IHtmlInput dataObject, string fieldID, string currentValue)
    {
      if (this.loAction == LOCompensationSetting.LOActions.NoAction || !this.EnableLOCompensationRule(dataObject) || !this.amountFields.Contains(fieldID) && !this.paidToFields.Contains(fieldID) || !this.HasViolation(dataObject, fieldID, currentValue))
        return;
      bool flag1 = false;
      bool flag2 = false;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      for (int index = 0; index < this.amountFields.Count; ++index)
      {
        if (!(this.amountFields[index] == fieldID) && !(dataObject.GetField(this.paidToFields[index]) != "Broker"))
        {
          string field = dataObject.GetField(this.amountFields[index]);
          string str = dataObject.GetField(this.paidByFields[index]);
          if (str == string.Empty && LOCompensationSetting.SellerFields.IndexOf("," + this.amountFields[index] + ",") > -1)
            str = "Seller";
          else if (this.paidByFields[index] == "NEWHUD.X227")
            str = "Lender";
          else if (this.paidByFields[index] == "SYS.X265")
            str = "";
          if (str == string.Empty)
          {
            if (field != string.Empty)
              flag1 = true;
          }
          else if (field != string.Empty)
            flag2 = true;
          if (flag1 & flag2)
            break;
        }
      }
      if ((!flag1 || flag2) && !(!flag1 & flag2))
        return;
      for (int index = 0; index < this.amountFields.Count; ++index)
      {
        if (this.amountFields[index] == fieldID)
        {
          string field = dataObject.GetField(this.paidByFields[index]);
          if (flag1 && field != string.Empty)
          {
            dataObject.SetField(this.paidByFields[index], "");
            break;
          }
          if (!flag2 || !(field == string.Empty))
            break;
          dataObject.SetField(this.paidByFields[index], "Lender");
          break;
        }
      }
    }

    public bool HasViolation(IHtmlInput dataObject)
    {
      return this.HasViolation(dataObject, (string) null, (string) null);
    }

    public bool HasViolation(IHtmlInput dataObject, string fieldID, string currentValue)
    {
      if (this.loAction == LOCompensationSetting.LOActions.NoAction || !this.EnableLOCompensationRule(dataObject))
        return false;
      if (fieldID != null)
      {
        string[] compensationFields = this.GetCompensationFields(fieldID);
        if (compensationFields == null || compensationFields.Length == 0)
          return false;
      }
      if (dataObject == null)
        return false;
      bool flag1 = false;
      bool flag2 = false;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      for (int index = 0; index < this.amountFields.Count; ++index)
      {
        if (!((fieldID == null || !(this.paidToFields[index] == fieldID) ? dataObject.GetField(this.paidToFields[index]) : currentValue) != "Broker") && (!(this.paidToFields[index] == "NEWHUD.X627") || !(dataObject.GetField("3119") != string.Empty) && !(dataObject.GetField("NEWHUD.X715") != "Include Origination Points") && !(dataObject.GetField("NEWHUD.X713") == "Origination Charge")))
        {
          string str1 = fieldID == null || !(this.amountFields[index] == fieldID) ? dataObject.GetField(this.amountFields[index]) : currentValue;
          string str2 = fieldID == null || !(this.paidByFields[index] == fieldID) ? dataObject.GetField(this.paidByFields[index]) : currentValue;
          if (!(str1 == string.Empty))
          {
            if (str2 == string.Empty && !this.UseSellerForBorrower && LOCompensationSetting.SellerFields.IndexOf("," + this.amountFields[index] + ",") > -1)
              str2 = "Seller";
            else if (this.paidByFields[index] == "NEWHUD.X227")
              str2 = "Lender";
            else if (this.paidByFields[index] == "SYS.X265")
              str2 = "";
            if (str2 == string.Empty)
              flag1 = true;
            else
              flag2 = true;
            if (flag1 & flag2)
              return true;
          }
        }
      }
      return false;
    }

    public string[] GetPaidByFields() => this.paidByFields.ToArray();

    public string[] GetAmountFields() => this.amountFields.ToArray();

    public string[] GetCompensationFields(string fieldID)
    {
      if (this.loAction == LOCompensationSetting.LOActions.NoAction)
        return (string[]) null;
      if (this.paidByFields.Contains(fieldID))
        return this.paidByFields.ToArray();
      if (this.amountFields.Contains(fieldID))
        return this.amountFields.ToArray();
      if (this.paidToFields.Contains(fieldID))
        return this.paidToFields.ToArray();
      return fieldID == "3292" || fieldID == "745" || fieldID == "NEWHUD.X1718" ? this.amountFields.ToArray() : (string[]) null;
    }

    public LOCompensationSetting.LOActions LOAction => this.loAction;

    public bool UseSellerForBorrower => this.useSellerForBorrower;

    public enum LOActions
    {
      Fix,
      WarningOnly,
      NoAction,
    }

    public enum LastCompensationActions
    {
      TakeNoAction,
      ToBorrower,
      To3rdParty,
    }
  }
}
