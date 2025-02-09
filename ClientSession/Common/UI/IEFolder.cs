// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.IEFolder
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface IEFolder
  {
    FileAttachment[] Add(LoanDataMgr loanDataMgr, FormItemInfo[] formList);

    DocumentLog[] AppendDocumentSet(LoanDataMgr loanDataMgr);

    DocumentLog[] SelectDocuments(LoanDataMgr loanDataMgr);

    DocumentLog[] SelectDocuments(LoanDataMgr loanDataMgr, DocumentLog[] docList);

    DocumentLog[] SelectDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      bool allowContinue);

    DocumentLog[] SelectDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList);

    DocumentLog[] SelectDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList,
      bool allowContinue);

    DocumentLog[] SelectDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList,
      FileSystemEntry defaultStackingEntry);

    DocumentLog[] SelectDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentLog[] defaultList,
      FileSystemEntry defaultStackingEntry,
      bool allowContinue);

    string SelectEVaultDocument(LoanDataMgr loanDataMgr);

    bool PreviewDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList,
      string[] packageList);

    bool PrintDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList,
      string[] packageList);

    bool SendDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList,
      string[] packageList);

    void ViewDisclosures(LoanDataMgr loanDataMgr, string eDisclosurePackageViewableFile);

    void ViewDisclosures(
      LoanDataMgr loanDataMgr,
      string eDisclosurePackageViewableFile,
      string packageId,
      string recordGuid);

    string UpdateSupportingData(
      LoanDataMgr loanDataMgr,
      string loanGuid,
      string packageId,
      string supportingKey);

    bool PreviewClosing(LoanDataMgr loanDataMgr, string[] pdfList, string[] titleList);

    bool PrintClosing(LoanDataMgr loanDataMgr, string[] pdfList, string[] titleList);

    bool ViewClosingOverflow(
      LoanDataMgr loanDataMgr,
      string[] pdfList,
      string[] titleList,
      string selectedTitle,
      Dictionary<string, Dictionary<int, List<string>>> overflowInfo);

    bool SendClosing(LoanDataMgr loanDataMgr, string[] pdfList, string[] titleList);

    bool PreviewPreClosing(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList);

    bool PrintPreClosing(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList);

    bool SendPreClosing(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      string[] titleList);

    void LaunchEClose(LoanDataMgr loanDataMgr);

    void LaunchENote(LoanDataMgr loanDataMgr);

    void LaunchEDisclosures(LoanDataMgr loanDataMgr, int height, int width);

    bool PreviewDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] docList,
      DocumentLog[] neededList,
      ConditionLog cond);

    bool PrintDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] docList,
      DocumentLog[] neededList,
      ConditionLog cond);

    bool SendDisclosures(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] docList,
      DocumentLog[] neededList,
      ConditionLog cond);

    bool PreviewRequest(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      ConditionLog cond);

    bool PrintRequest(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      ConditionLog cond);

    bool SendRequest(
      LoanDataMgr loanDataMgr,
      string coversheetFile,
      string[] pdfList,
      DocumentLog[] signList,
      DocumentLog[] neededList,
      ConditionLog cond);

    bool SendConsentRequest(LoanDataMgr loanDataMgr);

    string Export(LoanDataMgr loanDataMgr, ConditionLog cond);

    string Export(LoanDataMgr loanDataMgr, ConditionLog[] condList);

    string Export(LoanDataMgr loanDataMgr, DocumentLog doc);

    string Export(LoanDataMgr loanDataMgr, DocumentLog[] docList);

    string Export(LoanDataMgr loanDataMgr, FileAttachment file);

    string Export(LoanDataMgr loanDataMgr, FileAttachment[] fileList);

    DocumentLog[] Request(LoanDataMgr loanDataMgr, DocumentLog doc);

    DocumentLog[] Request(LoanDataMgr loanDataMgr, DocumentLog doc, ConditionLog cond);

    DocumentLog[] Request(LoanDataMgr loanDataMgr, DocumentLog[] docList);

    DocumentLog[] Request(LoanDataMgr loanDataMgr, DocumentLog[] docList, ConditionLog cond);

    bool Retrieve(LoanDataMgr loanDataMgr, Sessions.Session session);

    bool Retrieve(LoanDataMgr loanDataMgr, DocumentLog doc, Sessions.Session session);

    bool Retrieve(LoanDataMgr loanDataMgr, DocumentLog[] docList, Sessions.Session session);

    bool ExportDocuments(
      LoanDataMgr loanDataMgr,
      DocumentLog[] docList,
      DocumentExportTemplate exportTemplate);

    bool SendDocuments(LoanDataMgr loanDataMgr, DocumentLog[] docList);

    bool SendForms(LoanDataMgr loanDataMgr, FormItemInfo[] formList);

    bool SendForms(LoanDataMgr loanDataMgr, string[] formList, string pdfFile);

    void View(LoanDataMgr loanDataMgr, PreliminaryConditionLog cond);

    void View(LoanDataMgr loanDataMgr, UnderwritingConditionLog cond);

    void View(LoanDataMgr loanDataMgr, PostClosingConditionLog cond);

    void View(LoanDataMgr loanDataMgr, SellConditionLog cond);

    void View(LoanDataMgr loanDataMgr, DocumentLog doc);

    void View(LoanDataMgr loanDataMgr, FileAttachment file, Sessions.Session session);

    void View(LoanDataMgr loanDataMgr, FileAttachment[] fileList, Sessions.Session session);

    bool Minimize();

    void ImportConditions(LoanDataMgr loanDataMgr, bool isAutoImported);

    bool IsEnhancedConditionTemplateActive(EnhancedConditionTemplate template);

    bool IsEnhancedConditionAllowedOnLoan(
      LoanDataMgr loanDataMgr,
      EnhancedConditionTemplate template);

    bool Print(LoanDataMgr loanDataMgr, string url, string authorizationHeader, int currentPage);

    void ImportDocuments();

    void ShowEfolderDialogWithDocumentTab(LoanDataMgr loanDataMgr, Sessions.Session session);
  }
}
