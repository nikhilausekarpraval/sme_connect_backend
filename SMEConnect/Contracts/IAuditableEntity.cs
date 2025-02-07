namespace SMEConnect.Contracts
{

    public interface IAuditableEntity
    {
        DateTime? ModifiedOnDt { get; set; }
        string? ModifiedBy { get; set; }
    }

}
