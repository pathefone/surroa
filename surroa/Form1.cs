using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace surroa
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            //READS
        }

        private void label10_Click(object sender, EventArgs e)
        {
            //WRITES
        }

        private void label11_Click(object sender, EventArgs e)
        {
            //SENT
        }

        private void label14_Click(object sender, EventArgs e)
        {
            //RECEIVED
        }

        public class PerformanceParameters
        {
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


        private int AveragePerformance(PerformanceCounter performanceCounter)
        {
            int time = 15;
            int[] average = new int[15]; //maybe 14
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


        private PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        private PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        private PerformanceCounter diskReadsPerformanceCounter = new PerformanceCounter("PhysicalDisk","Disk Reads/sec","_Total");
        private PerformanceCounter diskWritesPerformanceCounter = new PerformanceCounter("PhysicalDisk","Disk Writes/sec","_Total");
        PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");


        private void button1_Click(object sender, EventArgs e)
        {


            //CPU

            label5.Text = "Calculating..";
            Thread.Sleep(300);
            Task<int> cpu_task = new Task<int>(() =>
            {
                return AveragePerformance(cpuCounter);
            });
            cpu_task.Start();

            
            //RAM

            label6.Text = "Measuring RAM usage...";
            Thread.Sleep(300);
            Task<int> ram_task = new Task<int>(() =>
            {
                return AveragePerformance(ramCounter);
            });
            ram_task.Start();


            //I/O

            label7.Text = "Calculating..";
            Thread.Sleep(300);
            Task<int> diskRead_task = new Task<int>(() =>
            {
                return AveragePerformance(diskReadsPerformanceCounter);
                
            });
            diskRead_task.Start();

            Task<int> diskWrite_task = new Task<int>(() =>
            {
                return AveragePerformance(diskWritesPerformanceCounter);
            });
            diskWrite_task.Start();


            //NETWORK

            string instance = performanceCounterCategory.GetInstanceNames()[0]; // 1st NIC !
            PerformanceCounter performanceCounterSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            PerformanceCounter performanceCounterReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);

            label11.Text = "Measuring..";
            Refresh();
            Thread.Sleep(300);

            Task<int> netSent_task = new Task<int>(() =>
            {
                return AveragePerformance(performanceCounterSent);
            });
            netSent_task.Start();

            Task<int> netReceive_task = new Task<int>(() =>
            {
                return AveragePerformance(performanceCounterReceived);
            });
            netReceive_task.Start();


            PerformanceParameters performanceParameters = new PerformanceParameters();
            //After threads finish
            performanceParameters.cpu_average = cpu_task.Result;
            performanceParameters.ram_average = ram_task.Result;
            performanceParameters.diskRead_average = diskRead_task.Result;
            performanceParameters.diskWrite_average = diskWrite_task.Result;
            performanceParameters.netSent_average = netSent_task.Result;
            performanceParameters.netReceive_average = netReceive_task.Result;

            label5.Text = performanceParameters.cpu_average + "%";
            label6.Text = performanceParameters.ram_average + "MB";
            label7.Text = performanceParameters.diskRead_average + "/s";
            label10.Text = performanceParameters.diskWrite_average + "/s";
            label11.Text = performanceParameters.netSent_average + "Bytes /s";
            label14.Text = performanceParameters.netReceive_average + "Bytes /s";


            List<PerformanceParameters> _JSONdata = new List<PerformanceParameters>();
            _JSONdata.Add(new PerformanceParameters()
            {
                cpu_average = performanceParameters.cpu_average,
                ram_average = performanceParameters.ram_average,
                diskRead_average = performanceParameters.diskRead_average,
                diskWrite_average = performanceParameters.diskWrite_average,
                netSent_average = performanceParameters.netSent_average,
                netReceive_average = performanceParameters.netReceive_average
            });

            string json = System.Text.Json.JsonSerializer.Serialize(_JSONdata);
            try
            {
                System.IO.Directory.CreateDirectory(@"C:\Users\LimitedEditionBOo\source\surroa_config");
                File.WriteAllText(@"C:\Users\LimitedEditionBOo\source\surroa_config\Config.json", json);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }




        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string fileName = "Config.json";
            string jsonSTRING = File.ReadAllText(@"C:\Users\LimitedEditionBOo\source\surroa_config\" + fileName);
            
            PerformanceParameters performanceParameters = JsonConvert.DeserializeObject<PerformanceParameters>(jsonSTRING.Substring(1, jsonSTRING.Length-2));
         
            label5.Text = performanceParameters.cpu_average + "%";
            label6.Text = performanceParameters.ram_average + "MB";
            label7.Text = performanceParameters.diskRead_average + "/s";
            label10.Text = performanceParameters.diskWrite_average + "/s";
            label11.Text = performanceParameters.netSent_average + "Bytes /s";
            label14.Text = performanceParameters.netReceive_average + "Bytes /s";
            
        }
    }
}
