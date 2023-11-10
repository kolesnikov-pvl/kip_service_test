using FluentMigrator;

namespace kip_service_test
{
    [Migration(1, "Added table ReportRequest")]
    public class Step1 : Migration
    {
        public override void Up()
        {
            if (!Schema.Table(nameof(ReportRequest)).Exists())
            {
                Create.Table(nameof(ReportRequest))
                    .WithColumn(nameof(ReportRequest.Id)).AsInt32().Identity().PrimaryKey()
                    .WithColumn(nameof(ReportRequest.UserId)).AsString(255)
                    .WithColumn(nameof(ReportRequest.StartTime)).AsDateTime().Nullable()
                    .WithColumn(nameof(ReportRequest.EndTime)).AsDateTime().NotNullable()
                    .WithColumn(nameof(ReportRequest.IsCompleted)).AsBoolean().Nullable()
                    .WithColumn(nameof(ReportRequest.QueryGuid)).AsGuid();
            }
        }

        public override void Down() { }
    }
}