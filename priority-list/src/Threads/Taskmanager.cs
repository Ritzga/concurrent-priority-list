using System;
using System.Numerics;
using System.Threading;
using System.Transactions;
using World.Collections;
using World.Tasks;
using static World.Tasks.Duty;

namespace World.Threads
{
    /// <summary>
    /// Class to add randomly new tasks to the taskpool
    /// </summary>
    class Taskmanager
    {
        public PriorityList list;

        private readonly Random rand;
        private readonly int maxDuties = 100;

        public Taskmanager(PriorityList list)
        {
            this.list = list;
            rand = new Random();
        }

        public void ManageTasks()
        {
            Console.WriteLine("Taskmanager started!");
            try
            {
                while (true)
                {
                    if (list.Count <= maxDuties)
                    {
                        //cast int to enum 
                        //(DutyType)rand.Next(0, 9)
                        //from 0 to n-1 
                        var task = new Duty(new Vector2(rand.Next(0, 20), rand.Next(0, 20)), (DutyType)rand.Next(0, Enum.GetNames(typeof(DutyType)).Length - 1), rand.Next(100, 8000));
                        while (!list.Add(task))
                        {
                            Thread.Sleep(100);
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine(DateTime.Now.ToString("H:mm:ss") + " - Task manager #" + Thread.GetCurrentProcessorId() + " stoped: " + e.Message);
            }
        }
    }
}
