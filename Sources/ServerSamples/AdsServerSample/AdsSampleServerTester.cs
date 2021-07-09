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

        _server = new AdsSampleServer(27000,"AdsSampleServer", this);
        _loggerAppender = new LoggerAppender(AppendLoggerListDelegate);
        enableDisableControls();
    }

    void AdsSampleServerTester_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        try
        {
            // Disconnect the server from the local ADS router.

            _server.Disconnect();
        }
        catch (Exception)
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

    void enableDisableControls()
    {
        _buttonConnect.Enabled = !_server.IsConnected;
        _buttonDisconnect.Enabled = _server.IsConnected;

        _addNotButton.Enabled = _server.IsConnected;
        _delNotButton.Enabled = _server.IsConnected;
        _readButton.Enabled = _server.IsConnected;
        _readDeviceInfoButton.Enabled = _server.IsConnected;
        _readStateButton.Enabled = _server.IsConnected;
        _readWriteButton.Enabled = _server.IsConnected;
        _writeButton.Enabled = _server.IsConnected;
        _writeControlButton.Enabled = _server.IsConnected;

        AmsAddress address = _server.Address;
        this.Text = string.Format("AdsServerSample '{0}'", address != null ? address.ToString() : string.Empty);

    }

    /* Event handlers for buttons.
     */

    private void OnConnectClicked(object sender, EventArgs e)
    {
        try
        {
            /* Connect the server to the local ADS router. Now the server is ready to 
             * answer requests.
             */
            _server.Connect();
            enableDisableControls();
            _loggerListbox.Items.Add("Server is connected to port " + _server.Address.Port);
        }
        catch (Exception)
        {
            MessageBox.Show("Connect failed");
        }
        finally
        {
            enableDisableControls();
        }
    }

    private void OnDisconnectClicked(object sender, EventArgs e)
    {
        try
        {
            /* Disconnect the server from the local ADS router.
             */
            _server.Disconnect();
            _loggerListbox.Items.Add("Server is disconnected");
        }
        catch (Exception)
        {
            MessageBox.Show("Disconnect failed");
        }
        finally
        {
            enableDisableControls();
        }
    }

    /* The following button event handlers send ADS requests to this server. The responses are
     * handled by the confirmation methods in the AdsSampleServer class.
     * The invokeId is always set to 0 in this sample. Use the invokeId in your server implemtation
     * to match requests an confirmations.
     */
    #region Request Button Handlers 

    private void OnReadDevInfoClicked(object sender, EventArgs e)
    {
        try
        {
            _server.AdsReadDeviceInfoReq(_server.Address, // receiver address
                                         0);              // invokeId  
        }
        catch (Exception)
        {
            _loggerListbox.Items.Add("Read Device Info call failed.");
        }
        finally
        {
            enableDisableControls();
        }
    }

    private void OnReadClicked(object sender, EventArgs e)
    {
        try
        {
            _server.AdsReadReq(_server.Address, // receiver address
                               0,               // invokeId 
                               0x10000,         // index group
                               0,               // index offset
                               4);              // number of bytes to read
        }
        catch (Exception)
        {

            _loggerListbox.Items.Add("Read call failed.");
        }
        finally
        {
            enableDisableControls();
        }
    }

    private void OnWriteClicked(object sender, EventArgs e)
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
        catch (Exception)
        {
            _loggerListbox.Items.Add("Write call failed.");
        }
        finally
        {
            enableDisableControls();
        }
    }

    private void OnReadStateClicked(object sender, EventArgs e)
    {
        try
        {
            _server.AdsReadStateReq(_server.Address, // receiver address
                                     0);             // invokeId
        }
        catch (Exception)
        {
            _loggerListbox.Items.Add("Read State call failed.");
        }
        finally
        {
            enableDisableControls();
        }
    }

    private void OnWriteControlClicked(object sender, EventArgs e)
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
        catch (Exception)
        {
            _loggerListbox.Items.Add("Write Control call failed.");
        }
        finally
        {
            enableDisableControls();
        }
    }

    private void OnAddNotificationClicked(object sender, EventArgs e)
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
        catch (Exception)
        {
            _loggerListbox.Items.Add("Add Device Notification call failed.");
        }
        finally
        {
            enableDisableControls();
        }
    }

    private void OnDeleteNotificationClicked(object sender, EventArgs e)
    {
        try
        {
            _server.AdsDelDeviceNotificationReq(
                                _server.Address,            // receiver address
                                0,                          // invokeId
                                _serverNotificationHandle   // notification handle to be deleted
                                );
        }
        catch (Exception)
        {

            _loggerListbox.Items.Add("Add Device Notification call failed.");
        }
        finally
        {
            enableDisableControls();
        }
    }

    private void OnReadWriteClicked(object sender, EventArgs e)
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
        catch (Exception)
        {
            _loggerListbox.Items.Add("Read Write call failed.");
        }
        finally
        {
            enableDisableControls();
        }
    }

    #endregion


    public uint ServerNotificationHandle
    {
        get { return _serverNotificationHandle; }
        set { _serverNotificationHandle = value; }
    }

    public void AppendLoggerList(string logMessage)
    {
        this.Invoke(_loggerAppender, new object[] { logMessage });
    }

    private void AppendLoggerListDelegate(string logMessage)
    {
        _loggerListbox.Items.Add(logMessage);
    }
}
