// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.eFolder.SelectDocuments
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ClientServer.eFolder
{
  public class SelectDocuments
  {
    private FileAttachment[] fileAttachments;
    private DocumentLog[] loanDocs;
    private DocumentLog[] preSelectedDocs;
    private SelectDocumentsReasonType selectDocumentsReasonType;
    private StackingOrderSetTemplate stackingTemplate;
    private bool isListValid;

    public SelectDocuments(
      FileAttachment[] fileAttachments,
      DocumentLog[] loanDocs,
      DocumentLog[] selectedDocs,
      SelectDocumentsReasonType reasonType,
      StackingOrderSetTemplate stackingTemplate)
    {
      this.fileAttachments = fileAttachments;
      this.loanDocs = loanDocs;
      this.preSelectedDocs = selectedDocs;
      this.selectDocumentsReasonType = reasonType;
      this.stackingTemplate = stackingTemplate;
    }

    public SelectionDocument[] GetSelectionDocuments()
    {
      this.isListValid = true;
      List<SelectionDocument> source1 = new List<SelectionDocument>();
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport)
      {
        if (this.stackingTemplate != null)
        {
          foreach (string docName in this.stackingTemplate.DocNames)
            source1.Add(new SelectionDocument()
            {
              DocName = docName,
              RequiredString = this.stackingTemplate.RequiredDocs.Contains((object) docName) ? "Yes" : "No"
            });
        }
        return source1.ToArray();
      }
      if (this.stackingTemplate == null)
      {
        foreach (DocumentLog loanDoc in this.loanDocs)
        {
          if (this.HasAttachmentInLoan(loanDoc))
          {
            bool flag = false;
            if (this.preSelectedDocs != null && Array.IndexOf<DocumentLog>(this.preSelectedDocs, loanDoc) >= 0)
              flag = true;
            source1.Add(new SelectionDocument()
            {
              DocName = loanDoc.Title,
              DocumentLog = loanDoc,
              IsChecked = flag
            });
          }
        }
        return source1.OrderBy<SelectionDocument, string>((Func<SelectionDocument, string>) (x => x.DocName)).ToList<SelectionDocument>().ToArray();
      }
      foreach (string docName in this.stackingTemplate.DocNames)
      {
        bool flag1 = false;
        bool flag2 = false;
        foreach (DocumentLog loanDoc in this.loanDocs)
        {
          if (this.DocMatchesStackingItem(loanDoc, docName) && this.IsDocAvailableExternally(loanDoc))
          {
            flag1 = true;
            SelectionDocument selectionDocument = new SelectionDocument();
            selectionDocument.DocName = loanDoc.Title;
            selectionDocument.DocumentLog = loanDoc;
            selectionDocument.RequiredString = this.stackingTemplate.RequiredDocs.Contains((object) docName) ? "Yes" : "No";
            if (this.HasAttachmentInLoan(loanDoc))
            {
              flag2 = true;
              if (this.preSelectedDocs != null && Array.IndexOf<DocumentLog>(this.preSelectedDocs, loanDoc) >= 0)
                selectionDocument.IsChecked = true;
              if (this.selectDocumentsReasonType != SelectDocumentsReasonType.PrintSave || this.selectDocumentsReasonType == SelectDocumentsReasonType.PrintSave && this.stackingTemplate.AutoSelectDocuments)
                selectionDocument.IsChecked = true;
            }
            else
            {
              selectionDocument.IsChecked = false;
              selectionDocument.CheckBoxVisible = false;
              selectionDocument.RequiredError = this.stackingTemplate.RequiredDocs.Contains((object) docName);
              selectionDocument.OptionalWarning = !selectionDocument.RequiredError;
            }
            source1.Add(selectionDocument);
          }
        }
        if (this.stackingTemplate.RequiredDocs.Contains((object) docName) && (!flag1 || !flag2))
          this.isListValid = false;
        if (!flag1)
        {
          SelectionDocument selectionDocument = new SelectionDocument()
          {
            DocName = docName,
            RequiredString = this.stackingTemplate.RequiredDocs.Contains((object) docName) ? "Yes" : "No",
            IsChecked = false,
            CheckBoxVisible = false,
            RequiredError = this.stackingTemplate.RequiredDocs.Contains((object) docName)
          };
          selectionDocument.OptionalWarning = !selectionDocument.RequiredError;
          source1.Add(selectionDocument);
        }
      }
      if (!this.stackingTemplate.FilterDocuments)
      {
        List<SelectionDocument> source2 = new List<SelectionDocument>();
        foreach (DocumentLog loanDoc in this.loanDocs)
        {
          bool flag = false;
          foreach (string docName in this.stackingTemplate.DocNames)
          {
            if (this.DocMatchesStackingItem(loanDoc, docName))
              flag = true;
          }
          if (!flag && this.HasAttachmentInLoan(loanDoc) && this.IsDocAvailableExternally(loanDoc))
          {
            SelectionDocument selectionDocument = new SelectionDocument();
            selectionDocument.DocumentLog = loanDoc;
            selectionDocument.DocName = loanDoc.Title;
            selectionDocument.IsChecked = false;
            if (this.preSelectedDocs != null && Array.IndexOf<DocumentLog>(this.preSelectedDocs, loanDoc) >= 0)
              selectionDocument.IsChecked = true;
            source2.Add(selectionDocument);
          }
        }
        foreach (SelectionDocument selectionDocument in source2.OrderBy<SelectionDocument, string>((Func<SelectionDocument, string>) (x => x.DocName)).ToList<SelectionDocument>())
          source1.Add(selectionDocument);
      }
      return source1.ToArray();
    }

    public bool IsListValid => this.isListValid;

    private bool HasAttachmentInLoan(DocumentLog doc)
    {
      foreach (FileAttachment fileAttachment in this.fileAttachments)
      {
        if (doc.Files.Contains(fileAttachment.ID))
          return true;
      }
      return false;
    }

    private bool DocMatchesStackingItem(DocumentLog doc, string stackingName)
    {
      return doc.Title.ToLower().Trim() == stackingName.ToLower().Trim() || this.stackingTemplate.NDEDocGroups.Contains((object) stackingName.Trim()) && doc.GroupName.ToLower().Trim() == stackingName.ToLower().Trim();
    }

    private bool IsDocAvailableExternally(DocumentLog doc)
    {
      bool flag = false;
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.PrintSave)
        flag = true;
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.SendToBorrower)
        flag = doc.IsWebcenter;
      if (this.selectDocumentsReasonType == SelectDocumentsReasonType.SendToThirdParty || this.selectDocumentsReasonType == SelectDocumentsReasonType.DocumentExport)
        flag = doc.IsThirdPartyDoc;
      return flag;
    }
  }
}
