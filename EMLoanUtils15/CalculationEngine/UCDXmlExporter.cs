// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.UCDXmlExporter
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class UCDXmlExporter : UCDXmlExporterBase
  {
    private LoanData loan;
    private bool triggerWorstCaseCalc = true;

    public UCDXmlExporter(LoanData loan)
      : this(loan, false, true, false)
    {
    }

    public void TriggerCalculation()
    {
      this.setTotalFields = true;
      this.loan.Calculator.CalculateProjectedPaymentTable(this.setTotalFields);
      this.triggerWorstCaseCalc = false;
      this.calculationOnly = true;
      this.forLoanEstimate = false;
      this.BuildXMLInner();
      this.secALines = this.secBLines = this.secCLines = this.secELines = this.secFLines = this.secGLines = this.secHLines = 0;
      this.borTotalOfPAC = this.borTotalOfPOC = this.borTotalOfsectionA = this.borTotalOfsectionB = this.borTotalOfsectionC = this.borTotalOfPACInSecI = this.borTotalOfPOCInSecI = this.borTotalOfsectionE = this.borTotalOfsectionF = this.borTotalOfsectionG = this.borTotalOfsectionH = this.selTotalOfPAC = this.selTotalOfPOC = this.othTotal = 0M;
      this.borLETotalOfsectionA = this.borLETotalOfsectionB = this.borLETotalOfsectionC = this.borLETotalOfsectionE = this.borLETotalOfsectionF = this.borLETotalOfsectionG = this.borLETotalOfsectionH = 0M;
      this.borUnroundedLETotalOfsectionA = this.borUnroundedLETotalOfsectionB = this.borUnroundedLETotalOfsectionC = this.borUnroundedLETotalOfsectionE = this.borUnroundedLETotalOfsectionF = this.borUnroundedLETotalOfsectionG = this.borUnroundedLETotalOfsectionH = 0M;
      this.forLoanEstimate = true;
      this.BuildXML();
    }

    public UCDXmlExporter(LoanData loan, bool forLoanEstimate, bool setTotalFields)
      : this(loan, forLoanEstimate, setTotalFields, false)
    {
    }

    public UCDXmlExporter(LoanData loan, bool forLoanEstimate, bool setTotalFields, bool fullUCD)
    {
      this.namespaces = new XmlNamespaceManager(this.xmlDoc.NameTable);
      this.loan = loan;
      this.forLoanEstimate = forLoanEstimate;
      this.setTotalFields = setTotalFields;
    }

    protected override void BuildXMLInner()
    {
      this.isBiweekly = this.loan.GetField("423") == "Biweekly";
      if (this.triggerWorstCaseCalc && this.setTotalFields)
        this.loan.Calculator.CalculateProjectedPaymentTable(this.setTotalFields);
      this.cdXPathBase = "";
      this.xmlDoc = new XmlDocument();
      using (StringReader txtReader = new StringReader("<mismo:MESSAGE MISMOReferenceModelIdentifier=\"3.3.0299\" xmlns:mismo=\"http://www.mismo.org/residential/2009/schemas\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.mismo.org/residential/2009/schemas MISMO_3.3.0_B299.xsd\"></mismo:MESSAGE>"))
        this.xmlDoc.Load((TextReader) txtReader);
      this.namespaces = new XmlNamespaceManager(this.xmlDoc.NameTable);
      this.namespaces.AddNamespace("mismo", "http://www.mismo.org/residential/2009/schemas");
      this.feeCount = 0;
      this.prepaidItemCount = 0;
      this.escrowItemCount = 0;
      this.createHeader();
      this.createSectionF();
      this.createSectionG();
      this.createSectionA();
      this.createSectionBandC();
      if (this.setLE2X32ToEmpty)
        this.loan.SetFieldFromCal("LE2.X32", "");
      this.createSectionE();
      this.createSectionH();
      if (!this.setTotalFields)
        return;
      this.saveTotalFields();
    }

    public override XmlDocument BuildXMLDocument()
    {
      this.BuildXMLInner();
      return this.xmlDoc;
    }

    public string BuildXML()
    {
      this.BuildXMLInner();
      return this.calculationOnly ? string.Empty : this.xmlDoc.OuterXml;
    }

    private void saveTotalFields()
    {
      if (this.forLoanEstimate)
      {
        this.loan.SetCurrentField("LE2.XSTA", Utils.ArithmeticRounding(this.borLETotalOfsectionA, 0).ToString());
        this.loan.SetCurrentField("LE2.XSTB", Utils.ArithmeticRounding(this.borLETotalOfsectionB, 0).ToString());
        this.loan.SetCurrentField("LE2.XSTC", Utils.ArithmeticRounding(this.borLETotalOfsectionC, 0).ToString());
        this.loan.SetCurrentField("LE2.XSTD", Utils.ArithmeticRounding(this.borLETotalOfsectionA + this.borLETotalOfsectionB + this.borLETotalOfsectionC, 0).ToString());
        this.loan.SetCurrentField("LE2.XSTD_DV", Utils.ArithmeticRounding(this.borUnroundedLETotalOfsectionA + this.borUnroundedLETotalOfsectionB + this.borUnroundedLETotalOfsectionC, 2).ToString());
        Decimal num1 = Utils.ArithmeticRounding(this.borLETotalOfsectionE, 0);
        Decimal num2 = num1;
        this.loan.SetCurrentField("LE2.XSTE", num1.ToString());
        Decimal num3 = Utils.ArithmeticRounding(this.borLETotalOfsectionF, 0);
        Decimal num4 = num2 + num3;
        this.loan.SetCurrentField("LE2.XSTF", num3.ToString());
        Decimal num5 = Utils.ArithmeticRounding(this.borLETotalOfsectionG, 0);
        Decimal num6 = num4 + num5;
        this.loan.SetCurrentField("LE2.XSTG", num5.ToString());
        Decimal num7 = Utils.ArithmeticRounding(this.borLETotalOfsectionH, 0);
        Decimal val1 = num6 + num7;
        this.loan.SetCurrentField("LE2.XSTH", num7.ToString());
        this.loan.SetCurrentField("LE2.XSTI", Utils.ArithmeticRounding(val1, 0).ToString());
        this.loan.SetCurrentField("LE2.XSTI_DV", Utils.ArithmeticRounding(this.borUnroundedLETotalOfsectionE + this.borUnroundedLETotalOfsectionF + this.borUnroundedLETotalOfsectionG + this.borUnroundedLETotalOfsectionH, 2).ToString());
        Decimal val2 = this.borLETotalOfsectionA + this.borLETotalOfsectionB + this.borLETotalOfsectionC + this.borLETotalOfsectionE + this.borLETotalOfsectionF + this.borLETotalOfsectionG + this.borLETotalOfsectionH;
        this.loan.SetCurrentField("LE2.XDI", Utils.ArithmeticRounding(val2, 0).ToString());
        this.loan.SetCurrentField("LE2.XSTJ", Utils.ArithmeticRounding(val2 - (Decimal) Utils.ParseInt((object) this.getField("LE2.XLC"), 0), 0).ToString());
        this.loan.SetCurrentField("LE2.XSTJDV", (this.borUnroundedLETotalOfsectionA + this.borUnroundedLETotalOfsectionB + this.borUnroundedLETotalOfsectionC + this.borUnroundedLETotalOfsectionE + this.borUnroundedLETotalOfsectionF + this.borUnroundedLETotalOfsectionG + this.borUnroundedLETotalOfsectionH - Utils.ParseDecimal((object) this.getField("LE2.XLCDV"))).ToString());
      }
      else
      {
        this.loan.SetCurrentField("CD2.XSTA", Utils.ArithmeticRounding(this.borTotalOfsectionA, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XSTB", Utils.ArithmeticRounding(this.borTotalOfsectionB, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XSTC", Utils.ArithmeticRounding(this.borTotalOfsectionC, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XSTD", Utils.ArithmeticRounding(this.borTotalOfsectionA + this.borTotalOfsectionB + this.borTotalOfsectionC, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XLCAC", Utils.ArithmeticRounding(this.borTotalOfPAC, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XLCBC", Utils.ArithmeticRounding(this.borTotalOfPOC, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XSTE", Utils.ArithmeticRounding(this.borTotalOfsectionE, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XSTF", Utils.ArithmeticRounding(this.borTotalOfsectionF, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XSTG", Utils.ArithmeticRounding(this.borTotalOfsectionG, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XSTH", Utils.ArithmeticRounding(this.borTotalOfsectionH, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XSTI", Utils.ArithmeticRounding(this.borTotalOfsectionE + this.borTotalOfsectionF + this.borTotalOfsectionG + this.borTotalOfsectionH, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XOCAC", Utils.ArithmeticRounding(this.borTotalOfPACInSecI, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XOCBC", Utils.ArithmeticRounding(this.borTotalOfPOCInSecI, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XBCCAC", Utils.ArithmeticRounding(this.borTotalOfPAC + this.borTotalOfPACInSecI, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XBCCBC", Utils.ArithmeticRounding(this.borTotalOfPOC + this.borTotalOfPOCInSecI, 2).ToString("0.00"));
        Decimal num = Utils.ArithmeticRounding(this.borTotalOfsectionA + this.borTotalOfsectionB + this.borTotalOfsectionC + this.borTotalOfsectionE + this.borTotalOfsectionF + this.borTotalOfsectionG + this.borTotalOfsectionH - Utils.ParseDecimal((object) this.getField("CD2.XSTLC")), 2);
        this.loan.SetCurrentField("CD2.XSTJ", num.ToString("0.00"));
        if (this.getField("CD3.X137") == "Y")
          this.loan.SetCurrentField("CD3.X1", num.ToString("0.00"));
        else
          this.loan.SetCurrentField("CD3.X1", (num - Utils.ParseDecimal((object) this.getField("CD3.X103"))).ToString("0.00"));
        this.loan.SetCurrentField("CD3.X46", Utils.ArithmeticRounding(this.selTotalOfPAC, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XSCCAC", Utils.ArithmeticRounding(this.selTotalOfPAC, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XSCCBC", Utils.ArithmeticRounding(this.selTotalOfPOC, 2).ToString("0.00"));
        this.loan.SetCurrentField("CD2.XCCBO", Utils.ArithmeticRounding(this.othTotal, 2).ToString("0.00"));
      }
      this.loan.Calculator.FormCalculation("CD2.XSTB");
      this.loan.Calculator.FormCalculation("VASUMM.X133");
    }

    protected override string getField(string fieldID)
    {
      return fieldID == string.Empty ? string.Empty : UCDXmlExporterBase.removeSpecialCharacters(this.loan.GetField(fieldID));
    }
  }
}
