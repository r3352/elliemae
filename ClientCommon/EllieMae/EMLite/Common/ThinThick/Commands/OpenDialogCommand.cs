// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ThinThick.Commands.OpenDialogCommand
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.ThinThick.Operation.Interfaces;
using EllieMae.EMLite.Common.ThinThick.Operation.Location;
using EllieMae.EMLite.Common.ThinThick.Requests;
using EllieMae.EMLite.Common.ThinThick.Requests.Interaction;
using EllieMae.EMLite.Common.ThinThick.Requests.Pipeline;
using EllieMae.EMLite.Common.ThinThick.Requests.Trading;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace EllieMae.EMLite.Common.ThinThick.Commands
{
  [ComVisible(true)]
  [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
  public class OpenDialogCommand(CommandContext context) : CommandBase(context)
  {
    public override string Execute(string routine, string jsonParams = "")
    {
      IResponse resp = (IResponse) null;
      switch (routine)
      {
        case "OpBusinessRuleFindFieldDialog.ShowDialog":
          using (IOpBusinessRuleFindFieldDialog ruleFindFieldDialog = OperationLocator.GetInstance().Resolve<IOpBusinessRuleFindFieldDialog>())
          {
            resp = (IResponse) ruleFindFieldDialog.ShowDialog(this.DeserializeRequest<OpBusinessRuleFindFieldDialogShowDialogRequest>(jsonParams));
            break;
          }
        case "OpCommon.ExportToExcel":
          using (IOpCommon opCommon = OperationLocator.GetInstance().Resolve<IOpCommon>())
          {
            resp = (IResponse) opCommon.ExportToExcel(this.DeserializeRequest<OpExportRequest>(jsonParams));
            break;
          }
        case "OpCommon.Print":
          using (IOpCommon opCommon = OperationLocator.GetInstance().Resolve<IOpCommon>())
          {
            resp = (IResponse) opCommon.Print(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpCommon.SetMenuState":
          using (IOpCommon opCommon = OperationLocator.GetInstance().Resolve<IOpCommon>())
          {
            resp = (IResponse) opCommon.SetMenuState(this.DeserializeRequest<OpMenuStateRequest>(jsonParams));
            break;
          }
        case "OpCommon.SetPipelineViewXml":
          using (IOpCommon opCommon = OperationLocator.GetInstance().Resolve<IOpCommon>())
          {
            resp = (IResponse) opCommon.SetPipelineViewXml(this.DeserializeRequest<OpXmlRequest>(jsonParams));
            break;
          }
        case "OpFieldSearchRuleEditor.OpenEditor":
          using (IOpFieldSearchRuleEditor searchRuleEditor = OperationLocator.GetInstance().Resolve<IOpFieldSearchRuleEditor>())
          {
            resp = (IResponse) searchRuleEditor.OpenEditor(this.DeserializeRequest<OpFieldSearchRuleOpenEditorRequest>(jsonParams));
            break;
          }
        case "OpHelp.SetHelpTargetName":
          using (IOpHelp opHelp = OperationLocator.GetInstance().Resolve<IOpHelp>())
          {
            resp = (IResponse) opHelp.SetHelpTargetName(this.DeserializeRequest<OpSetHelpRequest>(jsonParams));
            break;
          }
        case "OpLogging.WriteLog":
          using (IOpLogging opLogging = OperationLocator.GetInstance().Resolve<IOpLogging>())
          {
            resp = (IResponse) opLogging.WriteLog(this.DeserializeRequest<OpLoggingRequest>(jsonParams));
            break;
          }
        case "OpPipeline.CreateAppointment":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.CreateAppointment(this.DeserializeRequest<OpContactGuidRequest>(jsonParams));
            break;
          }
        case "OpPipeline.CreateNewLoan":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.CreateNewLoan(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.DuplicateLoan":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.DuplicateLoan(this.DeserializeRequest<OpDuplicateLoanRequest>(jsonParams));
            break;
          }
        case "OpPipeline.ExportFannieMaeFormattedFile":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.ExportFannieMaeFormattedFile(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.ExportLEFPipeline":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.ExportLEFPipeline(this.DeserializeRequest<OpExportRequest>(jsonParams));
            break;
          }
        case "OpPipeline.GenerateNCMLDReport":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.GenerateNCMLDReport(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.GenerateNMLSReport":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.GenerateNMLSReport(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.ImportLoans":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.ImportLoans(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.InvestorStandardExportPipeline":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.InvestorStandardExportPipeline(this.DeserializeRequest<OpServiceRequest>(jsonParams));
            break;
          }
        case "OpPipeline.NotifyUsers":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.NotifyUsers(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.OpenLoan":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.OpenLoan(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.OpenLoanForm":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.OpenLoanForm(this.DeserializeRequest<OpOpenLoanFormRequest>(jsonParams));
            break;
          }
        case "OpPipeline.OpenLoanMailbox":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.OpenLoanMailbox(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.PrintForms":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.PrintForms(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.ProcessEPassUrl":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.ProcessEPassUrl(this.DeserializeRequest<OpProcessEPassUrlRequest>(jsonParams));
            break;
          }
        case "OpPipeline.RebuildLoan":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.RebuildLoan(this.DeserializeRequest<OpRebuildLoanRequest>(jsonParams));
            break;
          }
        case "OpPipeline.SelectField":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.SelectField(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.SetPipelineView":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.SetPipelineView(this.DeserializeRequest<OpPipelineViewRequest>(jsonParams));
            break;
          }
        case "OpPipeline.SetThinPipelineInfos":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.SetThinPipelineInfos(this.DeserializeRequest<OpThinPipelineInfosRequest>(jsonParams));
            break;
          }
        case "OpPipeline.ShowLockConfirmation":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.ShowLockConfirmation(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.StartConversation":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.StartConversation(this.DeserializeRequest<OpStartConversationRequest>(jsonParams));
            break;
          }
        case "OpPipeline.TransferLoans":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.TransferLoans(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpPipeline.eFolderExport":
          using (IOpPipeline opPipeline = OperationLocator.GetInstance().Resolve<IOpPipeline>())
          {
            resp = (IResponse) opPipeline.eFolderExport(this.DeserializeRequest<OpExportRequest>(jsonParams));
            break;
          }
        case "OpSessionManager.GetSessionId":
          using (IOpSessionManager opSessionManager = OperationLocator.GetInstance().Resolve<IOpSessionManager>())
          {
            resp = (IResponse) opSessionManager.GetSessionId(this.DeserializeRequest<OpSimpleRequest>(jsonParams));
            break;
          }
        case "OpTrading.SelectBusinessContact":
          using (IOpTrading opTrading = OperationLocator.GetInstance().Resolve<IOpTrading>())
          {
            resp = (IResponse) opTrading.SelectBusinessContact(this.DeserializeRequest<OpRxContactInfoRequest>(jsonParams));
            break;
          }
        case "OpTrading.SetCurrentOrArchived":
          using (IOpTrading opTrading = OperationLocator.GetInstance().Resolve<IOpTrading>())
          {
            resp = (IResponse) opTrading.SetCurrentOrArchived(this.DeserializeRequest<OpCurrentOrArchivedRequest>(jsonParams));
            break;
          }
        case "OpTrading.SetTradingIds":
          using (IOpTrading opTrading = OperationLocator.GetInstance().Resolve<IOpTrading>())
          {
            resp = (IResponse) opTrading.SetTradingIds(this.DeserializeRequest<OpIdsSetRequest>(jsonParams));
            break;
          }
        case "OpTrading.SetTradingScreen":
          using (IOpTrading opTrading = OperationLocator.GetInstance().Resolve<IOpTrading>())
          {
            resp = (IResponse) opTrading.SetTradingScreen(this.DeserializeRequest<OpTradingScreenSetRequest>(jsonParams));
            break;
          }
      }
      return this.ConvertToJson(resp);
    }
  }
}
