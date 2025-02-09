// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.IExternalAttachment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Interface for IExternalAttachment class to support External Attachments
  /// </summary>
  /// <exclude />
  public interface IExternalAttachment
  {
    Guid Guid { get; }

    int ExternalOrgID { get; }

    string FileName { get; set; }

    string Description { get; set; }

    int Category { get; set; }

    DateTime FileDate { get; set; }

    string UserWhoAdded { get; }

    DateTime ExpirationDate { get; set; }

    int DaysToExpire { get; set; }

    string RealFileName { get; }

    DateTime DateAdded { get; }
  }
}
