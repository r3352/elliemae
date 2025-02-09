// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure2015
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Represents a single Disclosure Tracking 2015 record associated with a Loan.
  /// </summary>
  /// <remarks>The inherited Date property of a Disclosure represents the
  /// date on which the disclosure was made.
  /// <p>Disclosure instances become invalid
  /// when the <see cref="M:EllieMae.Encompass.BusinessObjects.Loans.Loan.Refresh">Refresh</see> method is
  /// invoked on the parent <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Loan">Loan</see> object. Attempting
  /// to access this object after invoking Refresh() will result in an
  /// exception.</p>
  /// </remarks>
  /// <example>
  ///     The following code access Disclosures2015 in a loan.
  ///     <code>
  ///       <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// using EllieMae.Encompass.BusinessObjects.Loans.Logging;
  /// using EllieMae.Encompass.BusinessObjects.Users;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server. We will need to be logged
  ///       // in as an Administrator to modify the user accounts.
  ///       Session session = new Session();
  ///       session.Start("EMServer", "myuser", "myuserpwd");
  /// 
  ///      //Open a Loan by providing loan guid
  ///       var loan = session.Loans.Open("{b8bee82b-d1cd-48ce-b97f-365a9e60defb}");
  /// 
  ///       if (loan.Log.Disclosures2015 != null && loan.Log.Disclosures2015.Count > 0)
  ///       {
  ///          Console.WriteLine(@"DisclosedDate2015: " + loan.Log.Disclosures2015[0].DisclosedDate2015);
  ///          Console.WriteLine(@"DisclosureRecordType2015: " + loan.Log.Disclosures2015[0].DisclosureRecordType2015);
  ///          Console.WriteLine(@"BorrowerReceivedMethod2015: " + loan.Log.Disclosures2015[0].BorrowerReceivedMethod2015);
  ///          Console.WriteLine(@"CoBorrowerReceivedMethod2015: " + loan.Log.Disclosures2015[0].CoBorrowerReceivedMethod2015);
  ///          Console.WriteLine(@"BorrowerActualReceivedDate2015: " + loan.Log.Disclosures2015[0].BorrowerActualReceivedDate2015);
  ///          Console.WriteLine(@"CoBorrowerActualReceivedDate2015: " + loan.Log.Disclosures2015[0].CoBorrowerActualReceivedDate2015);
  ///        }
  /// 
  ///       loan.Close();
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  ///     </code>
  ///   </example>
  public class Disclosure2015 : DisclosureBase, IDisclosure2015
  {
    private bool allowExtendedDisclosureAccess;
    private bool getExtendedDisclosureAccessSetting;

    internal Disclosure2015(Loan loan, DisclosureTracking2015Log discItem)
      : base(loan, (DisclosureTrackingBase) discItem)
    {
    }

    private bool AllowExtendedDisclosureAccess
    {
      get
      {
        if (!this.getExtendedDisclosureAccessSetting)
        {
          this.allowExtendedDisclosureAccess = SmartClientUtils.GetAttribute(this.loan.Session.ClientID, "EncompassSDK", "EnableSDKExtendedDisclosure") == "1";
          this.getExtendedDisclosureAccessSetting = true;
        }
        return this.allowExtendedDisclosureAccess;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType" /> for the current entry.
    /// </summary>
    /// <remarks>This property will always return the value
    /// <see cref="F:EllieMae.Encompass.BusinessObjects.Loans.Logging.LogEntryType.Disclosure2015" />.</remarks>
    public override LogEntryType EntryType => LogEntryType.Disclosure2015;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.StandardDisclosure2015Type" /> for the disclosure.
    /// </summary>
    public StandardDisclosure2015Type DisclosureType
    {
      get
      {
        this.EnsureValid();
        StandardDisclosure2015Type disclosureType = StandardDisclosure2015Type.None;
        if (this.DiscItem.DisclosedForCD)
          disclosureType |= StandardDisclosure2015Type.CD;
        if (this.DiscItem.DisclosedForLE)
          disclosureType |= StandardDisclosure2015Type.LE;
        if (this.DiscItem.ProviderListSent)
          disclosureType |= StandardDisclosure2015Type.PROVIDERLIST;
        if (this.DiscItem.ProviderListNoFeeSent)
          disclosureType |= StandardDisclosure2015Type.PROVIDERLISTNOFEE;
        if (this.DiscItem.DisclosedForSafeHarbor)
          disclosureType |= StandardDisclosure2015Type.SAFEHARBOR;
        return disclosureType;
      }
    }

    /// <summary>
    /// Gets or sets the flag indicating disclosure is invalid.
    /// </summary>
    /// <remarks>An invalid disclosure is one that was never actually made and will not be considered for
    /// the purposes of determining the compliance timeline of the loan.</remarks>
    public bool EnabledForCompliance
    {
      get
      {
        this.EnsureValid();
        return this.DiscItem.IsDisclosed;
      }
      set
      {
        this.EnsureEditable();
        this.DiscItem.IsDisclosed = value;
      }
    }

    /// <summary>Gets or sets the 2015 DeliveryMethod</summary>
    public DeliveryMethod2015 DeliveryMethod
    {
      get
      {
        this.EnsureValid();
        return (DeliveryMethod2015) this.DiscItem.DisclosureMethod;
      }
      set
      {
        this.EnsureEditable();
        this.DiscItem.DisclosureMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }
    }

    /// <summary>Gets or sets the 2015 DeliveryMethodOther</summary>
    /// 
    ///             /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string DeliveryMethodOther
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.DisclosedMethodOther;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.DisclosedMethodOther = value;
      }
    }

    /// <summary>Gets or sets the BorrowerActualReceivedDate.</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime BorrowerActualReceivedDate
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.BorrowerActualReceivedDate;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.BorrowerActualReceivedDate = value;
      }
    }

    /// <summary>Gets or sets the BorrowerDisclosedMethod.</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DeliveryMethod2015 BorrowerReceivedMethod
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return (DeliveryMethod2015) this.DiscItem.BorrowerDisclosedMethod;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.EnsureValidDisclosedMethod(value);
        this.DiscItem.BorrowerDisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }
    }

    /// <summary>Gets or sets the BorrowerDisclosedMethodOther.</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string BorrowerReceivedMethodOther
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.BorrowerDisclosedMethodOther;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.BorrowerDisclosedMethodOther = value;
      }
    }

    /// <summary>Gets or sets the CoBorrowerActualReceivedDate.</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative. </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime CoBorrowerActualReceivedDate
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.CoBorrowerActualReceivedDate;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.CoBorrowerActualReceivedDate = value;
      }
    }

    /// <summary>Gets or sets the CoBorrowerDisclosedMethod.</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DeliveryMethod2015 CoBorrowerReceivedMethod
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return (DeliveryMethod2015) this.DiscItem.CoBorrowerDisclosedMethod;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.EnsureValidDisclosedMethod(value);
        this.DiscItem.CoBorrowerDisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }
    }

    /// <summary>Gets or sets the CoBorrowerDisclosedMethodOther.</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string CoBorrowerReceivedMethodOther
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.CoBorrowerDisclosedMethodOther;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.CoBorrowerDisclosedMethodOther = value;
      }
    }

    /// <summary>Gets or sets the DisclosureRecordType</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DisclosureRecordType DisclosureRecordType
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return (DisclosureRecordType) this.DiscItem.DisclosureType;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.DisclosureType = (DisclosureTracking2015Log.DisclosureTypeEnum) value;
      }
    }

    /// <summary>
    /// Allow Manual Fulfillment based on - automatic fulfillment is disabled, Manual fulfillment is enabled, no automatic fulfillment exists
    /// </summary>
    private bool AllowManualFulfillment
    {
      get
      {
        try
        {
          return !(((IConfigurationManager) this.loan.Session.GetObject("ConfigurationManager")).GetCompanySetting("Fulfillment", "ServiceEnabled") == "Y") & ((IFeaturesAclManager) this.loan.Session.GetAclManager(AclCategory.Features)).CheckPermission(AclFeature.ToolsTab_DT_ManualFulfillment, this.loan.Session.UserID) && this.DiscItem.FullfillmentProcessedDate == DateTime.MinValue;
        }
        catch (Exception ex)
        {
          return false;
        }
      }
    }

    /// <summary>Get or Set the Manual FulfilledBy</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string ManualFulfilledBy
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return this.DiscItem.eDisclosureManuallyFulfilledBy;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.eDisclosureManuallyFulfilledBy = value;
      }
    }

    /// <summary>Get or Set the Manual FulfilledDateTime</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime ManualFulfilledDateTime
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return this.DiscItem.eDisclosureManualFulfillmentDate;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.eDisclosureManualFulfillmentDate = value;
      }
    }

    /// <summary>Get or Set the Manual FulfillmentMethod</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DeliveryMethod2015 ManualFulfillmentMethod
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return (DeliveryMethod2015) this.DiscItem.eDisclosureManualFulfillmentMethod;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.eDisclosureManualFulfillmentMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }
    }

    /// <summary>Get or Set the Manual FulfillmentCommets</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string ManualFulfillmentCommets
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return this.DiscItem.eDisclosureManualFulfillmentComment;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.eDisclosureManualFulfillmentComment = value;
      }
    }

    /// <summary>Get or Set the Manual FulfillmentActualReceivedDate</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime ManualFulfillmentActualReceivedDate
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return this.DiscItem.ActualFulfillmentDate;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        this.DiscItem.ActualFulfillmentDate = value;
      }
    }

    /// <summary>Get the Manual ManualFulfillmentPresumedReceivedDate</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime ManualFulfillmentPresumedReceivedDate
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess && this.AllowManualFulfillment)
          return this.DiscItem.PresumedFulfillmentDate;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
    }

    /// <summary>Get the MergedDoc</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DataObject MergedDoc
    {
      get
      {
        if (!this.AllowExtendedDisclosureAccess || !this.AllowManualFulfillment)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        if (this.DiscItem.eDisclosurePackageViewableFile != string.Empty)
        {
          BinaryObject supportingData = this.Loan.Unwrap().GetSupportingData(this.DiscItem.eDisclosurePackageViewableFile);
          if (supportingData != null)
            return new DataObject(supportingData);
        }
        return (DataObject) null;
      }
    }

    /// <summary>Get BorrowerName if it exists</summary>
    public string BorrowerName => this.DiscItem.BorrowerName;

    /// <summary>Get CoBorrowerName if it exists</summary>
    public string CoBorrowerName => this.DiscItem.CoBorrowerName;

    /// <summary>Get eDisclosureBorrowerName</summary>
    public string eDisclosureBorrowerName => this.DiscItem.eDisclosureBorrowerName;

    /// <summary>Get eDisclosureCoBorrowerName</summary>
    public string eDisclosureCoBorrowerName => this.DiscItem.eDisclosureCoBorrowerName;

    /// <summary>Gets or sets the DisclosedDate</summary>
    /// <remarks>This property is not accessible in the SDK by default. To gain access to this property please contact your Ellie Mae account representative.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DateTime DisclosedDate
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.DisclosedDateTime;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
      set
      {
        if (!this.AllowExtendedDisclosureAccess)
          throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
        DateTime dateTime = this.Loan.Unwrap().AddBusinessDates(value, 3);
        if (this.DiscItem.IsBorrowerPresumedDateLocked)
          this.DiscItem.LockedBorrowerPresumedReceivedDate = dateTime;
        else
          this.DiscItem.BorrowerPresumedReceivedDate = dateTime;
        if (!string.IsNullOrEmpty(this.DiscItem.CoBorrowerName))
        {
          if (this.DiscItem.IsCoBorrowerPresumedDateLocked)
            this.DiscItem.LockedCoBorrowerPresumedReceivedDate = dateTime;
          else
            this.DiscItem.CoBorrowerPresumedReceivedDate = dateTime;
        }
        this.DiscItem.DisclosedDate = value;
      }
    }

    /// <summary>Gets the DisclosureRecordType</summary>
    public DisclosureRecordType DisclosureRecordType2015
    {
      get => (DisclosureRecordType) this.DiscItem.DisclosureType2015;
    }

    /// <summary>Gets the DisclosedDate</summary>
    public DateTime DisclosedDate2015 => this.DiscItem.DisclosedDate2015;

    /// <summary>Gets the BorrowerReceivedMethod</summary>
    public DeliveryMethod2015 BorrowerReceivedMethod2015
    {
      get => (DeliveryMethod2015) this.DiscItem.BorrowerDisclosedMethod2015;
    }

    /// <summary>Gets the CoBorrowerReceivedMethod</summary>
    public DeliveryMethod2015 CoBorrowerReceivedMethod2015
    {
      get => (DeliveryMethod2015) this.DiscItem.CoBorrowerDisclosedMethod2015;
    }

    /// <summary>Gets the BorrowerActualReceivedDate</summary>
    public DateTime BorrowerActualReceivedDate2015 => this.DiscItem.BorrowerActualReceivedDate2015;

    /// <summary>Gets the CoBorrowerActualReceivedDate</summary>
    public DateTime CoBorrowerActualReceivedDate2015
    {
      get => this.DiscItem.CoBorrowerActualReceivedDate2015;
    }

    private void EnsureValidDisclosedMethod(DeliveryMethod2015 disclosedMethod)
    {
      bool flag = false;
      switch (disclosedMethod)
      {
        case DeliveryMethod2015.Unknown:
          flag = false;
          break;
        case DeliveryMethod2015.Mail:
          flag = true;
          break;
        case DeliveryMethod2015.eDisclosure:
          flag = false;
          break;
        case DeliveryMethod2015.Fax:
          flag = true;
          break;
        case DeliveryMethod2015.InPerson:
          flag = true;
          break;
        case DeliveryMethod2015.Other:
          flag = true;
          break;
        case DeliveryMethod2015.Email:
          flag = true;
          break;
        case DeliveryMethod2015.Phone:
          flag = false;
          break;
        case DeliveryMethod2015.Signature:
          flag = false;
          break;
      }
      if (!flag)
        throw new ArgumentException(disclosedMethod.ToString() + " is not an allowed value.");
    }

    /// <summary>Gets the UCD XML string</summary>
    public string UCD
    {
      get
      {
        string key = nameof (UCD) + this.DiscItem.Guid + ".XML";
        BinaryObject supportingData = this.Loan.Unwrap().GetSupportingData(key);
        return supportingData != null ? supportingData.ToString() : string.Empty;
      }
    }

    /// <summary>
    /// Get property to know whether the NBO exist for the current DTLog
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool IsNonBorrowingOwnerExist
    {
      get
      {
        if (this.AllowExtendedDisclosureAccess)
          return this.DiscItem.IsNboExist;
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      }
    }

    /// <summary>
    /// Gets or sets the date the disclosure was received by the borrower.
    /// </summary>
    /// <remarks>If the disclosure has not been received, this property will be null; otherwise,
    /// it will be a DateTime value. To mark a disclosure as not having been received, set this
    /// property to null.
    /// When the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Logging.Disclosure2015.DeliveryMethod" /> for the disclosure is set to Mail, Fax, or InPerson,
    /// the ReceivedDate will be set automatically based on compliance regulations. For Mail deliveries,
    /// the ReceivedDate is always three postal days from the date sent. For Fax and InPerson, the
    /// date received will match the disclosure date. Attempting to set this property when the DeliveryMethod
    /// is any of those values will result in an exception.
    /// </remarks>
    public override object ReceivedDate
    {
      get
      {
        this.EnsureValid();
        return !this.DiscItem.Received ? (object) null : (object) this.discItem.ReceivedDate;
      }
      set
      {
        this.EnsureEditable();
        if (value == null)
        {
          this.DiscItem.ReceivedDate = DateTime.MinValue;
        }
        else
        {
          DateTime dateTime = value is DateTime ? Convert.ToDateTime(value) : throw new ArgumentException("The specified value must be a DateTime.");
          if (dateTime.Date < this.DiscItem.Date.Date)
            throw new ArgumentException("The specified date is prior to the disclosure date.");
          this.DiscItem.ReceivedDate = dateTime.Date;
        }
      }
    }

    internal override bool IseDiclosure => this.DeliveryMethod == DeliveryMethod2015.eDisclosure;

    private DisclosureTracking2015Log DiscItem => (DisclosureTracking2015Log) this.discItem;

    /// <summary>Gets the number of NBO Records</summary>
    public int NumberOfNonBorrowingOwnerContact
    {
      get => Utils.ParseInt((object) this.DiscItem.GetAttribute("NBOcount"), 0);
    }

    /// <summary>Gets the number of Vesting Records</summary>
    public int NumberOfVestingParties
    {
      get => Utils.ParseInt((object) this.DiscItem.GetAttribute("TRcount"), 0);
    }

    /// <summary>Gets the number of Alert CoC</summary>
    public int NumberOfGoodFaithChangeOfCircumstance
    {
      get => Utils.ParseInt((object) this.DiscItem.GetAttribute("XCOCcount"), 0);
    }

    /// <summary>Returns the associated attribute</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string GetAttribute(string name) => this.DiscItem.GetAttribute(name);

    /// <summary>
    /// This returns all the NBO's associated with the current DisclosureLog
    /// </summary>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public List<NonBorrowerOwner> GetAllNBOItems()
    {
      if (!this.AllowExtendedDisclosureAccess)
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      List<NonBorrowerOwner> allNboItems = new List<NonBorrowerOwner>();
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in this.DiscItem.GetAllnboItems())
        allNboItems.Add(new NonBorrowerOwner(allnboItem.Value));
      return allNboItems;
    }

    /// <summary>
    /// This method returns all the COC's associated with the current DisclosureLog
    /// </summary>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ChangeOfCircumstanceItems GetAllCOCItems()
    {
      if (!this.AllowExtendedDisclosureAccess)
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      List<COCCollection> cocCollectionList = new List<COCCollection>();
      return new ChangeOfCircumstanceItems(this.PopulateAttributeToCOCFields(), this.DiscItem.GetAttribute("XCOCChangedCircumstances"), this.DiscItem.GetAttribute("XCOCFeeLevelIndicator"));
    }

    private List<COCCollection> PopulateAttributeToCOCFields()
    {
      List<COCCollection> cocFields = new List<COCCollection>();
      int changeOfCircumstance = this.DiscItem.NumberOfGoodFaithChangeOfCircumstance;
      for (int OrderId = 1; OrderId <= changeOfCircumstance; ++OrderId)
      {
        string str = "XCOC" + OrderId.ToString("00");
        COCCollection cocCollection = new COCCollection(this.DiscItem.GetAttribute(str + "01"), Utils.ParseDate((object) this.DiscItem.GetAttribute(str + "03")), Utils.ParseDate((object) this.DiscItem.GetAttribute(str + "04")), this.DiscItem.GetAttribute(str + "05"), this.DiscItem.GetAttribute(str + "06"), this.DiscItem.GetAttribute(str + "07"), this.DiscItem.GetAttribute(str + "08"), this.DiscItem.GetAttribute(str + "09"), this.DiscItem.GetAttribute(str + "98"), this.DiscItem.GetAttribute(str + "_Description"), Utils.ParseLong((object) this.DiscItem.GetAttribute(str + "_Amount"), 0L), OrderId);
        cocFields.Add(cocCollection);
      }
      return cocFields;
    }

    /// <summary>
    /// This method returns all the Vesting fields associated with the current DisclosureLog
    /// </summary>
    /// <returns></returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public List<DisclosedVestingFieldsForDisclosure2015> GetAllVestingItems()
    {
      if (!this.AllowExtendedDisclosureAccess)
        throw new InvalidOperationException("Extended Disclosure update is not allowed by system policy.");
      return this.PopulateAttributeToVestingFields();
    }

    private List<DisclosedVestingFieldsForDisclosure2015> PopulateAttributeToVestingFields()
    {
      List<DisclosedVestingFieldsForDisclosure2015> vestingFields = new List<DisclosedVestingFieldsForDisclosure2015>();
      int ofVestingParties = this.DiscItem.NumberOfVestingParties;
      for (int OrderId = 1; OrderId <= ofVestingParties; ++OrderId)
      {
        string str = "TR" + OrderId.ToString("00");
        string attribute1 = this.DiscItem.GetAttribute(str + "01");
        string attribute2 = this.DiscItem.GetAttribute(str + "02");
        string attribute3 = this.DiscItem.GetAttribute(str + "03");
        VestingType vestingTypeFromString = this.GetVestingTypeFromString(this.DiscItem.GetAttribute(str + "04"));
        string attribute4 = this.DiscItem.GetAttribute(str + "05");
        TrusteeType tType = this.DiscItem.GetAttribute(str + "06").ToLower() == "trust 1" ? TrusteeType.Trust1 : TrusteeType.Trust2;
        string attribute5 = this.DiscItem.GetAttribute(str + "07");
        string attribute6 = this.DiscItem.GetAttribute(str + "08");
        bool boolean = Utils.ParseBoolean((object) this.DiscItem.GetAttribute(str + "09"));
        string attribute7 = this.DiscItem.GetAttribute(str + "10");
        string attribute8 = this.DiscItem.GetAttribute(str + "11");
        DateTime date = Utils.ParseDate((object) this.DiscItem.GetAttribute(str + "12"));
        OccupancyStatus statusFromString = this.GetOccupancyStatusFromString(this.DiscItem.GetAttribute(str + "13"));
        OccupancyIntent intentFromString = this.GetOccupancyIntentFromString(this.DiscItem.GetAttribute(str + "14"));
        string attribute9 = this.DiscItem.GetAttribute(str + "99");
        Utils.ParseInt((object) this.DiscItem.GetAttribute(str + OrderId.ToString("00")));
        DisclosedVestingFieldsForDisclosure2015 forDisclosure2015 = new DisclosedVestingFieldsForDisclosure2015(attribute1, attribute2, attribute3, vestingTypeFromString, attribute4, tType, attribute5, attribute6, boolean, attribute7, attribute8, date, statusFromString, intentFromString, attribute9, OrderId);
        vestingFields.Add(forDisclosure2015);
      }
      return vestingFields;
    }

    private VestingType GetVestingTypeFromString(string vestingType)
    {
      if (string.IsNullOrWhiteSpace(vestingType))
        return VestingType.None;
      switch (vestingType.ToLower())
      {
        case "co-signer":
          return VestingType.Cosigner;
        case "individual":
          return VestingType.Individual;
        case "non title spouse":
          return VestingType.NonTitleSpouse;
        case "officer":
          return VestingType.Officer;
        case "settlor":
          return VestingType.Settlor;
        case "settlor trustee":
          return VestingType.SettlorTrustee;
        case "title only":
          return VestingType.TitleOnly;
        case "title only settlor trustee":
          return VestingType.TitleOnlySettlorTrustee;
        case "title only trustee":
          return VestingType.TitleOnlyTrustee;
        case "trustee":
          return VestingType.Trustee;
        default:
          return VestingType.None;
      }
    }

    private OccupancyStatus GetOccupancyStatusFromString(string occStatus)
    {
      if (string.IsNullOrWhiteSpace(occStatus))
        return OccupancyStatus.None;
      switch (occStatus.ToLower())
      {
        case "primary":
          return OccupancyStatus.PrimaryResidence;
        case "secondary":
          return OccupancyStatus.SecondHome;
        case "investment":
          return OccupancyStatus.Investor;
        default:
          return OccupancyStatus.None;
      }
    }

    private OccupancyIntent GetOccupancyIntentFromString(string occIntent)
    {
      if (string.IsNullOrWhiteSpace(occIntent))
        return OccupancyIntent.None;
      switch (occIntent.ToLower())
      {
        case "will occupy":
          return OccupancyIntent.WillOccupy;
        case "will not occupy":
          return OccupancyIntent.WillNotOccupy;
        case "currently occupy":
          return OccupancyIntent.CurrentlyOccupy;
        default:
          return OccupancyIntent.None;
      }
    }
  }
}
