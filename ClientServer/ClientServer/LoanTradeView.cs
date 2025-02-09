// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanTradeView
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
  public class LoanTradeView : BinaryConvertible<LoanTradeView>, ITemplateSetting
  {
    private string name;
    private FieldFilterList filter;
    private TableLayout layout;

    public LoanTradeView()
    {
    }

    public LoanTradeView(string name) => this.name = name;

    public LoanTradeView(XmlSerializationInfo info)
    {
      this.name = info.GetString(nameof (name));
      this.filter = (FieldFilterList) info.GetValue(nameof (filter), typeof (FieldFilterList));
      this.layout = (TableLayout) info.GetValue(nameof (layout), typeof (TableLayout));
    }

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

    public override string ToString() => this.Name;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("name", (object) this.name);
      info.AddValue("filter", (object) this.filter);
      info.AddValue("layout", (object) this.layout);
    }

    public static explicit operator LoanTradeView(BinaryObject binaryObject)
    {
      return BinaryConvertible<LoanTradeView>.Parse(binaryObject);
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

    public ITemplateSetting Duplicate() => (ITemplateSetting) this.Clone();
  }
}
