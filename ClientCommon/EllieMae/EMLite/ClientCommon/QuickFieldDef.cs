// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientCommon.QuickFieldDef
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.ClientCommon
{
  internal class QuickFieldDef
  {
    private string originalID = string.Empty;
    private FieldDefinition fd;

    public QuickFieldDef(string originalID, FieldDefinition fd)
    {
      this.originalID = originalID;
      this.fd = fd;
    }

    public string OriginalID => this.originalID;

    public FieldDefinition FieldDef => this.fd;
  }
}
