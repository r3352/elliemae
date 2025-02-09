// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.DocumentAttachment
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine.Log;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class DocumentAttachment : eDeliveryAttachment
  {
    private DocumentLog doc;
    private string signatureType;
    private string[] signatureFields;
    private ForBorrowerType intendedFor;
    private int pageCount;
    private PdfReader pdfReader;

    public DocumentAttachment(string filepath, DocumentLog doc)
      : base(filepath, doc.Title)
    {
      this.doc = doc;
      this.id = doc.Guid;
      this.pageCount = 0;
    }

    public DocumentLog Document => this.doc;

    public string SignatureType
    {
      get => this.signatureType;
      set => this.signatureType = value;
    }

    public string[] SignatureFields
    {
      get => this.signatureFields;
      set => this.signatureFields = value;
    }

    public ForBorrowerType IntendedFor
    {
      get => this.intendedFor;
      set => this.intendedFor = value;
    }

    public int PageCount
    {
      get => this.pageCount;
      set => this.pageCount = value;
    }

    public PdfReader PdfReader
    {
      get => this.pdfReader;
      set => this.pdfReader = value;
    }

    public bool AllSignerExists(List<eDeliveryRecipient> signers, List<string> excludeSigners)
    {
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) this.SignatureFields);
      foreach (eDeliveryRecipient signer in signers)
      {
        stringList.Remove(signer.CheckboxField);
        stringList.Remove(signer.InitialsField);
        stringList.Remove(signer.SignatureField);
      }
      for (int index = stringList.Count - 1; index >= 0; --index)
      {
        foreach (string excludeSigner in excludeSigners)
        {
          string signer = excludeSigner;
          stringList.RemoveAll((Predicate<string>) (x => x.ToUpper().Contains(signer.ToUpper())));
        }
      }
      return stringList.Count <= 0;
    }

    public void SetPackageId(string packageId) => this.doc.PackageId = packageId;
  }
}
