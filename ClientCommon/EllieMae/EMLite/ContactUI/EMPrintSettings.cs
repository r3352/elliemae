// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.EMPrintSettings
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class EMPrintSettings
  {
    private string printerName = "";
    private int numCopy = 1;
    private bool bCollate;

    public EMPrintSettings(string printerName, int numCopy, bool bCollate)
    {
      this.printerName = printerName;
      this.numCopy = numCopy;
      this.bCollate = bCollate;
    }

    public string PrinterName => this.printerName;

    public int NumCopy => this.numCopy;

    public bool Collate => this.bCollate;
  }
}
