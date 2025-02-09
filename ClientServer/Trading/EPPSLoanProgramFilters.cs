// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.EPPSLoanProgramFilters
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [CLSCompliant(true)]
  [Serializable]
  public class EPPSLoanProgramFilters : 
    BinaryConvertible<EPPSLoanProgramFilters>,
    IEnumerable<EPPSLoanProgramFilter>,
    IEnumerable
  {
    private XmlList<EPPSLoanProgramFilter> items = new XmlList<EPPSLoanProgramFilter>();

    public EPPSLoanProgramFilters()
    {
    }

    internal EPPSLoanProgramFilters(EPPSLoanProgramFilters source)
    {
      foreach (EPPSLoanProgramFilter source1 in source)
        this.items.Add(new EPPSLoanProgramFilter(source1));
    }

    public EPPSLoanProgramFilters(XmlSerializationInfo info)
    {
      this.items = (XmlList<EPPSLoanProgramFilter>) info.GetValue(nameof (items), typeof (XmlList<EPPSLoanProgramFilter>));
    }

    public void Add(EPPSLoanProgramFilter item)
    {
      int index = 0;
      if (index >= this.items.Count)
        this.items.Add(item);
      else
        this.items.Insert(index, item);
    }

    public void Clear() => this.items.Clear();

    public int Count => this.items.Count;

    public EPPSLoanProgramFilter this[int index] => this.items[index];

    public IEnumerator<EPPSLoanProgramFilter> GetEnumerator()
    {
      return (IEnumerator<EPPSLoanProgramFilter>) this.items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("items", (object) this.items);
    }

    public static explicit operator EPPSLoanProgramFilters(BinaryObject o)
    {
      return (EPPSLoanProgramFilters) BinaryConvertibleObject.Parse(o, typeof (EPPSLoanProgramFilters));
    }
  }
}
