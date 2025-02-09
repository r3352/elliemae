// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanDefaultProviders
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanDefaultProviders : FormDataBase
  {
    private const string className = "LoanDefaultProviders�";
    private static string sw = Tracing.SwDataEngine;
    private SessionObjects sessionObjects;
    private string providerXml;
    public static string[] LoanFields = new string[97]
    {
      "617",
      "619",
      "620",
      "1244",
      "621",
      "623",
      "618",
      "974",
      "622",
      "89",
      "1246",
      "610",
      "612",
      "613",
      "1175",
      "614",
      "616",
      "611",
      "615",
      "87",
      "1011",
      "411",
      "412",
      "413",
      "1174",
      "414",
      "419",
      "416",
      "417",
      "88",
      "1243",
      "L252",
      "VEND.X157",
      "VEND.X158",
      "VEND.X159",
      "VEND.X160",
      "VEND.X161",
      "VEND.X162",
      "VEND.X163",
      "VEND.X164",
      "VEND.X165",
      "L248",
      "708",
      "709",
      "1252",
      "710",
      "712",
      "707",
      "711",
      "93",
      "1254",
      "1500",
      "VEND.X14",
      "VEND.X15",
      "VEND.X16",
      "VEND.X17",
      "VEND.X18",
      "VEND.X13",
      "VEND.X19",
      "VEND.X21",
      "VEND.X20",
      "624",
      "626",
      "627",
      "1245",
      "628",
      "630",
      "625",
      "629",
      "90",
      "1247",
      "REGZGFE.X8",
      "VEND.X171",
      "VEND.X172",
      "VEND.X173",
      "VEND.X174",
      "VEND.X175",
      "984",
      "1410",
      "1411",
      "VEND.X176",
      "VEND.X212",
      "VEND.X214",
      "VEND.X216",
      "VEND.X218",
      "VEND.X155",
      "VEND.X156",
      "VEND.X221",
      "VEND.X222",
      "VEND.X223",
      "VEND.X224",
      "VEND.X225",
      "VEND.X226",
      "VEND.X227",
      "VEND.X228",
      "VEND.X229",
      "VEND.X230"
    };

    public LoanDefaultProviders(SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.Reset();
    }

    public LoanDefaultProviders(string providerXml)
    {
      this.providerXml = providerXml;
      this.Reset();
    }

    public bool ApplyToLoan(LoanData loan)
    {
      XPathNodeIterator xpathNodeIterator = this.loadProviderXML();
      if (xpathNodeIterator == null)
        return false;
      while (xpathNodeIterator.MoveNext())
      {
        string attribute1 = xpathNodeIterator.Current.GetAttribute("id", string.Empty);
        string attribute2 = xpathNodeIterator.Current.GetAttribute("value", string.Empty);
        if (attribute1 != string.Empty && attribute1 != null && attribute2 != string.Empty && attribute2 != null)
          loan.SetCurrentField(attribute1, attribute2);
      }
      return true;
    }

    public void Reset()
    {
      foreach (string loanField in LoanDefaultProviders.LoanFields)
        this.SetCurrentField(loanField, "");
      XPathNodeIterator xpathNodeIterator = this.loadProviderXML();
      if (xpathNodeIterator == null)
        return;
      while (xpathNodeIterator.MoveNext())
        this.SetCurrentField(xpathNodeIterator.Current.GetAttribute("id", string.Empty), xpathNodeIterator.Current.GetAttribute("value", string.Empty));
    }

    private XPathNodeIterator loadProviderXML()
    {
      XPathDocument xpathDocument;
      try
      {
        if (this.providerXml == null)
          this.providerXml = this.sessionObjects.CurrentUser.GetDefaultProviderInfo();
        if (this.providerXml == "")
          return (XPathNodeIterator) null;
        xpathDocument = new XPathDocument((TextReader) new StringReader(this.providerXml));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDefaultProviders.sw, TraceLevel.Error, nameof (LoanDefaultProviders), "Error loading default provider XML: " + (object) ex);
        return (XPathNodeIterator) null;
      }
      return xpathDocument.CreateNavigator().Select("/DefaultProviders/Field");
    }

    public void SaveDefaultProviders()
    {
      try
      {
        string providerInfo = this.buildXML();
        this.sessionObjects.CurrentUser.UpdateDefaultProviderInfo(providerInfo);
        this.providerXml = providerInfo;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanDefaultProviders.sw, TraceLevel.Error, nameof (LoanDefaultProviders), "Provider save exception: " + (object) ex);
        throw;
      }
    }

    private string buildXML()
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlElement xmlElement1 = (XmlElement) xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("DefaultProviders"));
      foreach (string loanField in LoanDefaultProviders.LoanFields)
      {
        string simpleField = this.GetSimpleField(loanField);
        XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("Field"));
        xmlElement2.SetAttribute("id", loanField);
        xmlElement2.SetAttribute("value", simpleField);
      }
      return xmlDocument.OuterXml;
    }
  }
}
