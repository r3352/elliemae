// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DDMFieldExecContext
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using Microsoft.Win32;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class DDMFieldExecContext
  {
    private Stopwatch sw;

    public string RuleName { get; set; }

    public string ScenarioName { get; set; }

    public DDMFeildExecRuleType RuleType { get; set; }

    public DDMFieldExecCmdType CommandType { get; set; }

    public string CommandDetail { get; set; }

    public bool IsLogOn { get; set; }

    public static bool IsDDMDiagnosticOn()
    {
      try
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass", false))
          return registryKey != null && string.Concat(registryKey.GetValue("DDMDiagnostics")).ToLower() == "1";
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static void LogHeader(string loanGuid, string userId)
    {
      try
      {
        if (!DDMFieldExecContext.IsDDMDiagnosticOn())
          return;
        Tracing.Log(true, "DDM_DIAGNOSTICS_HEADER", "FieldProvider", string.Format("Loan {0}, {1}, {2}", (object) loanGuid, (object) userId, (object) DateTime.Now.ToString("G")));
      }
      catch (Exception ex)
      {
      }
    }

    public static void LogHeader(string message)
    {
      try
      {
        if (!DDMFieldExecContext.IsDDMDiagnosticOn())
          return;
        Tracing.Log(true, "DDM_DIAGNOSTICS_HEADER", "FieldProvider", message);
      }
      catch (Exception ex)
      {
      }
    }

    public static DDMFieldExecContext StartTimer()
    {
      try
      {
        DDMFieldExecContext fieldExecContext = new DDMFieldExecContext();
        if (DDMFieldExecContext.IsDDMDiagnosticOn())
          fieldExecContext.sw = Stopwatch.StartNew();
        return fieldExecContext;
      }
      catch (Exception ex)
      {
        return (DDMFieldExecContext) null;
      }
    }

    public void LogPerf(string subject)
    {
      try
      {
        long num = 0;
        if (!DDMFieldExecContext.IsDDMDiagnosticOn())
          return;
        if (this.sw.IsRunning)
        {
          this.sw.Stop();
          num = this.sw.ElapsedMilliseconds;
        }
        Tracing.Log(true, "DDM_DIAGNOSTICS_PERF", "FieldProvider", string.Format("{0} - Elapsed Time {1} ms", (object) subject, (object) num));
      }
      catch (Exception ex)
      {
      }
    }

    public void LogDetail(SetFieldResult setField)
    {
      this.LogDetail(setField.FieldId, setField.OriginalValue, setField.NewValue, setField.LockIconActivated);
    }

    public void LogDetail(string fieldId, string origValue, string newValue, bool LockIconSet = false)
    {
      try
      {
        if (!DDMFieldExecContext.IsDDMDiagnosticOn() || !this.IsLogOn)
          return;
        Tracing.Log(true, "DDM_DIAGNOSTICS_DETAIL", "FieldProvider", string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}{7}", (object) fieldId, (object) this.RuleName, (object) this.ScenarioName, (object) origValue, (object) newValue, this.RuleType == DDMFeildExecRuleType.FeeRule ? (object) "Fee Rule" : (object) "Field Rule", (object) this.CommandType.ToString(), LockIconSet ? (object) ", Field Lock = ON" : (object) string.Empty));
        if (this.CommandType == DDMFieldExecCmdType.Regular)
          return;
        Tracing.Log(true, "DDM_DIAGNOSTICS_DETAIL", "FieldProvider", (this.CommandType == DDMFieldExecCmdType.Calculation ? "Calculation Detail: " : "Table Detail: ") + this.CommandDetail);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
