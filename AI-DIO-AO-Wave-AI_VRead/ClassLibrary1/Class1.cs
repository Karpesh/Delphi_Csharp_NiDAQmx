using System;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NationalInstruments.DAQmx;
using NationalInstruments;
using System.Runtime.CompilerServices;
using System.Diagnostics;



    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IMyNiDAQmxEvent
    {
        [DispId(1)]
        void AI_DataReceived(double [,] ScaledData, int[] sampleCounts, int chanCount);
    }
    
    public interface IMyNiDAQmx
    {
        int Dig_Out_Line(short dev, short port, short linenum, short state);
        int Dig_In_Line(short dev, short port, short linenum, out short state);
//      void DIG_Line_Config(int slot, int port, int linenum, int direction);
        
 //     void AI_Configure();
        int AI_Start();
        void AI_Stop();
   //   void AI_Reset();
        double AI_sampleRate { get; set; }
        double AI_rangeMinimum { get; set; }
        double AI_rangeMaximum { get; set; }
        int AI_samplesPerChannel { get; set; }
        string AI_ChanelString { get; set; }

//      void AO_Configure(int slot, int chan, int outputPolarity, int IntOrExtRef, double refVoltage, int updateMode);
        int AO_VWrite(int slot, int chan, double voltage);

        int Device { get; set; }


        int Wave_Start(string ChanelNames, double MinValue, double MaxValue, int PointsPerPeriod, double Frec, double[,] Data);
        void Wave_Stop();

        int AI_VRead(short dev, short chan, short gain, out double voltage);

  //    int Nothing { get; set; }
               
    }

    // Restrict clients to using only implemented interfaces.


    public delegate void AI_DataReceivedDelegate(double[,] ScaledData, int[] sampleCounts, int chanCount);

    [
    ComSourceInterfaces("IMyNiDAQmxEvent"),
    ClassInterface(ClassInterfaceType.None)
    ]
    public class MyNiDAQmx : IMyNiDAQmx 
    {



        private int device = 1;   // Dev1=device 1
        // Для аналогового ввода
        // for _continious_ analog input
        private double sampleRate = 10000;
        private double rangeMinimum = -10;
        private double rangeMaximum = 10;
        private int samplesPerChannel = 1000;
        private string ChanelString = "0,1";

        private Task AI_Task;
        private Task runningTask;

        private Task WaveTask;

       

      


        private AnalogWaveform<double>[] data;
       


        private AnalogMultiChannelReader analogInReader;
        private AsyncCallback analogCallback;

        public event AI_DataReceivedDelegate AI_DataReceived;

       


        // The class must have a parameterless constructor for COM interoperability
        public MyNiDAQmx() { }

        // Implement MyInterface methods

 //       public void DIG_Line_Config(int slot, int port, int linenum, int direction) { }

        public int Dig_Out_Line(short dev, short port, short linenum, short state)
        {
            Task digitalWriteTask = new Task();

            try
            {
                
                StringBuilder chanstr = new StringBuilder("Dev");
                chanstr.Append(dev);
                chanstr.Append("/Port");
                chanstr.Append(port);
                chanstr.Append("/line");
                chanstr.Append(linenum);

                digitalWriteTask.DOChannels.CreateChannel(chanstr.ToString(), "", ChannelLineGrouping.OneChannelForAllLines);
                bool bstate = false;
                if (state!=0) {bstate=true;}
                bool[] statearr = { bstate };
                DigitalSingleChannelWriter writer = new DigitalSingleChannelWriter(digitalWriteTask.Stream);
                writer.WriteSingleSampleMultiLine(true, statearr);
                return (0);
            }
            catch (DaqException ex)
            {
                MessageBox.Show(ex.Message);
                return (ex.Error);   
            }
            finally
            {
                digitalWriteTask.Dispose();
                
            }

        }

        public int Dig_In_Line(short dev, short port, short linenum, out short state)
        {
            
            Task digitalReadTask = new Task(); 
     
            state = 1;
            try
            {
                
                StringBuilder chanstr = new StringBuilder("Dev");
                chanstr.Append(dev);
                chanstr.Append("/Port");
                chanstr.Append(port);
                chanstr.Append("/line");
                chanstr.Append(linenum);

                digitalReadTask.DIChannels.CreateChannel(chanstr.ToString(), "", ChannelLineGrouping.OneChannelForAllLines);
                DigitalSingleChannelReader reader = new DigitalSingleChannelReader(digitalReadTask.Stream);
                bool bstate = reader.ReadSingleSampleMultiLine()[0];
                if (!bstate) { state = 0; };
                return (0);
            }
            catch (DaqException ex)
            {
                if (ex.Error == -200587) {Dig_In_Line(dev, port, linenum, out state);}
              //  MessageBox.Show(ex.Message);
                return (ex.Error);
            }
            finally
            {
                digitalReadTask.Dispose();
                
            }

        }

       

//        public void AI_Configure(){}

        public int AI_Start()
        {
            if (runningTask != null) // сначала останавливаем работающее сканирование
            {
                //Dispose of the task
                runningTask = null;
                AI_Task.Dispose();
            }

                try
                {
                    if (runningTask == null)
                    {
                    AI_Task = new Task();

                    // Create a channel
                    AI_Task.AIChannels.CreateVoltageChannel(AI_ChanelStringToChanelList(ChanelString), "",
                        (AITerminalConfiguration)(-1), AI_rangeMinimum, AI_rangeMaximum, AIVoltageUnits.Volts);

                    // Configure timing specs    
                    AI_Task.Timing.ConfigureSampleClock("", AI_sampleRate, SampleClockActiveEdge.Rising,
                        SampleQuantityMode.ContinuousSamples, AI_samplesPerChannel);

                    // Verify the task
                    AI_Task.Control(TaskAction.Verify);
                    runningTask = AI_Task;
                    analogInReader = new AnalogMultiChannelReader(AI_Task.Stream);
                    analogCallback = new AsyncCallback(AnalogInCallback);
                    analogInReader.SynchronizeCallbacks = true;
                    analogInReader.BeginReadWaveform(samplesPerChannel, analogCallback, AI_Task);
                    }
                    return (0);
                }

                catch (DaqException exception)
                {
                    MessageBox.Show(exception.Message);
                    runningTask = null;
                    AI_Task.Dispose();
                    return (exception.Error);
                }
            }
       
   

        public void AI_Stop()
        {
            if (runningTask != null)
            {
                //Dispose of the task
                runningTask = null;
                AI_Task.Dispose();
            }
        }


        public double AI_sampleRate
        {
            get { return sampleRate; }
            set { sampleRate = value; }
        }

        public double AI_rangeMinimum
        {
            get { return rangeMinimum; }
            set { rangeMinimum = value; }
        }

        public double AI_rangeMaximum
        {
            get { return rangeMaximum; }
            set { rangeMaximum = value; }
        }

        public int AI_samplesPerChannel
        {
            get { return samplesPerChannel; }
            set { samplesPerChannel = value; }
        }

        public int Device
        {
            get { return device; }
            set { device = value; }
        }

   /*     public int Nothing
        {
            get { return 0; }
            set { }
        }
*/
        public string AI_ChanelString
        {
            get { return ChanelString; }
            set { ChanelString = value; }
        }

        private void AnalogInCallback(IAsyncResult ar)
        {
            try
            {
                if ((runningTask == ar.AsyncState)&&(runningTask!=null))
                {
                    //Read the available data from the channels
                    data = analogInReader.EndReadWaveform(ar);
                    
                     //Формируем массив для передачи
                    double[,] TempArray = new double[runningTask.AIChannels.Count, samplesPerChannel];
                    int [] SamplesArray = new int [runningTask.AIChannels.Count];
                    for (int i =0; i<runningTask.AIChannels.Count; i++)
                     {
                         SamplesArray[i] = data[i].SampleCount;
                       for (int j = 0; j < samplesPerChannel; j++)
                           TempArray[i,j]=data[i].Samples[j].Value;
                     }
                    //Передаем массив, вызываем событие

                    AI_DataReceived(TempArray, SamplesArray, runningTask.AIChannels.Count);

                    analogInReader.BeginMemoryOptimizedReadWaveform(samplesPerChannel, analogCallback, AI_Task, data);

                }
            }
            catch (DaqException exception)
            {
                //Display Errors
               // MessageBox.Show(exception.Message+"тутут");
                runningTask = null;
                AI_Task.Dispose();
            }
        }

        private string AI_ChanelStringToChanelList(string ChanStr)
        {
            string Result = "";

            int i = 0;
            foreach (char s in ChanStr)
            {
                if ((ChanStr[i] != ',') && (ChanStr[i] != ' '))
                { Result = Result + " Dev" + device + "/ai" + ChanStr[i]; }
                else
                { Result = Result + ChanStr[i]; }

                i = i + 1;
            }
            return Result;
        }

 //       public void AO_Configure(int slot, int chan, int outputPolarity, int IntOrExtRef, double refVoltage, int updateMode) {}

       
        
        public int AO_VWrite(int slot, int chan, double voltage)
        {
            try
            {
               
                using (Task AO_Task = new Task())
                {
                    AO_Task.AOChannels.CreateVoltageChannel("Dev"+slot+"/ao"+chan, "aoChannel",
                        -10, 10, AOVoltageUnits.Volts);
                    AnalogSingleChannelWriter writer = new AnalogSingleChannelWriter(AO_Task.Stream);
                    writer.WriteSingleSample(true, voltage);
                }
                return (0);
            }
            catch (DaqException ex)
            {
                if (ex.Error != -200561)
                {
                    MessageBox.Show(ex.Message);
                    return (ex.Error);
                }
                return (0);
            }
        }



        public int Wave_Start(string ChanelNames, double MinValue, double MaxValue, int PointsPerPeriod, double Freq, double[,] Data)
        {
            try
            {
                // create the task and channel
                WaveTask = new Task();
                WaveTask.AOChannels.CreateVoltageChannel(ChanelNames, "", MinValue, MaxValue, AOVoltageUnits.Volts);

                // verify the task before doing the waveform calculations
                WaveTask.Control(TaskAction.Verify);


                // configure the sample clock with the calculated rate
                WaveTask.Timing.ConfigureSampleClock("", Freq * PointsPerPeriod,
                    SampleClockActiveEdge.Rising,
                    SampleQuantityMode.ContinuousSamples, PointsPerPeriod);


                AnalogMultiChannelWriter writer =
                    new AnalogMultiChannelWriter(WaveTask.Stream);

                //write data to buffer
                writer.WriteMultiSample(false, Data);

                //start writing out data
                WaveTask.Start();

                                
                return (0);
            }
            catch (DaqException err)
            {
                //statusCheckTimer.Enabled = false;
                MessageBox.Show(err.Message);
                WaveTask.Dispose();
                return (err.Error);
            }
           
        }

        public void Wave_Stop()
        {
            
            if (WaveTask != null)
            {
                try
                {
                    WaveTask.Stop();
                   
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                   
                }
                WaveTask.Dispose();
                WaveTask = null;
                
            }    
        }

        public int AI_VRead(short dev, short chan, short gain, out double voltage)
        {
            try
            {


                //Create a new task
                Task AI_VReadTask = new Task();

               
                    StringBuilder chanstr = new StringBuilder("Dev");
                    chanstr.Append(dev);
                    chanstr.Append("/ai");
                    chanstr.Append(chan);


                    //Create a virtual channel
                    AI_VReadTask.AIChannels.CreateVoltageChannel(chanstr.ToString(), "",
                            (AITerminalConfiguration)(-1), -10,
                            10, AIVoltageUnits.Volts);
                    //set gain
                    AI_VReadTask.AIChannels[0].Gain = gain;


                    AnalogMultiChannelReader reader = new AnalogMultiChannelReader(AI_VReadTask.Stream);

                    //Verify the Task
                    AI_VReadTask.Control(TaskAction.Verify);

                    

                    double[] data = reader.ReadSingleSample();     
                    voltage = data[0];
                    return (0);

                }
                catch (DaqException ex)
                {
                    MessageBox.Show(ex.Message);
                    voltage = 0;
                    return (ex.Error);  
                }
                finally
                {
                   
                }
            }

        

    }
