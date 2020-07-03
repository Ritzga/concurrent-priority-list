using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Transactions;
using World.Collections;
using World.Structure;
using World.Tasks;

namespace World.Threads
{
    /// <summary>
    /// Thread-class to work on tasks
    /// </summary>
    public class WorkerThread
    {
        public WorkerThread(Vector2 worldPosition, PriorityList priorityList)
        {
            this.worldPosition = worldPosition;
            taskPool = priorityList;
        }

        public Vector2 worldPosition;

        private Random rand = new Random();
        private Duty currentDuty = null;
        //pointer to task list
        private PriorityList taskPool;
        // sum of priority of duties
        private int dutiesPriorities = 0;
        //count of all duties where done
        public int DutiesDone { get; private set; } = 0;
        //calculates the everage priority of all duties where done
        public float AverageDutyPritority { get => (float)dutiesPriorities/DutiesDone; }
        //count of all custom duties where done
        public int CustomDutiesDone { get; private set; } = 0;
        //logs on console if true
        public bool LogOnTerminal { get; set; } = false;
        public WorkingState State { get; private set; } = WorkingState.IDLE;
        //custom duties to solve
        public ConcurrentQueue<Duty> CustomDuties { get; set; } = new ConcurrentQueue<Duty>();
        public void StartWork()
        {
            Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Workerthread #" + Thread.CurrentThread.ManagedThreadId + " started!");
            Work();
        }

        public void Work()
        {
            try
            {
                while (true)
                {
                    if(currentDuty != null)
                    {
                        State = WorkingState.FOUND_TASK;
                        if (LogOnTerminal)
                            Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Workerthread #" + Thread.CurrentThread.ManagedThreadId + 
                                " task found! tasktype:" + currentDuty.dutyType +
                                " diff:" + currentDuty.difficult +
                                " location:" + currentDuty.worldPosition);
                        if (worldPosition != currentDuty.worldPosition)
                        {
                            WalkToLocation();
                        }
                        WorkOnTask();
                    }
                    else
                    {
                        if (LogOnTerminal)
                            Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Workerthread #" + Thread.CurrentThread.ManagedThreadId + " searches task!");
                        //If an entry exists, it will processed first
                        if (CustomDuties.TryDequeue(out var duty))
                        {
                            currentDuty = duty;
                        }
                        else
                        {
                            currentDuty = taskPool.GetTask((float)rand.NextDouble() * 100f);
                        }
                    }
                    State = WorkingState.IDLE;
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Workerthread #" + Thread.CurrentThread.ManagedThreadId + " stopped: "+ e.Message);
                if (currentDuty != null)
                {
                    taskPool.Add(currentDuty);
                    Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Workerthread #" + Thread.CurrentThread.ManagedThreadId + " gave task back to list!");
                }
            }

        }

        private void WalkToLocation()
        {
            State = WorkingState.WALK_TO_TASK;
            if (LogOnTerminal)
                Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Workerthread #" + Thread.CurrentThread.ManagedThreadId + " walks to task!");
             //"walk" to the location
            Thread.Sleep(Graph.ManhattanDistance(worldPosition, currentDuty.worldPosition) *100);
            worldPosition = currentDuty.worldPosition;
            if (LogOnTerminal)
                Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Workerthread #" + Thread.CurrentThread.ManagedThreadId + " walked to work!");
        }

        private void WorkOnTask()
        {
            
            try
            {
                using TransactionScope scope = new TransactionScope();
                if (LogOnTerminal)
                    Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Workerthread #" + Thread.CurrentThread.ManagedThreadId + " beginning with task!");
                State = WorkingState.WORKING;
                //"work" on the task
                Thread.Sleep(currentDuty.difficult);
                //check if the duty is custom or not
                dutiesPriorities += currentDuty.Custom ? 0 : currentDuty.Priority;
                DutiesDone += currentDuty.Custom ? 0: 1;
                CustomDutiesDone += currentDuty.Custom ? 1 : 0;
                currentDuty = null;
                scope.Complete();
                if (LogOnTerminal)
                    Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Workerthread #" + Thread.CurrentThread.ManagedThreadId + " finished task!");
            }
            catch (TransactionAbortedException e)
            {
                Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Thread #" + Thread.CurrentThread.ManagedThreadId + " transaction error: " + e.Message);
            }
        }

        public bool GetDuty(out Duty duty)
        {
            duty = currentDuty;
            return duty != null;
        }

        public enum WorkingState
        {
            IDLE,
            FOUND_TASK,
            WALK_TO_TASK,
            WORKING
        }
    }
}