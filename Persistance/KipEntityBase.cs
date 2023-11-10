using System;
using Incoding.Core;
using Incoding.Core.Data;
using Incoding.Core.Extensions;
using Incoding.Core.Quality;

namespace kip_service_test.Persistance
{
    public class KipEntityBase:IEntity
    {
       
        public virtual object Id { get; set; }

        public override int GetHashCode() => this.Id.ReturnOrDefault<object, int>((Func<object, int>)(r => r.GetHashCode()), 0);

        public override bool Equals(object obj) => this.IsReferenceEquals(obj) && this.GetHashCode().Equals(obj.GetHashCode());

        public static bool operator ==(KipEntityBase left, KipEntityBase right) => object.Equals((object)left, (object)right);

        public static bool operator !=(KipEntityBase left, KipEntityBase right) => !object.Equals((object)left, (object)right);
    }
}