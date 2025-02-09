// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ThinThick.Operations.OpTrading
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ThinThick;
using EllieMae.EMLite.Common.ThinThick.Operation;
using EllieMae.EMLite.Common.ThinThick.Operation.Interfaces;
using EllieMae.EMLite.Common.ThinThick.Requests.Trading;
using EllieMae.EMLite.Common.ThinThick.Responses;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.MainUI;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ThinThick.Operations
{
  public class OpTrading : OperationBase, IOpTrading, IOperation, IDisposable
  {
    private const string className = "OpTrading";
    private static readonly string sw = Tracing.SwThinThick;

    public OpSimpleResponse SetTradingScreen(OpTradingScreenSetRequest request)
    {
      OpSimpleResponse opSimpleResponse = new OpSimpleResponse();
      MainScreen.Instance.TradingBrowser.SetCurrentScreen(request.TradingScreen);
      return opSimpleResponse;
    }

    public OpSimpleResponse SetTradingIds(OpIdsSetRequest request)
    {
      OpSimpleResponse opSimpleResponse = new OpSimpleResponse();
      if (!MainScreen.Instance.TradingBrowser.SetTradingIds(request.Ids))
      {
        opSimpleResponse.ErrorCode = ErrorCodes.MethodNotSupported;
        opSimpleResponse.ErrorMessage = "Method not supported by selected trading screen";
      }
      return opSimpleResponse;
    }

    public OpRxContactInfoResponse SelectBusinessContact(OpRxContactInfoRequest request)
    {
      OpRxContactInfoResponse contactInfoResponse = new OpRxContactInfoResponse();
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact(request.Category, request.CompanyName, request.ContactName, (RxContactInfo) null, CRMRoleType.NotFound, false, ""))
      {
        if (rxBusinessContact.ShowDialog(request.CommandContext.SourceWindow) == DialogResult.OK)
          contactInfoResponse.RxContactInfo = rxBusinessContact.RxContactRecord;
        contactInfoResponse.DialogResult = rxBusinessContact.DialogResult.ToString();
      }
      return contactInfoResponse;
    }

    public OpSimpleResponse SetCurrentOrArchived(OpCurrentOrArchivedRequest request)
    {
      MainScreen.Instance.TradingBrowser.CurrentOrArchived = request.CurrentOrArchived;
      return new OpSimpleResponse();
    }
  }
}
