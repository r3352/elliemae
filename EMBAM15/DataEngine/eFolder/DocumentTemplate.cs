// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.DocumentTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class DocumentTemplate : IXmlSerializable, IIdentifiable, IHashableContents
  {
    private string guid;
    private string name = string.Empty;
    private string description = string.Empty;
    private string sourceType = "Needed";
    private string source = string.Empty;
    private int daysTillDue;
    private int daysTillExpire;
    private int isCondition;
    private string conditionInfo = string.Empty;
    private bool efolder = true;
    private bool openingDocument;
    private bool preClosingDocument;
    private bool closingDocument;
    private OpeningCriteria openingCriteria;
    private PreClosingCriteria preClosingCriteria;
    private DocumentCriteria closingCriteria;
    private bool isWebcenter = true;
    private bool isTPOWebcenterPortal = true;
    private bool isThirdPartyDoc = true;
    private string signatureType = string.Empty;
    private ImageConversionType conversionType;
    private bool saveOriginalFormat;
    private string sourceBorrower = string.Empty;
    private string sourceCoborrower = string.Empty;

    public DocumentTemplate(string name)
    {
      this.guid = System.Guid.NewGuid().ToString();
      this.name = name;
    }

    public DocumentTemplate(string guid, string name)
    {
      this.guid = guid;
      this.name = name;
    }

    public DocumentTemplate(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (Guid), System.Guid.NewGuid().ToString());
      this.name = info.GetString(nameof (Name));
      this.description = info.GetString(nameof (Description), string.Empty);
      this.sourceType = info.GetString("Type", "Needed");
      this.daysTillDue = info.GetInteger(nameof (DaysTillDue));
      this.daysTillExpire = info.GetInteger(nameof (DaysTillExpire));
      this.isCondition = info.GetInteger(nameof (IsCondition), 0);
      this.conditionInfo = info.GetString(nameof (ConditionInfo), string.Empty);
      this.efolder = info.GetBoolean(nameof (eFolder), true);
      this.conversionType = info.GetEnum<ImageConversionType>(nameof (ConversionType), ImageConversionType.BlackAndWhite);
      this.saveOriginalFormat = info.GetBoolean(nameof (SaveOriginalFormat), false);
      if (this.IsPrintable)
      {
        this.source = info.GetString(nameof (Source), string.Empty);
        this.sourceBorrower = info.GetString(nameof (SourceBorrower), string.Empty);
        this.sourceCoborrower = info.GetString(nameof (SourceCoborrower), string.Empty);
        this.openingDocument = info.GetBoolean("eDisclosure", false);
        this.preClosingDocument = info.GetBoolean(nameof (PreClosingDocument), false);
        this.closingDocument = info.GetBoolean(nameof (ClosingDocument), false);
        this.openingCriteria = (OpeningCriteria) info.GetValue(nameof (OpeningCriteria), typeof (OpeningCriteria), (object) null);
        this.preClosingCriteria = (PreClosingCriteria) info.GetValue(nameof (PreClosingCriteria), typeof (PreClosingCriteria), (object) null);
        this.closingCriteria = (DocumentCriteria) info.GetValue("DocumentCriteria", typeof (DocumentCriteria), (object) null);
        this.signatureType = info.GetString(nameof (SignatureType), string.Empty);
        if (string.IsNullOrEmpty(this.signatureType))
          this.signatureType = "eSignable";
      }
      if (!string.IsNullOrEmpty(info.GetString(nameof (IsWebcenter), (string) null)))
      {
        this.isWebcenter = info.GetBoolean(nameof (IsWebcenter), true);
        this.isTPOWebcenterPortal = info.GetBoolean(nameof (IsTPOWebcenterPortal), true);
        this.isThirdPartyDoc = info.GetBoolean(nameof (IsThirdPartyDoc), true);
      }
      else
      {
        this.isWebcenter = info.GetBoolean("IsExternal", true);
        this.isTPOWebcenterPortal = this.isWebcenter;
        this.isThirdPartyDoc = this.isWebcenter;
      }
    }

    int IHashableContents.GetContentsHashCode()
    {
      return ObjectArrayHelpers.GetAggregateHash((object) this.Guid, (object) this.Name, (object) this.Description, (object) this.SourceType, (object) this.Source, (object) this.DaysTillDue, (object) this.DaysTillExpire, (object) this.IsCondition, (object) this.ConditionInfo, (object) this.eFolder, (object) this.OpeningDocument, (object) this.PreClosingDocument, (object) this.ClosingDocument, (object) this.IsWebcenter, (object) this.IsTPOWebcenterPortal, (object) this.IsThirdPartyDoc, (object) this.SignatureType, (object) (int) this.ConversionType, (object) this.SaveOriginalFormat, (object) this.SourceBorrower, (object) this.SourceCoborrower, (object) this.ClosingCriteria, (object) this.PreClosingCriteria, (object) this.OpeningCriteria);
    }

    public string Guid => this.guid;

    public string Name => this.name;

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public string SourceType
    {
      get => this.sourceType;
      set => this.sourceType = value;
    }

    public string Source
    {
      get => this.source;
      set => this.source = value;
    }

    public int DaysTillDue
    {
      get => this.daysTillDue;
      set => this.daysTillDue = value;
    }

    public int DaysTillExpire
    {
      get => this.daysTillExpire;
      set => this.daysTillExpire = value;
    }

    public int IsCondition
    {
      get => this.isCondition;
      set => this.isCondition = value;
    }

    public string ConditionInfo => this.conditionInfo;

    public bool eFolder => this.efolder;

    public bool OpeningDocument
    {
      get => this.openingDocument;
      set => this.openingDocument = value;
    }

    public OpeningCriteria OpeningCriteria
    {
      get => this.openingCriteria;
      set => this.openingCriteria = value;
    }

    public bool PreClosingDocument
    {
      get => this.preClosingDocument;
      set => this.preClosingDocument = value;
    }

    public PreClosingCriteria PreClosingCriteria
    {
      get => this.preClosingCriteria;
      set => this.preClosingCriteria = value;
    }

    public bool ClosingDocument
    {
      get => this.closingDocument;
      set => this.closingDocument = value;
    }

    public DocumentCriteria ClosingCriteria
    {
      get => this.closingCriteria;
      set => this.closingCriteria = value;
    }

    public bool IsWebcenter
    {
      get => this.isWebcenter;
      set => this.isWebcenter = value;
    }

    public bool IsTPOWebcenterPortal
    {
      get => this.isTPOWebcenterPortal;
      set => this.isTPOWebcenterPortal = value;
    }

    public bool IsThirdPartyDoc
    {
      get => this.isThirdPartyDoc;
      set => this.isThirdPartyDoc = value;
    }

    public string SignatureType
    {
      get => this.signatureType;
      set => this.signatureType = value;
    }

    public ImageConversionType ConversionType
    {
      get => this.conversionType;
      set => this.conversionType = value;
    }

    public bool SaveOriginalFormat
    {
      get => this.saveOriginalFormat;
      set => this.saveOriginalFormat = value;
    }

    public string SourceBorrower
    {
      get => this.sourceBorrower;
      set => this.sourceBorrower = value;
    }

    public string SourceCoborrower
    {
      get => this.sourceCoborrower;
      set => this.sourceCoborrower = value;
    }

    public DocumentLog CreateLogEntry(string addedBy, string pairId)
    {
      return new DocumentLog(this, addedBy, pairId);
    }

    public bool IsPrintable
    {
      get
      {
        string sourceType = this.sourceType;
        return sourceType == "Standard Form" || sourceType == "Custom Form" || sourceType == "Borrower Specific Custom Form";
      }
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Guid", (object) this.guid);
      info.AddValue("Name", (object) this.name);
      info.AddValue("Description", (object) this.description);
      info.AddValue("Type", (object) this.sourceType);
      info.AddValue("DaysTillDue", (object) this.daysTillDue);
      info.AddValue("DaysTillExpire", (object) this.daysTillExpire);
      info.AddValue("IsCondition", (object) this.isCondition);
      info.AddValue("ConditionInfo", (object) this.conditionInfo);
      info.AddValue("eFolder", (object) this.efolder);
      info.AddValue("IsWebcenter", (object) this.isWebcenter);
      info.AddValue("IsTPOWebcenterPortal", (object) this.isTPOWebcenterPortal);
      info.AddValue("IsThirdPartyDoc", (object) this.isThirdPartyDoc);
      info.AddValue("ConversionType", (object) this.conversionType);
      info.AddValue("SaveOriginalFormat", (object) this.saveOriginalFormat);
      if (!this.IsPrintable)
        return;
      info.AddValue("Source", (object) this.source);
      info.AddValue("SourceBorrower", (object) this.sourceBorrower);
      info.AddValue("SourceCoborrower", (object) this.sourceCoborrower);
      info.AddValue("eDisclosure", (object) this.openingDocument);
      info.AddValue("PreClosingDocument", (object) this.preClosingDocument);
      info.AddValue("PreClosingCriteria", (object) this.preClosingCriteria);
      info.AddValue("ClosingDocument", (object) this.closingDocument);
      info.AddValue("DocumentCriteria", (object) this.closingCriteria);
      info.AddValue("OpeningCriteria", (object) this.openingCriteria);
      info.AddValue("SignatureType", (object) this.signatureType);
    }

    public override string ToString() => this.name;
  }
}
