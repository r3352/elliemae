// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalAttachment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents a single External organization attachment.</summary>
  public class ExternalAttachment : IExternalAttachment
  {
    private ExternalOrgAttachments externalOrgAttachment;

    internal ExternalAttachment(ExternalOrgAttachments externalOrgAttachment)
    {
      this.externalOrgAttachment = externalOrgAttachment;
    }

    /// <summary>Gets Guid of the attachment record</summary>
    public Guid Guid => this.externalOrgAttachment.Guid;

    /// <summary>Gets the id external organization</summary>
    public int ExternalOrgID => this.externalOrgAttachment.ExternalOrgID;

    /// <summary>Gets or sets file name of the attachment.</summary>
    public string FileName
    {
      get => this.externalOrgAttachment.FileName;
      set => this.externalOrgAttachment.FileName = value;
    }

    /// <summary>Gets or sets attachment description</summary>
    public string Description
    {
      get => this.externalOrgAttachment.Description;
      set => this.externalOrgAttachment.Description = value;
    }

    /// <summary>Gets or sets attachment category</summary>
    public int Category
    {
      get => this.externalOrgAttachment.Category;
      set => this.externalOrgAttachment.Category = value;
    }

    /// <summary>Gets or sets file date</summary>
    public DateTime FileDate
    {
      get => this.externalOrgAttachment.FileDate;
      set => this.externalOrgAttachment.FileDate = value;
    }

    /// <summary>
    /// Gets the Id of the Encompass user who added the attachment.
    /// </summary>
    public string UserWhoAdded => this.externalOrgAttachment.UserWhoAdded;

    /// <summary>Gets or sets the expiration date</summary>
    public DateTime ExpirationDate
    {
      get => this.externalOrgAttachment.ExpirationDate;
      set => this.externalOrgAttachment.ExpirationDate = value;
    }

    /// <summary>Gets or sets the number of days to expire</summary>
    public int DaysToExpire
    {
      get => this.externalOrgAttachment.DaysToExpire;
      set => this.externalOrgAttachment.DaysToExpire = value;
    }

    /// <summary>Gets the real file name</summary>
    public string RealFileName => this.externalOrgAttachment.RealFileName;

    /// <summary>Gets the date added</summary>
    public DateTime DateAdded => this.externalOrgAttachment.DateAdded;

    internal static List<ExternalAttachment> ToList(List<ExternalOrgAttachments> attachments)
    {
      List<ExternalAttachment> list = new List<ExternalAttachment>();
      for (int index = 0; index < attachments.Count; ++index)
        list.Add(new ExternalAttachment(attachments[index]));
      return list;
    }
  }
}
