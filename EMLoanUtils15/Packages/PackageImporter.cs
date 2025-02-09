// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Packages.PackageImporter
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Packages
{
  public class PackageImporter
  {
    private IConnection conn;
    private ISession session;
    private ArrayList importedForms = new ArrayList();
    private PackageImportConflictResolver conflictResolver;
    private PackageImportConflictOption conflictOption;

    public PackageImporter(IConnection conn, PackageImportConflictResolver conflictResolver)
    {
      this.conn = conn;
      this.session = conn.Session;
      this.conflictResolver = conflictResolver;
    }

    public PackageImporter(IConnection conn, PackageImportConflictOption conflictOption)
    {
      this.conn = conn;
      this.session = conn.Session;
      this.conflictOption = conflictOption;
    }

    public string[] GetImportedFormIDs() => (string[]) this.importedForms.ToArray(typeof (string));

    public bool Import(ExportPackage pkg)
    {
      this.verifyPublishingEnabled();
      return this.importCustomDataObjects(pkg) && this.importPlugins(pkg) && this.importFields(pkg) && this.importAssemblies(pkg) && this.importForms(pkg);
    }

    private void verifyPublishingEnabled()
    {
      if ((this.conn == null || !this.conn.IsServerInProcess) && (EnableDisableSetting) ((IServerManager) this.session.GetObject("ServerManager")).GetServerSetting("FormBuilder.Publishing") == EnableDisableSetting.Disabled)
        throw new SecurityException("The target server's policies do not permit remote publishing of custom forms.");
    }

    private bool importFields(ExportPackage pkg)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.session.GetObject("ConfigurationManager");
      CustomFieldsInfo loanCustomFields = configurationManager.GetLoanCustomFields();
      CustomFieldsInfo fieldsInfo = new CustomFieldsInfo(false);
      foreach (CustomFieldInfo field1 in pkg.Fields)
      {
        CustomFieldInfo field2 = loanCustomFields.GetField(field1.FieldID);
        if (field2 != null && !field2.IsEmpty())
        {
          switch (this.resolveConflict(ExportPackageObjectType.LoanCustomField, field1.FieldID))
          {
            case PackageImportConflictOption.Overwrite:
              break;
            case PackageImportConflictOption.Abort:
              return false;
            default:
              continue;
          }
        }
        fieldsInfo.Add(field1);
      }
      if (fieldsInfo.GetNonEmptyCount() > 0)
        configurationManager.UpdateLoanCustomFields(fieldsInfo, false);
      return true;
    }

    private bool importAssemblies(ExportPackage pkg)
    {
      IFormManager formManager = (IFormManager) this.session.GetObject("FormManager");
      foreach (string assembly in pkg.Assemblies)
      {
        Version assemblyFileVersion = formManager.GetCustomFormAssemblyFileVersion(assembly);
        if (pkg.Assemblies.VerionOf(assembly) < assemblyFileVersion)
        {
          switch (this.resolveConflict(ExportPackageObjectType.CodebaseAssembly, assembly))
          {
            case PackageImportConflictOption.Overwrite:
              break;
            case PackageImportConflictOption.Abort:
              return false;
            default:
              continue;
          }
        }
        using (BinaryObject assemblyData = pkg.Assemblies.Extract(assembly))
          formManager.SaveCustomFormAssembly(assembly, assemblyData);
      }
      return true;
    }

    private bool importPlugins(ExportPackage pkg)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.session.GetObject("ConfigurationManager");
      string[] pluginAssemblyNames = configurationManager.GetPluginAssemblyNames();
      foreach (string plugin in pkg.Plugins)
      {
        string assemblyName = plugin;
        if (((IEnumerable<string>) pluginAssemblyNames).Any<string>((Func<string, bool>) (x => x.Equals(assemblyName, StringComparison.CurrentCultureIgnoreCase))))
        {
          switch (this.resolveConflict(ExportPackageObjectType.Plugin, assemblyName))
          {
            case PackageImportConflictOption.Overwrite:
              break;
            case PackageImportConflictOption.Abort:
              return false;
            default:
              continue;
          }
        }
        using (BinaryObject pluginAssembly = pkg.Plugins.Extract(assemblyName))
          configurationManager.InstallPlugin(assemblyName, pluginAssembly);
      }
      return true;
    }

    private bool importCustomDataObjects(ExportPackage pkg)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.session.GetObject("ConfigurationManager");
      string[] customDataObjectNames = configurationManager.GetCustomDataObjectNames();
      foreach (string customDataObject in pkg.CustomDataObjects)
      {
        string customDataObjectName = customDataObject;
        if (((IEnumerable<string>) customDataObjectNames).Any<string>((Func<string, bool>) (x => x.Equals(customDataObjectName, StringComparison.CurrentCultureIgnoreCase))))
        {
          switch (this.resolveConflict(ExportPackageObjectType.CustomData, customDataObjectName))
          {
            case PackageImportConflictOption.Overwrite:
              break;
            case PackageImportConflictOption.Abort:
              return false;
            default:
              continue;
          }
        }
        using (BinaryObject data = pkg.CustomDataObjects.Extract(customDataObjectName))
          configurationManager.SaveCustomDataObject(customDataObjectName, data);
      }
      return true;
    }

    private bool importForms(ExportPackage pkg)
    {
      IFormManager formManager = (IFormManager) this.session.GetObject("FormManager");
      foreach (InputFormInfo form1 in pkg.Forms)
      {
        if (formManager.GetCustomFormModificationDate(form1.FormID) != DateTime.MinValue)
        {
          switch (this.resolveConflict(ExportPackageObjectType.InputForm, form1.Name))
          {
            case PackageImportConflictOption.Overwrite:
              break;
            case PackageImportConflictOption.Abort:
              return false;
            default:
              continue;
          }
        }
        using (BinaryObject form2 = pkg.Forms.Extract(form1))
          formManager.SaveCustomForm(form1, form2);
        if (!this.importedForms.Contains((object) form1.FormID))
          this.importedForms.Add((object) form1.FormID);
      }
      return true;
    }

    private PackageImportConflictOption resolveConflict(
      ExportPackageObjectType objectType,
      string objectName)
    {
      return this.conflictResolver != null ? this.conflictResolver(objectType, objectName) : this.conflictOption;
    }
  }
}
