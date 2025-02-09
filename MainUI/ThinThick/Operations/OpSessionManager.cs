// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ThinThick.Operations.OpSessionManager
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.ThinThick.Operation;
using EllieMae.EMLite.Common.ThinThick.Operation.Interfaces;
using EllieMae.EMLite.Common.ThinThick.Requests;
using EllieMae.EMLite.Common.ThinThick.Responses;
using System;

#nullable disable
namespace EllieMae.EMLite.ThinThick.Operations
{
  public class OpSessionManager : OperationBase, IOpSessionManager, IOperation, IDisposable
  {
    private const string className = "OpSessionManager";
    private static readonly string sw = Tracing.SwThinThick;

    public OpSessionIdGetResponse GetSessionId(OpSimpleRequest request)
    {
      OpSessionIdGetResponse sessionId = new OpSessionIdGetResponse();
      string instanceName = request.CommandContext.Session.ServerIdentity == null ? string.Empty : request.CommandContext.Session.ServerIdentity.InstanceName;
      sessionId.SessionId = instanceName + "_" + request.CommandContext.Session.SessionObjects.SessionID;
      OAuth2.AuthToken accessToken = new OAuth2(request.CommandContext.Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(request.CommandContext.Session.SessionObjects)).GetAccessToken(instanceName, request.CommandContext.Session.SessionObjects.SessionID, "loc");
      sessionId.authToken = accessToken.TypeAndToken;
      return sessionId;
    }
  }
}
