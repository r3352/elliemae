// Decompiled with JetBrains decompiler
// Type: Elli.SQE.DD.LoanQueryableFields
// Assembly: Elli.SQE.DD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD1B11DD-560E-4083-B59A-D629AF47C790
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.DD.dll

using Elli.Common.Extensions;
using Elli.Common.Fields;
using Elli.Common.ModelFields;
using Elli.Domain.Mortgage;
using Elli.SQE.DD.Mapping;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.SQE.DD
{
  public sealed class LoanQueryableFields
  {
    public const string RootName = "Loan";
    public const string MappingFieldPrefix = "SysMapping";
    private static LoanQueryableFields _instance;
    private static readonly Loan _loan = Loan.Create(Guid.NewGuid());
    private static readonly IList<EncompassField> _encompassFields = EncompassFieldData.GetAllFields();
    private ConcurrentDictionary<string, QueryableField> _cacheFields = new ConcurrentDictionary<string, QueryableField>();
    private LoanHierarchy _hierarchy;
    private List<QueryableField> _queryableFields;

    static LoanQueryableFields()
    {
      LoanQueryableFields.AddLoanSummaryFields();
      LoanQueryableFields.AddSupplementFields();
      LoanQueryableFields.AddVirtualFields();
    }

    public static LoanQueryableFields Instance
    {
      get
      {
        return LoanQueryableFields._instance ?? (LoanQueryableFields._instance = new LoanQueryableFields());
      }
    }

    private LoanQueryableFields()
    {
    }

    public static Loan LoanTemplate => LoanQueryableFields._loan;

    public static IList<EncompassField> EncompassFields => LoanQueryableFields._encompassFields;

    public LoanHierarchy Hierarchy => this._hierarchy ?? (this._hierarchy = this.ToStructure());

    public List<QueryableField> QueryableFields
    {
      get
      {
        if (this._queryableFields != null && this._queryableFields.Count > 0)
          return this._queryableFields;
        this._queryableFields = new List<QueryableField>();
        this.Hierarchy.GetChildrenOfType<LoanPropertyHierarchy>(HierarchyLevel.All).ForEach<LoanHierarchy>((Action<LoanHierarchy>) (x =>
        {
          this._queryableFields.Add(x.Field.ToQueryableField(x.Type));
          this._queryableFields.AddRange((IEnumerable<QueryableField>) x.CastTo<LoanPropertyHierarchy>().GenerateQueryableFieldsOfTheSameKind());
        }));
        return this._queryableFields;
      }
    }

    public QueryableField this[string fieldId]
    {
      get
      {
        QueryableField queryableField1;
        if (this._cacheFields.TryGetValue(fieldId, out queryableField1))
          return queryableField1;
        QueryableField queryableField2 = this.QueryableFields.Find((Predicate<QueryableField>) (x => string.Equals(x.ID, fieldId, StringComparison.OrdinalIgnoreCase)));
        if (queryableField2 != null)
          this._cacheFields.TryAdd(fieldId, queryableField2);
        return queryableField2;
      }
    }

    private LoanHierarchy ToStructure()
    {
      this.ClearCache();
      LoanHierarchy root = this.CreateRoot();
      foreach (EncompassField encompassField in (IEnumerable<EncompassField>) LoanQueryableFields.EncompassFields)
      {
        if (!encompassField.ModelPath.IsNullOrWhiteSpace())
        {
          encompassField.ModelPath = this.TranslateModelPath(encompassField.ModelPath);
          root.Insert(LoanQueryableFields.LoanTemplate, encompassField, ModelFieldPath.CreateWithFullPath(encompassField.ModelPath));
        }
      }
      return root;
    }

    private void ClearCache()
    {
      if (this._queryableFields != null)
        this._queryableFields.Clear();
      if (this._cacheFields == null)
        return;
      this._cacheFields.Clear();
    }

    private LoanHierarchy CreateRoot()
    {
      LoanEntityHierarchy root = new LoanEntityHierarchy((LoanHierarchy) null);
      root.Key = "Loan";
      root.Type = LoanQueryableFields.LoanTemplate.GetType();
      return (LoanHierarchy) root;
    }

    private string TranslateModelPath(string modelPath)
    {
      string str = modelPath;
      if (modelPath.IndexOf("Loan.CurrentApplication.", StringComparison.Ordinal) == 0)
        str = modelPath.Replace("Loan.CurrentApplication.", "Loan.Applications[0].");
      return str;
    }

    private static void AddEncompassField(string fieldId, string format, string modelPath)
    {
      LoanQueryableFields.AddEncompassField(new EncompassField()
      {
        ID = fieldId,
        Format = format,
        ModelPath = modelPath
      });
    }

    private static void AddEncompassField(string fieldId, EncompassField field)
    {
      if (field == null)
        return;
      EncompassField encompassField = field.Clone().CastTo<EncompassField>();
      encompassField.ID = fieldId;
      LoanQueryableFields._encompassFields.Add(encompassField);
    }

    private static void AddEncompassField(EncompassField field)
    {
      if (field == null)
        return;
      LoanQueryableFields._encompassFields.Add(field);
    }

    private static EncompassField GetEncompassField(string fieldId)
    {
      return LoanQueryableFields._encompassFields.FirstOrDefault<EncompassField>((Func<EncompassField, bool>) (field => field.ID == fieldId));
    }

    private bool IsDebugContinue(EncompassField field)
    {
      return !(field.ID == "1109") && !(field.ID == "LoanAmount");
    }

    private static void AddLoanSummaryFields()
    {
      LoanQueryableFields.AddEncompassField("LoanAmount", LoanQueryableFields.GetEncompassField("1109"));
      LoanQueryableFields.AddEncompassField("LoanNumber", LoanQueryableFields.GetEncompassField("364"));
      LoanQueryableFields.AddEncompassField("LoanFolder", "STRING", "Loan.LoanFolder");
    }

    private static void AddSupplementFields()
    {
      LoanQueryableFields.AddEncompassField("SysMappingFeesFeeType", "STRING", "Loan.Fees[(1 == 1)].FeeType");
      LoanQueryableFields.AddEncompassField("SysMappingContactType", "STRING", "Loan.Contacts[(1 == 1)].ContactType");
    }

    private static void AddVirtualFields()
    {
    }
  }
}
