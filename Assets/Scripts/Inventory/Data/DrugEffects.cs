namespace Inventory.Data
{
    /// <summary>
    /// Represents a property of a drug.
    ///
    /// Drug effects are primarily used by the economy system
    /// to determine regional demand.
    /// </summary>
    public enum DrugEffect
    {
        PainRelief,
        Energy,
        Focus,
        Relaxation,
        Hallucination,
        Appetite
    }
}