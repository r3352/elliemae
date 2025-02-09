// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AlertDataCompletionFieldCollection
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class AlertDataCompletionFieldCollection : List<AlertDataCompletionField>
  {
    public void AddIds(params string[] ids)
    {
      this.AddIds(((IEnumerable<string>) ids).AsEnumerable<string>());
    }

    public void AddIds(IEnumerable<string> ids)
    {
      foreach (string id in ids)
        this.Add(new AlertDataCompletionField(id));
    }

    public void AddField(AlertDataCompletionField field)
    {
      this.Add(new AlertDataCompletionField(field.FieldID, field.FieldType, field.ReadOnly, field.Excluded));
    }

    public void AddFields(IEnumerable<AlertDataCompletionField> fields)
    {
      foreach (AlertDataCompletionField field in fields)
        this.Add(new AlertDataCompletionField(field.FieldID, field.FieldType, field.ReadOnly, field.Excluded));
    }

    public bool RemoveField(string fieldID)
    {
      AlertDataCompletionField dataCompletionField = this.Get(fieldID);
      return dataCompletionField != null && !dataCompletionField.ReadOnly && this.Remove(dataCompletionField);
    }

    public void RemoveFields(IEnumerable<AlertDataCompletionField> fields)
    {
      if (fields == null || !fields.Any<AlertDataCompletionField>())
        return;
      foreach (AlertDataCompletionField field in fields)
        this.RemoveField(field.FieldID);
    }

    public bool Exists(string fieldID)
    {
      return !string.IsNullOrWhiteSpace(fieldID) && this != null && this.Count != 0 && this.Any<AlertDataCompletionField>((Func<AlertDataCompletionField, bool>) (field => field.FieldID.Equals(fieldID, StringComparison.InvariantCultureIgnoreCase)));
    }

    public AlertDataCompletionField Get(string fieldID)
    {
      return string.IsNullOrWhiteSpace(fieldID) || this == null || this.Count == 0 ? (AlertDataCompletionField) null : this.Find((Predicate<AlertDataCompletionField>) (field => string.Compare(field.FieldID, fieldID, StringComparison.InvariantCultureIgnoreCase) == 0));
    }

    public bool IsExcluded(string fieldID, bool throwException)
    {
      AlertDataCompletionField dataCompletionField = this.Get(fieldID);
      if (dataCompletionField != null)
        return dataCompletionField.Excluded;
      if (throwException)
        throw new Exception("Field " + fieldID + " does not exists.");
      return false;
    }

    public AlertDataCompletionFieldCollection Clone()
    {
      AlertDataCompletionFieldCollection completionFieldCollection = new AlertDataCompletionFieldCollection();
      foreach (AlertDataCompletionField dataCompletionField in (List<AlertDataCompletionField>) this)
        completionFieldCollection.Add(dataCompletionField.Clone());
      return completionFieldCollection;
    }
  }
}
