// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.PipelineElementData
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class PipelineElementData
  {
    private string fieldName;
    private PipelineInfo pinfo;

    public PipelineElementData(string fieldName, PipelineInfo pinfo)
    {
      this.fieldName = fieldName;
      this.pinfo = pinfo;
    }

    public string FieldName => this.fieldName;

    public PipelineInfo PipelineInfo => this.pinfo;

    public object GetValue() => this.pinfo.Info[(object) this.fieldName];

    public override string ToString() => string.Concat(this.GetValue());
  }
}
