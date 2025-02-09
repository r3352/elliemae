// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Services.ClosedLoanBillingInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.Common.Services
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://tempuri.org/")]
  [Serializable]
  public class ClosedLoanBillingInfo
  {
    private string clientIDField;
    private string closingDateCalculationField;
    private string billingCategoryCalculationField;

    public string ClientID
    {
      get => this.clientIDField;
      set => this.clientIDField = value;
    }

    public string ClosingDateCalculation
    {
      get => this.closingDateCalculationField;
      set => this.closingDateCalculationField = value;
    }

    public string BillingCategoryCalculation
    {
      get => this.billingCategoryCalculationField;
      set => this.billingCategoryCalculationField = value;
    }
  }
}
