// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.TableFeeListBase
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
  [CLSCompliant(true)]
  [Serializable]
  public class TableFeeListBase : BinaryConvertibleObject
  {
    private Hashtable tbl = new Hashtable();
    private ArrayList nameList = new ArrayList();

    public TableFeeListBase()
    {
    }

    public TableFeeListBase(XmlSerializationInfo info)
    {
      foreach (string name in info)
        this.InsertFee((TableFeeListBase.FeeTable) info.GetValue(name, typeof (TableFeeListBase.FeeTable)));
    }

    public virtual void InsertFee(
      string name,
      bool useThis,
      string calcBasedOn,
      string rounding,
      string nearest,
      string offset,
      string rateList,
      string feeType)
    {
      this.InsertFee(new TableFeeListBase.FeeTable(name, useThis, calcBasedOn, rounding, nearest, offset, rateList, feeType));
    }

    public virtual void InsertFee(TableFeeListBase.FeeTable fee)
    {
      this.nameList.Add((object) fee.TableName);
      this.tbl.Add((object) fee.TableName, (object) fee);
    }

    public virtual void UpdateFee(
      string name,
      string newName,
      bool useThis,
      string calcBasedOn,
      string rounding,
      string nearest,
      string offset,
      string rateList,
      string feeType)
    {
      this.nameList.Remove((object) name);
      TableFeeListBase.FeeTable feeTable = (TableFeeListBase.FeeTable) this.tbl[(object) name];
      this.tbl.Remove((object) name);
      feeTable.TableName = newName;
      feeTable.UseThis = useThis;
      feeTable.CalcBasedOn = calcBasedOn;
      feeTable.Rounding = rounding;
      feeTable.Nearest = nearest;
      feeTable.Offset = offset;
      feeTable.RateList = rateList;
      feeTable.FeeType = feeType;
      this.nameList.Add((object) newName);
      this.tbl.Add((object) newName, (object) feeTable);
    }

    public int Count => this.tbl.Count;

    public Hashtable FeeTables => this.tbl;

    public virtual TableFeeListBase.FeeTable GetTableAt(int i)
    {
      return (TableFeeListBase.FeeTable) this.tbl[this.nameList[i]];
    }

    public virtual TableFeeListBase.FeeTable GetTable(string name)
    {
      return (TableFeeListBase.FeeTable) this.tbl[(object) name];
    }

    public virtual bool TableNameExists(string name)
    {
      return this.tbl != null && this.tbl.Keys.Cast<string>().Where<string>((Func<string, bool>) (a => a.Trim().Equals(name.Trim().ToLower(), StringComparison.OrdinalIgnoreCase))).Count<string>() > 0;
    }

    public bool HasDefault(string feeType)
    {
      if (this.tbl == null)
        return false;
      foreach (DictionaryEntry dictionaryEntry in this.tbl)
      {
        TableFeeListBase.FeeTable feeTable = (TableFeeListBase.FeeTable) dictionaryEntry.Value;
        if (feeTable.UseThis && string.Compare(feeTable.FeeType, feeType, true) == 0)
          return true;
      }
      return false;
    }

    public void RemoveDefaultTable(string feeType)
    {
      if (this.tbl == null)
        return;
      foreach (DictionaryEntry dictionaryEntry in this.tbl)
      {
        TableFeeListBase.FeeTable feeTable = (TableFeeListBase.FeeTable) dictionaryEntry.Value;
        if (feeTable.UseThis && string.Compare(feeTable.FeeType, feeType, true) == 0)
          feeTable.UseThis = false;
      }
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

    public static explicit operator TableFeeListBase(BinaryObject obj)
    {
      return obj == null ? (TableFeeListBase) null : (TableFeeListBase) BinaryConvertibleObject.Parse(obj, typeof (TableFeeListBase));
    }

    [Serializable]
    public class FeeTable : IXmlSerializable
    {
      public bool UseThis;
      public string CalcBasedOn;
      public string Rounding;
      public string Rate;
      public string Nearest;
      public string Offset;
      public string RateList;
      public string FeeType;
      private string tableName = string.Empty;

      public string TableName
      {
        get => this.tableName;
        set => this.tableName = value;
      }

      public FeeTable(
        string name,
        bool useThis,
        string calcBasedOn,
        string rounding,
        string nearest,
        string offset,
        string rateList,
        string feeType)
      {
        this.TableName = name;
        this.UseThis = useThis;
        this.CalcBasedOn = calcBasedOn;
        this.Rounding = rounding;
        this.Nearest = nearest;
        this.Offset = offset;
        this.RateList = rateList;
        this.FeeType = feeType;
      }

      public FeeTable(XmlSerializationInfo info)
      {
        this.TableName = info.GetString(nameof (TableName));
        this.UseThis = info.GetBoolean(nameof (UseThis));
        this.CalcBasedOn = info.GetString(nameof (CalcBasedOn));
        this.Rounding = info.GetString(nameof (Rounding));
        this.Nearest = info.GetString(nameof (Nearest));
        this.Offset = info.GetString(nameof (Offset));
        this.RateList = info.GetString(nameof (RateList));
        try
        {
          this.FeeType = info.GetString(nameof (FeeType));
        }
        catch (Exception ex)
        {
          this.FeeType = "";
        }
      }

      public virtual void GetXmlObjectData(XmlSerializationInfo info)
      {
        info.AddValue("TableName", (object) this.TableName);
        info.AddValue("UseThis", (object) this.UseThis);
        info.AddValue("CalcBasedOn", (object) this.CalcBasedOn);
        info.AddValue("Rounding", (object) this.Rounding);
        info.AddValue("Nearest", (object) this.Nearest);
        info.AddValue("Offset", (object) this.Offset);
        info.AddValue("RateList", (object) this.RateList);
        info.AddValue("FeeType", (object) this.FeeType);
      }

      public object Clone()
      {
        return (object) new TableFeeListBase.FeeTable(this.tableName, this.UseThis, this.CalcBasedOn, this.Rounding, this.Nearest, this.Offset, this.RateList, this.FeeType);
      }
    }
  }
}
