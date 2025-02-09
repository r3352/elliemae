// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MIRecordXML
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class MIRecordXML : BinaryConvertibleObject
  {
    private SortedList tbl = new SortedList();

    public MIRecordXML()
    {
    }

    public MIRecordXML(XmlSerializationInfo info)
    {
      foreach (string name in info)
        this.InsertMIRecord((MIRecord) info.GetValue(name, typeof (MIRecord)));
    }

    public MIRecord[] GetRecords()
    {
      if (this.tbl == null && this.tbl.Count == 0)
        return (MIRecord[]) null;
      MIRecord[] records = new MIRecord[this.tbl.Count];
      for (int index = 0; index < this.tbl.Count; ++index)
        records[index] = (MIRecord) this.tbl.GetByIndex(index);
      return records;
    }

    public void InsertMIRecord(MIRecord miRecord)
    {
      if (this.tbl.ContainsKey((object) miRecord.Id))
        return;
      this.tbl.Add((object) miRecord.Id, (object) miRecord);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      for (int index = 0; index < this.tbl.Count; ++index)
        info.AddValue(index.ToString(), this.tbl.GetByIndex(index));
    }

    public static explicit operator MIRecordXML(BinaryObject obj)
    {
      return (MIRecordXML) BinaryConvertibleObject.Parse(obj, typeof (MIRecordXML));
    }
  }
}
