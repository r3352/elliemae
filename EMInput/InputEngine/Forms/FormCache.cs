// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.FormCache
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Cache;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class FormCache : IDisposable
  {
    private Sessions.Session session;
    private ShadowedCache cache;

    public FormCache(Sessions.Session session)
    {
      this.session = session;
      this.cache = new ShadowedCache(session.SessionObjects.SystemID, "Forms");
    }

    public string FetchCustomFormAssembly(string assemblyName)
    {
      string fileName = assemblyName + ".dll";
      string filePath = this.cache.GetFilePath(fileName);
      if (this.cache.GetFileVersion(fileName) == this.session.FormManager.GetCustomFormAssemblyFileVersion(assemblyName))
        return filePath;
      BinaryObject customFormAssembly = this.session.FormManager.GetCustomFormAssembly(assemblyName);
      this.cache.Put(fileName, customFormAssembly);
      return filePath;
    }

    public string FetchCustomForm(InputFormInfo form)
    {
      string fileName = FileSystem.EncodeFilename(form.Name, false) + ".emfrm";
      string filePath = this.cache.GetFilePath(fileName);
      DateTime modificationDate1 = this.cache.GetLastModificationDate(fileName);
      DateTime modificationDate2 = this.session.FormManager.GetCustomFormModificationDate(form.FormID);
      if (modificationDate1 == modificationDate2)
        return filePath;
      this.cache.Put(fileName, this.session.FormManager.GetCustomForm(form.FormID) ?? throw new ObjectNotFoundException("Custom Form '" + form.FormID + "' does not exist", ObjectType.CustomForm, (object) form.FormID), modificationDate2);
      return filePath;
    }

    public void Dispose()
    {
      if (this.cache == null)
        return;
      this.cache.Dispose();
      this.cache = (ShadowedCache) null;
    }
  }
}
