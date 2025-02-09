// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs.FilterDef
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Server.ServerObjects.SearchEngine.Enums;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.SearchEngine.Defs
{
  [Serializable]
  public abstract class FilterDef
  {
    public FilterDef(string column, string tableAlias, FilterDataType dataType)
    {
      this.Column = column;
      this.TableAlias = tableAlias;
      this.DataType = dataType;
    }

    public virtual string Column { get; set; }

    public virtual string TableAlias { get; set; }

    public virtual FilterDataType DataType { get; set; }

    public virtual Dictionary<string, string> DataMappings { get; set; }

    public abstract SearchFilter GetFilter(string fragmentName, string filterQuery, ref int i);

    public abstract bool ValidateFilter(string fragmentName, string fragmentValue);
  }
}
