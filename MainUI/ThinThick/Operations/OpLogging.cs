// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ThinThick.Operations.OpLogging
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ThinThick.Operation;
using EllieMae.EMLite.Common.ThinThick.Operation.Interfaces;
using EllieMae.EMLite.Common.ThinThick.Requests.Interaction;
using EllieMae.EMLite.Common.ThinThick.Responses;
using System;

#nullable disable
namespace EllieMae.EMLite.ThinThick.Operations
{
  public class OpLogging : OperationBase, IOpLogging, IOperation, IDisposable
  {
    private const string className = "OpLogging";
    private static readonly string sw = Tracing.SwThinThick;

    public OpSimpleResponse WriteLog(OpLoggingRequest request)
    {
      OpSimpleResponse opSimpleResponse = new OpSimpleResponse();
      Tracing.Log(OpLogging.sw, nameof (OpLogging), request.TraceLevel, request.Message);
      return opSimpleResponse;
    }
  }
}
