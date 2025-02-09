// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.SwitchPreferences
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public enum SwitchPreferences
  {
    [Description("--remote-debugging-port=9222")] RemoteDebuggingPort,
    [Description("--force-renderer-accessibility")] ForceRendererAccessibility,
    [Description("--aggressive-cache-discard")] AggressiveCacheDiscard,
    [Description("--crash-dump-dir=")] CrashDumpDir,
    [Description("--cloud-print-file")] CloudPrintFile,
    [Description("--cloud-print-file-type")] CloudPrintFileType,
    [Description("--cloud-print-url")] CloudPrintUrl,
    [Description("--debug-print")] DebugPrint,
    [Description("--debug-devtools")] DebugDevTool,
    [Description("--disable-print-preview")] DisablePrintPreview,
    [Description("--print-to-pdf")] PrintToPdf,
    [Description("--print-to-pdf-no-header")] PrintToPdfNoHeader,
  }
}
