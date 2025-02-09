// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PaymentScheduleSnapshot
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class PaymentScheduleSnapshot
  {
    private Hashtable fieldSnapshot;
    public double F_le1x24_cd4x34;
    private int actualNumberOfTerm = 360;
    private PaymentSchedule[] monthlyPayments;
    private EscrowSchedule[] escrowPayments;
    private int statementPrintDueDay = 14;
    private double minLateCharge;
    private double maxLateCharge = double.MaxValue;

    public PaymentScheduleSnapshot() => this.fieldSnapshot = new Hashtable();

    public PaymentScheduleSnapshot(XmlNode node)
      : this(node, true, true)
    {
    }

    public PaymentScheduleSnapshot(XmlNode node, bool includeEscrowPayments, bool includeBuyDown)
      : this()
    {
      XmlNodeList xmlNodeList1 = node.SelectNodes("Payment");
      XmlNodeList xmlNodeList2 = node.SelectNodes("Field");
      if (xmlNodeList2 != null)
      {
        foreach (XmlElement xmlElement in xmlNodeList2)
        {
          string attribute1 = xmlElement.GetAttribute("id");
          string attribute2 = xmlElement.GetAttribute("val");
          if (!((attribute2 ?? "") == ""))
            this.SetField(attribute1, attribute2);
        }
      }
      if (xmlNodeList1 != null)
      {
        this.actualNumberOfTerm = xmlNodeList1.Count;
        this.monthlyPayments = new PaymentSchedule[xmlNodeList1.Count];
        int index = 0;
        foreach (XmlElement xmlElement in xmlNodeList1)
        {
          this.monthlyPayments[index] = new PaymentSchedule();
          this.monthlyPayments[index].Payment = Utils.ParseDouble((object) xmlElement.GetAttribute("Amount"));
          this.monthlyPayments[index].CurrentRate = Utils.ParseDouble((object) xmlElement.GetAttribute("Rate"));
          this.monthlyPayments[index].Balance = Utils.ParseDouble((object) xmlElement.GetAttribute("Balance"));
          this.monthlyPayments[index].Interest = Utils.ParseDouble((object) xmlElement.GetAttribute("Interest"));
          this.monthlyPayments[index].MortgageInsurance = Utils.ParseDouble((object) xmlElement.GetAttribute("MI"));
          this.monthlyPayments[index].PayDate = xmlElement.GetAttribute("Date");
          this.monthlyPayments[index].Principal = Utils.ParseDouble((object) xmlElement.GetAttribute("Principal"));
          this.monthlyPayments[index].BuydownSubsidyAmount = Utils.ParseDouble((object) xmlElement.GetAttribute("BuydownSubsidyAmount"));
          this.monthlyPayments[index].BalanceForMI = Utils.ParseDouble((object) xmlElement.GetAttribute("BalanceForMI"));
          this.monthlyPayments[index].OriginalNoteRate = Utils.ParseDouble((object) xmlElement.GetAttribute("OriginalNoteRate"));
          this.monthlyPayments[index].USDAAnnualFee = Utils.ParseDouble((object) xmlElement.GetAttribute("USDAAnnualFee"));
          this.monthlyPayments[index].USDAAnnualUPB = Utils.ParseDouble((object) xmlElement.GetAttribute("USDAAnnualUPB"));
          this.monthlyPayments[index].USDAMonthlyFee = Utils.ParseDouble((object) xmlElement.GetAttribute("USDAMonthlyFee"));
          this.monthlyPayments[index].USDAMonthlyPayment = Utils.ParseDouble((object) xmlElement.GetAttribute("USDAMonthlyPayment"));
          this.monthlyPayments[index].RemainingLTV = Utils.ParseDouble((object) xmlElement.GetAttribute("RemainingLTV"));
          ++index;
        }
      }
      if (includeEscrowPayments)
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        DateTime dateTime = Utils.ParseDate((object) this.GetField("682"));
        if (dateTime == DateTime.MinValue)
          dateTime = DateTime.Today;
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        int num7 = 0;
        int num8 = 0;
        int num9 = 0;
        this.escrowPayments = new EscrowSchedule[49];
        for (int index1 = 1; index1 <= 48; ++index1)
        {
          this.escrowPayments[index1 - 1] = new EscrowSchedule();
          for (int index2 = 1; index2 <= 11; ++index2)
          {
            string id = "HUD" + index1.ToString("00") + index2.ToString("00");
            switch (index2)
            {
              case 10:
                id = "HUD" + index1.ToString("00") + "60";
                break;
              case 11:
                id = "HUD" + index1.ToString("00") + "10";
                break;
            }
            string field = this.GetField(id);
            switch (index2 - 1)
            {
              case 0:
                if (field != string.Empty && field.Length == 5)
                {
                  string str1 = field.Substring(0, 2);
                  string str2 = field.Substring(3);
                  this.escrowPayments[index1 - 1].PayDate = Utils.ParseDate((object) (str1 + "/" + dateTime.Day.ToString("00") + "/" + str2));
                  break;
                }
                this.escrowPayments[index1 - 1].PayDate = Utils.ParseDate((object) field);
                break;
              case 1:
                this.escrowPayments[index1 - 1].Tax = Utils.ParseDouble((object) field);
                this.escrowPayments[index1 - 1].PayDate_Tax = this.escrowPayments[index1 - 1].PayDate;
                if (this.escrowPayments[index1 - 1].Tax != 0.0)
                {
                  ++num1;
                  if (num1 <= 4 && this.GetField("HUD" + num1.ToString("00") + "41") != string.Empty)
                  {
                    this.escrowPayments[index1 - 1].PayDate_Tax = Utils.ParseDate((object) this.GetField("HUD" + num1.ToString("00") + "41"));
                    break;
                  }
                  break;
                }
                break;
              case 2:
                this.escrowPayments[index1 - 1].HazardInsurance = Utils.ParseDouble((object) field);
                this.escrowPayments[index1 - 1].PayDate_Hazard = this.escrowPayments[index1 - 1].PayDate;
                if (this.escrowPayments[index1 - 1].HazardInsurance != 0.0)
                {
                  ++num2;
                  if (num2 <= 4 && this.GetField("HUD" + num2.ToString("00") + "42") != string.Empty)
                  {
                    this.escrowPayments[index1 - 1].PayDate_Hazard = Utils.ParseDate((object) this.GetField("HUD" + num2.ToString("00") + "42"));
                    break;
                  }
                  break;
                }
                break;
              case 3:
                this.escrowPayments[index1 - 1].MortgageInsurance = Utils.ParseDouble((object) field);
                this.escrowPayments[index1 - 1].PayDate_Mortgage = this.escrowPayments[index1 - 1].PayDate;
                if (this.escrowPayments[index1 - 1].MortgageInsurance != 0.0)
                {
                  ++num3;
                  if (num3 <= 4 && this.GetField("HUD" + num3.ToString("00") + "43") != string.Empty)
                  {
                    this.escrowPayments[index1 - 1].PayDate_Mortgage = Utils.ParseDate((object) this.GetField("HUD" + num3.ToString("00") + "43"));
                    break;
                  }
                  break;
                }
                break;
              case 4:
                this.escrowPayments[index1 - 1].FloodInsurance = Utils.ParseDouble((object) field);
                this.escrowPayments[index1 - 1].PayDate_Flood = this.escrowPayments[index1 - 1].PayDate;
                if (this.escrowPayments[index1 - 1].FloodInsurance != 0.0)
                {
                  ++num4;
                  if (num4 <= 4 && this.GetField("HUD" + num4.ToString("00") + "44") != string.Empty)
                  {
                    this.escrowPayments[index1 - 1].PayDate_Flood = Utils.ParseDate((object) this.GetField("HUD" + num4.ToString("00") + "44"));
                    break;
                  }
                  break;
                }
                break;
              case 5:
                this.escrowPayments[index1 - 1].CityTax = Utils.ParseDouble((object) field);
                this.escrowPayments[index1 - 1].PayDate_City = this.escrowPayments[index1 - 1].PayDate;
                if (this.escrowPayments[index1 - 1].CityTax != 0.0)
                {
                  ++num5;
                  if (num5 <= 4 && this.GetField("HUD" + num5.ToString("00") + "45") != string.Empty)
                  {
                    this.escrowPayments[index1 - 1].PayDate_City = Utils.ParseDate((object) this.GetField("HUD" + num5.ToString("00") + "45"));
                    break;
                  }
                  break;
                }
                break;
              case 6:
                this.escrowPayments[index1 - 1].UserTax1 = Utils.ParseDouble((object) field);
                this.escrowPayments[index1 - 1].PayDate_User1 = this.escrowPayments[index1 - 1].PayDate;
                if (this.escrowPayments[index1 - 1].UserTax1 != 0.0)
                {
                  ++num6;
                  if (num6 <= 4 && this.GetField("HUD" + num6.ToString("00") + "46") != string.Empty)
                  {
                    this.escrowPayments[index1 - 1].PayDate_User1 = Utils.ParseDate((object) this.GetField("HUD" + num6.ToString("00") + "46"));
                    break;
                  }
                  break;
                }
                break;
              case 7:
                this.escrowPayments[index1 - 1].UserTax2 = Utils.ParseDouble((object) field);
                this.escrowPayments[index1 - 1].PayDate_User2 = this.escrowPayments[index1 - 1].PayDate;
                if (this.escrowPayments[index1 - 1].UserTax2 != 0.0)
                {
                  ++num7;
                  if (num7 <= 4 && this.GetField("HUD" + num7.ToString("00") + "47") != string.Empty)
                  {
                    this.escrowPayments[index1 - 1].PayDate_User2 = Utils.ParseDate((object) this.GetField("HUD" + num7.ToString("00") + "47"));
                    break;
                  }
                  break;
                }
                break;
              case 8:
                this.escrowPayments[index1 - 1].UserTax3 = Utils.ParseDouble((object) field);
                this.escrowPayments[index1 - 1].PayDate_User3 = this.escrowPayments[index1 - 1].PayDate;
                if (this.escrowPayments[index1 - 1].UserTax3 != 0.0)
                {
                  ++num8;
                  if (num8 <= 4 && this.GetField("HUD" + num8.ToString("00") + "48") != string.Empty)
                  {
                    this.escrowPayments[index1 - 1].PayDate_User3 = Utils.ParseDate((object) this.GetField("HUD" + num8.ToString("00") + "48"));
                    break;
                  }
                  break;
                }
                break;
              case 9:
                this.escrowPayments[index1 - 1].USDAPremium = Utils.ParseDouble((object) field);
                this.escrowPayments[index1 - 1].PayDate_USDAPremium = this.escrowPayments[index1 - 1].PayDate;
                if (this.escrowPayments[index1 - 1].USDAPremium != 0.0)
                {
                  ++num9;
                  if (num9 <= 4 && this.GetField("HUD" + num9.ToString("00") + "49") != string.Empty)
                  {
                    this.escrowPayments[index1 - 1].PayDate_USDAPremium = Utils.ParseDate((object) this.GetField("HUD" + num9.ToString("00") + "49"));
                    break;
                  }
                  break;
                }
                break;
              case 10:
                this.escrowPayments[index1 - 1].TotalPayment = Utils.ParseDouble((object) field);
                break;
            }
          }
        }
      }
      if (!includeBuyDown)
        return;
      this.GetBuyDown();
    }

    private void GetBuyDown()
    {
      this.BuyDown = new BuyDownInfo[6];
      for (int index = 0; index < this.BuyDown.Length; ++index)
      {
        this.BuyDown[index] = new BuyDownInfo();
        this.BuyDown[index].FundTotalAmount = this.GetNumField((3097 + index * 4).ToString());
        this.BuyDown[index].RemainingMonthsCount = this.GetNumField((index + 3124).ToString());
        this.BuyDown[index].SubsidyAmount = this.GetNumField((3096 + index * 4).ToString());
        this.BuyDown[index].BuydownIndex = index + 1;
      }
    }

    public void ToXml(XmlNode snapShotNode, XmlDocument xmldoc)
    {
      for (int index = 1; index <= this.actualNumberOfTerm; ++index)
      {
        AttributeWriter attributeWriter = new AttributeWriter((XmlElement) snapShotNode.AppendChild((XmlNode) xmldoc.CreateElement("Payment")));
        attributeWriter.Write("No", (object) index.ToString());
        attributeWriter.Write("Amount", (object) this.monthlyPayments[index - 1].Payment.ToString("N2"));
        attributeWriter.Write("Rate", (object) this.monthlyPayments[index - 1].CurrentRate.ToString("N3"));
        attributeWriter.Write("Balance", (object) this.monthlyPayments[index - 1].Balance.ToString("N2"));
        attributeWriter.Write("Interest", (object) this.monthlyPayments[index - 1].Interest.ToString("N2"));
        attributeWriter.Write("MI", (object) this.monthlyPayments[index - 1].MortgageInsurance.ToString("N2"));
        attributeWriter.Write("Date", (object) this.monthlyPayments[index - 1].PayDate);
        attributeWriter.Write("Principal", (object) this.monthlyPayments[index - 1].Principal.ToString("N2"));
        attributeWriter.Write("BuydownSubsidyAmount", (object) this.monthlyPayments[index - 1].BuydownSubsidyAmount);
        attributeWriter.Write("BalanceForMI", (object) this.monthlyPayments[index - 1].BalanceForMI.ToString("N2"));
        attributeWriter.Write("OriginalNoteRate", (object) this.monthlyPayments[index - 1].OriginalNoteRate.ToString("N3"));
        attributeWriter.Write("USDAAnnualFee", (object) this.monthlyPayments[index - 1].USDAAnnualFee.ToString("N2"));
        attributeWriter.Write("USDAAnnualUPB", (object) this.monthlyPayments[index - 1].USDAAnnualUPB.ToString("N2"));
        attributeWriter.Write("USDAMonthlyFee", (object) this.monthlyPayments[index - 1].USDAMonthlyFee.ToString("N2"));
        attributeWriter.Write("USDAMonthlyPayment", (object) this.monthlyPayments[index - 1].USDAMonthlyPayment.ToString("N2"));
        attributeWriter.Write("RemainingLTV", (object) this.monthlyPayments[index - 1].RemainingLTV.ToString("N3"));
      }
      foreach (DictionaryEntry dictionaryEntry in this.fieldSnapshot)
      {
        if (!(dictionaryEntry.Value.ToString() == string.Empty))
        {
          AttributeWriter attributeWriter = new AttributeWriter((XmlElement) snapShotNode.AppendChild((XmlNode) xmldoc.CreateElement("Field")));
          attributeWriter.Write("id", (object) dictionaryEntry.Key.ToString());
          attributeWriter.Write("val", (object) dictionaryEntry.Value.ToString());
        }
      }
    }

    public void SetField(string id, string val)
    {
      id = id.ToUpper();
      if (this.fieldSnapshot.ContainsKey((object) id.ToUpper()))
        this.fieldSnapshot[(object) id] = (object) val;
      else
        this.fieldSnapshot.Add((object) id, (object) val);
    }

    public int GetIntField(string id)
    {
      id = id.ToUpper();
      return this.fieldSnapshot.ContainsKey((object) id.ToUpper()) ? Utils.ParseInt((object) this.fieldSnapshot[(object) id].ToString()) : 0;
    }

    public double GetNumField(string id)
    {
      id = id.ToUpper();
      return this.fieldSnapshot.ContainsKey((object) id.ToUpper()) ? Utils.ParseDouble((object) this.fieldSnapshot[(object) id].ToString()) : 0.0;
    }

    public string GetField(string id)
    {
      id = id.ToUpper();
      return this.fieldSnapshot.ContainsKey((object) id.ToUpper()) ? this.fieldSnapshot[(object) id].ToString() : string.Empty;
    }

    public int ActualNumberOfTerm
    {
      get => this.actualNumberOfTerm;
      set => this.actualNumberOfTerm = value;
    }

    public int LoanTerm
    {
      get
      {
        int num = this.GetIntField("4");
        if (num <= 0)
          num = 360;
        return this.IsBiweekly ? num / 12 * 26 : num;
      }
    }

    public PaymentSchedule[] MonthlyPayments
    {
      get => this.monthlyPayments;
      set => this.monthlyPayments = value;
    }

    public EscrowSchedule[] EscrowPayments => this.escrowPayments;

    public double MarginRate => this.GetNumField("689");

    public double IndexRate => this.GetNumField("688");

    public double NoteRate => this.GetNumField("3");

    public int PaymentDueDays => this.GetIntField("672");

    public double LoanAmount => this.GetNumField("2");

    public int StatementPrintDueDay
    {
      set => this.statementPrintDueDay = value;
      get => this.statementPrintDueDay;
    }

    public double MinLateCharge
    {
      set => this.minLateCharge = value;
      get => this.minLateCharge;
    }

    public double MaxLateCharge
    {
      set => this.maxLateCharge = value;
      get => this.maxLateCharge;
    }

    public bool IsNegativeARM
    {
      get
      {
        return this.GetNumField("690") != 0.0 || this.GetNumField("691") != 0.0 || this.GetNumField("1712") != 0.0 || this.GetNumField("1713") != 0.0 || this.GetNumField("698") != 0.0;
      }
    }

    public bool IsBiweekly => this.GetField("423") == "Biweekly";

    public bool IsBuydownLoan => this.GetNumField("3119") > 0.0;

    public bool IsARMLoan => this.GetField("608") == "AdjustableRate";

    public int DaysPerYear
    {
      get
      {
        int intField = this.GetIntField("SYS.X2");
        return intField == 0 ? 360 : intField;
      }
    }

    public double RateFactorBiweekly(double currentRate)
    {
      return currentRate / (100.0 * ((double) this.DaysPerYear / 14.0));
    }

    public int NumberPayPerYear => this.GetField("423") == "Biweekly" ? 26 : 12;

    public double EscrowBeginbalance => this.GetNumField("ESCROWBEGINBALANCE");

    public double BuyDownFundBalance => this.GetNumField("3119");

    public double EscrowDue => this.GetNumField("HUD24");

    public string LoanPurposeType => this.GetField("19");

    public double LateChargePercent => this.GetNumField("674");

    public string LateChargeType => this.GetField("1719");

    public string ZeroPercentPaymentOption => this.GetField("4746");

    public BuyDownInfo[] BuyDown { get; private set; }
  }
}
