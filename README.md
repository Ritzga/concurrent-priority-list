# Description

Its a concurrent priotity list with 10 priotities and random tasks.

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

## Documentation
⋅⋅*[ConcurrentQueue](https://referencesource.microsoft.com/#mscorlib/System/Collections/Concurrent/ConcurrentQueue.cs)
⋅⋅*[ConcurrentDictionary](https://referencesource.microsoft.com/#mscorlib/system/Collections/Concurrent/ConcurrentDictionary.cs)