// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Acl.LoanDuplicationAclManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Server.Remoting.SessionObjects;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Acl.Interfaces;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;

#nullable disable
namespace Elli.Server.Remoting.Acl
{
  public class LoanDuplicationAclManager : SessionBoundObject, ILoanDuplicationAclManager
  {
    private const string className = "LoanDuplicationAclManager";
    private const string objKeyPrefix = "aclmanager#";

    public LoanDuplicationAclManager Initialize(ISession session)
    {
      this.InitializeInternal(session, "aclmanager#" + (object) 12);
      return this;
    }

    public virtual LoanDuplicationTemplateAclInfo[] GetAccessibleLoanDuplicationTemplates(
      Persona[] personas)
    {
      this.onApiCalled(nameof (LoanDuplicationAclManager), nameof (GetAccessibleLoanDuplicationTemplates), (object[]) personas);
      try
      {
        int[] personaIDs = (int[]) null;
        if (personas != null && personas.Length != 0)
        {
          personaIDs = new int[personas.Length];
          for (int index = 0; index < personas.Length; ++index)
            personaIDs[index] = personas[index].ID;
        }
        return this.GetAccessibleLoanDuplicationTemplates(personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanDuplicationAclManager), ex, this.Session.SessionInfo);
        return (LoanDuplicationTemplateAclInfo[]) null;
      }
    }

    public virtual LoanDuplicationTemplateAclInfo[] GetAccessibleLoanDuplicationTemplates(
      int[] personaIDs)
    {
      this.onApiCalled(nameof (LoanDuplicationAclManager), nameof (GetAccessibleLoanDuplicationTemplates), new object[1]
      {
        (object) personaIDs
      });
      try
      {
        return LoanDuplicationAclDbAccessor.GetAccessibleLoanDuplicationTemplates(personaIDs);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanDuplicationAclManager), ex, this.Session.SessionInfo);
        return (LoanDuplicationTemplateAclInfo[]) null;
      }
    }

    public virtual LoanDuplicationTemplateAclInfo[] GetAccessibleLoanDuplicationTemplates(
      int personaID)
    {
      this.onApiCalled(nameof (LoanDuplicationAclManager), nameof (GetAccessibleLoanDuplicationTemplates), new object[1]
      {
        (object) personaID
      });
      try
      {
        return LoanDuplicationAclDbAccessor.GetAccessibleLoanDuplicationTemplates(new int[1]
        {
          personaID
        });
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanDuplicationAclManager), ex, this.Session.SessionInfo);
        return (LoanDuplicationTemplateAclInfo[]) null;
      }
    }

    public virtual void SetPermissions(
      LoanDuplicationTemplateAclInfo[] loanDuplicationAclInfoList,
      int personaID)
    {
      this.onApiCalled(nameof (LoanDuplicationAclManager), nameof (SetPermissions), new object[2]
      {
        (object) loanDuplicationAclInfoList,
        (object) personaID
      });
      try
      {
        this.UpdateLoanDuplicationSettings(loanDuplicationAclInfoList, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanDuplicationAclManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DuplicateACLLoanDuplication(int sourcePersonaID, int desPersonaID)
    {
      this.onApiCalled(nameof (LoanDuplicationAclManager), nameof (DuplicateACLLoanDuplication), new object[2]
      {
        (object) sourcePersonaID,
        (object) desPersonaID
      });
      try
      {
        IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
        if (sourcePersonaID == 1)
          LoanDuplicationAclDbAccessor.DuplicateACLLoanDuplication(configurationManager.GetTemplateDirEntries(TemplateSettingsType.LoanDuplicationTemplate, FileSystemEntry.PublicRoot), desPersonaID);
        else
          LoanDuplicationAclDbAccessor.DuplicateACLLoanDuplication(sourcePersonaID, desPersonaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanDuplicationAclManager), ex, this.Session.SessionInfo);
      }
    }

    private void UpdateLoanDuplicationSettings(
      LoanDuplicationTemplateAclInfo[] loanDuplicationAclInfoList,
      int personaID)
    {
      this.onApiCalled(nameof (LoanDuplicationAclManager), nameof (UpdateLoanDuplicationSettings), new object[2]
      {
        (object) loanDuplicationAclInfoList,
        (object) personaID
      });
      try
      {
        LoanDuplicationAclDbAccessor.UpdateLoanDuplicationSettings(loanDuplicationAclInfoList, personaID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (LoanDuplicationAclManager), ex, this.Session.SessionInfo);
      }
    }
  }
}
