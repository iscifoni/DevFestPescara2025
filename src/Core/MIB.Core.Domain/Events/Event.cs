namespace MIB.Core.Domain.Events
{
    public abstract class Event
    {
        public DateTime Data { get; protected set; }

        protected Event() => Data = DateTime.Now;

        public void SetParameters(string unitaLavorativa, string userName, string utente)
        {
            if (!string.IsNullOrWhiteSpace(UnitaLavorativa) || !string.IsNullOrWhiteSpace(UserName) || !string.IsNullOrWhiteSpace(Utente))
                throw new InvalidOperationException("Parameters have already been set.");

            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException($"'{nameof(userName)}' cannot be null or whitespace.", nameof(userName));

            if (string.IsNullOrWhiteSpace(utente))
                throw new ArgumentException($"'{nameof(utente)}' cannot be null or whitespace.", nameof(utente));

            UnitaLavorativa = unitaLavorativa;
            UserName = userName;
            Utente = utente;
        }

        public string UnitaLavorativa { get; protected set; } = null!;

        public string UserName { get; protected set; } = null!;

        public string Utente { get; protected set; } = null!;
    }
}