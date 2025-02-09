// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerAdvancedCodeAction
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
  public class TriggerAdvancedCodeAction : TriggerAction
  {
    private string sourceCode;

    public TriggerAdvancedCodeAction(string sourceCode) => this.sourceCode = sourceCode;

    public TriggerAdvancedCodeAction(XmlSerializationInfo info)
    {
      this.sourceCode = info.GetString(nameof (sourceCode));
    }

    public string SourceCode => this.sourceCode;

    public override TriggerActionType ActionType => TriggerActionType.AdvancedCode;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("sourceCode", (object) this.sourceCode);
    }
  }
}
