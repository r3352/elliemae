// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServerInternalsInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ServerInternalsInfo
  {
    private string[] headers = new string[0];
    public readonly List<string[]> Contents = new List<string[]>();

    public string[] Headers => this.headers;

    public void SetHeadersIfNotAlreadySet(string[] headers)
    {
      if (this.headers != null && this.headers.Length != 0)
        return;
      this.headers = headers;
    }

    public void Add(string[] content)
    {
      if (content.Length != this.Headers.Length)
      {
        int num = (int) MessageBox.Show("content length != header length");
      }
      else
        this.Contents.Add(content);
    }
  }
}
