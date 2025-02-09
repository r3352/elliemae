// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineRequest
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  internal class DocEngineRequest
  {
    private XmlDocument requestXml;

    public XmlDocument RequestXml => this.requestXml;

    public DocEngineRequest(
      SessionObjects sessionObjects,
      string requestType,
      string correlationID = null)
    {
      PerformanceMeter.Current.AddCheckpoint("BEGIN", 39, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\DocEngineRequest.cs");
      this.requestXml = new XmlDocument();
      this.requestXml.LoadXml("<" + requestType + " />");
      Guid guid = Guid.NewGuid();
      this.requestXml.DocumentElement.SetAttribute("PartnerID", "EncompassCloser");
      this.requestXml.DocumentElement.SetAttribute("RequestGuid", guid.ToString());
      if (correlationID != null)
        this.requestXml.DocumentElement.SetAttribute("CorrelationID", correlationID);
      this.requestXml.DocumentElement.SetAttribute("EncompassVersion", VersionInformation.CurrentVersion.GetExtendedVersionWithHotfix(sessionObjects.ServerLicense.Edition));
      PerformanceMeter.Current.AddNote("TransactionType: " + requestType + ", RequestGuid: " + guid.ToString() + (!string.IsNullOrEmpty(correlationID) ? ", CorrelationID: " + correlationID : string.Empty));
      PerformanceMeter.Current.AddVariable("TransactionType", (object) requestType);
      PerformanceMeter.Current.AddVariable("RequestGuid", (object) guid.ToString());
      if (string.IsNullOrEmpty(correlationID))
        PerformanceMeter.Current.AddVariable("CorrelationID", (object) correlationID);
      XmlElement xmlElement1 = (XmlElement) this.requestXml.DocumentElement.AppendChild((XmlNode) this.requestXml.CreateElement("USER"));
      xmlElement1.SetAttribute("ClientID", sessionObjects.CompanyInfo.ClientID);
      xmlElement1.SetAttribute("UserName", sessionObjects.UserID);
      xmlElement1.SetAttribute("Password", sessionObjects.CompanyInfo.Password);
      string str = sessionObjects.RuntimeEnvironment == RuntimeEnvironment.Hosted ? "Hosted" : "Default";
      XmlElement xmlElement2 = (XmlElement) this.requestXml.DocumentElement.AppendChild((XmlNode) this.requestXml.CreateElement("REQUEST_OPTIONS"));
      xmlElement2.SetAttribute("InstanceID", sessionObjects.StartupInfo.ServerInstanceName);
      XmlElement xmlElement3 = (XmlElement) xmlElement2.AppendChild((XmlNode) this.requestXml.CreateElement("ENCOMPASS_SETTINGS"));
      xmlElement3.AppendChild((XmlNode) this.requestXml.CreateElement("URLAPageNumbering")).InnerXml = sessionObjects.URLAPageNumbering.ToString();
      xmlElement3.AppendChild((XmlNode) this.requestXml.CreateElement("StateStatutoryElectionForMD")).InnerText = this.StateStatutoryElectionForMD(sessionObjects);
      xmlElement3.AppendChild((XmlNode) this.requestXml.CreateElement("PoliciesDefault072021GSEInstruments")).InnerText = sessionObjects.GetCompanySettingFromCache("Policies", "UniformInstrumentsDefaultDate") ?? "";
      xmlElement2.SetAttribute("Environment", str);
      PerformanceMeter.Current.AddCheckpoint("END", 94, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\LoanUtils\\DocEngine\\DocEngineRequest.cs");
    }

    public string StateStatutoryElectionForMD(SessionObjects sessionObjects)
    {
      if (!sessionObjects.CompanyInfo.StateBranchLicensing.StatutoryElectionInMaryland)
        return "NoStatutory";
      switch (sessionObjects.CompanyInfo.StateBranchLicensing.StatutoryElectionInMaryland2)
      {
        case "10":
          return "CreditGrantorAllLoans";
        case "00":
          return "CreditGrantorJrLiens";
        case "01":
          return "SecondMortgageJrLiens";
        default:
          return "NoStatutory";
      }
    }

    public void SetParameter(string name, string value)
    {
      this.requestXml.DocumentElement.SetAttribute(name, value);
    }

    public void SetExtendedParameter(string name, string value)
    {
      XmlElement element = this.requestXml.CreateElement(name);
      element.SetAttribute(nameof (value), value);
      this.requestXml.DocumentElement.AppendChild((XmlNode) element);
    }

    public override string ToString() => this.requestXml.OuterXml;

    public void EncryptLoanData(XmlElement elementToEncrypt)
    {
      string xmlString = "<RSAKeyValue><Modulus>1xfiw8Cr9QdskOGyD1Fnj6bp+OyehOckgf0fJed9vs5ggTqhmJex6BaDv3bgsxSMj3Iaq7K2vydC7LqaxV4K1WW8Rwd3ztLwOQ/+pxw7VJaodq8fTSk8NWjAB54WiM1Rwsng6RD8OKVfaTTFSdHYq+fS5e+K+h9xRrqyuelSBrU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
      string str = "EncryptedElement1";
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider())
      {
        using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
        {
          cryptoServiceProvider.FromXmlString(xmlString);
          rijndaelManaged.KeySize = 256;
          byte[] numArray = new EncryptedXml().EncryptData(elementToEncrypt, (SymmetricAlgorithm) rijndaelManaged, false);
          EncryptedData encryptedData = new EncryptedData();
          encryptedData.Type = "http://www.w3.org/2001/04/xmlenc#Element";
          encryptedData.Id = str;
          encryptedData.EncryptionMethod = new EncryptionMethod("http://www.w3.org/2001/04/xmlenc#aes256-cbc");
          EncryptedKey encryptedKey = new EncryptedKey();
          byte[] cipherValue = EncryptedXml.EncryptKey(rijndaelManaged.Key, (RSA) cryptoServiceProvider, false);
          encryptedKey.CipherData = new CipherData(cipherValue);
          encryptedKey.EncryptionMethod = new EncryptionMethod("http://www.w3.org/2001/04/xmlenc#rsa-1_5");
          DataReference dataReference = new DataReference();
          dataReference.Uri = "#" + str;
          encryptedKey.AddReference(dataReference);
          encryptedData.KeyInfo.AddClause((KeyInfoClause) new KeyInfoEncryptedKey(encryptedKey));
          encryptedKey.KeyInfo.AddClause((KeyInfoClause) new KeyInfoName()
          {
            Value = "rsaKey"
          });
          encryptedData.CipherData.CipherValue = numArray;
          EncryptedXml.ReplaceElement(elementToEncrypt, encryptedData, false);
        }
      }
    }
  }
}
