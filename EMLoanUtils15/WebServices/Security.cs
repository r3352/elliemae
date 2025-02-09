// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.Security
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "Security", Namespace = "http://www.elliemae.com/edm/platform")]
  public class Security : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private string PasswordField;
    private string SecurityClientIdField;
    private string UserIdField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember(IsRequired = true)]
    public string Password
    {
      get => this.PasswordField;
      set => this.PasswordField = value;
    }

    [DataMember(IsRequired = true)]
    public string SecurityClientId
    {
      get => this.SecurityClientIdField;
      set => this.SecurityClientIdField = value;
    }

    [DataMember(IsRequired = true)]
    public string UserId
    {
      get => this.UserIdField;
      set => this.UserIdField = value;
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ClientConsentDataSaveRequest.ClientConsentDataSaveRequestBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ClientConsentDataSaveRequestClientConsentDataSaveRequestBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string CityField;
      private string ClientIdField;
      private int? ConsentModelField;
      private string FaxField;
      private string FulfillmentFeeField;
      private string PhoneField;
      private string StateField;
      private string StreetAddressField;
      private string UserNameField;
      private string ZipField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string City
      {
        get => this.CityField;
        set => this.CityField = value;
      }

      [DataMember(IsRequired = true)]
      public string ClientId
      {
        get => this.ClientIdField;
        set => this.ClientIdField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public int? ConsentModel
      {
        get => this.ConsentModelField;
        set => this.ConsentModelField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string Fax
      {
        get => this.FaxField;
        set => this.FaxField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string FulfillmentFee
      {
        get => this.FulfillmentFeeField;
        set => this.FulfillmentFeeField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string Phone
      {
        get => this.PhoneField;
        set => this.PhoneField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string State
      {
        get => this.StateField;
        set => this.StateField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string StreetAddress
      {
        get => this.StreetAddressField;
        set => this.StreetAddressField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string UserName
      {
        get => this.UserNameField;
        set => this.UserNameField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string Zip
      {
        get => this.ZipField;
        set => this.ZipField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ClientConsentDataSaveResponse.ClientConsentDataSaveResponseBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ClientConsentDataSaveResponseClientConsentDataSaveResponseBody : 
      IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private bool SuccessField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember]
      public bool Success
      {
        get => this.SuccessField;
        set => this.SuccessField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConsentClientDataGetRequest.ConsentClientDataGetRequestBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ConsentClientDataGetRequestConsentClientDataGetRequestBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ClientIdField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(IsRequired = true)]
      public string ClientId
      {
        get => this.ClientIdField;
        set => this.ClientIdField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConsentClientDataGetResponse.ConsentClientDataGetResponseBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ConsentClientDataGetResponseConsentClientDataGetResponseBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string CityField;
      private int? ConsentModelField;
      private string FaxField;
      private string FulfillmentFeeField;
      private string PhoneField;
      private string StateField;
      private string StreetAddressField;
      private string UserNameField;
      private string ZipField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember]
      public string City
      {
        get => this.CityField;
        set => this.CityField = value;
      }

      [DataMember]
      public int? ConsentModel
      {
        get => this.ConsentModelField;
        set => this.ConsentModelField = value;
      }

      [DataMember]
      public string Fax
      {
        get => this.FaxField;
        set => this.FaxField = value;
      }

      [DataMember]
      public string FulfillmentFee
      {
        get => this.FulfillmentFeeField;
        set => this.FulfillmentFeeField = value;
      }

      [DataMember]
      public string Phone
      {
        get => this.PhoneField;
        set => this.PhoneField = value;
      }

      [DataMember]
      public string State
      {
        get => this.StateField;
        set => this.StateField = value;
      }

      [DataMember]
      public string StreetAddress
      {
        get => this.StreetAddressField;
        set => this.StreetAddressField = value;
      }

      [DataMember]
      public string UserName
      {
        get => this.UserNameField;
        set => this.UserNameField = value;
      }

      [DataMember]
      public string Zip
      {
        get => this.ZipField;
        set => this.ZipField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConsentGetHtmlRequest.ConsentGetHtmlRequestBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ConsentGetHtmlRequestConsentGetHtmlRequestBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ClientIdField;
      private bool ConsentDeclineField;
      private string LoanGuidField;
      private string PackageIdField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(IsRequired = true)]
      public string ClientId
      {
        get => this.ClientIdField;
        set => this.ClientIdField = value;
      }

      [DataMember(IsRequired = true)]
      public bool ConsentDecline
      {
        get => this.ConsentDeclineField;
        set => this.ConsentDeclineField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string LoanGuid
      {
        get => this.LoanGuidField;
        set => this.LoanGuidField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string PackageId
      {
        get => this.PackageIdField;
        set => this.PackageIdField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConsentGetHtmlResponse.ConsentGetHtmlResponseBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ConsentGetHtmlResponseConsentGetHtmlResponseBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ConsentVerbiageField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember]
      public string ConsentVerbiage
      {
        get => this.ConsentVerbiageField;
        set => this.ConsentVerbiageField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "IsConsentRequiredRequest.IsConsentRequiredRequestBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class IsConsentRequiredRequestIsConsentRequiredRequestBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ClientIdField;
      private string LoanGuidField;
      private string PackageIdField;
      private Security.User UserDetailsField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(IsRequired = true)]
      public string ClientId
      {
        get => this.ClientIdField;
        set => this.ClientIdField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string LoanGuid
      {
        get => this.LoanGuidField;
        set => this.LoanGuidField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string PackageId
      {
        get => this.PackageIdField;
        set => this.PackageIdField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public Security.User UserDetails
      {
        get => this.UserDetailsField;
        set => this.UserDetailsField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "User", Namespace = "http://www.elliemae.com/edm/platform")]
    public class User : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string EncompassContactGuidField;
      private string UserConsentDataIdField;
      private string UserEmailField;
      private string UserFirstNameField;
      private string UserLastNameField;
      private string UserTypeField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string EncompassContactGuid
      {
        get => this.EncompassContactGuidField;
        set => this.EncompassContactGuidField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string UserConsentDataId
      {
        get => this.UserConsentDataIdField;
        set => this.UserConsentDataIdField = value;
      }

      [DataMember(IsRequired = true)]
      public string UserEmail
      {
        get => this.UserEmailField;
        set => this.UserEmailField = value;
      }

      [DataMember(IsRequired = true)]
      public string UserFirstName
      {
        get => this.UserFirstNameField;
        set => this.UserFirstNameField = value;
      }

      [DataMember(IsRequired = true)]
      public string UserLastName
      {
        get => this.UserLastNameField;
        set => this.UserLastNameField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string UserType
      {
        get => this.UserTypeField;
        set => this.UserTypeField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "IsConsentRequiredResponse.IsConsentRequiredResponseBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class IsConsentRequiredResponseIsConsentRequiredResponseBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private bool? ResultField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember]
      public bool? Result
      {
        get => this.ResultField;
        set => this.ResultField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "UserConsentDataSaveRequest.UserConsentDataSaveRequestBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class UserConsentDataSaveRequestUserConsentDataSaveRequestBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ApplicationIdField;
      private Security.Client ClientField;
      private DateTime? ConsentDateField;
      private string ConsentIpAddressField;
      private string ConsentModelField;
      private bool? ConsentRequestField;
      private bool? ConsentStatusField;
      private string ImageFileIdField;
      private bool? IsFromDeclineField;
      private bool? IsLoanLevelConsentAtPackageSendField;
      private string LoanGuidField;
      private int? PackageIdField;
      private string PdfFileIdField;
      private bool? UseBranchAddressField;
      private Security.User UserField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(IsRequired = true)]
      public string ApplicationId
      {
        get => this.ApplicationIdField;
        set => this.ApplicationIdField = value;
      }

      [DataMember]
      public Security.Client Client
      {
        get => this.ClientField;
        set => this.ClientField = value;
      }

      [DataMember]
      public DateTime? ConsentDate
      {
        get => this.ConsentDateField;
        set => this.ConsentDateField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string ConsentIpAddress
      {
        get => this.ConsentIpAddressField;
        set => this.ConsentIpAddressField = value;
      }

      [DataMember]
      public string ConsentModel
      {
        get => this.ConsentModelField;
        set => this.ConsentModelField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public bool? ConsentRequest
      {
        get => this.ConsentRequestField;
        set => this.ConsentRequestField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public bool? ConsentStatus
      {
        get => this.ConsentStatusField;
        set => this.ConsentStatusField = value;
      }

      [DataMember]
      public string ImageFileId
      {
        get => this.ImageFileIdField;
        set => this.ImageFileIdField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public bool? IsFromDecline
      {
        get => this.IsFromDeclineField;
        set => this.IsFromDeclineField = value;
      }

      [DataMember]
      public bool? IsLoanLevelConsentAtPackageSend
      {
        get => this.IsLoanLevelConsentAtPackageSendField;
        set => this.IsLoanLevelConsentAtPackageSendField = value;
      }

      [DataMember(IsRequired = true)]
      public string LoanGuid
      {
        get => this.LoanGuidField;
        set => this.LoanGuidField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public int? PackageId
      {
        get => this.PackageIdField;
        set => this.PackageIdField = value;
      }

      [DataMember]
      public string PdfFileId
      {
        get => this.PdfFileIdField;
        set => this.PdfFileIdField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public bool? UseBranchAddress
      {
        get => this.UseBranchAddressField;
        set => this.UseBranchAddressField = value;
      }

      [DataMember]
      public Security.User User
      {
        get => this.UserField;
        set => this.UserField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "Client", Namespace = "http://www.elliemae.com/edm/platform")]
    public class Client : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ClientCityField;
      private string ClientFaxField;
      private string ClientFulfillmentFeeField;
      private string ClientIdField;
      private string ClientPhoneField;
      private string ClientStateField;
      private string ClientStreetAddressField;
      private string ClientUserNameField;
      private string ClientZipField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string ClientCity
      {
        get => this.ClientCityField;
        set => this.ClientCityField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string ClientFax
      {
        get => this.ClientFaxField;
        set => this.ClientFaxField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string ClientFulfillmentFee
      {
        get => this.ClientFulfillmentFeeField;
        set => this.ClientFulfillmentFeeField = value;
      }

      [DataMember(IsRequired = true)]
      public string ClientId
      {
        get => this.ClientIdField;
        set => this.ClientIdField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string ClientPhone
      {
        get => this.ClientPhoneField;
        set => this.ClientPhoneField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string ClientState
      {
        get => this.ClientStateField;
        set => this.ClientStateField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string ClientStreetAddress
      {
        get => this.ClientStreetAddressField;
        set => this.ClientStreetAddressField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string ClientUserName
      {
        get => this.ClientUserNameField;
        set => this.ClientUserNameField = value;
      }

      [DataMember(EmitDefaultValue = false)]
      public string ClientZip
      {
        get => this.ClientZipField;
        set => this.ClientZipField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "UserConsentDataSaveResponse.UserConsentDataSaveResponseBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class UserConsentDataSaveResponseUserConsentDataSaveResponseBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private bool SuccessField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember]
      public bool Success
      {
        get => this.SuccessField;
        set => this.SuccessField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "UserConsentDataGetRequest.UserConsentDataGetRequestBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class UserConsentDataGetRequestUserConsentDataGetRequestBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ClientIdField;
      private string LoanGuidField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(IsRequired = true)]
      public string ClientId
      {
        get => this.ClientIdField;
        set => this.ClientIdField = value;
      }

      [DataMember(IsRequired = true)]
      public string LoanGuid
      {
        get => this.LoanGuidField;
        set => this.LoanGuidField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "UserConsentDataGetResponse.UserConsentDataGetResponseBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class UserConsentDataGetResponseUserConsentDataGetResponseBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ApplicationIdField;
      private DateTime? ConsentDateField;
      private string ConsentIpAddressField;
      private bool? ConsentStatusField;
      private DateTime? DateConsentRequestField;
      private int? PackageIdField;
      private Security.User UserField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember]
      public string ApplicationId
      {
        get => this.ApplicationIdField;
        set => this.ApplicationIdField = value;
      }

      [DataMember]
      public DateTime? ConsentDate
      {
        get => this.ConsentDateField;
        set => this.ConsentDateField = value;
      }

      [DataMember]
      public string ConsentIpAddress
      {
        get => this.ConsentIpAddressField;
        set => this.ConsentIpAddressField = value;
      }

      [DataMember]
      public bool? ConsentStatus
      {
        get => this.ConsentStatusField;
        set => this.ConsentStatusField = value;
      }

      [DataMember]
      public DateTime? DateConsentRequest
      {
        get => this.DateConsentRequestField;
        set => this.DateConsentRequestField = value;
      }

      [DataMember]
      public int? PackageId
      {
        get => this.PackageIdField;
        set => this.PackageIdField = value;
      }

      [DataMember]
      public Security.User User
      {
        get => this.UserField;
        set => this.UserField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConsentPDFGetRequest.ConsentPDFGetRequestBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ConsentPDFGetRequestConsentPDFGetRequestBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ClientIdField;
      private int? GetAllPackagesField;
      private string LoanGuidField;
      private int? PackageIdField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(IsRequired = true)]
      public string ClientId
      {
        get => this.ClientIdField;
        set => this.ClientIdField = value;
      }

      [DataMember]
      public int? GetAllPackages
      {
        get => this.GetAllPackagesField;
        set => this.GetAllPackagesField = value;
      }

      [DataMember(IsRequired = true)]
      public string LoanGuid
      {
        get => this.LoanGuidField;
        set => this.LoanGuidField = value;
      }

      [DataMember]
      public int? PackageId
      {
        get => this.PackageIdField;
        set => this.PackageIdField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConsentPdfResponse", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ConsentPdfResponse : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ConsentPdfField;
      private int? PackageIdField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember]
      public string ConsentPdf
      {
        get => this.ConsentPdfField;
        set => this.ConsentPdfField = value;
      }

      [DataMember]
      public int? PackageId
      {
        get => this.PackageIdField;
        set => this.PackageIdField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ClientSecurity", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ClientSecurity : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ClientAuthCodeField;
      private string SecurityClientIdField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(IsRequired = true)]
      public string ClientAuthCode
      {
        get => this.ClientAuthCodeField;
        set => this.ClientAuthCodeField = value;
      }

      [DataMember(IsRequired = true)]
      public string SecurityClientId
      {
        get => this.SecurityClientIdField;
        set => this.SecurityClientIdField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConsentSaveExternalRequest.ConsentSaveExternalRequestBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ConsentSaveExternalRequestConsentSaveExternalRequestBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private string ApplicationIdField;
      private string ClientIdField;
      private DateTime ConsentDateField;
      private string ConsentIpAddressField;
      private bool ConsentStatusField;
      private string LoanGuidField;
      private string UserEmailField;
      private string UserFirstNameField;
      private string UserLastNameField;
      private string UserTypeField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember(IsRequired = true)]
      public string ApplicationId
      {
        get => this.ApplicationIdField;
        set => this.ApplicationIdField = value;
      }

      [DataMember(IsRequired = true)]
      public string ClientId
      {
        get => this.ClientIdField;
        set => this.ClientIdField = value;
      }

      [DataMember(IsRequired = true)]
      public DateTime ConsentDate
      {
        get => this.ConsentDateField;
        set => this.ConsentDateField = value;
      }

      [DataMember(IsRequired = true)]
      public string ConsentIpAddress
      {
        get => this.ConsentIpAddressField;
        set => this.ConsentIpAddressField = value;
      }

      [DataMember(IsRequired = true)]
      public bool ConsentStatus
      {
        get => this.ConsentStatusField;
        set => this.ConsentStatusField = value;
      }

      [DataMember(IsRequired = true)]
      public string LoanGuid
      {
        get => this.LoanGuidField;
        set => this.LoanGuidField = value;
      }

      [DataMember(IsRequired = true)]
      public string UserEmail
      {
        get => this.UserEmailField;
        set => this.UserEmailField = value;
      }

      [DataMember(IsRequired = true)]
      public string UserFirstName
      {
        get => this.UserFirstNameField;
        set => this.UserFirstNameField = value;
      }

      [DataMember(IsRequired = true)]
      public string UserLastName
      {
        get => this.UserLastNameField;
        set => this.UserLastNameField = value;
      }

      [DataMember(IsRequired = true)]
      public string UserType
      {
        get => this.UserTypeField;
        set => this.UserTypeField = value;
      }
    }

    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ConsentSaveExternalResponse.ConsentSaveExternalResponseBody", Namespace = "http://www.elliemae.com/edm/platform")]
    public class ConsentSaveExternalResponseConsentSaveExternalResponseBody : IExtensibleDataObject
    {
      private ExtensionDataObject extensionDataField;
      private bool SuccessField;

      public ExtensionDataObject ExtensionData
      {
        get => this.extensionDataField;
        set => this.extensionDataField = value;
      }

      [DataMember]
      public bool Success
      {
        get => this.SuccessField;
        set => this.SuccessField = value;
      }
    }
  }
}
