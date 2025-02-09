// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.PriceAdjustmentTemplate
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
  public class PriceAdjustmentTemplate : BinaryConvertible<PriceAdjustmentTemplate>, ITemplateSetting
  {
    private string guid;
    private string name;
    private string description;
    private TradePriceAdjustments adjustments;

    public PriceAdjustmentTemplate()
      : this("", "")
    {
    }

    public PriceAdjustmentTemplate(string guid, string name, string description)
    {
      this.guid = guid;
      this.name = name;
      this.description = description;
      this.adjustments = new TradePriceAdjustments();
    }

    public PriceAdjustmentTemplate(string name, string description)
    {
      this.guid = System.Guid.NewGuid().ToString();
      this.name = name;
      this.description = description;
      this.adjustments = new TradePriceAdjustments();
    }

    public PriceAdjustmentTemplate(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (guid));
      this.name = info.GetString(nameof (name));
      this.description = info.GetString("desc");
      this.adjustments = (TradePriceAdjustments) info.GetValue(nameof (adjustments), typeof (TradePriceAdjustments));
    }

    private PriceAdjustmentTemplate(PriceAdjustmentTemplate source)
      : this()
    {
      this.description = source.description;
      this.adjustments = new TradePriceAdjustments(source.adjustments);
    }

    public string Guid
    {
      get => this.guid;
      set => this.guid = value;
    }

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

    public TradePriceAdjustments PriceAdjustments => this.adjustments;

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable[(object) "Guid"] = (object) this.guid;
      insensitiveHashtable[(object) "Description"] = (object) this.description;
      return insensitiveHashtable;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("guid", (object) this.guid);
      info.AddValue("name", (object) this.name);
      info.AddValue("desc", (object) this.description);
      info.AddValue("adjustments", (object) this.adjustments);
    }

    public ITemplateSetting Duplicate() => (ITemplateSetting) new PriceAdjustmentTemplate(this);

    public static explicit operator PriceAdjustmentTemplate(BinaryObject o)
    {
      return BinaryConvertible<PriceAdjustmentTemplate>.Parse(o);
    }
  }
}
