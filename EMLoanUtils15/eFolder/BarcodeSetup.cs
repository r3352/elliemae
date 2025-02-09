// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.BarcodeSetup
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  public class BarcodeSetup : BinaryConvertibleObject
  {
    private bool enabled;
    private bool requestedDocuments;
    private bool openingDocuments;
    private bool preClosingDocuments;
    private bool closingDocuments;
    private bool requestedDocumentsInfoDocs;
    private bool openingDocumentsInfoDocs;
    private bool preClosingDocumentsInfoDocs;
    private bool closingDocumentsInfoDocs;
    private bool existingAttachmentsFromEFolder;
    private string[] propertyStates;
    private string[] stateList = new string[53]
    {
      "AK",
      "AL",
      "AZ",
      "AR",
      "CA",
      "CO",
      "CT",
      "DE",
      "DC",
      "FL",
      "GA",
      "HI",
      "ID",
      "IL",
      "IN",
      "IA",
      "KS",
      "KY",
      "LA",
      "ME",
      "MD",
      "MA",
      "MI",
      "MN",
      "MS",
      "MO",
      "MT",
      "NE",
      "NV",
      "NH",
      "NJ",
      "NM",
      "NY",
      "NC",
      "ND",
      "OH",
      "OK",
      "OR",
      "PA",
      "PR",
      "RI",
      "SC",
      "SD",
      "TN",
      "TX",
      "UT",
      "VT",
      "VA",
      "VI",
      "WA",
      "WV",
      "WI",
      "WY"
    };
    public static BarcodeSetup _barcodeSetup;

    public BarcodeSetup()
    {
      this.enabled = true;
      this.requestedDocuments = true;
      this.openingDocuments = true;
      this.preClosingDocuments = true;
      this.closingDocuments = true;
      this.requestedDocumentsInfoDocs = false;
      this.openingDocumentsInfoDocs = false;
      this.PreClosingDocumentsInfoDocs = false;
      this.closingDocumentsInfoDocs = false;
      this.existingAttachmentsFromEFolder = false;
      this.propertyStates = (string[]) null;
    }

    public BarcodeSetup(bool includeAllStates)
    {
      this.enabled = true;
      this.requestedDocuments = true;
      this.openingDocuments = true;
      this.preClosingDocuments = true;
      this.closingDocuments = true;
      this.requestedDocumentsInfoDocs = false;
      this.openingDocumentsInfoDocs = false;
      this.PreClosingDocumentsInfoDocs = false;
      this.closingDocumentsInfoDocs = false;
      this.existingAttachmentsFromEFolder = false;
      this.propertyStates = this.stateList;
    }

    public BarcodeSetup(XmlSerializationInfo info)
    {
      this.enabled = info.GetBoolean(nameof (Enabled), true);
      this.requestedDocuments = info.GetBoolean(nameof (RequestedDocuments), true);
      this.openingDocuments = info.GetBoolean(nameof (OpeningDocuments), true);
      this.preClosingDocuments = info.GetBoolean(nameof (PreClosingDocuments), true);
      this.closingDocuments = info.GetBoolean(nameof (ClosingDocuments), true);
      this.requestedDocumentsInfoDocs = info.GetBoolean(nameof (RequestedDocumentsInfoDocs), false);
      this.openingDocumentsInfoDocs = info.GetBoolean(nameof (OpeningDocumentsInfoDocs), false);
      this.PreClosingDocumentsInfoDocs = info.GetBoolean(nameof (PreClosingDocumentsInfoDocs), false);
      this.closingDocumentsInfoDocs = info.GetBoolean(nameof (ClosingDocumentsInfoDocs), false);
      this.existingAttachmentsFromEFolder = info.GetBoolean(nameof (ExistingAttachmentsFromEFolder), false);
      XmlList<string> xmlList = (XmlList<string>) info.GetValue(nameof (PropertyStates), typeof (XmlList<string>), (object) null);
      if (xmlList != null)
        this.propertyStates = xmlList.ToArray();
      else
        this.propertyStates = (string[]) null;
    }

    public bool Enabled
    {
      get => this.enabled;
      set => this.enabled = value;
    }

    public bool RequestedDocuments
    {
      get => this.requestedDocuments;
      set => this.requestedDocuments = value;
    }

    public bool OpeningDocuments
    {
      get => this.openingDocuments;
      set => this.openingDocuments = value;
    }

    public bool PreClosingDocuments
    {
      get => this.preClosingDocuments;
      set => this.preClosingDocuments = value;
    }

    public bool ClosingDocuments
    {
      get => this.closingDocuments;
      set => this.closingDocuments = value;
    }

    public bool ExistingAttachmentsFromEFolder
    {
      get => this.existingAttachmentsFromEFolder;
      set => this.existingAttachmentsFromEFolder = value;
    }

    public string[] PropertyStates
    {
      get => this.propertyStates;
      set => this.propertyStates = value;
    }

    public bool RequestedDocumentsInfoDocs
    {
      get => this.requestedDocumentsInfoDocs;
      set => this.requestedDocumentsInfoDocs = value;
    }

    public bool OpeningDocumentsInfoDocs
    {
      get => this.openingDocumentsInfoDocs;
      set => this.openingDocumentsInfoDocs = value;
    }

    public bool PreClosingDocumentsInfoDocs
    {
      get => this.preClosingDocumentsInfoDocs;
      set => this.preClosingDocumentsInfoDocs = value;
    }

    public bool ClosingDocumentsInfoDocs
    {
      get => this.closingDocumentsInfoDocs;
      set => this.closingDocumentsInfoDocs = value;
    }

    public bool CheckRequestedDocuments(LoanData loanData)
    {
      return this.enabled && this.requestedDocuments && this.checkPropertyState(loanData);
    }

    public bool CheckOpeningDocuments(LoanData loanData)
    {
      return this.enabled && this.openingDocuments && this.checkPropertyState(loanData);
    }

    public bool CheckPreClosingDocuments(LoanData loanData)
    {
      return this.enabled && this.preClosingDocuments && this.checkPropertyState(loanData);
    }

    public bool CheckClosingDocuments(LoanData loanData)
    {
      return this.enabled && this.closingDocuments && this.checkPropertyState(loanData);
    }

    public bool CheckExistingAttachmentsFromEFolder(LoanData loanData)
    {
      return this.enabled && this.existingAttachmentsFromEFolder && this.checkPropertyState(loanData);
    }

    private bool checkPropertyState(LoanData loanData)
    {
      return this.propertyStates == null || Array.IndexOf<string>(this.propertyStates, loanData.GetSimpleField("14")) != -1;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Enabled", (object) this.enabled);
      info.AddValue("RequestedDocuments", (object) this.requestedDocuments);
      info.AddValue("OpeningDocuments", (object) this.openingDocuments);
      info.AddValue("PreClosingDocuments", (object) this.preClosingDocuments);
      info.AddValue("ClosingDocuments", (object) this.closingDocuments);
      info.AddValue("RequestedDocumentsInfoDocs", (object) this.requestedDocumentsInfoDocs);
      info.AddValue("OpeningDocumentsInfoDocs", (object) this.openingDocumentsInfoDocs);
      info.AddValue("PreClosingDocumentsInfoDocs", (object) this.PreClosingDocumentsInfoDocs);
      info.AddValue("ClosingDocumentsInfoDocs", (object) this.closingDocumentsInfoDocs);
      info.AddValue("ExistingAttachmentsFromEFolder", (object) this.existingAttachmentsFromEFolder);
      if (this.propertyStates != null)
        info.AddValue("PropertyStates", (object) new XmlList<string>((IEnumerable<string>) this.propertyStates));
      else
        info.AddValue("PropertyStates", (object) null);
    }

    public static BarcodeSetup GetBarcodeSetup(ISession session)
    {
      return BarcodeSetup.GetBarcodeSetup(session, true);
    }

    public static BarcodeSetup GetBarcodeSetup(ISession session, bool useCachedCopy)
    {
      if (!useCachedCopy || BarcodeSetup._barcodeSetup == null)
      {
        BinaryObject systemSettings = ((IConfigurationManager) session.GetObject("ConfigurationManager")).GetSystemSettings("Barcodes");
        BarcodeSetup._barcodeSetup = systemSettings == null ? new BarcodeSetup() : systemSettings.ToObject<BarcodeSetup>();
      }
      return BarcodeSetup._barcodeSetup;
    }

    public static void SaveBarcodeSetup(ISession session, BarcodeSetup barcodeSetup)
    {
      ((IConfigurationManager) session.GetObject("ConfigurationManager")).SaveSystemSettings("Barcodes", (BinaryObject) (BinaryConvertibleObject) barcodeSetup);
      BarcodeSetup._barcodeSetup = barcodeSetup;
    }
  }
}
