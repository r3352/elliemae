// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Licensing.ApiRegistrationInfo
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.Encompass.Licensing
{
  /// <remarks />
  /// <exclude />
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://encompass.elliemae.com/jedservices/")]
  [ComVisible(false)]
  [Serializable]
  public class ApiRegistrationInfo
  {
    private string clientIDField;
    private bool autoAuthorizeSessionsField;

    /// <remarks />
    public string ClientID
    {
      get => this.clientIDField;
      set => this.clientIDField = value;
    }

    /// <remarks />
    public bool AutoAuthorizeSessions
    {
      get => this.autoAuthorizeSessionsField;
      set => this.autoAuthorizeSessionsField = value;
    }
  }
}
