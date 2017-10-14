namespace CommitteeAdministration.Models
{
    /// <summary>
    /// this enums is the type of committee condition when an operator inserts real values this condition should be calculated
    /// </summary>
    public enum CommitteeCondition
    {
        /// <summary>
        /// The very good condition (fingilish: besyar matloob)
        /// </summary>
        VeryGood,

        /// <summary>
        /// The good
        /// </summary>
        Good,

        /// <summary>
        /// The bad
        /// </summary>
        Bad,

        /// <summary>
        /// The extremely bad
        /// </summary>
        ExtremelyBad,
        /// <summary>
        /// The none, when a field has nothing in real value we return none
        /// </summary>
        None
    }
}