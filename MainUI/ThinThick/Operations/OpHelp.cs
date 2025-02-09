// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ThinThick.Operations.OpHelp
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.Common.ThinThick.Operation;
using EllieMae.EMLite.Common.ThinThick.Operation.Interfaces;
using EllieMae.EMLite.Common.ThinThick.Requests.Interaction;
using EllieMae.EMLite.Common.ThinThick.Responses;
using EllieMae.EMLite.MainUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ThinThick.Operations
{
  public class OpHelp : OperationBase, IOpHelp, IOperation, IDisposable
  {
    private const string className = "OpHelp";
    private static readonly string sw = Tracing.SwThinThick;

    public OpSimpleResponse SetHelpTargetName(OpSetHelpRequest request)
    {
      OpSimpleResponse opSimpleResponse = new OpSimpleResponse();
      if (MainScreen.Instance.Browser == null)
      {
        opSimpleResponse.ErrorCode = ErrorCodes.BrowserIsNull;
        opSimpleResponse.ErrorMessage = "Browser is null";
        return opSimpleResponse;
      }
      MainScreen.Instance.Browser.SetHelpTargetName(request.HelpTargetName);
      return opSimpleResponse;
    }
  }
}
