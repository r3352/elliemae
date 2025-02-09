// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerCopyAction
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum.Triggers;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerCopyAction : TriggerAction
  {
    private string[] targetFieldIds;

    public TriggerCopyAction(string[] targetFieldIds) => this.targetFieldIds = targetFieldIds;

    public TriggerCopyAction(XmlSerializationInfo info)
    {
      this.targetFieldIds = ((List<string>) info.GetValue(nameof (targetFieldIds), typeof (XmlList<string>))).ToArray();
    }

    public string[] TargetFieldIDs => this.targetFieldIds;

    public override TriggerActionType ActionType => TriggerActionType.Copy;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("targetFieldIds", (object) new XmlList<string>((IEnumerable<string>) this.targetFieldIds));
    }
  }
}
