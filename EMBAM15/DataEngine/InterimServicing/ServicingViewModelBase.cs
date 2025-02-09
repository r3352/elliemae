// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.ServicingViewModelBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public abstract class ServicingViewModelBase
  {
    private ServicingTransactionBase[] transactions;
    private int selectedYear;
    private int paymentCount;
    private DateTime lastPaymentReceivedDate = DateTime.MinValue;
    private List<int> paymentYears = new List<int>();

    public ServicingViewModelBase(ServicingTransactionBase[] transactions)
    {
      this.transactions = transactions;
      this.GenerateAnnualSummary(0);
    }

    public ServicingTransactionBase[] Transactions => this.transactions;

    public int SelectedYear => this.selectedYear;

    public bool HasTransaction => this.transactions != null && this.transactions.Length >= 1;

    public int PaymentCount
    {
      get => this.paymentCount;
      set => this.paymentCount = value;
    }

    public DateTime LastPaymentReceivedDate
    {
      get => this.lastPaymentReceivedDate;
      set => this.lastPaymentReceivedDate = value;
    }

    public List<int> PaymentYears
    {
      get
      {
        List<int> paymentYears = new List<int>();
        foreach (int paymentYear in this.paymentYears)
        {
          bool flag = true;
          foreach (int num in paymentYears)
          {
            if (num == paymentYear)
              flag = false;
            if (!flag)
              break;
          }
          if (flag)
            paymentYears.Add(paymentYear);
        }
        return paymentYears;
      }
    }

    public int GetMaxPaymentYear(int closingTaxYear, int upperBoundTaxYear)
    {
      if (this.paymentYears.Count == 0)
        return 0;
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      foreach (int paymentYear in this.paymentYears)
      {
        if (!dictionary.ContainsKey(paymentYear))
          dictionary.Add(paymentYear, paymentYear);
      }
      if (upperBoundTaxYear - closingTaxYear == 1)
        return dictionary.ContainsKey(upperBoundTaxYear) ? upperBoundTaxYear : 0;
      if (upperBoundTaxYear - closingTaxYear > 1)
      {
        int maxPaymentYear = 0;
        foreach (KeyValuePair<int, int> keyValuePair in dictionary)
        {
          if (keyValuePair.Key > closingTaxYear && keyValuePair.Key < DateTime.Today.Year && keyValuePair.Key > maxPaymentYear)
            maxPaymentYear = keyValuePair.Key;
        }
        return maxPaymentYear;
      }
      if (upperBoundTaxYear - closingTaxYear != 0)
        return 0;
      int maxPaymentYear1 = 0;
      foreach (KeyValuePair<int, int> keyValuePair in dictionary)
      {
        if (keyValuePair.Key > closingTaxYear && keyValuePair.Key > DateTime.Today.Year)
        {
          maxPaymentYear1 = keyValuePair.Key;
          break;
        }
      }
      return maxPaymentYear1;
    }

    public virtual void GenerateAnnualSummary(int year)
    {
      this.selectedYear = year;
      if (!this.HasTransaction)
        return;
      this.reset();
      Dictionary<string, ServicingTransactionBase> dictionary = new Dictionary<string, ServicingTransactionBase>();
      for (int index = 0; index < this.Transactions.Length; ++index)
      {
        if (this.Transactions[index] is PaymentReversalLog)
        {
          PaymentReversalLog transaction = (PaymentReversalLog) this.Transactions[index];
          if (!dictionary.ContainsKey(transaction.TransactionGUID))
            dictionary.Add(transaction.PaymentGUID, (ServicingTransactionBase) transaction);
        }
      }
      for (int index = 0; index < this.Transactions.Length; ++index)
      {
        if (this.Transactions[index] is PaymentTransactionLog)
        {
          PaymentTransactionLog transaction = (PaymentTransactionLog) this.Transactions[index];
          if (!dictionary.ContainsKey(transaction.TransactionGUID))
          {
            List<int> paymentYears = this.paymentYears;
            DateTime paymentReceivedDate = transaction.PaymentReceivedDate;
            int year1 = paymentReceivedDate.Year;
            paymentYears.Add(year1);
            if (year > 0)
            {
              paymentReceivedDate = transaction.PaymentReceivedDate;
              if (paymentReceivedDate.Year == year)
              {
                ++this.PaymentCount;
                this.tallyAnnualSummarySubItems(transaction);
              }
            }
            if (this.LastPaymentReceivedDate == DateTime.MinValue)
              this.LastPaymentReceivedDate = transaction.PaymentReceivedDate;
            else if (this.LastPaymentReceivedDate < transaction.PaymentReceivedDate)
              this.LastPaymentReceivedDate = transaction.PaymentReceivedDate;
          }
        }
      }
    }

    private void reset()
    {
      this.paymentCount = 0;
      this.lastPaymentReceivedDate = DateTime.MinValue;
      this.paymentYears = new List<int>();
      this.resetSubItems();
    }

    protected abstract void tallyAnnualSummarySubItems(PaymentTransactionLog payTransLog);

    protected abstract void resetSubItems();
  }
}
