namespace pgmpm.MiningAlgorithm.InductiveV2
{
    /// <summary>
    /// Tree node operations
    /// </summary>
    /// <author>Thomas Meents, Bernd Nottbeck</author>
    public enum OperationsEnum
    {
        isXOR, 
        isLoop,
        isSequence, 
        isParallel, 
        isLeaf,
        isUnkown
    }
}