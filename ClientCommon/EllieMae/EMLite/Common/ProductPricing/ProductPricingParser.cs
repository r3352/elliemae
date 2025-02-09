// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ProductPricing.ProductPricingParser
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Collections;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common.ProductPricing
{
  public class ProductPricingParser
  {
    private Dictionary<string, string> rawData;
    private ObservableHashtable currentData;
    private List<string> uncleanFormFields = new List<string>();
    private const string externalActionKey = "ExternalAction";
    private string importArea = "";
    private Hashtable originalScreenData;
    private bool isValidateLockTx;

    public ProductPricingParser(string partnerNameValuePairData, Hashtable currentScreenData)
    {
      this.initParams(partnerNameValuePairData, currentScreenData);
      this.merge();
    }

    public Hashtable MergedData
    {
      get
      {
        if (this.currentData != null && this.originalScreenData != null)
        {
          foreach (string key in (IEnumerable) this.currentData.Keys)
          {
            if (this.originalScreenData[(object) key] != null)
              this.originalScreenData[(object) key] = this.currentData[(object) key];
            else
              this.originalScreenData.Add((object) key, this.currentData[(object) key]);
          }
        }
        return this.originalScreenData;
      }
    }

    public Dictionary<string, string> FormatedPartnerData => this.rawData;

    public ExternalAction ExternalActionRequested
    {
      get
      {
        if (!this.rawData.ContainsKey("ExternalAction"))
          return ExternalAction.None;
        switch (this.rawData["ExternalAction"])
        {
          case "LockAndConfirm":
            return ExternalAction.LockAndConfirm;
          case "DenyLock":
            return ExternalAction.DenyLock;
          default:
            return ExternalAction.None;
        }
      }
    }

    private void initParams(string partnerNameValuePairData, Hashtable currentScreenData)
    {
      this.originalScreenData = currentScreenData;
      this.currentData = ObservableHashtable.ConvertFrom(currentScreenData);
      this.rawData = new ValuePairXmlWriter(partnerNameValuePairData, "FieldID", "FieldValue").GetKeyValuePair();
      if (this.rawData.ContainsKey("Import"))
        this.importArea = this.rawData["Import"].ToLower();
      if (this.rawData.ContainsKey("ValidateLock") && this.rawData["ValidateLock"].ToLower() == "true")
        this.isValidateLockTx = true;
      this.initUncleanFields();
      this.performCalculation();
      this.formatFieldValues();
      this.currentData.BeforeChangeItem += new HashtableEventHandler(this.beforeCurrentDataChangeListener);
    }

    private void initUncleanFields()
    {
      if (!this.rawData.ContainsKey("UncleanFields") || string.IsNullOrEmpty(this.rawData["UncleanFields"]))
        return;
      string[] strArray = this.rawData["UncleanFields"].ToLower().Split(',');
      if (strArray == null)
        return;
      foreach (string str1 in strArray)
      {
        string fieldID = str1.Trim();
        if (!this.isExternalActionID(fieldID))
        {
          string str2 = "";
          if (this.importArea == "buy" || this.importArea == "sell" || this.importArea == "both" || this.importArea == "")
            str2 = this.getBuySideFieldID(fieldID);
          if (!string.IsNullOrEmpty(str2))
            this.uncleanFormFields.Add(str2);
        }
      }
    }

    private void merge()
    {
      if (this.importArea == "sell")
      {
        this.cleanSellSideData();
        if (this.currentData.ContainsKey((object) "2220"))
          this.currentData[(object) "2220"] = (object) DateTime.Now.ToString("MM/dd/yyyy");
        else
          this.currentData.Add((object) "2220", (object) DateTime.Now.ToString("MM/dd/yyyy"));
      }
      else if (this.importArea == "buy" || this.importArea == "")
      {
        this.cleanBuySideData(this.isValidateLockTx);
        if (!this.isValidateLockTx)
        {
          if (this.currentData.ContainsKey((object) "2149"))
            this.currentData[(object) "2149"] = (object) DateTime.Now.ToString("MM/dd/yyyy");
          else
            this.currentData.Add((object) "2149", (object) DateTime.Now.ToString("MM/dd/yyyy"));
        }
        if (this.currentData.ContainsKey((object) "2029"))
          this.currentData[(object) "2029"] = (object) Session.UserInfo.FullName;
        else
          this.currentData.Add((object) "2029", (object) Session.UserInfo.FullName);
      }
      else if (this.importArea == "both")
      {
        this.cleanBuySideData(false);
        this.cleanSellSideData();
        if (!this.isValidateLockTx)
        {
          if (this.currentData.ContainsKey((object) "2149"))
            this.currentData[(object) "2149"] = (object) DateTime.Now.ToString("MM/dd/yyyy");
          else
            this.currentData.Add((object) "2149", (object) DateTime.Now.ToString("MM/dd/yyyy"));
          if (this.currentData.ContainsKey((object) "2220"))
            this.currentData[(object) "2220"] = (object) DateTime.Now.ToString("MM/dd/yyyy");
          else
            this.currentData.Add((object) "2220", (object) DateTime.Now.ToString("MM/dd/yyyy"));
        }
        if (this.currentData.ContainsKey((object) "2029"))
          this.currentData[(object) "2029"] = (object) Session.UserInfo.FullName;
        else
          this.currentData.Add((object) "2029", (object) Session.UserInfo.FullName);
      }
      else
        this.cleanBuySideData(false);
      if (this.importArea == "buy" || this.importArea == "sell" || this.importArea == "both" || this.importArea == "")
      {
        foreach (string key in this.rawData.Keys)
        {
          if (!this.isExternalActionID(key) && (!this.isValidateLockTx || !(key == "2150")))
          {
            string buySideFieldId = this.getBuySideFieldID(key);
            if (this.currentData.ContainsKey((object) buySideFieldId))
            {
              if (buySideFieldId == "2204")
              {
                string str = "";
                if (this.isValidateLockTx && !string.IsNullOrEmpty(this.currentData[(object) buySideFieldId].ToString()))
                  str = this.currentData[(object) buySideFieldId].ToString() + "\r\n\r\n";
                this.currentData[(object) buySideFieldId] = (object) (str + this.rawData[key]);
              }
              else
                this.currentData[(object) buySideFieldId] = (object) this.rawData[key];
            }
            else
              this.currentData.Add((object) buySideFieldId, (object) this.rawData[key]);
          }
        }
      }
      if (!(this.importArea == "buy") || !this.isValidateLockTx)
        return;
      new LockRequestCalculator(Session.SessionObjects, (LoanData) null).PerformSnapshotCalculations((Hashtable) this.currentData);
      Dictionary<string, string> requestMap = LoanLockUtils.InitializePricingFieldsBuySideToRequestMap();
      foreach (string key in requestMap.Keys)
      {
        if (this.currentData[(object) key] == null || Convert.ToString(this.currentData[(object) key]) == "")
        {
          if (this.currentData.Contains((object) requestMap[key]))
            this.currentData.Remove((object) requestMap[key]);
        }
        else
          this.currentData[(object) requestMap[key]] = this.currentData[(object) key];
      }
      this.currentData[(object) "2144"] = this.currentData[(object) "2204"] == null ? (object) "" : this.currentData[(object) "2204"];
      if (Convert.ToString(this.currentData[(object) "2626"]) == "Correspondent")
        this.currentData[(object) "3965"] = this.currentData[(object) "3911"] == null ? (object) "" : this.currentData[(object) "3911"];
      if (this.currentData[(object) "3363"] == null || !(Convert.ToString(this.currentData[(object) "3363"]) != ""))
        return;
      this.currentData[(object) "2091"] = this.currentData[(object) "3358"] == null ? (object) "" : this.currentData[(object) "3358"];
      this.currentData[(object) "3369"] = this.currentData[(object) "2151"] == null ? (object) "" : this.currentData[(object) "2151"];
      this.currentData[(object) "3360"] = this.currentData[(object) "3363"] == null ? (object) "" : this.currentData[(object) "3363"];
      this.currentData[(object) "3361"] = this.currentData[(object) "3364"] == null ? (object) "" : this.currentData[(object) "3364"];
      this.currentData[(object) "3362"] = this.currentData[(object) "3365"] == null ? (object) "" : this.currentData[(object) "3365"];
    }

    private bool isUncleanField(string fieldId)
    {
      return !string.IsNullOrEmpty(this.uncleanFormFields.Find((Predicate<string>) (s => s == fieldId.ToLower())));
    }

    private string getBuySideFieldID(string fieldID)
    {
      string buySideFieldId = fieldID;
      if (!Utils.IsInt((object) fieldID))
        return fieldID;
      int num = int.Parse(fieldID);
      if (num > 2087 && num < 2122)
      {
        buySideFieldId = string.Concat((object) (num + 60));
      }
      else
      {
        switch (num)
        {
          case 2142:
            buySideFieldId = "2202";
            break;
          case 2143:
            buySideFieldId = "2203";
            break;
          case 2848:
            buySideFieldId = "2205";
            break;
          default:
            if (num > 2413 && num < 2420)
            {
              buySideFieldId = string.Concat((object) (num + 34));
              break;
            }
            if (num > 2646 && num < 2690)
            {
              buySideFieldId = string.Concat((object) (num + 86));
              break;
            }
            break;
        }
      }
      return buySideFieldId;
    }

    private bool isExternalActionID(string fieldID) => fieldID == "ExternalAction";

    private void cleanBuySideData(bool isValidateLockTx)
    {
      if (!isValidateLockTx)
      {
        if (this.currentData.ContainsKey((object) "2205"))
          this.currentData[(object) "2205"] = (object) "";
        if (this.currentData.ContainsKey((object) "2215"))
          this.currentData[(object) "2215"] = (object) "";
      }
      for (int index = 2148; index <= 2203; ++index)
      {
        if ((!isValidateLockTx || index != 2149 && index != 2150 && index != 2151) && this.currentData.ContainsKey((object) index.ToString()))
          this.currentData[(object) index.ToString()] = (object) "";
      }
      for (int index = 2448; index <= 2481; ++index)
      {
        if (this.currentData.ContainsKey((object) index.ToString()))
          this.currentData[(object) index.ToString()] = (object) "";
      }
      for (int index = 2733; index <= 2775; ++index)
      {
        if (this.currentData.ContainsKey((object) index.ToString()))
          this.currentData[(object) index.ToString()] = (object) "";
      }
      if (!this.rawData.ContainsKey("3380"))
        return;
      for (int index = 3380; index <= 3407; ++index)
      {
        if (this.currentData.ContainsKey((object) index.ToString()))
          this.currentData[(object) index.ToString()] = (object) "";
      }
    }

    private void cleanSellSideData()
    {
      if (this.currentData.ContainsKey((object) "2817"))
        this.currentData[(object) "2817"] = (object) "";
      if (this.currentData.ContainsKey((object) "2818"))
        this.currentData[(object) "2818"] = (object) "";
      if (this.currentData.ContainsKey((object) "3123"))
        this.currentData[(object) "3123"] = (object) "";
      for (int index = 2219; index <= 2252; ++index)
      {
        if (this.currentData.ContainsKey((object) index.ToString()))
          this.currentData[(object) index.ToString()] = (object) "";
      }
      for (int index = 2273; index <= 2297; ++index)
      {
        if (this.currentData.ContainsKey((object) index.ToString()))
          this.currentData[(object) index.ToString()] = (object) "";
      }
      for (int index = 2482; index <= 2487; ++index)
      {
        if (this.currentData.ContainsKey((object) index.ToString()))
          this.currentData[(object) index.ToString()] = (object) "";
      }
      for (int index = 2776; index <= 2784; ++index)
      {
        if (this.currentData.ContainsKey((object) index.ToString()))
          this.currentData[(object) index.ToString()] = (object) "";
      }
      this.currentData[(object) "3055"] = (object) "";
    }

    private void performCalculation()
    {
      if (this.rawData.ContainsKey("3043"))
        this.calculateLockRequestLoan("3043");
      this.CalculateBasePrice();
    }

    private void CalculateBasePrice()
    {
      if (!(Session.LoanData.GetField("2626") == "Correspondent") || !this.rawData.ContainsKey("OPTIMAL.HISTORY"))
        return;
      Decimal fromEppsTxHistory = LockUtils.GetSRPFromEppsTxHistory(this.rawData["OPTIMAL.HISTORY"], false);
      if (!(fromEppsTxHistory != 0M))
        return;
      this.rawData["2205"] = fromEppsTxHistory.ToString();
      if (!this.rawData.ContainsKey("2161"))
        return;
      this.rawData["2161"] = (Utils.ParseDecimal((object) this.rawData["2161"]) - fromEppsTxHistory).ToString();
    }

    private void calculateLockRequestLoan(string id)
    {
      bool flag1 = true;
      bool flag2 = this.getValue("2952", "") == "FHA";
      bool flag3 = this.getValue("3046", "") == "Y";
      double num1 = double.Parse(this.getValue("3047", "0"));
      double num2 = double.Parse(this.getValue("3043", "0"));
      if (id == "3043" || id == "2952")
      {
        if (flag2)
          num2 = (double) (int) Utils.ParseDouble((object) double.Parse(this.getValue("3043", "0")));
        else if (flag1)
          num2 = Utils.ArithmeticRounding(Utils.ParseDouble((object) double.Parse(this.getValue("3043", "0"))), 0);
        this.rawData[id] = num2.ToString();
      }
      if (flag3)
      {
        this.rawData["3044"] = (num2 == 0.0 ? 0.0 : Utils.ArithmeticRounding(double.Parse(this.getValue("3045", "0")) / num2 * 100.0, 6)).ToString();
      }
      else
      {
        double num3 = !flag2 ? Utils.ArithmeticRounding(num2 * double.Parse(this.getValue("3044", "0")) / 100.0, 2) : Convert.ToDouble(Math.Truncate(100M * Convert.ToDecimal(num2 * double.Parse(this.getValue("3044", "0")) / 100.0)) / 100M);
        this.rawData["3045"] = num3.ToString();
        num1 = Math.Round(num3 % 1.0, 2);
        this.rawData["3047"] = num1.ToString();
      }
      double num4 = num2 + double.Parse(this.getValue("3045", "0"));
      double val = double.Parse(this.getValue("3047", "0")) + double.Parse(this.getValue("3049", "0")) <= double.Parse(this.getValue("3045", "0")) ? num4 - (double.Parse(this.getValue("3047", "0")) + double.Parse(this.getValue("3049", "0"))) : num4 - double.Parse(this.getValue("3045", "0"));
      double num5 = val;
      if (flag2)
        num5 = (double) (int) val;
      else if (flag1)
        num5 = Utils.ArithmeticRounding(val, 0);
      if (num5 != val)
      {
        this.rawData["3047"] = Utils.ArithmeticRounding(num5 >= val ? num1 - (num5 - val) : num1 + val - num5, 2).ToString();
        val = num5;
      }
      this.rawData["2965"] = val.ToString();
    }

    private string getValue(string key, string defaultValue)
    {
      if (this.rawData.ContainsKey(key))
        return this.rawData[key];
      return !this.currentData.ContainsKey((object) key) ? defaultValue : string.Concat(this.currentData[(object) key]);
    }

    private void formatFieldValues()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      foreach (string key in this.rawData.Keys)
      {
        dictionary.Add(key, this.rawData[key]);
        FieldDefinition fieldDefinition = Session.LoanData.GetFieldDefinition(key);
        switch (key.ToLower())
        {
          case "2947":
          case "2951":
          case "2952":
          case "2953":
          case "2958":
          case "2961":
          case "2962":
          case "2963":
          case "3046":
          case "4463":
          case "optimal.history":
            continue;
          default:
            dictionary[key] = fieldDefinition.ToDisplayValue(this.rawData[key]);
            continue;
        }
      }
      this.rawData = dictionary;
    }

    private void beforeCurrentDataChangeListener(object sender, HashtableEventArgs args)
    {
      string key = (string) args.Key;
      if (!string.IsNullOrEmpty((string) args.Value))
        args.KeepChanges = true;
      else if (this.isUncleanField(key))
        args.KeepChanges = false;
      else
        args.KeepChanges = true;
    }
  }
}
