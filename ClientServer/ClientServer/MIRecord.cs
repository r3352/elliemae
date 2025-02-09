// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MIRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class MIRecord : IXmlSerializable
  {
    private int id;
    private string tabName = string.Empty;
    private string scenarioKey = string.Empty;
    private string uiKey = string.Empty;
    private FieldFilter[] scenarios;
    private double premium1st;
    private double subsequentPremium;
    private double monthly1st;
    private int months1st;
    private double monthly2st;
    private int months2st;
    private double cutoff;
    private int groupOrder;

    public MIRecord(DataRow r)
    {
      this.id = Utils.ParseInt((object) r[nameof (id)].ToString());
      if (r.Table.Columns.Contains(nameof (TabName)) && r[nameof (TabName)] != null)
        this.tabName = (string) r[nameof (TabName)];
      this.scenarioKey = (string) r[nameof (ScenarioKey)];
      this.premium1st = Utils.ParseDouble((object) r[nameof (Premium1st)].ToString());
      this.subsequentPremium = Utils.ParseDouble((object) r["Subsequent"].ToString());
      this.monthly1st = Utils.ParseDouble((object) r[nameof (Monthly1st)].ToString());
      this.months1st = Utils.ParseInt((object) r[nameof (Months1st)].ToString());
      this.monthly2st = Utils.ParseDouble((object) r[nameof (Monthly2st)].ToString());
      this.months2st = Utils.ParseInt((object) r[nameof (Months2st)].ToString());
      this.cutoff = Utils.ParseDouble((object) r[nameof (Cutoff)].ToString());
      this.groupOrder = Utils.ParseInt((object) r[nameof (GroupOrder)].ToString());
      this.uiKey = string.Concat(r[nameof (UIKey)]);
      this.scenarios = (FieldFilter[]) null;
    }

    public MIRecord(
      string scenarioKey,
      FieldFilter[] scenarios,
      double premium1st,
      double subsequentPremium,
      double monthly1st,
      int months1st,
      double monthly2st,
      int months2st,
      double cutoff,
      int groupOrder)
    {
      this.scenarioKey = scenarioKey;
      this.scenarios = scenarios;
      this.premium1st = premium1st;
      this.subsequentPremium = subsequentPremium;
      this.monthly1st = monthly1st;
      this.months1st = months1st;
      this.monthly2st = monthly2st;
      this.months2st = months2st;
      this.cutoff = cutoff;
      this.groupOrder = groupOrder;
    }

    public MIRecord(XmlSerializationInfo info)
    {
      this.id = info.GetInteger(nameof (Id));
      this.scenarioKey = info.GetString(nameof (ScenarioKey));
      this.scenarios = (FieldFilter[]) info.GetValue(nameof (Scenarios), typeof (FieldFilter[]));
      this.premium1st = info.GetDouble(nameof (Premium1st));
      this.subsequentPremium = info.GetDouble(nameof (SubsequentPremium));
      this.monthly1st = info.GetDouble(nameof (Monthly1st));
      this.months1st = info.GetInteger(nameof (Months1st));
      this.monthly2st = info.GetDouble(nameof (Monthly2st));
      this.months2st = info.GetInteger(nameof (Months2st));
      this.cutoff = info.GetDouble(nameof (Cutoff));
      try
      {
        this.uiKey = info.GetString(nameof (UIKey));
      }
      catch (Exception ex)
      {
        this.uiKey = "";
      }
      try
      {
        this.groupOrder = info.GetInteger(nameof (GroupOrder));
      }
      catch (Exception ex)
      {
        this.groupOrder = 0;
      }
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Id", (object) this.id);
      info.AddValue("ScenarioKey", (object) this.scenarioKey);
      info.AddValue("Scenarios", (object) this.scenarios);
      info.AddValue("Premium1st", (object) this.premium1st.ToString("N3"));
      info.AddValue("SubsequentPremium", (object) this.subsequentPremium.ToString("N3"));
      info.AddValue("Monthly1st", (object) this.monthly1st.ToString("N3"));
      info.AddValue("Months1st", (object) this.months1st.ToString("0"));
      info.AddValue("Monthly2st", (object) this.monthly2st.ToString("N3"));
      info.AddValue("Months2st", (object) this.months2st.ToString("0"));
      info.AddValue("Cutoff", (object) this.cutoff.ToString("N3"));
      info.AddValue("GroupOrder", (object) this.groupOrder.ToString("0"));
      info.AddValue("UIKey", (object) this.uiKey);
    }

    public static string GetScenarios(FieldFilter[] filters, bool useDescription)
    {
      if (filters == null)
        return string.Empty;
      string scenarios = "";
      for (int index = 0; index < filters.Length; ++index)
      {
        string str1 = scenarios + "  " + filters[index].LeftParenthesesToString;
        string str2 = (!useDescription ? str1 + filters[index].GetSimpleString(DisplayTypes.AllAndFieldID) : str1 + filters[index].GetSimpleString(DisplayTypes.All)) + filters[index].RightParenthesesToString;
        if (index + 1 < filters.Length)
          str2 = str2 + "  " + filters[index].JointTokenToString;
        scenarios = str2.Trim();
      }
      return scenarios;
    }

    public int Id
    {
      set => this.id = value;
      get => this.id;
    }

    public string TabName
    {
      set => this.tabName = value;
      get => this.tabName;
    }

    public string ScenarioKeyForUI => MIRecord.GetScenarios(this.scenarios, true);

    public string ScenarioKey => this.scenarioKey;

    public string UIKey => this.uiKey;

    public FieldFilter[] Scenarios
    {
      set => this.scenarios = value;
      get => this.scenarios;
    }

    public double Premium1st => this.premium1st;

    public double SubsequentPremium => this.subsequentPremium;

    public double Monthly1st => this.monthly1st;

    public int Months1st => this.months1st;

    public double Monthly2st => this.monthly2st;

    public int Months2st => this.months2st;

    public double Cutoff => this.cutoff;

    public int GroupOrder
    {
      set => this.groupOrder = value;
      get => this.groupOrder;
    }

    public object Clone()
    {
      MIRecord miRecord = (MIRecord) this.MemberwiseClone();
      FieldFilter[] fieldFilterArray = new FieldFilter[this.scenarios.Length];
      for (int index = 0; index < this.scenarios.Length; ++index)
        fieldFilterArray[index] = (FieldFilter) this.scenarios[index].Clone();
      miRecord.Scenarios = fieldFilterArray;
      return (object) miRecord;
    }
  }
}
