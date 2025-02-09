// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DocumentTrackingView
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DocumentTrackingView : BinaryConvertible<DocumentTrackingView>, ITemplateSetting
  {
    private string name;
    private FieldFilterList filter;
    private TableLayout layout;
    private string docGroup;
    private string stackingOrder;

    public DocumentTrackingView()
    {
    }

    public DocumentTrackingView(string name) => this.name = name;

    public DocumentTrackingView(XmlSerializationInfo info)
    {
      this.name = info.GetString(nameof (name));
      this.filter = (FieldFilterList) info.GetValue(nameof (filter), typeof (FieldFilterList));
      this.layout = (TableLayout) info.GetValue(nameof (layout), typeof (TableLayout));
      this.docGroup = info.GetString("docgroup");
      this.stackingOrder = info.GetString("stackingorder");
    }

    public string Id { get; set; }

    public bool IsDefault { get; set; }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public FieldFilterList Filter
    {
      get => this.filter;
      set => this.filter = value;
    }

    public TableLayout Layout
    {
      get => this.layout;
      set => this.layout = value;
    }

    public string DocGroup
    {
      get => this.docGroup;
      set => this.docGroup = value;
    }

    public string StackingOrder
    {
      get => this.stackingOrder;
      set => this.stackingOrder = value;
    }

    public override string ToString() => this.Name;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("name", (object) this.name);
      info.AddValue("filter", (object) this.filter);
      info.AddValue("layout", (object) this.layout);
      info.AddValue("docgroup", (object) this.docGroup);
      info.AddValue("stackingorder", (object) this.stackingOrder);
    }

    string ITemplateSetting.TemplateName
    {
      get => this.name;
      set => this.name = value;
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

    public virtual ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public static explicit operator DocumentTrackingView(BinaryObject binaryObject)
    {
      return BinaryConvertible<DocumentTrackingView>.Parse(binaryObject);
    }
  }
}
