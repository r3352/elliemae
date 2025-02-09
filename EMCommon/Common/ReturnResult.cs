// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ReturnResult
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [GeneratedCode("System.Xml", "4.8.3752.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://hosted.elliemae.com/")]
  [Serializable]
  public class ReturnResult
  {
    private ReturnCode returnCodeField;
    private string descriptionField;
    private string installUrlIDField;
    private bool updateByEMField;
    private string[] testCIDsField;

    public ReturnCode ReturnCode
    {
      get => this.returnCodeField;
      set => this.returnCodeField = value;
    }

    public string Description
    {
      get => this.descriptionField;
      set => this.descriptionField = value;
    }

    public string InstallUrlID
    {
      get => this.installUrlIDField;
      set => this.installUrlIDField = value;
    }

    public bool UpdateByEM
    {
      get => this.updateByEMField;
      set => this.updateByEMField = value;
    }

    public string[] TestCIDs
    {
      get => this.testCIDsField;
      set => this.testCIDsField = value;
    }
  }
}
