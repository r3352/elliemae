// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DocLoanActionPair
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DocLoanActionPair : LoanActionBase
  {
    public readonly string DocGuid;
    public readonly bool AttachedRequired;

    public DocLoanActionPair(string docGuid, string loanActionID, bool attachedRequired)
      : base(loanActionID)
    {
      this.DocGuid = docGuid;
      this.AttachedRequired = attachedRequired;
    }

    public DocLoanActionPair(XmlSerializationInfo info)
      : base(info)
    {
      this.DocGuid = info.GetString(nameof (DocGuid));
      this.AttachedRequired = info.GetBoolean(nameof (AttachedRequired));
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("DocGuid", (object) this.DocGuid);
      info.AddValue("AttachRequired", (object) this.AttachedRequired);
    }
  }
}
