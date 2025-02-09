// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.DummyProvider
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.DataEngine.eFolder;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  internal class DummyProvider : IEnhancedConditionsProvider
  {
    private Exerciser Exerciser { get; set; } = new Exerciser();

    Task<ExportablePackage> IEnhancedConditionsProvider.GetEnhancedConditions(bool isSource)
    {
      return Task.FromResult<ExportablePackage>(this.Exerciser.CreatePackage());
    }

    Task<IList<ConditionTemplate>> IEnhancedConditionsProvider.GetStandardConditions()
    {
      throw new NotImplementedException();
    }

    Task<SyncResult> IEnhancedConditionsProvider.UpsertEnhancedConditionTemplates(
      IEnumerable<EnhancedConditionTemplate> templates,
      bool useInsert,
      CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
