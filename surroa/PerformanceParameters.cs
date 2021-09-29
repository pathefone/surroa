using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace surroa
{


    class PerformanceParameters
    {
        public int AveragePerformance(PerformanceCounter performanceCounter)
        {
            int time = 15;
            int[] average = new int[15];
            performanceCounter.NextValue();
            Thread.Sleep(300);


            while (time != 0)
            {
                int _value = (int)performanceCounter.NextValue();
                average[time - 1] = _value;
                time--;
                Thread.Sleep(1000);
            }

            return (int)average.Average();

        }

        private int AveragePerformanceTask(PerformanceCounter performanceCounter)
        {
            Task<int> _task = new Task<int>(() =>
            {
                return AveragePerformance(performanceCounter);
            });
            _task.Start();

            return _task.Result;
        }



        [JsonProperty("cpu_average")]
            public int cpu_average { get; set; }

            [JsonProperty("ram_average")]
            public int ram_average { get; set; }

            [JsonProperty("diskRead_average")]
            public int diskRead_average { get; set; }

            [JsonProperty("diskWrite_average")]
            public int diskWrite_average { get; set; }

            [JsonProperty("netSent_average")]
            public int netSent_average { get; set; }

            [JsonProperty("netReceive_average")]
            public int netReceive_average { get; set; }


        
    }
}
