// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IFormManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IFormManager
  {
    InputFormInfo GetFormInfo(string formId);

    InputFormInfo[] GetAllFormInfos();

    InputFormInfo[] GetFormInfos(InputFormType formType);

    InputFormInfo[] GetFormInfos(InputFormCategory category);

    InputFormInfo[] GetFormInfos(InputFormType formType, InputFormCategory category);

    InputFormInfo[] GetFormInfos(string[] formIds);

    InputFormInfo GetFormInfoByName(string name);

    void RenameForm(string formId, string newName);

    void SetFormOrder(InputFormInfo[] forms);

    BinaryObject GetCustomForm(string formId);

    void SaveCustomForm(InputFormInfo formInfo, BinaryObject form);

    void DeleteCustomForm(string formId);

    DateTime GetCustomFormModificationDate(string formId);

    Version GetCustomFormAssemblyFileVersion(string assemblyName);

    string[] GetCustomFormAssemblyNames();

    BinaryObject GetCustomFormAssembly(string assemblyName);

    void SaveCustomFormAssembly(string assemblyName, BinaryObject assemblyData);
  }
}
