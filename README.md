Process Cube Explorer
=====================

Process Mining extracts implicit knowledge from event logs and generates process models to 
visualize the underlying process. The *Process Cube Explorer* can connect to various databases
and load *multidimensional event logs* in which the characteristics of the process-instance 
are modelled as dimensions in a datawarehouse-like schema. See the documentation (in German) 
or example-database-script on how this works. 
You can then select dimensions and filters to create subsets of the event log to compare 
characteristics. This implementation also supports event-dimensions.
As *process discovery*-algorithms we offer the *Alpha Miner* (and two extensions), the *Heuristic Miner*
(in an improved twice-as-fast-implementation) and the *Inductive Miner - infrequent*.
The results are petrinets that can be automatically compared, printed, exported, etc.
You also have the chance to run two *conformance checking*-algorithms on them, the
*Comparing Footprints*-Algorithm and a *Token Replay*.

The Process Cube Explorer was created by the Projektgruppe MPM at University of Oldenburg 
from april 2013 to march 2014. 
You can find it at https://github.com/pgmpm/ProcessCubeExplorer

Authors are:
- [Jannik Arndt](http://www.jannikarndt.de)
- Thomas Meents
- Bernd Nottbeck
- Moritz Eversmann
- Andrej Albrecht
- Bernhard Bruns
- Krystian Zielonka
- Christopher Licht
- Naby Moussa Sow
- Roman Bauer
- Markus Holznagel

(by percentage of authorship)

Copyright 2014 Projektgruppe MPM. All Rights Reserved.

Process Cube Explorer is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Process Cube Explorer is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Process Cube Explorer. If not, see <http://www.gnu.org/licenses/>.

This project also includes a slightly modified version of the open-source 
ModernUI-Framework by FirstFloor, hosted at https://mui.codeplex.com under MS-PL.

# Usage

You an either download the compiled version or the whole repository, load it in Visual Studio
and compile it yourself. Notice that some unit-tests will fail if you don't enter credentials
for a test-database (and we didn't want to publish ours). 

### Connecting to a database
The software supports the following databases:
- Oracle
- MySQL
- MS-SQL
- PostgreSQL
- SQLite
Please note that you need to install the odac-drivers to use oracle-databases
(http://www.oracle.com/technetwork/topics/dotnet/utilsoft-086879.html)

### Selecting data
The [example-data](https://github.com/pgmpm/ProcessCubeExplorer/blob/master/Running%20Example.sql) is (made up) clinical data, so you want to reduce it (either via 
dimension or filter) to one icd-10. Currently there are entries for T14.20 and T14.21.

### Mining
You can choose one of the currently three mining-algorithms. These correspond (more or less)
the the theoretical algorithms descriped in the [documentation](https://github.com/pgmpm/ProcessCubeExplorer/blob/master/Documentation%20(German).pdf) (in German) or here:
- [Alpha Miner ++](http://wwwis.win.tue.nl/~wvdaalst/publications/p221.pdf)
- [Heuristic Miner](http://www.researchgate.net/profile/A_Weijters/publication/229124308_Process_Mining_with_the_Heuristics_Miner-algorithm/file/9fcfd510d615ef2b04.pdf)
- [Inductive Miner - infrequent](http://fluxicon.com/blog/wp-content/uploads/2013/09/Discovering-Block-Structured-Process-Models.pdf)

### Interpreting the results
After the mining-step you should see a matrix with the results. You can double-click on any 
field to enlarge the process model, get more information or run the comparing-footprint-algorithm
or the token replay.

# Contributing to the project
The Process Cube Explorer was developed as a research-framework and is very easy to extend.
Although the [documentation](https://github.com/pgmpm/ProcessCubeExplorer/blob/master/Documentation%20(German).pdf) is in German, the uml-class-diagrams on pages 33 through 42 will help you.

### Adding a mining-algorithm
Adding a new mining-algorithm is easy: Make yourself familiar with the `field`-class and the `IMiner`-interface.
Your algorithm will need to implement the interface, which means it will be initialized with a `field`-
object. The field contains everything you need, most important the eventlog. Your `Mine()`-Method should
return a `ProcessModel`-Object. Currently the only implementation is for petrinets, if your algorithm
creates something different make sure to implement that kind of model as well.
To use your algorithm you just have to create a `UserContent`-View and add it to the `ListOfMiners` in the
`MinerFactory`.

### Adding a processmodel-type
Currently all three algorithms return petrinets, so only these are implemented. There is however an abstract class
`ProcessModel` and we tried to use that whenever possible. This means you can just derive a new model-class
from the `ProcessModel`-class and use it in your mining-algorithm. You still need to implement a visualization
though, which should be derived from the abstract class `AbstractProcessModelVisualizer`.

### Adding a diff-algorithm
There also is an interface (`IDifference`) and a factory (`DiffFactory`) for diff-algorithms. Currently 
the snapshot-diff is implemented.

### Adding a consolidation-algorithm
Use the `IConsolidator`-Interface and the `ConsolidatorFactory`.

### Adding a database-type
Most database-types are already supported, you can however derive your own from the abstract `MPMdbConnection`-class.
Keep in mind that you might need to derive another class from the `SQLCreator`, since dialects may differ.
