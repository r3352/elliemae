// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Workflow.MilestoneTemplateSimpleCondition
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Workflow
{
  public class MilestoneTemplateSimpleCondition : IXmlSerializable, IFilterEvaluator
  {
    private XmlList<string> loanChannels = new XmlList<string>();
    private XmlList<string> loanTypes = new XmlList<string>();
    private XmlList<string> loanPurposes = new XmlList<string>();

    public MilestoneTemplateSimpleCondition()
    {
    }

    public MilestoneTemplateSimpleCondition(XmlSerializationInfo info)
    {
      this.loanChannels = info.GetValue<XmlList<string>>(nameof (LoanChannels));
      this.loanTypes = info.GetValue<XmlList<string>>(nameof (LoanTypes));
      this.loanPurposes = info.GetValue<XmlList<string>>(nameof (LoanPurposes));
    }

    public List<string> LoanChannels => (List<string>) this.loanChannels;

    public List<string> LoanTypes => (List<string>) this.loanTypes;

    public List<string> LoanPurposes => (List<string>) this.loanPurposes;

    public bool IsEmpty()
    {
      return this.loanChannels.Count <= 0 && this.loanPurposes.Count <= 0 && this.loanTypes.Count <= 0;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("LoanChannels", (object) this.loanChannels);
      info.AddValue("LoanTypes", (object) this.loanTypes);
      info.AddValue("LoanPurposes", (object) this.loanPurposes);
    }

    public bool Evaluate(object value) => this.Evaluate(value, FilterEvaluationOption.None);

    public bool Evaluate(object value, FilterEvaluationOption options)
    {
      if (!(value is LoanData loanData))
        throw new ArgumentException("Specified value must be a LoanData object");
      return (this.loanChannels.Count <= 0 || this.loanChannels.Contains(loanData.GetField("2626"))) && (this.loanPurposes.Count <= 0 || this.loanPurposes.Contains(loanData.GetField("19"))) && (this.loanTypes.Count <= 0 || this.loanTypes.Contains(loanData.GetField("1172")));
    }

    public QueryCriterion ToQueryCriterion()
    {
      throw new NotSupportedException("The method or operation is not implemented.");
    }

    public QueryCriterion ToQueryCriterion(FilterEvaluationOption options)
    {
      throw new NotSupportedException("The method or operation is not implemented.");
    }

    public override string ToString()
    {
      string fieldValueClause1 = MilestoneTemplateSimpleCondition.getFieldValueClause("Channel", (List<string>) this.loanChannels, (FieldDefinition) StandardFields.GetField("2626"));
      string fieldValueClause2 = MilestoneTemplateSimpleCondition.getFieldValueClause("Loan Type", (List<string>) this.loanTypes, (FieldDefinition) StandardFields.GetField("1172"));
      string fieldValueClause3 = MilestoneTemplateSimpleCondition.getFieldValueClause("Loan Purpose", (List<string>) this.loanPurposes, (FieldDefinition) StandardFields.GetField("19"));
      List<string> stringList = new List<string>();
      if (fieldValueClause1 != "")
        stringList.Add(fieldValueClause1);
      if (fieldValueClause2 != "")
        stringList.Add(fieldValueClause2);
      if (fieldValueClause3 != "")
        stringList.Add(fieldValueClause3);
      return string.Join(" And ", stringList.ToArray());
    }

    private static string getFieldValueClause(
      string fieldDesc,
      List<string> values,
      FieldDefinition fieldDef)
    {
      if (values.Count == 0)
        return "";
      if (values.Count == 1)
      {
        string text = fieldDef.Options.ValueToText(values[0]);
        if (text == "")
          text = values[0];
        return fieldDesc + " = \"" + text + "\"";
      }
      List<string> stringList = new List<string>();
      foreach (string str1 in values)
      {
        string str2 = fieldDef.Options.ValueToText(str1);
        if (str2 == "")
          str2 = str1;
        stringList.Add("\"" + str2 + "\"");
      }
      return fieldDesc + " is any of (" + string.Join(", ", stringList.ToArray()) + ")";
    }
  }
}
