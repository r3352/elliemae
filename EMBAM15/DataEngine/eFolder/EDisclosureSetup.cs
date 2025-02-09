// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.EDisclosureSetup
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class EDisclosureSetup : IXmlSerializable
  {
    private EDisclosureChannel retailChannel;
    private EDisclosureChannel wholesaleChannel;
    private EDisclosureChannel brokerChannel;
    private EDisclosureChannel correspondentChannel;
    private LoanChannel defaultChannel;
    private bool allowESigningConventional;
    private bool allowESigningFHA;
    private bool allowESigningVA;
    private bool allowESigningUSDA;
    private bool allowESigningOther;
    private bool allowESigningHELOC;
    private string consentModelType = string.Empty;
    private bool useBranchAddress;
    private List<EllieMae.EMLite.DataEngine.AutoRetrieveSettings> autoRetrieveSettings;

    public EDisclosureSetup()
    {
      this.retailChannel = new EDisclosureChannel();
      this.wholesaleChannel = new EDisclosureChannel();
      this.brokerChannel = new EDisclosureChannel();
      this.correspondentChannel = new EDisclosureChannel();
      this.defaultChannel = LoanChannel.BankedRetail;
      this.allowESigningConventional = true;
      this.allowESigningFHA = true;
      this.allowESigningVA = true;
      this.allowESigningUSDA = true;
      this.allowESigningOther = true;
      this.allowESigningHELOC = true;
      this.consentModelType = string.Empty;
      this.useBranchAddress = false;
      this.autoRetrieveSettings = (List<EllieMae.EMLite.DataEngine.AutoRetrieveSettings>) null;
    }

    public EDisclosureSetup(XmlSerializationInfo info)
    {
      this.retailChannel = (EDisclosureChannel) info.GetValue(nameof (RetailChannel), typeof (EDisclosureChannel));
      this.wholesaleChannel = (EDisclosureChannel) info.GetValue(nameof (WholesaleChannel), typeof (EDisclosureChannel));
      this.brokerChannel = (EDisclosureChannel) info.GetValue(nameof (BrokerChannel), typeof (EDisclosureChannel));
      this.correspondentChannel = (EDisclosureChannel) info.GetValue(nameof (CorrespondentChannel), typeof (EDisclosureChannel));
      this.defaultChannel = info.GetEnum<LoanChannel>(nameof (DefaultChannel));
      bool boolean = info.GetBoolean("AllowElectronicSignatures", true);
      this.allowESigningConventional = info.GetBoolean(nameof (AllowESigningConventional), boolean);
      this.allowESigningFHA = info.GetBoolean(nameof (AllowESigningFHA), boolean);
      this.allowESigningVA = info.GetBoolean(nameof (AllowESigningVA), boolean);
      this.allowESigningUSDA = info.GetBoolean(nameof (AllowESigningUSDA), boolean);
      this.allowESigningOther = info.GetBoolean(nameof (AllowESigningOther), boolean);
      this.allowESigningHELOC = info.GetBoolean(nameof (AllowESigningHELOC), boolean);
      this.consentModelType = info.GetString(nameof (ConsentModelType), string.Empty);
      if (string.IsNullOrEmpty(this.consentModelType))
        this.consentModelType = "Loan level consent";
      this.useBranchAddress = info.GetBoolean(nameof (UseBranchAddress), false);
      this.autoRetrieveSettings = (List<EllieMae.EMLite.DataEngine.AutoRetrieveSettings>) info.GetValue(nameof (AutoRetrieveSettings), typeof (XmlList<EllieMae.EMLite.DataEngine.AutoRetrieveSettings>), (object) null);
    }

    public EDisclosureChannel RetailChannel
    {
      get => this.retailChannel;
      set => this.retailChannel = value;
    }

    public EDisclosureChannel WholesaleChannel
    {
      get => this.wholesaleChannel;
      set => this.wholesaleChannel = value;
    }

    public EDisclosureChannel BrokerChannel
    {
      get => this.brokerChannel;
      set => this.brokerChannel = value;
    }

    public EDisclosureChannel CorrespondentChannel
    {
      get => this.correspondentChannel;
      set => this.correspondentChannel = value;
    }

    public LoanChannel DefaultChannel
    {
      get => this.defaultChannel;
      set => this.defaultChannel = value;
    }

    public bool AllowESigningConventional
    {
      get => this.allowESigningConventional;
      set => this.allowESigningConventional = value;
    }

    public bool AllowESigningFHA
    {
      get => this.allowESigningFHA;
      set => this.allowESigningFHA = value;
    }

    public bool AllowESigningVA
    {
      get => this.allowESigningVA;
      set => this.allowESigningVA = value;
    }

    public bool AllowESigningUSDA
    {
      get => this.allowESigningUSDA;
      set => this.allowESigningUSDA = value;
    }

    public bool AllowESigningOther
    {
      get => this.allowESigningOther;
      set => this.allowESigningOther = value;
    }

    public bool AllowESigningHELOC
    {
      get => this.allowESigningHELOC;
      set => this.allowESigningHELOC = value;
    }

    public bool AllowESigning(LoanData loanData)
    {
      switch (loanData.GetField("1172"))
      {
        case "Conventional":
          return this.allowESigningConventional;
        case "FHA":
          return this.allowESigningFHA;
        case "VA":
          return this.allowESigningVA;
        case "Other":
          return this.allowESigningOther;
        case "FarmersHomeAdministration":
          return this.allowESigningUSDA;
        case "HELOC":
          return this.allowESigningHELOC;
        default:
          return false;
      }
    }

    public EDisclosureChannel GetChannel(LoanData loanData)
    {
      LoanChannel defaultChannel = (LoanChannel) new LoanChannelNameProvider().GetValue(loanData.GetField("2626"));
      if (defaultChannel == LoanChannel.None)
        defaultChannel = this.defaultChannel;
      switch (defaultChannel - 1)
      {
        case LoanChannel.None:
          return this.retailChannel;
        case LoanChannel.BankedRetail:
          return this.wholesaleChannel;
        case LoanChannel.BankedWholesale:
          return this.brokerChannel;
        case LoanChannel.Brokered:
          return this.correspondentChannel;
        default:
          return (EDisclosureChannel) null;
      }
    }

    public EDisclosurePackage[] GetChannelPackages(LoanData loanData, MilestoneTemplate template)
    {
      EDisclosureChannel channel = this.GetChannel(loanData);
      string channelID = (string) null;
      if (this.retailChannel == channel)
        channelID = "RetailChannel";
      if (this.wholesaleChannel == channel)
        channelID = "WholesaleChannel";
      if (this.brokerChannel == channel)
        channelID = "BrokerChannel";
      if (this.correspondentChannel == channel)
        channelID = "CorrespondentChannel";
      return channel.GetPackages(loanData, template, channelID);
    }

    public string ConsentModelType
    {
      get => this.consentModelType;
      set => this.consentModelType = value;
    }

    public bool UseBranchAddress
    {
      get => this.useBranchAddress;
      set => this.useBranchAddress = value;
    }

    public List<EllieMae.EMLite.DataEngine.AutoRetrieveSettings> AutoRetrieveSettings
    {
      get => this.autoRetrieveSettings;
      set => this.autoRetrieveSettings = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("RetailChannel", (object) this.retailChannel);
      info.AddValue("WholesaleChannel", (object) this.wholesaleChannel);
      info.AddValue("BrokerChannel", (object) this.brokerChannel);
      info.AddValue("CorrespondentChannel", (object) this.correspondentChannel);
      info.AddValue("DefaultChannel", (object) this.defaultChannel);
      info.AddValue("AllowESigningConventional", (object) this.allowESigningConventional);
      info.AddValue("AllowESigningFHA", (object) this.allowESigningFHA);
      info.AddValue("AllowESigningVA", (object) this.allowESigningVA);
      info.AddValue("AllowESigningUSDA", (object) this.allowESigningUSDA);
      info.AddValue("AllowESigningOther", (object) this.allowESigningOther);
      info.AddValue("AllowESigningHELOC", (object) this.allowESigningHELOC);
      info.AddValue("ConsentModelType", (object) this.consentModelType);
      info.AddValue("UseBranchAddress", (object) this.useBranchAddress);
      info.AddValue("AutoRetrieveSettings", (object) new XmlList<EllieMae.EMLite.DataEngine.AutoRetrieveSettings>((IEnumerable<EllieMae.EMLite.DataEngine.AutoRetrieveSettings>) this.autoRetrieveSettings));
    }
  }
}
