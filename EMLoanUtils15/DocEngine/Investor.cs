// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.Investor
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class Investor
  {
    public const string GenericInvestorCode = "0000�";
    public static string[] InvestorFields = new string[2]
    {
      "PlanCode.ProgSpnsrNm",
      "PlanCode.InvCd"
    };
    public static readonly Investor GenericInvestor = new Investor("0000", "Generic");
    private string investorCode;
    private string investorName;

    internal Investor(XmlElement investorXml)
    {
      this.investorCode = investorXml.GetAttribute(nameof (InvestorCode));
      this.investorName = investorXml.GetAttribute("InvestorName");
    }

    internal Investor(string investorCode, string investorName)
    {
      this.investorCode = investorCode;
      this.investorName = investorName;
    }

    public string Name => this.investorName;

    public string InvestorCode => this.investorCode;

    public bool IsGeneric => this.investorCode == "0000";

    public void Apply(IHtmlInput dataObj, DocumentOrderType orderType)
    {
      if (this.IsGeneric)
      {
        dataObj.SetField(Plan.MapFieldIDForOrderType("PlanCode.ProgSpnsrNm", orderType), "");
        dataObj.SetField(Plan.MapFieldIDForOrderType("PlanCode.InvCd", orderType), "");
      }
      else
      {
        dataObj.SetField(Plan.MapFieldIDForOrderType("PlanCode.ProgSpnsrNm", orderType), this.Name);
        dataObj.SetField(Plan.MapFieldIDForOrderType("PlanCode.InvCd", orderType), this.InvestorCode);
      }
    }

    public override string ToString() => this.Name;

    public override bool Equals(object obj)
    {
      return obj is Investor investor && this.investorCode == investor.investorCode;
    }

    public override int GetHashCode() => this.investorCode.GetHashCode();

    public static bool IsInvestorField(string fieldId)
    {
      return Array.Find<string>(Investor.InvestorFields, (Predicate<string>) new StringPredicate(fieldId, true)) != null;
    }

    internal static Investor[] ExtractCollection(DocEngineResponse response)
    {
      List<Investor> investorList = new List<Investor>();
      foreach (XmlElement selectNode in response.ResponseXml.SelectNodes("//INVESTOR"))
        investorList.Add(new Investor(selectNode));
      return investorList.ToArray();
    }
  }
}
