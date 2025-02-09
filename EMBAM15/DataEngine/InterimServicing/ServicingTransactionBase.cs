// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.ServicingTransactionBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public abstract class ServicingTransactionBase
  {
    private int transactionNo;
    private string transactionGUID = string.Empty;
    private ServicingTransactionTypes transactionType;
    private ServicingPaymentMethods paymentMethod;
    private DateTime transactionDate = DateTime.MinValue;
    private double transactionAmount;
    private DateTime createdDateTime = DateTime.MinValue;
    private string createdByName = string.Empty;
    private string createdByID = string.Empty;
    private DateTime modifiedDateTime = DateTime.MinValue;
    private string modifiedByName = string.Empty;
    private string modifiedByID = string.Empty;

    public ServicingTransactionBase()
    {
      this.transactionGUID = Guid.NewGuid().ToString();
      this.createdDateTime = DateTime.Now;
    }

    public ServicingTransactionBase(XmlElement e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.transactionGUID = attributeReader.GetString("GUID");
      this.transactionDate = attributeReader.GetDate("Date");
      this.transactionType = (ServicingTransactionTypes) ServicingEnum.ToEnum(attributeReader.GetString("Type"), typeof (ServicingTransactionTypes));
      this.paymentMethod = (ServicingPaymentMethods) ServicingEnum.ToEnum(attributeReader.GetString(nameof (PaymentMethod)), typeof (ServicingPaymentMethods));
      this.transactionAmount = attributeReader.GetDouble(nameof (TransactionAmount), 0.0);
      this.createdDateTime = attributeReader.GetDate("CreatedTime");
      this.createdByName = attributeReader.GetString(nameof (CreatedByName));
      this.createdByID = attributeReader.GetString(nameof (CreatedByID));
      this.modifiedDateTime = attributeReader.GetDate("ModifiedTime");
      this.modifiedByName = attributeReader.GetString(nameof (ModifiedByName));
      this.modifiedByID = attributeReader.GetString(nameof (ModifiedByID));
    }

    public virtual void Add(XmlElement newlog, bool use5DecimalsForIndexRates)
    {
      newlog.SetAttribute("GUID", this.transactionGUID);
      newlog.SetAttribute("Date", this.transactionDate.ToString("MM/dd/yyyy"));
      newlog.SetAttribute("CreatedTime", this.createdDateTime.ToString("MM/dd/yyyy HH:mm:ss"));
      newlog.SetAttribute("CreatedByName", this.createdByName);
      newlog.SetAttribute("CreatedByID", this.createdByID);
      if (this.modifiedDateTime != DateTime.MinValue)
      {
        newlog.SetAttribute("ModifiedTime", this.modifiedDateTime.ToString("MM/dd/yyyy HH:mm:ss"));
        newlog.SetAttribute("ModifiedByName", this.modifiedByName);
        newlog.SetAttribute("ModifiedByID", this.modifiedByID);
      }
      newlog.SetAttribute("TransactionAmount", this.transactionAmount.ToString("N2"));
      if (this.paymentMethod != ServicingPaymentMethods.None)
        newlog.SetAttribute("PaymentMethod", this.paymentMethod.ToString());
      else
        newlog.SetAttribute("PaymentMethod", "");
    }

    public virtual int TransactionNo
    {
      get => this.transactionNo;
      set => this.transactionNo = value;
    }

    public virtual string TransactionGUID
    {
      get => this.transactionGUID;
      set => this.transactionGUID = value;
    }

    public virtual ServicingTransactionTypes TransactionType
    {
      get => this.transactionType;
      set => this.transactionType = value;
    }

    public virtual ServicingPaymentMethods PaymentMethod
    {
      get => this.paymentMethod;
      set => this.paymentMethod = value;
    }

    public virtual DateTime TransactionDate
    {
      get => this.transactionDate;
      set => this.transactionDate = value;
    }

    public virtual double TransactionAmount
    {
      get => this.transactionAmount;
      set => this.transactionAmount = value;
    }

    public virtual DateTime CreatedDateTime
    {
      get => this.createdDateTime;
      set => this.createdDateTime = value;
    }

    public virtual string CreatedByName
    {
      get => this.createdByName;
      set => this.createdByName = value;
    }

    public virtual string CreatedByID
    {
      get => this.createdByID;
      set => this.createdByID = value;
    }

    public virtual DateTime ModifiedDateTime
    {
      get => this.modifiedDateTime;
      set => this.modifiedDateTime = value;
    }

    public virtual string ModifiedByName
    {
      get => this.modifiedByName;
      set => this.modifiedByName = value;
    }

    public virtual string ModifiedByID
    {
      get => this.modifiedByID;
      set => this.modifiedByID = value;
    }
  }
}
