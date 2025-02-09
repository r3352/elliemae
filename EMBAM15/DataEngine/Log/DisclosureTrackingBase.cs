// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DisclosureTrackingBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DisclosureTrackingBase : LogRecordBase, IDisclosureTrackingLog
  {
    private string disclosedBy = "";
    private string disclosedByFullName = "";
    protected DisclosureTrackingBase.DisclosedMethod method = DisclosureTrackingBase.DisclosedMethod.ByMail;
    private List<DisclosureTrackingFormItem> formList = new List<DisclosureTrackingFormItem>();
    protected List<DisclosedLoanItem> loanDataList = new List<DisclosedLoanItem>();
    protected Hashtable loanDataUniqueList = new Hashtable();
    private string containSafeHarbor = "False";
    private DateTime receivedDate = DateTime.MinValue;
    private string isLocked = "False";
    private DateTime disclosureCreatedDTTM = DateTime.MinValue;
    private string manuallyCreated = "False";
    protected string borrowerName = "";
    private string borrowerType = "";
    protected string coBorrowerName = "";
    private string coBorrowerType = "";
    protected string propertyAddress = "";
    protected string propertyCity = "";
    protected string propertyState = "";
    protected string propertyZip = "";
    protected string loanProgram = "";
    protected string loanAmount = "";
    protected DateTime applicationDate = DateTime.MinValue;
    private string edisclosureBorrowerName = "";
    private string edisclosureBorrowerEmail = "";
    private DateTime edisclosureBorrowerAuthenticatedDate = DateTime.MinValue;
    private string edisclosureBorrowerAuthenticatedIP = "";
    private DateTime edisclosureBorrowerViewMessageDate = DateTime.MinValue;
    private DateTime edisclosureBorrowerViewConsentDate = DateTime.MinValue;
    private DateTime edisclosureBorrowerAcceptConsentDate = DateTime.MinValue;
    private DateTime edisclosureBorrowerRejectConsentDate = DateTime.MinValue;
    private string edisclosureBorrowerAcceptConsentIP = "";
    private string edisclosureBorrowerRejectConsentIP = "";
    private DateTime edisclosureBorrowereSignedDate = DateTime.MinValue;
    private string edisclosureBorrowereSignedIP = "";
    private DateTime edisclosureBorrowerWetSignedDate = DateTime.MinValue;
    private string edisclosureCoBorrowerName = "";
    private string edisclosureCoBorrowerEmail = "";
    private DateTime edisclosureCoBorrowerAuthenticatedDate = DateTime.MinValue;
    private string edisclosureCoBorrowerAuthenticatedIP = "";
    private DateTime edisclosureCoBorrowerViewMessageDate = DateTime.MinValue;
    private DateTime edisclosureCoBorrowerViewConsentDate = DateTime.MinValue;
    private DateTime edisclosureCoBorrowerAcceptConsentDate = DateTime.MinValue;
    private DateTime edisclosureCoBorrowerRejectConsentDate = DateTime.MinValue;
    private string edisclosureCoBorrowerAcceptConsentIP = "";
    private string edisclosureCoBorrowerRejectConsentIP = "";
    private DateTime edisclosureCoBorrowereSignedDate = DateTime.MinValue;
    private string edisclosureCoBorrowereSignedIP = "";
    private DateTime edisclosureCoBorrowerWebSignedDate = DateTime.MinValue;
    private string edisclosureLOName = "";
    private string edisclosureLOUserId = "";
    private DateTime edisclosureLOViewMessageDate = DateTime.MinValue;
    private DateTime edisclosureLOeSignedDate = DateTime.MinValue;
    private string edisclosureLOeSignedIP = "";
    private DateTime edisclosurePackageCreatedDate = DateTime.MinValue;
    private string edisclosurePackageViewableFile = "";
    private bool edisclosureApplicationPackage;
    private bool edisclosureThreeDayPackage;
    private bool edisclosureLockPackage;
    private bool edisclosureApprovalPackage;
    private string edisclosurePackageID = "";
    private string edisclosureDisclosedMessage = "";
    private string edisclosureConsentPDF = "";
    private string fulfillmentOrderedBy = "";
    private DateTime fullfillmentProcessedDate = DateTime.MinValue;
    private string fulfillmentOrderedBy_CoBorrower = "";
    private DateTime fullfillmentProcessedDate_CoBorrower = DateTime.MinValue;
    private string edisclosureManuallyFulfilledBy = "";
    private DateTime edisclosureManualFulfillmentDate = DateTime.MinValue;
    private DisclosureTrackingBase.DisclosedMethod edisclosureManualFulfillmentMethod;
    private DisclosureTrackingBase.DisclosedMethod edisclosureAutomatedFulfillmentMethod;
    private string fulfillmentTrackingNumber = "";
    private string edisclosureManualFulfillmentComment = "";
    private string borrowerPairID = "";
    private string isDisclosedByLocked = "False";
    private string lockedDisclosedByField = "";
    private bool isWetSigned;
    private string isDisclosedReceivedDateLocked = "False";
    protected DateTime lockedDisclosedReceivedDate = DateTime.MinValue;
    private bool hasAccess = true;
    private DateTime eDisclosureBorrowerDocumentViewedDate;
    private DateTime eDisclosureCoborrowerDocumentViewedDate;
    private string eDisclosureBorrowerLoanLevelConsent;
    private string eDisclosureCoBorrowerLoanLevelConsent;
    private DateTime eDisclosureeSignDocViewedDate = DateTime.MinValue;
    private DateTime eDisclosureWetSignDocViewedDate = DateTime.MinValue;
    private DateTime eDisclosureInfoDocViewedDate = DateTime.MinValue;
    private string lockedBorrowerType = string.Empty;
    private string lockedCoBorrowerType = string.Empty;

    public DisclosureTrackingBase(
      DateTime date,
      LoanData loanData,
      bool manuallyCreated,
      bool containSafeHarbor,
      bool useLe1X9TimeZone)
      : base(date, "")
    {
      this.TimeZoneInfo = !useLe1X9TimeZone ? (System.TimeZoneInfo) null : (!this.UseLE1X9ForTimeZone(loanData) ? Utils.GetTimeZoneInfo("PST") : Utils.GetTimeZoneInfo(loanData.GetField("LE1.XG9") == "" ? loanData.GetField("LE1.X9") : loanData.GetField("LE1.XG9")));
      if (manuallyCreated)
        this.manuallyCreated = "True";
      this.disclosureCreatedDTTM = Utils.TruncateDateTime(date);
      if (containSafeHarbor)
        this.containSafeHarbor = "True";
      this.populateAttributes(loanData);
    }

    public DisclosureTrackingBase(LogList log, XmlElement e, bool useLe1X9TimeZone)
      : base(log, e)
    {
      if (this.Date.Kind.Equals((object) DateTimeKind.Unspecified))
        useLe1X9TimeZone = false;
      this.TimeZoneInfo = !useLe1X9TimeZone ? (System.TimeZoneInfo) null : (!this.UseLE1X9ForTimeZone(log.Loan) ? Utils.GetTimeZoneInfo("PST") : Utils.GetTimeZoneInfo(log.Loan.GetField("LE1.XG9") == "" ? log.Loan.GetField("LE1.X9") : log.Loan.GetField("LE1.XG9")));
      AttributeReader attributeReader1 = new AttributeReader(e);
      this.disclosedBy = attributeReader1.GetString(nameof (DisclosedBy));
      this.isDisclosedByLocked = attributeReader1.GetString(nameof (isDisclosedByLocked), false);
      this.disclosureCreatedDTTM = attributeReader1.GetDate(nameof (DisclosureCreatedDTTM), DateTime.MinValue);
      try
      {
        this.containSafeHarbor = attributeReader1.GetString("ContainSafeHarbor");
      }
      catch
      {
      }
      this.manuallyCreated = attributeReader1.GetString("ManuallyCreated");
      this.borrowerName = attributeReader1.GetString(nameof (BorrowerName));
      this.borrowerType = attributeReader1.GetString(nameof (BorrowerType));
      this.coBorrowerName = attributeReader1.GetString(nameof (CoBorrowerName));
      this.coBorrowerType = attributeReader1.GetString(nameof (CoBorrowerType));
      this.propertyAddress = attributeReader1.GetString(nameof (PropertyAddress));
      this.propertyCity = attributeReader1.GetString(nameof (PropertyCity));
      this.propertyState = attributeReader1.GetString(nameof (PropertyState));
      this.propertyZip = attributeReader1.GetString(nameof (PropertyZip));
      this.loanProgram = attributeReader1.GetString(nameof (LoanProgram));
      this.loanAmount = attributeReader1.GetString(nameof (LoanAmount));
      this.applicationDate = attributeReader1.GetDate(nameof (ApplicationDate), DateTime.MinValue);
      this.edisclosureBorrowerName = attributeReader1.GetString(nameof (eDisclosureBorrowerName));
      this.edisclosureBorrowerEmail = attributeReader1.GetString(nameof (eDisclosureBorrowerEmail));
      this.edisclosureBorrowerAuthenticatedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureBorrowerAuthenticatedDate), DateTime.MinValue));
      this.edisclosureBorrowerAuthenticatedIP = attributeReader1.GetString(nameof (eDisclosureBorrowerAuthenticatedIP));
      this.edisclosureBorrowerViewMessageDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureBorrowerViewMessageDate), DateTime.MinValue));
      this.edisclosureBorrowerViewConsentDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureBorrowerViewConsentDate), DateTime.MinValue));
      this.edisclosureBorrowerAcceptConsentDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureBorrowerAcceptConsentDate), DateTime.MinValue));
      this.edisclosureBorrowerRejectConsentDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureBorrowerRejectConsentDate), DateTime.MinValue));
      this.edisclosureBorrowerAcceptConsentIP = attributeReader1.GetString(nameof (eDisclosureBorrowerAcceptConsentIP));
      this.edisclosureBorrowerRejectConsentIP = attributeReader1.GetString(nameof (eDisclosureBorrowerRejectConsentIP));
      this.edisclosureBorrowereSignedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureBorrowereSignedDate), DateTime.MinValue));
      this.edisclosureBorrowereSignedIP = attributeReader1.GetString(nameof (eDisclosureBorrowereSignedIP));
      this.edisclosureBorrowerWetSignedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureBorrowerWetSignedDate), DateTime.MinValue));
      this.edisclosureCoBorrowerName = attributeReader1.GetString(nameof (eDisclosureCoBorrowerName));
      this.edisclosureCoBorrowerEmail = attributeReader1.GetString(nameof (eDisclosureCoBorrowerEmail));
      this.edisclosureCoBorrowerAuthenticatedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureCoBorrowerAuthenticatedDate), DateTime.MinValue));
      this.edisclosureCoBorrowerAuthenticatedIP = attributeReader1.GetString(nameof (eDisclosureCoBorrowerAuthenticatedIP));
      this.edisclosureCoBorrowerViewMessageDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureCoBorrowerViewMessageDate), DateTime.MinValue));
      this.edisclosureCoBorrowerViewConsentDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureCoBorrowerViewConsentDate), DateTime.MinValue));
      this.edisclosureCoBorrowerAcceptConsentDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureCoBorrowerAcceptConsentDate), DateTime.MinValue));
      this.edisclosureCoBorrowerRejectConsentDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureCoBorrowerRejectConsentDate), DateTime.MinValue));
      this.edisclosureCoBorrowerAcceptConsentIP = attributeReader1.GetString(nameof (eDisclosureCoBorrowerAcceptConsentIP));
      this.edisclosureCoBorrowerRejectConsentIP = attributeReader1.GetString(nameof (eDisclosureCoBorrowerRejectConsentIP));
      this.edisclosureCoBorrowereSignedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureCoBorrowereSignedDate), DateTime.MinValue));
      this.edisclosureCoBorrowereSignedIP = attributeReader1.GetString(nameof (eDisclosureCoBorrowereSignedIP));
      this.edisclosureCoBorrowerWebSignedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureCoBorrowerWebSignedDate), DateTime.MinValue));
      this.edisclosureLOName = attributeReader1.GetString(nameof (eDisclosureLOName));
      this.edisclosureLOViewMessageDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureLOViewMessageDate), DateTime.MinValue));
      this.edisclosureLOeSignedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureLOeSignedDate), DateTime.MinValue));
      this.edisclosureLOeSignedIP = attributeReader1.GetString(nameof (eDisclosureLOeSignedIP));
      this.edisclosurePackageCreatedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosurePackageCreatedDate), DateTime.MinValue));
      this.edisclosurePackageID = attributeReader1.GetString(nameof (eDisclosurePackageID));
      this.edisclosureConsentPDF = attributeReader1.GetString(nameof (edisclosureConsentPDF));
      this.fulfillmentOrderedBy = attributeReader1.GetString(nameof (FulfillmentOrderedBy));
      this.fullfillmentProcessedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (FullfillmentProcessedDate), DateTime.MinValue));
      this.fulfillmentOrderedBy_CoBorrower = attributeReader1.GetString(nameof (FulfillmentOrderedBy_CoBorrower));
      this.fullfillmentProcessedDate_CoBorrower = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (FullfillmentProcessedDate_CoBorrower), DateTime.MinValue));
      this.edisclosureManuallyFulfilledBy = attributeReader1.GetString(nameof (eDisclosureManuallyFulfilledBy));
      this.edisclosureManualFulfillmentDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureManualFulfillmentDate), DateTime.MinValue));
      try
      {
        this.edisclosureManualFulfillmentMethod = (DisclosureTrackingBase.DisclosedMethod) Enum.Parse(typeof (DisclosureTrackingBase.DisclosedMethod), attributeReader1.GetString(nameof (eDisclosureManualFulfillmentMethod)), true);
      }
      catch
      {
      }
      try
      {
        this.edisclosureAutomatedFulfillmentMethod = (DisclosureTrackingBase.DisclosedMethod) Enum.Parse(typeof (DisclosureTrackingBase.DisclosedMethod), attributeReader1.GetString(nameof (eDisclosureAutomatedFulfillmentMethod)), true);
      }
      catch
      {
      }
      this.fulfillmentTrackingNumber = attributeReader1.GetString(nameof (fulfillmentTrackingNumber));
      this.edisclosureManualFulfillmentComment = attributeReader1.GetString(nameof (eDisclosureManualFulfillmentComment));
      this.edisclosurePackageViewableFile = attributeReader1.GetString(nameof (eDisclosurePackageViewableFile));
      this.edisclosureApplicationPackage = attributeReader1.GetBoolean(nameof (eDisclosureApplicationPackage), false);
      this.edisclosureThreeDayPackage = attributeReader1.GetBoolean(nameof (eDisclosureThreeDayPackage), false);
      this.edisclosureLockPackage = attributeReader1.GetBoolean(nameof (eDisclosureLockPackage), false);
      this.edisclosureApprovalPackage = attributeReader1.GetBoolean(nameof (eDisclosureApprovalPackage), false);
      this.edisclosureDisclosedMessage = attributeReader1.GetString(nameof (edisclosureDisclosedMessage));
      this.borrowerPairID = attributeReader1.GetString(nameof (BorrowerPairID), false);
      this.lockedDisclosedReceivedDate = attributeReader1.GetDate(nameof (lockedDisclosedReceivedDate), DateTime.MinValue);
      this.isWetSigned = attributeReader1.GetBoolean(nameof (isWetSigned), false);
      this.eDisclosureBorrowerDocumentViewedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureBorrowerDocumentViewedDate), DateTime.MinValue));
      this.eDisclosureCoborrowerDocumentViewedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureCoborrowerDocumentViewedDate), DateTime.MinValue));
      this.eDisclosureBorrowerLoanLevelConsent = attributeReader1.GetString(nameof (eDisclosureBorrowerLoanLevelConsent));
      this.eDisclosureCoBorrowerLoanLevelConsent = attributeReader1.GetString(nameof (eDisclosureCoBorrowerLoanLevelConsent));
      this.eDisclosureeSignDocViewedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureeSignDocViewedDate), DateTime.MinValue));
      this.eDisclosureWetSignDocViewedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureWetSignDocViewedDate), DateTime.MinValue));
      this.eDisclosureInfoDocViewedDate = this.ConvertToLoanTimeZone(attributeReader1.GetDate(nameof (eDisclosureInfoDocViewedDate), DateTime.MinValue));
      try
      {
        this.method = (DisclosureTrackingBase.DisclosedMethod) Enum.Parse(typeof (DisclosureTrackingBase.DisclosedMethod), attributeReader1.GetString("DisclosedMethod"), true);
      }
      catch
      {
      }
      XmlNodeList xmlNodeList1 = e.SelectNodes("Forms/Form");
      for (int i = 0; i < xmlNodeList1.Count; ++i)
      {
        AttributeReader attributeReader2 = new AttributeReader((XmlElement) xmlNodeList1[i]);
        this.formList.Add(new DisclosureTrackingFormItem(attributeReader2.GetString("Name"), (DisclosureTrackingFormItem.FormType) Enum.Parse(typeof (DisclosureTrackingFormItem.FormType), attributeReader2.GetString("Type"))));
      }
      XmlNodeList xmlNodeList2 = e.SelectNodes("Fields/Field");
      for (int i = 0; i < xmlNodeList2.Count; ++i)
      {
        AttributeReader attributeReader3 = new AttributeReader((XmlElement) xmlNodeList2[i]);
        if (attributeReader3.GetString("FieldID") == nameof (edisclosureDisclosedMessage))
          this.edisclosureDisclosedMessage = attributeReader3.GetString("FieldValue");
        else if (attributeReader3.GetString("FieldID") == "DISCLOSURE.X867")
          this.containSafeHarbor = "True";
        this.loanDataList.Add(new DisclosedLoanItem(attributeReader3.GetString("FieldID"), attributeReader3.GetString("FieldValue")));
      }
      this.MarkAsClean();
    }

    public bool UseLE1X9ForTimeZone(LoanData loanData)
    {
      return Mapping.UseNewCompliance(21.104M, loanData.GetField("COMPLIANCEVERSION.X1"));
    }

    public static List<string> GetDisclosedSafeHarborFields()
    {
      List<string> safeHarborFields = new List<string>();
      safeHarborFields.AddRange((IEnumerable<string>) new string[16]
      {
        "4000",
        "4001",
        "4002",
        "4003",
        "4004",
        "4005",
        "4006",
        "4007",
        "11",
        "12",
        "14",
        "15",
        "1109",
        "3",
        "4",
        "325"
      });
      for (int index = 688; index < 868; ++index)
        safeHarborFields.Add("DISCLOSURE.X" + (object) index);
      for (int index = 977; index <= 992; ++index)
        safeHarborFields.Add("DISCLOSURE.X" + (object) index);
      return safeHarborFields;
    }

    public System.TimeZoneInfo TimeZoneInfo { get; private set; }

    public virtual DateTime DisclosedDate
    {
      get => this.date;
      set
      {
        if (this.IsLocked)
          this.date = value;
        this.MarkAsDirty();
      }
    }

    public virtual bool IsLocked
    {
      get => this.isLocked == "True";
      set
      {
        if (value)
        {
          this.isLocked = "True";
          this.date = this.Date;
        }
        else
        {
          this.isLocked = "False";
          this.date = DateTime.Parse(this.Date.ToString("MM/dd/yyyy  HH:mm:ss"));
        }
        this.MarkAsDirty();
      }
    }

    public DateTime DateAdded => this.DisclosureCreatedDTTM;

    public string DisclosedBy
    {
      get => this.disclosedBy;
      set
      {
        this.disclosedBy = value;
        this.MarkAsDirty();
      }
    }

    public virtual string DisclosedByFullName
    {
      get => this.IsDisclosedByLocked ? this.lockedDisclosedByField : this.disclosedByFullName;
      set
      {
        if (this.IsDisclosedByLocked)
          this.lockedDisclosedByField = value;
        else
          this.disclosedByFullName = value;
        this.MarkAsDirty();
      }
    }

    public bool DisclosedForSafeHarbor => this.containSafeHarbor == "True";

    public string eDisclosurePackageViewableFile
    {
      get => this.edisclosurePackageViewableFile;
      set
      {
        this.edisclosurePackageViewableFile = value;
        this.MarkAsDirty();
      }
    }

    public bool eDisclosureApplicationPackage
    {
      get => this.edisclosureApplicationPackage;
      set
      {
        this.edisclosureApplicationPackage = value;
        this.MarkAsDirty();
      }
    }

    public string eDisclosureDisclosedMessage
    {
      get
      {
        return this.edisclosureDisclosedMessage != "" ? this.edisclosureDisclosedMessage : this.GetDisclosedField("edisclosureDisclosedMessage");
      }
      set
      {
        this.edisclosureDisclosedMessage = value;
        this.SetDisclosedField("edisclosureDisclosedMessage", value);
      }
    }

    public bool IsWetSigned
    {
      get => this.isWetSigned;
      set
      {
        this.isWetSigned = value;
        this.MarkAsDirty();
      }
    }

    public EnhancedDisclosureTracking2015Log.DisclosureTrackingDocuments Documents { get; set; }

    public bool HasAccess
    {
      get => this.hasAccess;
      set => this.hasAccess = value;
    }

    public virtual DateTime ReceivedDate
    {
      get
      {
        return this.IsDisclosedReceivedDateLocked ? this.lockedDisclosedReceivedDate : this.receivedDate;
      }
      set
      {
        if (this.IsDisclosedReceivedDateLocked)
          this.lockedDisclosedReceivedDate = value;
        else
          this.receivedDate = value;
        this.MarkAsDirty();
      }
    }

    public virtual bool IsDisclosedReceivedDateLocked
    {
      get => this.isDisclosedReceivedDateLocked == "True";
      set
      {
        this.isDisclosedReceivedDateLocked = !value ? "False" : "True";
        this.lockedDisclosedReceivedDate = this.ReceivedDate;
        this.MarkAsDirty();
      }
    }

    public DateTime GetReceivedDateFromCalc => this.receivedDate;

    public virtual DateTime DisclosureCreatedDTTM => this.disclosureCreatedDTTM;

    public bool eDisclosureThreeDayPackage
    {
      get => this.edisclosureThreeDayPackage;
      set
      {
        this.edisclosureThreeDayPackage = value;
        this.MarkAsDirty();
      }
    }

    public bool eDisclosureLockPackage
    {
      get => this.edisclosureLockPackage;
      set
      {
        this.edisclosureLockPackage = value;
        this.MarkAsDirty();
      }
    }

    public bool eDisclosureApprovalPackage
    {
      get => this.edisclosureApprovalPackage;
      set
      {
        this.edisclosureApprovalPackage = value;
        this.MarkAsDirty();
      }
    }

    public bool Received => this.receivedDate != DateTime.MinValue;

    public virtual void populateAttributes(LoanData loandata)
    {
      this.borrowerName = loandata.GetField("36") + " " + loandata.GetField("37");
      this.borrowerType = loandata.GetField("4008");
      this.coBorrowerName = loandata.GetField("68") + " " + loandata.GetField("69");
      this.coBorrowerType = loandata.GetField("4009");
      this.propertyAddress = loandata.GetField("11");
      this.propertyCity = loandata.GetField("12");
      this.propertyState = loandata.GetField("14");
      this.propertyZip = loandata.GetField("15");
      this.loanProgram = loandata.GetField("1401");
      this.loanAmount = loandata.GetField("2");
      this.applicationDate = Utils.ParseDate((object) loandata.GetField("745"), DateTime.MinValue);
    }

    [CLSCompliant(false)]
    public virtual string BorrowerName
    {
      get
      {
        if (this.borrowerName == "" && this.Log != null && this.Log.Loan != null)
        {
          this.borrowerName = this.Log.Loan.GetField("1868");
          if (this.borrowerName == "")
            this.borrowerName = this.FormatName(this.Log.Loan.GetField("4000"), this.Log.Loan.GetField("4001"), this.Log.Loan.GetField("4002"), this.Log.Loan.GetField("4003"));
        }
        return this.borrowerName;
      }
    }

    public virtual string FormatName(
      string firstName,
      string middleName,
      string lastName,
      string suffix)
    {
      return firstName.Trim() + (middleName.Trim() != "" ? " " + middleName.Trim() : "") + (lastName.Trim() != "" ? " " + lastName.Trim() : "") + (suffix.Trim() != "" ? " " + suffix.Trim() : "");
    }

    public string BorrowerType
    {
      get => this.borrowerType;
      set => this.SetDisclosedField("4008", value);
    }

    [CLSCompliant(false)]
    public virtual string CoBorrowerName
    {
      get
      {
        if (this.coBorrowerName == "" && this.borrowerName == "" && this.Log != null && this.Log.Loan != null)
        {
          this.coBorrowerName = this.Log.Loan.GetField("1873");
          if (this.coBorrowerName == "")
            this.coBorrowerName = this.FormatName(this.Log.Loan.GetField("4004"), this.Log.Loan.GetField("4005"), this.Log.Loan.GetField("4006"), this.Log.Loan.GetField("4007"));
        }
        return this.coBorrowerName;
      }
    }

    public string CoBorrowerType
    {
      get => this.coBorrowerType;
      set => this.SetDisclosedField("4009", value);
    }

    [CLSCompliant(false)]
    public virtual string PropertyAddress => this.propertyAddress;

    [CLSCompliant(false)]
    public virtual string PropertyCity => this.propertyCity;

    [CLSCompliant(false)]
    public virtual string PropertyState => this.propertyState;

    [CLSCompliant(false)]
    public virtual string PropertyZip => this.propertyZip;

    [CLSCompliant(false)]
    public virtual string LoanProgram => this.loanProgram;

    [CLSCompliant(false)]
    public virtual string LoanAmount => this.loanAmount;

    public bool IsManuallyCreated => this.manuallyCreated == "True";

    public bool IsDisclosedByLocked
    {
      get => this.isDisclosedByLocked == "True";
      set
      {
        this.isDisclosedByLocked = !value ? "False" : "True";
        this.lockedDisclosedByField = this.DisclosedByFullName;
        this.MarkAsDirty();
      }
    }

    [CLSCompliant(false)]
    public virtual DateTime ApplicationDate => this.applicationDate;

    public DisclosureTrackingFormItem[] DisclosedFormList => this.formList.ToArray();

    public DisclosedLoanItem[] DisclosedData => this.loanDataList.ToArray();

    public Dictionary<string, string> GetDisclosedFields(List<string> fieldIDs)
    {
      Dictionary<string, string> disclosedFields = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (DisclosedLoanItem loanData in this.loanDataList)
      {
        if (fieldIDs.Contains(loanData.FieldID) && !disclosedFields.ContainsKey(loanData.FieldID))
          disclosedFields.Add(loanData.FieldID, loanData.FieldValue);
      }
      return disclosedFields;
    }

    public void PopulateLoanDataList(Dictionary<string, string> dataFields)
    {
      foreach (KeyValuePair<string, string> dataField in dataFields)
      {
        if (!this.loanDataUniqueList.Contains((object) dataField.Key))
        {
          this.loanDataList.Add(new DisclosedLoanItem(dataField.Key, dataField.Value));
          this.loanDataUniqueList.Add((object) dataField.Key, (object) dataField.Value);
        }
      }
    }

    public virtual string GetDisclosedField(string fieldId)
    {
      foreach (DisclosedLoanItem loanData in this.loanDataList)
      {
        if (string.Compare(loanData.FieldID, fieldId, true) == 0)
          return loanData.FieldValue;
      }
      return "";
    }

    public bool IsFieldExist(string fieldId) => this.loanDataUniqueList.Contains((object) fieldId);

    public void SetDisclosedField(string fieldId, string value)
    {
      foreach (DisclosedLoanItem loanData in this.loanDataList)
      {
        if (string.Compare(loanData.FieldID, fieldId, true) == 0)
        {
          loanData.FieldValue = value;
          break;
        }
      }
    }

    public bool IsFieldLocked(string fieldId)
    {
      foreach (DisclosedLoanItem loanData in this.loanDataList)
      {
        if (string.Compare(loanData.FieldID, fieldId + "_LOCKED", true) == 0)
          return true;
      }
      return false;
    }

    public void AddDisclosedFormItem(string formName, DisclosureTrackingFormItem.FormType formType)
    {
      this.MarkAsDirty();
      this.formList.Add(new DisclosureTrackingFormItem(formName, formType));
    }

    public void AddDisclosedLoanInfo(string fieldID, string fieldValue)
    {
      this.MarkAsDirty();
      if (this.loanDataUniqueList.Contains((object) fieldID))
        return;
      this.loanDataList.Add(new DisclosedLoanItem(fieldID, fieldValue));
      this.loanDataUniqueList.Add((object) fieldID, (object) fieldValue);
    }

    public string AddDisclosedFieldString(string fieldID, string fieldValue)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(fieldID).Append("~!@").Append(fieldValue).Append("!~@");
      return stringBuilder.ToString();
    }

    public int NumOfDisclosedDocs => this.formList != null ? this.formList.Count : 0;

    public virtual DateTime eDisclosurePackageCreatedDate
    {
      get => this.edisclosurePackageCreatedDate;
      set => this.edisclosurePackageCreatedDate = value;
    }

    public string eDisclosureBorrowerName
    {
      get => this.edisclosureBorrowerName;
      set => this.edisclosureBorrowerName = value;
    }

    public string eDisclosureBorrowerEmail
    {
      get => this.edisclosureBorrowerEmail;
      set => this.edisclosureBorrowerEmail = value;
    }

    public virtual DateTime eDisclosureBorrowerAuthenticatedDate
    {
      get => this.edisclosureBorrowerAuthenticatedDate;
      set => this.edisclosureBorrowerAuthenticatedDate = value;
    }

    public string eDisclosureBorrowerAuthenticatedIP
    {
      get => this.edisclosureBorrowerAuthenticatedIP;
      set => this.edisclosureBorrowerAuthenticatedIP = value;
    }

    public virtual DateTime eDisclosureBorrowerViewMessageDate
    {
      get => this.edisclosureBorrowerViewMessageDate;
      set => this.edisclosureBorrowerViewMessageDate = value;
    }

    public virtual DateTime eDisclosureBorrowerViewConsentDate
    {
      get => this.edisclosureBorrowerViewConsentDate;
      set => this.edisclosureBorrowerViewConsentDate = value;
    }

    public virtual DateTime eDisclosureBorrowerAcceptConsentDate
    {
      get => this.edisclosureBorrowerAcceptConsentDate;
      set => this.edisclosureBorrowerAcceptConsentDate = value;
    }

    public virtual DateTime eDisclosureBorrowerRejectConsentDate
    {
      get => this.edisclosureBorrowerRejectConsentDate;
      set => this.edisclosureBorrowerRejectConsentDate = value;
    }

    public string eDisclosureBorrowerAcceptConsentIP
    {
      get => this.edisclosureBorrowerAcceptConsentIP;
      set => this.edisclosureBorrowerAcceptConsentIP = value;
    }

    public string eDisclosureBorrowerRejectConsentIP
    {
      get => this.edisclosureBorrowerRejectConsentIP;
      set => this.edisclosureBorrowerRejectConsentIP = value;
    }

    public virtual DateTime eDisclosureBorrowereSignedDate
    {
      get => this.edisclosureBorrowereSignedDate;
      set => this.edisclosureBorrowereSignedDate = value;
    }

    public string eDisclosureBorrowereSignedIP
    {
      get => this.edisclosureBorrowereSignedIP;
      set => this.edisclosureBorrowereSignedIP = value;
    }

    public virtual DateTime eDisclosureBorrowerWetSignedDate
    {
      get => this.edisclosureBorrowerWetSignedDate;
      set => this.edisclosureBorrowerWetSignedDate = value;
    }

    public string eDisclosureCoBorrowerName
    {
      get => this.edisclosureCoBorrowerName;
      set => this.edisclosureCoBorrowerName = value;
    }

    public string eDisclosureCoBorrowerEmail
    {
      get => this.edisclosureCoBorrowerEmail;
      set => this.edisclosureCoBorrowerEmail = value;
    }

    public virtual DateTime eDisclosureCoBorrowerAuthenticatedDate
    {
      get => this.edisclosureCoBorrowerAuthenticatedDate;
      set => this.edisclosureCoBorrowerAuthenticatedDate = value;
    }

    public string eDisclosureCoBorrowerAuthenticatedIP
    {
      get => this.edisclosureCoBorrowerAuthenticatedIP;
      set => this.edisclosureCoBorrowerAuthenticatedIP = value;
    }

    public virtual DateTime eDisclosureCoBorrowerViewMessageDate
    {
      get => this.edisclosureCoBorrowerViewMessageDate;
      set => this.edisclosureCoBorrowerViewMessageDate = value;
    }

    public virtual DateTime eDisclosureCoBorrowerViewConsentDate
    {
      get => this.edisclosureCoBorrowerViewConsentDate;
      set => this.edisclosureCoBorrowerViewConsentDate = value;
    }

    public virtual DateTime eDisclosureCoBorrowerAcceptConsentDate
    {
      get => this.edisclosureCoBorrowerAcceptConsentDate;
      set => this.edisclosureCoBorrowerAcceptConsentDate = value;
    }

    public virtual DateTime eDisclosureCoBorrowerRejectConsentDate
    {
      get => this.edisclosureCoBorrowerRejectConsentDate;
      set => this.edisclosureCoBorrowerRejectConsentDate = value;
    }

    public string eDisclosureCoBorrowerAcceptConsentIP
    {
      get => this.edisclosureCoBorrowerAcceptConsentIP;
      set => this.edisclosureCoBorrowerAcceptConsentIP = value;
    }

    public string eDisclosureCoBorrowerRejectConsentIP
    {
      get => this.edisclosureCoBorrowerRejectConsentIP;
      set => this.edisclosureCoBorrowerRejectConsentIP = value;
    }

    public virtual DateTime eDisclosureCoBorrowereSignedDate
    {
      get => this.edisclosureCoBorrowereSignedDate;
      set => this.edisclosureCoBorrowereSignedDate = value;
    }

    public string eDisclosureCoBorrowereSignedIP
    {
      get => this.edisclosureCoBorrowereSignedIP;
      set => this.edisclosureCoBorrowereSignedIP = value;
    }

    public virtual DateTime eDisclosureCoBorrowerWebSignedDate
    {
      get => this.edisclosureCoBorrowerWebSignedDate;
      set => this.edisclosureCoBorrowerWebSignedDate = value;
    }

    public string eDisclosureLOName
    {
      get => this.edisclosureLOName;
      set => this.edisclosureLOName = value;
    }

    public string eDisclosureLOUserId
    {
      get => this.edisclosureLOUserId;
      set => this.edisclosureLOUserId = value;
    }

    public virtual DateTime eDisclosureLOViewMessageDate
    {
      get => this.edisclosureLOViewMessageDate;
      set => this.edisclosureLOViewMessageDate = value;
    }

    public virtual DateTime eDisclosureLOeSignedDate
    {
      get => this.edisclosureLOeSignedDate;
      set => this.edisclosureLOeSignedDate = value;
    }

    public string eDisclosureLOeSignedIP
    {
      get => this.edisclosureLOeSignedIP;
      set => this.edisclosureLOeSignedIP = value;
    }

    public string eDisclosurePackageID
    {
      get => this.edisclosurePackageID;
      set => this.edisclosurePackageID = value;
    }

    public string eDisclosureConsentPDF
    {
      get => this.edisclosureConsentPDF;
      set => this.edisclosureConsentPDF = value;
    }

    public string FulfillmentOrderedBy
    {
      get => this.fulfillmentOrderedBy;
      set => this.fulfillmentOrderedBy = value;
    }

    public virtual DateTime FullfillmentProcessedDate
    {
      get => this.fullfillmentProcessedDate;
      set => this.fullfillmentProcessedDate = value;
    }

    public string FulfillmentOrderedBy_CoBorrower
    {
      get => this.fulfillmentOrderedBy_CoBorrower;
      set => this.fulfillmentOrderedBy_CoBorrower = value;
    }

    public virtual DateTime FullfillmentProcessedDate_CoBorrower
    {
      get => this.fullfillmentProcessedDate_CoBorrower;
      set => this.fullfillmentProcessedDate_CoBorrower = value;
    }

    public string eDisclosureManuallyFulfilledBy
    {
      get => this.edisclosureManuallyFulfilledBy;
      set
      {
        this.edisclosureManuallyFulfilledBy = value;
        this.MarkAsDirty();
      }
    }

    public virtual DateTime eDisclosureManualFulfillmentDate
    {
      get => this.edisclosureManualFulfillmentDate;
      set
      {
        this.edisclosureManualFulfillmentDate = value;
        this.MarkAsDirty();
      }
    }

    public virtual DisclosureTrackingBase.DisclosedMethod eDisclosureManualFulfillmentMethod
    {
      get => this.edisclosureManualFulfillmentMethod;
      set
      {
        this.edisclosureManualFulfillmentMethod = value;
        this.MarkAsDirty();
      }
    }

    public virtual DisclosureTrackingBase.DisclosedMethod eDisclosureAutomatedFulfillmentMethod
    {
      get => this.edisclosureAutomatedFulfillmentMethod;
      set
      {
        this.edisclosureAutomatedFulfillmentMethod = value;
        this.MarkAsDirty();
      }
    }

    public string FulfillmentTrackingNumber
    {
      get => this.fulfillmentTrackingNumber;
      set
      {
        this.fulfillmentTrackingNumber = value;
        this.MarkAsDirty();
      }
    }

    public string eDisclosureManualFulfillmentComment
    {
      get => this.edisclosureManualFulfillmentComment;
      set
      {
        this.edisclosureManualFulfillmentComment = value;
        this.MarkAsDirty();
      }
    }

    public string BorrowerPairID
    {
      get => this.borrowerPairID;
      set => this.borrowerPairID = value;
    }

    public virtual DateTime EDisclosureBorrowerDocumentViewedDate
    {
      get => this.eDisclosureBorrowerDocumentViewedDate;
      set => this.eDisclosureBorrowerDocumentViewedDate = value;
    }

    public virtual DateTime EDisclosureCoborrowerDocumentViewedDate
    {
      get => this.eDisclosureCoborrowerDocumentViewedDate;
      set => this.eDisclosureCoborrowerDocumentViewedDate = value;
    }

    public string EDisclosureBorrowerLoanLevelConsent
    {
      get
      {
        if (this.eDisclosureBorrowerLoanLevelConsent == "1")
          return "Accepted";
        if (this.eDisclosureBorrowerLoanLevelConsent == "0")
          return "Rejected";
        return this.eDisclosureBorrowerLoanLevelConsent == "" ? "" : this.eDisclosureBorrowerLoanLevelConsent;
      }
      set => this.eDisclosureBorrowerLoanLevelConsent = value;
    }

    public string EDisclosureCoBorrowerLoanLevelConsent
    {
      get
      {
        if (this.eDisclosureCoBorrowerLoanLevelConsent == "1")
          return "Accepted";
        if (this.eDisclosureCoBorrowerLoanLevelConsent == "0")
          return "Rejected";
        return this.eDisclosureCoBorrowerLoanLevelConsent == "" ? "" : this.eDisclosureCoBorrowerLoanLevelConsent;
      }
      set => this.eDisclosureCoBorrowerLoanLevelConsent = value;
    }

    public virtual DateTime EDisclosureeSignDocViewedDate
    {
      get => this.eDisclosureeSignDocViewedDate;
      set => this.eDisclosureeSignDocViewedDate = value;
    }

    public virtual DateTime EDisclosureWetSignDocViewedDate
    {
      get => this.eDisclosureWetSignDocViewedDate;
      set => this.eDisclosureWetSignDocViewedDate = value;
    }

    public virtual DateTime EDisclosureInfoDocViewedDate
    {
      get => this.eDisclosureInfoDocViewedDate;
      set => this.eDisclosureInfoDocViewedDate = value;
    }

    public virtual DateTime eDisclosureBorrowerInformationalViewedDate { get; set; }

    public virtual string eDisclosureBorrowerInformationalViewedIP { get; set; }

    public virtual DateTime eDisclosureBorrowerInformationalCompletedDate { get; set; }

    public virtual string eDisclosureBorrowerInformationalCompletedIP { get; set; }

    public virtual DateTime eDisclosureCoBorrowerInformationalViewedDate { get; set; }

    public virtual string eDisclosureCoBorrowerInformationalViewedIP { get; set; }

    public virtual DateTime eDisclosureCoBorrowerInformationalCompletedDate { get; set; }

    public virtual string eDisclosureCoBorrowerInformationalCompletedIP { get; set; }

    public virtual DateTime eDisclosureLOInformationalViewedDate { get; set; }

    public virtual string eDisclosureLOInformationalViewedIP { get; set; }

    public virtual DateTime eDisclosureLOInformationalCompletedDate { get; set; }

    public virtual string eDisclosureLOInformationalCompletedIP { get; set; }

    public override void MarkAsClean() => base.MarkAsClean();

    public override bool IsDirty() => base.IsDirty();

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("DisclosedBy", (object) this.disclosedBy);
      attributeWriter.Write("DisclosedMethod", (object) this.method.ToString());
      attributeWriter.Write("BorrowerPairID", (object) this.borrowerPairID);
      if (this.disclosureCreatedDTTM != DateTime.MinValue)
        attributeWriter.Write("DisclosureCreatedDTTM", (object) this.disclosureCreatedDTTM);
      else
        attributeWriter.Write("DisclosureCreatedDTTM", (object) "");
      attributeWriter.Write("ContainSafeHarbor", (object) this.containSafeHarbor);
      attributeWriter.Write("ManuallyCreated", (object) this.manuallyCreated);
      attributeWriter.Write("BorrowerName", (object) this.borrowerName);
      attributeWriter.Write("BorrowerType", (object) this.borrowerType);
      attributeWriter.Write("CoBorrowerName", (object) this.coBorrowerName);
      attributeWriter.Write("CoBorrowerType", (object) this.coBorrowerType);
      attributeWriter.Write("PropertyAddress", (object) this.propertyAddress);
      attributeWriter.Write("PropertyCity", (object) this.propertyCity);
      attributeWriter.Write("PropertyState", (object) this.propertyState);
      attributeWriter.Write("PropertyZip", (object) this.propertyZip);
      attributeWriter.Write("LoanProgram", (object) this.loanProgram);
      attributeWriter.Write("LoanAmount", (object) this.loanAmount);
      attributeWriter.Write("ApplicationDate", (object) this.applicationDate);
      attributeWriter.Write("eDisclosureBorrowerName", (object) this.edisclosureBorrowerName);
      attributeWriter.Write("eDisclosureBorrowerEmail", (object) this.edisclosureBorrowerEmail);
      attributeWriter.Write("eDisclosureBorrowerAuthenticatedDate", (object) this.ConvertToUtc(this.edisclosureBorrowerAuthenticatedDate));
      attributeWriter.Write("eDisclosureBorrowerAuthenticatedIP", (object) this.edisclosureBorrowerAuthenticatedIP);
      attributeWriter.Write("eDisclosureBorrowerViewMessageDate", (object) this.ConvertToUtc(this.edisclosureBorrowerViewMessageDate));
      attributeWriter.Write("eDisclosureBorrowerViewConsentDate", (object) this.ConvertToUtc(this.edisclosureBorrowerViewConsentDate));
      attributeWriter.Write("eDisclosureBorrowerAcceptConsentDate", (object) this.ConvertToUtc(this.edisclosureBorrowerAcceptConsentDate));
      attributeWriter.Write("eDisclosureBorrowerRejectConsentDate", (object) this.ConvertToUtc(this.edisclosureBorrowerRejectConsentDate));
      attributeWriter.Write("eDisclosureBorrowerAcceptConsentIP", (object) this.edisclosureBorrowerAcceptConsentIP);
      attributeWriter.Write("eDisclosureBorrowerRejectConsentIP", (object) this.edisclosureBorrowerRejectConsentIP);
      attributeWriter.Write("eDisclosureBorrowereSignedDate", (object) this.ConvertToUtc(this.edisclosureBorrowereSignedDate));
      attributeWriter.Write("eDisclosureBorrowereSignedIP", (object) this.edisclosureBorrowereSignedIP);
      attributeWriter.Write("eDisclosureBorrowerWetSignedDate", (object) this.ConvertToUtc(this.edisclosureBorrowerWetSignedDate));
      attributeWriter.Write("eDisclosureCoBorrowerName", (object) this.edisclosureCoBorrowerName);
      attributeWriter.Write("eDisclosureCoBorrowerEmail", (object) this.edisclosureCoBorrowerEmail);
      attributeWriter.Write("eDisclosureCoBorrowerAuthenticatedDate", (object) this.ConvertToUtc(this.edisclosureCoBorrowerAuthenticatedDate));
      attributeWriter.Write("eDisclosureCoBorrowerAuthenticatedIP", (object) this.edisclosureCoBorrowerAuthenticatedIP);
      attributeWriter.Write("eDisclosureCoBorrowerViewMessageDate", (object) this.ConvertToUtc(this.edisclosureCoBorrowerViewMessageDate));
      attributeWriter.Write("eDisclosureCoBorrowerViewConsentDate", (object) this.ConvertToUtc(this.edisclosureCoBorrowerViewConsentDate));
      attributeWriter.Write("eDisclosureCoBorrowerAcceptConsentDate", (object) this.ConvertToUtc(this.edisclosureCoBorrowerAcceptConsentDate));
      attributeWriter.Write("eDisclosureCoBorrowerRejectConsentDate", (object) this.ConvertToUtc(this.edisclosureCoBorrowerRejectConsentDate));
      attributeWriter.Write("eDisclosureCoBorrowerAcceptConsentIP", (object) this.edisclosureCoBorrowerAcceptConsentIP);
      attributeWriter.Write("eDisclosureCoBorrowerRejectConsentIP", (object) this.edisclosureCoBorrowerRejectConsentIP);
      attributeWriter.Write("eDisclosureCoBorrowereSignedDate", (object) this.ConvertToUtc(this.edisclosureCoBorrowereSignedDate));
      attributeWriter.Write("eDisclosureCoBorrowereSignedIP", (object) this.edisclosureCoBorrowereSignedIP);
      attributeWriter.Write("eDisclosureCoBorrowerWebSignedDate", (object) this.ConvertToUtc(this.edisclosureCoBorrowerWebSignedDate));
      attributeWriter.Write("eDisclosureLOName", (object) this.edisclosureLOName);
      attributeWriter.Write("eDisclosureLOViewMessageDate", (object) this.ConvertToUtc(this.edisclosureLOViewMessageDate));
      attributeWriter.Write("eDisclosureLOeSignedDate", (object) this.ConvertToUtc(this.edisclosureLOeSignedDate));
      attributeWriter.Write("eDisclosureLOeSignedIP", (object) this.edisclosureLOeSignedIP);
      attributeWriter.Write("eDisclosurePackageID", (object) this.edisclosurePackageID);
      attributeWriter.Write("FulfillmentOrderedBy", (object) this.fulfillmentOrderedBy);
      attributeWriter.Write("eDisclosureManuallyFulfilledBy", (object) this.edisclosureManuallyFulfilledBy);
      attributeWriter.Write("eDisclosureManualFulfillmentDate", (object) this.ConvertToUtc(this.edisclosureManualFulfillmentDate));
      attributeWriter.Write("eDisclosureManualFulfillmentMethod", (object) this.edisclosureManualFulfillmentMethod.ToString());
      attributeWriter.Write("eDisclosureAutomatedFulfillmentMethod", (object) this.edisclosureAutomatedFulfillmentMethod.ToString());
      attributeWriter.Write("fulfillmentTrackingNumber", (object) this.fulfillmentTrackingNumber.ToString());
      attributeWriter.Write("eDisclosureManualFulfillmentComment", (object) this.edisclosureManualFulfillmentComment);
      attributeWriter.Write("eDisclosurePackageCreatedDate", (object) this.ConvertToUtc(this.edisclosurePackageCreatedDate));
      attributeWriter.Write("FullfillmentProcessedDate", (object) this.ConvertToUtc(this.fullfillmentProcessedDate));
      attributeWriter.Write("eDisclosurePackageViewableFile", (object) this.edisclosurePackageViewableFile);
      attributeWriter.Write("eDisclosureApplicationPackage", (object) this.edisclosureApplicationPackage);
      attributeWriter.Write("eDisclosureThreeDayPackage", (object) this.edisclosureThreeDayPackage);
      attributeWriter.Write("eDisclosureLockPackage", (object) this.edisclosureLockPackage);
      attributeWriter.Write("eDisclosureApprovalPackage", (object) this.edisclosureApprovalPackage);
      attributeWriter.Write("isWetSigned", (object) this.isWetSigned);
      attributeWriter.Write("isDisclosedByLocked", (object) this.isDisclosedByLocked);
      attributeWriter.Write("lockedDisclosedReceivedDate", (object) this.lockedDisclosedReceivedDate);
      attributeWriter.Write("edisclosureDisclosedMessage", (object) this.edisclosureDisclosedMessage);
      attributeWriter.Write("edisclosureConsentPDF", (object) this.edisclosureConsentPDF);
      attributeWriter.Write("FullfillmentProcessedDate_CoBorrower", (object) this.ConvertToUtc(this.fullfillmentProcessedDate_CoBorrower));
      attributeWriter.Write("FulfillmentOrderedBy_CoBorrower", (object) this.fulfillmentOrderedBy_CoBorrower);
      attributeWriter.Write("eDisclosureBorrowerDocumentViewedDate", (object) this.ConvertToUtc(this.eDisclosureBorrowerDocumentViewedDate));
      attributeWriter.Write("eDisclosureCoborrowerDocumentViewedDate", (object) this.ConvertToUtc(this.eDisclosureCoborrowerDocumentViewedDate));
      attributeWriter.Write("eDisclosureBorrowerLoanLevelConsent", (object) this.eDisclosureBorrowerLoanLevelConsent);
      attributeWriter.Write("eDisclosureCoBorrowerLoanLevelConsent", (object) this.eDisclosureCoBorrowerLoanLevelConsent);
      attributeWriter.Write("eDisclosureeSignDocViewedDate", (object) this.ConvertToUtc(this.eDisclosureeSignDocViewedDate));
      attributeWriter.Write("eDisclosureWetSignDocViewedDate", (object) this.ConvertToUtc(this.eDisclosureWetSignDocViewedDate));
      attributeWriter.Write("eDisclosureInfoDocViewedDate", (object) this.ConvertToUtc(this.eDisclosureInfoDocViewedDate));
      XmlElement element1 = e.OwnerDocument.CreateElement("Forms");
      for (int index = 0; index < this.formList.Count; ++index)
      {
        DisclosureTrackingFormItem form = this.formList[index];
        XmlElement element2 = e.OwnerDocument.CreateElement("Form");
        element2.SetAttribute("Name", form.FormName);
        element2.SetAttribute("Type", form.OutputFormType.ToString());
        element1.AppendChild((XmlNode) element2);
      }
      e.AppendChild((XmlNode) element1);
    }

    public override int CompareTo(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return base.CompareTo(obj);
      if (!(obj is DisclosureTrackingBase disclosureTrackingBase))
        throw new Exception("Invalid value for comparison");
      if (this.Date > disclosureTrackingBase.Date)
        return 1;
      if (!(this.Date == disclosureTrackingBase.Date))
        return -1;
      if (this.disclosureCreatedDTTM > disclosureTrackingBase.disclosureCreatedDTTM)
        return 1;
      return this.disclosureCreatedDTTM == disclosureTrackingBase.disclosureCreatedDTTM ? 0 : -1;
    }

    public override string ToString()
    {
      return "DisclosedBy:" + this.DisclosedByFullName + "; DisclosedDate:" + this.DisclosedDate.ToString("MM/dd/yyyy hh:mm:ss tt") + "; BorrowerName:" + (this.BorrowerName != "" ? this.BorrowerName : this.CoBorrowerName) + "; DisclosedMethod:" + this.DisclosedMethodName + "; LogGuid:" + this.Guid.ToString();
    }

    public DateTime ConvertToLoanTimeZone(DateTime dt)
    {
      if (this.TimeZoneInfo == null)
        return dt;
      if (dt.Kind == DateTimeKind.Local)
        return System.TimeZoneInfo.ConvertTime(dt, System.TimeZoneInfo.Local, this.TimeZoneInfo);
      return dt.Kind == DateTimeKind.Utc ? System.TimeZoneInfo.ConvertTimeFromUtc(dt, this.TimeZoneInfo) : dt;
    }

    public DateTime ConvertToUtc(DateTime dt)
    {
      return dt.Kind == DateTimeKind.Utc || this.TimeZoneInfo == null ? dt : Utils.ConvertTimeToUtc(dt, this.TimeZoneInfo);
    }

    public virtual string DisclosedMethodName
    {
      get
      {
        switch (this.method)
        {
          case DisclosureTrackingBase.DisclosedMethod.ByMail:
            return "U.S. Mail";
          case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
            return "eFolder eDisclosures";
          case DisclosureTrackingBase.DisclosedMethod.Fax:
            return "Fax";
          case DisclosureTrackingBase.DisclosedMethod.InPerson:
            return "In Person";
          case DisclosureTrackingBase.DisclosedMethod.Other:
            return "Other";
          case DisclosureTrackingBase.DisclosedMethod.Email:
            return "Email";
          default:
            return "U.S. Mail";
        }
      }
    }

    public enum DisclosedMethod
    {
      None,
      ByMail,
      eDisclosure,
      Fax,
      InPerson,
      Other,
      Email,
      Phone,
      Signature,
      ClosingDocsOrder,
      eClose,
      OvernightShipping,
    }
  }
}
