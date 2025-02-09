// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AlertFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class AlertFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static AlertFields()
    {
      AlertFields.All.Add((FieldDefinition) new AlertStatusField(AlertStatusProperty.DateActivated, "Alert Activation Date/Time", FieldFormat.DATETIME));
      AlertFields.All.Add((FieldDefinition) new AlertStatusField(AlertStatusProperty.ActivatedBy, "Alert Activated By", FieldFormat.STRING));
      AlertFields.All.Add((FieldDefinition) new AlertStatusField(AlertStatusProperty.DateCleared, "Alert Cleared Date/Time", FieldFormat.DATETIME));
      AlertFields.All.Add((FieldDefinition) new AlertStatusField(AlertStatusProperty.ClearedBy, "Alert Cleared By", FieldFormat.STRING));
    }
  }
}
