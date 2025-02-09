// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.IEnhancedConditionsProvider
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.DataEngine.eFolder;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public interface IEnhancedConditionsProvider
  {
    Task<IList<ConditionTemplate>> GetStandardConditions();

    Task<ExportablePackage> GetEnhancedConditions(bool isSource);

    Task<SyncResult> UpsertEnhancedConditionTemplates(
      IEnumerable<EnhancedConditionTemplate> templates,
      bool useInsert,
      CancellationToken cancellationToken);
  }
}
