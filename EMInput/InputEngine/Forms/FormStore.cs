// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.FormStore
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class FormStore
  {
    private FormStore()
    {
    }

    public static string GetFormHTMLPath(Sessions.Session session, InputFormInfo form)
    {
      return form.Type == InputFormType.Custom ? FormStore.getCustomFormHTML(session, form) : FormStore.getStandardFormHTML(form);
    }

    public static string GetFormHTMLPath(Sessions.Session session, string formId)
    {
      try
      {
        return FormStore.GetFormHTMLPath(session, new InputFormInfo(formId, formId, InputFormType.Standard));
      }
      catch
      {
        return FormStore.GetFormHTMLPath(session, new InputFormInfo(formId, formId, InputFormType.Custom));
      }
    }

    private static string getStandardFormHTML(InputFormInfo form)
    {
      string resourceFileFullPath = AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, form.FormID + ".EMFRM"), SystemSettings.LocalAppDir, true);
      if (File.Exists(resourceFileFullPath))
        return FormExtractor.ExtractForm(form.FormID, resourceFileFullPath);
      throw new ObjectNotFoundException("The standard form '" + form.FormID + "' could not be found", ObjectType.CustomForm, (object) form.FormID);
    }

    private static string getCustomFormHTML(Sessions.Session session, InputFormInfo form)
    {
      string emfrmPath = (string) null;
      using (FormCache formCache = new FormCache(session))
        emfrmPath = formCache.FetchCustomForm(form);
      return FormExtractor.ExtractForm(form.FormID, emfrmPath);
    }
  }
}
