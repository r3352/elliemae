// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.EfolderDocTrackViewAccessorDB
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  internal class EfolderDocTrackViewAccessorDB : IEfolderDocTrackViewAccessor
  {
    private const string _cacheKeyPrefix = "eFolderDocView_�";

    public List<ViewSummary> GetViewsSummary(string userId)
    {
      List<ViewSummary> viewsSummary = (List<ViewSummary>) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT Id, [Name] From " + EfolderDocTrackViewAccessor.eFolderDocTrackViewTblName);
      dbQueryBuilder.AppendLine("WHERE UserId = @userId");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(new DbCommandParameter(nameof (userId), (object) userId, DbType.String));
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        viewsSummary = new List<ViewSummary>();
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          viewsSummary.Add(new ViewSummary()
          {
            Id = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Id"]),
            Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Name"])
          });
      }
      return viewsSummary;
    }

    public DocumentTrackingView GetView(string userId, string viewId)
    {
      if (!int.TryParse(viewId, out int _))
        throw new ObjectNotFoundException("A view with id '" + viewId + "' does not exist for the user: '" + userId + "'.", ObjectType.Template, (object) viewId);
      DocumentTrackingView view = ClientContext.GetCurrent()?.Cache?.Get<DocumentTrackingView>("eFolderDocView_" + userId + "_" + viewId, (Func<DocumentTrackingView>) (() =>
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT ID, [Name],Filters,Layouts, DocumentGroup, StackingOrder");
        dbQueryBuilder.AppendLine("From " + EfolderDocTrackViewAccessor.eFolderDocTrackViewTblName);
        dbQueryBuilder.AppendLine("WHERE [ID] = @Id AND [UserId] = @UserId");
        DbCommandParameter[] parameters = new DbCommandParameter[2]
        {
          new DbCommandParameter("Id", (object) viewId, DbType.Int64),
          new DbCommandParameter("UserId", (object) userId, DbType.String)
        };
        DataRow dataRow = dbQueryBuilder.ExecuteRowQuery(parameters);
        if (dataRow == null)
          return (DocumentTrackingView) null;
        return new DocumentTrackingView()
        {
          Id = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Id"]),
          Name = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Name"]),
          Filter = FieldFilterList.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Filters"]) == "NULL" ? (string) null : EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Filters"])),
          Layout = TableLayout.Parse(EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Layouts"]) == "NULL" ? (string) null : EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["Layouts"])),
          DocGroup = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["DocumentGroup"]) == "NULL" ? (string) null : EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["DocumentGroup"]),
          StackingOrder = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["StackingOrder"]) == "NULL" ? (string) null : EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["StackingOrder"])
        };
      }));
      if (view == null)
        return (DocumentTrackingView) null;
      string b = User.GetPrivateProfileString(userId, "DocumentTrackingView", "DefaultView");
      if (!string.IsNullOrWhiteSpace(b))
        b = b.Substring(b.LastIndexOf("\\") + 1);
      view.IsDefault = string.Equals(view.Name, b, StringComparison.CurrentCultureIgnoreCase);
      return view;
    }

    public DocumentTrackingView CreateView(string userId, DocumentTrackingView view)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbValueList values = new DbValueList()
        {
          new DbValue("Name", (object) view.Name),
          new DbValue("UserId", (object) userId),
          new DbValue("Filters", (object) view.Filter?.ToXml()),
          new DbValue("Layouts", (object) view.Layout?.ToXML()),
          new DbValue("DocumentGroup", (object) view.DocGroup),
          new DbValue("StackingOrder", (object) view.StackingOrder),
          new DbValue("CreatedBy", (object) userId)
        };
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(EfolderDocTrackViewAccessor.eFolderDocTrackViewTblName);
        dbQueryBuilder.InsertInto(table, values, true, true);
        object obj = dbQueryBuilder.ExecuteScalar();
        view.Id = obj.ToString();
        if (view.IsDefault)
          User.WritePrivateProfileString(userId, "DocumentTrackingView", "DefaultView", "Personal:\\" + userId + "\\" + view.Name);
        return view;
      }
      catch (Exception ex)
      {
        SqlException sqlException = ex is SqlException ? (SqlException) ex : (ex.InnerException is SqlException ? (SqlException) ex.InnerException : (SqlException) null);
        if (sqlException != null && sqlException.Number == 2627 && sqlException.Message.Contains("UserIdNameUnique"))
          throw new DuplicateObjectException("A view with the same name already exists for the user: '" + userId + "'.", ObjectType.Template, (object) view.Name);
        throw;
      }
    }

    public DocumentTrackingView UpdateView(string userId, DocumentTrackingView view)
    {
      if (!int.TryParse(view.Id, out int _))
        throw new ObjectNotFoundException("A view with id '" + view.Id + "' does not exist for the user: '" + userId + "'.", ObjectType.Template, (object) view.Id);
      return ClientContext.GetCurrent()?.Cache?.Put<DocumentTrackingView>("eFolderDocView_" + userId + "_" + view.Id, (Func<DocumentTrackingView>) (() =>
      {
        try
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          DbValueList values = new DbValueList()
          {
            new DbValue("Name", (object) view.Name),
            new DbValue("Filters", (object) view.Filter?.ToXml()),
            new DbValue("Layouts", (object) view.Layout?.ToXML()),
            new DbValue("DocumentGroup", (object) view.DocGroup),
            new DbValue("StackingOrder", (object) view.StackingOrder),
            new DbValue("LastModifiedDate", (object) DateTime.UtcNow),
            new DbValue("LastModifiedBy", (object) userId)
          };
          DbValueList keys = new DbValueList()
          {
            new DbValue("Id", (object) view.Id),
            new DbValue("UserId", (object) userId)
          };
          DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable(EfolderDocTrackViewAccessor.eFolderDocTrackViewTblName);
          dbQueryBuilder.Update(table, values, keys);
          if ((int) dbQueryBuilder.ExecuteNonQueryWithRowCount() <= 0)
            throw new ObjectNotFoundException("A view with id '" + view.Id + "' does not exist for the user: '" + userId + "'.", ObjectType.Template, (object) view.Id);
          if (view.IsDefault)
            User.WritePrivateProfileString(userId, "DocumentTrackingView", "DefaultView", "Personal:\\" + userId + "\\" + view.Name);
          return view;
        }
        catch (Exception ex)
        {
          SqlException sqlException = ex is SqlException ? (SqlException) ex : (ex.InnerException is SqlException ? (SqlException) ex.InnerException : (SqlException) null);
          if (sqlException != null && sqlException.Number == 2627 && sqlException.Message.Contains("UserIdNameUnique"))
            throw new DuplicateObjectException("A view with the same name already exists for the user: '" + userId + "'.", ObjectType.Template, (object) view.Name);
          throw;
        }
      }), CacheSetting.Low);
    }

    public bool DeleteView(string userId, string viewId)
    {
      if (!int.TryParse(viewId, out int _))
        throw new ObjectNotFoundException("A view with id '" + viewId + "' does not exist for the user: '" + userId + "'.", ObjectType.Template, (object) viewId);
      bool result = false;
      ClientContext.GetCurrent()?.Cache?.Remove("eFolderDocView_" + userId + "_" + viewId, (Action) (() =>
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE [ID] = {1}", (object) EfolderDocTrackViewAccessor.eFolderDocTrackViewTblName, (object) EllieMae.EMLite.DataAccess.SQL.EncodeString(viewId)));
        result = (int) dbQueryBuilder.ExecuteNonQueryWithRowCount() > 0;
      }), CacheSetting.Low);
      return result;
    }
  }
}
