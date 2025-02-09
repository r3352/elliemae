// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Acl.LoanDuplicationAclManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Acl.Interfaces;
using EllieMae.EMLite.Common;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.RemotingServices.Acl
{
  public class LoanDuplicationAclManager : ManagerBase
  {
    private ILoanDuplicationAclManager loanDuplicationAclMgr;

    internal LoanDuplicationAclManager(Sessions.Session session)
      : base(session)
    {
      this.loanDuplicationAclMgr = (ILoanDuplicationAclManager) this.session.GetAclManager(AclCategory.LoanDuplicationTemplates);
    }

    public LoanDuplicationTemplateAclInfo[] GetAccessibleLoanDuplicationTemplates(Persona[] personas)
    {
      int[] personaIDs = (int[]) null;
      if (personas != null && personas.Length != 0)
      {
        personaIDs = new int[personas.Length];
        for (int index = 0; index < personas.Length; ++index)
          personaIDs[index] = personas[index].ID;
      }
      return this.loanDuplicationAclMgr.GetAccessibleLoanDuplicationTemplates(personaIDs);
    }

    public LoanDuplicationTemplateAclInfo[] GetAccessibleLoanDuplicationTemplates(int[] personaIDs)
    {
      return this.loanDuplicationAclMgr.GetAccessibleLoanDuplicationTemplates(personaIDs);
    }

    public LoanDuplicationTemplateAclInfo[] GetAccessibleLoanDuplicationTemplates(int personaID)
    {
      return this.loanDuplicationAclMgr.GetAccessibleLoanDuplicationTemplates(personaID);
    }

    public void SetPermissions(
      LoanDuplicationTemplateAclInfo[] loanDuplicationAclInfoList,
      int personaID)
    {
      this.loanDuplicationAclMgr.SetPermissions(loanDuplicationAclInfoList, personaID);
    }

    public void DuplicateACLLoanDuplication(int sourcePersonaID, int desPersonaID)
    {
      this.loanDuplicationAclMgr.DuplicateACLLoanDuplication(sourcePersonaID, desPersonaID);
    }

    public override void ClearCaches(string key) => this.clearCache(key);

    public List<string> GetAccessibleTemplatesForLoanDuplication()
    {
      List<string> forLoanDuplication = new List<string>();
      if (!this.session.UserInfo.IsSuperAdministrator())
      {
        foreach (LoanDuplicationTemplateAclInfo duplicationTemplate in this.GetAccessibleLoanDuplicationTemplates(this.session.UserInfo.GetPersonaIDs()))
          forLoanDuplication.Add(duplicationTemplate.TemplateName);
      }
      else
      {
        foreach (FileSystemEntry templateDirEntry in this.session.ConfigurationManager.GetFilteredTemplateDirEntries(TemplateSettingsType.LoanDuplicationTemplate, new FileSystemEntry("\\", "", FileSystemEntry.Types.File, (string) null)))
          forLoanDuplication.Add(templateDirEntry.Name);
      }
      return forLoanDuplication;
    }
  }
}
