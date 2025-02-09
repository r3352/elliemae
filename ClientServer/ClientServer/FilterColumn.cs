// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FilterColumn
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class FilterColumn
  {
    public FilterColumn()
    {
    }

    public FilterColumn(string name, List<string> values)
    {
      this.Name = name;
      this.Values = values;
    }

    public FilterColumn(string name, string value)
    {
      this.Name = name;
      this.Values = new List<string>();
      this.Values.Add(value);
    }

    public string Name { get; set; }

    public List<string> Values { get; set; }
  }
}
