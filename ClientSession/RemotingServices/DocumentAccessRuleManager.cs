// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DocumentAccessRuleManager
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class DocumentAccessRuleManager : ManagerBase
  {
    private IBpmManager docAccessRuleMgr;

    internal static DocumentAccessRuleManager Instance
    {
      get => Session.DefaultInstance.DocumentAccessRuleManager;
    }

    internal static void RefreshInstance()
    {
      if (Session.DefaultInstance == null)
        return;
      Session.DefaultInstance.ResetDocumentAccessRuleManager();
    }

    internal DocumentAccessRuleManager(Sessions.Session session)
      : base(session, ClientSessionCacheID.DocumentAccessRule)
    {
      this.docAccessRuleMgr = this.session.SessionObjects.BpmManager;
    }

    public DocumentDefaultAccessRuleInfo[] GetDocumentDefaultAccessRules() => this.getAccessRules();

    public DocumentDefaultAccessRuleInfo GetDocumentDefaultAccessRule(int roleAddedBy)
    {
      foreach (DocumentDefaultAccessRuleInfo accessRule in this.getAccessRules())
      {
        if (accessRule.RoleAddedBy == roleAddedBy)
          return accessRule;
      }
      return new DocumentDefaultAccessRuleInfo(roleAddedBy);
    }

    public void SaveDocumentDefaultAccessRules(DocumentDefaultAccessRuleInfo[] rules)
    {
      this.docAccessRuleMgr.SaveDocumentDefaultAccessRules(rules);
      this.ClearCache();
    }

    private DocumentDefaultAccessRuleInfo[] getAccessRules()
    {
      DocumentDefaultAccessRuleInfo[] subject = (DocumentDefaultAccessRuleInfo[]) this.GetSubjectFromCache("rules");
      if (subject == null)
      {
        subject = this.docAccessRuleMgr.GetDocumentDefaultAccessRules();
        this.SetSubjectCache("rules", (object) subject);
      }
      return subject;
    }
  }
}
