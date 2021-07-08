using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.IO;

using TwinCAT.Ads.Server;
using TwinCAT.Ads;

/*
 * Extend the TcAdsServer class to implement your own ADS server.
 */
public class AdsSampleServer : TcAdsServer 
    {

        private byte[] _dataBuffer = { 1, 2, 3, 4 };
        private AdsState _localAdsState = AdsState.Run;
        private ushort _localDeviceState = 0;
        private Hashtable _notificationTable = new Hashtable();
        private uint _currentNotificationHandle = 0;
        private AdsSampleServerTester _gui;
       
         /* Instanstiate an ADS server with a fix ADS port asigned by the ADS router.
         */
        public AdsSampleServer(ushort port, string portName, AdsSampleServerTester gui) 
                                            : base(port, portName)
        {
            // custom intialization  
            _gui = gui;
        }
	
        /*
         * Instanstiate an ADS server with an unfixed ADS port asigned by the ADS router.
         */
        public AdsSampleServer(string portName, AdsSampleServerTester gui)
            : base(portName)
        {
            // custom intialization  
            _gui = gui;
        }

        /* Overwrite the indication methods of the TcAdsServer class for the services your ADS server
         * provides. They are called upon incoming requests. All indications that are not overwritten in
         * this class return the ADS DeviceServiceNotSupported error code to the requestor.
         * This server replys on: ReadDeviceInfo, Read, Write and ReadState requests. 
         */
        public override void AdsReadDeviceInfoInd(AmsAddress rAddr,
                                                  uint invokeId)
        {
            AdsVersion version = new AdsVersion();
            version.Version = 1;
            version.Revision = 0;
            version.Build = 111;

            // Send a response to the requestor

            AdsReadDeviceInfoRes(rAddr,                 // requestor's AMS address     
                                invokeId,               // invoke id provided by requestor
                                AdsErrorCode.NoError,   // ADS error code
                                "C#_TestServer",        // name of this server
                                version);               // version of this server
        }

        public override void AdsWriteInd(AmsAddress rAddr,
                                uint invokeId,
                                uint indexGroup,
                                uint indexOffset,
                                uint cbLength,
                                byte[] data)
        {
            AdsErrorCode adsError = AdsErrorCode.NoError;

            switch (indexGroup) /* use index group (and offset) to distiguish between the services
                                    of this server */
            {
                case 0x10000:    
                    if (cbLength == 4 && data.Length == 4)
                    {
                        Array.Copy(data, _dataBuffer, 4);
                    }
                    else
                    {
                        adsError = AdsErrorCode.DeviceInvalidParam;
                    }
                    break;
				case 0x20000: /* used for the PLC Sample */
					if (cbLength == 4 && data.Length == 4)
					{
						BinaryReader binReader = new BinaryReader(new MemoryStream(data));
						_gui.AppendLoggerList(String.Format("PLC Counter: {0}", binReader.ReadUInt32()));
					}
					break;

                default:        /* other services are not supported */
                        adsError = AdsErrorCode.DeviceServiceNotSupported;
                        break;

            }

            // Send a response to the requestor

            AdsWriteRes(rAddr,          // requestor's AMS address   
                        invokeId,       // invoke id provided by requestor
                        adsError);      // ADS error code
        }

        public override void AdsReadInd(AmsAddress rAddr,
                               uint invokeId,
                               uint indexGroup,
                               uint indexOffset,
                               uint cbLength)
        {
            /* Distinguish between services like in AdsWriteInd */

            // Send a response to the requestor
           
            AdsReadRes(rAddr,                   // requestor's AMS address
                        invokeId,               // invoke id provided by requestor
                        AdsErrorCode.NoError,   // ADS error code
                        4,                      // length of the data buffer
                        _dataBuffer);            // data buffer
        }

    public override void AdsReadStateInd(AmsAddress rAddr,
                                        uint invokeId)
    {
            AdsReadStateRes(rAddr,                 // requestor's AMS address
                            invokeId,              // invoke id provided by requestor
                            AdsErrorCode.NoError,   // ADS error code
                            _localAdsState,        // ADS state
                            _localDeviceState);    // device state
    }

    public override void AdsWriteControlInd(AmsAddress rAddr,
                                            uint invokeId,
                                            AdsState adsState,
                                            ushort deviceState,
                                            uint cbLength,
                                            byte[] pDeviceData)
    {
        // Set requested ADS and device status

        _localAdsState = adsState;
        _localDeviceState = deviceState;

        // Send a response to the requestor

        AdsWriteControlRes(rAddr,                   // requestor's AMS address
                           invokeId,                // invoke id provided by requestor
                           AdsErrorCode.NoError);   // ADS error code
    }

    public override void AdsAddDeviceNotificationInd(AmsAddress rAddr,
                                                     uint invokeId,
                                                     uint indexGroup,
                                                     uint indexOffset,
                                                     uint cbLength,
                                                     AdsTransMode transMode,
                                                     uint maxDelay,
                                                     uint cycleTime)
    {
        /* Create a new notifcation entry an store it in the notification table */
        NotificationRequestEntry notEntry = new NotificationRequestEntry(rAddr,
                                                                         indexGroup,
                                                                         indexOffset,
                                                                         cbLength,
                                                                         transMode,
                                                                         maxDelay,
                                                                         cycleTime);
        _notificationTable.Add(_currentNotificationHandle, notEntry);

        // Send a response to the requestor
        AdsAddDeviceNotificationRes(rAddr,
                                    invokeId,
                                    AdsErrorCode.NoError,
                                    _currentNotificationHandle++);
                                    
    }

    public override void AdsDelDeviceNotificationInd(AmsAddress rAddr,
                                                     uint invokeId,
                                                     uint hNotification)
    {
        AdsErrorCode errorCode = AdsErrorCode.NoError;

        /* check if the requested notification handle is still in the notification table */
        if (_notificationTable.Contains(hNotification))
        {
            _notificationTable.Remove(hNotification);   // remove the notification handle from
                                                        // the notification table
        }
        else // notification handle is not in the notofication table -> return an error code
             // to the requestor
        {
            errorCode = AdsErrorCode.DeviceNotifyHandleInvalid;
        }

        // Send a response to the requestor

        AdsDelDeviceNotificationRes(rAddr,
                                    invokeId,
                                    errorCode);

    }

    public override void AdsDeviceNotificationInd(AmsAddress rAddr,
                                                  uint invokeId,
                                                  uint numStapHeaders,
                                                  TcAdsStampHeader[] stampHeaders)
    {
        /**
         * Call notification handlers.
         */

        _gui.AppendLoggerList("Received Device Notification Request"); 
    }

    public override void AdsReadWriteInd(AmsAddress rAddr,
                                         uint invokeId,
                                         uint indexGroup,
                                         uint indexOffset,
                                         uint cbReadLength,
                                         uint cbWriteLength,
                                         byte[] data)
    {
        /* Distinguish between services like in AdsWriteInd */ 
        
        // Send a response to the requestor

        if (cbReadLength == 4 && cbWriteLength == 4)
        {
            AdsReadWriteRes(rAddr,                   // requestor's AMS address
                            invokeId,                // invoke id provided by requestor
                            AdsErrorCode.NoError,    // ADS error code
                            cbReadLength,
                            _dataBuffer);
        }
        else
        {
            AdsReadWriteRes(rAddr,                             // requestor's AMS address
                            invokeId,                          // invoke id provided by requestor
                            AdsErrorCode.DeviceInvalidSize,    // ADS error code
                            0,
                            null);
            return;
        }

        Array.Copy(data, _dataBuffer, 4);
        
    }
    
    /* Overwrite the  confirmation methods of the TcAdsServer class for the requestts your ADS server
     * sends. They are called upon incoming responses. These sample implemetations only add a log message
     * to the sample form.
     */
    public override void AdsReadStateCon(AmsAddress rAddr,
                                         uint invokeId,
                                         AdsErrorCode result,
                                         AdsState adsState,
                                         ushort deviceState)
    {
        _gui.AppendLoggerList("Received Read State Confirmation");
    }

    public override void AdsReadCon(AmsAddress rAddr,
                                    uint invokeId,
                                    AdsErrorCode result,
                                    uint cbLength,
                                    byte[] data)
    {
        _gui.AppendLoggerList("Received Read Confirmation");
    }

    public override void AdsWriteCon(AmsAddress rAddr,
                                     uint invokeId,
                                     AdsErrorCode result)
    {
        _gui.AppendLoggerList("Received Write Confirmation");
    }

    public override void AdsReadDeviceInfoCon(AmsAddress rAddr,
                                              uint invokeId,
                                              AdsErrorCode result,
                                              string name,
                                              AdsVersion version)
    {
        _gui.AppendLoggerList("Received Read Device Info Confirmation");
    }

    public override void AdsWriteControlCon(AmsAddress rAddr,
                                            uint invokeId,
                                            AdsErrorCode result)
    {
        _gui.AppendLoggerList("Received Write Control Confirmation");
    }

    public override void AdsAddDeviceNotificationCon(AmsAddress rAddr,
                                                     uint invokeId,
                                                     AdsErrorCode result,
                                                     uint notificationHandle)
    {
        _gui.ServerNotificationHandle = notificationHandle;
        _gui.AppendLoggerList("Received Add Device Notification Confirmation. Notification handle: "
                                                                            + notificationHandle);  
    }

    public override void AdsDelDeviceNotificationCon(AmsAddress rAddr,
                                                     uint invokeId,
                                                      AdsErrorCode result)
    {
        _gui.AppendLoggerList("Received Delete Device Notification Confirmation");
    }

    public override void AdsReadWriteCon(AmsAddress rAddr,
                                         uint invokeId,
                                         AdsErrorCode result,
                                         uint cbLength,
                                         byte[] data)
    {
        _gui.AppendLoggerList("Received Read Write Confirmation");
    }
    
}