// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.WebServices.CompanySettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.eFolder.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://loancenter.elliemae.com/eFolder")]
  [Serializable]
  public class CompanySettings
  {
    private string clientIDField;
    private string settingNameField;
    private bool settingValueField;

    public string ClientID
    {
      get => this.clientIDField;
      set => this.clientIDField = value;
    }

    public string SettingName
    {
      get => this.settingNameField;
      set => this.settingNameField = value;
    }

    public bool SettingValue
    {
      get => this.settingValueField;
      set => this.settingValueField = value;
    }
  }
}
