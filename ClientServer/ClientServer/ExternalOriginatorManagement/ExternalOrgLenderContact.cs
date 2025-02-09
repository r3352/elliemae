// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.ExternalOrgLenderContact
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  [Serializable]
  public class ExternalOrgLenderContact
  {
    public ExternalOrgLenderContact()
    {
    }

    public ExternalOrgLenderContact(
      string userID,
      int? externalOrgID,
      int displayOrder,
      string name,
      string title,
      string email,
      string phone,
      bool wholesaleChannelEnabled,
      bool delegatedChannelEnabled,
      bool nonDelegatedChannelEnabled,
      ExternalOrgCompanyContactSourceTable source = ExternalOrgCompanyContactSourceTable.ExternalOrgLenderContacts,
      int sourceId = 0,
      bool hidden = false)
    {
      this.UserID = userID;
      this.ExternalOrgID = externalOrgID;
      this.DisplayOrder = displayOrder;
      this.Name = name;
      this.Title = title;
      this.Email = email;
      this.Phone = phone;
      this.isWholesaleChannelEnabled = wholesaleChannelEnabled;
      this.isDelegatedChannelEnabled = delegatedChannelEnabled;
      this.isNonDelegatedChannelEnabled = nonDelegatedChannelEnabled;
      this.isHidden = hidden;
      this.Source = source;
      this.ContactID = sourceId;
    }

    public ExternalOrgCompanyContactSourceTable Source { get; set; }

    public int ContactID { get; set; }

    public string UserID { get; set; }

    public int? ExternalOrgID { get; set; }

    public int DisplayOrder { get; set; }

    public string Name { get; set; }

    public string Title { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public bool isWholesaleChannelEnabled { get; set; }

    public bool isDelegatedChannelEnabled { get; set; }

    public bool isNonDelegatedChannelEnabled { get; set; }

    public bool isHidden { get; set; }

    public bool isPrimarySalesRep { get; set; }

    public static implicit operator ExternalOrgSalesRep(ExternalOrgLenderContact it)
    {
      return new ExternalOrgSalesRep()
      {
        salesRepId = it.ContactID,
        externalOrgId = it.ExternalOrgID.Value,
        userId = it.UserID,
        userName = it.Name,
        title = it.Title,
        email = it.Email,
        phone = it.Email,
        isPrimarySalesRep = it.isPrimarySalesRep,
        isWholesaleChannelEnabled = it.isWholesaleChannelEnabled,
        isDelegatedChannelEnabled = it.isDelegatedChannelEnabled,
        isNonDelegatedChannelEnabled = it.isNonDelegatedChannelEnabled
      };
    }
  }
}
