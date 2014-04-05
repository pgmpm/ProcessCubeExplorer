//<author>Christopher Licht, Bernhard Bruns, Moritz Eversmann</author>
using pgmpm.MatrixSelection.Fields;
using pgmpm.Model.PetriNet;
using System;
using System.Collections.Generic;
namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// This class provides helper-methods for the consolidation
    /// </summary>
    /// <author>Christopher Licht, Bernhard Bruns, Moritz Eversmann</author>
    public static class ConsolidationHelper
    {
        /// <summary>
        /// To start the consolidation process.
        /// </summary>
        /// <param name="_listWithFields">List with all fields of the matrix-selection.</param>
        /// <param name="_listOfSelectedOptions">List with the selected options of the list view.</param>
        /// <param name="_listOfSelectedEvents">List with selected events from the listbox.</param>
        /// <param name="andOrSelection">Integer (0 == AND) (1 == OR)</param>
        /// <param name="numberOfEvents"></param>
        /// <returns>Returns all fields which fulfill the condition of the selected options.</returns>
        /// <author>Christopher Licht, Moritz Eversmann</author>
        public static HashSet<Field> StartConsolidation(HashSet<Field> _listWithFields, HashSet<String> _listOfSelectedOptions, HashSet<String> _listOfSelectedEvents, int andOrSelection, int numberOfEvents)
        {
            HashSet<Field> _ListOfSelectedFields = new HashSet<Field>();
            if (andOrSelection == 0)
                return
                _ListOfSelectedFields = StartANDHandling(_listWithFields, _listOfSelectedOptions, _listOfSelectedEvents, numberOfEvents);
            else
                return
                _ListOfSelectedFields = StartORHandling(_listWithFields, _listOfSelectedOptions, _listOfSelectedEvents, numberOfEvents);
        }

        /// <summary>
        /// To start the and-handling process.
        /// </summary>
        /// <param name="_listWithFields">List with all fields of the matrix-selection.</param>
        /// <param name="_listOfSelectedOptions">List with selected options.</param>
        /// <param name="_listOfEvents">List with selected events from the listbox.</param>
        /// <param name="numberOfEvents"></param>
        /// <returns>Returns a hash set with fields, which fulfilled the necessary conditions.</returns>
        /// <author>Christopher Licht</author>
        private static HashSet<Field> StartANDHandling(HashSet<Field> _listWithFields, HashSet<String> _listOfSelectedOptions, HashSet<String> _listOfEvents, int numberOfEvents)
        {
            HashSet<Field> _ListOfFieldsWithAND = new HashSet<Field>();
            Boolean isLoopExisting = false;
            Boolean isParallelismExisting = false;
            Boolean isSelectedEventExisting = false;
            Boolean isProcessmodelWithNumberOfEventsExisting = false;
            int countExistingTransition = 0;

            foreach (var currentField in _listWithFields)
                CallForEverySelectedOptionTheNecessaryMethodsAND(_listOfSelectedOptions, _listOfEvents, _ListOfFieldsWithAND, ref isLoopExisting, ref isParallelismExisting, ref isSelectedEventExisting, ref countExistingTransition, currentField, numberOfEvents, ref isProcessmodelWithNumberOfEventsExisting);

            return _ListOfFieldsWithAND;
        }

        /// <summary>
        /// This method calls the methods which depends on the selected options from the listbox.
        /// </summary>
        /// <param name="_listOfSelectedOptions">List with selected options.</param>
        /// <param name="_listOfEvents">For checking the current transition against all selected events.</param>
        /// <param name="_ListOfFieldsWithAND">Contains the fields which fulfill the conditions.</param>
        /// <param name="IsLoopExisting">Boolean if loops are in the current field.</param>
        /// <param name="IsParallelismExisting">Boolean if parallelism are in the current field.</param>
        /// <param name="IsSelectedEventExisting">Boolean if the selected events are in the current field.</param>
        /// <param name="CountExistingTransition">Add 1, if the transition was found on the net.</param>
        /// <param name="CurrentField">The current field contains a net.</param>
        /// <param name="numberOfEvents"></param>
        /// <param name="IsProcessmodelWithNumberOfEventsExisting"></param>
        /// <author>Christopher Licht, Bernhard Bruns</author>
        private static void CallForEverySelectedOptionTheNecessaryMethodsAND(HashSet<String> _listOfSelectedOptions, HashSet<String> _listOfEvents, HashSet<Field> _ListOfFieldsWithAND, ref Boolean IsLoopExisting, ref Boolean IsParallelismExisting, ref Boolean IsSelectedEventExisting, ref int CountExistingTransition, Field CurrentField, int numberOfEvents, ref Boolean IsProcessmodelWithNumberOfEventsExisting)
        {
            if (CurrentField.ProcessModel != null)
            {
                IsSelectedEventExisting = false;
                CountExistingTransition = 0;

                if (_listOfSelectedOptions.Contains("Loop"))
                    IsLoopExisting = SearchLoops(CurrentField);
                if (_listOfSelectedOptions.Contains("Parallelism"))
                    IsParallelismExisting = SearchParallelism(CurrentField);
                if (_listOfSelectedOptions.Contains("Events"))
                {
                    PetriNet currentPetriNet = (PetriNet)CurrentField.ProcessModel;

                    foreach (Transition currentTransition in currentPetriNet.Transitions)
                    {
                        Boolean isSelectedTransitionExisting = false;
                        isSelectedTransitionExisting = SearchSelectedEvent(currentTransition, _listOfEvents);
                        if (isSelectedTransitionExisting)
                            CountExistingTransition++;
                    }

                    if (CountExistingTransition == _listOfEvents.Count)
                    {
                        CountExistingTransition = 0;
                        IsSelectedEventExisting = true;
                    }
                }
                if (_listOfSelectedOptions.Contains("Min. Number of Events"))
                {
                    IsProcessmodelWithNumberOfEventsExisting = SearchMinimumNumberOfEvents(CurrentField, numberOfEvents);
                }
                AddCurrentFieldToANDListIfConditionsFulfilled(_listOfSelectedOptions, _ListOfFieldsWithAND, IsLoopExisting, IsParallelismExisting, IsSelectedEventExisting, IsProcessmodelWithNumberOfEventsExisting, CurrentField);
            }
        }

        /// <summary>
        /// This method adds the current field to the AND-List, if one of the condition is fulfilled.
        /// </summary>
        /// <param name="_listOfSelectedOptions">List with selected options.</param>
        /// <param name="_ListOfFieldsWithAND">Contains the fields which fulfill the conditions.</param>
        /// <param name="IsLoopExisting">Boolean if loops are in the current field.</param>
        /// <param name="IsParallelismExisting">Boolean if parallelism are in the current field.</param>
        /// <param name="IsSelectedEventExisting">Boolean if the selected events are in the current field.</param>
        /// <param name="IsProcessmodelWithNumberOfEventsExisting"></param>
        /// <param name="CurrentField">The current field contains a net.</param>
        /// <author>Christopher Licht, Bernhard Bruns</author>
        private static void AddCurrentFieldToANDListIfConditionsFulfilled(HashSet<String> _listOfSelectedOptions, HashSet<Field> _ListOfFieldsWithAND, Boolean IsLoopExisting, Boolean IsParallelismExisting, Boolean IsSelectedEventExisting, Boolean IsProcessmodelWithNumberOfEventsExisting, Field CurrentField)
        {
            if ((_listOfSelectedOptions.Contains("Loop") && IsLoopExisting) && (_listOfSelectedOptions.Contains("Events") && IsSelectedEventExisting) && (IsParallelismExisting == false))
                _ListOfFieldsWithAND.Add(CurrentField);
            else if ((_listOfSelectedOptions.Contains("Loop") && IsLoopExisting) && (_listOfSelectedOptions.Contains("Parallelism") && IsParallelismExisting) && IsSelectedEventExisting == false)
                _ListOfFieldsWithAND.Add(CurrentField);
            else if ((_listOfSelectedOptions.Contains("Parallelism") && IsParallelismExisting) && (_listOfSelectedOptions.Contains("Events") && IsSelectedEventExisting) && IsLoopExisting == false)
                _ListOfFieldsWithAND.Add(CurrentField);
            else if ((_listOfSelectedOptions.Contains("Parallelism") && IsParallelismExisting) && (_listOfSelectedOptions.Contains("Events") && IsSelectedEventExisting) && (_listOfSelectedOptions.Contains("Loop") && IsLoopExisting))
                _ListOfFieldsWithAND.Add(CurrentField);
            else if (_listOfSelectedOptions.Contains("Min. Number of Events") && IsProcessmodelWithNumberOfEventsExisting)
                _ListOfFieldsWithAND.Add(CurrentField);

        }

        /// <summary>
        /// To start the or-handling process.
        /// </summary>
        /// <param name="_listWithFields">List with all fields of the matrix-selection.</param>
        /// <param name="_listOfSelectedOptions">List with selected options.</param>
        /// <param name="_listOfSelectedEvents">List with selected events from the listbox.</param>
        /// <param name="numberOfEvents"></param>
        /// <returns>Returns a hash set with fields, which fulfilled the necessary conditions.</returns>
        /// <author>Christopher Licht</author>
        private static HashSet<Field> StartORHandling(HashSet<Field> _listWithFields, HashSet<String> _listOfSelectedOptions, HashSet<String> _listOfSelectedEvents, int numberOfEvents)
        {
            HashSet<Field> _ListOfFieldsWithOR = new HashSet<Field>();
            Boolean isLoopExisting = false;
            Boolean isParallelismExisting = false;
            Boolean isSelectedEventExisting = false;
            Boolean isProcessmodelWithNumberOfEventsExisting = false;

            foreach (var currentElementOfSelectedOption in _listOfSelectedOptions)
                CallForEverySelectedOptionTheNecessaryMethodsOR(_listWithFields, _listOfSelectedEvents, _ListOfFieldsWithOR, ref isLoopExisting, ref isParallelismExisting, ref isSelectedEventExisting, ref isProcessmodelWithNumberOfEventsExisting, currentElementOfSelectedOption, numberOfEvents);

            return _ListOfFieldsWithOR;
        }

        /// <summary>
        /// This method calls the methods which depends on the selected options from the listbox.
        /// </summary>
        /// <param name="_listWithFields">List with all fields of the matrix-selection.</param>
        /// <param name="_listOfSelectedEvents">List with selected events from the listbox.</param>
        /// <param name="_ListOfFieldsWithOR">List with fields which fulfill the conditions.</param>
        /// <param name="IsLoopExisting">Boolean if loops are in the current field.</param>
        /// <param name="IsParallelismExisting">Boolean if parallelism are in the current field.</param>
        /// <param name="IsSelectedEventExisting">Boolean if the selected events are in the current field.</param>
        /// <param name="IsProcessmodelWithNumberOfEventsExisting"></param>
        /// <param name="CurrentElementOfSelectedOption">The current element from the listbox.</param>
        /// <param name="numberOfEvents"></param>
        /// <author>Christopher Licht, Bernhard Bruns</author>
        private static void CallForEverySelectedOptionTheNecessaryMethodsOR(HashSet<Field> _listWithFields, HashSet<String> _listOfSelectedEvents, HashSet<Field> _ListOfFieldsWithOR, ref Boolean IsLoopExisting, ref Boolean IsParallelismExisting, ref Boolean IsSelectedEventExisting, ref Boolean IsProcessmodelWithNumberOfEventsExisting, String CurrentElementOfSelectedOption, int numberOfEvents)
        {
            switch (CurrentElementOfSelectedOption)
            {
                case "Loop": foreach (var currentField in _listWithFields)
                        IsLoopExisting = SearchLoops(currentField) ? _ListOfFieldsWithOR.Add(currentField) : false;
                    break;
                case "Parallelism": foreach (var CurrentField in _listWithFields)
                        IsParallelismExisting = SearchParallelism(CurrentField) ? _ListOfFieldsWithOR.Add(CurrentField) : false;
                    break;
                case "Events":
                    foreach (var CurrentField in _listWithFields)
                        IsSelectedEventExisting = SearchAndImproveSelectedEventsOR(_listOfSelectedEvents, _ListOfFieldsWithOR, IsSelectedEventExisting, CurrentField);
                    break;
                case "Min. Number of Events": foreach (var CurrentField in _listWithFields)
                        IsProcessmodelWithNumberOfEventsExisting = SearchMinimumNumberOfEvents(CurrentField, numberOfEvents) ? _ListOfFieldsWithOR.Add(CurrentField) : false;
                    break;


                default:
                    throw new NotImplementedException("Could not be found.");
            }
        }

        /// <summary>
        /// Calls for every field the method SearchSelectedEvent and returns true or false.
        /// </summary>
        /// <param name="_listOfSelectedEvents">List with selected events from the list box.</param>
        /// <param name="_ListOfFieldsWithOR">List with fields which fulfill the conditions.</param>
        /// <param name="IsSelectedEventExisting">Boolean if the selected events are in the current field.</param>
        /// <param name="CurrentField">This field contains a net.</param>
        /// <returns>Returns true, if the selected event were found in the current field, else false.</returns>
        /// <author>Christopher Licht</author>
        private static bool SearchAndImproveSelectedEventsOR(HashSet<String> _listOfSelectedEvents, HashSet<Field> _ListOfFieldsWithOR, Boolean IsSelectedEventExisting, Field CurrentField)
        {
            if (CurrentField.ProcessModel != null)
            {
                PetriNet currentPetriNet = (PetriNet)CurrentField.ProcessModel;
                int countExistingTransition = 0;

                foreach (var currentTransition in currentPetriNet.Transitions)
                {
                    IsSelectedEventExisting = SearchSelectedEvent(currentTransition, _listOfSelectedEvents);
                    if (IsSelectedEventExisting)
                        countExistingTransition++;
                }

                //If all events were found in the petri net, the current field will be add to the OR-list.
                if (countExistingTransition == _listOfSelectedEvents.Count)
                {
                    _ListOfFieldsWithOR.Add(CurrentField);
                    countExistingTransition = 0;
                }
            }
            return IsSelectedEventExisting;
        }

        /// <summary>
        /// Search for loops in the process models of the fields.
        /// </summary>
        /// <param name="currentField">Field with the current net.</param>
        /// <author>Christopher Licht</author>
        private static bool SearchLoops(Field currentField)
        {
            Boolean isLoopExisitng = false;

            if (currentField.ProcessModel != null)
            {
                PetriNet currentPetriNet = (PetriNet)currentField.ProcessModel;

                if (currentPetriNet.CountLoops() > 0)
                    isLoopExisitng = true;
            }
            return isLoopExisitng;
        }

        /// <summary>
        /// Search for parallelism in the process models of the fields.
        /// </summary>
        /// <param name="currentField">Field with the current net.</param>
        /// <author>Christopher Licht</author>
        private static bool SearchParallelism(Field currentField)
        {
            Boolean andJoin = false;
            Boolean andSplit = false;
            Boolean isParallelismExisting = false;

            if (currentField.ProcessModel != null)
            {
                PetriNet currentPetriNet = (PetriNet)currentField.ProcessModel;

                foreach (var currentTransition in currentPetriNet.Transitions)
                {
                    if (currentTransition.IsANDJoin)
                        andJoin = true;

                    if (currentTransition.IsANDSplit)
                        andSplit = true;
                }

                if (andJoin && andSplit)
                    isParallelismExisting = true;
            }
            return isParallelismExisting;
        }

        /// <summary>
        /// To search the selected events from the list.
        /// </summary>
        /// <param name="currentTransition">For checking the current transition against all selected events.</param>
        /// <param name="_listOfEvents">For checking the current transition against all selected events.</param>
        /// <returns>True if the condition is fulfilled, else false.</returns>
        /// <author>Christopher Licht</author>
        private static bool SearchSelectedEvent(Transition currentTransition, HashSet<String> _listOfEvents)
        {
            Boolean isSelectedEventExisting = false;

            foreach (var currentSelectedEvent in _listOfEvents)
                if (currentTransition.Name.Equals(currentSelectedEvent))
                    return true;

            return isSelectedEventExisting;
        }

        /// <summary>
        /// Returns the maximum number of transitions in the processmodels
        /// </summary>
        /// <returns></returns>
        /// <author>Bernhard Bruns</author>
        public static int GetMaximumNumberOfUsedEvents()
        {
            int maximumNumber = 0;

            foreach (Field f in MainWindow.MatrixSelection.MatrixFields)
            {
                PetriNet p = (PetriNet)f.ProcessModel;

                if (p != null && p.Transitions.Count > maximumNumber)
                    maximumNumber = p.Transitions.Count;
            }

            return maximumNumber;
        }


        /// <summary>
        /// Search for processmodels with a minimum number of events.
        /// </summary>
        /// <param name="currentField">Field with the current net.</param>
        /// <param name="numberOfEvents"></param>
        /// <author>Bernhard Bruns</author>
        private static bool SearchMinimumNumberOfEvents(Field currentField, int numberOfEvents)
        {
            Boolean isMinimumNumberOfEvents = false;

            if (currentField.ProcessModel != null)
            {
                PetriNet currentPetriNet = (PetriNet)currentField.ProcessModel;

                if (currentPetriNet.Transitions.Count >= numberOfEvents)
                    isMinimumNumberOfEvents = true;
            }
            return isMinimumNumberOfEvents;
        }
    }
}
