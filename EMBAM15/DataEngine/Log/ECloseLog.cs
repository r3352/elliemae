// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.ECloseLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Xml.AutoMapping;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class ECloseLog : LogRecordBase
  {
    public static readonly string XmlType = "EClose";

    static ECloseLog()
    {
      XmlAutoMapper.AddProfile<ECloseLog>(XmlAutoMapper.NewProfile<ECloseLog>().ForMember<bool>((Expression<Func<ECloseLog, bool>>) (dt => dt.DisplayInLog), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<bool>((Expression<Func<ECloseLog, bool>>) (dt => dt.IsRemoved), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<bool>((Expression<Func<ECloseLog, bool>>) (dt => dt.IsNew), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<string>((Expression<Func<ECloseLog, string>>) (dt => dt.Comments), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<string>>) (opts => opts.Ignore())).ForMember<LogList>((Expression<Func<ECloseLog, LogList>>) (dt => dt.Log), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<LogList>>) (opts => opts.Ignore())).ForMember<bool>((Expression<Func<ECloseLog, bool>>) (dt => dt.IsAttachedToLog), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<string>((Expression<Func<ECloseLog, string>>) (dt => dt.Guid), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<string>>) (opts => opts.Ignore())).ForMember<DateTime>((Expression<Func<ECloseLog, DateTime>>) (dt => dt.DateUpdated), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<DateTime>>) (opts => opts.Ignore())).ForMember<bool>((Expression<Func<ECloseLog, bool>>) (dt => dt.IsLoanOperationalLog), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<bool>>) (opts => opts.Ignore())).ForMember<Audit>((Expression<Func<ECloseLog, Audit>>) (dt => dt.Audit), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<Audit>>) (opts => opts.CreateUsing((Func<IXmlMapperContext, ECloseLog, Audit>) ((xmlElement, parent) => new Audit(parent))))).ForCollectionMember<ECloseLog.Party>((Expression<Func<ECloseLog, IList<ECloseLog.Party>>>) (dt => dt.Parties), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<IList<ECloseLog.Party>, ECloseLog.Party>>) (opts => opts.CreateItemUsing((Func<IXmlMapperContext, ECloseLog, ECloseLog.Party>) ((xmlElement, parent) => new ECloseLog.Party())))).ForCollectionMember<ECloseLog.Document>((Expression<Func<ECloseLog, IList<ECloseLog.Document>>>) (dt => dt.Documents), (Action<XmlAutoMapper.Profile<ECloseLog>.ProfileOptions<IList<ECloseLog.Document>, ECloseLog.Document>>) (opts => opts.CreateItemUsing((Func<IXmlMapperContext, ECloseLog, ECloseLog.Document>) ((xmlElement, parent) => new ECloseLog.Document(parent))))));
    }

    protected override bool ReadDateInUtc => true;

    public ECloseLog()
      : base(DateTime.UtcNow, string.Empty)
    {
      this.Parties = (IList<ECloseLog.Party>) new List<ECloseLog.Party>();
      this.Documents = (IList<ECloseLog.Document>) new List<ECloseLog.Document>();
      this.Audit = new Audit(this);
    }

    public ECloseLog(LogList log, XmlElement e)
      : base(log, e)
    {
      XmlAutoMapper.FromXml((IXmlMapperContext) new XmlElementBasedMapperContext(e), (object) this);
      this.MarkAsClean();
    }

    public string UserID { get; set; }

    public string OrderID { get; set; }

    public IList<ECloseLog.Party> Parties { get; set; }

    public IList<ECloseLog.Document> Documents { get; set; }

    public Audit Audit { get; set; }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      new AttributeWriter(e).Write("Type", (object) ECloseLog.XmlType);
      XmlAutoMapper.ToXml((object) this, (IXmlMapperContext) new XmlElementBasedMapperContext(e));
    }

    public class Party
    {
      public string Id { get; set; }

      public string Type { get; set; }

      public string FullName { get; set; }

      public ECloseLog.LoanEntity LoanEntity { get; set; }
    }

    public class LoanEntity
    {
      public string EntityId { get; set; }

      public string EntityType { get; set; }
    }

    public class Document
    {
      private ECloseLog _parent;

      public string Id { get; set; }

      public string Title { get; set; }

      public string Type { get; set; }

      public string SignatureType { get; set; }

      public string FileKey { get; set; }

      public string Size { get; set; }

      public Document(ECloseLog parent) => this._parent = parent;

      public Document()
      {
      }
    }
  }
}
