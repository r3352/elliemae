// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AlertDataCompletionField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class AlertDataCompletionField
  {
    private string fieldId;
    private AlertDataCompletionFieldType fieldType;
    private bool @readonly;
    private bool excluded;

    public AlertDataCompletionField(string fieldId)
      : this(fieldId, AlertDataCompletionFieldType.Standard, true, false)
    {
    }

    public AlertDataCompletionField(
      string fieldId,
      AlertDataCompletionFieldType fieldType,
      bool @readonly,
      bool excluded)
    {
      this.fieldId = fieldId;
      this.fieldType = fieldType;
      this.@readonly = @readonly;
      this.excluded = excluded;
    }

    public string FieldID => this.fieldId;

    public AlertDataCompletionFieldType FieldType => this.fieldType;

    public bool ReadOnly => this.@readonly;

    public bool Excluded
    {
      get => this.excluded;
      set => this.excluded = value;
    }

    public AlertDataCompletionField Clone()
    {
      return new AlertDataCompletionField(this.fieldId, this.fieldType, this.@readonly, this.excluded);
    }
  }
}
