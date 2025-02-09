// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.OrgNodeTag
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class OrgNodeTag
  {
    private int oid;
    private string description;

    public int Oid => this.oid;

    public string Description => this.description;

    public OrgNodeTag(int oid, string description)
    {
      this.oid = oid;
      this.description = description;
    }
  }
}
