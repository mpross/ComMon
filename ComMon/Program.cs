using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms;
// Com port monitor.
// By M.P.Ros with many bits stolen from online (mainly StackOverflow)
namespace SeisIsoCom
{
    class SerialPortProgram
    {
        // Port set-up
        static private SerialPort port = new SerialPort("COM4",
          115200, Parity.None, 8, StopBits.One);
        //Data file path
        string path = System.IO.Directory.GetCurrentDirectory() + @"/data.txt";
        //COM line holder
        string red = "";
        //Loop count
        int count = 0;

        [STAThread]
        static void Main(string[] args)
        {
            Console.Write("Com port: ");
            string portName = Console.ReadLine().ToUpper();
            port= new SerialPort(portName,115200, Parity.None, 8, StopBits.One);
            new SerialPortProgram();
        }
        private SerialPortProgram()
        {
            port.DataReceived += new
              SerialDataReceivedEventHandler(port_DataReceived);
            try
            {
                port.Open();
                Application.Run();
            }
            catch(IOException error)
            {
                Console.WriteLine(error.ToString());
                Console.ReadLine();
            }
        }

        private void port_DataReceived(object sender,
          SerialDataReceivedEventArgs e)
        {
            red = port.ReadLine();
            try {
                Console.WriteLine(red);

                //Waits for gunk to clear off the port
                if (count > 1000)
                {
                    if (!File.Exists(path))
                    {
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine("");
                        }
                    }

                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(red);
                    }
                }
            }
            catch
            {
                Console.WriteLine("File Write Error");
            }

            count++;
        }

    }
}
