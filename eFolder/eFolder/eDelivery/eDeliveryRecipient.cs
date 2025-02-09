// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.eDeliveryRecipient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class eDeliveryRecipient
  {
    private string signerID;
    private eDeliveryEntityType entityType;
    private string firstName;
    private string middleName;
    private string lastName;
    private string suffixName;
    private string unparsedName;
    private string emailAddress;
    private string userID;
    private string signatureField;
    private string initialsField;
    private bool autoSign;
    private string authCode;
    private string recipientType;
    private string borrowerId;
    public bool Ischecked = true;
    private string recipientId;
    private int nboIndex;
    private string cellPhoneNumber;
    private string homePhoneNumber;
    private string workPhoneNumber;
    private string partyId;
    private string phoneNumber;
    private PhoneType phoneType;

    public eDeliveryRecipient(string signerID, eDeliveryEntityType entityType)
    {
      this.signerID = signerID;
      this.entityType = entityType;
    }

    public string CheckboxField { get; set; }

    public string SignerID => this.signerID;

    public eDeliveryEntityType EntityType => this.entityType;

    public string FirstName
    {
      get => this.firstName;
      set => this.firstName = value;
    }

    public string MiddleName
    {
      get => this.middleName;
      set => this.middleName = value;
    }

    public string LastName
    {
      get => this.lastName;
      set => this.lastName = value;
    }

    public string SuffixName
    {
      get => this.suffixName;
      set => this.suffixName = value;
    }

    public string UnparsedName
    {
      get => this.unparsedName;
      set => this.unparsedName = value;
    }

    public string EmailAddress
    {
      get => this.emailAddress;
      set => this.emailAddress = value;
    }

    public string UserID
    {
      get => this.userID;
      set => this.userID = value;
    }

    public string SignatureField
    {
      get => this.signatureField;
      set => this.signatureField = value;
    }

    public string InitialsField
    {
      get => this.initialsField;
      set => this.initialsField = value;
    }

    public bool AutoSign
    {
      get => this.autoSign;
      set => this.autoSign = value;
    }

    public string AuthCode
    {
      get => this.authCode;
      set => this.authCode = value;
    }

    public string RecipientType
    {
      get => this.recipientType;
      set => this.recipientType = value;
    }

    public bool RequiresSigning(DocumentAttachment attachment)
    {
      return attachment.SignatureFields != null && (Array.Exists<string>(attachment.SignatureFields, (Predicate<string>) (element => element.StartsWith(this.signatureField, StringComparison.OrdinalIgnoreCase))) || Array.Exists<string>(attachment.SignatureFields, (Predicate<string>) (element => element.StartsWith(this.initialsField, StringComparison.OrdinalIgnoreCase))));
    }

    public string BorrowerId
    {
      get => this.borrowerId;
      set => this.borrowerId = value;
    }

    public string RecipientId
    {
      get => this.recipientId;
      set => this.recipientId = value;
    }

    public int NboIndex
    {
      get => this.nboIndex;
      set => this.nboIndex = value;
    }

    public string CellPhoneNumber
    {
      get => this.cellPhoneNumber;
      set => this.cellPhoneNumber = value;
    }

    public string HomePhoneNumber
    {
      get => this.homePhoneNumber;
      set => this.homePhoneNumber = value;
    }

    public string WorkPhoneNumber
    {
      get => this.workPhoneNumber;
      set => this.workPhoneNumber = value;
    }

    public string PartyId
    {
      get => this.partyId;
      set => this.partyId = value;
    }

    public string PhoneNumber
    {
      get => this.phoneNumber;
      set => this.phoneNumber = value;
    }

    public PhoneType PhoneType
    {
      get => this.phoneType;
      set => this.phoneType = value;
    }
  }
}
