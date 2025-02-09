// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.SellConditionTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class SellConditionTemplate : ConditionTemplate
  {
    private string category = string.Empty;
    private string priorTo = string.Empty;
    private int forRoleID = -1;
    private bool allowToClear;
    private bool isInternal;
    private bool isExternal = true;
    private string tpoCondDocType = string.Empty;
    private string tpoCondDocGuid = string.Empty;
    private string providerURN = string.Empty;

    public SellConditionTemplate()
    {
    }

    public SellConditionTemplate(string guid)
      : base(guid)
    {
    }

    public SellConditionTemplate(XmlSerializationInfo info)
      : base(info)
    {
      this.category = info.GetString(nameof (Category));
      this.forRoleID = info.GetInteger(nameof (ForRoleID));
      this.allowToClear = info.GetBoolean(nameof (AllowToClear));
      this.priorTo = info.GetString(nameof (PriorTo));
      this.isInternal = info.GetBoolean(nameof (IsInternal), false);
      this.isExternal = info.GetBoolean(nameof (IsExternal), !this.isInternal);
      this.tpoCondDocType = info.GetString(nameof (TPOCondDocType), "SameNameAsCondition");
      this.tpoCondDocGuid = info.GetString(nameof (TPOCondDocGuid), string.Empty);
      this.providerURN = info.GetString(nameof (ProviderURN), string.Empty);
    }

    public override ConditionType ConditionType => ConditionType.Sell;

    public string Category
    {
      get => this.category;
      set => this.category = value;
    }

    public string ProviderURN
    {
      get => this.providerURN;
      set => this.providerURN = value;
    }

    public string PriorTo
    {
      get => this.priorTo;
      set => this.priorTo = value;
    }

    public int ForRoleID
    {
      get => this.forRoleID;
      set => this.forRoleID = value;
    }

    public bool AllowToClear
    {
      get => this.allowToClear;
      set => this.allowToClear = value;
    }

    public bool IsInternal
    {
      get => this.isInternal;
      set => this.isInternal = value;
    }

    public bool IsExternal
    {
      get => this.isExternal;
      set => this.isExternal = value;
    }

    public override ConditionLog CreateLogEntry(string addedBy, string pairId)
    {
      return (ConditionLog) new SellConditionLog(this, addedBy, pairId);
    }

    public string TPOCondDocType
    {
      get => this.tpoCondDocType;
      set => this.tpoCondDocType = value;
    }

    public string TPOCondDocGuid
    {
      get => this.tpoCondDocGuid;
      set => this.tpoCondDocGuid = value;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("Category", (object) this.category);
      info.AddValue("ForRoleID", (object) this.forRoleID);
      info.AddValue("AllowToClear", (object) this.allowToClear);
      info.AddValue("PriorTo", (object) this.priorTo);
      info.AddValue("IsInternal", (object) this.isInternal);
      info.AddValue("IsExternal", (object) this.isExternal);
      info.AddValue("TPOCondDocType", (object) this.tpoCondDocType);
      info.AddValue("TPOCondDocGuid", (object) this.tpoCondDocGuid);
      info.AddValue("ProviderURN", (object) this.providerURN);
    }
  }
}
