// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TaskLoanActionPair
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TaskLoanActionPair : LoanActionBase
  {
    public readonly string TaskGuid;

    public TaskLoanActionPair(string taskGuid, string loanActionID)
      : base(loanActionID)
    {
      this.TaskGuid = taskGuid;
    }

    public TaskLoanActionPair(XmlSerializationInfo info)
      : base(info)
    {
      this.TaskGuid = info.GetString(nameof (TaskGuid));
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("TaskGuid", (object) this.TaskGuid);
    }
  }
}
