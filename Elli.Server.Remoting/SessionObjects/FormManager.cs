// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.FormManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using System;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class FormManager : SessionBoundObject, IFormManager
  {
    private const string className = "FormManager";

    public FormManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (FormManager).ToLower());
      return this;
    }

    public virtual DateTime GetCustomFormModificationDate(string formId)
    {
      this.onApiCalled(nameof (FormManager), nameof (GetCustomFormModificationDate), new object[1]
      {
        (object) formId
      });
      try
      {
        return InputForms.GetCustomFormModificationDate(formId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return DateTime.MinValue;
      }
    }

    public virtual string[] GetCustomFormAssemblyNames()
    {
      this.onApiCalled(nameof (FormManager), nameof (GetCustomFormAssemblyNames), Array.Empty<object>());
      try
      {
        return InputForms.GetCustomFormAssemblyNames();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return (string[]) null;
      }
    }

    public virtual BinaryObject GetCustomFormAssembly(string assemblyName)
    {
      this.onApiCalled(nameof (FormManager), "GetFormAssembly", new object[1]
      {
        (object) assemblyName
      });
      try
      {
        return BinaryObject.Marshal(InputForms.GetCustomFormAssembly(assemblyName));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return (BinaryObject) null;
      }
    }

    public virtual InputFormInfo GetFormInfo(string formId)
    {
      this.onApiCalled(nameof (FormManager), nameof (GetFormInfo), new object[1]
      {
        (object) formId
      });
      try
      {
        return InputForms.GetFormInfo(formId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return (InputFormInfo) null;
      }
    }

    public virtual InputFormInfo[] GetAllFormInfos()
    {
      this.onApiCalled(nameof (FormManager), nameof (GetAllFormInfos), Array.Empty<object>());
      try
      {
        return InputForms.GetAllFormInfos();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return (InputFormInfo[]) null;
      }
    }

    public virtual InputFormInfo[] GetFormInfos(InputFormType formType)
    {
      this.onApiCalled(nameof (FormManager), nameof (GetFormInfos), new object[1]
      {
        (object) formType
      });
      try
      {
        return InputForms.GetFormInfos(formType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return (InputFormInfo[]) null;
      }
    }

    public virtual InputFormInfo[] GetFormInfos(InputFormCategory category)
    {
      return this.GetFormInfos(InputFormType.All, category);
    }

    public virtual InputFormInfo[] GetFormInfos(InputFormType formType, InputFormCategory category)
    {
      this.onApiCalled(nameof (FormManager), nameof (GetFormInfos), new object[2]
      {
        (object) formType,
        (object) category
      });
      try
      {
        return InputForms.GetFormInfos(formType, category);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return (InputFormInfo[]) null;
      }
    }

    public virtual InputFormInfo[] GetFormInfos(string[] formIds)
    {
      this.onApiCalled(nameof (FormManager), nameof (GetFormInfos), (object[]) formIds);
      try
      {
        return InputForms.GetFormInfos(formIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return (InputFormInfo[]) null;
      }
    }

    public virtual InputFormInfo GetFormInfoByName(string formName)
    {
      this.onApiCalled(nameof (FormManager), nameof (GetFormInfoByName), new object[1]
      {
        (object) formName
      });
      try
      {
        return InputForms.GetFormInfoByName(formName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return (InputFormInfo) null;
      }
    }

    public virtual void SaveCustomForm(InputFormInfo formInfo, BinaryObject form)
    {
      this.onApiCalled(nameof (FormManager), nameof (SaveCustomForm), new object[2]
      {
        (object) formInfo,
        (object) form
      });
      form.Download();
      try
      {
        InputForms.SaveCustomForm(formInfo, form);
        InputFormsAclDbAccessor.SynchrnizeAdminAccessRight();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
      }
    }

    public virtual void SaveCustomFormAssembly(string assemblyName, BinaryObject assemblyData)
    {
      this.onApiCalled(nameof (FormManager), nameof (SaveCustomFormAssembly), new object[2]
      {
        (object) assemblyName,
        (object) assemblyData
      });
      assemblyData.Download();
      try
      {
        InputForms.SaveCustomFormAssembly(assemblyName, assemblyData);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
      }
    }

    public virtual Version GetCustomFormAssemblyFileVersion(string assemblyName)
    {
      this.onApiCalled(nameof (FormManager), "GetCustomFormAssemblyVersion", new object[1]
      {
        (object) assemblyName
      });
      try
      {
        return InputForms.GetCustomFormAssemblyFileVersion(assemblyName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return (Version) null;
      }
    }

    public virtual BinaryObject GetCustomForm(string formId)
    {
      this.onApiCalled(nameof (FormManager), nameof (GetCustomForm), new object[1]
      {
        (object) formId
      });
      try
      {
        return BinaryObject.Marshal(InputForms.GetCustomForm(formId));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
        return (BinaryObject) null;
      }
    }

    public virtual void RenameForm(string formId, string newName)
    {
      this.onApiCalled(nameof (FormManager), nameof (RenameForm), new object[2]
      {
        (object) formId,
        (object) newName
      });
      try
      {
        InputForms.RenameForm(formId, newName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
      }
    }

    public virtual void DeleteCustomForm(string formId)
    {
      this.onApiCalled(nameof (FormManager), nameof (DeleteCustomForm), new object[1]
      {
        (object) formId
      });
      try
      {
        InputForms.DeleteCustomForm(formId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
      }
    }

    public virtual void SetFormOrder(InputFormInfo[] forms)
    {
      this.onApiCalled(nameof (FormManager), nameof (SetFormOrder), (object[]) forms);
      try
      {
        InputForms.SetFormOrder(forms);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (FormManager), ex);
      }
    }
  }
}
