// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerCompleteTasksAction
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
  public class TriggerCompleteTasksAction : TriggerAction
  {
    private string[] taskNames;

    public TriggerCompleteTasksAction(string[] taskNames) => this.taskNames = taskNames;

    public TriggerCompleteTasksAction(XmlSerializationInfo info)
    {
      this.taskNames = ((List<string>) info.GetValue(nameof (taskNames), typeof (XmlList<string>))).ToArray();
    }

    public string[] TaskNames => this.taskNames;

    public override TriggerActionType ActionType => TriggerActionType.CompleteTasks;

    public bool ContainsTask(string taskName)
    {
      for (int index = 0; index < this.taskNames.Length; ++index)
      {
        if (string.Compare(this.taskNames[index], taskName, true) == 0)
          return true;
      }
      return false;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("taskNames", (object) new XmlList<string>((IEnumerable<string>) this.taskNames));
    }
  }
}
