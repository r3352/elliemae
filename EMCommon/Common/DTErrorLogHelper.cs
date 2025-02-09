// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DTErrorLogHelper
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class DTErrorLogHelper
  {
    private StreamWriter writer;
    private StreamReader reader;

    public string LogPath { get; set; }

    public DTErrorLogHelper() => this.DTErrorTLogDirectory();

    public DTErrorLogHelper(StreamWriter logWriter)
    {
      this.DTErrorTLogDirectory();
      this.writer = logWriter;
    }

    private void connect()
    {
      Directory.CreateDirectory(Path.GetDirectoryName(this.LogPath));
      this.writer = new StreamWriter(this.LogPath, true);
    }

    public void Write(string text)
    {
      lock (this)
      {
        Exception exception = (Exception) null;
        try
        {
          if (this.writer == null)
            return;
          this.writer.Write(text);
        }
        catch (IOException ex)
        {
          exception = (Exception) ex;
        }
      }
    }

    public void WriteLine(
      string userId,
      DateTime dateTime,
      string source,
      string clientId,
      string log)
    {
      this.connect();
      this.Write("{" + dateTime.ToString() + "} {" + userId + "} {" + clientId + "} {" + source + "} \n {" + log + "}" + Environment.NewLine + Environment.NewLine);
    }

    public string ReadLog()
    {
      string str = "";
      if (File.Exists(this.LogPath))
      {
        this.reader = new StreamReader(this.LogPath);
        str = this.reader.ReadToEnd();
        this.reader.Close();
      }
      return str;
    }

    public void DTErrorTLogDirectory()
    {
      string str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Encompass\\DTErrorLog");
      if (!Directory.Exists(str))
        Directory.CreateDirectory(str);
      this.LogPath = Path.Combine(str, "DTErrorLog.log");
    }

    public void Close()
    {
      lock (this)
      {
        try
        {
          if (this.writer == null)
            return;
          this.writer.Close();
        }
        catch
        {
        }
        finally
        {
          this.writer = (StreamWriter) null;
        }
      }
    }
  }
}
