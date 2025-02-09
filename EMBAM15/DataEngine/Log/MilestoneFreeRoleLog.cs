// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.MilestoneFreeRoleLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class MilestoneFreeRoleLog : LoanAssociateLog
  {
    public static string XmlType = "MilestoneFreeRole";

    public MilestoneFreeRoleLog()
    {
    }

    public MilestoneFreeRoleLog(LogList log, XmlElement e)
      : base(log, e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      if (this.RoleID == RoleInfo.FileStarter.ID)
        this.SetRoleInformation(-1, "");
      this.MarkAsClean();
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      new AttributeWriter(e).Write("Type", (object) MilestoneFreeRoleLog.XmlType);
    }
  }
}
