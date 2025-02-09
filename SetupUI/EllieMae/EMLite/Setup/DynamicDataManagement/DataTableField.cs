// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataTableField
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataTableField
  {
    public string FieldID { get; set; }

    public string Description { get; set; }

    public ReportingDatabaseColumnType rCriteria { get; set; }

    public FieldFormat FieldType { get; set; }

    public FieldValueBase Value { get; set; }

    public bool IsOutput { get; set; }

    public bool IsDirty { get; set; }
  }
}
