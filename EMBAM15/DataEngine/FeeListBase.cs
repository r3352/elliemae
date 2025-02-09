// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FeeListBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FeeListBase : BinaryConvertibleObject
  {
    private Hashtable tbl = new Hashtable();
    private ArrayList nameList = new ArrayList();

    public FeeListBase()
    {
    }

    public FeeListBase(XmlSerializationInfo info)
    {
      foreach (string name in info)
        this.InsertFee((FeeListBase.FeeTable) info.GetValue(name, typeof (FeeListBase.FeeTable)));
    }

    public virtual void InsertFee(string name, string calcBasedOn, string rate, string additional)
    {
      this.InsertFee(new FeeListBase.FeeTable(name, calcBasedOn, rate, additional));
    }

    public virtual void InsertFee(FeeListBase.FeeTable fee)
    {
      this.nameList.Add((object) fee.FeeName);
      this.tbl.Add((object) fee.FeeName, (object) fee);
    }

    public virtual void UpdateFee(
      string name,
      string newName,
      string calcBasedOn,
      string rate,
      string additional)
    {
      this.nameList.Remove((object) name);
      FeeListBase.FeeTable feeTable = (FeeListBase.FeeTable) this.tbl[(object) name];
      this.tbl.Remove((object) name);
      feeTable.FeeName = newName;
      feeTable.CalcBasedOn = calcBasedOn;
      feeTable.Rate = rate;
      feeTable.Additional = additional;
      this.nameList.Add((object) newName);
      this.tbl.Add((object) newName, (object) feeTable);
    }

    public int Count => this.tbl.Count;

    public FeeListBase.FeeTable[] FeeList
    {
      get => this.tbl.Values.Cast<FeeListBase.FeeTable>().ToArray<FeeListBase.FeeTable>();
    }

    public virtual FeeListBase.FeeTable GetTableAt(int i)
    {
      return (FeeListBase.FeeTable) this.tbl[this.nameList[i]];
    }

    public virtual FeeListBase.FeeTable GetTable(string name)
    {
      return (FeeListBase.FeeTable) this.tbl[(object) name];
    }

    public virtual FeeListBase.FeeTable GetTableNonCaseSensitive(string name)
    {
      try
      {
        return (FeeListBase.FeeTable) this.tbl[(object) this.tbl.Keys.Cast<string>().Where<string>((Func<string, bool>) (a => a.Trim().Equals(name.ToLower().Trim(), StringComparison.OrdinalIgnoreCase))).First<string>()];
      }
      catch (Exception ex)
      {
      }
      return (FeeListBase.FeeTable) null;
    }

    public virtual bool TableNameExists(string name)
    {
      return this.tbl != null && this.tbl.Keys.Cast<string>().Where<string>((Func<string, bool>) (a => a.Trim().Equals(name.ToLower().Trim(), StringComparison.OrdinalIgnoreCase))).Count<string>() > 0;
    }

    public virtual void RemoveTable(string name)
    {
      this.tbl.Remove((object) name);
      this.nameList.Remove((object) name);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      for (int index = 0; index < this.nameList.Count; ++index)
        info.AddValue(index.ToString(), this.tbl[this.nameList[index]]);
    }

    public static explicit operator FeeListBase(BinaryObject obj)
    {
      return obj == null ? (FeeListBase) null : (FeeListBase) BinaryConvertibleObject.Parse(obj, typeof (FeeListBase));
    }

    [Serializable]
    public class FeeTable : IXmlSerializable
    {
      public string FeeName;
      public string CalcBasedOn;
      public string Rate;
      public string Additional;

      public FeeTable(string name, string calcBasedOn, string rate, string additional)
      {
        this.FeeName = name;
        this.CalcBasedOn = calcBasedOn;
        this.Rate = rate;
        this.Additional = additional;
      }

      public FeeTable(XmlSerializationInfo info)
      {
        this.FeeName = info.GetString(nameof (FeeName));
        this.CalcBasedOn = info.GetString(nameof (CalcBasedOn));
        this.Rate = info.GetString(nameof (Rate));
        this.Additional = info.GetString(nameof (Additional));
      }

      public virtual void GetXmlObjectData(XmlSerializationInfo info)
      {
        info.AddValue("FeeName", (object) this.FeeName);
        info.AddValue("CalcBasedOn", (object) this.CalcBasedOn);
        info.AddValue("Rate", (object) this.Rate);
        info.AddValue("Additional", (object) this.Additional);
      }
    }
  }
}
