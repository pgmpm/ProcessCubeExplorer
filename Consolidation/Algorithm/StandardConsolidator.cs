using pgmpm.MatrixSelection.Fields;
using pgmpm.Model.PetriNet;
using System;
using System.Collections.Generic;

namespace pgmpm.Consolidation.Algorithm
{
    /// <summary>
    /// Class for the consolidation-algorithm 'StandardConsolidator'
    /// </summary>
    /// <author>Bernhard Bruns, Moritz Eversmann, Christopher Licht</author>
    public class StandardConsolidator : IConsolidator
    {
        private HashSet<Field> _listOfMatrixFields;
        private HashSet<Field> _resultConsolidationListOr = new HashSet<Field>();
        private HashSet<Field> _resultConsolidationListAnd = new HashSet<Field>();

        private HashSet<String> _listOfSelectedOptions;
        private HashSet<String> _listOfSelectedEvents;
        private int _numberOfEvents;
        private int _andOrSelection;


        /// <summary>
        /// Constructor for the StandardConsolidator
        /// </summary>
        /// <param name="listOfMatrixFields"></param>
        public StandardConsolidator(HashSet<Field> listOfMatrixFields)
        {
            _listOfMatrixFields = listOfMatrixFields;

            _listOfSelectedOptions = (HashSet<String>)ConsolidatorSettings.Get("SelectedOptions");
            _listOfSelectedEvents = (HashSet<String>)ConsolidatorSettings.Get("SelectedEvents");
            _numberOfEvents = (int)ConsolidatorSettings.Get("NumberOfEvents");
            _andOrSelection = (int)ConsolidatorSettings.Get("AndOrSelection");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List which contains the consolidated results</returns>
        public HashSet<Field> Consolidate()
        {

            if (ConsolidatorSettings.ProcessModelType == typeof(PetriNet))
                ConsolidateForPetrinet();
            else
                throw new NotImplementedException();


            if (_andOrSelection == 1)
                return _resultConsolidationListOr;

            return _resultConsolidationListAnd;
        }

        /// <summary>
        /// This function consolidate the results only for petrinet
        /// </summary>
        private void ConsolidateForPetrinet()
        {
            if (_listOfSelectedOptions.Contains(ConsolidatorSettings.ConsolidationOptions[0]))
            {
                _resultConsolidationListOr.UnionWith(GetListOfFieldsWithLoops());

                if (_resultConsolidationListAnd.Count == 0)
                    _resultConsolidationListAnd.UnionWith(GetListOfFieldsWithLoops());
                else
                    _resultConsolidationListAnd.IntersectWith(GetListOfFieldsWithLoops());
            }

            if (_listOfSelectedOptions.Contains(ConsolidatorSettings.ConsolidationOptions[1]))
            {
                _resultConsolidationListOr.UnionWith(GetListOfFieldsWithParallelism());

                if (_resultConsolidationListAnd.Count == 0)
                    _resultConsolidationListAnd.UnionWith(GetListOfFieldsWithParallelism());
                else
                    _resultConsolidationListAnd.IntersectWith(GetListOfFieldsWithParallelism());
            }

            if (_listOfSelectedOptions.Contains(ConsolidatorSettings.ConsolidationOptions[2]))
            {
                _resultConsolidationListOr.UnionWith(GetListOfFieldsWithSelectedEvents());

                if (_resultConsolidationListAnd.Count == 0)
                    _resultConsolidationListAnd.UnionWith(GetListOfFieldsWithSelectedEvents());
                else
                    _resultConsolidationListAnd.IntersectWith(GetListOfFieldsWithSelectedEvents());
            }

            if (_listOfSelectedOptions.Contains(ConsolidatorSettings.ConsolidationOptions[3]))
            {
                _resultConsolidationListOr.UnionWith(GetListOfFieldsWithMinimumNumberOfEvents());

                if (_resultConsolidationListAnd.Count == 0)
                    _resultConsolidationListAnd.UnionWith(GetListOfFieldsWithMinimumNumberOfEvents());
                else
                    _resultConsolidationListAnd.IntersectWith(GetListOfFieldsWithMinimumNumberOfEvents());
            }
        }

        /// <summary>
        /// Returns a list of fields with loops
        /// </summary>
        /// <returns></returns>
        private HashSet<Field> GetListOfFieldsWithLoops()
        {
            HashSet<Field> listWithLoops = new HashSet<Field>();

            foreach (Field currentField in _listOfMatrixFields)
            {
                if (currentField.ProcessModel != null)
                {
                    if (currentField.ProcessModel.GetType() == typeof(PetriNet))
                    {
                        var currentPetriNet = (PetriNet)currentField.ProcessModel;

                        if (currentPetriNet.CountLoops() > 0)
                            listWithLoops.Add(currentField);
                    }
                    else
                        throw new NotImplementedException();
                }
            }
            return listWithLoops;
        }

        /// <summary>
        /// Returns a list of processmodels with a minimum number of events.
        /// </summary>
        /// <author>Bernhard Bruns</author>
        private HashSet<Field> GetListOfFieldsWithMinimumNumberOfEvents()
        {
            HashSet<Field> listWithNumberOfEvents = new HashSet<Field>();

            foreach (Field currentField in _listOfMatrixFields)
            {
                if (currentField.ProcessModel != null)
                {
                    if (currentField.ProcessModel.GetType() == typeof(PetriNet))
                    {
                        var currentPetriNet = (PetriNet)currentField.ProcessModel;

                        if (currentPetriNet.Transitions.Count >= _numberOfEvents)
                            listWithNumberOfEvents.Add(currentField);
                    }
                    else
                        throw new NotImplementedException();
                }
            }

            return listWithNumberOfEvents;
        }

        /// <summary>
        /// Search for parallelism in the process models of the fields.
        /// </summary>
        /// <returns>List of fields with parallelism.</returns>
        /// <author>Christopher Licht</author>
        private HashSet<Field> GetListOfFieldsWithParallelism()
        {
            Boolean andJoin;
            Boolean andSplit;
            HashSet<Field> listWithParallelism = new HashSet<Field>();

            foreach (Field currentField in _listOfMatrixFields)
            {
                andJoin = false;
                andSplit = false;

                if (currentField.ProcessModel != null)
                {
                    if (currentField.ProcessModel.GetType() == typeof(PetriNet))
                    {
                        var currentPetriNet = (PetriNet)currentField.ProcessModel;

                        foreach (var currentTransition in currentPetriNet.Transitions)
                        {
                            if (currentTransition.IsANDJoin)
                                andJoin = true;

                            if (currentTransition.IsANDSplit)
                                andSplit = true;
                        }

                        if (andJoin && andSplit)
                            listWithParallelism.Add(currentField);
                    }
                    else
                        throw new NotImplementedException();
                }


            }
            return listWithParallelism;
        }

        /// <summary>
        /// Returns a list of processmodels with the selected Events
        /// </summary>
        /// /// <author>Moritz Eversmann, Bernhard Bruns</author>
        /// <returns></returns>
        private HashSet<Field> GetListOfFieldsWithSelectedEvents()
        {
            HashSet<Field> listWithSelectedEvents = new HashSet<Field>();

            foreach (Field currentField in _listOfMatrixFields)
            {
                if (currentField.ProcessModel != null)
                {
                    if (currentField.ProcessModel.GetType() == typeof(PetriNet))
                    {
                        var currentPetriNet = (PetriNet)currentField.ProcessModel;

                        foreach (Transition currentTransition in currentPetriNet.Transitions)
                        {
                            if (_listOfSelectedEvents.Contains(currentTransition.Name))
                                listWithSelectedEvents.Add(currentField);
                        }
                    }
                    else
                        throw new NotImplementedException();
                }
            }
            return listWithSelectedEvents;
        }
    }
}
