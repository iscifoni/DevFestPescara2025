namespace MIB.Core.Domain
{
    public abstract class Entity
    {
        public virtual object Id { get; set; } = default!;
    }

    public class EntitySQL : Entity
    {
        new public virtual long Id { get; set; }
    }

    public abstract class Entity<T> : Entity
    {
        new public virtual T Id
        {
#pragma warning disable CS8603 // Possible null reference return.
            get => base.Id == null ? default : (T)base.Id;
#pragma warning restore CS8603 // Possible null reference return.
            set => base.Id = value!;
        }
    }
}