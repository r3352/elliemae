// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.EnhancedDisclosureTracking2015Log
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using Elli.ElliEnum;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.TimeZones;
using EllieMae.EMLite.Common.Xml.AutoMapping;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class EnhancedDisclosureTracking2015Log : 
    LogRecordBase,
    IDisclosureTracking2015Log,
    IDisclosureTrackingLog
  {
    private LoanData loanData;
    private DateTime receivedDate = DateTime.MinValue;
    private List<DisclosedLoanItem> loanDataList = new List<DisclosedLoanItem>();
    private Hashtable loanDataUniqueList = new Hashtable();
    private Hashtable loanDataFromOtherLogs = new Hashtable();
    private static ConcurrentDictionary<string, Dictionary<string, string>> cachedDisclosureTrackingLoanSnapShots = new ConcurrentDictionary<string, Dictionary<string, string>>();
    public static readonly string XmlType = "EnhancedDisclosureTracking2015";
    private string ucd = string.Empty;
    private string udt = string.Empty;
    private List<string[]> itemizationFields = new List<string[]>();
    private bool includedInTimeline;
    private DisclosureTracking2015Log.TrackingLogStatus status;
    private DisclosureTrackingBase.DisclosedMethod disclosedMethod;
    private ObservableCollection<EnhancedDisclosureTracking2015Log.FulfillmentFields> fulfillments;
    private DisclosureTracking2015Log.DisclosureTypeEnum disclosureType;
    private string edisclosureConsentPDF = string.Empty;
    private Dictionary<string, INonBorrowerOwnerItem> nonBorrowerOwnerCollections = new Dictionary<string, INonBorrowerOwnerItem>();
    private bool initializingFromXml;

    static EnhancedDisclosureTracking2015Log()
    {
      EnhancedDisclosureTracking2015Log.DisclosureRecipientType result;
      XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log>().ForMember<bool>((Expression<Func<EnhancedDisclosureTracking2015Log, bool>>) (dt => dt.DisplayInLog), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<bool>((Expression<Func<EnhancedDisclosureTracking2015Log, bool>>) (dt => dt.IsRemoved), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<bool>((Expression<Func<EnhancedDisclosureTracking2015Log, bool>>) (dt => dt.IsNew), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<string>((Expression<Func<EnhancedDisclosureTracking2015Log, string>>) (dt => dt.Comments), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<string>>) (opts => opts.Ignore())).ForMember<LogList>((Expression<Func<EnhancedDisclosureTracking2015Log, LogList>>) (dt => dt.Log), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<LogList>>) (opts => opts.Ignore())).ForMember<bool>((Expression<Func<EnhancedDisclosureTracking2015Log, bool>>) (dt => dt.IsAttachedToLog), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<string>((Expression<Func<EnhancedDisclosureTracking2015Log, string>>) (dt => dt.Guid), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<string>>) (opts => opts.Ignore())).ForMember<DateTime>((Expression<Func<EnhancedDisclosureTracking2015Log, DateTime>>) (dt => dt.Date), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<DateTime>>) (opts => opts.Ignore())).ForMember<DateTime>((Expression<Func<EnhancedDisclosureTracking2015Log, DateTime>>) (dt => dt.DateUpdated), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<DateTime>>) (opts => opts.Ignore())).ForMember<bool>((Expression<Func<EnhancedDisclosureTracking2015Log, bool>>) (dt => dt.IsLoanOperationalLog), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<string>((Expression<Func<EnhancedDisclosureTracking2015Log, string>>) (dt => dt.UCD), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<string>>) (opts => opts.Ignore())).ForMember<string>((Expression<Func<EnhancedDisclosureTracking2015Log, string>>) (dt => dt.UDT), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<string>>) (opts => opts.Ignore())).ForMember<EnhancedDisclosureTracking2015Log.IntentToProceedFields>((Expression<Func<EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.IntentToProceedFields>>) (dt => dt.IntentToProceed), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<EnhancedDisclosureTracking2015Log.IntentToProceedFields>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.IntentToProceedFields>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.IntentToProceedFields(parent))))).ForMember<EnhancedDisclosureTracking2015Log.DisclosedDateField>((Expression<Func<EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.DisclosedDateField>>) (dt => dt.DisclosedDate), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<EnhancedDisclosureTracking2015Log.DisclosedDateField>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.DisclosedDateField>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.DisclosedDateField(parent))))).ForMember<EnhancedDisclosureTracking2015Log.LockableUserRefField>((Expression<Func<EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.LockableUserRefField>>) (dt => dt.DisclosedBy), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<EnhancedDisclosureTracking2015Log.LockableUserRefField>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.LockableUserRefField>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.LockableUserRefField(parent))))).ForMember<EnhancedDisclosureTracking2015Log.TrackingFields>((Expression<Func<EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.TrackingFields>>) (dt => dt.Tracking), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<EnhancedDisclosureTracking2015Log.TrackingFields>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.TrackingFields>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.TrackingFields(parent))))).ForCollectionMember<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Expression<Func<EnhancedDisclosureTracking2015Log, IList<EnhancedDisclosureTracking2015Log.DisclosureRecipient>>>) (dt => dt.DisclosureRecipients), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<IList<EnhancedDisclosureTracking2015Log.DisclosureRecipient>, EnhancedDisclosureTracking2015Log.DisclosureRecipient>>) (opts => opts.CreateItemUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.DisclosureRecipient>) ((xmlElement, parent) => xmlElement.HasAttribute("Role") && Enum.TryParse<EnhancedDisclosureTracking2015Log.DisclosureRecipientType>(xmlElement.GetAttribute("Role"), out result) ? EnhancedDisclosureTracking2015Log.DisclosureRecipient.CreateNew(result, parent) : (EnhancedDisclosureTracking2015Log.DisclosureRecipient) null)))).ForCollectionMember<EnhancedDisclosureTracking2015Log.FulfillmentFields>((Expression<Func<EnhancedDisclosureTracking2015Log, IList<EnhancedDisclosureTracking2015Log.FulfillmentFields>>>) (dt => dt.Fulfillments), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<IList<EnhancedDisclosureTracking2015Log.FulfillmentFields>, EnhancedDisclosureTracking2015Log.FulfillmentFields>>) (opts => opts.CreateItemUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.FulfillmentFields>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.FulfillmentFields(parent))))).ForMember<EnhancedDisclosureTracking2015Log.LockableField<string>>((Expression<Func<EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.LockableField<string>>>) (x => x.DisclosedApr), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<EnhancedDisclosureTracking2015Log.LockableField<string>>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.LockableField<string>>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.LockableField<string>(parent))))).ForMember<EnhancedDisclosureTracking2015Log.LockableField<string>>((Expression<Func<EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.LockableField<string>>>) (x => x.DisclosedFinanceCharge), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<EnhancedDisclosureTracking2015Log.LockableField<string>>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.LockableField<string>>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.LockableField<string>(parent))))).ForMember<EnhancedDisclosureTracking2015Log.LockableField<string>>((Expression<Func<EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.LockableField<string>>>) (x => x.DisclosedDailyInterest), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<EnhancedDisclosureTracking2015Log.LockableField<string>>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.LockableField<string>>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.LockableField<string>(parent))))).ForMember<EnhancedDisclosureTracking2015Log.LoanEstimateFields>((Expression<Func<EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.LoanEstimateFields>>) (x => x.LoanEstimate), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<EnhancedDisclosureTracking2015Log.LoanEstimateFields>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.LoanEstimateFields>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.LoanEstimateFields(parent))))).ForMember<EnhancedDisclosureTracking2015Log.ClosingDisclosureFields>((Expression<Func<EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.ClosingDisclosureFields>>) (x => x.ClosingDisclosure), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<EnhancedDisclosureTracking2015Log.ClosingDisclosureFields>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log, EnhancedDisclosureTracking2015Log.ClosingDisclosureFields>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.ClosingDisclosureFields(parent))))).ForMember<bool>((Expression<Func<EnhancedDisclosureTracking2015Log, bool>>) (dt => dt.ChangedCircumstanceIndicator), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<bool>((Expression<Func<EnhancedDisclosureTracking2015Log, bool>>) (dt => dt.FeeLevelDisclosuresIndicator), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<System.TimeZoneInfo>((Expression<Func<EnhancedDisclosureTracking2015Log, System.TimeZoneInfo>>) (dt => dt.TimeZoneInfo), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log>.ProfileOptions<System.TimeZoneInfo>>) (opts => opts.Ignore())));
    }

    public System.TimeZoneInfo TimeZoneInfo { get; private set; }

    protected override bool ReadDateInUtc => true;

    public string Provider { get; set; }

    public string ProviderDescription { get; set; }

    public IList<EnhancedDisclosureTracking2015Log.DisclosureContentType> Contents { get; set; }

    public bool IncludedInTimeline
    {
      get => this.includedInTimeline;
      set
      {
        if (((this.initializingFromXml ? 0 : (this.status == DisclosureTracking2015Log.TrackingLogStatus.Pending ? 1 : 0)) & (value ? 1 : 0)) != 0)
          throw new DataValidationException("IncludedInTimeline cannot be set to true when status is Pending");
        this.includedInTimeline = value;
        if (!this.initializingFromXml && this.IsAttachedToLog && this.Log.Loan.Calculator != null)
        {
          if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE) || this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD))
            this.Log.Loan.Calculator.UpdateDisclosureTypeForTimeline((IDisclosureTracking2015Log) this);
          this.Log.Loan.Calculator.CalculateLastDisclosedCDorLE((IDisclosureTracking2015Log) this);
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
          this.Log.Loan.Calculator.UpdateLogs();
        }
        this.MarkAsDirty();
      }
    }

    public DisclosureTracking2015Log.TrackingLogStatus Status
    {
      get => this.status;
      set
      {
        if (value == DisclosureTracking2015Log.TrackingLogStatus.Pending && this.status == DisclosureTracking2015Log.TrackingLogStatus.Active)
          this.includedInTimeline = false;
        this.status = value;
      }
    }

    public EnhancedDisclosureTracking2015Log.LockableUserRefField DisclosedBy { get; set; }

    public DisclosureTrackingBase.DisclosedMethod DisclosedMethod
    {
      get => this.disclosedMethod;
      set
      {
        this.disclosedMethod = value;
        this.CalculateLatestDisclosure2015();
        this.MarkAsDirty();
      }
    }

    public string DisclosedMethodDescription { get; set; }

    public DateTime DisclosureCreatedDate { get; set; }

    public EnhancedDisclosureTracking2015Log.TrackingFields Tracking { get; set; }

    public IList<EnhancedDisclosureTracking2015Log.FulfillmentFields> Fulfillments
    {
      get => (IList<EnhancedDisclosureTracking2015Log.FulfillmentFields>) this.fulfillments;
      set
      {
        if (this.fulfillments != null)
          this.fulfillments.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.FulfillmentFieldsChangedEventHandler);
        if (value == null)
        {
          this.fulfillments = (ObservableCollection<EnhancedDisclosureTracking2015Log.FulfillmentFields>) null;
        }
        else
        {
          this.fulfillments = new ObservableCollection<EnhancedDisclosureTracking2015Log.FulfillmentFields>();
          this.fulfillments.CollectionChanged += new NotifyCollectionChangedEventHandler(this.FulfillmentFieldsChangedEventHandler);
          foreach (EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillmentFields in (IEnumerable<EnhancedDisclosureTracking2015Log.FulfillmentFields>) value)
            this.fulfillments.Add(fulfillmentFields);
        }
      }
    }

    public EnhancedDisclosureTracking2015Log.DisclosureTrackingDocuments Documents { get; set; }

    public DisclosureTracking2015Log.DisclosureTypeEnum DisclosureType
    {
      get => this.disclosureType;
      set
      {
        this.disclosureType = value;
        this.CalculateLatestDisclosure2015();
        this.MarkAsDirty();
      }
    }

    public void AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum type)
    {
      this.disclosureType = type;
      this.MarkAsDirty();
    }

    public EnhancedDisclosureTracking2015Log.LoanEstimateFields LoanEstimate { get; set; }

    public EnhancedDisclosureTracking2015Log.ClosingDisclosureFields ClosingDisclosure { get; set; }

    public string ChangeInCircumstance { get; set; }

    public string ChangeInCircumstanceComments { get; set; }

    public bool ChangedCircumstanceIndicator
    {
      get => Utils.ParseBoolean((object) this.GetAttribute("XCOCChangedCircumstances"));
      set => this.SetAttribute("XCOCChangedCircumstances", value ? "Y" : "N");
    }

    public bool FeeLevelDisclosuresIndicator
    {
      get => Utils.ParseBoolean((object) this.GetAttribute("XCOCFeeLevelIndicator"));
    }

    public EnhancedDisclosureTracking2015Log.IntentToProceedFields IntentToProceed { get; set; }

    public IList<EnhancedDisclosureTracking2015Log.DisclosureRecipient> DisclosureRecipients { get; set; }

    public DateTime ApplicationDate { get; set; }

    public EnhancedDisclosureTracking2015Log.LockableField<string> DisclosedApr { get; set; }

    public EnhancedDisclosureTracking2015Log.LockableField<string> DisclosedFinanceCharge { get; set; }

    public string LoanProgram { get; set; }

    public Decimal LoanAmount { get; set; }

    public EnhancedDisclosureTracking2015Log.Address PropertyAddress { get; set; }

    public EnhancedDisclosureTracking2015Log.DisclosedDateField DisclosedDate { get; set; }

    public EnhancedDisclosureTracking2015Log.LockableField<string> DisclosedDailyInterest { get; set; }

    public string LinkedGuid { get; set; }

    public Dictionary<string, string> Attributes { get; set; }

    public string UCD
    {
      get => this.ucd;
      set => this.ucd = value;
    }

    public string UDT
    {
      get => this.udt;
      set => this.udt = value;
    }

    private EnhancedDisclosureTracking2015Log.BorrowerRecipient GetFirstBorrowerDisclosureRecipient()
    {
      return this.DisclosureRecipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (rec => rec is EnhancedDisclosureTracking2015Log.BorrowerRecipient)) as EnhancedDisclosureTracking2015Log.BorrowerRecipient;
    }

    private EnhancedDisclosureTracking2015Log.CoborrowerRecipient GetFirstCoborrowerDisclosureRecipient()
    {
      return this.DisclosureRecipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (rec => rec is EnhancedDisclosureTracking2015Log.CoborrowerRecipient)) as EnhancedDisclosureTracking2015Log.CoborrowerRecipient;
    }

    private EnhancedDisclosureTracking2015Log.DisclosureRecipient GetRecipientByID(string Id)
    {
      return this.DisclosureRecipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (rec => rec.Id == Id));
    }

    private EnhancedDisclosureTracking2015Log.DisclosureRecipient GetFirstLODisclosureRecipient(
      bool createIfMissing)
    {
      if (createIfMissing && !this.DisclosureRecipients.Any<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (rec => rec.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate && "Originator".Equals(rec.RoleDescription, StringComparison.OrdinalIgnoreCase))))
        this.DisclosureRecipients.Add(new EnhancedDisclosureTracking2015Log.DisclosureRecipient(this, EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate)
        {
          Id = System.Guid.NewGuid().ToString(),
          RoleDescription = "Originator"
        });
      return this.DisclosureRecipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (rec => rec.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate && "Originator".Equals(rec.RoleDescription, StringComparison.OrdinalIgnoreCase)));
    }

    DisclosedLoanItem[] IDisclosureTracking2015Log.DisclosedData => this.loanDataList.ToArray();

    string IDisclosureTracking2015Log.eDisclosureLOUserId
    {
      get => this.GetFirstLODisclosureRecipient(false)?.Id;
      set => this.GetFirstLODisclosureRecipient(true).Id = value;
    }

    string IDisclosureTracking2015Log.eDisclosureLOeSignedIP
    {
      get => this.GetFirstLODisclosureRecipient(false)?.Tracking.ESignedIP ?? string.Empty;
      set => this.GetFirstLODisclosureRecipient(true).Tracking.ESignedIP = value;
    }

    DateTime IDisclosureTracking2015Log.eDisclosureLOViewMessageDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient = this.GetFirstLODisclosureRecipient(false);
        return disclosureRecipient == null ? DateTime.MinValue : disclosureRecipient.Tracking.ViewMessageDate.DateTime;
      }
      set
      {
        this.GetFirstLODisclosureRecipient(true).Tracking.ViewMessageDate = this.CreateDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureLOeSignedDate
    {
      get
      {
        return this.GetFirstLODisclosureRecipient(false) != null ? this.GetFirstLODisclosureRecipient(false).Tracking.ESignedDate.DateTime : DateTime.MinValue;
      }
      set
      {
        this.GetFirstLODisclosureRecipient(true).Tracking.ESignedDate = this.CreateDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureLOInformationalViewedDate
    {
      get
      {
        return this.GetFirstLODisclosureRecipient(false) != null ? this.GetFirstLODisclosureRecipient(false).Tracking.InformationalViewedDate.DateTime : DateTime.MinValue;
      }
      set
      {
        this.GetFirstLODisclosureRecipient(true).Tracking.InformationalViewedDate = this.CreateDateTimeWithZone(value);
      }
    }

    string IDisclosureTracking2015Log.eDisclosureLOInformationalViewedIP
    {
      get
      {
        return this.GetFirstLODisclosureRecipient(false)?.Tracking.InformationalViewedIP ?? string.Empty;
      }
      set => this.GetFirstLODisclosureRecipient(true).Tracking.InformationalViewedIP = value;
    }

    DateTime IDisclosureTracking2015Log.eDisclosureLOInformationalCompletedDate
    {
      get
      {
        return this.GetFirstLODisclosureRecipient(false) != null ? this.GetFirstLODisclosureRecipient(false).Tracking.InformationalCompletedDate.DateTime : DateTime.MinValue;
      }
      set
      {
        this.GetFirstLODisclosureRecipient(true).Tracking.InformationalCompletedDate = this.CreateDateTimeWithZone(value);
      }
    }

    string IDisclosureTracking2015Log.eDisclosureLOInformationalCompletedIP
    {
      get
      {
        return this.GetFirstLODisclosureRecipient(false)?.Tracking.InformationalCompletedIP ?? string.Empty;
      }
      set => this.GetFirstLODisclosureRecipient(true).Tracking.InformationalCompletedIP = value;
    }

    string IDisclosureTracking2015Log.Guid => this.Guid;

    bool IDisclosureTracking2015Log.IsIntentReceivedByLocked
    {
      get => this.IntentToProceed.ReceivedBy.UseUserValue;
      set => this.IntentToProceed.ReceivedBy.UseUserValue = value;
    }

    string IDisclosureTracking2015Log.LockedIntentReceivedByField
    {
      get => this.IntentToProceed.ReceivedBy.UserValue;
      set => this.IntentToProceed.ReceivedBy.UserValue = value;
    }

    bool IDisclosureTracking2015Log.IntentToProceed
    {
      get => this.IntentToProceed.Intent;
      set => this.IntentToProceed.Intent = value;
    }

    DateTime IDisclosureTracking2015Log.IntentToProceedDate
    {
      get => this.IntentToProceed.Date;
      set => this.IntentToProceed.Date = value;
    }

    string IDisclosureTracking2015Log.IntentToProceedReceivedBy
    {
      get => this.IntentToProceed.ReceivedBy.ComputedValue;
      set => this.IntentToProceed.ReceivedBy.ComputedValue = value;
    }

    DisclosureTrackingBase.DisclosedMethod IDisclosureTracking2015Log.IntentToProceedReceivedMethod
    {
      get => this.IntentToProceed.ReceivedMethod;
      set => this.IntentToProceed.ReceivedMethod = value;
    }

    string IDisclosureTracking2015Log.IntentToProceedReceivedMethodOther
    {
      get => this.IntentToProceed.ReceivedMethodOther;
      set => this.IntentToProceed.ReceivedMethodOther = value;
    }

    string IDisclosureTracking2015Log.IntentToProceedComments
    {
      get => this.IntentToProceed.Comments;
      set => this.IntentToProceed.Comments = value;
    }

    bool IDisclosureTracking2015Log.LEDisclosedByBroker => this.LoanEstimate.IsDisclosedByBroker;

    bool IDisclosureTracking2015Log.IsLocked
    {
      get => this.DisclosedDate.UseUserValue;
      set => this.DisclosedDate.UseUserValue = value;
    }

    bool IDisclosureTracking2015Log.IsDisclosed
    {
      get => this.IncludedInTimeline;
      set => this.IncludedInTimeline = value;
    }

    bool IDisclosureTracking2015Log.UCDCreationError { get; set; }

    string IDisclosureTracking2015Log.DisclosureTypeName
    {
      get
      {
        switch (this.DisclosureType)
        {
          case DisclosureTracking2015Log.DisclosureTypeEnum.Initial:
            return "Initial";
          case DisclosureTracking2015Log.DisclosureTypeEnum.Revised:
            return "Revised";
          case DisclosureTracking2015Log.DisclosureTypeEnum.Final:
            return "Final";
          case DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation:
            return "Post Consummation";
          default:
            return "Initial";
        }
      }
    }

    LoanData IDisclosureTracking2015Log.LogLoanData => this.loanData;

    int IDisclosureTracking2015Log.NumOfDisclosedDocs
    {
      get
      {
        return this.Documents != null && this.Documents.Forms != null ? this.Documents.Forms.Count : 0;
      }
    }

    void IDisclosureTracking2015Log.RemoveShapsnot()
    {
      this.loanDataList.Clear();
      this.loanDataUniqueList.Clear();
    }

    bool IDisclosureTrackingLog.IsFieldLocked(string fieldId)
    {
      foreach (DisclosedLoanItem loanData in this.loanDataList)
      {
        if (string.Compare(loanData.FieldID, fieldId + "_LOCKED", true) == 0)
          return true;
      }
      return false;
    }

    DateTime IDisclosureTracking2015Log.ReceivedDate { get; set; }

    bool IDisclosureTracking2015Log.IsDisclosedReceivedDateLocked { get; set; }

    bool IDisclosureTracking2015Log.Received => this.receivedDate != DateTime.MinValue;

    DateTime IDisclosureTracking2015Log.DisclosedDate
    {
      get
      {
        return !this.DisclosedDate.UseUserValue ? this.DisclosedDate.ComputedValue.DateTime.Date : this.DisclosedDate.UserValue;
      }
      set
      {
        if (this.DisclosedDate.UseUserValue)
          this.DisclosedDate.UserValue = value;
        else
          this.date = value;
        this.CalculateLatestDisclosure2015();
        this.MarkAsDirty();
      }
    }

    DateTime IDisclosureTracking2015Log.DisclosedDateTime
    {
      get
      {
        return !this.DisclosedDate.UseUserValue ? this.DisclosedDate.ComputedValue.DateTime : this.DisclosedDate.UserValue;
      }
    }

    DateTime IDisclosureTracking2015Log.DateAdded => this.DisclosureCreatedDate;

    bool IDisclosureTracking2015Log.IsDisclosedByLocked
    {
      get => this.DisclosedBy.UseUserValue;
      set => this.DisclosedBy.UseUserValue = value;
    }

    string IDisclosureTracking2015Log.LockedDisclosedByField
    {
      get => this.DisclosedBy.UserValue;
      set => this.DisclosedBy.UserValue = value;
    }

    string IDisclosureTracking2015Log.DisclosedByFullName
    {
      get => this.DisclosedBy.ComputedName;
      set => this.DisclosedBy.ComputedName = value;
    }

    string IDisclosureTracking2015Log.DisclosedBy
    {
      get => this.DisclosedBy.ComputedValue;
      set => this.DisclosedBy.ComputedValue = value;
    }

    DisclosureTrackingBase.DisclosedMethod IDisclosureTracking2015Log.DisclosureMethod
    {
      get => this.DisclosedMethod;
      set => this.DisclosedMethod = value;
    }

    string IDisclosureTracking2015Log.DisclosedMethodOther
    {
      get => this.DisclosedMethodDescription;
      set => this.DisclosedMethodDescription = value;
    }

    public bool UseForUCDExport { get; set; }

    string IDisclosureTracking2015Log.DisclosedMethodName
    {
      get => DisclosureTracking2015Log.GetDisclosedMethodName(this.DisclosedMethod);
    }

    bool IDisclosureTracking2015Log.DisclosedForLE
    {
      get => this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE);
    }

    bool IDisclosureTracking2015Log.DisclosedForCD
    {
      get => this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD);
    }

    bool IDisclosureTracking2015Log.DisclosedForSafeHarbor
    {
      get
      {
        return this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.SafeHarbor);
      }
    }

    bool IDisclosureTracking2015Log.ProviderListSent
    {
      get
      {
        return this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.ServiceProviderList);
      }
    }

    bool IDisclosureTracking2015Log.ProviderListNoFeeSent
    {
      get
      {
        return this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.ServiceProviderListNoFee);
      }
    }

    bool IDisclosureTracking2015Log.IsNboExist
    {
      get
      {
        return this.DisclosureRecipients.Any<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (rec => rec.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner));
      }
    }

    string IDisclosureTrackingLog.BorrowerPairID
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.BorrowerPairId;
    }

    DisclosureTracking2015Log.DisclosureTypeEnum IDisclosureTracking2015Log.DisclosureType
    {
      get => this.DisclosureType;
      set => this.DisclosureType = value;
    }

    string IDisclosureTracking2015Log.DisclosedAPR
    {
      get
      {
        return !this.DisclosedApr.UseUserValue ? this.DisclosedApr.ComputedValue : this.DisclosedApr.UserValue;
      }
      set
      {
        if (this.DisclosedApr.UseUserValue)
          this.DisclosedApr.UserValue = value;
        else
          this.DisclosedApr.ComputedValue = value;
      }
    }

    bool IDisclosureTracking2015Log.IsDisclosedAPRLocked
    {
      get => this.DisclosedApr.UseUserValue;
      set => this.DisclosedApr.UseUserValue = value;
    }

    string IDisclosureTracking2015Log.LinkedGuid
    {
      get => this.LinkedGuid;
      set => this.LinkedGuid = value;
    }

    string IDisclosureTracking2015Log.FinanceCharge
    {
      get
      {
        return !this.DisclosedFinanceCharge.UseUserValue ? this.DisclosedFinanceCharge.ComputedValue : this.DisclosedFinanceCharge.UserValue;
      }
      set
      {
        if (this.DisclosedFinanceCharge.UseUserValue)
          this.DisclosedFinanceCharge.UserValue = value;
        else
          this.DisclosedFinanceCharge.ComputedValue = value;
      }
    }

    string IDisclosureTracking2015Log.DisclosedDailyInterest
    {
      get
      {
        return !this.DisclosedDailyInterest.UseUserValue ? this.DisclosedDailyInterest.ComputedValue : this.DisclosedDailyInterest.UserValue;
      }
      set
      {
        if (this.DisclosedDailyInterest.UseUserValue)
          this.DisclosedDailyInterest.UserValue = value;
        else
          this.DisclosedDailyInterest.ComputedValue = value;
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosurePackageCreatedDate
    {
      get => this.Tracking.PackageCreatedDate.DateTime;
      set => this.Tracking.PackageCreatedDate = this.CreateDateTimeWithZone(value);
    }

    string IDisclosureTracking2015Log.BorrowerName
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.Name ?? string.Empty;
    }

    bool IDisclosureTrackingLog.eDisclosureApplicationPackage
    {
      get
      {
        return this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ApplicationPackage);
      }
      set
      {
        if (value)
        {
          if (this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ApplicationPackage))
            return;
          this.Tracking.Indicators.Add(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ApplicationPackage);
        }
        else
        {
          if (!this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ApplicationPackage))
            return;
          this.Tracking.Indicators.Remove(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ApplicationPackage);
        }
      }
    }

    bool IDisclosureTrackingLog.eDisclosureThreeDayPackage
    {
      get
      {
        return this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ThreeDayPackage);
      }
      set
      {
        if (value)
        {
          if (this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ThreeDayPackage))
            return;
          this.Tracking.Indicators.Add(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ThreeDayPackage);
        }
        else
        {
          if (!this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ThreeDayPackage))
            return;
          this.Tracking.Indicators.Remove(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ThreeDayPackage);
        }
      }
    }

    bool IDisclosureTrackingLog.eDisclosureLockPackage
    {
      get
      {
        return this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.LockPackage);
      }
      set
      {
        if (value)
        {
          if (this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.LockPackage))
            return;
          this.Tracking.Indicators.Add(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.LockPackage);
        }
        else
        {
          if (!this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.LockPackage))
            return;
          this.Tracking.Indicators.Remove(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.LockPackage);
        }
      }
    }

    bool IDisclosureTrackingLog.eDisclosureApprovalPackage
    {
      get
      {
        return this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ApprovalPackage);
      }
      set
      {
        if (value)
        {
          if (this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ApprovalPackage))
            return;
          this.Tracking.Indicators.Add(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ApprovalPackage);
        }
        else
        {
          if (!this.Tracking.Indicators.Contains(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ApprovalPackage))
            return;
          this.Tracking.Indicators.Remove(EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators.ApprovalPackage);
        }
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureBorrowerViewMessageDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.ViewMessageDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.ViewMessageDate = this.CreateDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureBorrowereSignedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.ESignedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.ESignedDate = this.CreateDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureBorrowerAcceptConsentDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.AcceptConsentDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.AcceptConsentDate = this.CreateDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureBorrowerInformationalViewedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.InformationalViewedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.InformationalViewedDate = this.CreateDateTimeWithZone(value);
      }
    }

    string IDisclosureTracking2015Log.eDisclosureBorrowerInformationalViewedIP
    {
      get
      {
        return this.GetFirstBorrowerDisclosureRecipient()?.Tracking.InformationalViewedIP ?? string.Empty;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.InformationalViewedIP = value;
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureBorrowerInformationalCompletedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.InformationalCompletedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.InformationalCompletedDate = this.CreateDateTimeWithZone(value);
      }
    }

    string IDisclosureTracking2015Log.eDisclosureBorrowerInformationalCompletedIP
    {
      get
      {
        return this.GetFirstBorrowerDisclosureRecipient()?.Tracking.InformationalCompletedIP ?? string.Empty;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.InformationalCompletedIP = value;
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureBorrowerRejectConsentDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.RejectConsentDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.RejectConsentDate = this.CreateDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureBorrowerAuthenticatedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.AuthenticatedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.AuthenticatedDate = this.CreateDateTimeWithZone(value);
      }
    }

    string IDisclosureTracking2015Log.eDisclosureBorrowerEmail
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.Email ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Email = value;
      }
    }

    string IDisclosureTracking2015Log.eDisclosureBorrowerAcceptConsentIP
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.Tracking.AcceptConsentIP ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.AcceptConsentIP = value;
      }
    }

    string IDisclosureTracking2015Log.eDisclosureBorrowerRejectConsentIP
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.Tracking.RejectConsentIP ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.RejectConsentIP = value;
      }
    }

    string IDisclosureTracking2015Log.eDisclosureBorrowereSignedIP
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.Tracking.ESignedIP ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.ESignedIP = value;
      }
    }

    string IDisclosureTracking2015Log.eDisclosureBorrowerAuthenticatedIP
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.Tracking.AuthenticatedIP ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.AuthenticatedIP = value;
      }
    }

    string IDisclosureTracking2015Log.Comments
    {
      get => this.comments;
      set => this.comments = value;
    }

    DisclosureTrackingBase.DisclosedMethod IDisclosureTracking2015Log.BorrowerDisclosedMethod
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? DisclosureTrackingBase.DisclosedMethod.None : disclosureRecipient.DisclosedMethod;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.DisclosedMethod = value;
      }
    }

    string IDisclosureTracking2015Log.BorrowerDisclosedMethodOther
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.DisclosedMethodDescription ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.DisclosedMethodDescription = value;
      }
    }

    string IDisclosureTracking2015Log.CoBorrowerDisclosedMethodOther
    {
      get
      {
        return this.GetFirstCoborrowerDisclosureRecipient()?.DisclosedMethodDescription ?? string.Empty;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.DisclosedMethodDescription = value;
      }
    }

    bool IDisclosureTracking2015Log.IsBorrowerPresumedDateLocked
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient != null && disclosureRecipient.PresumedReceivedDate.UseUserValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.PresumedReceivedDate.UseUserValue = value;
      }
    }

    DateTime IDisclosureTracking2015Log.BorrowerPresumedReceivedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.PresumedReceivedDate.ComputedValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.PresumedReceivedDate.ComputedValue = value;
      }
    }

    DateTime IDisclosureTracking2015Log.LockedBorrowerPresumedReceivedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.PresumedReceivedDate.UserValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.PresumedReceivedDate.UserValue = value;
      }
    }

    DateTime IDisclosureTracking2015Log.BorrowerActualReceivedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.ActualReceivedDate;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.ActualReceivedDate = value;
      }
    }

    bool IDisclosureTracking2015Log.IsBorrowerTypeLocked
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient != null && disclosureRecipient.BorrowerType.UseUserValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.BorrowerType.UseUserValue = value;
      }
    }

    string IDisclosureTracking2015Log.BorrowerType
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.BorrowerType.ComputedValue;
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.BorrowerType.ComputedValue = value;
      }
    }

    string IDisclosureTracking2015Log.LockedBorrowerType
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.BorrowerType.UserValue;
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.BorrowerType.UserValue = value;
      }
    }

    bool IDisclosureTracking2015Log.IsWetSigned
    {
      get
      {
        return this.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.WetSignature;
      }
      set
      {
      }
    }

    string IDisclosureTracking2015Log.EDisclosureBorrowerLoanLevelConsent
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.Tracking.LoanLevelConsent;
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.LoanLevelConsent = value;
      }
    }

    DateTime IDisclosureTracking2015Log.EDisclosureBorrowerDocumentViewedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return new DateTime();
        if (this.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.WetSignature)
          return disclosureRecipient.Tracking.ViewWetSignedDate.DateTime;
        if (this.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.eSignature)
          return disclosureRecipient.Tracking.ViewESignedDate.DateTime;
        return this.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.Informational ? disclosureRecipient.Tracking.InformationalViewedDate.DateTime : new DateTime();
      }
    }

    string IDisclosureTracking2015Log.eDisclosureBorrowerName
    {
      get => this.GetFirstBorrowerDisclosureRecipient()?.Name;
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Name = value;
      }
    }

    string IDisclosureTracking2015Log.CoBorrowerName
    {
      get => this.GetFirstCoborrowerDisclosureRecipient()?.Name ?? string.Empty;
    }

    DisclosureTrackingBase.DisclosedMethod IDisclosureTracking2015Log.CoBorrowerDisclosedMethod
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? DisclosureTrackingBase.DisclosedMethod.None : disclosureRecipient.DisclosedMethod;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.DisclosedMethod = value;
      }
    }

    bool IDisclosureTracking2015Log.IsCoBorrowerPresumedDateLocked
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient != null && disclosureRecipient.PresumedReceivedDate.UseUserValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.PresumedReceivedDate.UseUserValue = value;
      }
    }

    DateTime IDisclosureTracking2015Log.CoBorrowerPresumedReceivedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.PresumedReceivedDate.ComputedValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.PresumedReceivedDate.ComputedValue = value;
      }
    }

    DateTime IDisclosureTracking2015Log.LockedCoBorrowerPresumedReceivedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.PresumedReceivedDate.UserValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.PresumedReceivedDate.UserValue = value;
      }
    }

    DateTime IDisclosureTracking2015Log.CoBorrowerActualReceivedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.ActualReceivedDate;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.ActualReceivedDate = value;
      }
    }

    bool IDisclosureTracking2015Log.IsCoBorrowerTypeLocked
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient != null && disclosureRecipient.BorrowerType.UseUserValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.BorrowerType.UseUserValue = value;
      }
    }

    string IDisclosureTracking2015Log.CoBorrowerType
    {
      get => this.GetFirstCoborrowerDisclosureRecipient()?.BorrowerType.ComputedValue;
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.BorrowerType.ComputedValue = value;
      }
    }

    string IDisclosureTracking2015Log.LockedCoBorrowerType
    {
      get => this.GetFirstCoborrowerDisclosureRecipient()?.BorrowerType.UserValue;
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.BorrowerType.UserValue = value;
      }
    }

    string IDisclosureTracking2015Log.EDisclosureCoBorrowerLoanLevelConsent
    {
      get => this.GetFirstCoborrowerDisclosureRecipient()?.Tracking.LoanLevelConsent;
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.LoanLevelConsent = value;
      }
    }

    DateTime IDisclosureTracking2015Log.EDisclosureCoborrowerDocumentViewedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return new DateTime();
        if (this.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.WetSignature)
          return disclosureRecipient.Tracking.ViewWetSignedDate.DateTime;
        if (this.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.eSignature)
          return disclosureRecipient.Tracking.ViewESignedDate.DateTime;
        return this.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.Informational ? disclosureRecipient.Tracking.InformationalViewedDate.DateTime : new DateTime();
      }
    }

    string IDisclosureTracking2015Log.eDisclosureCoBorrowerName
    {
      get => this.GetFirstCoborrowerDisclosureRecipient()?.Name;
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Name = value;
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureCoBorrowerViewMessageDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.ViewMessageDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.ViewMessageDate = this.ConvertToDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureCoBorrowereSignedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.ESignedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.ESignedDate = this.ConvertToDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureCoBorrowerInformationalViewedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.InformationalViewedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.InformationalViewedDate = this.CreateDateTimeWithZone(value);
      }
    }

    string IDisclosureTracking2015Log.eDisclosureCoBorrowerInformationalViewedIP
    {
      get
      {
        return this.GetFirstCoborrowerDisclosureRecipient()?.Tracking.InformationalViewedIP ?? string.Empty;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.InformationalViewedIP = value;
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureCoBorrowerInformationalCompletedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.InformationalCompletedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.InformationalCompletedDate = this.CreateDateTimeWithZone(value);
      }
    }

    string IDisclosureTracking2015Log.eDisclosureCoBorrowerInformationalCompletedIP
    {
      get
      {
        return this.GetFirstCoborrowerDisclosureRecipient()?.Tracking.InformationalCompletedIP ?? string.Empty;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.InformationalCompletedIP = value;
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureCoBorrowerAcceptConsentDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.AcceptConsentDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.AcceptConsentDate = this.ConvertToDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureCoBorrowerRejectConsentDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.RejectConsentDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.RejectConsentDate = this.ConvertToDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.eDisclosureCoBorrowerAuthenticatedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? new DateTime() : disclosureRecipient.Tracking.AuthenticatedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.AuthenticatedDate = this.ConvertToDateTimeWithZone(value);
      }
    }

    string IDisclosureTracking2015Log.eDisclosureCoBorrowerEmail
    {
      get => this.GetFirstCoborrowerDisclosureRecipient()?.Email ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Email = value;
      }
    }

    string IDisclosureTracking2015Log.eDisclosureCoBorrowerAcceptConsentIP
    {
      get => this.GetFirstCoborrowerDisclosureRecipient()?.Tracking.AcceptConsentIP ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.AcceptConsentIP = value;
      }
    }

    string IDisclosureTracking2015Log.eDisclosureCoBorrowerRejectConsentIP
    {
      get => this.GetFirstCoborrowerDisclosureRecipient()?.Tracking.RejectConsentIP ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.RejectConsentIP = value;
      }
    }

    string IDisclosureTracking2015Log.eDisclosureCoBorrowereSignedIP
    {
      get => this.GetFirstCoborrowerDisclosureRecipient()?.Tracking.ESignedIP ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.ESignedIP = value;
      }
    }

    string IDisclosureTracking2015Log.eDisclosureCoBorrowerAuthenticatedIP
    {
      get => this.GetFirstCoborrowerDisclosureRecipient()?.Tracking.AuthenticatedIP ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.Tracking.AuthenticatedIP = value;
      }
    }

    string IDisclosureTrackingLog.GetDisclosedField(string fieldId)
    {
      return this.GetDisclosedField(fieldId);
    }

    string IDisclosureTrackingLog.eDisclosureDisclosedMessage => string.Empty;

    string IDisclosureTracking2015Log.eDisclosurePackageID
    {
      get => this.Tracking.PackageId;
      set => this.Tracking.PackageId = value;
    }

    bool IDisclosureTracking2015Log.IsLoanDataListExist => this.loanDataList.Count != 0;

    Dictionary<string, string> IDisclosureTracking2015Log.AttributeList => this.Attributes;

    string IDisclosureTracking2015Log.eDisclosureLOName
    {
      get
      {
        EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient = this.GetFirstLODisclosureRecipient(false);
        return disclosureRecipient != null ? disclosureRecipient.Name : string.Empty;
      }
      set => this.GetFirstLODisclosureRecipient(true).Name = value;
    }

    DateTime IDisclosureTracking2015Log.LockedDisclosedDateField
    {
      get => this.DisclosedDate.UserValue;
      set => this.DisclosedDate.UserValue = value;
    }

    DateTime IDisclosureTracking2015Log.OriginalDisclosedDate
    {
      get => this.DisclosedDate.ComputedValue.DateTime;
      set => this.DisclosedDate.ComputedValue = this.ConvertToDateTimeWithZone(value);
    }

    string IDisclosureTracking2015Log.eDisclosureConsentPDF
    {
      get => this.edisclosureConsentPDF;
      set => this.edisclosureConsentPDF = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIsChangeInAPR
    {
      get => this.ClosingDisclosure.IsChangeInAPR;
      set => this.ClosingDisclosure.IsChangeInAPR = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIsChangeInLoanProduct
    {
      get => this.ClosingDisclosure.IsChangeInLoanProduct;
      set => this.ClosingDisclosure.IsChangeInLoanProduct = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIsPrepaymentPenaltyAdded
    {
      get => this.ClosingDisclosure.IsPrepaymentPenaltyAdded;
      set => this.ClosingDisclosure.IsPrepaymentPenaltyAdded = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIsChangeInSettlementCharges
    {
      get => this.ClosingDisclosure.IsChangeInSettlementCharges;
      set => this.ClosingDisclosure.IsChangeInSettlementCharges = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIs24HourAdvancePreview
    {
      get => this.ClosingDisclosure.Is24HourAdvancePreview;
      set => this.ClosingDisclosure.Is24HourAdvancePreview = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIsToleranceCure
    {
      get => this.ClosingDisclosure.IsToleranceCure;
      set => this.ClosingDisclosure.IsToleranceCure = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIsClericalErrorCorrection
    {
      get => this.ClosingDisclosure.IsClericalErrorCorrection;
      set => this.ClosingDisclosure.IsClericalErrorCorrection = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIsChangedCircumstanceEligibility
    {
      get => this.ClosingDisclosure.IsChangedCircumstanceEligibility;
      set => this.ClosingDisclosure.IsChangedCircumstanceEligibility = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIsRevisionsRequestedByConsumer
    {
      get => this.ClosingDisclosure.IsRevisionsRequestedByConsumer;
      set => this.ClosingDisclosure.IsRevisionsRequestedByConsumer = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIsInterestRateDependentCharges
    {
      get => this.ClosingDisclosure.IsInterestRateDependentCharges;
      set => this.ClosingDisclosure.IsInterestRateDependentCharges = value;
    }

    bool IDisclosureTracking2015Log.CDReasonIsOther
    {
      get => this.ClosingDisclosure.IsOther;
      set => this.ClosingDisclosure.IsOther = value;
    }

    bool IDisclosureTracking2015Log.LEReasonIsChangedCircumstanceSettlementCharges
    {
      get => this.LoanEstimate.IsChangedCircumstanceSettlementCharges;
      set => this.LoanEstimate.IsChangedCircumstanceSettlementCharges = value;
    }

    bool IDisclosureTracking2015Log.LEReasonIsChangedCircumstanceEligibility
    {
      get => this.LoanEstimate.IsChangedCircumstanceEligibility;
      set => this.LoanEstimate.IsChangedCircumstanceEligibility = value;
    }

    bool IDisclosureTracking2015Log.LEReasonIsRevisionsRequestedByConsumer
    {
      get => this.LoanEstimate.IsRevisionsRequestedByConsumer;
      set => this.LoanEstimate.IsRevisionsRequestedByConsumer = value;
    }

    bool IDisclosureTracking2015Log.LEReasonIsInterestRateDependentCharges
    {
      get => this.LoanEstimate.IsInterestRateDependentCharges;
      set => this.LoanEstimate.IsInterestRateDependentCharges = value;
    }

    bool IDisclosureTracking2015Log.LEReasonIsExpiration
    {
      get => this.LoanEstimate.IsExpiration;
      set => this.LoanEstimate.IsExpiration = value;
    }

    bool IDisclosureTracking2015Log.LEReasonIsDelayedSettlementOnConstructionLoans
    {
      get => this.LoanEstimate.IsDelayedSettlementOnConstructionLoans;
      set => this.LoanEstimate.IsDelayedSettlementOnConstructionLoans = value;
    }

    bool IDisclosureTracking2015Log.LEReasonIsOther
    {
      get => this.LoanEstimate.IsOther;
      set => this.LoanEstimate.IsOther = value;
    }

    bool IDisclosureTracking2015Log.LEReasonAnyChecked
    {
      get
      {
        IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) this;
        return disclosureTracking2015Log.LEReasonIsInterestRateDependentCharges || disclosureTracking2015Log.LEReasonIsChangedCircumstanceSettlementCharges || disclosureTracking2015Log.LEReasonIsChangedCircumstanceEligibility || disclosureTracking2015Log.LEReasonIsRevisionsRequestedByConsumer || disclosureTracking2015Log.LEReasonIsExpiration || disclosureTracking2015Log.LEReasonIsDelayedSettlementOnConstructionLoans;
      }
    }

    int IDisclosureTracking2015Log.NumberOfGoodFaithChangeOfCircumstance
    {
      get => Utils.ParseInt((object) this.GetDisclosedField("XCOCcount"), 0);
    }

    private EnhancedDisclosureTracking2015Log.FulfillmentFields GetFirstFulfillment(bool? isManual = null)
    {
      return this.Fulfillments.FirstOrDefault<EnhancedDisclosureTracking2015Log.FulfillmentFields>((Func<EnhancedDisclosureTracking2015Log.FulfillmentFields, bool>) (x =>
      {
        if (!isManual.HasValue)
          return true;
        int num1 = x.IsManual ? 1 : 0;
        bool? nullable = isManual;
        int num2 = nullable.GetValueOrDefault() ? 1 : 0;
        return num1 == num2 & nullable.HasValue;
      }));
    }

    private EnhancedDisclosureTracking2015Log.FulfillmentFields GetFirstFulfillmentForRecipient(
      string recipientId,
      bool? isManual = null)
    {
      if (string.IsNullOrWhiteSpace(recipientId))
        return (EnhancedDisclosureTracking2015Log.FulfillmentFields) null;
      EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment(isManual);
      if (firstFulfillment == null)
        return (EnhancedDisclosureTracking2015Log.FulfillmentFields) null;
      return !firstFulfillment.Recipients.Any<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>((Func<EnhancedDisclosureTracking2015Log.FulfillmentRecipient, bool>) (fr => string.Equals(fr.Id, recipientId, StringComparison.CurrentCultureIgnoreCase))) ? (EnhancedDisclosureTracking2015Log.FulfillmentFields) null : firstFulfillment;
    }

    DateTime IDisclosureTrackingLog.eDisclosureManualFulfillmentDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment(new bool?(true));
        return firstFulfillment == null ? DateTime.MinValue : firstFulfillment.ProcessedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment(new bool?(true));
        if (firstFulfillment == null)
          return;
        firstFulfillment.ProcessedDate = this.CreateDateTimeWithZone(value);
      }
    }

    DisclosureTrackingBase.DisclosedMethod IDisclosureTrackingLog.eDisclosureManualFulfillmentMethod
    {
      get
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment(new bool?(true));
        return firstFulfillment == null ? DisclosureTrackingBase.DisclosedMethod.None : firstFulfillment.DisclosedMethod;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment(new bool?(true));
        if (firstFulfillment == null)
          return;
        firstFulfillment.DisclosedMethod = value;
      }
    }

    DisclosureTrackingBase.DisclosedMethod IDisclosureTrackingLog.eDisclosureAutomatedFulfillmentMethod
    {
      get
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment(new bool?(false));
        return firstFulfillment == null || firstFulfillment.IsManual ? DisclosureTrackingBase.DisclosedMethod.None : firstFulfillment.DisclosedMethod;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment(new bool?(false));
        if (firstFulfillment == null)
          return;
        firstFulfillment.DisclosedMethod = value;
      }
    }

    string IDisclosureTrackingLog.eDisclosureManuallyFulfilledBy
    {
      get => this.GetFirstFulfillment(new bool?(true))?.OrderedBy ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment(new bool?(true));
        if (firstFulfillment == null)
          return;
        firstFulfillment.OrderedBy = value;
      }
    }

    string IDisclosureTrackingLog.FulfillmentTrackingNumber
    {
      get => this.GetFirstFulfillment(new bool?(false))?.TrackingNumber ?? string.Empty;
      set
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment(new bool?(false));
        if (firstFulfillment == null)
          return;
        firstFulfillment.TrackingNumber = value;
      }
    }

    string IDisclosureTrackingLog.eDisclosureManualFulfillmentComment
    {
      get
      {
        return this.GetFirstFulfillment(new bool?(true))?.GetFirstRecipient()?.Comments ?? string.Empty;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment(new bool?(true));
        if (firstFulfillment == null)
          return;
        firstFulfillment.GetFirstRecipient().Comments = value;
      }
    }

    DateTime IDisclosureTracking2015Log.PresumedFulfillmentDate
    {
      get
      {
        return this.GetFirstFulfillment()?.GetFirstRecipient()?.PresumedDate.DateTime ?? DateTime.MinValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment();
        if (firstFulfillment == null)
          return;
        firstFulfillment.GetFirstRecipient().PresumedDate = this.CreateDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.ActualFulfillmentDate
    {
      get
      {
        return this.GetFirstFulfillment()?.GetFirstRecipient()?.ActualDate.DateTime ?? DateTime.MinValue;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.FulfillmentFields firstFulfillment = this.GetFirstFulfillment();
        if (firstFulfillment == null)
          return;
        firstFulfillment.GetFirstRecipient().ActualDate = this.CreateDateTimeWithZone(value);
      }
    }

    string IDisclosureTracking2015Log.BorrowerFulfillmentMethodDescription
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        return disclosureRecipient == null ? string.Empty : disclosureRecipient.DisclosedMethodDescription ?? string.Empty;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.DisclosedMethodDescription = value;
      }
    }

    string IDisclosureTracking2015Log.CoBorrowerFulfillmentMethodDescription
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        return disclosureRecipient == null ? string.Empty : disclosureRecipient.DisclosedMethodDescription ?? string.Empty;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        disclosureRecipient.DisclosedMethodDescription = value;
      }
    }

    DateTime IDisclosureTracking2015Log.FullfillmentProcessedDate
    {
      get
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return DateTime.MinValue;
        EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillmentForRecipient = this.GetFirstFulfillmentForRecipient(disclosureRecipient.Id, new bool?(false));
        return fulfillmentForRecipient == null ? DateTime.MinValue : fulfillmentForRecipient.ProcessedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.BorrowerRecipient disclosureRecipient = this.GetFirstBorrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillmentForRecipient = this.GetFirstFulfillmentForRecipient(disclosureRecipient.Id, new bool?(false));
        if (fulfillmentForRecipient == null)
          return;
        fulfillmentForRecipient.ProcessedDate = this.ConvertToDateTimeWithZone(value);
      }
    }

    DateTime IDisclosureTracking2015Log.FullfillmentProcessedDate_CoBorrower
    {
      get
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return DateTime.MinValue;
        EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillmentForRecipient = this.GetFirstFulfillmentForRecipient(disclosureRecipient.Id, new bool?(false));
        return fulfillmentForRecipient == null ? DateTime.MinValue : fulfillmentForRecipient.ProcessedDate.DateTime;
      }
      set
      {
        EnhancedDisclosureTracking2015Log.CoborrowerRecipient disclosureRecipient = this.GetFirstCoborrowerDisclosureRecipient();
        if (disclosureRecipient == null)
          return;
        EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillmentForRecipient = this.GetFirstFulfillmentForRecipient(disclosureRecipient.Id, new bool?(false));
        if (fulfillmentForRecipient == null)
          return;
        fulfillmentForRecipient.ProcessedDate = this.ConvertToDateTimeWithZone(value);
      }
    }

    Dictionary<string, INonBorrowerOwnerItem> IDisclosureTracking2015Log.GetAllnboItems()
    {
      return this.nonBorrowerOwnerCollections;
    }

    Dictionary<string, INonBorrowerOwnerItem> IDisclosureTracking2015Log.NonBorrowerOwnerCollections
    {
      get => this.nonBorrowerOwnerCollections;
    }

    void IDisclosureTracking2015Log.SetnboAttributeValue(
      string nboInstance,
      object value,
      DisclosureTracking2015Log.NBOUpdatableFields fld)
    {
      if (value == null)
        return;
      bool flag = false;
      switch (fld)
      {
        case DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethod:
          this.nonBorrowerOwnerCollections[nboInstance].DisclosedMethod = Utils.ParseInt(value);
          flag = true;
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethodOther:
          this.nonBorrowerOwnerCollections[nboInstance].DisclosedMethodOther = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate:
          this.nonBorrowerOwnerCollections[nboInstance].PresumedReceivedDate = Utils.ParseDate(value);
          flag = true;
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.lockedPresumedReceivedDate:
          this.nonBorrowerOwnerCollections[nboInstance].lockedPresumedReceivedDate = Utils.ParseDate(value);
          flag = true;
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.isPresumedDateLocked:
          this.nonBorrowerOwnerCollections[nboInstance].isPresumedDateLocked = Utils.ParseBoolean(value);
          flag = true;
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.ActualReceivedDate:
          this.nonBorrowerOwnerCollections[nboInstance].ActualReceivedDate = Utils.ParseDate(value);
          flag = true;
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.isBorrowerTypeLocked:
          this.nonBorrowerOwnerCollections[nboInstance].isBorrowerTypeLocked = Convert.ToBoolean(value);
          flag = true;
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.BorrowerType:
          this.nonBorrowerOwnerCollections[nboInstance].BorrowerType = value.ToString();
          flag = true;
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.LockedBorrowerType:
          this.nonBorrowerOwnerCollections[nboInstance].LockedBorrowerType = value.ToString();
          flag = true;
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAuthenticatedDate = Utils.ParseDate(value);
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedIP:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAuthenticatedIP = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOViewMessageDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOViewMessageDate = Utils.ParseDate(value);
          flag = true;
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBORejectConsentDate = Utils.ParseDate(value);
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentIP:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBORejectConsentIP = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOSignedDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOSignedDate = Utils.ParseDate(value);
          flag = true;
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignedIP:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOeSignedIP = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOLoanLevelConsent:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOLoanLevelConsent = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAcceptConsentDate = Utils.ParseDate(value);
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentIP:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAcceptConsentIP = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBODocumentViewedDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBODocumentViewedDate = Utils.ParseDate(value);
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignatures:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOeSignatures = Utils.ParseBoolean(value);
          break;
      }
      if (!flag)
        return;
      this.reCalcDisclosureTracking();
    }

    public void UpdateRecipients(
      Dictionary<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> localRecipients)
    {
      if (localRecipients == null)
        return;
      foreach (KeyValuePair<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> localRecipient1 in localRecipients)
      {
        KeyValuePair<string, EnhancedDisclosureTracking2015Log.DisclosureRecipient> localRecipient = localRecipient1;
        if (this.DisclosureRecipients.First<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (rec => rec.Id == localRecipient.Key)) != null)
        {
          EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient = localRecipient.Value;
        }
      }
      this.reCalcDisclosureTracking();
    }

    string IDisclosureTracking2015Log.GetnboAttributeValue(
      string nboInstance,
      DisclosureTracking2015Log.NBOUpdatableFields flds)
    {
      INonBorrowerOwnerItem borrowerOwnerCollection = this.nonBorrowerOwnerCollections[nboInstance];
      if (borrowerOwnerCollection == null)
        throw new ArgumentException("Invalid nboInstance: " + nboInstance);
      switch (flds)
      {
        case DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethod:
          return borrowerOwnerCollection.DisclosedMethod.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethodOther:
          return borrowerOwnerCollection.DisclosedMethodOther;
        case DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate:
          return borrowerOwnerCollection.PresumedReceivedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.lockedPresumedReceivedDate:
          return borrowerOwnerCollection.lockedPresumedReceivedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.isPresumedDateLocked:
          return !borrowerOwnerCollection.isPresumedDateLocked ? "F" : "T";
        case DisclosureTracking2015Log.NBOUpdatableFields.ActualReceivedDate:
          return borrowerOwnerCollection.ActualReceivedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.isBorrowerTypeLocked:
          return !borrowerOwnerCollection.isBorrowerTypeLocked ? "F" : "T";
        case DisclosureTracking2015Log.NBOUpdatableFields.BorrowerType:
          return borrowerOwnerCollection.BorrowerType;
        case DisclosureTracking2015Log.NBOUpdatableFields.LockedBorrowerType:
          return borrowerOwnerCollection.LockedBorrowerType;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedDate:
          return borrowerOwnerCollection.eDisclosureNBOAuthenticatedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedIP:
          return borrowerOwnerCollection.eDisclosureNBOAuthenticatedIP;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOViewMessageDate:
          return borrowerOwnerCollection.eDisclosureNBOViewMessageDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentDate:
          return borrowerOwnerCollection.eDisclosureNBORejectConsentDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentIP:
          return borrowerOwnerCollection.eDisclosureNBORejectConsentIP;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOSignedDate:
          return borrowerOwnerCollection.eDisclosureNBOSignedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignedIP:
          return borrowerOwnerCollection.eDisclosureNBOeSignedIP;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOLoanLevelConsent:
          return borrowerOwnerCollection.eDisclosureNBOLoanLevelConsent.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentDate:
          return borrowerOwnerCollection.eDisclosureNBOAcceptConsentDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentIP:
          return borrowerOwnerCollection.eDisclosureNBOAcceptConsentIP;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBODocumentViewedDate:
          return borrowerOwnerCollection.eDisclosureNBODocumentViewedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignatures:
          return !borrowerOwnerCollection.eDisclosureNBOeSignatures ? "0" : "1";
        default:
          return string.Empty;
      }
    }

    void IDisclosureTracking2015Log.PopulateLoanDataList(Dictionary<string, string> dataFields)
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

    Dictionary<string, string> IDisclosureTrackingLog.GetDisclosedFields(List<string> fieldIDs)
    {
      Dictionary<string, string> disclosedFields = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (DisclosedLoanItem loanData in this.loanDataList)
      {
        if (fieldIDs.Contains(loanData.FieldID) && !disclosedFields.ContainsKey(loanData.FieldID))
          disclosedFields.Add(loanData.FieldID, loanData.FieldValue);
      }
      return disclosedFields;
    }

    int IDisclosureTracking2015Log.CompareDisclosedDate(IDisclosureTracking2015Log obj)
    {
      IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) this;
      int num = DateTime.Compare(disclosureTracking2015Log.DisclosedDateTime, obj.DisclosedDateTime);
      if (num == 0)
        num = DateTime.Compare(disclosureTracking2015Log.Date, obj.Date);
      return num;
    }

    public EnhancedDisclosureTracking2015Log(LogList log, XmlElement e)
      : base(log, e)
    {
      this.loanData = log.Loan;
      this.TimeZoneInfo = EnhancedDisclosureTracking2015Log.GetTimeZoneInfo(this.loanData);
      if (this.Date.Kind.Equals((object) DateTimeKind.Unspecified))
        this.Date = Utils.ConvertTimeToUtc(this.Date, this.TimeZoneInfo);
      this.initializingFromXml = true;
      XmlAutoMapper.FromXml((IXmlMapperContext) new XmlElementBasedMapperContext(e, this.TimeZoneInfo), (object) this);
      this.PopulateNBOOobject();
      this.InitializeObjects();
      this.initializingFromXml = false;
      this.MarkAsClean();
    }

    public EnhancedDisclosureTracking2015Log(
      DateTime date,
      LoanData currentLoanData,
      string provider,
      IEnumerable<EnhancedDisclosureTracking2015Log.DisclosureContentType> disclosureContentTypes = null,
      IEnumerable<DisclosureTrackingFormItem> forms = null,
      IEnumerable<EnhancedDisclosureTracking2015Log.DisclosureRecipient> recipients = null,
      DisclosureTracking2015Log.TrackingLogStatus status = DisclosureTracking2015Log.TrackingLogStatus.Active,
      DisclosureTrackingFormItem.FormSignatureType signatureType = DisclosureTrackingFormItem.FormSignatureType.None)
      : this(date, currentLoanData, currentLoanData, provider, disclosureContentTypes, forms, recipients, status)
    {
    }

    public EnhancedDisclosureTracking2015Log(
      DateTime date,
      LoanData currentLoanData,
      LoanData loanDataForRead,
      string provider,
      IEnumerable<EnhancedDisclosureTracking2015Log.DisclosureContentType> disclosureContentTypes = null,
      IEnumerable<DisclosureTrackingFormItem> forms = null,
      IEnumerable<EnhancedDisclosureTracking2015Log.DisclosureRecipient> recipients = null,
      DisclosureTracking2015Log.TrackingLogStatus status = DisclosureTracking2015Log.TrackingLogStatus.Active,
      DisclosureTrackingFormItem.FormSignatureType signatureType = DisclosureTrackingFormItem.FormSignatureType.None)
      : base(date.ToUniversalTime(), string.Empty)
    {
      this.loanData = currentLoanData;
      this.TimeZoneInfo = EnhancedDisclosureTracking2015Log.GetTimeZoneInfo(this.loanData);
      if (disclosureContentTypes != null)
        this.ValidateDisclosureContentTypes(disclosureContentTypes);
      this.Contents = disclosureContentTypes == null ? (IList<EnhancedDisclosureTracking2015Log.DisclosureContentType>) new List<EnhancedDisclosureTracking2015Log.DisclosureContentType>() : (IList<EnhancedDisclosureTracking2015Log.DisclosureContentType>) new List<EnhancedDisclosureTracking2015Log.DisclosureContentType>(disclosureContentTypes);
      this.Fulfillments = (IList<EnhancedDisclosureTracking2015Log.FulfillmentFields>) new List<EnhancedDisclosureTracking2015Log.FulfillmentFields>();
      this.DisclosedDate = new EnhancedDisclosureTracking2015Log.DisclosedDateField(this);
      this.DisclosedBy = new EnhancedDisclosureTracking2015Log.LockableUserRefField(this);
      this.DisclosedApr = new EnhancedDisclosureTracking2015Log.LockableField<string>(this);
      this.DisclosedFinanceCharge = new EnhancedDisclosureTracking2015Log.LockableField<string>(this);
      this.DisclosedDailyInterest = new EnhancedDisclosureTracking2015Log.LockableField<string>(this);
      this.Tracking = new EnhancedDisclosureTracking2015Log.TrackingFields(this);
      this.Documents = new EnhancedDisclosureTracking2015Log.DisclosureTrackingDocuments(forms ?? (IEnumerable<DisclosureTrackingFormItem>) new List<DisclosureTrackingFormItem>(), signatureType);
      this.LoanEstimate = new EnhancedDisclosureTracking2015Log.LoanEstimateFields(this);
      this.ClosingDisclosure = new EnhancedDisclosureTracking2015Log.ClosingDisclosureFields(this);
      this.IntentToProceed = new EnhancedDisclosureTracking2015Log.IntentToProceedFields(this);
      this.Attributes = new Dictionary<string, string>();
      this.PropertyAddress = new EnhancedDisclosureTracking2015Log.Address();
      this.IncludedInTimeline = status == DisclosureTracking2015Log.TrackingLogStatus.Active;
      this.Status = status;
      this.Provider = provider;
      this.DisclosureCreatedDate = Utils.TruncateDateTime(date).ToUniversalTime();
      this.DisclosureRecipients = (IList<EnhancedDisclosureTracking2015Log.DisclosureRecipient>) this.PopulateRecipients(recipients == null ? new List<EnhancedDisclosureTracking2015Log.DisclosureRecipient>() : new List<EnhancedDisclosureTracking2015Log.DisclosureRecipient>(recipients), loanDataForRead);
      this.PopulateNBOOobject();
      this.populateAttributes(loanDataForRead);
      this.SetUDTSnapshotData(loanDataForRead);
      this.setFormFields(loanDataForRead);
      this.Documents.SignatureType = this.EvaluateFormSignatureType(this.Documents.Forms, this.Contents, loanDataForRead);
    }

    private void InitializeObjects()
    {
      if (this.Contents == null)
        this.Contents = (IList<EnhancedDisclosureTracking2015Log.DisclosureContentType>) new List<EnhancedDisclosureTracking2015Log.DisclosureContentType>();
      if (this.Fulfillments == null)
        this.Fulfillments = (IList<EnhancedDisclosureTracking2015Log.FulfillmentFields>) new List<EnhancedDisclosureTracking2015Log.FulfillmentFields>();
      if (this.DisclosureRecipients == null)
        this.DisclosureRecipients = (IList<EnhancedDisclosureTracking2015Log.DisclosureRecipient>) new List<EnhancedDisclosureTracking2015Log.DisclosureRecipient>();
      if (this.DisclosedDate == null)
        this.DisclosedDate = new EnhancedDisclosureTracking2015Log.DisclosedDateField(this);
      if (this.DisclosedBy == null)
        this.DisclosedBy = new EnhancedDisclosureTracking2015Log.LockableUserRefField(this);
      if (this.DisclosedApr == null)
        this.DisclosedApr = new EnhancedDisclosureTracking2015Log.LockableField<string>(this);
      if (this.DisclosedFinanceCharge == null)
        this.DisclosedFinanceCharge = new EnhancedDisclosureTracking2015Log.LockableField<string>(this);
      if (this.DisclosedDailyInterest == null)
        this.DisclosedDailyInterest = new EnhancedDisclosureTracking2015Log.LockableField<string>(this);
      if (this.Tracking == null)
        this.Tracking = new EnhancedDisclosureTracking2015Log.TrackingFields(this);
      if (this.Documents == null)
        this.Documents = new EnhancedDisclosureTracking2015Log.DisclosureTrackingDocuments();
      if (this.LoanEstimate == null)
        this.LoanEstimate = new EnhancedDisclosureTracking2015Log.LoanEstimateFields(this);
      if (this.ClosingDisclosure == null)
        this.ClosingDisclosure = new EnhancedDisclosureTracking2015Log.ClosingDisclosureFields(this);
      if (this.IntentToProceed == null)
        this.IntentToProceed = new EnhancedDisclosureTracking2015Log.IntentToProceedFields(this);
      if (this.Attributes == null)
        this.Attributes = new Dictionary<string, string>();
      if (this.PropertyAddress != null)
        return;
      this.PropertyAddress = new EnhancedDisclosureTracking2015Log.Address();
    }

    public string DisclosedMethodNameByType(DisclosureTrackingBase.DisclosedMethod type)
    {
      switch (type)
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
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          return "Phone";
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          return "eFolder eDisclosures";
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
          return "Closing Docs Order";
        case DisclosureTrackingBase.DisclosedMethod.eClose:
          return "eClose";
        default:
          return "In Person";
      }
    }

    public string GetDisclosedField(string fieldId, bool retrieveFromOtherLog)
    {
      string empty = string.Empty;
      if ((fieldId.StartsWith("XCOC") || fieldId.StartsWith("NBOC") || fieldId.StartsWith("NBOC")) && fieldId.Length >= 8 || fieldId.StartsWith("TR") && fieldId.Length >= 6)
        return this.GetAttribute(fieldId) ?? string.Empty;
      if (this.loanDataList.Count == 0 && this.loanData.SnapshotProvider != null)
      {
        if (EnhancedDisclosureTracking2015Log.cachedDisclosureTrackingLoanSnapShots.Count > 20)
          EnhancedDisclosureTracking2015Log.cachedDisclosureTrackingLoanSnapShots.Clear();
        string key = this.loanData.GUID + "-" + this.Guid;
        Dictionary<string, string> dataFields;
        EnhancedDisclosureTracking2015Log.cachedDisclosureTrackingLoanSnapShots.TryGetValue(key, out dataFields);
        if (dataFields == null)
        {
          bool ucdExists = this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD) || this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE);
          dataFields = EnhancedDisclosureTracking2015Log.cachedDisclosureTrackingLoanSnapShots[key] = this.loanData.SnapshotProvider.GetLoanSnapshot(LogSnapshotType.DisclosureTracking, new System.Guid(this.Guid), ucdExists);
        }
        this.PopulateLoanDataList(dataFields);
      }
      string str = (this.loanDataFromOtherLogs == null || !this.loanDataFromOtherLogs.ContainsKey((object) fieldId) ? (!(fieldId == "FV.X396") && !(fieldId == "FV.X397") || this.loanDataUniqueList.ContainsKey((object) "FV.X396") || this.loanDataUniqueList.ContainsKey((object) "FV.X397") ? (string) this.loanDataUniqueList[(object) fieldId] : (fieldId == "FV.X396" ? (string) this.loanDataUniqueList[(object) "FV.X366"] : string.Empty)) : (string) this.loanDataFromOtherLogs[(object) fieldId]) ?? string.Empty;
      if (fieldId == "1868" && str == string.Empty && this.loanData != null)
      {
        str = this.loanData.GetField("1868");
        if (str == string.Empty)
          str = this.FormatName(this.loanData.GetField("4000"), this.loanData.GetField("4001"), this.loanData.GetField("4002"), this.loanData.GetField("4003"));
      }
      if (fieldId == "1873" && str == string.Empty && this.loanData != null && (string) this.loanDataUniqueList[(object) "1868"] + string.Empty == string.Empty)
      {
        str = this.loanData.GetField("1873");
        if (str == string.Empty)
          str = this.FormatName(this.loanData.GetField("4004"), this.loanData.GetField("4005"), this.loanData.GetField("4006"), this.loanData.GetField("4007"));
      }
      if (this.loanData != null)
      {
        FieldFormat format = fieldId == "SSPLcount" || fieldId == "TRcount" || fieldId == "NBOcount" ? FieldFormat.INTEGER : this.loanData.GetFormat(fieldId);
        str = Utils.ApplyFieldFormatting(str, format);
      }
      return str ?? string.Empty;
    }

    public string GetDisclosedField(string fieldId) => this.GetDisclosedField(fieldId, true);

    public string GetAttribute(string name)
    {
      return this.Attributes.ContainsKey(name) ? this.Attributes[name] : string.Empty;
    }

    private void SetAttribute(string name, string value)
    {
      this.Attributes[name] = value ?? string.Empty;
    }

    public virtual string FormatName(
      string firstName,
      string middleName,
      string lastName,
      string suffix)
    {
      return firstName.Trim() + (middleName.Trim() != string.Empty ? " " + middleName.Trim() : string.Empty) + (lastName.Trim() != string.Empty ? " " + lastName.Trim() : string.Empty) + (suffix.Trim() != string.Empty ? " " + suffix.Trim() : string.Empty);
    }

    public void PopulateLoanDataList(Dictionary<string, string> dataFields)
    {
      if (dataFields == null)
        return;
      foreach (KeyValuePair<string, string> dataField in dataFields)
      {
        if (!this.loanDataUniqueList.Contains((object) dataField.Key))
        {
          this.loanDataList.Add(new DisclosedLoanItem(dataField.Key, dataField.Value));
          this.loanDataUniqueList.Add((object) dataField.Key, (object) dataField.Value);
        }
      }
    }

    public void SetLoanDataFromOtherLogs(string id, string val)
    {
      if (this.loanDataFromOtherLogs.ContainsKey((object) id))
        this.loanDataFromOtherLogs[(object) id] = (object) val;
      else
        this.loanDataFromOtherLogs.Add((object) id, (object) val);
    }

    public void ResetLoanDataFromOtherLogs() => this.loanDataFromOtherLogs.Clear();

    public string GetCureLogComment()
    {
      string cureLogComment = string.Empty;
      if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE))
        cureLogComment = (this.LoanEstimate.IsChangedCircumstanceSettlementCharges ? "Changed Circumstance - Settlement Charges," : string.Empty) + (this.LoanEstimate.IsChangedCircumstanceEligibility ? "Changed Circumstance - Eligibility," : string.Empty) + (this.LoanEstimate.IsRevisionsRequestedByConsumer ? "Revisions requested by the Consumer," : string.Empty) + (this.LoanEstimate.IsInterestRateDependentCharges ? "Interest Rate dependent charges (Rate Lock)," : string.Empty) + (this.LoanEstimate.IsExpiration ? "Expiration (Intent to Proceed received after 10 business days)," : string.Empty) + (this.LoanEstimate.IsDelayedSettlementOnConstructionLoans ? "Delayed Settlement on Construction Loans," : string.Empty) + (this.LoanEstimate.IsOther ? "," + this.LoanEstimate.OtherDescription : string.Empty);
      if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD))
        cureLogComment = (this.ClosingDisclosure.IsChangeInAPR ? "Change in APR," : string.Empty) + (this.ClosingDisclosure.IsChangeInLoanProduct ? "Change in Loan Product," : string.Empty) + (this.ClosingDisclosure.IsPrepaymentPenaltyAdded ? "Prepayment Penalty Added," : string.Empty) + (this.ClosingDisclosure.IsChangeInSettlementCharges ? "Change in Settlement Charges," : string.Empty) + (this.ClosingDisclosure.IsChangedCircumstanceEligibility ? "Changed Circumstance - Eligibility," : string.Empty) + (this.ClosingDisclosure.IsRevisionsRequestedByConsumer ? "Revisions requested by the Consumer," : string.Empty) + (this.ClosingDisclosure.IsInterestRateDependentCharges ? "Interest Rate dependent charges (Rate Lock)," : string.Empty) + (this.ClosingDisclosure.Is24HourAdvancePreview ? "24-hour Advanced Preview," : string.Empty) + (this.ClosingDisclosure.IsToleranceCure ? "Tolerance Cure," : string.Empty) + (this.ClosingDisclosure.IsClericalErrorCorrection ? "Clerical Error Correction," : string.Empty) + (this.ClosingDisclosure.IsOther ? "," + this.ClosingDisclosure.OtherDescription : string.Empty);
      if (cureLogComment != string.Empty)
      {
        if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE))
          cureLogComment = "LE issued with Change of Circumstance containing the following reasons: " + cureLogComment.TrimEnd(',');
        else if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD))
          cureLogComment = "CD issued with Change of Circumstance containing the following reasons:" + cureLogComment.TrimEnd(',');
      }
      else if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE))
        cureLogComment = "LE issued with Change of Circumstance";
      else if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD))
        cureLogComment = "CD issued with Change of Circumstance";
      return cureLogComment;
    }

    public void CreateCureLog(
      double appliedCureAmount,
      string cureLogComment,
      Hashtable triggerFields,
      string resolvedById,
      string resolvedBy,
      bool newLog)
    {
      if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE) && this.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.Revised || appliedCureAmount == 0.0 || RegulationAlerts.GetGoodFaithFeeVarianceViolationAlert(this.loanData) != null)
        return;
      string fieldValue = DateTime.Now.ToString("MM/dd/yyyy");
      GoodFaithFeeVarianceCureLog rec = new GoodFaithFeeVarianceCureLog(Utils.ParseDate((object) fieldValue), resolvedById, resolvedBy, appliedCureAmount.ToString(), this.loanData.GetField("FV.X348"), cureLogComment, "Variance Cured", DateTime.Now);
      foreach (DictionaryEntry triggerField1 in triggerFields)
      {
        ArrayList arrayList = (ArrayList) triggerField1.Value;
        for (int index = 0; index < arrayList.Count; ++index)
        {
          GFFVAlertTriggerField triggerField2 = (GFFVAlertTriggerField) arrayList[index];
          rec.TriggerFieldList.Add(triggerField2);
        }
      }
      if (newLog)
      {
        DisclosedLoanItem disclosedLoanItem1 = new DisclosedLoanItem("3171", fieldValue);
        if (this.loanDataList.Contains(disclosedLoanItem1))
        {
          this.loanDataList.Remove(disclosedLoanItem1);
          this.loanDataList.Add(disclosedLoanItem1);
        }
        DisclosedLoanItem disclosedLoanItem2 = new DisclosedLoanItem("3172", cureLogComment);
        if (this.loanDataList.Contains(disclosedLoanItem2))
        {
          this.loanDataList.Remove(disclosedLoanItem2);
          this.loanDataList.Add(disclosedLoanItem2);
        }
        DisclosedLoanItem disclosedLoanItem3 = new DisclosedLoanItem("3173", resolvedById);
        if (this.loanDataList.Contains(disclosedLoanItem3))
        {
          this.loanDataList.Remove(disclosedLoanItem3);
          this.loanDataList.Add(disclosedLoanItem3);
        }
        DisclosedLoanItem disclosedLoanItem4 = new DisclosedLoanItem("FV.X366", appliedCureAmount.ToString());
        if (this.loanDataList.Contains(disclosedLoanItem4))
        {
          this.loanDataList.Remove(disclosedLoanItem4);
          this.loanDataList.Add(disclosedLoanItem4);
        }
      }
      this.loanData.GetLogList().AddRecord((LogRecordBase) rec);
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      new AttributeWriter(e).Write("Type", (object) EnhancedDisclosureTracking2015Log.XmlType);
      XmlAutoMapper.ToXml((object) this, (IXmlMapperContext) new XmlElementBasedMapperContext(e, this.TimeZoneInfo));
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

    public void AppendFieldValues(Dictionary<string, string> fields)
    {
      StringBuilder stringBuilder = new StringBuilder(this.udt);
      foreach (KeyValuePair<string, string> field in fields)
      {
        this.AddDisclosedLoanInfo(field.Key, field.Value);
        stringBuilder.Append(this.AddDisclosedFieldString(field.Key, field.Value));
      }
      this.udt = stringBuilder.ToString();
    }

    public bool IsFieldExist(string fieldId) => this.loanDataUniqueList.Contains((object) fieldId);

    public void SetItemizationFields(List<string[]> fields, LoanData loanDataForRead)
    {
      StringBuilder stringBuilder = new StringBuilder();
      this.itemizationFields = fields;
      foreach (string[] itemizationField in this.itemizationFields)
      {
        for (int index = 2; index < itemizationField.Length; ++index)
        {
          string simpleField = loanDataForRead.GetSimpleField(itemizationField[index]);
          if (!(simpleField.Trim() == string.Empty))
          {
            if (!this.IsFieldExist(itemizationField[index]))
              stringBuilder.Append(this.AddDisclosedFieldString(itemizationField[index], simpleField));
            this.AddDisclosedLoanInfo(itemizationField[index], simpleField);
          }
        }
      }
      if ((this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE) || this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD)) && loanDataForRead.Calculator != null)
      {
        string fieldValue1 = loanDataForRead.Calculator.SetTotalLenderCredit((IDisclosureTracking2015Log) this);
        string fieldValue2 = loanDataForRead.Calculator.SetCannotIncrease((IDisclosureTracking2015Log) this);
        this.AddDisclosedLoanInfo("TotalLenderCredit", fieldValue1);
        stringBuilder.Append(this.AddDisclosedFieldString("TotalLenderCredit", fieldValue1));
        this.AddDisclosedLoanInfo("CannotIncrease", fieldValue2);
        stringBuilder.Append(this.AddDisclosedFieldString("CannotIncrease", fieldValue2));
      }
      this.udt += stringBuilder.ToString();
    }

    public void SetUDTSnapshotData(LoanData loanDataForRead)
    {
      this.udt += this.ExtractUDTSnapshotData(loanDataForRead);
    }

    public DateTimeWithZone ConvertToDateTimeWithZone(DateTime srcDate)
    {
      return DateTimeWithZone.ConvertToTimeZone(srcDate, this.TimeZoneInfo);
    }

    public DateTimeWithZone CreateDateTimeWithZone(DateTime srcDate)
    {
      return DateTimeWithZone.Create(srcDate, this.TimeZoneInfo);
    }

    public static bool UseLE1X9ForTimeZone(LoanData loandata)
    {
      return Mapping.UseNewCompliance(21.104M, loandata.GetField("COMPLIANCEVERSION.X1"));
    }

    public static System.TimeZoneInfo GetTimeZoneInfo(LoanData loandata)
    {
      string timeZoneCode = loandata.GetField("LE1.XG9") == "" ? loandata.GetField("LE1.X9") : loandata.GetField("LE1.XG9");
      return !EnhancedDisclosureTracking2015Log.UseLE1X9ForTimeZone(loandata) ? Utils.GetTimeZoneInfo("PST") : Utils.GetTimeZoneInfo(timeZoneCode);
    }

    private string ExtractUDTSnapshotData(LoanData loanDataForRead)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string disclosureSnapshotField in DisclosureTracking2015Log.disclosureSnapshotFields)
      {
        if (!string.IsNullOrEmpty(disclosureSnapshotField))
        {
          string simpleField = loanDataForRead.GetSimpleField(disclosureSnapshotField);
          if (!(simpleField.Trim() == string.Empty))
          {
            this.AddDisclosedLoanInfo(disclosureSnapshotField, simpleField);
            stringBuilder.Append(this.AddDisclosedFieldString(disclosureSnapshotField, simpleField));
          }
        }
      }
      if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD))
      {
        foreach (string disclosedCdField in DisclosureTracking2015Log.DisclosedCDFields)
        {
          if (!string.IsNullOrEmpty(disclosedCdField) && !disclosedCdField.StartsWith("CD3.XH"))
          {
            string simpleField = loanDataForRead.GetSimpleField(disclosedCdField);
            if (!(simpleField.Trim() == string.Empty) || !(disclosedCdField != "L726"))
            {
              this.AddDisclosedLoanInfo(disclosedCdField, simpleField);
              stringBuilder.Append(this.AddDisclosedFieldString(disclosedCdField, simpleField));
            }
          }
        }
      }
      if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE))
      {
        foreach (string disclosedLeField in DisclosureTracking2015Log.DisclosedLEFields)
        {
          if (!string.IsNullOrEmpty(disclosedLeField) && !disclosedLeField.StartsWith("CD3.XH"))
          {
            string simpleField = loanDataForRead.GetSimpleField(disclosedLeField);
            if (!(simpleField.Trim() == string.Empty) || !(disclosedLeField != "L726"))
            {
              this.AddDisclosedLoanInfo(disclosedLeField, simpleField);
              stringBuilder.Append(this.AddDisclosedFieldString(disclosedLeField, simpleField));
            }
          }
        }
      }
      if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD) || this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE))
      {
        foreach (string itemizationField in DisclosureTracking2015Log.DisclosedItemizationFields)
        {
          if (!string.IsNullOrEmpty(itemizationField))
          {
            string simpleField = loanDataForRead.GetSimpleField(itemizationField);
            if (!(simpleField.Trim() == string.Empty) || !(itemizationField != "FV.X396") || !(itemizationField != "FV.X397"))
            {
              this.AddDisclosedLoanInfo(itemizationField, simpleField);
              stringBuilder.Append(this.AddDisclosedFieldString(itemizationField, simpleField));
            }
          }
        }
        foreach (string[] itemizationField in this.itemizationFields)
        {
          for (int index = 2; index <= itemizationField.Length; ++index)
          {
            if (!string.IsNullOrEmpty(itemizationField[index]))
            {
              string simpleField = loanDataForRead.GetSimpleField(itemizationField[index]);
              if (!(simpleField.Trim() == string.Empty))
              {
                this.AddDisclosedLoanInfo(itemizationField[index], simpleField);
                stringBuilder.Append(this.AddDisclosedFieldString(itemizationField[index], simpleField));
              }
            }
          }
        }
      }
      if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.SafeHarbor))
      {
        foreach (string disclosedSafeHarborField in DisclosureTrackingBase.GetDisclosedSafeHarborFields())
        {
          if (!string.IsNullOrEmpty(disclosedSafeHarborField) && !DisclosureTracking2015Log.disclosureSnapshotFields.Contains(disclosedSafeHarborField))
          {
            this.AddDisclosedLoanInfo(disclosedSafeHarborField, loanDataForRead.GetSimpleField(disclosedSafeHarborField));
            stringBuilder.Append(this.AddDisclosedFieldString(disclosedSafeHarborField, loanDataForRead.GetSimpleField(disclosedSafeHarborField)));
            if (loanDataForRead.IsLocked(disclosedSafeHarborField) && !DisclosureTracking2015Log.disclosureSnapshotFields.Contains(disclosedSafeHarborField + "_LOCKED"))
            {
              this.AddDisclosedLoanInfo(disclosedSafeHarborField + "_LOCKED", loanDataForRead.GetFieldFromCal(disclosedSafeHarborField));
              stringBuilder.Append(this.AddDisclosedFieldString(disclosedSafeHarborField + "_LOCKED", loanDataForRead.GetFieldFromCal(disclosedSafeHarborField)));
            }
          }
        }
      }
      if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.ServiceProviderList) || this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.ServiceProviderListNoFee))
      {
        int serviceProviders = loanDataForRead.GetNumberOfSettlementServiceProviders();
        this.AddDisclosedLoanInfo("SSPLcount", serviceProviders.ToString());
        stringBuilder.Append(this.AddDisclosedFieldString("SSPLcount", serviceProviders.ToString()));
        for (int index = 1; index <= serviceProviders; ++index)
        {
          string str1 = index.ToString("00");
          foreach (string disclosedSsplField in DisclosureTracking2015Log.DisclosedSSPLFields)
          {
            string str2 = disclosedSsplField.Substring(disclosedSsplField.Length - 2, 2);
            this.AddDisclosedLoanInfo("SP" + str1 + str2, loanDataForRead.GetField("SP" + str1 + str2));
            stringBuilder.Append(this.AddDisclosedFieldString("SP" + str1 + str2, loanDataForRead.GetField("SP" + str1 + str2)));
          }
        }
      }
      this.AddDisclosedLoanInfo("edisclosureDisclosedMessage", string.Empty);
      stringBuilder.Append(this.AddDisclosedFieldString("edisclosureDisclosedMessage", string.Empty));
      return stringBuilder.ToString();
    }

    internal override void OnRecordAdd() => this.CalculateLatestDisclosure2015();

    private void reCalcDisclosureTracking()
    {
      if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
        this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
      this.MarkAsDirty();
    }

    private void PopulateNBOOobject()
    {
      if (this.Attributes == null)
        return;
      IEnumerable<KeyValuePair<string, string>> source = this.Attributes.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (p => p.Key.ToLower() == "nbocount"));
      if (source == null || !source.Any<KeyValuePair<string, string>>())
        return;
      foreach (string key in this.Attributes.Keys.Where<string>((Func<string, bool>) (k => k.StartsWith("NBOC") && k.EndsWith("01"))).Select<string, string>((Func<string, string>) (k => k.Substring(0, 6))).ToList<string>())
      {
        string FName = this.Attributes.ContainsKey(key + "01") ? this.Attributes[key + "01"] : string.Empty;
        string MName = this.Attributes.ContainsKey(key + "02") ? this.Attributes[key + "02"] : string.Empty;
        string LName = this.Attributes.ContainsKey(key + "03") ? this.Attributes[key + "03"] : string.Empty;
        string Suffix = this.Attributes.ContainsKey(key + "04") ? this.Attributes[key + "04"] : string.Empty;
        string Address = this.Attributes.ContainsKey(key + "05") ? this.Attributes[key + "05"] : string.Empty;
        string City = this.Attributes.ContainsKey(key + "06") ? this.Attributes[key + "06"] : string.Empty;
        string State = this.Attributes.ContainsKey(key + "07") ? this.Attributes[key + "07"] : string.Empty;
        string Zip = this.Attributes.ContainsKey(key + "08") ? this.Attributes[key + "08"] : string.Empty;
        string VestingType = this.Attributes.ContainsKey(key + "09") ? this.Attributes[key + "09"] : string.Empty;
        string HomePhone = this.Attributes.ContainsKey(key + "10") ? this.Attributes[key + "10"] : string.Empty;
        string Email = this.Attributes.ContainsKey(key + "11") ? this.Attributes[key + "11"] : string.Empty;
        bool IsNoThirdPartyEmail = this.Attributes.ContainsKey(key + "12") && Utils.ParseBoolean((object) this.Attributes[key + "12"]);
        string BusiPhone = this.Attributes.ContainsKey(key + "13") ? this.Attributes[key + "13"] : string.Empty;
        string Cell = this.Attributes.ContainsKey(key + "14") ? this.Attributes[key + "14"] : string.Empty;
        string Fax = this.Attributes.ContainsKey(key + "15") ? this.Attributes[key + "15"] : string.Empty;
        DateTime DOB = this.Attributes.ContainsKey(key + "16") ? Utils.ParseDate((object) this.Attributes[key + "16"]) : DateTime.MinValue;
        string InstanceId = key.Substring(key.Length - 2, 2);
        string Guid = this.Attributes.ContainsKey(key + "98") ? this.Attributes[key + "98"] : string.Empty;
        EnhancedDisclosureTracking2015Log.NBORecipient recipient = this.DisclosureRecipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (rec => rec is EnhancedDisclosureTracking2015Log.NBORecipient && string.Equals((rec as EnhancedDisclosureTracking2015Log.NBORecipient).Guid, Guid))) as EnhancedDisclosureTracking2015Log.NBORecipient;
        EnhancedDisclosureTracking2015Log.NonBorrowerOwnerItem borrowerOwnerItem = new EnhancedDisclosureTracking2015Log.NonBorrowerOwnerItem(FName, MName, LName, Suffix, Address, City, State, Zip, VestingType, HomePhone, Email, IsNoThirdPartyEmail, BusiPhone, Cell, Fax, DOB, Guid, InstanceId, (IDisclosureTracking2015Log) this, recipient);
        this.nonBorrowerOwnerCollections.Add(key, (INonBorrowerOwnerItem) borrowerOwnerItem);
      }
    }

    private void ValidateDisclosureContentTypes(
      IEnumerable<EnhancedDisclosureTracking2015Log.DisclosureContentType> disclosureContentTypes)
    {
      HashSet<EnhancedDisclosureTracking2015Log.DisclosureContentType> disclosureContentTypeSet = disclosureContentTypes != null && disclosureContentTypes.Any<EnhancedDisclosureTracking2015Log.DisclosureContentType>() ? new HashSet<EnhancedDisclosureTracking2015Log.DisclosureContentType>(disclosureContentTypes) : throw new ArgumentNullException(nameof (disclosureContentTypes), "disclosureContentTypes cannot be null or empty");
      if (disclosureContentTypeSet.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE) && disclosureContentTypeSet.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD))
        throw new ArgumentException("disclosureContentTypes cannot contain both LE and CD", nameof (disclosureContentTypes));
      if (disclosureContentTypeSet.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.ServiceProviderList) && disclosureContentTypeSet.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.ServiceProviderListNoFee))
        throw new ArgumentException("disclosureContentTypes cannot contain both ServiceProviderList and ServiceProviderListNoFee", nameof (disclosureContentTypes));
    }

    private Dictionary<string, string> addNBOFields(
      LoanData loanDataForRead,
      List<string> nboIds,
      bool throwOnMissingid)
    {
      HashSet<string> stringSet = new HashSet<string>((IEnumerable<string>) nboIds);
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      int num = 0;
      for (int index1 = 1; index1 <= loanDataForRead.GetNumberOfNonBorrowingOwnerContact(); ++index1)
      {
        string str1 = "NBOC" + index1.ToString("00");
        string field = loanDataForRead.GetField(str1 + "98");
        if (nboIds.Contains(field))
        {
          stringSet.Remove(field);
          ++num;
          for (int index2 = 1; index2 <= 16; ++index2)
          {
            string str2 = str1 + index2.ToString("00");
            this.Attributes.Add(str2, loanDataForRead.GetField(str2));
          }
          this.Attributes.Add(str1 + "98", field);
          dictionary[field] = str1;
        }
      }
      if (stringSet.Any<string>())
        throw new DataValidationException("Invalid NBO Ids: " + string.Join(",", (IEnumerable<string>) stringSet));
      this.Attributes["NBOcount"] = num.ToString();
      return dictionary;
    }

    private void setFormFields(LoanData loanDataForRead)
    {
      if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE))
      {
        this.Attributes.Add("ChangesLEReceivedDate", loanDataForRead.GetField("3165"));
        this.Attributes.Add("RevisedLEDueDate", loanDataForRead.GetField("3167"));
        this.addCOCFields(loanDataForRead);
        this.ChangeInCircumstance = loanDataForRead.GetField("3169");
        this.ChangeInCircumstanceComments = loanDataForRead.GetField("LE1.X86");
        this.LoanEstimate.IsChangedCircumstanceSettlementCharges = loanDataForRead.GetField("LE1.X78") == "Y";
        this.LoanEstimate.IsChangedCircumstanceEligibility = loanDataForRead.GetField("LE1.X79") == "Y";
        this.LoanEstimate.IsRevisionsRequestedByConsumer = loanDataForRead.GetField("LE1.X80") == "Y";
        this.LoanEstimate.IsInterestRateDependentCharges = loanDataForRead.GetField("LE1.X81") == "Y";
        this.LoanEstimate.IsExpiration = loanDataForRead.GetField("LE1.X82") == "Y";
        this.LoanEstimate.IsDelayedSettlementOnConstructionLoans = loanDataForRead.GetField("LE1.X83") == "Y";
        this.LoanEstimate.IsOther = loanDataForRead.GetField("LE1.X84") == "Y";
        this.LoanEstimate.OtherDescription = loanDataForRead.GetField("LE1.X85");
        this.loanData.SetField("3169", string.Empty);
        this.loanData.SetField("LE1.X90", string.Empty);
        this.loanData.SetField("LE1.X86", string.Empty);
        this.loanData.SetField("LE1.X78", "N");
        this.loanData.SetField("LE1.X79", "N");
        this.loanData.SetField("LE1.X80", "N");
        this.loanData.SetField("LE1.X81", "N");
        this.loanData.SetField("LE1.X82", "N");
        this.loanData.SetField("LE1.X83", "N");
        this.loanData.SetField("LE1.X84", "N");
        this.loanData.SetField("LE1.X85", string.Empty);
        this.loanData.SetField("3165", "//");
        this.loanData.SetField("3167", "//");
        this.loanData.SetField("3168", "N");
      }
      if (!this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD))
        return;
      this.Attributes.Add("ChangesCDReceivedDate", loanDataForRead.GetField("CD1.X62"));
      this.Attributes.Add("RevisedCDDueDate", loanDataForRead.GetField("CD1.X63"));
      this.addCOCFields(loanDataForRead);
      this.ChangeInCircumstance = loanDataForRead.GetField("CD1.X64");
      this.ChangeInCircumstanceComments = loanDataForRead.GetField("CD1.X65");
      this.ClosingDisclosure.IsChangeInAPR = loanDataForRead.GetField("CD1.X52") == "Y";
      this.ClosingDisclosure.IsChangeInLoanProduct = loanDataForRead.GetField("CD1.X53") == "Y";
      this.ClosingDisclosure.IsPrepaymentPenaltyAdded = loanDataForRead.GetField("CD1.X54") == "Y";
      this.ClosingDisclosure.IsChangeInSettlementCharges = loanDataForRead.GetField("CD1.X55") == "Y";
      this.ClosingDisclosure.Is24HourAdvancePreview = loanDataForRead.GetField("CD1.X56") == "Y";
      this.ClosingDisclosure.IsToleranceCure = loanDataForRead.GetField("CD1.X57") == "Y";
      this.ClosingDisclosure.IsClericalErrorCorrection = loanDataForRead.GetField("CD1.X58") == "Y";
      this.ClosingDisclosure.IsOther = loanDataForRead.GetField("CD1.X59") == "Y";
      this.ClosingDisclosure.IsChangedCircumstanceEligibility = loanDataForRead.GetField("CD1.X68") == "Y";
      this.ClosingDisclosure.IsRevisionsRequestedByConsumer = loanDataForRead.GetField("CD1.X66") == "Y";
      this.ClosingDisclosure.IsInterestRateDependentCharges = loanDataForRead.GetField("CD1.X67") == "Y";
      this.ClosingDisclosure.OtherDescription = loanDataForRead.GetField("CD1.X60");
      this.loanData.SetField("CD1.X64", string.Empty);
      this.loanData.SetField("CD1.X70", string.Empty);
      this.loanData.SetField("CD1.X65", string.Empty);
      this.loanData.SetField("CD1.X52", "N");
      this.loanData.SetField("CD1.X53", "N");
      this.loanData.SetField("CD1.X54", "N");
      this.loanData.SetField("CD1.X55", "N");
      this.loanData.SetField("CD1.X56", "N");
      this.loanData.SetField("CD1.X57", "N");
      this.loanData.SetField("CD1.X58", "N");
      this.loanData.SetField("CD1.X59", "N");
      this.loanData.SetField("CD1.X66", "N");
      this.loanData.SetField("CD1.X67", "N");
      this.loanData.SetField("CD1.X68", "N");
      this.loanData.SetField("CD1.X60", string.Empty);
      this.loanData.SetField("CD1.X62", "//");
      this.loanData.SetField("CD1.X63", "//");
      this.loanData.SetField("CD1.X61", "N");
      this.loanData.SetField("3171", "//");
      this.loanData.SetField("3172", string.Empty);
      this.loanData.SetField("3173", string.Empty);
    }

    private void addCOCFields(LoanData loanDataForRead)
    {
      this.Attributes.Add("XCOCChangedCircumstances", this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD) ? loanDataForRead.GetField("CD1.X61") : loanDataForRead.GetField("3168"));
      this.Attributes.Add("XCOCFeeLevelIndicator", loanDataForRead.GetField("4461"));
      if (!(loanDataForRead.GetField("4461") == "Y") || !this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD) && !this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE))
        return;
      int changeOfCircumstance = loanDataForRead.GetNumberOfGoodFaithChangeOfCircumstance();
      Dictionary<string, GFFVAlertTriggerField> gffAlertFieldList = changeOfCircumstance > 0 ? this.getGFFAlertFieldList(loanDataForRead) : (Dictionary<string, GFFVAlertTriggerField>) null;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      int num = 0;
      for (int index = 1; index <= changeOfCircumstance; ++index)
      {
        string str1 = "XCOC" + index.ToString("00");
        string field = loanDataForRead.GetField(str1 + "01");
        if (!string.IsNullOrEmpty(field))
        {
          foreach (string discloseCocField in DisclosureTracking2015Log.DiscloseCOCFields)
          {
            string str2 = discloseCocField.Substring(discloseCocField.Length - 2, 2);
            this.Attributes.Add(str1 + str2, loanDataForRead.GetField(str1 + str2));
          }
          string str3;
          string str4 = str3 = string.Empty;
          if (gffAlertFieldList.ContainsKey(field))
          {
            GFFVAlertTriggerField alertTriggerField = gffAlertFieldList[field];
            if (alertTriggerField != null)
            {
              str3 = alertTriggerField.Description;
              str4 = alertTriggerField.ItemizationValue;
            }
          }
          this.Attributes.Add(str1 + "_Description", str3);
          this.Attributes.Add(str1 + "_Amount", str4);
          ++num;
        }
      }
      this.Attributes.Add("XCOCcount", num.ToString());
    }

    private List<string> addTRFieldsForBorrowerpair(LoanData loanDataForRead, string borrowerPairId)
    {
      List<string> stringList = new List<string>();
      if (this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD) || this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE))
      {
        int additionalVestingParties = loanDataForRead.GetNumberOfAdditionalVestingParties();
        int num = 0;
        string empty = string.Empty;
        for (int index = 1; index <= additionalVestingParties; ++index)
        {
          string str1 = "TR" + index.ToString("00");
          if (string.Compare(loanDataForRead.GetField(str1 + "05"), borrowerPairId, true) == 0)
          {
            ++num;
            string str2 = "TR" + num.ToString("00");
            foreach (string disclosedVestingField in DisclosureTracking2015Log.DisclosedVestingFields)
            {
              string str3 = disclosedVestingField.Substring(disclosedVestingField.Length - 2, 2);
              string field = loanDataForRead.GetField(str1 + str3);
              this.Attributes.Add(str2 + str3, field);
              if (str3 == "99" && !string.IsNullOrWhiteSpace(field))
                stringList.Add(field);
            }
          }
        }
        this.Attributes.Add("TRcount", num.ToString());
      }
      return stringList;
    }

    private void addTRFieldsForNboIds(LoanData loanDataForRead, List<string> nboIds)
    {
      if (!this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD) && !this.Contents.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE))
        return;
      int additionalVestingParties = loanDataForRead.GetNumberOfAdditionalVestingParties();
      int num = 0;
      for (int index = 1; index <= additionalVestingParties; ++index)
      {
        string str1 = "TR" + index.ToString("00");
        string field1 = loanDataForRead.GetField(str1 + "99");
        if (nboIds.Contains(field1))
        {
          ++num;
          string str2 = "TR" + num.ToString("00");
          foreach (string disclosedVestingField in DisclosureTracking2015Log.DisclosedVestingFields)
          {
            string str3 = disclosedVestingField.Substring(disclosedVestingField.Length - 2, 2);
            string field2 = loanDataForRead.GetField(str1 + str3);
            this.Attributes.Add(str2 + str3, field2);
          }
        }
      }
      this.Attributes.Add("TRcount", num.ToString());
    }

    private Dictionary<string, GFFVAlertTriggerField> getGFFAlertFieldList(LoanData loanDataForRead)
    {
      Dictionary<string, GFFVAlertTriggerField> gffAlertFieldList = new Dictionary<string, GFFVAlertTriggerField>();
      Hashtable varianceAlertDetails = loanDataForRead.Calculator.GetGFFVarianceAlertDetails();
      if (varianceAlertDetails == null)
        return gffAlertFieldList;
      string empty = string.Empty;
      for (int index = 1; index <= 3; ++index)
      {
        string str;
        switch (index)
        {
          case 1:
            str = "Cannot Decrease";
            break;
          case 2:
            str = "Cannot Increase";
            break;
          default:
            str = "10% Variance";
            break;
        }
        string key = str;
        if (varianceAlertDetails.ContainsKey((object) key))
        {
          ArrayList arrayList = (ArrayList) varianceAlertDetails[(object) key];
          if (arrayList != null && arrayList.Count != 0)
          {
            foreach (GFFVAlertTriggerField alertTriggerField in arrayList)
            {
              if (!gffAlertFieldList.ContainsKey(alertTriggerField.FieldId))
                gffAlertFieldList.Add(alertTriggerField.FieldId, alertTriggerField);
            }
          }
        }
      }
      return gffAlertFieldList;
    }

    private List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> PopulateRecipients(
      List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> recipients,
      LoanData loanDataForRead)
    {
      List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> disclosureRecipientList1 = new List<EnhancedDisclosureTracking2015Log.DisclosureRecipient>();
      Dictionary<string, string> dictionary1;
      if (!recipients.Any<EnhancedDisclosureTracking2015Log.DisclosureRecipient>())
      {
        List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> disclosureRecipientList2 = disclosureRecipientList1;
        EnhancedDisclosureTracking2015Log.BorrowerRecipient borrowerRecipient = new EnhancedDisclosureTracking2015Log.BorrowerRecipient(this);
        borrowerRecipient.Id = System.Guid.NewGuid().ToString();
        borrowerRecipient.BorrowerPairId = loanDataForRead.CurrentBorrowerPair.Id;
        disclosureRecipientList2.Add((EnhancedDisclosureTracking2015Log.DisclosureRecipient) borrowerRecipient);
        System.Guid guid;
        if (EnhancedDisclosureTracking2015Log.IsCoborowerExist(loanDataForRead, loanDataForRead.CurrentBorrowerPair.Id))
        {
          List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> disclosureRecipientList3 = disclosureRecipientList1;
          EnhancedDisclosureTracking2015Log.CoborrowerRecipient coborrowerRecipient = new EnhancedDisclosureTracking2015Log.CoborrowerRecipient(this);
          guid = System.Guid.NewGuid();
          coborrowerRecipient.Id = guid.ToString();
          coborrowerRecipient.BorrowerPairId = loanDataForRead.CurrentBorrowerPair.Id;
          disclosureRecipientList3.Add((EnhancedDisclosureTracking2015Log.DisclosureRecipient) coborrowerRecipient);
        }
        List<string> nboIds = this.addTRFieldsForBorrowerpair(loanDataForRead, loanDataForRead.CurrentBorrowerPair.Id);
        foreach (string str in nboIds)
        {
          List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> disclosureRecipientList4 = disclosureRecipientList1;
          EnhancedDisclosureTracking2015Log.NBORecipient nboRecipient = new EnhancedDisclosureTracking2015Log.NBORecipient(this);
          guid = System.Guid.NewGuid();
          nboRecipient.Id = guid.ToString();
          nboRecipient.Guid = str;
          disclosureRecipientList4.Add((EnhancedDisclosureTracking2015Log.DisclosureRecipient) nboRecipient);
        }
        dictionary1 = this.addNBOFields(loanDataForRead, nboIds, false);
      }
      else
      {
        List<EnhancedDisclosureTracking2015Log.BorrowerRecipient> source = new List<EnhancedDisclosureTracking2015Log.BorrowerRecipient>();
        Dictionary<string, EnhancedDisclosureTracking2015Log.CoborrowerRecipient> dictionary2 = new Dictionary<string, EnhancedDisclosureTracking2015Log.CoborrowerRecipient>();
        List<EnhancedDisclosureTracking2015Log.NBORecipient> nboRecipientList = new List<EnhancedDisclosureTracking2015Log.NBORecipient>();
        List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> disclosureRecipientList5 = new List<EnhancedDisclosureTracking2015Log.DisclosureRecipient>();
        foreach (EnhancedDisclosureTracking2015Log.DisclosureRecipient recipient in recipients)
        {
          if (string.IsNullOrEmpty(recipient.Id))
            recipient.Id = System.Guid.NewGuid().ToString();
          switch (recipient.Role)
          {
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower:
              EnhancedDisclosureTracking2015Log.BorrowerRecipient borrower = recipient as EnhancedDisclosureTracking2015Log.BorrowerRecipient;
              if (source.Any<EnhancedDisclosureTracking2015Log.BorrowerRecipient>((Func<EnhancedDisclosureTracking2015Log.BorrowerRecipient, bool>) (brr => string.Equals(brr.BorrowerPairId, borrower.BorrowerPairId))))
                throw new DataValidationException("Multiple borrowers with BorrowerPairId: " + borrower.BorrowerPairId);
              source.Add(borrower);
              continue;
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower:
              EnhancedDisclosureTracking2015Log.CoborrowerRecipient coborrowerRecipient = recipient as EnhancedDisclosureTracking2015Log.CoborrowerRecipient;
              if (dictionary2.ContainsKey(coborrowerRecipient.BorrowerPairId))
                throw new DataValidationException("Multiple coborrowers with BorrowerPairId: " + coborrowerRecipient.BorrowerPairId);
              dictionary2.Add(coborrowerRecipient.BorrowerPairId, coborrowerRecipient);
              continue;
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner:
              nboRecipientList.Add(recipient as EnhancedDisclosureTracking2015Log.NBORecipient);
              continue;
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate:
            case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Other:
              disclosureRecipientList5.Add(recipient);
              continue;
            default:
              throw new Exception("Unknown DisclosureRecipientType " + (object) recipient.Role);
          }
        }
        System.Guid guid;
        if (!recipients.Any<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (x => x.Role.Equals((object) EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower) || x.Role.Equals((object) EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower))))
        {
          List<EnhancedDisclosureTracking2015Log.BorrowerRecipient> borrowerRecipientList = source;
          EnhancedDisclosureTracking2015Log.BorrowerRecipient borrowerRecipient = new EnhancedDisclosureTracking2015Log.BorrowerRecipient(this);
          guid = System.Guid.NewGuid();
          borrowerRecipient.Id = guid.ToString();
          borrowerRecipient.BorrowerPairId = loanDataForRead.CurrentBorrowerPair.Id;
          borrowerRecipientList.Add(borrowerRecipient);
        }
        foreach (EnhancedDisclosureTracking2015Log.BorrowerRecipient borrowerRecipient in source)
        {
          disclosureRecipientList1.Add((EnhancedDisclosureTracking2015Log.DisclosureRecipient) borrowerRecipient);
          borrowerRecipient.SetParent(this);
          EnhancedDisclosureTracking2015Log.CoborrowerRecipient coborrowerRecipient1;
          if (dictionary2.TryGetValue(borrowerRecipient.BorrowerPairId, out coborrowerRecipient1))
          {
            disclosureRecipientList1.Add((EnhancedDisclosureTracking2015Log.DisclosureRecipient) coborrowerRecipient1);
            coborrowerRecipient1.SetParent(this);
            dictionary2.Remove(borrowerRecipient.BorrowerPairId);
          }
          else if (EnhancedDisclosureTracking2015Log.IsCoborowerExist(loanDataForRead, borrowerRecipient.BorrowerPairId))
          {
            List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> disclosureRecipientList6 = disclosureRecipientList1;
            EnhancedDisclosureTracking2015Log.CoborrowerRecipient coborrowerRecipient2 = new EnhancedDisclosureTracking2015Log.CoborrowerRecipient(this);
            guid = System.Guid.NewGuid();
            coborrowerRecipient2.Id = guid.ToString();
            coborrowerRecipient2.BorrowerPairId = borrowerRecipient.BorrowerPairId;
            disclosureRecipientList6.Add((EnhancedDisclosureTracking2015Log.DisclosureRecipient) coborrowerRecipient2);
          }
        }
        foreach (string key in dictionary2.Keys)
        {
          EnhancedDisclosureTracking2015Log.BorrowerRecipient borrowerRecipient1 = new EnhancedDisclosureTracking2015Log.BorrowerRecipient(this);
          borrowerRecipient1.Id = System.Guid.NewGuid().ToString();
          borrowerRecipient1.BorrowerPairId = key;
          EnhancedDisclosureTracking2015Log.BorrowerRecipient borrowerRecipient2 = borrowerRecipient1;
          disclosureRecipientList1.Add((EnhancedDisclosureTracking2015Log.DisclosureRecipient) borrowerRecipient2);
          EnhancedDisclosureTracking2015Log.CoborrowerRecipient coborrowerRecipient = dictionary2[key];
          disclosureRecipientList1.Add((EnhancedDisclosureTracking2015Log.DisclosureRecipient) coborrowerRecipient);
          coborrowerRecipient.SetParent(this);
        }
        List<string> nboIds = new List<string>();
        foreach (EnhancedDisclosureTracking2015Log.NBORecipient nboRecipient in nboRecipientList)
        {
          if (string.IsNullOrEmpty(nboRecipient.Guid))
            throw new DataValidationException("Guid for NBO Recipient cannot be null");
          nboIds.Add(nboRecipient.Guid);
          disclosureRecipientList1.Add((EnhancedDisclosureTracking2015Log.DisclosureRecipient) nboRecipient);
          nboRecipient.SetParent(this);
        }
        this.addTRFieldsForNboIds(loanDataForRead, nboIds);
        dictionary1 = this.addNBOFields(loanDataForRead, nboIds, true);
        foreach (EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient in disclosureRecipientList5)
        {
          disclosureRecipientList1.Add(disclosureRecipient);
          disclosureRecipient.SetParent(this);
        }
      }
      foreach (EnhancedDisclosureTracking2015Log.DisclosureRecipient disclosureRecipient in disclosureRecipientList1)
      {
        switch (disclosureRecipient.Role)
        {
          case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower:
            EnhancedDisclosureTracking2015Log.BorrowerRecipient borrowerRecipient = disclosureRecipient as EnhancedDisclosureTracking2015Log.BorrowerRecipient;
            BorrowerPair borrowerPair1 = loanDataForRead.GetBorrowerPair(borrowerRecipient.BorrowerPairId);
            if (borrowerPair1 == null)
              throw new DataValidationException("Invalid BorrowerpairId: " + borrowerRecipient.BorrowerPairId);
            borrowerRecipient.BorrowerPairEntityId = !string.IsNullOrWhiteSpace(borrowerRecipient.BorrowerPairEntityId) ? borrowerRecipient.BorrowerPairEntityId : borrowerPair1.Borrower.EID;
            borrowerRecipient.RoleEntityId = !string.IsNullOrWhiteSpace(borrowerRecipient.RoleEntityId) ? borrowerRecipient.RoleEntityId : borrowerPair1.Borrower.EID;
            borrowerRecipient.Guid = !string.IsNullOrWhiteSpace(borrowerRecipient.Guid) ? borrowerRecipient.Guid : borrowerPair1.Borrower.Id;
            if (string.IsNullOrWhiteSpace(borrowerRecipient.Name))
              borrowerRecipient.Name = EnhancedDisclosureTracking2015Log.ConcatNonEmpty(loanDataForRead.GetField("36", borrowerPair1), loanDataForRead.GetField("37", borrowerPair1));
            borrowerRecipient.BorrowerType.ComputedValue = loanDataForRead.GetField("4008", borrowerPair1);
            if (string.IsNullOrWhiteSpace(borrowerRecipient.Name))
            {
              borrowerRecipient.Name = loanDataForRead.GetField("1868", borrowerPair1);
              if (string.IsNullOrWhiteSpace(borrowerRecipient.Name) && loanDataForRead.Calculator != null)
              {
                loanDataForRead.Calculator.FormCalculation("UPDATEBORROWERVESTINGNAME");
                borrowerRecipient.Name = loanDataForRead.GetField("1868", borrowerPair1);
              }
            }
            borrowerRecipient.Name = borrowerRecipient.Name.Trim();
            borrowerRecipient.FirstName = loanDataForRead.GetField("4000", borrowerPair1);
            borrowerRecipient.MiddleName = loanDataForRead.GetField("4001", borrowerPair1);
            borrowerRecipient.LastName = loanDataForRead.GetField("4002", borrowerPair1);
            borrowerRecipient.SuffixToName = loanDataForRead.GetField("4003", borrowerPair1);
            borrowerRecipient.MailingAddress = new EnhancedDisclosureTracking2015Log.Address();
            borrowerRecipient.MailingAddress.Street1 = loanDataForRead.GetField("1416", borrowerPair1);
            borrowerRecipient.MailingAddress.City = loanDataForRead.GetField("1417", borrowerPair1);
            borrowerRecipient.MailingAddress.State = loanDataForRead.GetField("1418", borrowerPair1);
            borrowerRecipient.MailingAddress.Zip = loanDataForRead.GetField("1419", borrowerPair1);
            borrowerRecipient.MailingAddress.ForeignAddressIndicator = Utils.ParseBoolean((object) loanDataForRead.GetField("URLA.X267", borrowerPair1));
            borrowerRecipient.MailingAddress.Country = loanDataForRead.GetField("URLA.X269", borrowerPair1);
            continue;
          case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower:
            EnhancedDisclosureTracking2015Log.CoborrowerRecipient coborrowerRecipient = disclosureRecipient as EnhancedDisclosureTracking2015Log.CoborrowerRecipient;
            BorrowerPair borrowerPair2 = loanDataForRead.GetBorrowerPair(coborrowerRecipient.BorrowerPairId);
            coborrowerRecipient.BorrowerPairEntityId = !string.IsNullOrWhiteSpace(coborrowerRecipient.BorrowerPairEntityId) ? coborrowerRecipient.BorrowerPairEntityId : borrowerPair2.Borrower.EID;
            coborrowerRecipient.RoleEntityId = !string.IsNullOrWhiteSpace(coborrowerRecipient.RoleEntityId) ? coborrowerRecipient.RoleEntityId : borrowerPair2.CoBorrower.EID;
            coborrowerRecipient.Guid = !string.IsNullOrWhiteSpace(coborrowerRecipient.Guid) ? coborrowerRecipient.Guid : borrowerPair2.CoBorrower.Id;
            if (string.IsNullOrWhiteSpace(coborrowerRecipient.Name))
              coborrowerRecipient.Name = EnhancedDisclosureTracking2015Log.ConcatNonEmpty(loanDataForRead.GetField("68", borrowerPair2), loanDataForRead.GetField("69", borrowerPair2));
            coborrowerRecipient.BorrowerType.ComputedValue = loanDataForRead.GetField("4009", borrowerPair2);
            if (string.IsNullOrWhiteSpace(coborrowerRecipient.Name))
            {
              coborrowerRecipient.Name = loanDataForRead.GetField("1873", borrowerPair2);
              if (string.IsNullOrWhiteSpace(coborrowerRecipient.Name) && loanDataForRead.Calculator != null)
              {
                loanDataForRead.Calculator.FormCalculation("UPDATECOBORROWERVESTINGNAME");
                coborrowerRecipient.Name = loanDataForRead.GetField("1873", borrowerPair2);
              }
            }
            coborrowerRecipient.Name = coborrowerRecipient.Name.Trim();
            coborrowerRecipient.FirstName = loanDataForRead.GetField("4004", borrowerPair2);
            coborrowerRecipient.MiddleName = loanDataForRead.GetField("4005", borrowerPair2);
            coborrowerRecipient.LastName = loanDataForRead.GetField("4006", borrowerPair2);
            coborrowerRecipient.SuffixToName = loanDataForRead.GetField("4007", borrowerPair2);
            coborrowerRecipient.MailingAddress = new EnhancedDisclosureTracking2015Log.Address();
            coborrowerRecipient.MailingAddress.Street1 = loanDataForRead.GetField("1519", borrowerPair2);
            coborrowerRecipient.MailingAddress.City = loanDataForRead.GetField("1520", borrowerPair2);
            coborrowerRecipient.MailingAddress.State = loanDataForRead.GetField("1521", borrowerPair2);
            coborrowerRecipient.MailingAddress.Zip = loanDataForRead.GetField("1522", borrowerPair2);
            coborrowerRecipient.MailingAddress.ForeignAddressIndicator = Utils.ParseBoolean((object) loanDataForRead.GetField("URLA.X268", borrowerPair2));
            coborrowerRecipient.MailingAddress.Country = loanDataForRead.GetField("URLA.X270", borrowerPair2);
            continue;
          case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner:
            EnhancedDisclosureTracking2015Log.NBORecipient nboRecipient = disclosureRecipient as EnhancedDisclosureTracking2015Log.NBORecipient;
            string str = dictionary1[nboRecipient.Guid];
            nboRecipient.RoleEntityId = !string.IsNullOrWhiteSpace(nboRecipient.RoleEntityId) ? nboRecipient.RoleEntityId : loanDataForRead.GetEidOfNonBorrowingOwnerContactAt(int.Parse(str.Substring(4)));
            if (string.IsNullOrWhiteSpace(nboRecipient.Name))
              nboRecipient.Name = EnhancedDisclosureTracking2015Log.ConcatNonEmpty(loanDataForRead.GetField(str + "01"), loanDataForRead.GetField(str + "02"), loanDataForRead.GetField(str + "03"), loanDataForRead.GetField(str + "04"));
            nboRecipient.BorrowerType.ComputedValue = loanDataForRead.GetField(str + "09");
            continue;
          default:
            continue;
        }
      }
      return disclosureRecipientList1;
    }

    private static bool IsCoborowerExist(LoanData loanDataForRead, string borrowerPairId)
    {
      BorrowerPair borrowerPair = loanDataForRead.GetBorrowerPair(borrowerPairId);
      return !string.IsNullOrWhiteSpace(loanDataForRead.GetField("4004", borrowerPair)) || !string.IsNullOrWhiteSpace(loanDataForRead.GetField("4005", borrowerPair)) || !string.IsNullOrWhiteSpace(loanDataForRead.GetField("4006", borrowerPair)) || !string.IsNullOrWhiteSpace(loanDataForRead.GetField("4007", borrowerPair));
    }

    private static string ConcatNonEmpty(params string[] strs)
    {
      return strs == null ? string.Empty : string.Join(" ", ((IEnumerable<string>) strs).Where<string>((Func<string, bool>) (str => !string.IsNullOrWhiteSpace(str)))).Trim();
    }

    private void populateAttributes(LoanData loanDataForRead)
    {
      this.PropertyAddress = new EnhancedDisclosureTracking2015Log.Address(loanDataForRead.GetField("12"), loanDataForRead.GetField("14"), loanDataForRead.GetField("11"), string.Empty, loanDataForRead.GetField("15"));
      this.LoanProgram = loanDataForRead.GetField("1401");
      this.LoanAmount = Utils.ParseDecimal((object) loanDataForRead.GetField("2"));
      this.DisclosedDailyInterest = new EnhancedDisclosureTracking2015Log.LockableField<string>(this, false, string.Empty, loanDataForRead.GetField("334"));
      this.DisclosedApr = new EnhancedDisclosureTracking2015Log.LockableField<string>(this, false, string.Empty, loanDataForRead.GetField("799"));
      this.DisclosedFinanceCharge = new EnhancedDisclosureTracking2015Log.LockableField<string>(this, false, string.Empty, loanDataForRead.GetField("1206"));
      this.ApplicationDate = Utils.ParseDate((object) loanDataForRead.GetField("3142"), DateTime.MinValue);
    }

    private DateTime GetDateFromAttributes(string key)
    {
      string s;
      DateTime result;
      return this.Attributes.TryGetValue(key, out s) && DateTime.TryParse(s, out result) ? Utils.TruncateDate(result) : DateTime.MinValue;
    }

    private void SetDateFromAttributes(string key, DateTime value)
    {
      value = Utils.TruncateDate(value);
      if (object.Equals((object) value, (object) DateTime.MinValue))
      {
        if (!this.Attributes.ContainsKey(key))
          return;
        this.Attributes.Remove(key);
      }
      else
        this.Attributes[key] = Utils.TruncateDate(value).ToString("yyyy-MM-dd");
    }

    private DisclosureTrackingFormItem.FormSignatureType EvaluateFormSignatureType(
      IList<DisclosureTrackingFormItem> forms,
      IList<EnhancedDisclosureTracking2015Log.DisclosureContentType> disclosureContentTypes,
      LoanData loanDataForRead)
    {
      bool flag1 = disclosureContentTypes.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.LE);
      bool flag2 = disclosureContentTypes.Contains(EnhancedDisclosureTracking2015Log.DisclosureContentType.CD);
      bool boolean = Utils.ParseBoolean((object) loanDataForRead.GetField("LE2.X28"));
      DisclosureTrackingFormItem trackingFormItem1 = (DisclosureTrackingFormItem) null;
      DisclosureTrackingFormItem trackingFormItem2 = (DisclosureTrackingFormItem) null;
      if (flag2)
      {
        trackingFormItem1 = forms.FirstOrDefault<DisclosureTrackingFormItem>((Func<DisclosureTrackingFormItem, bool>) (form => string.Equals(form.FormName, "Closing Disclosure", StringComparison.OrdinalIgnoreCase)));
        trackingFormItem2 = forms.FirstOrDefault<DisclosureTrackingFormItem>((Func<DisclosureTrackingFormItem, bool>) (form => string.Equals(form.FormName, "Closing Disclosure (Alternate)", StringComparison.OrdinalIgnoreCase)));
      }
      else if (flag1)
      {
        trackingFormItem1 = forms.FirstOrDefault<DisclosureTrackingFormItem>((Func<DisclosureTrackingFormItem, bool>) (form => string.Equals(form.FormName, "Loan Estimate", StringComparison.OrdinalIgnoreCase)));
        trackingFormItem2 = forms.FirstOrDefault<DisclosureTrackingFormItem>((Func<DisclosureTrackingFormItem, bool>) (form => string.Equals(form.FormName, "Loan Estimate (Alternate)", StringComparison.OrdinalIgnoreCase)));
      }
      if (trackingFormItem1 != null && trackingFormItem2 != null)
        return boolean ? trackingFormItem2.SignatureType : trackingFormItem1.SignatureType;
      if (trackingFormItem1 != null)
        return trackingFormItem1.SignatureType;
      return trackingFormItem2 != null ? trackingFormItem2.SignatureType : DisclosureTrackingFormItem.FormSignatureType.None;
    }

    private void CalculateLatestDisclosure2015()
    {
      if (this.initializingFromXml || !this.IsAttachedToLog || this.Log.Loan.Calculator == null)
        return;
      this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
    }

    private void ContentsChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.MarkAsDirty();
    }

    private void FulfillmentFieldsChangedEventHandler(
      object sender,
      NotifyCollectionChangedEventArgs e)
    {
      foreach (EnhancedDisclosureTracking2015Log.FulfillmentFields newItem in (IEnumerable) e.NewItems)
        newItem.SetParent(this);
    }

    public class Address
    {
      static Address()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.Address>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.Address>().ForMember<string>((Expression<Func<EnhancedDisclosureTracking2015Log.Address, string>>) (dt => dt.AddressFullName), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.Address>.ProfileOptions<string>>) (opts => opts.Ignore())));
      }

      public string City { get; set; }

      public string State { get; set; }

      public string Street1 { get; set; }

      public string Street2 { get; set; }

      public string Zip { get; set; }

      public bool ForeignAddressIndicator { get; set; }

      public string Country { get; set; }

      public string AddressFullName
      {
        get
        {
          string addressFullName = EnhancedDisclosureTracking2015Log.ConcatNonEmpty(this.Street1, this.Street2, this.City, this.State, this.Zip);
          if (this.ForeignAddressIndicator)
            addressFullName = EnhancedDisclosureTracking2015Log.ConcatNonEmpty(addressFullName, this.Country);
          return addressFullName;
        }
      }

      public Address(string city, string state, string street1, string street2, string zip)
      {
        this.City = city;
        this.State = state;
        this.Street1 = street1;
        this.Street2 = street2;
        this.Zip = zip;
      }

      public Address()
      {
      }
    }

    public enum DisclosureContentType
    {
      LE,
      CD,
      ServiceProviderList,
      ServiceProviderListNoFee,
      SafeHarbor,
    }

    public enum DT2015TrackingIndicators
    {
      ApplicationPackage,
      ApprovalPackage,
      LockPackage,
      ThreeDayPackage,
    }

    public enum DisclosureRecipientType
    {
      Borrower,
      CoBorrower,
      NonBorrowingOwner,
      LoanAssociate,
      Other,
    }

    public enum FulfillmentRecipientType
    {
      Borrower,
      CoBorrower,
      NonBorrowingOwner,
    }

    public class LockableField<T, C, U> where T : EnhancedDisclosureTracking2015Log.LockableField<T, C, U>
    {
      protected EnhancedDisclosureTracking2015Log _parent;
      private bool useUserValue;
      private U userValue;
      private C computedValue;

      public virtual bool UseUserValue
      {
        get => this.useUserValue;
        set
        {
          this.useUserValue = value;
          this._parent?.CalculateLatestDisclosure2015();
          this._parent?.MarkAsDirty();
        }
      }

      public virtual U UserValue
      {
        get => this.userValue;
        set
        {
          this.userValue = value;
          this._parent?.CalculateLatestDisclosure2015();
          this._parent?.MarkAsDirty();
        }
      }

      public virtual C ComputedValue
      {
        get => this.computedValue;
        set
        {
          this.computedValue = value;
          this._parent?.MarkAsDirty();
        }
      }

      public LockableField(EnhancedDisclosureTracking2015Log parent) => this._parent = parent;

      public LockableField(
        EnhancedDisclosureTracking2015Log parent,
        bool useUserValue,
        U userValue,
        C computedValue)
        : this(parent)
      {
        this.UseUserValue = useUserValue;
        this.UserValue = userValue;
        this.ComputedValue = computedValue;
      }

      public void SetParent(EnhancedDisclosureTracking2015Log parent) => this._parent = parent;

      public EnhancedDisclosureTracking2015Log GetParentLog() => this._parent;

      public virtual void CopyPropertiesTo(T other)
      {
        other.ComputedValue = this.ComputedValue;
        other.UserValue = this.UserValue;
        other.UseUserValue = this.UseUserValue;
      }
    }

    public class LockableField<T> : 
      EnhancedDisclosureTracking2015Log.LockableField<EnhancedDisclosureTracking2015Log.LockableField<T>, T, T>
    {
      static LockableField()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.LockableField<T>>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.LockableField<T>>().ForMember<T>((Expression<Func<EnhancedDisclosureTracking2015Log.LockableField<T>, T>>) (field => field.Value), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.LockableField<T>>.ProfileOptions<T>>) (opts => opts.Ignore())));
      }

      public virtual T Value => !this.UseUserValue ? this.ComputedValue : this.UserValue;

      public LockableField(EnhancedDisclosureTracking2015Log parent)
        : base(parent)
      {
      }

      public LockableField(
        EnhancedDisclosureTracking2015Log parent,
        bool useUserValue,
        T userValue,
        T computedValue)
        : base(parent, useUserValue, userValue, computedValue)
      {
      }
    }

    public class DisclosedDateField : 
      EnhancedDisclosureTracking2015Log.LockableField<EnhancedDisclosureTracking2015Log.DisclosedDateField, DateTimeWithZone, DateTime>
    {
      static DisclosedDateField()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.DisclosedDateField>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.DisclosedDateField>().ForMember<DateTimeWithZone>((Expression<Func<EnhancedDisclosureTracking2015Log.DisclosedDateField, DateTimeWithZone>>) (field => field.ComputedValue), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.DisclosedDateField>.ProfileOptions<DateTimeWithZone>>) (opts => opts.Ignore())).ForMember<DateTime>((Expression<Func<EnhancedDisclosureTracking2015Log.DisclosedDateField, DateTime>>) (field => field.Value), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.DisclosedDateField>.ProfileOptions<DateTime>>) (opts => opts.Ignore())));
      }

      public DisclosedDateField(EnhancedDisclosureTracking2015Log parent)
        : base(parent)
      {
        if (this._parent == null)
          return;
        this.ComputedValue = this._parent.ConvertToDateTimeWithZone(this._parent.Date);
      }

      public override DateTime UserValue
      {
        get => base.UserValue;
        set => base.UserValue = Utils.TruncateDate(value);
      }

      public override DateTimeWithZone ComputedValue
      {
        get => base.ComputedValue;
        set => base.ComputedValue = value;
      }

      public DateTime Value => !this.UseUserValue ? this.ComputedValue.DateTime : this.UserValue;

      public override void CopyPropertiesTo(
        EnhancedDisclosureTracking2015Log.DisclosedDateField other)
      {
        other.UserValue = this.UserValue;
      }
    }

    public class LockableDateOnlyField(EnhancedDisclosureTracking2015Log parent) : 
      EnhancedDisclosureTracking2015Log.LockableField<DateTime>(parent)
    {
      static LockableDateOnlyField()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.LockableDateOnlyField>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.LockableDateOnlyField>().InheritParentConfiguration());
      }

      public override DateTime UserValue
      {
        get => base.UserValue;
        set => base.UserValue = Utils.TruncateDate(value);
      }

      public override DateTime ComputedValue
      {
        get => base.ComputedValue;
        set => base.ComputedValue = Utils.TruncateDate(value);
      }
    }

    public class LockableUserRefField(EnhancedDisclosureTracking2015Log parent) : 
      EnhancedDisclosureTracking2015Log.LockableField<string>(parent)
    {
      private string computedName;

      static LockableUserRefField()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.LockableUserRefField>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.LockableUserRefField>().InheritParentConfiguration());
      }

      public virtual string ComputedName
      {
        get => this.computedName;
        set
        {
          this.computedName = value;
          this._parent?.MarkAsDirty();
        }
      }
    }

    public class TrackingFields
    {
      private readonly EnhancedDisclosureTracking2015Log _parent;
      private ObservableCollection<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators> indicators;

      public IList<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators> Indicators
      {
        get => (IList<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators>) this.indicators;
        set
        {
          if (this.indicators != null && this._parent != null)
            this.indicators.CollectionChanged -= new NotifyCollectionChangedEventHandler(this._parent.ContentsChangedEventHandler);
          if (value == null)
          {
            this.indicators = (ObservableCollection<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators>) null;
          }
          else
          {
            this.indicators = new ObservableCollection<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators>();
            if (this._parent != null)
              this.indicators.CollectionChanged += new NotifyCollectionChangedEventHandler(this._parent.ContentsChangedEventHandler);
            foreach (EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators trackingIndicators in (IEnumerable<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators>) value)
              this.indicators.Add(trackingIndicators);
          }
        }
      }

      public DateTimeWithZone PackageCreatedDate { get; set; }

      public string PackageId { get; set; }

      public string DocPackageId { get; set; }

      public TrackingFields(EnhancedDisclosureTracking2015Log parent)
      {
        this._parent = parent;
        this.Indicators = (IList<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators>) new List<EnhancedDisclosureTracking2015Log.DT2015TrackingIndicators>();
      }

      public EnhancedDisclosureTracking2015Log GetParentLog() => this._parent;
    }

    public class FulfillmentFields
    {
      private EnhancedDisclosureTracking2015Log _parent;
      private string orderedBy;

      static FulfillmentFields()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.FulfillmentFields>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.FulfillmentFields>().ForCollectionMember<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>((Expression<Func<EnhancedDisclosureTracking2015Log.FulfillmentFields, IList<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>>>) (dt => dt.Recipients), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.FulfillmentFields>.ProfileOptions<IList<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>, EnhancedDisclosureTracking2015Log.FulfillmentRecipient>>) (opts => opts.CreateItemUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log.FulfillmentFields, EnhancedDisclosureTracking2015Log.FulfillmentRecipient>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.FulfillmentRecipient(parent._parent))))));
      }

      public bool IsManual { get; set; }

      public string Id { get; set; }

      public DisclosureTrackingBase.DisclosedMethod DisclosedMethod { get; set; }

      public IList<EnhancedDisclosureTracking2015Log.FulfillmentRecipient> Recipients { get; set; }

      public string OrderedBy
      {
        get => this.orderedBy;
        set
        {
          this.orderedBy = value;
          this._parent?.MarkAsDirty();
        }
      }

      public DateTimeWithZone ProcessedDate { get; set; }

      public string TrackingNumber { get; set; }

      public EnhancedDisclosureTracking2015Log.FulfillmentRecipient GetFirstRecipient()
      {
        return this.Recipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>();
      }

      public FulfillmentFields()
      {
        this.Recipients = (IList<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>) new List<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>();
      }

      public FulfillmentFields(EnhancedDisclosureTracking2015Log parent)
        : this()
      {
        this._parent = parent;
      }

      public void SetParent(EnhancedDisclosureTracking2015Log parent) => this._parent = parent;

      public EnhancedDisclosureTracking2015Log GetParentLog() => this._parent;
    }

    public class FulfillmentRecipient
    {
      private EnhancedDisclosureTracking2015Log _parent;

      public FulfillmentRecipient(EnhancedDisclosureTracking2015Log parent)
      {
        this._parent = parent;
      }

      public string Id { get; set; }

      public DateTimeWithZone PresumedDate { get; set; }

      public DateTimeWithZone ActualDate { get; set; }

      public string Comments { get; set; }

      public EnhancedDisclosureTracking2015Log GetParentLog() => this._parent;
    }

    public enum FulfillmentDisclosedMethodType
    {
      InPerson,
      ByMail,
    }

    public class DisclosureTrackingDocuments
    {
      public IList<DisclosureTrackingFormItem> Forms { get; set; }

      public string ViewableFormsFile { get; set; }

      public DisclosureTrackingFormItem.FormSignatureType SignatureType { get; set; }

      public DisclosureTrackingDocuments()
      {
        this.Forms = (IList<DisclosureTrackingFormItem>) new List<DisclosureTrackingFormItem>();
      }

      public DisclosureTrackingDocuments(
        IEnumerable<DisclosureTrackingFormItem> forms,
        DisclosureTrackingFormItem.FormSignatureType signatureType)
      {
        this.Forms = (IList<DisclosureTrackingFormItem>) new List<DisclosureTrackingFormItem>(forms);
      }
    }

    public class LoanEstimateFields
    {
      private readonly EnhancedDisclosureTracking2015Log _parent;
      private bool isDisclosedByBroker;

      static LoanEstimateFields()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.LoanEstimateFields>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.LoanEstimateFields>().ForMember<DateTime>((Expression<Func<EnhancedDisclosureTracking2015Log.LoanEstimateFields, DateTime>>) (r => r.ChangesReceivedDate), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.LoanEstimateFields>.ProfileOptions<DateTime>>) (opts => opts.Ignore())).ForMember<DateTime>((Expression<Func<EnhancedDisclosureTracking2015Log.LoanEstimateFields, DateTime>>) (r => r.RevisedDueDate), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.LoanEstimateFields>.ProfileOptions<DateTime>>) (opts => opts.Ignore())));
      }

      public bool IsDisclosedByBroker
      {
        get => this.isDisclosedByBroker;
        set
        {
          this.isDisclosedByBroker = value;
          this._parent?.MarkAsDirty();
        }
      }

      public bool IsChangedCircumstanceSettlementCharges { get; set; }

      public bool IsChangedCircumstanceEligibility { get; set; }

      public bool IsRevisionsRequestedByConsumer { get; set; }

      public bool IsInterestRateDependentCharges { get; set; }

      public bool IsExpiration { get; set; }

      public bool IsDelayedSettlementOnConstructionLoans { get; set; }

      public bool IsOther { get; set; }

      public string OtherDescription { get; set; }

      public DateTime ChangesReceivedDate
      {
        get => this._parent.GetDateFromAttributes("ChangesLEReceivedDate");
        set => this._parent.SetDateFromAttributes("ChangesLEReceivedDate", value);
      }

      public DateTime RevisedDueDate
      {
        get => this._parent.GetDateFromAttributes("RevisedLEDueDate");
        set => this._parent.SetDateFromAttributes("RevisedLEDueDate", value);
      }

      public LoanEstimateFields(EnhancedDisclosureTracking2015Log parent) => this._parent = parent;
    }

    public class ClosingDisclosureFields
    {
      private readonly EnhancedDisclosureTracking2015Log _parent;

      static ClosingDisclosureFields()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.ClosingDisclosureFields>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.ClosingDisclosureFields>().ForMember<DateTime>((Expression<Func<EnhancedDisclosureTracking2015Log.ClosingDisclosureFields, DateTime>>) (r => r.ChangesReceivedDate), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.ClosingDisclosureFields>.ProfileOptions<DateTime>>) (opts => opts.Ignore())).ForMember<DateTime>((Expression<Func<EnhancedDisclosureTracking2015Log.ClosingDisclosureFields, DateTime>>) (r => r.RevisedDueDate), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.ClosingDisclosureFields>.ProfileOptions<DateTime>>) (opts => opts.Ignore())));
      }

      public bool IsChangeInAPR { get; set; }

      public bool IsChangeInLoanProduct { get; set; }

      public bool IsPrepaymentPenaltyAdded { get; set; }

      public bool IsChangeInSettlementCharges { get; set; }

      public bool Is24HourAdvancePreview { get; set; }

      public bool IsToleranceCure { get; set; }

      public bool IsClericalErrorCorrection { get; set; }

      public bool IsChangedCircumstanceEligibility { get; set; }

      public bool IsInterestRateDependentCharges { get; set; }

      public bool IsRevisionsRequestedByConsumer { get; set; }

      public bool IsOther { get; set; }

      public string OtherDescription { get; set; }

      public DateTime ChangesReceivedDate
      {
        get => this._parent.GetDateFromAttributes("ChangesCDReceivedDate");
        set => this._parent.SetDateFromAttributes("ChangesCDReceivedDate", value);
      }

      public DateTime RevisedDueDate
      {
        get => this._parent.GetDateFromAttributes("RevisedCDDueDate");
        set => this._parent.SetDateFromAttributes("RevisedCDDueDate", value);
      }

      public ClosingDisclosureFields(EnhancedDisclosureTracking2015Log parent)
      {
        this._parent = parent;
      }
    }

    public class IntentToProceedFields
    {
      internal readonly EnhancedDisclosureTracking2015Log _parent;
      private bool intent;
      private DateTime _date;

      static IntentToProceedFields()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.IntentToProceedFields>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.IntentToProceedFields>().ForMember<EnhancedDisclosureTracking2015Log.LockableField<string>>((Expression<Func<EnhancedDisclosureTracking2015Log.IntentToProceedFields, EnhancedDisclosureTracking2015Log.LockableField<string>>>) (x => x.ReceivedBy), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.IntentToProceedFields>.ProfileOptions<EnhancedDisclosureTracking2015Log.LockableField<string>>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log.IntentToProceedFields, EnhancedDisclosureTracking2015Log.LockableField<string>>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.LockableField<string>(parent._parent))))));
      }

      public bool Intent
      {
        get => this.intent;
        set
        {
          this.intent = value;
          this._parent?.CalculateLatestDisclosure2015();
          this._parent?.MarkAsDirty();
        }
      }

      public DateTime Date
      {
        get => this._date;
        set => this._date = Utils.TruncateDate(value);
      }

      public EnhancedDisclosureTracking2015Log.LockableField<string> ReceivedBy { get; set; }

      public DisclosureTrackingBase.DisclosedMethod ReceivedMethod { get; set; }

      public string ReceivedMethodOther { get; set; }

      public string Comments { get; set; }

      public IntentToProceedFields(EnhancedDisclosureTracking2015Log parent)
      {
        this._parent = parent;
        this.ReceivedBy = new EnhancedDisclosureTracking2015Log.LockableField<string>(parent);
      }
    }

    public class DisclosureRecipient
    {
      internal EnhancedDisclosureTracking2015Log _parent;
      private DateTime _actualReceivedDate;

      static DisclosureRecipient()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.DisclosureRecipient>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.DisclosureRecipient>().ForMember<EnhancedDisclosureTracking2015Log.LockableDateOnlyField>((Expression<Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, EnhancedDisclosureTracking2015Log.LockableDateOnlyField>>) (x => x.PresumedReceivedDate), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.DisclosureRecipient>.ProfileOptions<EnhancedDisclosureTracking2015Log.LockableDateOnlyField>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log.DisclosureRecipient, EnhancedDisclosureTracking2015Log.LockableDateOnlyField>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.LockableDateOnlyField(parent._parent))))).ForMember<EnhancedDisclosureTracking2015Log.LockableField<string>>((Expression<Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, EnhancedDisclosureTracking2015Log.LockableField<string>>>) (x => x.BorrowerType), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.DisclosureRecipient>.ProfileOptions<EnhancedDisclosureTracking2015Log.LockableField<string>>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log.DisclosureRecipient, EnhancedDisclosureTracking2015Log.LockableField<string>>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.LockableField<string>(parent._parent))))).ForMember<EnhancedDisclosureTracking2015Log.DisclosureRecipientTracking>((Expression<Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, EnhancedDisclosureTracking2015Log.DisclosureRecipientTracking>>) (x => x.Tracking), (Action<XmlAutoMapper.Profile<EnhancedDisclosureTracking2015Log.DisclosureRecipient>.ProfileOptions<EnhancedDisclosureTracking2015Log.DisclosureRecipientTracking>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, EnhancedDisclosureTracking2015Log.DisclosureRecipient, EnhancedDisclosureTracking2015Log.DisclosureRecipientTracking>) ((xmlElement, parent) => new EnhancedDisclosureTracking2015Log.DisclosureRecipientTracking(parent._parent))))));
      }

      public string Id { get; set; }

      public string Name { get; set; }

      public string Email { get; set; }

      public string RoleEntityId { get; set; }

      public EnhancedDisclosureTracking2015Log.DisclosureRecipientType Role { get; set; }

      public string RoleDescription { get; set; }

      public DisclosureTrackingBase.DisclosedMethod DisclosedMethod { get; set; }

      public string DisclosedMethodDescription { get; set; }

      public EnhancedDisclosureTracking2015Log.LockableField<string> BorrowerType { get; set; }

      public EnhancedDisclosureTracking2015Log.LockableDateOnlyField PresumedReceivedDate { get; set; }

      public DateTime ActualReceivedDate
      {
        get => this._actualReceivedDate;
        set
        {
          this._actualReceivedDate = Utils.TruncateDate(value);
          this._parent?.CalculateLatestDisclosure2015();
          this._parent?.MarkAsDirty();
        }
      }

      public string UserId { get; set; }

      public EnhancedDisclosureTracking2015Log.DisclosureRecipientTracking Tracking { get; set; }

      public EnhancedDisclosureTracking2015Log GetParent() => this._parent;

      public DisclosureRecipient()
        : this((EnhancedDisclosureTracking2015Log) null)
      {
      }

      public DisclosureRecipient(EnhancedDisclosureTracking2015Log parent)
      {
        this._parent = parent;
        this.Role = EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Other;
        this.PresumedReceivedDate = new EnhancedDisclosureTracking2015Log.LockableDateOnlyField(this._parent);
        this.BorrowerType = new EnhancedDisclosureTracking2015Log.LockableField<string>(this._parent);
        this.Tracking = new EnhancedDisclosureTracking2015Log.DisclosureRecipientTracking(this._parent);
      }

      public DisclosureRecipient(
        EnhancedDisclosureTracking2015Log.DisclosureRecipientType role)
        : this()
      {
        this.Role = role;
      }

      public DisclosureRecipient(
        EnhancedDisclosureTracking2015Log parent,
        EnhancedDisclosureTracking2015Log.DisclosureRecipientType role)
        : this(parent)
      {
        this.Role = role;
      }

      public void SetParent(EnhancedDisclosureTracking2015Log parent)
      {
        this._parent = parent;
        this.PresumedReceivedDate.SetParent(parent);
        this.BorrowerType.SetParent(parent);
        this.Tracking.SetParent(parent);
      }

      public EnhancedDisclosureTracking2015Log.DisclosureRecipient Clone()
      {
        EnhancedDisclosureTracking2015Log.DisclosureRecipient other = EnhancedDisclosureTracking2015Log.DisclosureRecipient.CreateNew(this.Role, this._parent);
        this.CopyPropertiesTo(other);
        return other;
      }

      public void CopyPropertiesTo(
        EnhancedDisclosureTracking2015Log.DisclosureRecipient other)
      {
        other.Id = this.Id;
        other.Name = this.Name;
        other.Email = this.Email;
        other.RoleDescription = this.RoleDescription;
        other.RoleEntityId = this.RoleEntityId;
        other.DisclosedMethod = this.DisclosedMethod;
        other.DisclosedMethodDescription = this.DisclosedMethodDescription;
        this.BorrowerType.CopyPropertiesTo(other.BorrowerType);
        this.PresumedReceivedDate.CopyPropertiesTo((EnhancedDisclosureTracking2015Log.LockableField<DateTime>) other.PresumedReceivedDate);
        other.ActualReceivedDate = this.ActualReceivedDate;
        other.UserId = this.UserId;
        this.Tracking.CopyPropertiesTo(other.Tracking);
      }

      public static EnhancedDisclosureTracking2015Log.DisclosureRecipient CreateNew(
        EnhancedDisclosureTracking2015Log.DisclosureRecipientType role,
        EnhancedDisclosureTracking2015Log parent)
      {
        switch (role)
        {
          case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower:
            return (EnhancedDisclosureTracking2015Log.DisclosureRecipient) new EnhancedDisclosureTracking2015Log.BorrowerRecipient(parent);
          case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower:
            return (EnhancedDisclosureTracking2015Log.DisclosureRecipient) new EnhancedDisclosureTracking2015Log.CoborrowerRecipient(parent);
          case EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner:
            return (EnhancedDisclosureTracking2015Log.DisclosureRecipient) new EnhancedDisclosureTracking2015Log.NBORecipient(parent);
          default:
            return new EnhancedDisclosureTracking2015Log.DisclosureRecipient(parent, role);
        }
      }
    }

    public class NBORecipient : EnhancedDisclosureTracking2015Log.DisclosureRecipient
    {
      static NBORecipient()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.NBORecipient>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.NBORecipient>().InheritParentConfiguration());
      }

      public string Guid { get; set; }

      public NBORecipient()
        : base(EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner)
      {
      }

      public NBORecipient(EnhancedDisclosureTracking2015Log parent)
        : base(parent, EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner)
      {
      }
    }

    public class BorrowerRecipient : EnhancedDisclosureTracking2015Log.DisclosureRecipient
    {
      static BorrowerRecipient()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.BorrowerRecipient>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.BorrowerRecipient>().InheritParentConfiguration());
      }

      public string Guid { get; set; }

      public string BorrowerPairEntityId { get; set; }

      public string BorrowerPairId { get; set; }

      public string FirstName { get; set; }

      public string MiddleName { get; set; }

      public string LastName { get; set; }

      public string SuffixToName { get; set; }

      public EnhancedDisclosureTracking2015Log.Address MailingAddress { get; set; }

      public BorrowerRecipient()
        : base(EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)
      {
      }

      public BorrowerRecipient(EnhancedDisclosureTracking2015Log parent)
        : base(parent, EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)
      {
      }
    }

    public class CoborrowerRecipient : EnhancedDisclosureTracking2015Log.DisclosureRecipient
    {
      static CoborrowerRecipient()
      {
        XmlAutoMapper.AddProfile<EnhancedDisclosureTracking2015Log.CoborrowerRecipient>(XmlAutoMapper.NewProfile<EnhancedDisclosureTracking2015Log.CoborrowerRecipient>().InheritParentConfiguration());
      }

      public string Guid { get; set; }

      public string BorrowerPairEntityId { get; set; }

      public string BorrowerPairId { get; set; }

      public string FirstName { get; set; }

      public string MiddleName { get; set; }

      public string LastName { get; set; }

      public string SuffixToName { get; set; }

      public EnhancedDisclosureTracking2015Log.Address MailingAddress { get; set; }

      public CoborrowerRecipient()
        : base(EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower)
      {
      }

      public CoborrowerRecipient(EnhancedDisclosureTracking2015Log parent)
        : base(parent, EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower)
      {
      }
    }

    public class DisclosureRecipientTracking
    {
      protected EnhancedDisclosureTracking2015Log _parent;

      public DateTimeWithZone AcceptConsentDate { get; set; }

      public DateTimeWithZone ESignedDate { get; set; }

      public DateTimeWithZone WetSignedDate { get; set; }

      public DateTimeWithZone RejectConsentDate { get; set; }

      public DateTimeWithZone ViewConsentDate { get; set; }

      public DateTimeWithZone ViewMessageDate { get; set; }

      public DateTimeWithZone AuthenticatedDate { get; set; }

      public string AuthenticatedIP { get; set; }

      public string AcceptConsentIP { get; set; }

      public string RejectConsentIP { get; set; }

      public string ESignedIP { get; set; }

      public string LoanLevelConsent { get; set; }

      public DateTimeWithZone ViewESignedDate { get; set; }

      public DateTimeWithZone ViewWetSignedDate { get; set; }

      public DateTimeWithZone InformationalViewedDate { get; set; }

      public string InformationalViewedIP { get; set; }

      public DateTimeWithZone InformationalCompletedDate { get; set; }

      public string InformationalCompletedIP { get; set; }

      public DisclosureRecipientTracking(EnhancedDisclosureTracking2015Log parent)
      {
        this._parent = parent;
      }

      public void SetParent(EnhancedDisclosureTracking2015Log parent) => this._parent = parent;

      public EnhancedDisclosureTracking2015Log GetParentLog() => this._parent;

      public void CopyPropertiesTo(
        EnhancedDisclosureTracking2015Log.DisclosureRecipientTracking other)
      {
        other.AcceptConsentDate = this.AcceptConsentDate;
        other.ESignedDate = this.ESignedDate;
        other.WetSignedDate = this.WetSignedDate;
        other.RejectConsentDate = this.RejectConsentDate;
        other.ViewConsentDate = this.ViewConsentDate;
        other.ViewMessageDate = this.ViewMessageDate;
        other.AuthenticatedDate = this.AuthenticatedDate;
        other.AuthenticatedIP = this.AuthenticatedIP;
        other.AcceptConsentIP = this.AcceptConsentIP;
        other.RejectConsentIP = this.RejectConsentIP;
        other.ESignedIP = this.ESignedIP;
        other.LoanLevelConsent = this.LoanLevelConsent;
        other.ViewESignedDate = this.ViewESignedDate;
        other.ViewWetSignedDate = this.ViewWetSignedDate;
        other.InformationalViewedDate = this.InformationalViewedDate;
        other.InformationalViewedIP = this.InformationalViewedIP;
        other.InformationalCompletedDate = this.InformationalCompletedDate;
        other.InformationalCompletedIP = this.InformationalCompletedIP;
      }
    }

    public class NonBorrowerOwnerItem : INonBorrowerOwnerItem
    {
      public string FirstName { get; }

      public string MidName { get; }

      public string LastName { get; }

      public string Suffix { get; }

      public string Address { get; }

      public string City { get; }

      public string State { get; }

      public string Zip { get; }

      public string VestingType { get; }

      public string HomePhone { get; }

      public string Email { get; }

      public bool IsNoThirdPartyEmail { get; }

      public string BusiPhone { get; }

      public string Cell { get; }

      public string Fax { get; }

      public DateTime DOB { get; }

      public string TRGuid { get; set; }

      public string OrderID { get; }

      public DateTime ActualReceivedDate
      {
        get => this.Recipient.ActualReceivedDate;
        set => this.Recipient.ActualReceivedDate = value;
      }

      public string BorrowerType
      {
        get => this.Recipient.BorrowerType.ComputedValue;
        set => this.Recipient.BorrowerType.ComputedValue = value;
      }

      public int DisclosedMethod
      {
        get => (int) this.Recipient.DisclosedMethod;
        set => this.Recipient.DisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) value;
      }

      public string DisclosedMethodOther
      {
        get => this.Recipient.DisclosedMethodDescription;
        set => this.Recipient.DisclosedMethodDescription = value;
      }

      public DateTime PresumedReceivedDate
      {
        get => this.Recipient.PresumedReceivedDate.ComputedValue;
        set => this.Recipient.PresumedReceivedDate.ComputedValue = value;
      }

      public DateTime lockedPresumedReceivedDate
      {
        get => this.Recipient.PresumedReceivedDate.UserValue;
        set => this.Recipient.PresumedReceivedDate.UserValue = value;
      }

      public bool isPresumedDateLocked
      {
        get => this.Recipient.PresumedReceivedDate.UseUserValue;
        set => this.Recipient.PresumedReceivedDate.UseUserValue = value;
      }

      public bool isBorrowerTypeLocked
      {
        get => this.Recipient.BorrowerType.UseUserValue;
        set => this.Recipient.BorrowerType.UseUserValue = value;
      }

      public string LockedBorrowerType
      {
        get => this.Recipient.BorrowerType.UserValue;
        set => this.Recipient.BorrowerType.UserValue = value;
      }

      public DateTime eDisclosureNBOAuthenticatedDate
      {
        get => this.Recipient.Tracking.AuthenticatedDate.DateTime;
        set
        {
          this.Recipient.Tracking.AuthenticatedDate = this.Recipient.GetParent().CreateDateTimeWithZone(value);
        }
      }

      public string eDisclosureNBOAuthenticatedIP
      {
        get => this.Recipient.Tracking.AuthenticatedIP;
        set => this.Recipient.Tracking.AuthenticatedIP = value;
      }

      public string eDisclosureNBOeSignedIP
      {
        get => this.Recipient.Tracking.ESignedIP;
        set => this.Recipient.Tracking.ESignedIP = value;
      }

      public DateTime eDisclosureNBORejectConsentDate
      {
        get => this.Recipient.Tracking.RejectConsentDate.DateTime;
        set
        {
          this.Recipient.Tracking.RejectConsentDate = this.Recipient.GetParent().CreateDateTimeWithZone(value);
        }
      }

      public string eDisclosureNBORejectConsentIP
      {
        get => this.Recipient.Tracking.RejectConsentIP;
        set => this.Recipient.Tracking.RejectConsentIP = value;
      }

      public DateTime eDisclosureNBOSignedDate
      {
        get => this.Recipient.Tracking.ESignedDate.DateTime;
        set
        {
          this.Recipient.Tracking.ESignedDate = this.Recipient.GetParent().CreateDateTimeWithZone(value);
        }
      }

      public DateTime eDisclosureNBOViewMessageDate
      {
        get => this.Recipient.Tracking.ViewMessageDate.DateTime;
        set
        {
          this.Recipient.Tracking.ViewMessageDate = this.Recipient.GetParent().CreateDateTimeWithZone(value);
        }
      }

      public string eDisclosureNBOLoanLevelConsent
      {
        get => this.Recipient.Tracking.LoanLevelConsent;
        set => this.Recipient.Tracking.LoanLevelConsent = value;
      }

      public DateTime eDisclosureNBOAcceptConsentDate
      {
        get => this.Recipient.Tracking.AcceptConsentDate.DateTime;
        set
        {
          this.Recipient.Tracking.AcceptConsentDate = this.Recipient.GetParent().CreateDateTimeWithZone(value);
        }
      }

      public string eDisclosureNBOAcceptConsentIP
      {
        get => this.Recipient.Tracking.AcceptConsentIP;
        set => this.Recipient.Tracking.AcceptConsentIP = value;
      }

      public DateTime eDisclosureNBODocumentViewedDate
      {
        get
        {
          if (this.DTLog.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.WetSignature)
            return this.Recipient.Tracking.ViewWetSignedDate.DateTime;
          if (this.DTLog.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.eSignature)
            return this.Recipient.Tracking.ViewESignedDate.DateTime;
          return this.DTLog.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.Informational ? this.Recipient.Tracking.InformationalViewedDate.DateTime : new DateTime();
        }
        set
        {
          if (this.DTLog.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.WetSignature)
            this.Recipient.Tracking.ViewWetSignedDate = this.Recipient.GetParent().CreateDateTimeWithZone(value);
          else if (this.DTLog.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.eSignature)
          {
            this.Recipient.Tracking.ViewESignedDate = this.Recipient.GetParent().CreateDateTimeWithZone(value);
          }
          else
          {
            if (this.DTLog.Documents.SignatureType != DisclosureTrackingFormItem.FormSignatureType.Informational)
              return;
            this.Recipient.Tracking.InformationalViewedDate = this.Recipient.GetParent().CreateDateTimeWithZone(value);
          }
        }
      }

      public bool eDisclosureNBOeSignatures
      {
        get
        {
          return this.DTLog.Documents.SignatureType == DisclosureTrackingFormItem.FormSignatureType.eSignature;
        }
        set
        {
        }
      }

      public IDisclosureTracking2015Log DTLog { get; }

      public EnhancedDisclosureTracking2015Log.NBORecipient Recipient { get; }

      public INonBorrowerOwnerItem CloneForDuplicate()
      {
        return (INonBorrowerOwnerItem) new EnhancedDisclosureTracking2015Log.NonBorrowerOwnerItem(this.FirstName, this.MidName, this.LastName, this.Suffix, this.Address, this.City, this.State, this.Zip, this.VestingType, this.HomePhone, this.Email, this.IsNoThirdPartyEmail, this.BusiPhone, this.Cell, this.Fax, this.DOB, this.TRGuid, this.OrderID, this.DTLog, this.Recipient)
        {
          DisclosedMethod = this.DisclosedMethod,
          DisclosedMethodOther = this.DisclosedMethodOther,
          PresumedReceivedDate = this.PresumedReceivedDate,
          lockedPresumedReceivedDate = this.lockedPresumedReceivedDate,
          isPresumedDateLocked = this.isPresumedDateLocked,
          ActualReceivedDate = this.ActualReceivedDate,
          isBorrowerTypeLocked = this.isBorrowerTypeLocked,
          BorrowerType = this.BorrowerType,
          LockedBorrowerType = this.LockedBorrowerType,
          TRGuid = this.TRGuid,
          eDisclosureNBOAuthenticatedDate = this.eDisclosureNBOAuthenticatedDate,
          eDisclosureNBOAuthenticatedIP = this.eDisclosureNBOAuthenticatedIP,
          eDisclosureNBOViewMessageDate = this.eDisclosureNBOViewMessageDate,
          eDisclosureNBORejectConsentDate = this.eDisclosureNBORejectConsentDate,
          eDisclosureNBORejectConsentIP = this.eDisclosureNBORejectConsentIP,
          eDisclosureNBOSignedDate = this.eDisclosureNBOSignedDate,
          eDisclosureNBOeSignedIP = this.eDisclosureNBOeSignedIP,
          eDisclosureNBOAcceptConsentDate = this.eDisclosureNBOAcceptConsentDate,
          eDisclosureNBOAcceptConsentIP = this.eDisclosureNBOAcceptConsentIP,
          eDisclosureNBODocumentViewedDate = this.eDisclosureNBODocumentViewedDate,
          eDisclosureNBOLoanLevelConsent = this.eDisclosureNBOLoanLevelConsent,
          eDisclosureNBOeSignatures = this.eDisclosureNBOeSignatures
        };
      }

      public NonBorrowerOwnerItem(
        string FName,
        string MName,
        string LName,
        string Suffix,
        string Address,
        string City,
        string State,
        string Zip,
        string VestingType,
        string HomePhone,
        string Email,
        bool IsNoThirdPartyEmail,
        string BusiPhone,
        string Cell,
        string Fax,
        DateTime DOB,
        string TRGuid,
        string InstanceId,
        IDisclosureTracking2015Log parentLog,
        EnhancedDisclosureTracking2015Log.NBORecipient recipient)
      {
        this.FirstName = FName;
        this.MidName = MName;
        this.LastName = LName;
        this.Suffix = Suffix;
        this.Address = Address;
        this.City = City;
        this.State = State;
        this.Zip = Zip;
        this.VestingType = VestingType;
        this.HomePhone = HomePhone;
        this.Email = Email;
        this.IsNoThirdPartyEmail = IsNoThirdPartyEmail;
        this.BusiPhone = BusiPhone;
        this.Cell = Cell;
        this.Fax = Fax;
        this.DOB = DOB;
        this.TRGuid = TRGuid;
        this.OrderID = InstanceId;
        this.DTLog = parentLog;
        this.Recipient = recipient;
      }
    }
  }
}
