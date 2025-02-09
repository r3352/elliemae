// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Logging.TextTraceListener
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Diagnostics;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.Common.Logging
{
  public class TextTraceListener : TextWriterTraceListener
  {
    private StreamWriter traceWriter;

    public TextTraceListener(string name, string filePath, string fileName)
    {
      this.Name = name;
      if (!Directory.Exists(filePath))
        Directory.CreateDirectory(filePath);
      this.traceWriter = File.AppendText(Path.Combine(filePath, fileName));
      this.traceWriter.AutoFlush = true;
    }

    public override void Write(string message)
    {
      lock (this)
      {
        try
        {
          if (this.traceWriter == null)
            return;
          this.traceWriter.Write(message);
        }
        catch
        {
        }
      }
    }

    public override void WriteLine(string message)
    {
      lock (this)
      {
        try
        {
          if (this.traceWriter == null)
            return;
          this.traceWriter.WriteLine(message);
        }
        catch
        {
        }
      }
    }

    public override void Close()
    {
      lock (this)
      {
        try
        {
          if (this.traceWriter == null)
            return;
          this.traceWriter.Close();
        }
        catch
        {
        }
        finally
        {
          this.traceWriter = (StreamWriter) null;
        }
      }
    }
  }
}
