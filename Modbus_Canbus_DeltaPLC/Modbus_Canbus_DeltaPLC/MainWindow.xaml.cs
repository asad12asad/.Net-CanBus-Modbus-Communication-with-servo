using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasyModbus;

namespace Modbus_Canbus_DeltaPLC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread CommunicationThread;
        int[] ServoRData = new int[12];
        int[] ServoTData = new int[12];
        public MainWindow()
        {
            InitializeComponent();
            modbusClient = new ModbusClient();
        }
        ModbusClient modbusClient;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            modbusClient.IPAddress = _ip.Text.ToString();
            modbusClient.Port = 502;
            try
            {
                if (modbusClient.Connected)
                {
                    modbusClient.Disconnect();
                    (sender as Button).Content = "Connect";
                    C_status.Content = "DisConnected";
                }
                else
                {
                    modbusClient.Connect();
                }
                
                if (modbusClient.Connected)
                {
                    CommunicationThread = new Thread(CommunicationExecute);
                    CommunicationThread.Start();
                    (sender as Button).Content = "Disconnect";
                    C_status.Content = "Connected";
                }
                
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        delegate void CommunicationCallback();
        private void ComCallback()
        {
            int[] temp = new int[2];
            temp[0] = ServoRData[0];
            temp[1] = 0;
            _Status1.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();
            temp[0] = ServoRData[3];
            temp[1] = 0;
            _Status2.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();
            temp[0] = ServoRData[6];
            temp[1] = 0;
            _Status3.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();

            temp[0] = ServoRData[1];
            temp[1] = ServoRData[2];
            _Position1.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();
            temp[0] = ServoRData[4];
            temp[1] = ServoRData[5];
            _Position2.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();
            temp[0] = ServoRData[7];
            temp[1] = ServoRData[8];
            _Position3.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();



            temp[0] = ServoTData[0];
            temp[1] = 0;
            _CNW1.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();
            temp[0] = ServoTData[3];
            temp[1] = 0;
            _CNW2.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();
            temp[0] = ServoTData[6];
            temp[1] = 0;
            _CNW3.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();

            temp[0] = ServoTData[1];
            temp[1] = ServoTData[2];
            //_Target1.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();
            temp[0] = ServoTData[4];
            temp[1] = ServoTData[5];
           // _Target2.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();
            temp[0] = ServoTData[7];
            temp[1] = ServoTData[8];
           // _Target3.Text = ModbusClient.ConvertRegistersToInt(temp).ToString();
            //reset
           
        }
        public void CommunicationExecute()
        {
            while (modbusClient.Connected)
            {
                try
                {
                    //read servo variables
                    ServoRData = modbusClient.ReadHoldingRegisters(24032, 12);
                    ServoTData = modbusClient.ReadHoldingRegisters(25032, 12);
                    Dispatcher.BeginInvoke(new CommunicationCallback(ComCallback));
                    Thread.Sleep(10);
                }
                catch (Exception EE)
                {
                    MessageBox.Show(EE.ToString());
                }
            }
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (modbusClient.Connected)
            {
                modbusClient.WriteSingleCoil(0,true);// true m0
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (modbusClient.Connected)
            {
                modbusClient.WriteSingleCoil(31, (bool)(sender as CheckBox).IsChecked);// true m0
            }
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            if (modbusClient.Connected)
            {
                modbusClient.WriteSingleCoil(32, (bool)(sender as CheckBox).IsChecked);// true m0
            }
        }

        private void CheckBox_Click_2(object sender, RoutedEventArgs e)
        {
            if (modbusClient.Connected)
            {
                modbusClient.WriteSingleCoil(33, (bool)(sender as CheckBox).IsChecked);// true m0
            }
        }

        private void Execute1_Click(object sender, RoutedEventArgs e)
        {
            if (modbusClient.Connected)
            {
                modbusClient.WriteMultipleRegisters(25033, ModbusClient.ConvertIntToRegisters(Convert.ToInt32( _Target1.Text.ToString())));
               
                modbusClient.WriteSingleCoil(34, true);// true m0
            }
        }

        private void Execute2_Click(object sender, RoutedEventArgs e)
        {
            if (modbusClient.Connected)
            {
                modbusClient.WriteMultipleRegisters(25036, ModbusClient.ConvertIntToRegisters(Convert.ToInt32(_Target2.Text.ToString())));

                modbusClient.WriteSingleCoil(35, true);// true m0
            }
        }

        private void Execute3_Click(object sender, RoutedEventArgs e)
        {
            if (modbusClient.Connected)
            {
                modbusClient.WriteMultipleRegisters(25039, ModbusClient.ConvertIntToRegisters(Convert.ToInt32(_Target3.Text.ToString())));

                modbusClient.WriteSingleCoil(36, true);// true m0
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (modbusClient.Connected)
            {
                if (modbusClient.ReadCoils(34, 1)[0])// true m0)
                {
                    modbusClient.WriteSingleCoil(34, false);// false m34
                }
                
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (modbusClient.Connected)
            {
                
                if (modbusClient.ReadCoils(35, 1)[0]) // true m0)
                {
                    modbusClient.WriteSingleCoil(35, false);// false m35
                }
               
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (modbusClient.Connected)
            {
                if (modbusClient.ReadCoils(36, 1)[0])// true m0)
                {
                    modbusClient.WriteSingleCoil(36, false);// false m34
                }
 
            }
        }
    }
}
