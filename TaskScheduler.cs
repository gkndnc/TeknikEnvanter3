using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Envanter3
{
    public class TaskScheduler
    {
        private static TaskScheduler _instance;
        private List<Timer> timers = new List<Timer>();

        private TaskScheduler() { }

        public static TaskScheduler Instance => _instance ?? (_instance = new TaskScheduler());

        public void ScheduleTask(DateTime time, Action task)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeToGo = time - now;

            var timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromTicks(-1));

            timers.Add(timer);
        }
    }
}
