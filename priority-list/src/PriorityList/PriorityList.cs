using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Transactions;
using World.Tasks;

namespace World.Collections
{
    public class PriorityList
    {
        //each priority has their own queue
        private ConcurrentDictionary<int, ConcurrentQueue<Duty>> priorityList;
        //initial locked acess
        private bool _lock = false; 
        //steps for priority
        private readonly List<float> steps;

        //count of duties in queue
        public int Count { get; set; } = 0;
        //count of all added duties
        public int DutyCount { get; set; } = 0;
        //sets the dutytype to priority
        public Dictionary<Duty.DutyType, int> TypeList { get; private set; } 

        public PriorityList()
        {
            //steps for the priority chance
            steps = new List<float> {50, 30, 18, 10.8f, 6.48f, 3.88f, 2.33f, 1.4f, 0.8f};

            //sets the default for duty types to priority
            TypeList = new Dictionary<Duty.DutyType, int>()
            {
                { Duty.DutyType.BUILDING, 0 },
                { Duty.DutyType.CLEANING, 1 },
                { Duty.DutyType.COLLECTING, 2 },
                { Duty.DutyType.COLLECTING_CRYSTALS, 3 },
                { Duty.DutyType.COLLECTING_ORES, 4 },
                { Duty.DutyType.DIGGING, 5 },
                { Duty.DutyType.EXPLORING, 6 },
                { Duty.DutyType.PERFORM_JOBS, 7 },
                { Duty.DutyType.PROTECTING, 8 },
                { Duty.DutyType.REPAIRING, 9 },
            };

            //adds 10 elements to the dict
            priorityList = new ConcurrentDictionary<int, ConcurrentQueue<Duty>>();
            for (int i = 0; i < 10; i++)
            {
                priorityList.TryAdd(i, new ConcurrentQueue<Duty>());
            }
            //code
            _lock = true;
        }

        public bool Clear()
        {
            _lock = false;
            try
            {
                using TransactionScope scope = new TransactionScope();
                foreach (var queue in priorityList)
                {
                    queue.Value.Clear();
                }
                Count = 0;
                scope.Complete();
            }
            catch (TransactionAbortedException e)
            {
                Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Thread #" + Thread.CurrentThread.ManagedThreadId + " transaction error: " + e.Message);
                _lock = true;
                return false;
            }
            _lock = true;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="posibility"></param>
        /// Runtime of O(82) and O(1)
        /// <returns></returns>
        public Duty GetTask(float posibility)
        {
            if (_lock)
            {
                int prio = 0;
                //I'll find a better way to do this
                foreach(float step in steps) //O(9)
                {
                    if(posibility < step)
                    {
                        prio += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                priorityList[prio].TryDequeue(out Duty task);//O(1)
                if (task == null)
                {
                    if (prio < 9)
                    {
                        return GetTask(posibility-steps[prio]); //O(72)
                    }
                }
                else
                {
                    Count--;
                    task.Used = true;
                    return task;
                }
            }
            //no task found
            return null;
        }

        public bool Add(Duty task)
        {
            try
            {
                using TransactionScope scope = new TransactionScope();
                if (_lock)
                {
                    Count++;
                    DutyCount += task.Used ? 0 : 1;
                    priorityList[TypeList[task.dutyType]].Enqueue(task); //O(3)
                    task.Priority = TypeList[task.dutyType];
                    scope.Complete();
                    return true;
                }
            }
            catch (TransactionAbortedException e)
            {
                Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Thread #" + Thread.CurrentThread.ManagedThreadId + " transaction error: " + e.Message);
            }
            return false;
        }

        public bool SetPriorityOfType(Duty.DutyType dutyType, int newPriority)
        {
            try
            {
                using TransactionScope scope = new TransactionScope();
                _lock = false;
                TypeList[dutyType] = newPriority;
                _lock = true;
                scope.Complete();
                return true;
            }
            catch (TransactionAbortedException e)
            {
                Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Thread #" + Thread.CurrentThread.ManagedThreadId + " transaction error: " + e.Message);
                _lock = true;
            }
            return false;
        }
    }
}