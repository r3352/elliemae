// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CreditReportParser
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class CreditReportParser
  {
    private XmlDocument xmldoc;
    private static string className = nameof (CreditReportParser);
    private static string sw = Tracing.SwContact;

    public ContactCreditScores[] parseCreditReport()
    {
      string xml = this.LoadLiability();
      this.xmldoc = new XmlDocument();
      this.xmldoc.LoadXml(xml);
      string versionID = this.xmldoc.DocumentElement.GetAttribute("MISMOVersionID");
      if (versionID == null || versionID == "")
      {
        if (this.xmldoc.DocumentElement.Name == "MORTGAGEDATA")
          versionID = "1.1";
        else if (this.xmldoc.DocumentElement.Name == "RESPONSE_GROUP")
          versionID = "2.3";
      }
      return this.ParseLiabilities(versionID);
    }

    private ContactCreditScores[] ParseLiabilities(string versionID)
    {
      ContactCreditScores[] liabilities = new ContactCreditScores[0];
      switch (versionID)
      {
        case "1.1":
          liabilities = this.ParseLiabilities_11();
          break;
        case "2.3":
        case "2.3.1":
          liabilities = this.ParseLiabilities_23();
          break;
      }
      return liabilities;
    }

    private ContactCreditScores[] ParseLiabilities_11()
    {
      XmlNodeList xmlNodeList1 = this.xmldoc.SelectNodes("MORTGAGEDATA/BORROWER");
      XmlNodeList xmlNodeList2 = this.xmldoc.SelectNodes("MORTGAGEDATA/CREDITSCORE");
      Hashtable hashtable = new Hashtable();
      foreach (XmlElement elm1 in xmlNodeList1)
      {
        string attr1 = this.GetAttr(elm1, "BORROWERID");
        if (!hashtable.Contains((object) attr1))
        {
          string nodeValue1 = this.GetNodeValue(elm1, "FirstName");
          string nodeValue2 = this.GetNodeValue(elm1, "LastName");
          string nodeValue3 = this.GetNodeValue(elm1, "SSN");
          ArrayList arrayList = new ArrayList();
          foreach (XmlElement elm2 in xmlNodeList2)
          {
            string attr2 = this.GetAttr(elm2, "BORROWERIDREFS");
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            if (attr2 == attr1)
            {
              string attr3 = this.GetAttr(elm2, "DataRepositorySourceType");
              string score = this.GetNodeValue(elm2, "CreditScoreValue").Trim().Replace("@", "").Replace("|", "");
              CreditScore creditScore = new CreditScore(attr3.Replace("@", "").Replace("|", ""), score);
              arrayList.Add((object) creditScore);
            }
          }
          ContactCreditScores contactCreditScores = new ContactCreditScores(attr1, nodeValue1, nodeValue2, nodeValue3, (CreditScore[]) arrayList.ToArray(typeof (CreditScore)));
          hashtable.Add((object) attr1, (object) contactCreditScores);
        }
      }
      return (ContactCreditScores[]) new ArrayList(hashtable.Values).ToArray(typeof (ContactCreditScores));
    }

    private ContactCreditScores[] ParseLiabilities_23()
    {
      XmlNodeList xmlNodeList1 = this.xmldoc.SelectNodes("RESPONSE_GROUP/RESPONSE/RESPONSE_DATA/CREDIT_RESPONSE/BORROWER");
      XmlNodeList xmlNodeList2 = this.xmldoc.SelectNodes("RESPONSE_GROUP/RESPONSE/RESPONSE_DATA/CREDIT_RESPONSE/CREDIT_SCORE");
      Hashtable hashtable = new Hashtable();
      foreach (XmlElement elm1 in xmlNodeList1)
      {
        string attr1 = this.GetAttr(elm1, "BorrowerID");
        if (!hashtable.Contains((object) attr1))
        {
          string attr2 = this.GetAttr(elm1, "_FirstName");
          string attr3 = this.GetAttr(elm1, "_LastName");
          string attr4 = this.GetAttr(elm1, "_SSN");
          ArrayList arrayList = new ArrayList();
          foreach (XmlElement elm2 in xmlNodeList2)
          {
            string attr5 = this.GetAttr(elm2, "BorrowerID");
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            if (attr5 == attr1)
            {
              string attr6 = this.GetAttr(elm2, "CreditRepositorySourceType");
              string score = this.GetAttr(elm2, "_Value").Trim().Replace("@", "").Replace("|", "");
              CreditScore creditScore = new CreditScore(attr6.Replace("@", "").Replace("|", ""), score);
              arrayList.Add((object) creditScore);
            }
          }
          ContactCreditScores contactCreditScores = new ContactCreditScores(attr1, attr2, attr3, attr4, (CreditScore[]) arrayList.ToArray(typeof (CreditScore)));
          hashtable.Add((object) attr1, (object) contactCreditScores);
        }
      }
      return (ContactCreditScores[]) new ArrayList(hashtable.Values).ToArray(typeof (ContactCreditScores));
    }

    private string GetAttr(XmlElement elm, string attr)
    {
      return elm.GetAttribute(attr).Replace("\n", "").Replace("\r", "") ?? string.Empty;
    }

    private string GetNodeValue(XmlElement elm, string nodeName)
    {
      XmlNode xmlNode = elm.SelectSingleNode(nodeName);
      return xmlNode == null ? string.Empty : xmlNode.InnerText.ToString().Replace("\n", "").Replace("\r", "");
    }

    private string LoadLiability()
    {
      try
      {
        return this.LoadFile(this.liabilityFileNew()) ?? this.LoadFile(this.liabilityFileOld());
      }
      catch (Exception ex)
      {
        Tracing.Log(CreditReportParser.sw, CreditReportParser.className, TraceLevel.Error, "BAM Error: " + (object) ex);
        throw;
      }
    }

    private string LoadFile(string filename)
    {
      try
      {
        return Session.LoanDataMgr.GetSupportingData(filename)?.ToString();
      }
      catch (Exception ex)
      {
        Tracing.Log(CreditReportParser.sw, CreditReportParser.className, TraceLevel.Error, "BAM Error: " + (object) ex);
        throw;
      }
    }

    private string liabilityFileNew() => "liability-" + Session.LoanData.PairId + ".xml";

    private string liabilityFileOld()
    {
      return "liability-" + Session.LoanData.PairId.Substring(1) + ".xml";
    }
  }
}
