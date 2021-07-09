using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwinCAT.Ads;

namespace AccessByVarName
{
    class AccessByVarName
    {
        static void Main(string[] args)
        {
    #region CODE_SAMPLE

            //Create a new instance of class TcAdsClient
            TcAdsClient tcClient = new TcAdsClient();
            AdsStream dataStream = new AdsStream(4);
            AdsBinaryReader binReader = new AdsBinaryReader(dataStream);

            int iHandle = 0;
            int iValue = 0;
            
            try
            {
                //Connect to local PLC - Port 851
                tcClient.Connect(851);
                
                //Get the handle of the PLC variable "nCounter"
                iHandle = tcClient.CreateVariableHandle("MAIN.nCounter");
                Console.WriteLine("Press Enter five times to end");
                for(int i = 0; i < 5; i++)
                {
                    //Use the handle to read PLCVar
                    tcClient.Read(iHandle, dataStream);
                    iValue = binReader.ReadInt32();
                    dataStream.Position = 0;
                    Console.WriteLine("Value: " + iValue);
                    Console.ReadKey();
                }

                //Reset PLC variable to zero
                tcClient.WriteAny(iHandle, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            finally
            {
                tcClient.DeleteVariableHandle(iHandle);
                tcClient.Dispose();
            }
            #endregion
        }
    }
}
