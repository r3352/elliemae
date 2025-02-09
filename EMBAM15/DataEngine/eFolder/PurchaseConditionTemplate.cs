// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.PurchaseConditionTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class PurchaseConditionTemplate(string guid) : ConditionTemplate(guid)
  {
    public override ConditionType ConditionType => ConditionType.Purchase;

    public override ConditionLog CreateLogEntry(string addedBy, string pairId)
    {
      return (ConditionLog) new PurchaseConditionLog(this, addedBy, pairId);
    }

    public string Number { get; set; }

    public string Category { get; set; }

    public int Subcategory { get; set; }

    public int RoleID { get; set; }

    public bool AllowToClear { get; set; }

    public string PriorTo { get; set; }

    public bool IsInternal { get; set; }

    public bool IsExternal { get; set; }

    public List<DocumentTemplate> Documents { get; set; }
  }
}
