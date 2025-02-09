// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.SubmissionsCursor
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.DataDocs;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class SubmissionsCursor : ICursor, IDisposable
  {
    private DataDocsServiceHelper _serviceHelper = new DataDocsServiceHelper();
    private DeliveryStatus[] _deliveryStatuses;

    public SubmissionsCursor(DeliveryStatus[] deliveryStatuses)
    {
      this._deliveryStatuses = deliveryStatuses;
    }

    public int GetItemCount() => this._serviceHelper.GetSubmissionsCount();

    public int GetItemCount(int sqlRead) => this.GetItemCount();

    public object GetItem(int index, bool isExternalOrganization) => this.GetItem(index);

    public object GetItem(int index, bool isExternalOrganization, bool excludeArchivedLoans)
    {
      return this.GetItem(index);
    }

    public object GetItem(int index) => this.GetItems(index, 1)[0];

    public object[] GetItems(int startIndex, int count) => this.GetItems(startIndex, count, "");

    public object[] GetItems(int startIndex, int count, string sortOrder)
    {
      return (object[]) this._serviceHelper.GetSubmissions(startIndex, count, sortOrder);
    }

    public object[] GetItems(int startIndex, int count, bool isExternalOrganization)
    {
      return this.GetItems(startIndex, count);
    }

    public object[] GetItems(int startIndex, int count, bool isExternalOrganization, int sqlRead)
    {
      return this.GetItems(startIndex, count);
    }

    public object[] GetItems(
      int startIndex,
      int count,
      bool isExternalOrganization,
      int sqlRead,
      bool excludeArchivedLoans)
    {
      return this.GetItems(startIndex, count);
    }

    public void Dispose()
    {
    }

    public void SubmitForDelivery(List<SubmissionStatus> submissions)
    {
      this._serviceHelper.SubmitForDelivery(submissions);
    }

    public void RemoveFromDelivery(List<SubmissionStatus> submissions)
    {
      this._serviceHelper.RemoveFromDelivery(submissions);
    }

    public object[] GetFilteredSubmissions(
      int startIndex,
      int count,
      FieldFilterList filters,
      string sortOrder = "")
    {
      return (object[]) this._serviceHelper.GetFilteredSubmissions(startIndex, count, filters, sortOrder);
    }

    public void ClearFilter() => this._serviceHelper.ClearFilter();

    public string GetAuditReport(SubmissionStatus submission)
    {
      return this._serviceHelper.GetAuditReport(submission);
    }
  }
}
