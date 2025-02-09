// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SimpleTradeFilter
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Collections;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class SimpleTradeFilter : TradeEntity, IXmlSerializable, IFilterEvaluator
  {
    private XmlList<string> loanPrograms = new XmlList<string>();
    private XmlList<string> milestones = new XmlList<string>();
    private Range<Decimal> noteRateRange;
    private Range<int> termRange;
    private Range<Decimal> ltvRange;
    public XmlList<string> occupancyStatuses = new XmlList<string>();
    public XmlList<string> investorStatuses = new XmlList<string>();

    public SimpleTradeFilter(bool addInvestorStatus = true)
    {
      if (!addInvestorStatus)
        return;
      this.investorStatuses.Add("");
    }

    public SimpleTradeFilter(XmlSerializationInfo info)
    {
      this.ReadId(info);
      this.loanPrograms = (XmlList<string>) info.GetValue(nameof (loanPrograms), typeof (XmlList<string>));
      this.milestones = (XmlList<string>) info.GetValue(nameof (milestones), typeof (XmlList<string>));
      this.noteRateRange = (Range<Decimal>) info.GetValue("noteRate", typeof (Range<Decimal>));
      this.termRange = (Range<int>) info.GetValue(nameof (termRange), typeof (Range<int>));
      this.ltvRange = (Range<Decimal>) info.GetValue(nameof (ltvRange), typeof (Range<Decimal>));
      this.occupancyStatuses = (XmlList<string>) info.GetValue(nameof (occupancyStatuses), typeof (XmlList<string>));
      this.investorStatuses = (XmlList<string>) info.GetValue(nameof (investorStatuses), typeof (XmlList<string>));
    }

    public SimpleTradeFilter(SimpleTradeFilter source)
    {
      this.Id = source.Id;
      this.loanPrograms = new XmlList<string>((IEnumerable<string>) source.loanPrograms);
      this.milestones = new XmlList<string>((IEnumerable<string>) source.milestones);
      this.noteRateRange = source.noteRateRange;
      this.termRange = source.termRange;
      this.ltvRange = source.ltvRange;
      this.occupancyStatuses = new XmlList<string>((IEnumerable<string>) source.occupancyStatuses);
      this.investorStatuses = new XmlList<string>((IEnumerable<string>) source.investorStatuses);
    }

    public XmlList<string> LoanPrograms
    {
      get => this.loanPrograms;
      set => this.loanPrograms = value;
    }

    public XmlList<string> Milestones => this.milestones;

    public Range<Decimal> NoteRateRange
    {
      get => this.noteRateRange;
      set => this.noteRateRange = value;
    }

    public Range<int> TermRange
    {
      get => this.termRange;
      set => this.termRange = value;
    }

    public Range<Decimal> LTVRange
    {
      get => this.ltvRange;
      set => this.ltvRange = value;
    }

    [CLSCompliant(false)]
    public XmlList<string> InvestorStatuses => this.investorStatuses;

    [CLSCompliant(false)]
    public XmlList<string> OccupancyStatuses => this.occupancyStatuses;

    public bool MatchesAllCriteria(string fieldName, object value, FilterEvaluationOption options)
    {
      switch (fieldName.ToLower())
      {
        case "loan.currentmilestonename":
          return this.milestones.Count == 0 || (options & FilterEvaluationOption.NonVolatile) != FilterEvaluationOption.None || this.milestones.Exists(new Predicate<string>(new StringSearchAlgorithm(string.Concat(value)).Match));
        case "loan.investorstatus":
          return this.investorStatuses.Count == 0 || this.investorStatuses.Exists(new Predicate<string>(new StringSearchAlgorithm(string.Concat(value)).Match));
        case "loan.loanprogram":
          return this.loanPrograms.Count == 0 || this.loanPrograms.Exists(new Predicate<string>(new StringSearchAlgorithm(string.Concat(value)).Match));
        case "loan.loanrate":
          return this.noteRateRange == null || this.noteRateRange.IsInRange(Utils.ParseDecimal(value, 0M));
        case "loan.ltv":
          return this.ltvRange == null || this.ltvRange.IsInRange(Utils.ParseDecimal(value, 0M));
        case "loan.occupancystatus":
          return this.occupancyStatuses.Count == 0 || this.occupancyStatuses.Exists(new Predicate<string>(new StringSearchAlgorithm(string.Concat(value)).Match));
        case "loan.term":
          return this.termRange == null || this.termRange.IsInRange(Utils.ParseInt(value, 0));
        default:
          return true;
      }
    }

    public string[] GetFieldList()
    {
      List<string> stringList = new List<string>();
      if (this.loanPrograms.Count > 0)
        stringList.Add("Loan.LoanProgram");
      if (this.milestones.Count > 0)
        stringList.Add("Loan.CurrentMilestoneName");
      if (this.noteRateRange != null)
        stringList.Add("Loan.LoanRate");
      if (this.termRange != null)
        stringList.Add("Loan.Term");
      if (this.ltvRange != null)
        stringList.Add("Loan.LTV");
      if (this.occupancyStatuses.Count > 0)
        stringList.Add("Loan.OccupancyStatus");
      if (this.investorStatuses.Count > 0)
        stringList.Add("Loan.InvestorStatus");
      return stringList.ToArray();
    }

    public bool Evaluate(PipelineInfo pinfo) => this.Evaluate(pinfo, FilterEvaluationOption.None);

    public bool Evaluate(PipelineInfo pinfo, FilterEvaluationOption options)
    {
      Bitmask bitmask = new Bitmask((object) options);
      return (this.loanPrograms.Count <= 0 || this.loanPrograms.Exists(new Predicate<string>(new StringSearchAlgorithm(string.Concat(pinfo.GetField("LoanProgram"))).Match))) && (bitmask.Contains((object) FilterEvaluationOption.NonVolatile) || this.milestones.Count <= 0 || this.milestones.Exists(new Predicate<string>(new StringSearchAlgorithm(string.Concat(pinfo.GetField("CurrentMilestoneName"))).Match))) && (this.noteRateRange == null || this.noteRateRange.IsInRange(Utils.ParseDecimal(pinfo.GetField("LoanRate"), 0M))) && (this.termRange == null || this.termRange.IsInRange(Utils.ParseInt(pinfo.GetField("Term"), 0))) && (this.ltvRange == null || this.ltvRange.IsInRange(Utils.ParseDecimal(pinfo.GetField("LTV"), 0M))) && (this.occupancyStatuses.Count <= 0 || this.occupancyStatuses.Exists(new Predicate<string>(new StringSearchAlgorithm(string.Concat(pinfo.GetField("OccupancyStatus"))).Match))) && (bitmask.Contains((object) FilterEvaluationOption.NonVolatile) || this.investorStatuses.Count <= 0 || this.investorStatuses.Exists(new Predicate<string>(new StringSearchAlgorithm(string.Concat(pinfo.GetField("Loan.InvestorStatus"))).Match)));
    }

    bool IFilterEvaluator.Evaluate(object data) => this.Evaluate((PipelineInfo) data);

    bool IFilterEvaluator.Evaluate(object data, FilterEvaluationOption options)
    {
      return this.Evaluate((PipelineInfo) data, options);
    }

    public QueryCriterion ToQueryCriterion() => this.ToQueryCriterion(FilterEvaluationOption.None);

    public QueryCriterion ToQueryCriterion(FilterEvaluationOption options)
    {
      return this.ConvertToFilterList().CreateEvaluator().ToQueryCriterion(options);
    }

    public string ToXML() => new XmlSerializer().Serialize((object) this);

    public FieldFilterList ConvertToFilterList()
    {
      FieldFilterList filterList = new FieldFilterList();
      if (this.loanPrograms.Count == 1)
        filterList.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "1401", "Loan.LoanProgram", "Loan Program", OperatorTypes.IsExact, this.loanPrograms[0]));
      else if (this.loanPrograms.Count > 1)
      {
        for (int index = 0; index < this.loanPrograms.Count; ++index)
        {
          FieldFilter fieldFilter = new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "1401", "Loan.LoanProgram", "Loan Program", OperatorTypes.IsExact, this.loanPrograms[index]);
          if (index == 0)
            fieldFilter.LeftParentheses = 1;
          if (index == this.loanPrograms.Count - 1)
            fieldFilter.RightParentheses = 1;
          if (index < this.loanPrograms.Count - 1)
            fieldFilter.JointToken = JointTokens.Or;
          filterList.Add(fieldFilter);
        }
      }
      if (this.milestones.Count == 1)
        filterList.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "MS.STATUS", "Loan.CurrentMilestoneName", "Current Milestone", OperatorTypes.IsExact, this.milestones[0], true));
      else if (this.milestones.Count > 1)
      {
        for (int index = 0; index < this.milestones.Count; ++index)
        {
          FieldFilter fieldFilter = new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "MS.STATUS", "Loan.CurrentMilestoneName", "Current Milestone", OperatorTypes.IsExact, this.milestones[index], true);
          if (index == 0)
            fieldFilter.LeftParentheses = 1;
          if (index == this.milestones.Count - 1)
            fieldFilter.RightParentheses = 1;
          if (index < this.milestones.Count - 1)
            fieldFilter.JointToken = JointTokens.Or;
          filterList.Add(fieldFilter);
        }
      }
      Decimal num1;
      if (this.noteRateRange != null)
      {
        if (this.noteRateRange.Minimum > 0M && this.noteRateRange.Maximum < Decimal.MaxValue)
        {
          FieldFilterList fieldFilterList = filterList;
          Decimal num2 = this.noteRateRange.Minimum;
          string valueFrom = num2.ToString("0.000");
          num2 = this.noteRateRange.Maximum;
          string valueTo = num2.ToString("0.000");
          FieldFilter fieldFilter = new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric, "3", "Loan.LoanRate", "Note Rate", OperatorTypes.Between, valueFrom, valueTo);
          fieldFilterList.Add(fieldFilter);
        }
        else if (this.noteRateRange.Minimum > 0M)
        {
          FieldFilterList fieldFilterList = filterList;
          num1 = this.noteRateRange.Minimum;
          FieldFilter fieldFilter = new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric, "3", "Loan.LoanRate", "Note Rate", OperatorTypes.NotLessThan, num1.ToString("0.000"));
          fieldFilterList.Add(fieldFilter);
        }
        else if (this.noteRateRange.Maximum < Decimal.MaxValue)
        {
          FieldFilterList fieldFilterList = filterList;
          num1 = this.noteRateRange.Maximum;
          FieldFilter fieldFilter = new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric, "3", "Loan.LoanRate", "Note Rate", OperatorTypes.NotGreaterThan, num1.ToString("0.000"));
          fieldFilterList.Add(fieldFilter);
        }
      }
      if (this.termRange != null)
      {
        if (this.termRange.Minimum > 0 && this.termRange.Maximum < int.MaxValue)
        {
          FieldFilterList fieldFilterList = filterList;
          int num3 = this.termRange.Minimum;
          string valueFrom = num3.ToString("0");
          num3 = this.termRange.Maximum;
          string valueTo = num3.ToString("0");
          FieldFilter fieldFilter = new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric, "4", "Loan.Term", "Term", OperatorTypes.Between, valueFrom, valueTo);
          fieldFilterList.Add(fieldFilter);
        }
        else if (this.termRange.Minimum > 0)
          filterList.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric, "4", "Loan.Term", "Term", OperatorTypes.NotLessThan, this.termRange.Minimum.ToString("0")));
        else if (this.termRange.Maximum < int.MaxValue)
          filterList.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric, "4", "Loan.Term", "Term", OperatorTypes.NotGreaterThan, this.termRange.Maximum.ToString("0")));
      }
      if (this.ltvRange != null)
      {
        if (this.ltvRange.Minimum > 0M && this.ltvRange.Maximum < Decimal.MaxValue)
        {
          FieldFilterList fieldFilterList = filterList;
          num1 = this.ltvRange.Minimum;
          string valueFrom = num1.ToString("0.0");
          num1 = this.ltvRange.Maximum;
          string valueTo = num1.ToString("0.0");
          FieldFilter fieldFilter = new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric, "353", "Loan.LTV", "LTV", OperatorTypes.Between, valueFrom, valueTo);
          fieldFilterList.Add(fieldFilter);
        }
        else if (this.ltvRange.Minimum > 0M)
        {
          FieldFilterList fieldFilterList = filterList;
          num1 = this.ltvRange.Minimum;
          FieldFilter fieldFilter = new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric, "353", "Loan.LTV", "LTV", OperatorTypes.NotLessThan, num1.ToString("0.0"));
          fieldFilterList.Add(fieldFilter);
        }
        else if (this.ltvRange.Maximum < Decimal.MaxValue)
        {
          FieldFilterList fieldFilterList = filterList;
          num1 = this.ltvRange.Maximum;
          FieldFilter fieldFilter = new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric, "353", "Loan.LTV", "LTV", OperatorTypes.NotGreaterThan, num1.ToString("0.0"));
          fieldFilterList.Add(fieldFilter);
        }
      }
      if (this.occupancyStatuses.Count > 0)
        filterList.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList, "1811", "Loan.OccupancyStatus", "Occupancy Status", OperatorTypes.IsAnyOf, this.occupancyStatuses.ToArray(), this.getFieldDescription((IList<string>) this.occupancyStatuses, "1811")));
      if (this.investorStatuses.Count > 0)
      {
        if (this.investorStatuses.Contains("") && !this.investorStatuses.Contains("Rejected"))
          this.investorStatuses.Add("Rejected");
        filterList.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList, "2031", "Loan.InvestorStatus", "Investor Status", OperatorTypes.IsAnyOf, this.investorStatuses.ToArray(), this.getFieldDescription((IList<string>) this.investorStatuses, "2031"), true));
      }
      for (int index = 0; index < filterList.Count - 1; ++index)
      {
        if (filterList[index].JointToken == JointTokens.Nothing)
          filterList[index].JointToken = JointTokens.And;
      }
      return filterList;
    }

    private string getFieldDescription(IList<string> values, string fieldId)
    {
      FieldDefinition fieldDefinition = StandardFields.All[fieldId];
      List<string> stringList = new List<string>();
      for (int index = 0; index < values.Count; ++index)
        stringList.Add(fieldDefinition.Options.ValueToText(values[index]));
      return string.Join(", ", stringList.ToArray());
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.WriteId(info);
      info.AddValue("loanPrograms", (object) this.loanPrograms);
      info.AddValue("milestones", (object) this.milestones);
      info.AddValue("noteRate", (object) this.noteRateRange);
      info.AddValue("termRange", (object) this.termRange);
      info.AddValue("ltvRange", (object) this.ltvRange);
      info.AddValue("occupancyStatuses", (object) this.occupancyStatuses);
      info.AddValue("investorStatuses", (object) this.investorStatuses);
    }

    public static SimpleTradeFilter Parse(string xml)
    {
      return (xml ?? "") == "" ? (SimpleTradeFilter) null : (SimpleTradeFilter) new XmlSerializer().Deserialize(xml, typeof (SimpleTradeFilter));
    }
  }
}
