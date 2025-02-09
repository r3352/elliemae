// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.LockConfirmLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class LockConfirmLog : LogRecordBase
  {
    public static readonly string XmlType = "LockConfirm";
    private string dateConfirmed = string.Empty;
    private string confirmedBy = string.Empty;
    private string confirmedByFullName = string.Empty;
    private DateTime buySideExpirationDate = DateTime.MinValue;
    private DateTime sellSideExpirationDate = DateTime.MinValue;
    private DateTime sellSideDeliveryDate = DateTime.MinValue;
    private string sellSideDeliveredBy = string.Empty;
    private string requestGUID = string.Empty;
    private bool alertLoanOfficer;
    public static ArrayList BuySideLockFields = new ArrayList();
    private bool commitmentTermEnabled;
    private bool enableZeroParPricingWholesale;
    private bool enableZeroParPricingRetail;
    private bool hideLog;
    private bool isVoided;
    private bool includeConfirmCnt = true;

    public LockConfirmLog()
    {
    }

    static LockConfirmLog()
    {
      for (int index = 2148; index <= 2205; ++index)
        LockConfirmLog.BuySideLockFields.Add((object) index.ToString());
      LockConfirmLog.BuySideLockFields.Add((object) "2215");
      LockConfirmLog.BuySideLockFields.Add((object) "3256");
    }

    public LockConfirmLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.dateConfirmed = attributeReader.GetString("TimeConfirmed");
      this.confirmedByFullName = attributeReader.GetString(nameof (ConfirmedBy));
      this.confirmedBy = attributeReader.GetString("ConfirmedByID");
      this.requestGUID = attributeReader.GetString(nameof (RequestGUID));
      this.buySideExpirationDate = attributeReader.GetDate(nameof (BuySideExpirationDate));
      this.sellSideExpirationDate = attributeReader.GetDate(nameof (SellSideExpirationDate));
      this.sellSideDeliveryDate = attributeReader.GetDate(nameof (SellSideDeliveryDate));
      this.sellSideDeliveredBy = attributeReader.GetString(nameof (SellSideDeliveredBy));
      this.alertLoanOfficer = attributeReader.GetBoolean("Alert");
      this.commitmentTermEnabled = attributeReader.GetBoolean(nameof (CommitmentTermEnabled));
      this.enableZeroParPricingWholesale = attributeReader.GetBoolean(nameof (EnableZeroParPricingWholesale));
      this.enableZeroParPricingRetail = attributeReader.GetBoolean(nameof (EnableZeroParPricingRetail));
      this.isVoided = attributeReader.GetBoolean(nameof (Voided));
      this.hideLog = attributeReader.GetBoolean("HideLog", false);
      this.includeConfirmCnt = attributeReader.GetBoolean("IncludeConfirmCount", true);
      string s = attributeReader.GetString("Date");
      DateTime result;
      if (!s.EndsWith("Z") || !DateTime.TryParse(s, out result))
        return;
      this.date = System.TimeZoneInfo.ConvertTimeBySystemTimeZoneId(result, "Pacific Standard Time");
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) LockConfirmLog.XmlType);
      attributeWriter.Write("TimeConfirmed", (object) this.dateConfirmed);
      attributeWriter.Write("ConfirmedBy", (object) this.confirmedByFullName);
      attributeWriter.Write("ConfirmedByID", (object) this.confirmedBy);
      attributeWriter.Write("RequestGUID", (object) this.requestGUID);
      attributeWriter.Write("BuySideExpirationDate", (object) this.buySideExpirationDate);
      attributeWriter.Write("SellSideExpirationDate", (object) this.sellSideExpirationDate);
      attributeWriter.Write("SellSideDeliveryDate", (object) this.sellSideDeliveryDate);
      attributeWriter.Write("SellSideDeliveredBy", (object) this.sellSideDeliveredBy);
      attributeWriter.Write("Alert", (object) this.alertLoanOfficer);
      attributeWriter.Write("CommitmentTermEnabled", (object) this.commitmentTermEnabled);
      attributeWriter.Write("EnableZeroParPricingRetail", (object) this.enableZeroParPricingRetail);
      attributeWriter.Write("EnableZeroParPricingWholesale", (object) this.enableZeroParPricingWholesale);
      attributeWriter.Write("Voided", (object) this.isVoided);
      attributeWriter.Write("HideLog", (object) this.hideLog);
      attributeWriter.Write("IncludeConfirmCount", (object) this.includeConfirmCnt);
    }

    internal override void AttachToLog(LogList log)
    {
      base.AttachToLog(log);
      log.Loan.TriggerCalculation("LOCKRATE.REQUESTSTATUS", log.Loan.GetField("LOCKRATE.REQUESTSTATUS"));
      log.Loan.TriggerCalculation("LOCKRATE.LASTACTIONTIME", log.Loan.GetField("LOCKRATE.LASTACTIONTIME"));
      log.Loan.TriggerCalculation("LOCKRATE.CONFIRMATIONCOUNT", log.Loan.GetField("LOCKRATE.CONFIRMATIONCOUNT"));
    }

    public override DateTime GetSortDate() => DateTime.Parse(this.dateConfirmed);

    public override PipelineInfo.Alert[] GetPipelineAlerts()
    {
      if (this.Log.GetLastLockAlertLog() != this)
        return (PipelineInfo.Alert[]) null;
      return new PipelineInfo.Alert[1]
      {
        new PipelineInfo.Alert(9, "Lock confirmed by " + this.confirmedByFullName, "", this.Date, this.RequestGUID, this.Guid)
      };
    }

    public string DateConfirmed
    {
      get => this.dateConfirmed;
      set
      {
        this.dateConfirmed = value;
        this.MarkAsDirty();
      }
    }

    public DateTime DateTimeConfirmed
    {
      get
      {
        try
        {
          return DateTime.Parse(this.dateConfirmed, Utils.StandardDateFormatProvider);
        }
        catch
        {
          return DateTime.MinValue;
        }
      }
    }

    public string ConfirmedBy => this.confirmedBy;

    public string ConfirmedByFullName => this.confirmedByFullName;

    public bool Voided
    {
      set
      {
        this.isVoided = value;
        this.MarkAsDirty();
      }
      get => this.isVoided;
    }

    public bool IncludeConfirmCnt
    {
      set
      {
        this.includeConfirmCnt = value;
        this.MarkAsDirty();
      }
      get => this.includeConfirmCnt;
    }

    public void SetConfirmingUser(UserInfo user)
    {
      this.SetConfirmingUser(user.Userid, user.FullName);
    }

    public void SetConfirmingUser(string userId, string fullName)
    {
      this.confirmedBy = userId;
      this.confirmedByFullName = fullName;
      this.MarkAsDirty();
    }

    public string RequestGUID
    {
      get => this.requestGUID;
      set
      {
        this.requestGUID = value;
        this.MarkAsDirty();
      }
    }

    public DateTime BuySideExpirationDate
    {
      get => this.buySideExpirationDate;
      set
      {
        this.buySideExpirationDate = value;
        this.MarkAsDirty();
      }
    }

    public DateTime SellSideExpirationDate
    {
      get => this.sellSideExpirationDate;
      set
      {
        this.sellSideExpirationDate = value;
        this.MarkAsDirty();
      }
    }

    public DateTime SellSideDeliveryDate
    {
      get => this.sellSideDeliveryDate;
      set
      {
        this.sellSideDeliveryDate = value;
        this.MarkAsDirty();
      }
    }

    public string SellSideDeliveredBy
    {
      get => this.sellSideDeliveredBy;
      set
      {
        this.sellSideDeliveredBy = value;
        this.MarkAsDirty();
      }
    }

    public bool AlertLoanOfficer
    {
      get => this.alertLoanOfficer;
      set
      {
        this.alertLoanOfficer = value;
        this.MarkAsDirty();
      }
    }

    public bool CommitmentTermEnabled
    {
      get => this.commitmentTermEnabled;
      set
      {
        this.commitmentTermEnabled = value;
        this.MarkAsDirty();
      }
    }

    public bool EnableZeroParPricingRetail
    {
      get => this.enableZeroParPricingRetail;
      set
      {
        this.enableZeroParPricingRetail = value;
        this.MarkAsDirty();
      }
    }

    public bool EnableZeroParPricingWholesale
    {
      get => this.enableZeroParPricingWholesale;
      set
      {
        this.enableZeroParPricingWholesale = value;
        this.MarkAsDirty();
      }
    }

    public override bool DisplayInLog
    {
      get => !this.hideLog;
      set
      {
        this.hideLog = !value;
        this.MarkAsDirty();
      }
    }
  }
}
