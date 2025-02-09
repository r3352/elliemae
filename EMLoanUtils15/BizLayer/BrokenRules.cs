// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BizLayer.BrokenRules
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer.Core;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.BizLayer
{
  [Serializable]
  public class BrokenRules
  {
    private BrokenRules.RulesCollection _brokenRules = new BrokenRules.RulesCollection();
    [NotUndoable]
    [NonSerialized]
    private object _target;
    [NotUndoable]
    [NonSerialized]
    private HybridDictionary _rulesList;
    private BrokenRules.RulesCollection _rules = new BrokenRules.RulesCollection();

    public void SetTargetObject(object target) => this._target = target;

    private HybridDictionary RulesList
    {
      get
      {
        if (this._rulesList == null)
          this._rulesList = new HybridDictionary();
        return this._rulesList;
      }
    }

    private ArrayList GetRulesForName(string ruleName)
    {
      ArrayList rulesForName = (ArrayList) this.RulesList[(object) ruleName];
      if (rulesForName == null)
      {
        rulesForName = new ArrayList();
        this.RulesList.Add((object) ruleName, (object) rulesForName);
      }
      return rulesForName;
    }

    public void AddRule(BrokenRules.RuleHandler handler, string ruleName)
    {
      this.GetRulesForName(ruleName).Add((object) new BrokenRules.RuleMethod(this._target, handler, ruleName, BrokenRules.RuleArgs.Empty));
    }

    public void AddRule(
      BrokenRules.RuleHandler handler,
      string ruleName,
      BrokenRules.RuleArgs ruleArgs)
    {
      this.GetRulesForName(ruleName).Add((object) new BrokenRules.RuleMethod(this._target, handler, ruleName, ruleArgs));
    }

    public void AddRule(BrokenRules.RuleHandler handler, string ruleName, string propertyName)
    {
      this.GetRulesForName(ruleName).Add((object) new BrokenRules.RuleMethod(this._target, handler, ruleName, propertyName));
    }

    public void CheckRules(string ruleName)
    {
      ArrayList rules = (ArrayList) this.RulesList[(object) ruleName];
      if (rules == null)
        return;
      foreach (BrokenRules.RuleMethod rule in rules)
      {
        if (rule.Invoke())
          this.UnBreakRule(rule);
        else
          this.BreakRule(rule);
      }
    }

    public void CheckRules()
    {
      foreach (DictionaryEntry rules in this.RulesList)
      {
        foreach (BrokenRules.RuleMethod rule in (ArrayList) rules.Value)
        {
          if (rule.Invoke())
            this.UnBreakRule(rule);
          else
            this.BreakRule(rule);
        }
      }
    }

    private void UnBreakRule(BrokenRules.RuleMethod rule)
    {
      if (rule.RuleArgs.PropertyName == null)
        this.Assert(rule.ToString(), string.Empty, false);
      else
        this.Assert(rule.ToString(), string.Empty, rule.RuleArgs.PropertyName, false);
    }

    private void BreakRule(BrokenRules.RuleMethod rule)
    {
      if (rule.RuleArgs.PropertyName == null)
        this.Assert(rule.ToString(), rule.Description, true);
      else
        this.Assert(rule.ToString(), rule.Description, rule.RuleArgs.PropertyName, true);
    }

    public void Assert(string name, string description, bool isBroken)
    {
      if (isBroken)
        this._rules.Add(name, description);
      else
        this._rules.Remove(name);
    }

    public void Assert(string rule, string description, string property, bool isBroken)
    {
      if (isBroken)
        this._rules.Add(rule, description, property);
      else
        this._rules.Remove(rule);
    }

    public bool IsValid => this._rules.Count == 0;

    public bool IsBroken(string name) => this._rules.Contains(name);

    public BrokenRules.RulesCollection BrokenRulesCollection => this._rules;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (BrokenRules.Rule rule in (CollectionBase) this._rules)
      {
        if (flag)
          flag = false;
        else
          stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(rule.Description);
      }
      return stringBuilder.ToString();
    }

    [Serializable]
    public struct Rule
    {
      private string _name;
      private string _description;
      private string _property;

      internal Rule(string name, string description)
      {
        this._name = name;
        this._description = description;
        this._property = string.Empty;
      }

      internal Rule(string name, string description, string property)
      {
        this._name = name;
        this._description = description;
        this._property = property;
      }

      public string Name
      {
        get => this._name;
        set
        {
        }
      }

      public string Description
      {
        get => this._description;
        set
        {
        }
      }

      public string Property
      {
        get => this._property;
        set
        {
        }
      }
    }

    [Serializable]
    public class RulesCollection : BindableCollectionBase
    {
      private bool _validToEdit;

      public BrokenRules.Rule this[int index] => (BrokenRules.Rule) this.List[index];

      public BrokenRules.Rule RuleForProperty(string property)
      {
        foreach (BrokenRules.Rule rule in (IEnumerable) this.List)
        {
          if (rule.Property == property)
            return rule;
        }
        return new BrokenRules.Rule();
      }

      internal RulesCollection()
      {
        this.AllowEdit = false;
        this.AllowRemove = false;
        this.AllowNew = false;
      }

      internal void Add(string name, string description)
      {
        this.Remove(name);
        this._validToEdit = true;
        this.List.Add((object) new BrokenRules.Rule(name, description));
        this._validToEdit = false;
      }

      internal void Add(string name, string description, string property)
      {
        this.Remove(name);
        this._validToEdit = true;
        this.List.Add((object) new BrokenRules.Rule(name, description, property));
        this._validToEdit = false;
      }

      internal void Remove(string name)
      {
        this._validToEdit = true;
        for (int index = 0; index < this.List.Count; ++index)
        {
          if (((BrokenRules.Rule) this.List[index]).Name == name)
          {
            this.List.Remove(this.List[index]);
            break;
          }
        }
        this._validToEdit = false;
      }

      internal bool Contains(string name)
      {
        for (int index = 0; index < this.List.Count; ++index)
        {
          if (((BrokenRules.Rule) this.List[index]).Name == name)
            return true;
        }
        return false;
      }

      protected override void OnClear()
      {
        if (!this._validToEdit)
          throw new NotSupportedException("Clear is an invalid operation");
      }

      protected override void OnInsert(int index, object val)
      {
        if (!this._validToEdit)
          throw new NotSupportedException("Insert is an invalid operation");
      }

      protected override void OnRemove(int index, object val)
      {
        if (!this._validToEdit)
          throw new NotSupportedException("Remove is an invalid operation");
      }

      protected override void OnSet(int index, object oldValue, object newValue)
      {
        if (!this._validToEdit)
          throw new NotSupportedException("Changing an element is an invalid operation");
      }
    }

    public delegate bool RuleHandler(object target, BrokenRules.RuleArgs e);

    public class RuleArgs
    {
      private string _propertyName;
      private string _description;
      private static BrokenRules.RuleArgs _emptyArgs = new BrokenRules.RuleArgs();

      public string PropertyName => this._propertyName;

      public string Description
      {
        get => this._description;
        set => this._description = value;
      }

      public RuleArgs()
      {
      }

      public RuleArgs(string propertyName) => this._propertyName = propertyName;

      public static BrokenRules.RuleArgs Empty => BrokenRules.RuleArgs._emptyArgs;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class DescriptionAttribute : Attribute
    {
      private string _text = string.Empty;

      public DescriptionAttribute(string description) => this._text = description;

      public override string ToString() => this._text;
    }

    private class RuleMethod
    {
      private BrokenRules.RuleHandler _handler;
      private object _target;
      private string _ruleName;
      private BrokenRules.RuleArgs _args;
      private string _description;

      public override string ToString()
      {
        return this.RuleArgs.PropertyName == null ? this._handler.Method.Name : this._handler.Method.Name + "!" + this.RuleArgs.PropertyName;
      }

      public BrokenRules.RuleHandler Handler => this._handler;

      public string RuleName => this._ruleName;

      public BrokenRules.RuleArgs RuleArgs => this._args;

      public string Description
      {
        get
        {
          return this._args.Description != null && this._args.Description.Length > 0 ? string.Format(this._args.Description, (object) this.RuleName, (object) this.RuleArgs.PropertyName, (object) this.TypeName(this._target), (object) this._target.ToString()) : string.Format(this._description, (object) this.RuleName, (object) this.RuleArgs.PropertyName, (object) this.TypeName(this._target), (object) this._target.ToString());
        }
      }

      private string TypeName(object obj) => obj.GetType().Name;

      private string GetDescription(BrokenRules.RuleHandler handler)
      {
        object[] customAttributes = handler.Method.GetCustomAttributes(typeof (BrokenRules.DescriptionAttribute), false);
        return customAttributes.Length != 0 ? customAttributes[0].ToString() : "{2}.{0}:<no description>";
      }

      public RuleMethod(
        object target,
        BrokenRules.RuleHandler handler,
        string ruleName,
        BrokenRules.RuleArgs ruleArgs)
      {
        this._target = target;
        this._handler = handler;
        this._description = this.GetDescription(handler);
        this._ruleName = ruleName;
        this._args = ruleArgs;
      }

      public RuleMethod(
        object target,
        BrokenRules.RuleHandler handler,
        string ruleName,
        string propertyName)
      {
        this._target = target;
        this._handler = handler;
        this._description = this.GetDescription(handler);
        this._ruleName = ruleName;
        this._args = new BrokenRules.RuleArgs(propertyName);
      }

      public bool Invoke() => this._handler(this._target, this._args);
    }
  }
}
