using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
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

        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;
        private PerformanceCounter diskReadsPerformanceCounter = new PerformanceCounter();
        private PerformanceCounter diskWritesPerformanceCounter = new PerformanceCounter();
        private PerformanceCounter diskTransfersPerformanceCounter = new PerformanceCounter();

        private void button1_Click(object sender, EventArgs e)
        {
            //CPU
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            label5.Text = "Calculating..";
            Refresh();
            Thread.Sleep(2000);
            cpuCounter.NextValue();
            Thread.Sleep(1000);


            int time = 15;
            int[] average = new int[15];

            while (time != 0)
            {
                int cpu_value = (int)cpuCounter.NextValue();
                average[time - 1] = cpu_value;
                time--;
                Thread.Sleep(1000);

            }
            int average_cpu = (int)average.Average();

            label5.Text = average_cpu + "%";
            time = 15;

            //RAM
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            label6.Text = "Measuring RAM usage...";
            Refresh();
            Thread.Sleep(1000);
            ramCounter.NextValue();
            Thread.Sleep(1000);

            Array.Clear(average, 0, average.Length);

            while (time != 0)
            {
                int ram_value = (int)ramCounter.NextValue();
                average[time - 1] = ram_value;
                time--;
                Thread.Sleep(1000);
            }
            int average_ram = (int)average.Average();
            label6.Text = average_ram + "MB";
            time = 15;

            //I/O

            //fix to make average
            //enable multithreading

            this.diskReadsPerformanceCounter.CategoryName = "PhysicalDisk";
            this.diskReadsPerformanceCounter.CounterName = "Disk Reads/sec";
            this.diskReadsPerformanceCounter.InstanceName = "_Total";

            this.diskWritesPerformanceCounter.CategoryName = "PhysicalDisk";
            this.diskWritesPerformanceCounter.CounterName = "Disk Writes/sec";
            this.diskWritesPerformanceCounter.InstanceName = "_Total";


            label7.Text = "Calculating..";
            Refresh();
            Thread.Sleep(300);
            this.diskReadsPerformanceCounter.NextValue();
            this.diskWritesPerformanceCounter.NextValue();
            Thread.Sleep(300);

            Array.Clear(average, 0, average.Length);

            while (time != 0)
            {
                int disk_value = (int)this.diskReadsPerformanceCounter.NextValue();
                average[time - 1] = disk_value;
                time--;
                Thread.Sleep(1000);
            }
            int average_disk_read = (int)average.Average();

            string currentDiskReads = "Disk reads /s : " + this.diskReadsPerformanceCounter.NextValue().ToString() + Environment.NewLine;
            string currentDiskWrites = "Disk writes /s : " + this.diskWritesPerformanceCounter.NextValue().ToString() + Environment.NewLine;


            label7.Text = "Disk reads /s: " + average_disk_read;
            label10.Text = currentDiskWrites;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
    }
}
