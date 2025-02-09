// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SCPackageInfo
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
  public class SCPackageInfo
  {
    private string installUrlIDField;
    private string encVersionField;
    private string descriptionField;
    private DateTime releaseDateField;
    private string releaseNotesURLField;
    private bool isCurrentField;

    public string InstallUrlID
    {
      get => this.installUrlIDField;
      set => this.installUrlIDField = value;
    }

    public string EncVersion
    {
      get => this.encVersionField;
      set => this.encVersionField = value;
    }

    public string Description
    {
      get => this.descriptionField;
      set => this.descriptionField = value;
    }

    public DateTime ReleaseDate
    {
      get => this.releaseDateField;
      set => this.releaseDateField = value;
    }

    public string ReleaseNotesURL
    {
      get => this.releaseNotesURLField;
      set => this.releaseNotesURLField = value;
    }

    public bool IsCurrent
    {
      get => this.isCurrentField;
      set => this.isCurrentField = value;
    }
  }
}
