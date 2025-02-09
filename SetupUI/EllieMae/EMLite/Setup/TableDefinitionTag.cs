// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TableDefinitionTag
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TableDefinitionTag
  {
    public const string TBL_TYPE_DDM = "DDM";
    public const string TBL_TYPE_ESCROWTITLE = "ESCROWTITLE";
    public const string TBL_TYPE_TAX = "TAX";
    public const string TBL_TYPE_MI = "MI";

    public string TableName { get; set; }

    public string Type { get; set; }

    public string LineNumber { get; set; }

    public List<string> OutputColumns { get; set; }
  }
}
