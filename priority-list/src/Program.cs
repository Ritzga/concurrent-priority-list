using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using World.Collections;
using World.Tasks;
using World.Threads;
using static World.Tasks.Duty;

namespace World
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Paralell priority list startet!");
            var list = new PriorityList();
            var workerList = new Dictionary<Thread, WorkerThread>();

            var initTaskManager = new Taskmanager(list);
            var taskmanager = new Thread(new ThreadStart(initTaskManager.ManageTasks));
            taskmanager.Start();

            for(int i = 0; i < 20; i++)
            {
                var initWorker = new WorkerThread(new Vector2(1, 2), list);
                var worker = new Thread(new ThreadStart(initWorker.StartWork));
                workerList.Add(worker, initWorker);
                worker.Start();
            }
            Thread.Sleep(1000);
            while(true)
            {
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("Choose an option from the following list:");
                Console.WriteLine("* aw - Add new Worker");
                Console.WriteLine("* lw - List all active worker");
                Console.WriteLine("* kill {WorkerId} - Exit a worker thread with the id");
                Console.WriteLine("* killAll - Exit all worker threads");
                Console.WriteLine("* assT {WorkerId} {Taskdifficult} {X} {Y} - Assigne a task to a worker");
                Console.WriteLine("* addT {TaskType} {Taskdifficult} {Priority} {X} {Y} - Adds a task to the list");
                Console.WriteLine("* lt - lists all task types");
                Console.WriteLine("* sp {TaskType} {number from 0-9} - sets the priority of a task type");
                Console.WriteLine("* ww {WorkerId} - Activate the logging of a worker");
                Console.WriteLine("* uww {WorkerId} - Deactivate the logging of a worker");
                Console.WriteLine("* waw - Activate the logging of all worker");
                Console.WriteLine("* uaw - Deactivate the logging of all worker");
                Console.WriteLine("* ctl - Clear the whole task list");
                Console.WriteLine("* exit - Exit the program");
                Console.WriteLine("----------------------------------------------------\n");
                string input = Console.ReadLine();
                string[] com = input.Split(" ");
                Console.WriteLine("");
                // Use a switch statement to do the math.
                switch (com[0])
                {
                    case "aw": //addWorker
                        var initWorker = new WorkerThread(new Vector2(1, 2), list);
                        var worker = new Thread(new ThreadStart(initWorker.Work));
                        workerList.Add(worker, initWorker);
                        worker.Start();
                        break;
                    case "lw": //listWorker
                        //lazy-loading
                        workerList.AsParallel().Select(c => { Console.WriteLine("Workerthread #" + c.Key.ManagedThreadId); return c; }).ToList();
                        break;
                    case "kill": //kill {WorkerId}
                        try
                        {
                            Thread thread = workerList.AsParallel().Single(x => x.Key.ManagedThreadId == Convert.ToInt32(com[1])).Key;
                            thread.Interrupt();
                            thread.Join();
                        }
                        catch
                        {
                            Console.WriteLine("Worker not found!\n");
                        }
                        break;
                    case "killAll": //killAll
                        //lazy-loading
                        workerList.AsParallel().Select(c => { c.Key.Interrupt(); c.Key.Join(); return c; }).ToList();
                        break;
                    case "assT": //assignTask {WorkerId} {2 - TaskType} {3 - Taskdifficult} {4 - X} {5 - Y}
                        try
                        {
                            var customDuty = new Duty(new Vector2(Convert.ToInt32(com[4]), Convert.ToInt32(com[5])), (DutyType)Convert.ToInt32(com[2]), Convert.ToInt32(com[3]), true);
                            WorkerThread thread = workerList.AsParallel().Single(x => x.Key.ManagedThreadId == Convert.ToInt32(com[1])).Value;
                            thread.CustomDuties.Enqueue(customDuty);
                        }
                        catch
                        {
                            Console.WriteLine("Worker not found!\n");
                        }
                        break;
                    case "addT": //addTask {1 - TaskType} {2 - Taskdifficult} {3 - X} {4 - Y}
                        var duty = new Duty(new Vector2(Convert.ToInt32(com[3]), Convert.ToInt32(com[4])), (DutyType)Convert.ToInt32(com[1]), Convert.ToInt32(com[2]));
                        list.Add(duty);
                        duty = null;
                        break;
                    case "lt": //listTaskTypes
                        //lazy-loading
                        list.TypeList.AsParallel().Select(c => { Console.WriteLine("Type #" + c.Key.ToString() + " Priority: " + c.Value); return c; }).ToList();
                        break;
                    case "sp": //setPriority {TaskType} {Int 0-9}
                        list.SetPriorityOfType((DutyType)Convert.ToInt32(com[1]), Convert.ToInt32(com[2]));
                        break;
                    case "ww": //watchWorker {WorkerId}
                        try
                        {
                            WorkerThread workerObject = workerList.AsParallel().Single(x => x.Key.ManagedThreadId == Convert.ToInt32(com[1])).Value;
                            workerObject.LogOnTerminal = true;
                        }
                        catch
                        {
                            Console.WriteLine("Worker not found!\n");
                        }
                        break;
                    case "waw": //
                        //lazy-loading
                        workerList.AsParallel().Select(c => { c.Value.LogOnTerminal = true; return c; }).ToList();
                        break;
                    case "uw": //unwatchWorker {WorkerId}
                        try
                        {
                            WorkerThread workerObject = workerList.AsParallel().Single(x => x.Key.ManagedThreadId == Convert.ToInt32(com[1])).Value;
                            workerObject.LogOnTerminal = false;
                        }
                        catch
                        {
                            Console.WriteLine("Worker not found!\n");
                        }
                        break;
                    case "uaw": //unwatchAllWorker
                        //lazy-loading
                        workerList.AsParallel().Select(c => { c.Value.LogOnTerminal = false; return c; }).ToList();
                        break;
                    case "ctl": //clearTaskList
                        list.Clear();
                        break;
                    case "exit": //exit
                        taskmanager.Interrupt();
                        //get the whole metric data
                        int allDone = 0;
                        float prio = 0;
                        foreach (var entry in workerList)
                        {
                            allDone += entry.Value.DutiesDone;
                            prio += entry.Value.AverageDutyPritority;
                            entry.Key.Interrupt();
                        }
                        //wait a moment to give the thread time to exit
                        foreach (var entry in workerList)
                        {
                            entry.Key.Join();
                        }
                        Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - duties created: " + list.DutyCount);
                        Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - duties done: " + allDone);
                        Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - duties in priority queue: " + list.Count);
                        Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - sum of created duties and duties in queue: " + (list.Count + allDone));
                        Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - average duties from worker done: " + (float)allDone / workerList.Count);
                        Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - average priority of duties: " + (float)prio / workerList.Count);
                        Console.ReadLine();
                        Environment.Exit(Environment.ExitCode);
                        break;
                    default:
                        Console.WriteLine("Command not found!\n");
                        break;
                }
            }
        }
    }
}
