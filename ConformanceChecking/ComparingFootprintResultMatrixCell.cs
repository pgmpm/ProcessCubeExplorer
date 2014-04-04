using System;

namespace pgmpm.ConformanceChecking
{
    /// <summary>
    /// A representation of the CellStates of a ResultFootprint
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public static class ComparingFootprintResultMatrixCell
    {
        /// <summary>
        /// Dictionary with all available cell types of a result footprint. That shows the differences between two footprints
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        private static KeyPairDictonary<CellType, CellType, ResultCellType> _keyPairDictionaryResultCellTypeDefinition;
        static KeyPairDictonary<CellType, CellType, ResultCellType> KeyPairDictionaryResultCellTypeDefinition
        {
            get
            {
                if (_keyPairDictionaryResultCellTypeDefinition == null)
                {
                    _keyPairDictionaryResultCellTypeDefinition = new KeyPairDictonary<CellType, CellType, ResultCellType>();

                    _keyPairDictionaryResultCellTypeDefinition[CellType.Left, CellType.Nothing] = ResultCellType.LeftAndNothing;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Left, CellType.Right] = ResultCellType.LeftAndRight;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Left, CellType.Parallel] = ResultCellType.LeftAndParallel;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Left, CellType.NotExist] = ResultCellType.LeftAndNotExist;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Left, CellType.Loop] = ResultCellType.LeftAndLoop;

                    _keyPairDictionaryResultCellTypeDefinition[CellType.Right, CellType.Nothing] = ResultCellType.RightAndNothing;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Right, CellType.Left] = ResultCellType.RightAndLeft;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Right, CellType.Parallel] = ResultCellType.RightAndParallel;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Right, CellType.NotExist] = ResultCellType.RightAndNotExist;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Right, CellType.Loop] = ResultCellType.RightAndLoop;

                    _keyPairDictionaryResultCellTypeDefinition[CellType.Parallel, CellType.Left] = ResultCellType.ParallelAndLeft;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Parallel, CellType.Right] = ResultCellType.ParallelAndRight;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Parallel, CellType.Nothing] = ResultCellType.ParallelAndNothing;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Parallel, CellType.NotExist] = ResultCellType.ParallelAndNotExist;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Parallel, CellType.Loop] = ResultCellType.ParallelAndLoop;

                    _keyPairDictionaryResultCellTypeDefinition[CellType.NotExist, CellType.Nothing] = ResultCellType.NotExistAndNothing;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.NotExist, CellType.Left] = ResultCellType.NotExistAndLeft;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.NotExist, CellType.Right] = ResultCellType.NotExistAndRight;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.NotExist, CellType.Parallel] = ResultCellType.NotExistAndParallel;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.NotExist, CellType.Loop] = ResultCellType.NotExistAndLoop;

                    _keyPairDictionaryResultCellTypeDefinition[CellType.Nothing, CellType.NotExist] = ResultCellType.NothingAndNotExist;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Nothing, CellType.Left] = ResultCellType.NothingAndLeft;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Nothing, CellType.Right] = ResultCellType.NothingAndRight;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Nothing, CellType.Parallel] = ResultCellType.NothingAndParallel;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Nothing, CellType.Loop] = ResultCellType.NothingAndLoop;

                    _keyPairDictionaryResultCellTypeDefinition[CellType.Loop, CellType.NotExist] = ResultCellType.LoopAndNotExist;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Loop, CellType.Nothing] = ResultCellType.LoopAndNothing;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Loop, CellType.Left] = ResultCellType.LoopAndLeft;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Loop, CellType.Right] = ResultCellType.LoopAndRight;
                    _keyPairDictionaryResultCellTypeDefinition[CellType.Loop, CellType.Parallel] = ResultCellType.LoopAndParallel;
                }
                return _keyPairDictionaryResultCellTypeDefinition;
            }
        }

        /// <summary>
        /// Constructor that finds the result cell state who shows the difference between two other cell states.
        /// The object of this class change the state for the representation of the difference.
        /// </summary>
        /// <param name="cellType1">A cell state</param>
        /// <param name="cellType2">A second cell state to compare each other</param>
        /// <autor>Andrej Albrecht</autor>
        public static ResultCellType GetResultCellType(CellType cellType1, CellType cellType2)
        {
            try
            {
                return KeyPairDictionaryResultCellTypeDefinition[cellType1, cellType2];
            }
            catch (Exception)
            {
                return ResultCellType.NoDifferences;
            }
        }
    }


    /// <summary>
    /// Result cell types.
    /// Shows the relationship between two cells in a footprint.
    /// </summary>
    public enum ResultCellType
    {
        NoDifferences,

        RightAndNothing,
        RightAndLeft,
        RightAndParallel,
        RightAndNotExist,
        RightAndLoop,

        LeftAndNotExist,
        LeftAndNothing,
        LeftAndRight,
        LeftAndParallel,
        LeftAndLoop,

        ParallelAndNothing,
        ParallelAndRight,
        ParallelAndLeft,
        ParallelAndNotExist,
        ParallelAndLoop,

        NotExistAndLoop,
        NotExistAndNothing,
        NotExistAndLeft,
        NotExistAndRight,
        NotExistAndParallel,

        LoopAndNotExist,
        LoopAndNothing,
        LoopAndLeft,
        LoopAndRight,
        LoopAndParallel,

        NothingAndNotExist,
        NothingAndLoop,
        NothingAndRight,
        NothingAndLeft,
        NothingAndParallel
    };
}
