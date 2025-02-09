// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PrintSelectionEvent
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PrintSelectionEvent : IXmlSerializable
  {
    private PrintSelectionCondition[] conditions;
    private FormInfo[] selectedForms;
    private List<FormInfo> temporarySelectedForms = new List<FormInfo>();

    public PrintSelectionEvent(PrintSelectionCondition condition, FormInfo[] selectedForms)
      : this(new PrintSelectionCondition[1]{ condition }, selectedForms)
    {
    }

    public PrintSelectionEvent(PrintSelectionCondition[] conditions, FormInfo[] selectedForms)
    {
      if (conditions == null || selectedForms == null || selectedForms.Length == 0)
        throw new ArgumentNullException("Must specify a valid condition and form for Print Auto Selection event");
      this.conditions = conditions.Length != 0 ? conditions : throw new ArgumentException("Must specify at least one valid condition for Print Auto Selection event");
      this.selectedForms = selectedForms;
    }

    public PrintSelectionEvent(XmlSerializationInfo info)
    {
      XmlList<PrintSelectionConditionType> xmlList = (XmlList<PrintSelectionConditionType>) info.GetValue("conditionTypes", typeof (XmlList<PrintSelectionConditionType>));
      List<PrintSelectionCondition> selectionConditionList = new List<PrintSelectionCondition>();
      for (int index = 0; index < xmlList.Count; ++index)
        selectionConditionList.Add((PrintSelectionCondition) info.GetValue("condition" + (object) index, PrintSelectionEvent.getConditionObjectType(xmlList[index])));
      this.conditions = selectionConditionList.ToArray();
    }

    public PrintSelectionCondition[] Conditions
    {
      get => this.conditions;
      set => this.conditions = value;
    }

    public void AddForm(FormInfo form)
    {
      this.temporarySelectedForms.Add(form);
      this.selectedForms = this.temporarySelectedForms.ToArray();
    }

    public FormInfo[] SelectedForms
    {
      get => this.selectedForms;
      set => this.selectedForms = value;
    }

    public string[] GetActivationFields()
    {
      List<string> stringList = new List<string>();
      foreach (PrintSelectionCondition condition in this.conditions)
      {
        if (!stringList.Contains(condition.FieldID))
          stringList.Add(condition.FieldID);
      }
      return stringList.ToArray();
    }

    public string ToXml() => new XmlSerializer().Serialize((object) this);

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      XmlList<PrintSelectionConditionType> xmlList = new XmlList<PrintSelectionConditionType>();
      for (int index = 0; index < this.conditions.Length; ++index)
      {
        xmlList.Add(this.conditions[index].ConditionType);
        info.AddValue("condition" + (object) index, (object) this.conditions[index]);
      }
      info.AddValue("conditionTypes", (object) xmlList);
    }

    private static Type getConditionObjectType(PrintSelectionConditionType conditionType)
    {
      switch (conditionType)
      {
        case PrintSelectionConditionType.FixedValue:
          return typeof (PrintSelectionFixedValueCondition);
        case PrintSelectionConditionType.Range:
          return typeof (PrintSelectionRangeCondition);
        case PrintSelectionConditionType.ValueList:
          return typeof (PrintSelectionValueListCondition);
        case PrintSelectionConditionType.NonEmptyValue:
          return typeof (PrintSelectionNonEmptyValueCondition);
        default:
          return (Type) null;
      }
    }
  }
}
