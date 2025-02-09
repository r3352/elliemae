// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DbSize
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DbSize
  {
    public readonly string DatabaseName;
    public readonly int PhysicalSize;
    public readonly int LogicalSize;
    public readonly int DataSize;
    public readonly int IndexSize;

    public DbSize(string name, int psize, int lsize, int dsize, int isize)
    {
      this.DatabaseName = name;
      this.PhysicalSize = psize;
      this.LogicalSize = lsize;
      this.DataSize = dsize;
      this.IndexSize = isize;
    }
  }
}
