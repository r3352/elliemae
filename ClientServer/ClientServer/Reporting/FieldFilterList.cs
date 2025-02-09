// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.FieldFilterList
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class FieldFilterList : XmlList<FieldFilter>, ICloneable
  {
    private RelatedLoanMatchType relatedLoanMatchType;

    public FieldFilterList()
    {
    }

    public FieldFilterList(XmlSerializationInfo info)
      : base(info)
    {
      this.relatedLoanMatchType = (RelatedLoanMatchType) info.GetValue(nameof (relatedLoanMatchType), typeof (RelatedLoanMatchType), (object) RelatedLoanMatchType.None);
    }

    public FieldFilterList(FieldFilterList source)
    {
      foreach (FieldFilter filter in (List<FieldFilter>) source)
        this.Add(new FieldFilter(filter));
    }

    public FieldFilterList(FieldFilter[] filters)
      : this(filters, RelatedLoanMatchType.None)
    {
    }

    public FieldFilterList(FieldFilter[] filters, RelatedLoanMatchType matchType)
    {
      foreach (FieldFilter filter in filters)
        this.Add(new FieldFilter(filter));
      this.relatedLoanMatchType = matchType;
    }

    public RelatedLoanMatchType RelatedLoanMatchType
    {
      get => this.relatedLoanMatchType;
      set => this.relatedLoanMatchType = value;
    }

    public void Add(FieldFilter filter, JointTokens jointToken)
    {
      this.Add(filter);
      if (this.Count <= 1)
        return;
      this[this.Count - 2].JointToken = jointToken;
    }

    public void Merge(FieldFilterList filters)
    {
      this.AddRange((IEnumerable<FieldFilter>) filters);
      if (this.relatedLoanMatchType != RelatedLoanMatchType.None)
        return;
      this.relatedLoanMatchType = filters.relatedLoanMatchType;
    }

    public FilterEvaluator CreateEvaluator()
    {
      this.validateJointTokens();
      Stack<FieldFilterList.CriterionContainer> criterionContainerStack = new Stack<FieldFilterList.CriterionContainer>();
      criterionContainerStack.Push(new FieldFilterList.CriterionContainer());
      for (int index1 = 0; index1 < this.Count; ++index1)
      {
        FieldFilter filter = this[index1];
        for (int index2 = filter.LeftParentheses - filter.RightParentheses; index2 > 0; --index2)
          criterionContainerStack.Push(new FieldFilterList.CriterionContainer());
        FieldFilterList.CriterionContainer cri1 = new FieldFilterList.CriterionContainer(filter);
        criterionContainerStack.Peek().Join(cri1);
        for (int index3 = filter.RightParentheses - filter.LeftParentheses; index3 > 0; --index3)
        {
          FieldFilterList.CriterionContainer cri2 = criterionContainerStack.Count > 1 ? criterionContainerStack.Pop() : throw new Exception("The filter list does not have a balanced set of parentheses");
          criterionContainerStack.Peek().Join(cri2);
        }
      }
      return criterionContainerStack.Count == 1 ? criterionContainerStack.Pop().Current : throw new Exception("The filter list does not have a balanced set of parentheses");
    }

    public bool MatchesAllCriteria(string fieldName, object value, FilterEvaluationOption options)
    {
      this.validateJointTokens();
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) this)
      {
        if (string.Compare(fieldFilter.CriterionName, fieldName, true) == 0 && !fieldFilter.Evaluate(value, options))
          return false;
      }
      return true;
    }

    public string[] GetFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) this)
      {
        if (!stringList.Contains(fieldFilter.CriterionName))
          stringList.Add(fieldFilter.CriterionName);
      }
      return stringList.ToArray();
    }

    public string[] GetFieldIDList()
    {
      List<string> stringList = new List<string>();
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) this)
      {
        if (!stringList.Contains(fieldFilter.FieldID))
          stringList.Add(fieldFilter.FieldID);
      }
      return stringList.ToArray();
    }

    public JointTokens GetExclusiveJointToken()
    {
      this.validateJointTokens();
      JointTokens exclusiveJointToken = JointTokens.Nothing;
      foreach (FieldFilter fieldFilter in (List<FieldFilter>) this)
      {
        if (exclusiveJointToken == JointTokens.Nothing)
          exclusiveJointToken = fieldFilter.JointToken;
        else if (fieldFilter.JointToken != JointTokens.Nothing && exclusiveJointToken != fieldFilter.JointToken)
          return JointTokens.Nothing;
      }
      return exclusiveJointToken;
    }

    public FieldFilterList Combine(FieldFilterList flist, JointTokens jointToken)
    {
      this.validateJointTokens();
      flist.validateJointTokens();
      if (this.Count == 0)
        return (FieldFilterList) flist.Clone();
      if (flist.Count == 0)
        return (FieldFilterList) this.Clone();
      FieldFilterList fieldFilterList = (FieldFilterList) this.Clone();
      fieldFilterList[fieldFilterList.Count - 1].JointToken = jointToken;
      int count = fieldFilterList.Count;
      fieldFilterList.AddRange((IEnumerable<FieldFilter>) flist.Clone());
      JointTokens exclusiveJointToken = this.GetExclusiveJointToken();
      if (count == 1 || exclusiveJointToken != JointTokens.Nothing && exclusiveJointToken == flist.GetExclusiveJointToken())
        return fieldFilterList;
      ++fieldFilterList[0].LeftParentheses;
      ++fieldFilterList[count - 1].RightParentheses;
      if (flist.Count > 0)
      {
        ++fieldFilterList[count].LeftParentheses;
        ++fieldFilterList[fieldFilterList.Count - 1].RightParentheses;
      }
      return fieldFilterList;
    }

    public object Clone() => (object) new FieldFilterList(this);

    [CLSCompliant(false)]
    public string ToXML() => new XmlSerializer().Serialize((object) this);

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      if (this.relatedLoanMatchType == RelatedLoanMatchType.None)
        return;
      info.AddValue("relatedLoanMatchType", (object) this.relatedLoanMatchType);
    }

    public static FieldFilterList Parse(string xml)
    {
      return (xml ?? "") == "" ? (FieldFilterList) null : (FieldFilterList) new XmlSerializer().Deserialize(xml, typeof (FieldFilterList));
    }

    public override string ToString() => this.ToString(false);

    public string ToString(bool includeFormattingTags)
    {
      this.validateJointTokens();
      string str = "";
      for (int index = 0; index < this.Count; ++index)
      {
        FieldFilter fieldFilter = this[index];
        str = str + fieldFilter.LeftParenthesesToString + fieldFilter.GetSimpleString(DisplayTypes.All) + fieldFilter.RightParenthesesToString;
        if (index + 1 < this.Count)
          str = str + "  " + fieldFilter.JointTokenToString + "  ";
      }
      return str;
    }

    public bool IsEmpty => this.Count == 0;

    public string GetSummary()
    {
      string summary = "";
      for (int index = 0; index < this.Count; ++index)
      {
        FieldFilter fieldFilter = this[index];
        string str = summary + "  " + fieldFilter.LeftParenthesesToString + fieldFilter.GetSimpleString(DisplayTypes.All) + fieldFilter.RightParenthesesToString;
        if (index + 1 < this.Count)
          str = str + "  " + fieldFilter.JointTokenToString;
        summary = str.Trim();
      }
      return summary;
    }

    private void validateJointTokens()
    {
      for (int index = 0; index < this.Count - 1; ++index)
      {
        if (this[index].JointToken != JointTokens.And && this[index].JointToken != JointTokens.Or)
          throw new Exception("One or more Joint Tokens are not set in the filter");
      }
    }

    private class CriterionContainer
    {
      public FilterEvaluator Current = FilterEvaluator.Empty;
      public JointTokens JoinToken = JointTokens.And;

      public CriterionContainer()
      {
      }

      public CriterionContainer(FieldFilter filter)
      {
        this.Current = new FilterEvaluator((IFilterEvaluator) filter);
        this.JoinToken = filter.JointToken;
      }

      public void Join(FieldFilterList.CriterionContainer cri)
      {
        if (cri.Current == null)
          return;
        this.Current = this.Current != null ? this.Current.Join((IFilterEvaluator) cri.Current, this.JoinToken) : cri.Current;
        this.JoinToken = cri.JoinToken;
      }
    }
  }
}
