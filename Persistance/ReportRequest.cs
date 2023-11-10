using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using Incoding.Core.Data;
using Incoding.Core.Quality;
using JetBrains.Annotations;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Incoding.Core.Extensions.LinqSpecs;
using kip_service_test.Persistance;
using Newtonsoft.Json;
using NHibernate.Properties;

namespace kip_service_test
{
    public class ReportRequest : KipEntityBase
    {
        public new virtual int Id { get; set; }
        public virtual Guid QueryGuid { get; set; }
        public virtual string UserId { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndTime { get; set; }
        public virtual bool IsCompleted { get; set; }

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : ClassMap<ReportRequest>
        {
            public Map()
            {
                Id(r => r.Id ).GeneratedBy.Identity();
                Map(s => s.QueryGuid);
                Map(s => s.UserId);
                Map(s => s.StartTime);
                Map(s => s.EndTime);
                Map(y => y.IsCompleted);
            }
        }
         
        public class Where
        {
            public class ByQueryGuid:Specification<ReportRequest>
            {
                private Guid _guid;

                public ByQueryGuid( Guid guid)
                {
                    _guid = guid;
                }
                public override Expression<Func<ReportRequest, bool>> IsSatisfiedBy()
                {
                   return t=>t.QueryGuid==_guid;
                }
            }
        }
    }
}