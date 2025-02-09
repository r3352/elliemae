// Decompiled with JetBrains decompiler
// Type: EllieMae.EncompassAPI.WebServices.SCPackageInfo
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EncompassAPI.WebServices
{
  /// <remarks />
  [GeneratedCode("System.Xml", "4.8.3752.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://hosted.elliemae.com/")]
  [Serializable]
  public class SCPackageInfo
  {
    private string installUrlIDField;
    private string encVersionField;
    private string descriptionField;
    private DateTime releaseDateField;
    private string releaseNotesURLField;
    private bool isCurrentField;

    /// <remarks />
    public string InstallUrlID
    {
      get => this.installUrlIDField;
      set => this.installUrlIDField = value;
    }

    /// <remarks />
    public string EncVersion
    {
      get => this.encVersionField;
      set => this.encVersionField = value;
    }

    /// <remarks />
    public string Description
    {
      get => this.descriptionField;
      set => this.descriptionField = value;
    }

    /// <remarks />
    public DateTime ReleaseDate
    {
      get => this.releaseDateField;
      set => this.releaseDateField = value;
    }

    /// <remarks />
    public string ReleaseNotesURL
    {
      get => this.releaseNotesURLField;
      set => this.releaseNotesURLField = value;
    }

    /// <remarks />
    public bool IsCurrent
    {
      get => this.isCurrentField;
      set => this.isCurrentField = value;
    }
  }
}
