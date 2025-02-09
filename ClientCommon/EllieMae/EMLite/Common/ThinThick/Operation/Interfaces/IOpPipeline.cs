// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ThinThick.Operation.Interfaces.IOpPipeline
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.ThinThick.Requests;
using EllieMae.EMLite.Common.ThinThick.Requests.Pipeline;
using EllieMae.EMLite.Common.ThinThick.Responses;
using System;

#nullable disable
namespace EllieMae.EMLite.Common.ThinThick.Operation.Interfaces
{
  public interface IOpPipeline : IOperation, IDisposable
  {
    OpSimpleResponse CreateNewLoan(OpSimpleRequest request);

    OpSimpleResponse OpenLoanMailbox(OpSimpleRequest request);

    OpFieldDialogResponse SelectField(OpSimpleRequest request);

    OpSimpleResponse RebuildLoan(OpRebuildLoanRequest request);

    OpSimpleResponse OpenLoan(OpSimpleRequest request);

    OpSimpleResponse ProcessEPassUrl(OpProcessEPassUrlRequest request);

    OpSimpleResponse OpenLoanForm(OpOpenLoanFormRequest request);

    OpSimpleResponse ShowLockConfirmation(OpSimpleRequest request);

    OpSimpleResponse StartConversation(OpStartConversationRequest request);

    OpSimpleResponse ExportToExcel(OpExportRequest request);

    OpSimpleResponse PrintForms(OpSimpleRequest request);

    OpSimpleResponse SetThinPipelineInfos(OpThinPipelineInfosRequest request);

    OpSimpleResponse SetPipelineView(OpPipelineViewRequest request);

    OpSimpleResponse TransferLoans(OpSimpleRequest request);

    OpSimpleResponse NotifyUsers(OpSimpleRequest request);

    OpSimpleResponse DuplicateLoan(OpDuplicateLoanRequest request);

    OpDialogResponse eFolderExport(OpExportRequest request);

    OpDialogResponse InvestorStandardExportPipeline(OpServiceRequest request);

    OpDialogResponse ExportFannieMaeFormattedFile(OpSimpleRequest request);

    OpSimpleResponse ExportLEFPipeline(OpExportRequest request);

    OpSimpleResponse GenerateNMLSReport(OpSimpleRequest request);

    OpSimpleResponse GenerateNCMLDReport(OpSimpleRequest request);

    OpSimpleResponse CreateAppointment(OpContactGuidRequest request);

    OpSimpleResponse ImportLoans(OpSimpleRequest request);
  }
}
