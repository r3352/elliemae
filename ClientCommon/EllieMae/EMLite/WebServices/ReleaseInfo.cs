// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.ReleaseInfo
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
  [XmlType(Namespace = "http://www.elliemae.com/encompass")]
  [Serializable]
  public class ReleaseInfo
  {
    private string majorVersionField;
    private int sequenceNumberField;
    private string descriptionField;
    private string hotfixURLField;
    private string releaseNotesURLField;
    private DateTime releaseDateField;

    public string MajorVersion
    {
      get => this.majorVersionField;
      set => this.majorVersionField = value;
    }

    public int SequenceNumber
    {
      get => this.sequenceNumberField;
      set => this.sequenceNumberField = value;
    }

    public string Description
    {
      get => this.descriptionField;
      set => this.descriptionField = value;
    }

    public string HotfixURL
    {
      get => this.hotfixURLField;
      set => this.hotfixURLField = value;
    }

    public string ReleaseNotesURL
    {
      get => this.releaseNotesURLField;
      set => this.releaseNotesURLField = value;
    }

    public DateTime ReleaseDate
    {
      get => this.releaseDateField;
      set => this.releaseDateField = value;
    }
  }
}
