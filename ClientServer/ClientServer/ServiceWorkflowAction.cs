// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServiceWorkflowAction
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ServiceWorkflowAction : IXmlSerializable
  {
    public readonly Guid ActionID;
    public readonly ServiceWorkflowActionType ActionType;
    public readonly Guid TargetID;

    public ServiceWorkflowAction(string actionID, string actionType, string targetID)
    {
      this.ActionID = new Guid(actionID);
      Enum.TryParse<ServiceWorkflowActionType>(actionType, out this.ActionType);
      this.TargetID = new Guid(targetID);
    }

    public ServiceWorkflowAction(Guid actionID, string actionType, Guid targetID)
    {
      this.ActionID = actionID;
      Enum.TryParse<ServiceWorkflowActionType>(actionType, out this.ActionType);
      this.TargetID = targetID;
    }

    public ServiceWorkflowAction(XmlSerializationInfo info)
    {
      this.ActionID = new Guid(info.GetString(nameof (ActionID)));
      ServiceWorkflowActionType result;
      Enum.TryParse<ServiceWorkflowActionType>(info.GetString(nameof (ActionType)), out result);
      this.ActionType = result;
      this.TargetID = new Guid(info.GetString(nameof (TargetID)));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("ActionID", (object) this.ActionID);
      info.AddValue("ActionType", (object) this.ActionType.ToString());
      info.AddValue("TargetID", (object) this.TargetID.ToString());
    }
  }
}
