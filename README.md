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
- Jannik Arndt
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

## Connecting to a database
The software supports the following databases:
- Oracle
- MySQL
- MS-SQL
- PostgreSQL
- SQLite
Please note that you need to install the odac-drivers to use oracle-databases
(http://www.oracle.com/technetwork/topics/dotnet/utilsoft-086879.html)

## Selecting data
The example-data is (made up) clinical data, so you want to reduce it (either via 
dimension or filter) to one icd-10. Currently there are entries for T14.20 and T14.21.

## Mining
You can choose one of the currently three mining-algorithms. These correspond (more or less)
the the theoretical algorithms descriped in the documentation (in German) or here:
- Alpha Miner ++ 

