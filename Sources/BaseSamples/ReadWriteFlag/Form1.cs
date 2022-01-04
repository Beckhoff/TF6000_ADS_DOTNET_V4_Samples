using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sample01
{
	public class Form1 : System.Windows.Forms.Form
	{
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnRead;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Label label1;
		private TcAdsClient adsClient;

		public Form1()
		{				
			InitializeComponent();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(117, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(152, 22);
            this.textBox1.TabIndex = 0;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(288, 18);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(86, 28);
            this.btnRead.TabIndex = 1;
            this.btnRead.Text = "Read";
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(288, 55);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(86, 28);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "MAIN.nCounter:";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(384, 91);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Sample01";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			try
			{
				adsClient = new TcAdsClient();
				adsClient.Connect(851);
			}
			catch( Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			adsClient.Dispose();
		}		

    #region CODE_SAMPLE

		private void btnRead_Click(object sender, System.EventArgs e)
		{
			try
			{
                //Specify IndexGroup, IndexOffset and read PLCVar 
                uint iFlag = (uint)adsClient.ReadAny(0x4020, 0x0, typeof(UInt32));
                textBox1.Text = iFlag.ToString();
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

		private void btnWrite_Click(object sender, System.EventArgs e)
		{
			try
			{
                //Specify IndexGroup, IndexOffset and write PLCVar, then read the new Value
                UInt32 iFlag = 0;
                adsClient.WriteAny(0x4020, 0x0, iFlag);
                iFlag = (uint)adsClient.ReadAny(0x4020, 0x0, typeof(UInt32));
                textBox1.Text = iFlag.ToString();
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}
        #endregion

        public void ReadIndexGroupIndexOffset()
        {
            #region CODE_SAMPLE_READWRITE_IGIO

            using (TcAdsClient client = new TcAdsClient())
            {
                UInt32 valueToRead = 0;
                UInt32 valueToWrite = 42;

                client.Connect(AmsNetId.Local, 851);

                // Write an UINT32 Value
                AdsStream writeStream = new AdsStream(sizeof(uint));
                AdsBinaryWriter writer = new AdsBinaryWriter(writeStream);
                writer.Write(valueToWrite);
                adsClient.Write((uint)0x4020, (uint)0x0, writeStream, 0, 4);

                // Read an UINT32 Value
                AdsStream readStream = new AdsStream(sizeof(uint));
                adsClient.Read((uint)0x4020, (uint)0x0, readStream, 0, 4);
                readStream.Position = 0;
                AdsBinaryReader reader = new AdsBinaryReader(readStream);
                valueToRead = reader.ReadUInt32();
            }
            #endregion

            #region CODE_SAMPLE_READWRITE_BYHANDLE

            using (TcAdsClient client = new TcAdsClient())
            {
                int varHandle = 0;
                client.Connect(AmsNetId.Local, 851);
                try
                {
                    UInt16 valueToRead = 0;
                    UInt16 valueToWrite = 42;

                    // Create the Variable Handle
                    varHandle = client.CreateVariableHandle("MAIN.testVar"); //Test Var is defined as PLC INT

                    // Write an UINT16 Value
                    AdsStream writeStream = new AdsStream(sizeof(UInt16));
                    AdsBinaryWriter writer = new AdsBinaryWriter(writeStream);
                    writer.Write(valueToWrite); // Marshal the Value
                    adsClient.Write(varHandle, writeStream,0,sizeof(uint));

                    // Read an UINT16 Value
                    AdsStream readStream = new AdsStream(sizeof(UInt16));
                    adsClient.Read(varHandle, readStream, 0, sizeof(UInt16));
                    readStream.Position = 0;
                    AdsBinaryReader reader = new AdsBinaryReader(readStream);
                    valueToRead = reader.ReadUInt16(); // Unmarshal the Value
                }
                finally
                {
                    // Unregister VarHandle after Use
                    client.DeleteVariableHandle(varHandle);
                }
            }
            #endregion

        }

        public void ReadAny()
        {
            #region CODE_SAMPLE_READWRITE_ANY

            using (TcAdsClient client = new TcAdsClient())
            {
                UInt32 valueToRead = 0;
                UInt32 valueToWrite = 42;

                client.Connect(AmsNetId.Local, 851);
                adsClient.WriteAny(0x4020, 0x0, valueToWrite);
                valueToRead = (uint)adsClient.ReadAny(0x4020, 0x0, typeof(UInt32));
            }
            #endregion


            #region CODE_SAMPLE_BYHANDLE

            using (TcAdsClient client = new TcAdsClient())
            {
                int varHandle = 0;
                client.Connect(AmsNetId.Local, 851);
                try
                {
                    UInt32 valueToRead = 0;
                    UInt32 valueToWrite = 42;

                    varHandle = client.CreateVariableHandle("MAIN.nCounter");
                    adsClient.WriteAny(varHandle, valueToWrite);
                    valueToRead = (uint)adsClient.ReadAny(varHandle, typeof(UInt32));
                }
                finally
                {
                    // Unregister VarHandle after Use
                    client.DeleteVariableHandle(varHandle);
                }
            }
            #endregion

            #region CODE_SAMPLE_BYPATH

            using (TcAdsClient client = new TcAdsClient())
            {
                UInt32 valueToRead = 0;
                UInt32 valueToWrite = 42;

                client.Connect(AmsNetId.Local, 851);
                adsClient.WriteSymbol("MAIN.nCounter", valueToWrite, false);
                valueToRead = (uint)adsClient.ReadSymbol("MAIN.nCounter", typeof(UInt32),false);
            }
            #endregion

            #region CODE_SAMPLE_SYMBOL

            using (TcAdsClient client = new TcAdsClient())
            {
                UInt32 valueToRead = 0;
                UInt32 valueToWrite = 42;

                client.Connect(AmsNetId.Local, 851);

                ITcAdsSymbol symbol = adsClient.ReadSymbolInfo("MAIN.nCounter");
                adsClient.WriteSymbol(symbol, valueToWrite);
                valueToRead = (uint)adsClient.ReadSymbol(symbol);
            }
            #endregion

            #region CODE_SAMPLE_SYMBOLBROWSER

            using (TcAdsClient client = new TcAdsClient())
            {
                UInt32 valueToRead = 0;
                UInt32 valueToWrite = 42;

                client.Connect(AmsNetId.Local, 851);

                // Load all Symbols + DataTypes
                ISymbolLoader loader = SymbolLoaderFactory.Create(client, SymbolLoaderSettings.Default);
                Symbol symbol = (Symbol)loader.Symbols["MAIN.nCounter"];

                // Alternative with availability test
                ISymbol symbol2 = null;
                bool contains = loader.Symbols.TryGetInstance("MAIN.nCounter", out symbol2);

                // Works for ALL Primitive 'ANY TYPE' Symbols
                symbol.WriteValue(valueToWrite); 
                valueToRead = (UInt32)symbol.ReadValue();

                // Simple filtering of Symbols
                Regex filterExpression = new Regex(pattern: @"^MAIN.*"); // Everything that starts with "MAIN"
                
                // FilterFunction that filters for the InstancePath
                Func<ISymbol, bool> filter = s => filterExpression.IsMatch(s.InstancePath);
                SymbolIterator iterator = new SymbolIterator(symbols: loader.Symbols, recurse: true, predicate: filter);

                foreach(ISymbol filteredSymbol in iterator)
                {
                    Console.WriteLine(filteredSymbol.InstancePath);
                }
            }
            #endregion

            #region CODE_SAMPLE_DYNAMIC

            using (TcAdsClient client = new TcAdsClient())
            {
                UInt32 valueToRead = 0;
                UInt32 valueToWrite = 42;

                client.Connect(AmsNetId.Local, 851);

                // Load all Symbols + DataTypes
                // Primitive Parts will be automatically resolved to .NET Primitive types.
                IDynamicSymbolLoader loader = (IDynamicSymbolLoader)SymbolLoaderFactory.Create(client, SymbolLoaderSettings.DefaultDynamic);

                dynamic symbols = loader.SymbolsDynamic;
                dynamic main = symbols.Main;

                // Use typed object to use InfoTips
                DynamicSymbol nCounter = main.nCounter;

                // or to be fullDynamic 
                //dynamic nCounter = main.nCounter;

                // Works for ALL sorts of types (not restricted to ANY_TYPE basing primitive types)
                nCounter.WriteValue(valueToWrite);
                valueToRead = (uint)nCounter.ReadValue();
            }
            #endregion

            #region CODE_SAMPLE_DYNAMIC_COMPLEX

            using (TcAdsClient client = new TcAdsClient())
            {
                uint valueToRead = 0;
                uint valueToWrite = 42;

                client.Connect(AmsNetId.Local, 851);

                // Load all Symbols + DataTypes
                // Primitive Parts will be automatically resolved to .NET Primitive types.
                IDynamicSymbolLoader loader = (IDynamicSymbolLoader)SymbolLoaderFactory.Create(client, SymbolLoaderSettings.DefaultDynamic);

                dynamic symbols = loader.SymbolsDynamic;
                dynamic main = symbols.Main;

                // Use typed object to use InfoTips
                DynamicSymbol nCounter = main.nCounter; // UDINT

                // or to be fullDynamic 
                //dynamic nCounter = main.nCounter;

                // Works for ALL sorts of types (not restricted to ANY_TYPE basing primitive types)
                valueToRead = (uint)nCounter.ReadValue();
                // or
                var varValue = nCounter.ReadValue();
                // or
                dynamic dynValue = nCounter.ReadValue();

                // Same for writing
                nCounter.WriteValue(valueToWrite);

                // Or Notifications / Events
                nCounter.ValueChanged += NCounter_ValueChanged;

                //Reading complexTypes e.g. Struct

                DynamicSymbol myStructSymbol = main.plcStruct; // Dynamically created
                dynamic myStructVal = myStructSymbol.ReadValue(); // Takes an ADS Snapshot of the value

                dynamic int1Val = myStructVal.int1; // Value to an INT (short)
                dynamic valueNestedStruct = myStructVal.nestedStruct; //value to another complex type (here a nested Struct)

                myStructSymbol.ValueChanged += MyStructSymbol_ValueChanged;
            }
            #endregion
        }

       

        #region CODE_SAMPLE_DYNAMIC_COMPLEX2
        private void NCounter_ValueChanged(object sender, ValueChangedArgs e)
        {
            var uintVal = e.Value;
        }

        private void MyStructSymbol_ValueChanged(object sender, ValueChangedArgs e)
        {
            dynamic structValue = e.Value; // Snapshot of the whole Struct and all its contents
        }
        #endregion
    }
}
