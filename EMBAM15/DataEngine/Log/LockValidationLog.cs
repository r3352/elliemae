// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LockValidationLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Xml;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LockValidationLog : LogRecordBase
  {
    public static readonly string XmlType = "LockValidation";
    private string statusChangedBy = string.Empty;
    private string statusChangedByFullName = string.Empty;
    private string lockValidationStatus = string.Empty;
    private string requestGUID = string.Empty;
    private bool hideLog = true;

    public LockValidationLog()
    {
    }

    public LockValidationLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.statusChangedBy = attributeReader.GetString(nameof (StatusChangedBy));
      this.statusChangedByFullName = attributeReader.GetString(nameof (StatusChangedByFullName));
      this.lockValidationStatus = attributeReader.GetString(nameof (LockValidationStatus));
      this.requestGUID = attributeReader.GetString(nameof (RequestGUID));
      this.hideLog = attributeReader.GetBoolean("HideLog");
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) LockValidationLog.XmlType);
      attributeWriter.Write("StatusChangedBy", (object) this.statusChangedBy);
      attributeWriter.Write("StatusChangedByFullName", (object) this.statusChangedByFullName);
      attributeWriter.Write("LockValidationStatus", (object) this.lockValidationStatus);
      attributeWriter.Write("RequestGUID", (object) this.requestGUID);
      attributeWriter.Write("HideLog", (object) this.hideLog);
    }

    public string StatusChangedBy => this.statusChangedBy;

    public string StatusChangedByFullName => this.StatusChangedByFullName;

    public void SetStatusChangedByUser(UserInfo user)
    {
      this.statusChangedBy = user.Userid;
      this.statusChangedByFullName = user.FullName;
      this.MarkAsDirty();
    }

    public string RequestGUID
    {
      get => this.requestGUID;
      set
      {
        this.requestGUID = value;
        this.MarkAsDirty();
      }
    }

    public override bool DisplayInLog
    {
      get => !this.hideLog;
      set
      {
        this.hideLog = !value;
        this.MarkAsDirty();
      }
    }

    public string LockValidationStatus
    {
      get => this.lockValidationStatus;
      set
      {
        this.lockValidationStatus = value;
        this.MarkAsDirty();
      }
    }

    internal override void AttachToLog(LogList log) => base.AttachToLog(log);
  }
}
