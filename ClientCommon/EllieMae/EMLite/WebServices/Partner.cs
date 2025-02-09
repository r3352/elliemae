// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.Partner
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://epass.elliemaeservices.com/PPEservices")]
  [Serializable]
  public class Partner
  {
    private string partnerIDField;
    private string settingsSectionField;
    private string nameField;
    private string adminURLField;
    private string moreInfoURLField;
    private bool supportsImportToLoanField;
    private bool supportsPartnerRequestLockField;
    private bool supportsPartnerLockConfirmField;
    private bool showSellSideField;
    private bool isCustomizeInvestorNameField;

    public string PartnerID
    {
      get => this.partnerIDField;
      set => this.partnerIDField = value;
    }

    public string SettingsSection
    {
      get => this.settingsSectionField;
      set => this.settingsSectionField = value;
    }

    public string Name
    {
      get => this.nameField;
      set => this.nameField = value;
    }

    public string AdminURL
    {
      get => this.adminURLField;
      set => this.adminURLField = value;
    }

    public string MoreInfoURL
    {
      get => this.moreInfoURLField;
      set => this.moreInfoURLField = value;
    }

    public bool SupportsImportToLoan
    {
      get => this.supportsImportToLoanField;
      set => this.supportsImportToLoanField = value;
    }

    public bool SupportsPartnerRequestLock
    {
      get => this.supportsPartnerRequestLockField;
      set => this.supportsPartnerRequestLockField = value;
    }

    public bool SupportsPartnerLockConfirm
    {
      get => this.supportsPartnerLockConfirmField;
      set => this.supportsPartnerLockConfirmField = value;
    }

    public bool ShowSellSide
    {
      get => this.showSellSideField;
      set => this.showSellSideField = value;
    }

    public bool IsCustomizeInvestorName
    {
      get => this.isCustomizeInvestorNameField;
      set => this.isCustomizeInvestorNameField = value;
    }
  }
}
