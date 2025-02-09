// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CustomLetterXRef
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class CustomLetterXRef
  {
    public string Guid;
    public FileSystemEntry XRef;

    public CustomLetterXRef(string guid, FileSystemEntry xref)
    {
      this.Guid = guid;
      this.XRef = xref;
    }
  }
}
