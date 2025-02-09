// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.PipelineViewList
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class PipelineViewList : 
    BinaryConvertible<PipelineViewList>,
    ITemplateSetting,
    IComparable<PipelineViewList>
  {
    private const string className = "PipelineViewListItem";
    private PipelineView pipelineView;

    public PipelineViewList(PipelineView p) => this.pipelineView = p;

    public PipelineView PipelineView => this.pipelineView;

    public override string ToString() => this.pipelineView.Name;

    string ITemplateSetting.TemplateName
    {
      get => this.pipelineView.Name;
      set => this.pipelineView.Name = value;
    }

    public string Description
    {
      get => "";
      set
      {
      }
    }

    public Hashtable GetProperties()
    {
      return new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);
    }

    public int CompareTo(PipelineViewList other)
    {
      if (other == null)
        throw new Exception("Invalid value for comparison");
      return string.Compare(this.pipelineView.Name, other.pipelineView.Name, true);
    }

    public ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("PipelineView", (object) this.PipelineView);
    }

    public static explicit operator PipelineViewList(BinaryObject binaryObject)
    {
      return BinaryConvertible<PipelineViewList>.Parse(binaryObject);
    }

    public PipelineViewList(XmlSerializationInfo info)
    {
      this.pipelineView = (PipelineView) info.GetValue(nameof (PipelineView), typeof (PipelineView));
    }
  }
}
