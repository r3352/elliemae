// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.EfolderDocTrackViewAccessorXML
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  internal class EfolderDocTrackViewAccessorXML : IEfolderDocTrackViewAccessor
  {
    public List<ViewSummary> GetViewsSummary(string userId)
    {
      List<ViewSummary> viewsSummary = (List<ViewSummary>) null;
      FileSystemEntry[] directoryEntries = TemplateSettingsStore.GetDirectoryEntries(TemplateSettingsType.DocumentTrackingView, FileSystemEntry.PrivateRoot(userId));
      if (directoryEntries != null && directoryEntries.Length != 0)
      {
        viewsSummary = new List<ViewSummary>();
        foreach (FileSystemEntry fileSystemEntry in directoryEntries)
          viewsSummary.Add(new ViewSummary()
          {
            Id = TemplateSettings.EncodeForViewId(fileSystemEntry.Name),
            Name = fileSystemEntry.Name
          });
      }
      return viewsSummary;
    }

    public DocumentTrackingView GetView(string userId, string viewId)
    {
      string entryName = TemplateSettings.DecodeViewId(viewId);
      if (entryName == null)
        return (DocumentTrackingView) null;
      DocumentTrackingView view = (DocumentTrackingView) null;
      using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(TemplateSettingsType.DocumentTrackingView, new FileSystemEntry("\\", entryName, FileSystemEntry.Types.File, userId)))
      {
        if (latestVersion.Exists)
        {
          string b = User.GetPrivateProfileString(userId, "DocumentTrackingView", "DefaultView");
          if (!string.IsNullOrWhiteSpace(b))
            b = b.Substring(b.LastIndexOf("\\") + 1);
          view = (DocumentTrackingView) latestVersion.Data;
          view.Id = TemplateSettings.EncodeForViewId(view.Name);
          view.IsDefault = string.Equals(view.Name, b, StringComparison.CurrentCultureIgnoreCase);
        }
      }
      return view;
    }

    public DocumentTrackingView CreateView(string userId, DocumentTrackingView view)
    {
      DocumentTrackingView view1 = (DocumentTrackingView) null;
      FileSystemEntry entry = new FileSystemEntry("\\", view.Name, FileSystemEntry.Types.File, userId);
      using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(TemplateSettingsType.DocumentTrackingView, entry))
      {
        if (templateSettings.Exists)
          throw new DuplicateObjectException("A view with the same name already exists for the user: '" + userId + "'.", ObjectType.Template, (object) view.Name);
        templateSettings.CheckIn((BinaryObject) (BinaryConvertibleObject) view);
        if (view.IsDefault)
          User.WritePrivateProfileString(userId, "DocumentTrackingView", "DefaultView", entry.ToString());
        view1 = (DocumentTrackingView) templateSettings.Data;
        view1.Id = TemplateSettings.EncodeForViewId(view1.Name);
        view1.IsDefault = view.IsDefault;
      }
      return view1;
    }

    public DocumentTrackingView UpdateView(string userId, DocumentTrackingView view)
    {
      DocumentTrackingView documentTrackingView = (DocumentTrackingView) null;
      string str = TemplateSettings.DecodeViewId(view.Id);
      FileSystemEntry entry1 = !string.IsNullOrWhiteSpace(str) ? new FileSystemEntry("\\", str, FileSystemEntry.Types.File, userId) : throw new ObjectNotFoundException("A view with id '" + view.Id + "' does not exist for the user: '" + userId + "'.", ObjectType.Template, (object) view.Name);
      if (!TemplateSettingsStore.Exists(TemplateSettingsType.DocumentTrackingView, entry1))
        throw new ObjectNotFoundException("A view with id '" + view.Id + "' does not exist for the user: '" + userId + "'.", ObjectType.Template, (object) view.Name);
      if (!string.Equals(str, view.Name, StringComparison.CurrentCultureIgnoreCase))
      {
        FileSystemEntry entry2 = new FileSystemEntry("\\", view.Name, FileSystemEntry.Types.File, userId);
        if (TemplateSettingsStore.Exists(TemplateSettingsType.DocumentTrackingView, entry2))
          throw new DuplicateObjectException("A view with the same name already exists for the user: '" + userId + "'.", ObjectType.Template, (object) view.Name);
        TemplateSettingsStore.Delete(TemplateSettingsType.DocumentTrackingView, entry1);
        entry1 = entry2;
      }
      using (TemplateSettings templateSettings = TemplateSettingsStore.CheckOut(TemplateSettingsType.DocumentTrackingView, entry1))
      {
        templateSettings.CheckIn((BinaryObject) (BinaryConvertibleObject) view);
        if (view.IsDefault)
          User.WritePrivateProfileString(userId, "DocumentTrackingView", "DefaultView", entry1.ToString());
        documentTrackingView = (DocumentTrackingView) templateSettings.Data;
        documentTrackingView.Id = TemplateSettings.EncodeForViewId(documentTrackingView.Name);
        documentTrackingView.IsDefault = view.IsDefault;
      }
      return documentTrackingView;
    }

    public bool DeleteView(string userId, string viewId)
    {
      string str = TemplateSettings.DecodeViewId(viewId);
      if (string.IsNullOrWhiteSpace(str))
        throw new ObjectNotFoundException("A view with id '" + viewId + "' does not exist for the user: '" + userId + "'.", ObjectType.Template, (object) viewId);
      TemplateSettingsStore.Delete(TemplateSettingsType.DocumentTrackingView, new FileSystemEntry("\\" + str, FileSystemEntry.Types.File, userId));
      return true;
    }
  }
}
