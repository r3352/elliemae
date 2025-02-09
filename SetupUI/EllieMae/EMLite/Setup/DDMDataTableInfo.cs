// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DDMDataTableInfo
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DDMDataTableInfo
  {
    public string Name { get; set; }

    public string Description { get; set; }

    public DDMDataTableFieldInfo[] Fields { get; set; }

    public string GetFieldIdListString()
    {
      return this.Fields != null && this.Fields.Length != 0 ? string.Join("|", ((IEnumerable<DDMDataTableFieldInfo>) this.Fields).Where<DDMDataTableFieldInfo>((Func<DDMDataTableFieldInfo, bool>) (s => !s.IsOutput)).Select<DDMDataTableFieldInfo, string>((Func<DDMDataTableFieldInfo, string>) (s => s.FieldId)).ToArray<string>()) : "";
    }

    public string GetOutputIdListString()
    {
      return this.Fields != null && this.Fields.Length != 0 ? string.Join("|", ((IEnumerable<DDMDataTableFieldInfo>) this.Fields).Where<DDMDataTableFieldInfo>((Func<DDMDataTableFieldInfo, bool>) (s => s.IsOutput)).Select<DDMDataTableFieldInfo, string>((Func<DDMDataTableFieldInfo, string>) (s => s.FieldId)).ToArray<string>()) : "";
    }

    public int GetNonOutputFieldCount()
    {
      return ((IEnumerable<DDMDataTableFieldInfo>) this.Fields).Where<DDMDataTableFieldInfo>((Func<DDMDataTableFieldInfo, bool>) (f => !f.IsOutput)).Count<DDMDataTableFieldInfo>();
    }

    public int GetOutputFieldCount()
    {
      return ((IEnumerable<DDMDataTableFieldInfo>) this.Fields).Where<DDMDataTableFieldInfo>((Func<DDMDataTableFieldInfo, bool>) (f => f.IsOutput)).Count<DDMDataTableFieldInfo>();
    }
  }
}
