// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.HeadRelationTableDef
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public class HeadRelationTableDef(XmlElement tableDefNode) : HeadTableDef(tableDefNode)
  {
    private string virtualTableName = "";

    protected override void parseTableNode(XmlElement tableDefNode)
    {
      base.parseTableNode(tableDefNode);
      this.virtualTableName = tableDefNode.GetAttribute("virtualHeadTable");
    }

    public List<string> ForeignKeyTables
    {
      get
      {
        List<string> foreignKeyTables = new List<string>();
        foreach (ForeignKey foreignKey in this.foreignKeys)
        {
          if (!foreignKeyTables.Contains(foreignKey.PrimaryKeyTableName))
            foreignKeyTables.Add(foreignKey.PrimaryKeyTableName);
        }
        return foreignKeyTables;
      }
    }

    public string VirtualHeadTableName => this.virtualTableName;
  }
}
