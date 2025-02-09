// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerMoveLoanFolderAction
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerMoveLoanFolderAction : TriggerAction
  {
    private string loanFolderName = string.Empty;

    public TriggerMoveLoanFolderAction()
    {
    }

    public TriggerMoveLoanFolderAction(string loanTemplateName)
    {
      this.loanFolderName = loanTemplateName;
    }

    public TriggerMoveLoanFolderAction(XmlSerializationInfo info)
    {
      this.loanFolderName = info.GetString(nameof (loanFolderName));
    }

    public string LoanFolderName => this.loanFolderName;

    public override TriggerActionType ActionType => TriggerActionType.LoanMove;

    public override TriggerActivationEvent ActivationEvent => TriggerActivationEvent.LoanSaved;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("loanFolderName", (object) this.loanFolderName);
    }
  }
}
