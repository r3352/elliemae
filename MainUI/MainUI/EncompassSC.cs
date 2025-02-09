// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.EncompassSC
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.VersionInterface15;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class EncompassSC
  {
    public static void main(string[] args)
    {
      try
      {
        JedVersion.Encompass = "Encompass.exe";
        EllieMae.EMLite.ClickLoan.ClickLoan.Dummy();
        MainForm.main(args);
      }
      catch (Exception ex)
      {
        Exception innerException = ex.InnerException;
        AssemblyResolver.WriteToEventLogS("Unhandled exception: " + (innerException == null ? ex.Message : innerException.Message), EventLogEntryType.Error);
      }
    }
  }
}
