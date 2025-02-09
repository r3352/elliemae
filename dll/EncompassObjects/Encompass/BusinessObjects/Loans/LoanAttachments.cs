// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanAttachments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanAttachments : ILoanAttachments, IEnumerable
  {
    private static string[] validExtensions = new string[12]
    {
      ".pdf",
      ".doc",
      ".docx",
      ".txt",
      ".tif",
      ".jpg",
      ".jpeg",
      ".jpe",
      ".htm",
      ".html",
      ".emf",
      ".xps"
    };
    private Loan loan;
    private FileAttachmentCollection attachments;
    private Dictionary<string, Attachment> attachmentCache = new Dictionary<string, Attachment>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    internal LoanAttachments(Loan loan)
    {
      this.loan = loan;
      this.attachments = loan.Unwrap().FileAttachments;
    }

    public int Count => this.attachments.Count;

    public Attachment this[int index]
    {
      get
      {
        string id = this.attachments[index].ID;
        if (!this.attachmentCache.ContainsKey(id))
          this.attachmentCache[id] = new Attachment(this.loan, id);
        return this.attachmentCache[id];
      }
    }

    public Attachment GetAttachmentByName(string name)
    {
      FileAttachment fileAttachment = (FileAttachment) null;
      foreach (FileAttachment attachment in this.attachments)
      {
        if (string.Compare(attachment.ID, name, true) == 0)
        {
          fileAttachment = attachment;
          break;
        }
      }
      if (fileAttachment == null)
        return (Attachment) null;
      if (!this.attachmentCache.ContainsKey(name))
        this.attachmentCache[fileAttachment.ID] = new Attachment(this.loan, fileAttachment.ID);
      return this.attachmentCache[fileAttachment.ID];
    }

    public Attachment Add(string filePath)
    {
      if (this.loan.IsNew)
        throw new InvalidOperationException("Attachments cannot be created until loan is Committed.");
      if (!this.isValidFileExtension(filePath))
        throw new ArgumentException("The specified file does not have a supported extension.");
      return this.GetAttachmentByName(this.loan.Unwrap().FileAttachments.AddAttachment((AddReasonType) 12, filePath, "Untitled", (DocumentLog) null).ID);
    }

    public Attachment AddObject(DataObject data, string fileExtension)
    {
      if (this.loan.IsNew)
        throw new InvalidOperationException("Attachments cannot be created until loan is Committed.");
      if (!this.isValidFileExtension("test." + fileExtension))
        throw new ArgumentException("The specified file does not have a supported extension.");
      return this.GetAttachmentByName(this.loan.Unwrap().FileAttachments.AddAttachment((AddReasonType) 12, data.Unwrap(), fileExtension, "Untitled", (DocumentLog) null).ID);
    }

    public Attachment AddImage(string filePath)
    {
      if (this.loan.IsNew)
        throw new InvalidOperationException("Attachments cannot be created until loan is Committed.");
      if (!this.isValidFileExtension(filePath))
        throw new ArgumentException("The specified file does not have a supported extension.");
      return this.GetAttachmentByName((!this.loan.Unwrap().UseSkyDrive ? this.loan.Unwrap().FileAttachments.AddAttachment((AddReasonType) 16, filePath, "Untitled", (DocumentLog) null) : this.loan.Unwrap().FileAttachments.AddAttachment((AddReasonType) 12, filePath, "Untitled", (DocumentLog) null)).ID);
    }

    public Attachment AddObjectImage(DataObject data, string fileExtension)
    {
      if (this.loan.IsNew)
        throw new InvalidOperationException("Attachments cannot be created until loan is Committed.");
      if (!this.isValidFileExtension("test." + fileExtension))
        throw new ArgumentException("The specified file does not have a supported extension.");
      return this.GetAttachmentByName((!this.loan.Unwrap().UseSkyDrive ? this.loan.Unwrap().FileAttachments.AddAttachment((AddReasonType) 16, data.Unwrap(), fileExtension, "Untitled", (DocumentLog) null) : this.loan.Unwrap().FileAttachments.AddAttachment((AddReasonType) 12, data.Unwrap(), fileExtension, "Untitled", (DocumentLog) null)).ID);
    }

    public void Remove(Attachment attachment)
    {
      if (this.loan.IsNew)
        throw new InvalidOperationException("Attachments cannot be created until loan is Committed.");
      if (this.GetAttachmentByName(attachment.Name) == null)
        throw new ArgumentException("The specified attachment is not found");
      this.loan.Unwrap().FileAttachments.Remove((RemoveReasonType) 6, attachment.Name);
      if (!this.attachmentCache.ContainsKey(attachment.Name))
        return;
      this.attachmentCache.Remove(attachment.Name);
    }

    public void MovePageImage(
      Attachment sourceAttachment,
      AttachmentPageImage attachmentPageImage,
      Attachment targetAttachment)
    {
      this.loan.Unwrap().GetImageAttachment(targetAttachment.Name).Pages.AddRange(new PageImage[1]
      {
        attachmentPageImage.GetPageImage()
      });
      sourceAttachment.PageImages.Remove(attachmentPageImage);
    }

    public IEnumerator GetEnumerator()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < this.attachments.Count; ++index)
        arrayList.Add((object) this[index]);
      return arrayList.GetEnumerator();
    }

    public void Refresh()
    {
      this.attachments = this.loan.Unwrap().FileAttachments;
      this.attachmentCache.Clear();
    }

    private bool isValidFileExtension(string fileName)
    {
      string lower = Path.GetExtension(fileName).ToLower();
      for (int index = 0; index < LoanAttachments.validExtensions.Length; ++index)
      {
        if (lower == LoanAttachments.validExtensions[index])
          return true;
      }
      return false;
    }
  }
}
