// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeFilterTemplate
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradeFilterTemplate : BinaryConvertible<TradeFilterTemplate>, ITemplateSetting
  {
    private string guid;
    private string name;
    private string description;
    private TradeFilter filter;

    public TradeFilterTemplate(string name, string description, TradeFilter filter)
    {
      this.guid = System.Guid.NewGuid().ToString();
      this.name = name;
      this.description = description;
      this.filter = filter;
    }

    public TradeFilterTemplate(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (guid));
      this.name = info.GetString(nameof (name));
      this.description = info.GetString("desc");
      this.filter = (TradeFilter) info.GetValue(nameof (filter), typeof (TradeFilter));
    }

    public string Guid => this.guid;

    public string TemplateName
    {
      get => this.name;
      set => this.name = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public TradeFilter Filter => this.filter;

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "Guid", (object) this.guid);
      insensitiveHashtable.Add((object) "Type", (object) this.filter.FilterType.ToString());
      insensitiveHashtable.Add((object) "Description", (object) this.description);
      return insensitiveHashtable;
    }

    public virtual ITemplateSetting Duplicate()
    {
      TradeFilterTemplate tradeFilterTemplate = (TradeFilterTemplate) this.Clone();
      tradeFilterTemplate.guid = System.Guid.NewGuid().ToString();
      tradeFilterTemplate.TemplateName = "";
      return (ITemplateSetting) tradeFilterTemplate;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("guid", (object) this.guid);
      info.AddValue("name", (object) this.name);
      info.AddValue("desc", (object) this.description);
      info.AddValue("filter", (object) this.filter);
    }

    public static explicit operator TradeFilterTemplate(BinaryObject o)
    {
      return BinaryConvertible<TradeFilterTemplate>.Parse(o);
    }
  }
}
