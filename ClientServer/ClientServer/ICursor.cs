// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ICursor
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ICursor : IDisposable
  {
    int GetItemCount();

    int GetItemCount(int sqlRead);

    object GetItem(int index, bool isExternalOrganization);

    object GetItem(int index);

    object[] GetItems(int startIndex, int count);

    object[] GetItems(int startIndex, int count, bool isExternalOrganization);

    object[] GetItems(int startIndex, int count, bool isExternalOrganization, int sqlRead);
  }
}
