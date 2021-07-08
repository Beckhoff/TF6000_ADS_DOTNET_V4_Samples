using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TwinCAT.Ads;
using TwinCAT.Ads.Server;


    /* Sample Windows.Forms application that instantiates ans connects an AdsSampleServer. 
     */

    public partial class AdsSampleServerTester : Form
    {
        private AdsSampleServer _server;
        private uint _serverNotificationHandle = 0;    

        private delegate void LoggerAppender(String message);

        private event LoggerAppender _loggerAppender;

        public AdsSampleServerTester()
        {
            InitializeComponent();

            // Create a new AdsSampleServer instance listening on Ads port 27000.

            _server = new AdsSampleServer(/*27000,*/"AdsSampleServer", this);
            _loggerAppender = new LoggerAppender(AppendLoggerListDelegate);
        }

        void AdsSampleServerTester_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // Disconnect the server from the local ADS router.
                
                _server.Disconnect();
            }
            catch
            {
                // Do nothing if disconnect fails while closing the application
            }
        
        }

#if WindowsCE
        [MTAThread]
#else
        [STAThread]
#endif       
        static void Main()
        {
            Application.Run(new AdsSampleServerTester());
        }

        /* Event handlers for buttons.
         */
        
        private void _buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                /* Connect the server to the local ADS router. Now the server is ready to 
                 * answer requests.
                 */
                _server.Connect();
                _loggerListbox.Items.Add("Server is connected to port " + _server.Address.Port);
            }
            catch
            {
                MessageBox.Show("Connect failed");
            }
        }
        
        private void _buttonDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                /* Disconnect the server from the local ADS router.
                 */ 
                _server.Disconnect();
                _loggerListbox.Items.Add("Server is disconnected");
            }
            catch
            {
                MessageBox.Show("Disconnect failed");
            }
        }

        /* The following button event handlers send ADS requests to this server. The responses are
         * handled by the confirmation methods in the AdsSampleServer class.
         * The invokeId is always set to 0 in this sample. Use the invokeId in your server implemtation
         * to match requests an confirmations.
         */
        #region Request Button Handlers 

        private void _ReadDevInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                _server.AdsReadDeviceInfoReq(_server.Address, // receiver address
                                             0);              // invokeId  
            }
            catch
            {
                _loggerListbox.Items.Add("Read Device Info call failed.");
            }
        }

        private void _readButton_Click(object sender, EventArgs e)
        {
            try
            {
                _server.AdsReadReq(_server.Address, // receiver address
                                   0,               // invokeId 
                                   0x10000,         // index group
                                   0,               // index offset
                                   4);              // number of bytes to read
            }
            catch 
            {

                _loggerListbox.Items.Add("Read call failed.");
            }
        }

        private void _writeButton_Click(object sender, EventArgs e)
        {
            try
            {
                _server.AdsWriteReq(_server.Address, // receiver address
                                    0,               // invokeId 
                                    0x10000,         // index group
                                    0,               // index offset
                                    0,               // number of bytes to write
                                    null);           // data
            }
            catch 
            {

                _loggerListbox.Items.Add("Write call failed.");
            }
        }

        private void _readStateButton_Click(object sender, EventArgs e)
        {
            try
            {
                _server.AdsReadStateReq(_server.Address, // receiver address
                                         0);             // invokeId
            }
            catch 
            {
                
                 _loggerListbox.Items.Add("Read State call failed.");
            }
        }

        private void _writeContolButton_Click(object sender, EventArgs e)
        {
            try
            {
                _server.AdsWriteControlReq(_server.Address, // receiver address
                                            0,              // invokeId
                                            AdsState.Idle,  // new ADS state
                                            3,              // new device state
                                            0,              // number bytes in additional data buffer
                                            null);          // additonal data buffer
            }
            catch
            {
                _loggerListbox.Items.Add("Write Control call failed.");
            }
        }

        private void _addNotButton_Click(object sender, EventArgs e)
        {
            try
            {
				_server.AdsAddDeviceNotificationReq(new AmsAddress("172.16.5.167.1.1", 801), // receiver address
                                                    0,               // invokeId
                                                    (uint)AdsReservedIndexGroups.DeviceData,         // index group
                                                    (uint)AdsReservedIndexOffsets.DeviceDataAdsState,               // index offset
                                                    4,               // number of bytes to be sent
                                                    AdsTransMode.OnChange, // transmission mode
                                                    1000,           // maximum delay
                                                    1000);          // cycle time
            }
            catch
            {

                _loggerListbox.Items.Add("Add Device Notification call failed.");
            }
        }

        private void _delNotButton_Click(object sender, EventArgs e)
        {
            try
            {
                _server.AdsDelDeviceNotificationReq(
                                    _server.Address,            // receiver address
                                    0,                          // invokeId
                                    _serverNotificationHandle   // notification handle to be deleted
                                    );
            }
            catch
            {

                _loggerListbox.Items.Add("Add Device Notification call failed.");
            }
        }

        private void _readWriteButton_Click(object sender, EventArgs e)
        {
            try
            {
                _server.AdsReadWriteReq(_server.Address,    // receiver address
                                        0,                  // invokeId
                                        0x10000,            // index group
                                        0,                  // index offset
                                        4,                  // number of bytes to read
                                        0,                  // number of bytes to write
                                        null);              // write data buffer
            }
            catch 
            {

                _loggerListbox.Items.Add("Read Write call failed.");
            }
        }

        #endregion


        #region properties

        public uint ServerNotificationHandle
        {
            get { return _serverNotificationHandle; }
            set { _serverNotificationHandle = value; }
        }
        
        #endregion

        #region Helper Methods

        public void AppendLoggerList(string logMessage)
        {
            this.Invoke(_loggerAppender, new object[] { logMessage });
        }

        private void AppendLoggerListDelegate(string logMessage)
        {
            _loggerListbox.Items.Add(logMessage);
        }

        #endregion



        
    }
