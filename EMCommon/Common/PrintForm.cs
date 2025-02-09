// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.PrintForm
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common.Licensing;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class PrintForm
  {
    public readonly string FormID;
    public readonly string UIName;
    public readonly string Source;
    public readonly string FileName;
    public string FormVersion = "";
    public string URLAFormVersion = "";
    public string ExcludeFromPipline = "";
    public readonly EncompassEdition Edition;
    public readonly PrintForm.MergeLocationValues MergeLocation;
    private MergeParamValues _MergeParams = new MergeParamValues();

    public MergeParamValues MergeParams
    {
      get => this._MergeParams;
      set => this._MergeParams = new MergeParamValues(value);
    }

    public PrintForm(XmlElement e)
    {
      this.FormID = e.GetAttribute("key");
      this.UIName = e.GetAttribute(nameof (UIName));
      this.Source = e.GetAttribute("SourceName");
      this.FileName = e.GetAttribute("value");
      string attribute1 = e.GetAttribute(nameof (Edition));
      if (!string.IsNullOrEmpty(e.GetAttribute(nameof (FormVersion))))
        this.FormVersion = e.GetAttribute(nameof (FormVersion));
      if (!string.IsNullOrEmpty(e.GetAttribute(nameof (URLAFormVersion))))
        this.URLAFormVersion = e.GetAttribute(nameof (URLAFormVersion));
      if (!string.IsNullOrEmpty(e.GetAttribute("ExcludeFromPipeline")))
        this.ExcludeFromPipline = e.GetAttribute("ExcludeFromPipeline");
      this.Edition = string.IsNullOrEmpty(attribute1) ? EncompassEdition.None : (EncompassEdition) Enum.Parse(typeof (EncompassEdition), attribute1, true);
      string attribute2 = e.GetAttribute(nameof (MergeLocation));
      this.MergeLocation = string.IsNullOrEmpty(attribute2) ? PrintForm.MergeLocationValues.Local : (PrintForm.MergeLocationValues) Enum.Parse(typeof (PrintForm.MergeLocationValues), attribute2, true);
      this._MergeParams.Load((XmlElement) e.SelectSingleNode(nameof (MergeParams)));
    }

    public bool AppliesToEdition(EncompassEdition edition)
    {
      return this.Edition == EncompassEdition.None || edition == this.Edition;
    }

    public enum MergeLocationValues
    {
      Local,
      EDS,
    }
  }
}
