#region CODE_SAMPLE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TwinCAT.Ads;

namespace TwinCATAds_Sample08
{
	public partial class Form1 : Form
	{
		private TcAdsClient	_tcClient = null;
		private AdsStream		_adsStream = null;
		private BinaryReader _binRead = null;
		private int				_notificationHandle = 0;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				_tcClient = new TcAdsClient();

				/* connect the client to the local PLC */
				_tcClient.Connect(851);

				_adsStream = new AdsStream(2);				/* stream storing the ADS state of the PLC */
				_binRead = new BinaryReader(_adsStream);	/* reader to read the state data */

				/* register callback to react on state changes of the local AMS Router */
				_tcClient.AmsRouterNotification +=
										new AmsRouterNotificationEventHandler(AmsRouterNotificationCallback);


				_notificationHandle = _tcClient.AddDeviceNotification(
											(uint)AdsReservedIndexGroups.DeviceData,	/* index group of the device state*/
											(uint)AdsReservedIndexOffsets.DeviceDataAdsState, /*index offsset of the device state */
											_adsStream,	/* stream to store the state */
											AdsTransMode.OnChange,	/* transfer mode: transmit ste on change */
											0,	/* transmit changes immediately */
											0,
											null);

				/* register callback to react on state changes of the local PLC */
				_tcClient.AdsNotification += new AdsNotificationEventHandler(OnAdsNotification);
			}
			catch (AdsErrorException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		/* callback function called on state changes of the PLC */
		void OnAdsNotification(object sender, AdsNotificationEventArgs e)
		{
			if (e.NotificationHandle == _notificationHandle)
			{
				AdsState plcState = (AdsState)_binRead.ReadInt16(); /* state was written to the stream */
				_plcLabelValue.Text = plcState.ToString();
			}
		}

		/* callback function called on state changes of the local AMS Router */
		void AmsRouterNotificationCallback(object sender, AmsRouterNotificationEventArgs e)
		{
			_routerLabelValue.Text = e.State.ToString();
		}

		private void _exitButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				
                if(_notificationHandle != 0)
                    _tcClient.DeleteDeviceNotification(_notificationHandle);

                _tcClient.Dispose();
			}
			catch(AdsErrorException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
#endregion