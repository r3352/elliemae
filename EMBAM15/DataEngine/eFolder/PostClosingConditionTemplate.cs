// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.PostClosingConditionTemplate
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
  public class PostClosingConditionTemplate : ConditionTemplate
  {
    private string source = string.Empty;
    private string recipient = string.Empty;
    private bool isInternal;
    private bool isExternal;

    public DateTime LastModifiedDateTime { get; set; }

    public PostClosingConditionTemplate()
    {
    }

    public PostClosingConditionTemplate(string guid)
      : base(guid)
    {
    }

    public PostClosingConditionTemplate(XmlSerializationInfo info)
      : base(info)
    {
      this.source = info.GetString(nameof (Source));
      this.recipient = info.GetString(nameof (Recipient));
      this.isInternal = info.GetBoolean(nameof (IsInternal), false);
      this.isExternal = info.GetBoolean(nameof (IsExternal), false);
    }

    public override ConditionType ConditionType => ConditionType.PostClosing;

    public string Source
    {
      get => this.source;
      set => this.source = value;
    }

    public string Recipient
    {
      get => this.recipient;
      set => this.recipient = value;
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
      return (ConditionLog) new PostClosingConditionLog(this, addedBy, pairId);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("Source", (object) this.source);
      info.AddValue("Recipient", (object) this.recipient);
      info.AddValue("IsInternal", (object) this.isInternal);
      info.AddValue("IsExternal", (object) this.isExternal);
    }
  }
}
