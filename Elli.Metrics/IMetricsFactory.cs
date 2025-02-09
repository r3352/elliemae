// Decompiled with JetBrains decompiler
// Type: Elli.Metrics.IMetricsFactory
// Assembly: Elli.Metrics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D16DD1E4-F07E-4CB7-B2D0-A2DD51E6F671
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Metrics.dll

#nullable disable
namespace Elli.Metrics
{
  public interface IMetricsFactory
  {
    bool MetricsCollectionEnabled { get; }

    IFormBuilderMetricsRecorder CreateFormBuilderMetricsRecorder(string client, string instance);

    ILoanMetricsRecorder CreateLoanMetricsRecorder(string client, string instance);

    IMongoUtilityMetricsRecorder CreateMongoUtilityMetricsRecorder(string client, string instance);

    ILoanPipelineMetricsRecorder CreateLoanPipelineMetricsRecorder(string client, string instance);

    IContactsPaginationMetricsRecorder CreateContactsPaginationMetricsRecorder(
      string client,
      string instance);

    IHazelCastMetricsRecorder CreateHazelCastMetricsRecorder();

    ILoanSerializationMatrixRecorder CreateLoanSerializationMetricsRecorder(
      string client,
      string instance);

    ILoanAccessorMetricsRecorder CreateLoanAccessorMetricsRecorder(string client, string instance);
  }
}
