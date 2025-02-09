// Decompiled with JetBrains decompiler
// Type: EllieMae.EncompassAPI.WebServices.ReturnResult
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EncompassAPI.WebServices
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
