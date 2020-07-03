# Description

Its a concurrent priotity list with 10 priotities-ranks and random tasks.

## Commands

* aw - Add new Worker 
* lw - List all active worker 
* kill {WorkerId} - Exit a worker thread with the id 
* killAll - Exit all worker threads 
* assT {WorkerId} {Taskdifficult} {X} {Y} - Assigne a task to a worker 
* addT {TaskType} {Taskdifficult} {Priority} {X} {Y} - Adds a task to the list 
* lt - lists all task types 
* sp {TaskType} {number from 0-9} - sets the priority of a task type 
* ww {WorkerId} - Activate the logging of a worker 
* uww {WorkerId} - Deactivate the logging of a worker 
* waw - Activate the logging of all worker 
* uaw - Deactivate the logging of all worker 
* ctl - Clear the whole task list 
* exit - Exit the program 

## How it works

![alt text](https://raw.githubusercontent.com/Ritzga/concurrent-priority-list/master/doc/work.jpg "How it works")

## Some values

### With a 4 core cpu and 32 threads

![picture of distribution of duties on a 4 core cpu](https://raw.githubusercontent.com/Ritzga/concurrent-priority-list/master/doc/duties_done_4.jpg "4 core CPU")

### With a 12 core cpu and 32 threads

![picture of distribution of duties on a 12 core cpu](https://raw.githubusercontent.com/Ritzga/concurrent-priority-list/master/doc/duties_done_12.jpg "12 core CPU")

### Posibilities of every priority

![picture of posibilities of every priority](https://raw.githubusercontent.com/Ritzga/concurrent-priority-list/master/doc/probabilities.jpg "Posibilities of every priority")

### Posibilities of every priority in test

![picture of posibilities of every priority in test](https://raw.githubusercontent.com/Ritzga/concurrent-priority-list/master/doc/probability_duties.jpg "Test with more then 60k duties")

## Documentation
* [ConcurrentQueue](https://referencesource.microsoft.com/#mscorlib/System/Collections/Concurrent/ConcurrentQueue.cs)
* [ConcurrentDictionary](https://referencesource.microsoft.com/#mscorlib/system/Collections/Concurrent/ConcurrentDictionary.cs)
