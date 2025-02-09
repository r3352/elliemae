// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerApplyLoanTemplateAction
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerApplyLoanTemplateAction : TriggerAction
  {
    private string loanTemplateName = string.Empty;
    private string filePath = string.Empty;

    public TriggerApplyLoanTemplateAction()
    {
    }

    public TriggerApplyLoanTemplateAction(string filePath) => this.filePath = filePath;

    public TriggerApplyLoanTemplateAction(XmlSerializationInfo info)
    {
      this.filePath = info.GetString(nameof (filePath));
    }

    public string FilePath => this.filePath;

    public string LoanTemplateName
    {
      get => new FileSystemEntry(this.filePath, FileSystemEntry.Types.File, (string) null).Name;
    }

    public override TriggerActionType ActionType => TriggerActionType.ApplyLoanTemplate;

    public override TriggerActivationEvent ActivationEvent => TriggerActivationEvent.FieldChanged;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("filePath", (object) this.filePath);
    }
  }
}
