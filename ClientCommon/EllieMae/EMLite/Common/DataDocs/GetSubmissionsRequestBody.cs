// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DataDocs.GetSubmissionsRequestBody
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer.Reporting;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Common.DataDocs
{
  public class GetSubmissionsRequestBody
  {
    public static GetSubmissionsRequestBody GetRequestBody(
      string instanceId,
      string userId,
      bool isAdmin,
      string sortIndicator,
      FieldFilterList filters)
    {
      GetSubmissionsRequestBody requestBody = new GetSubmissionsRequestBody();
      string[] deliveryStatuses = new string[8]
      {
        "InProgress",
        "Completed",
        "Error",
        "Cancelled",
        "Submitted",
        "Delivered",
        "Accepted",
        "Rejected"
      };
      List<string> stringList = new List<string>();
      if (filters != null && filters.Count != 0)
      {
        requestBody.Filter = new Filter();
        requestBody.Filter.Operator = "AND";
        requestBody.Filter.Terms = new ArrayList();
        foreach (FieldFilter filter in filters.Where<FieldFilter>((Func<FieldFilter, bool>) (f => f.FieldDescription != "Status")))
        {
          Term term = GetSubmissionsRequestBody.mapFilterToTerm(filter, instanceId, userId);
          requestBody.Filter.Terms.Add((object) term);
        }
        foreach (FieldFilter fieldFilter in filters.Where<FieldFilter>((Func<FieldFilter, bool>) (f => f.FieldDescription == "Status")))
          stringList.Add(fieldFilter.ValueDescription.Replace(" ", ""));
        if (stringList.Count > 0)
          requestBody.Filter.Terms.Add(GetSubmissionsRequestBody.GetTermsForDeliveryStatuses(stringList.ToArray()));
        else
          requestBody.Filter.Terms.Add(GetSubmissionsRequestBody.GetTermsForDeliveryStatuses(deliveryStatuses));
      }
      requestBody.SortOrder = new SortOrderIndicator();
      requestBody.SortOrder.CanonicalName = "CreatedDate";
      requestBody.SortOrder.Value = string.IsNullOrEmpty(sortIndicator) ? "desc" : sortIndicator;
      return requestBody;
    }

    private static Term mapFilterToTerm(FieldFilter filter, string instanceId, string userId)
    {
      Term term = new Term();
      term.Value = filter.ValueDescription;
      switch (filter.FieldDescription)
      {
        case "Audit Green":
          term.CanonicalName = "AuditcountYellow";
          term.MatchType = GetSubmissionsRequestBody.MapNumericFilterOperatorToMatchType(filter.OperatorType);
          break;
        case "Audit Red":
          term.CanonicalName = "AuditcountRed";
          term.MatchType = GetSubmissionsRequestBody.MapNumericFilterOperatorToMatchType(filter.OperatorType);
          break;
        case "Audit Yellow":
          term.CanonicalName = "AuditcountGreen";
          term.MatchType = GetSubmissionsRequestBody.MapNumericFilterOperatorToMatchType(filter.OperatorType);
          break;
        case "Created By":
          term.CanonicalName = "CreatedBy";
          term.Value = string.Format("encompass\\{0}\\{1}", (object) instanceId, (object) userId);
          term.MatchType = "equals";
          break;
        case "Loan Number":
          term.CanonicalName = "loannumber";
          term.MatchType = "equals";
          break;
        case "Recipient Transaction ID":
          term.CanonicalName = "OrderId";
          term.MatchType = "equals";
          break;
        case "Reference ID":
          term.CanonicalName = "ReferenceId";
          term.MatchType = "equals";
          break;
        case "Submission Type":
          term.CanonicalName = "submissionType";
          term.MatchType = "equals";
          break;
        case "Submitted To":
          term.CanonicalName = "SubmittedTo";
          term.MatchType = "equals";
          break;
      }
      return term;
    }

    private static string MapNumericFilterOperatorToMatchType(OperatorTypes type)
    {
      string matchType = "equals";
      switch (type)
      {
        case OperatorTypes.Equals:
          matchType = "equals";
          break;
        case OperatorTypes.NotEqual:
          matchType = "notequals";
          break;
        case OperatorTypes.GreaterThan:
          matchType = "gt";
          break;
        case OperatorTypes.NotGreaterThan:
          matchType = "lte";
          break;
        case OperatorTypes.LessThan:
          matchType = "lt";
          break;
        case OperatorTypes.NotLessThan:
          matchType = "gte";
          break;
      }
      return matchType;
    }

    private static object GetTermsForDeliveryStatuses(string[] deliveryStatuses)
    {
      Filter deliveryStatuses1 = new Filter();
      deliveryStatuses1.Operator = "OR";
      deliveryStatuses1.Terms = new ArrayList();
      foreach (string deliveryStatuse in deliveryStatuses)
        deliveryStatuses1.Terms.Add((object) new Term()
        {
          CanonicalName = "Status",
          Value = deliveryStatuse,
          MatchType = "equals"
        });
      return (object) deliveryStatuses1;
    }

    private GetSubmissionsRequestBody()
    {
    }

    [JsonProperty("filter")]
    public Filter Filter { get; set; }

    [JsonProperty("sortOrder")]
    public SortOrderIndicator SortOrder { get; set; }
  }
}
