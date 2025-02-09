// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FieldReformatOnUIHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FieldReformatOnUIHandler
  {
    private IHtmlInput inputData;
    private LoanData loan;
    private Dictionary<string, string> fieldMapping = new Dictionary<string, string>()
    {
      {
        "3",
        "KBYO.XD3"
      },
      {
        "NEWHUD.X555",
        "KBYO.NEWHUDXD555"
      },
      {
        "799",
        "KBYO.XD799"
      },
      {
        "LE3.X16",
        "KBYO.LE3XD16"
      },
      {
        "689",
        "KBYO.XD689"
      },
      {
        "2625",
        "KBYO.XD2625"
      },
      {
        "697",
        "KBYO.XD697"
      },
      {
        "695",
        "KBYO.XD695"
      },
      {
        "4113",
        "KBYO.XD4113"
      }
    };

    public FieldReformatOnUIHandler(IHtmlInput inputData)
    {
      this.inputData = inputData;
      if (!(inputData is LoanData))
        return;
      this.loan = (LoanData) inputData;
    }

    public string GetFieldValue(string id, string val)
    {
      string id1 = "";
      return this.fieldMapping.TryGetValue(id, out id1) ? this.inputData.GetField(id1) : val;
    }
  }
}
