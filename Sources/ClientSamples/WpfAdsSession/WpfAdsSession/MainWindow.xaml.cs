using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;
using TwinCAT;
using TwinCAT.Ads;
using TwinCAT.TypeSystem;

namespace AdsSessionTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            cBError.SelectedItem = AdsErrorCode.ClientSyncTimeOut;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += _timer_Tick;

            tBDefaultResurrectionTime.Text = ((int)SessionSettings.DefaultResurrectionTime.TotalSeconds).ToString();
            tBDefaultTimeout.Text = ((int)SessionSettings.DefaultCommunicationTimeout.TotalSeconds).ToString();
            tbNetId.Text = AmsNetId.Local.ToString();
            tbPort.Text = "851";
            //EnableDisableControls();
            Update();
        }

        
        protected override void OnClosed(EventArgs e)
        {
            _timer.Stop();
            _timer = null;
            if (_connection != null)
            {
                IDisposable disp = _connection as IDisposable;
                
                if (disp != null)
                    disp.Dispose();
            }
            _connection = null;

            if (_session != null)
            {
                IDisposable disp = _session as IDisposable;
                
                if (disp != null)
                    disp.Dispose();
            }
            _session = null;
            base.OnClosed(e);
        }

        DispatcherTimer _timer = null;
        AdsSession _session = null;
        AdsConnection _connection = null;

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_connection == null)
                {
                    OnConnect();
                }
                else
                {
                    OnDisconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                OnDisconnect();
            }
            finally
            {
                EnableDisableControls();
                Update();
            }
        }

        private void OnDisconnect()
        {
            AmsAddress oldConnection = null;

            if (_timer != null)
                _timer.Stop();

            if (_connection != null)
            {
                oldConnection = _connection.Address;
                //_connection.AdsStateChanged -= _connection_AdsStateChanged;
                //_connection.AdsSymbolVersionChanged -= _connection_AdsSymbolVersionChanged;
                //_connection.ConnectionStatusChanged -= _connection_ConnectionStatusChanged;
                IDisposable disp = _connection as IDisposable;

                if (disp != null)
                    disp.Dispose();
            }

            if (_session != null)
            {
                IDisposable disp = _session as IDisposable;

                if (disp != null)
                    disp.Dispose();
            }
        
            _connection = null;
            _session = null;

            if (oldConnection != null)
                AddMessage(string.Format("Disconnected from {0} Port: {1}", oldConnection.NetId, oldConnection.Port));
        }

        private void OnConnect()
        {
            AmsNetId netId = AmsNetId.Parse(tbNetId.Text);
            int port = int.Parse(tbPort.Text);

            AmsAddress target = new AmsAddress(netId, port);

            int communicationTimeout = int.Parse(tBDefaultTimeout.Text) * 1000;
            int resurrectionTime = int.Parse(tBDefaultResurrectionTime.Text);

            SessionSettings settings = new SessionSettings(true, communicationTimeout);
            settings.ResurrectionTime = TimeSpan.FromSeconds((double)resurrectionTime);

            _session = new AdsSession(netId, port,settings);
            _connection = (AdsConnection)_session.Connect();

            try
            {
                _connection.AdsStateChanged += _connection_AdsStateChanged;
                _connection.AdsSymbolVersionChanged += _connection_AdsSymbolVersionChanged;
            }
            catch (AdsErrorException ex)
            {
                if (ex.ErrorCode == AdsErrorCode.DeviceServiceNotSupported)
                {
                    AddMessage(string.Format("Target {0} Port: {1} doesn't support state change notifications!",target,port));
                }
                else
                    throw ex;
            }
            _connection.ConnectionStateChanged += _connection_ConnectionStatusChanged;
            _timer.Start();

            AddMessage(string.Format("Connected to {0} Port: {1}", target, port));
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            Update();
        }


        private void _connection_ConnectionStatusChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            Update();
            EnableDisableControls();
            AddMessage(string.Format("Connection status has changed from '{0}' to '{1}'", e.OldState,e.NewState));
        }

        private void _connection_AdsSymbolVersionChanged(object sender, EventArgs e)
        {
            Update();
            AddMessage("Symbol version has changed");
        }

        private void _connection_AdsStateChanged(object sender, AdsStateChangedEventArgs e)
        {
            Update();
            AddMessage(string.Format("AdsState changed to '{0}", e.State.AdsState));
        }

        private void Update()
        {
            this.DataContext = null;
            this.DataContext = _session;

            SolidColorBrush adsStateBrush = new SolidColorBrush();
            SolidColorBrush connectionStateBrush = new SolidColorBrush();

            if (_connection != null)
            {
                tBConnectionState.Text = _connection.State.ToString();

                switch (_connection.State)
                {
                    case ConnectionState.Unknown:
                        connectionStateBrush = new SolidColorBrush();
                        break;
                    case ConnectionState.Disconnected:
                        connectionStateBrush = new SolidColorBrush(Colors.LightGray);
                        break;
                    case ConnectionState.Connected:
                        connectionStateBrush = new SolidColorBrush(Colors.LightGreen);
                        break;
                    case ConnectionState.Lost:
                        connectionStateBrush = new SolidColorBrush(Colors.Yellow);
                        break;
                    default:
                        break;
                }


                if (_connection.IsConnected)
                {
                    StateInfo info = new StateInfo();
                    AdsErrorCode errorCode = _connection.TryReadState(out info);
                    tBAdsState.Text = info.AdsState.ToString();

                    switch(info.AdsState)
                    {
                        case AdsState.Run:
                            adsStateBrush = new SolidColorBrush(Colors.LightGreen);
                            break;
                        case AdsState.Stop:
                        case AdsState.Error:
                            adsStateBrush = new SolidColorBrush(Colors.Red);
                            break;
                        case AdsState.Config:
                            adsStateBrush = new SolidColorBrush(Colors.Blue);
                            break;
                    }
                }
            }
            else
            {
                tBConnectionState.Text = ConnectionState.Unknown.ToString();
                tBAdsState.Text = AdsState.Invalid.ToString();
            }
            tBAdsState.Background = adsStateBrush;
            tBConnectionState.Background = connectionStateBrush;
        }

        private void EnableDisableControls()
        {
            AmsNetId netId = null;
            int port = -1;

            bool ok = AmsNetId.TryParse(tbNetId.Text, out netId);
            bool ok2 = int.TryParse(tbPort.Text, out port);

            bool validAddress = ok && ok2;

            if (_connection != null)
            {
                btnConnect.IsEnabled = true;
                btnConnect.Content = "Disconnect";
                btnInjectError.IsEnabled = !_connection.IsLost;
                btnSymbols.IsEnabled = !_connection.IsLost;
                btnResurrect.IsEnabled = _connection.IsLost;
            }
            else
            {
                btnConnect.IsEnabled = validAddress;
                btnConnect.Content = "Connect";
                btnInjectError.IsEnabled = false;
                btnSymbols.IsEnabled = false;
                btnResurrect.IsEnabled = false;
            }
        }

        private void btnInjectError_Click(object sender, RoutedEventArgs e)
        {
            _connection.InjectError(_errorCode);
            AddMessage(string.Format("Injecting Error '{0}'", _errorCode));
        }

        private void btnResurrect_Click(object sender, RoutedEventArgs e)
        {
            AdsException ex;

            if (!_connection.TryResurrect(out ex))
            {
                AddMessage(string.Format("Resurrection Error '{0}'", ex.Message));
            }
            else
            {
                AddMessage("Resurrection succeeded!");
            }
        }

        private void AddMessage(string str)
        {
            tBMessages.AppendText("\r\n" + str);
            tBMessages.ScrollToEnd();
        }

        AdsErrorCode _errorCode = AdsErrorCode.ClientSyncTimeOut;

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _errorCode = (AdsErrorCode)cBError.SelectedItem;
        }

        private void btnSymbols_Click(object sender, RoutedEventArgs e)
        {
            ReadOnlyDataTypeCollection dataTypes = _session.SymbolServer.DataTypes;
            ReadOnlySymbolCollection symbols = _session.SymbolServer.Symbols;

            SymbolsWindow window = new SymbolsWindow();
            window.SetSymbols(symbols, dataTypes);
            window.ShowDialog();
        }

        private void tbNetId_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableDisableControls();
        }

        private void tbPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableDisableControls();
        }
    }
}
