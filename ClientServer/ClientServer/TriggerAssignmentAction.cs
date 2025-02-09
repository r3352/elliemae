// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerAssignmentAction
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
  public class TriggerAssignmentAction : TriggerAction
  {
    private TriggerAssignment[] items;

    public TriggerAssignmentAction(TriggerAssignment[] items) => this.items = items;

    public TriggerAssignmentAction(XmlSerializationInfo info)
    {
      this.items = ((List<TriggerAssignment>) info.GetValue(nameof (items), typeof (XmlList<TriggerAssignment>))).ToArray();
    }

    public TriggerAssignment[] Assignments => this.items;

    public override TriggerActionType ActionType => TriggerActionType.Assign;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("items", (object) new XmlList<TriggerAssignment>((IEnumerable<TriggerAssignment>) this.items));
    }
  }
}
