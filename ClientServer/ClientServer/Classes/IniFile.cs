// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Classes.IniFile
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Classes
{
  internal class IniFile
  {
    private readonly string _filePath;

    public IniFile(string file) => this._filePath = file;

    public string Read(string section, string key)
    {
      StringBuilder RetVal = new StringBuilder((int) byte.MaxValue);
      IniFile.GetPrivateProfileString(section, key, "", RetVal, (int) byte.MaxValue, this._filePath);
      return RetVal.ToString();
    }

    public void Write(string section, string key, string value)
    {
      IniFile.WritePrivateProfileString(section, key, value, this._filePath);
    }

    public void DeleteKey(string section, string key) => this.Write(section, key, (string) null);

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern long WritePrivateProfileString(
      string Section,
      string Key,
      string Value,
      string FilePath);

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    private static extern int GetPrivateProfileString(
      string Section,
      string Key,
      string Default,
      StringBuilder RetVal,
      int Size,
      string FilePath);
  }
}
