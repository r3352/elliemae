// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalAttachment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class ExternalAttachment : IExternalAttachment
  {
    private ExternalOrgAttachments externalOrgAttachment;

    internal ExternalAttachment(ExternalOrgAttachments externalOrgAttachment)
    {
      this.externalOrgAttachment = externalOrgAttachment;
    }

    public Guid Guid => this.externalOrgAttachment.Guid;

    public int ExternalOrgID => this.externalOrgAttachment.ExternalOrgID;

    public string FileName
    {
      get => this.externalOrgAttachment.FileName;
      set => this.externalOrgAttachment.FileName = value;
    }

    public string Description
    {
      get => this.externalOrgAttachment.Description;
      set => this.externalOrgAttachment.Description = value;
    }

    public int Category
    {
      get => this.externalOrgAttachment.Category;
      set => this.externalOrgAttachment.Category = value;
    }

    public DateTime FileDate
    {
      get => this.externalOrgAttachment.FileDate;
      set => this.externalOrgAttachment.FileDate = value;
    }

    public string UserWhoAdded => this.externalOrgAttachment.UserWhoAdded;

    public DateTime ExpirationDate
    {
      get => this.externalOrgAttachment.ExpirationDate;
      set => this.externalOrgAttachment.ExpirationDate = value;
    }

    public int DaysToExpire
    {
      get => this.externalOrgAttachment.DaysToExpire;
      set => this.externalOrgAttachment.DaysToExpire = value;
    }

    public string RealFileName => this.externalOrgAttachment.RealFileName;

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
