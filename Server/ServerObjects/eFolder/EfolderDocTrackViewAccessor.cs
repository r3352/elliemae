// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.EfolderDocTrackViewAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Domain.Exceptions;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class EfolderDocTrackViewAccessor
  {
    private IEfolderDocTrackViewAccessor documentTrackingViewAccessor;
    public static string eFolderDocTrackViewTblName = "EfolderDocTrackViews";
    public static string migrationViewFlagName = "DocumentTrackingView";
    private readonly bool _isTemplateMigrateToDb;
    private readonly string _stdViewId;

    public EfolderDocTrackViewAccessor()
    {
      try
      {
        this._isTemplateMigrateToDb = Company.GetCompanySetting("MIGRATION", EfolderDocTrackViewAccessor.migrationViewFlagName).ToLower() == "true";
        if (this._isTemplateMigrateToDb)
        {
          this.documentTrackingViewAccessor = (IEfolderDocTrackViewAccessor) new EfolderDocTrackViewAccessorDB();
          this._stdViewId = "0";
        }
        else
        {
          this.documentTrackingViewAccessor = (IEfolderDocTrackViewAccessor) new EfolderDocTrackViewAccessorXML();
          this._stdViewId = TemplateSettings.EncodeForViewId(EfolderDocTrackViewAccessor.StandardDocumentTrackingView.Name);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(typeof (EfolderDocTrackViewAccessor).Name, ex);
      }
    }

    public List<ViewSummary> GetViewsSummary(string userId)
    {
      List<ViewSummary> views = new List<ViewSummary>();
      try
      {
        List<ViewSummary> viewsSummary = this.documentTrackingViewAccessor.GetViewsSummary(userId);
        views.Add(new ViewSummary()
        {
          Id = this._stdViewId,
          Name = EfolderDocTrackViewAccessor.StandardDocumentTrackingView.Name
        });
        if (viewsSummary != null)
          views.AddRange(viewsSummary.Select<ViewSummary, ViewSummary>((Func<ViewSummary, ViewSummary>) (item => item)));
        this.SetDefaultView(userId, views);
        return views;
      }
      catch (Exception ex)
      {
        Err.Reraise(typeof (EfolderDocTrackViewAccessor).Name, ex);
      }
      return views;
    }

    private void SetDefaultView(string userId, List<ViewSummary> views)
    {
      string defaultViewName = User.GetPrivateProfileString(userId, "DocumentTrackingView", "DefaultView");
      if (!string.IsNullOrWhiteSpace(defaultViewName))
        defaultViewName = defaultViewName.Substring(defaultViewName.LastIndexOf("\\") + 1);
      if (string.IsNullOrWhiteSpace(defaultViewName) || string.Equals(defaultViewName, EfolderDocTrackViewAccessor.StandardDocumentTrackingView.Name, StringComparison.CurrentCultureIgnoreCase) || views.FirstOrDefault<ViewSummary>((Func<ViewSummary, bool>) (x => x.Name.ToLower() == defaultViewName.ToLower())) == null)
        defaultViewName = EfolderDocTrackViewAccessor.StandardDocumentTrackingView.Name;
      views.FirstOrDefault<ViewSummary>((Func<ViewSummary, bool>) (x => x.Name.ToLower() == defaultViewName.ToLower())).IsDefault = true;
    }

    public DocumentTrackingView GetView(string userId, string viewId)
    {
      DocumentTrackingView view = (DocumentTrackingView) null;
      try
      {
        if (string.Equals(viewId, this._stdViewId, StringComparison.CurrentCultureIgnoreCase))
        {
          view = EfolderDocTrackViewAccessor.StandardDocumentTrackingView;
          view.Id = this._stdViewId;
          string a = User.GetPrivateProfileString(userId, "DocumentTrackingView", "DefaultView");
          if (!string.IsNullOrWhiteSpace(a))
            a = a.Substring(a.LastIndexOf("\\") + 1);
          if (string.Equals(a, view.Name, StringComparison.InvariantCultureIgnoreCase))
            view.IsDefault = true;
          return view;
        }
        view = this.documentTrackingViewAccessor.GetView(userId, viewId);
      }
      catch (Exception ex)
      {
        Err.Reraise(typeof (EfolderDocTrackViewAccessor).Name, ex);
      }
      return view;
    }

    public DocumentTrackingView CreateView(string userId, DocumentTrackingView view)
    {
      DocumentTrackingView view1 = (DocumentTrackingView) null;
      try
      {
        if (view == null)
          return view1;
        if (string.Equals(view.Name, EfolderDocTrackViewAccessor.StandardDocumentTrackingView.Name, StringComparison.CurrentCultureIgnoreCase))
          throw new DuplicateObjectException("A view with the same name already exists for the user '" + userId + "'.", ObjectType.Template, (object) view.Name);
        if (!TemplateSettings.IsValidName(view.Name))
          throw new ServerArgumentException("The specified view name is invalid.  The name must be non-empty, cannot contain the backslash (\\), and cannot use special ASCII characters.");
        view1 = this.documentTrackingViewAccessor.CreateView(userId, view);
        string templateName = view1.Id;
        if (!this._isTemplateMigrateToDb)
          templateName = TemplateSettings.DecodeViewId(view1.Id, false);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(userId, userId, ActionType.TemplateCreated, DateTime.Now, templateName, view.Name));
      }
      catch (Exception ex)
      {
        Err.Reraise(typeof (EfolderDocTrackViewAccessor).Name, ex);
      }
      return view1;
    }

    public DocumentTrackingView UpdateView(string userId, DocumentTrackingView view)
    {
      DocumentTrackingView documentTrackingView = (DocumentTrackingView) null;
      try
      {
        if (view == null)
          return documentTrackingView;
        string a = this._isTemplateMigrateToDb ? view.Id : TemplateSettings.DecodeViewId(view.Id);
        if (a == this._stdViewId || string.Equals(a, EfolderDocTrackViewAccessor.StandardDocumentTrackingView.Name, StringComparison.CurrentCultureIgnoreCase))
          throw new ForbiddenException("Standard view is not allowed to be updated.");
        if (string.Equals(view.Name, EfolderDocTrackViewAccessor.StandardDocumentTrackingView.Name, StringComparison.CurrentCultureIgnoreCase))
          throw new DuplicateObjectException("A view with the same name already exists for the user '" + userId + "'.", ObjectType.Template, (object) view.Name);
        if (!TemplateSettings.IsValidName(view.Name))
          throw new ServerArgumentException("The specified view name is invalid.  The name must be non-empty, cannot contain the backslash (\\), and cannot use special ASCII characters.");
        documentTrackingView = this.documentTrackingViewAccessor.UpdateView(userId, view);
        string templateName = view.Id;
        if (!this._isTemplateMigrateToDb)
          templateName = TemplateSettings.DecodeViewId(view.Id, false);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(userId, userId, ActionType.TemplateModified, DateTime.Now, templateName, view.Name));
      }
      catch (Exception ex)
      {
        Err.Reraise(typeof (EfolderDocTrackViewAccessor).Name, ex);
      }
      return documentTrackingView;
    }

    public bool DeleteView(string userId, string viewId)
    {
      bool flag = false;
      try
      {
        string a = this._isTemplateMigrateToDb ? viewId : TemplateSettings.DecodeViewId(viewId);
        if (a == this._stdViewId || string.Equals(a, EfolderDocTrackViewAccessor.StandardDocumentTrackingView.Name, StringComparison.CurrentCultureIgnoreCase))
          throw new ForbiddenException("Standard view is not allowed to be deleted.");
        flag = this.documentTrackingViewAccessor.DeleteView(userId, viewId);
        if (!this._isTemplateMigrateToDb)
          viewId = TemplateSettings.DecodeViewId(viewId);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new TemplateAuditRecord(userId, userId, ActionType.TemplateDeleted, DateTime.Now, viewId, viewId));
      }
      catch (Exception ex)
      {
        Err.Reraise(typeof (EfolderDocTrackViewAccessor).Name, ex);
      }
      return flag;
    }

    private static DocumentTrackingView StandardDocumentTrackingView
    {
      get
      {
        DocumentTrackingView documentTrackingView = new DocumentTrackingView("Standard View");
        TableLayout.Column column = new TableLayout.Column("NAME", "Name", HorizontalAlignment.Left, 225);
        column.SortOrder = SortOrder.Ascending;
        column.SortPriority = 0;
        List<TableLayout.Column> columnList = new List<TableLayout.Column>();
        columnList.InsertRange(0, (IEnumerable<TableLayout.Column>) new TableLayout.Column[10]
        {
          new TableLayout.Column("HASATTACHMENTS", "Attachments", HorizontalAlignment.Center, 26),
          new TableLayout.Column("HASCONDITIONS", "For Underwriting Condition", HorizontalAlignment.Center, 26),
          column,
          new TableLayout.Column("DESCRIPTION", "Description", HorizontalAlignment.Left, 225),
          new TableLayout.Column("BORROWER", "For Borrower Pair", HorizontalAlignment.Left, 145),
          new TableLayout.Column("DOCTYPE", "Type", HorizontalAlignment.Left, 110),
          new TableLayout.Column("DOCACCESS", "Access", HorizontalAlignment.Left, 110),
          new TableLayout.Column("MILESTONE", "For Milestone", HorizontalAlignment.Left, 125),
          new TableLayout.Column("DOCSTATUS", "Status", HorizontalAlignment.Left, 75),
          new TableLayout.Column("DATE", "Date", HorizontalAlignment.Left, 80)
        });
        TableLayout tableLayout = new TableLayout(columnList.ToArray());
        documentTrackingView.Layout = tableLayout;
        documentTrackingView.DocGroup = "(All Documents)";
        documentTrackingView.StackingOrder = "None";
        return documentTrackingView;
      }
    }
  }
}
