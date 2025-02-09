// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ThinThick.Operations.OpCommon
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.Common.ThinThick.Operation;
using EllieMae.EMLite.Common.ThinThick.Operation.Interfaces;
using EllieMae.EMLite.Common.ThinThick.Requests;
using EllieMae.EMLite.Common.ThinThick.Requests.Interaction;
using EllieMae.EMLite.Common.ThinThick.Responses;
using EllieMae.EMLite.MainUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ThinThick.Operations
{
  public class OpCommon : OperationBase, IOpCommon, IOperation, IDisposable
  {
    private const string className = "OpCommon";
    private static readonly string sw = Tracing.SwThinThick;

    public OpSimpleResponse SetMenuState(OpMenuStateRequest request)
    {
      if (MainScreen.Instance.Browser == null)
        return this.NullBrowserResponse;
      MainScreen.Instance.Browser.SetMenuState(request.MenuState, MainScreen.Instance.Browser.MenuItemCollection);
      return new OpSimpleResponse();
    }

    public OpSimpleResponse SetPipelineViewXml(OpXmlRequest request)
    {
      if (MainScreen.Instance.Browser == null)
        return this.NullBrowserResponse;
      OpSimpleResponse opSimpleResponse = new OpSimpleResponse();
      if (!MainScreen.Instance.Browser.SetSelectedPipelineView(request.Xml))
      {
        opSimpleResponse.ErrorCode = ErrorCodes.ViewNotSet;
        opSimpleResponse.ErrorMessage = "View is not set.";
      }
      return opSimpleResponse;
    }

    public OpSimpleResponse ExportToExcel(OpExportRequest request)
    {
      return MainScreen.Instance.Browser == null ? this.NullBrowserResponse : MainScreen.Instance.Browser.ExportToExcel(request);
    }

    public OpSimpleResponse Print(OpSimpleRequest request)
    {
      return MainScreen.Instance.Browser == null ? this.NullBrowserResponse : MainScreen.Instance.Browser.Print(request);
    }

    private OpSimpleResponse NullBrowserResponse
    {
      get
      {
        OpSimpleResponse nullBrowserResponse = new OpSimpleResponse();
        nullBrowserResponse.ErrorCode = ErrorCodes.BrowserIsNull;
        nullBrowserResponse.ErrorMessage = "Browser is null";
        return nullBrowserResponse;
      }
    }

    public OpSimpleResponse CloseDialog(OpCloseDialogRequest request)
    {
      MainScreen.Instance.GetBrowserWindow(request.Dialog).Close();
      return new OpSimpleResponse();
    }
  }
}
