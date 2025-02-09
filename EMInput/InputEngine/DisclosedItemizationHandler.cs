// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DisclosedItemizationHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DisclosedItemizationHandler : IHtmlInput
  {
    private IDisclosureTracking2015Log log;
    private LoanReportFieldDefs fieldDef;

    public DisclosedItemizationHandler(IDisclosureTracking2015Log log, LoanReportFieldDefs fieldDef)
    {
      this.log = log;
      this.fieldDef = fieldDef;
    }

    public FieldFormat GetFormat(string id) => Session.LoanData.GetFormat(id);

    public FieldDefinition GetFieldDefinition(string id)
    {
      return Session.LoanData.GetFieldDefinition(id) ?? (FieldDefinition) null;
    }

    public string GetField(string id) => this.log.GetDisclosedField(id);

    public string GetSimpleField(string id) => this.GetField(id);

    public string GetOrgField(string id) => this.GetField(id);

    public void SetField(string id, string val)
    {
    }

    public bool IsLocked(string id) => this.log.IsFieldLocked(id);

    public void RemoveLock(string id)
    {
    }

    public void AddLock(string id)
    {
    }

    public void SetCurrentField(string id, string val)
    {
    }

    public bool IsDirty(string id) => false;

    public void ClearDirtyTable()
    {
    }

    public void CleanField(string id)
    {
    }

    public void SetField(string id, string val, bool isUserModified)
    {
      throw new NotImplementedException();
    }
  }
}
