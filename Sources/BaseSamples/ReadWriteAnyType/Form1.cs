#region CODE_SAMPLE
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using TwinCAT.Ads;
using System.Threading;

namespace Sample14
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Button btnDeleteNotifications;
		internal System.Windows.Forms.Button btnAddNotifications;
		internal System.Windows.Forms.Button btnWrite;
		internal System.Windows.Forms.Button btnRead;
		internal System.Windows.Forms.GroupBox GroupBox3;
		internal System.Windows.Forms.TextBox tbComplexStruct_dintArr;
		internal System.Windows.Forms.Label Label14;
		internal System.Windows.Forms.TextBox tbComplexStruct_ByteVal;
		internal System.Windows.Forms.Label Label13;
		internal System.Windows.Forms.TextBox tbComplexStruct_SimpleStruct1_lrealVal;
		internal System.Windows.Forms.Label Label12;
		internal System.Windows.Forms.TextBox tbComplexStruct_SimpleStruct_dintVal;
		internal System.Windows.Forms.Label Label11;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.TextBox tbComplexStruct_stringVal;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.TextBox tbComplexStruct_boolVal;
		internal System.Windows.Forms.Label Label9;
		internal System.Windows.Forms.TextBox tbComplexStruct_IntVal;
		internal System.Windows.Forms.Label Label10;
		internal System.Windows.Forms.GroupBox GroupBox2;
		internal System.Windows.Forms.TextBox tbStr2;
		internal System.Windows.Forms.Label Label7;
		internal System.Windows.Forms.TextBox tbStr1;
		internal System.Windows.Forms.Label Label8;
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.TextBox tblreal1;
		internal System.Windows.Forms.Label Label6;
		internal System.Windows.Forms.TextBox tbUsint1;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.TextBox tbDint1;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.TextBox tbBool1;
		internal System.Windows.Forms.Label Label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		//PLC variable handles
		private int hdint1;
		private int hbool1;    
		private int husint1;
		private int hlreal1;
		private int hstr1;
		private int hstr2;
		private int hcomplexStruct;
		private ArrayList notificationHandles;
		
		private TcAdsClient adsClient;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
            this.btnDeleteNotifications = new System.Windows.Forms.Button();
            this.btnAddNotifications = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.tbComplexStruct_dintArr = new System.Windows.Forms.TextBox();
            this.Label14 = new System.Windows.Forms.Label();
            this.tbComplexStruct_ByteVal = new System.Windows.Forms.TextBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.tbComplexStruct_SimpleStruct1_lrealVal = new System.Windows.Forms.TextBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.tbComplexStruct_SimpleStruct_dintVal = new System.Windows.Forms.TextBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.tbComplexStruct_stringVal = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.tbComplexStruct_boolVal = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.tbComplexStruct_IntVal = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.tbStr2 = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.tbStr1 = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.tblreal1 = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.tbUsint1 = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.tbDint1 = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.tbBool1 = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.GroupBox3.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDeleteNotifications
            // 
            this.btnDeleteNotifications.Location = new System.Drawing.Point(566, 129);
            this.btnDeleteNotifications.Name = "btnDeleteNotifications";
            this.btnDeleteNotifications.Size = new System.Drawing.Size(135, 27);
            this.btnDeleteNotifications.TabIndex = 13;
            this.btnDeleteNotifications.Text = "Delete Notifications";
            this.btnDeleteNotifications.Click += new System.EventHandler(this.btnDeleteNotifications_Click);
            // 
            // btnAddNotifications
            // 
            this.btnAddNotifications.Location = new System.Drawing.Point(566, 92);
            this.btnAddNotifications.Name = "btnAddNotifications";
            this.btnAddNotifications.Size = new System.Drawing.Size(135, 27);
            this.btnAddNotifications.TabIndex = 12;
            this.btnAddNotifications.Text = "Add Notifications";
            this.btnAddNotifications.Click += new System.EventHandler(this.btnAddNotifications_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(566, 55);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(135, 27);
            this.btnWrite.TabIndex = 11;
            this.btnWrite.Text = "Write";
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(566, 18);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(135, 27);
            this.btnRead.TabIndex = 10;
            this.btnRead.Text = "Read";
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.tbComplexStruct_dintArr);
            this.GroupBox3.Controls.Add(this.Label14);
            this.GroupBox3.Controls.Add(this.tbComplexStruct_ByteVal);
            this.GroupBox3.Controls.Add(this.Label13);
            this.GroupBox3.Controls.Add(this.tbComplexStruct_SimpleStruct1_lrealVal);
            this.GroupBox3.Controls.Add(this.Label12);
            this.GroupBox3.Controls.Add(this.tbComplexStruct_SimpleStruct_dintVal);
            this.GroupBox3.Controls.Add(this.Label11);
            this.GroupBox3.Controls.Add(this.Label5);
            this.GroupBox3.Controls.Add(this.tbComplexStruct_stringVal);
            this.GroupBox3.Controls.Add(this.Label3);
            this.GroupBox3.Controls.Add(this.tbComplexStruct_boolVal);
            this.GroupBox3.Controls.Add(this.Label9);
            this.GroupBox3.Controls.Add(this.tbComplexStruct_IntVal);
            this.GroupBox3.Controls.Add(this.Label10);
            this.GroupBox3.Location = new System.Drawing.Point(221, 9);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(326, 323);
            this.GroupBox3.TabIndex = 9;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Complex Structure";
            // 
            // tbComplexStruct_dintArr
            // 
            this.tbComplexStruct_dintArr.Location = new System.Drawing.Point(77, 65);
            this.tbComplexStruct_dintArr.Name = "tbComplexStruct_dintArr";
            this.tbComplexStruct_dintArr.Size = new System.Drawing.Size(230, 22);
            this.tbComplexStruct_dintArr.TabIndex = 20;
            // 
            // Label14
            // 
            this.Label14.Location = new System.Drawing.Point(10, 65);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(57, 18);
            this.Label14.TabIndex = 19;
            this.Label14.Text = "dintArr:";
            // 
            // tbComplexStruct_ByteVal
            // 
            this.tbComplexStruct_ByteVal.Location = new System.Drawing.Point(77, 138);
            this.tbComplexStruct_ByteVal.Name = "tbComplexStruct_ByteVal";
            this.tbComplexStruct_ByteVal.Size = new System.Drawing.Size(230, 22);
            this.tbComplexStruct_ByteVal.TabIndex = 18;
            // 
            // Label13
            // 
            this.Label13.Location = new System.Drawing.Point(10, 138);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(67, 19);
            this.Label13.TabIndex = 17;
            this.Label13.Text = "byteVal:";
            // 
            // tbComplexStruct_SimpleStruct1_lrealVal
            // 
            this.tbComplexStruct_SimpleStruct1_lrealVal.Location = new System.Drawing.Point(125, 249);
            this.tbComplexStruct_SimpleStruct1_lrealVal.Name = "tbComplexStruct_SimpleStruct1_lrealVal";
            this.tbComplexStruct_SimpleStruct1_lrealVal.Size = new System.Drawing.Size(182, 22);
            this.tbComplexStruct_SimpleStruct1_lrealVal.TabIndex = 16;
            // 
            // Label12
            // 
            this.Label12.Location = new System.Drawing.Point(58, 249);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(57, 19);
            this.Label12.TabIndex = 15;
            this.Label12.Text = "lrealVal:";
            // 
            // tbComplexStruct_SimpleStruct_dintVal
            // 
            this.tbComplexStruct_SimpleStruct_dintVal.Location = new System.Drawing.Point(125, 286);
            this.tbComplexStruct_SimpleStruct_dintVal.Name = "tbComplexStruct_SimpleStruct_dintVal";
            this.tbComplexStruct_SimpleStruct_dintVal.Size = new System.Drawing.Size(182, 22);
            this.tbComplexStruct_SimpleStruct_dintVal.TabIndex = 14;
            // 
            // Label11
            // 
            this.Label11.Location = new System.Drawing.Point(58, 286);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(57, 19);
            this.Label11.TabIndex = 13;
            this.Label11.Text = "dintVal:";
            // 
            // Label5
            // 
            this.Label5.Location = new System.Drawing.Point(10, 212);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(96, 27);
            this.Label5.TabIndex = 12;
            this.Label5.Text = "simpleStruct1:";
            // 
            // tbComplexStruct_stringVal
            // 
            this.tbComplexStruct_stringVal.Location = new System.Drawing.Point(77, 175);
            this.tbComplexStruct_stringVal.Name = "tbComplexStruct_stringVal";
            this.tbComplexStruct_stringVal.Size = new System.Drawing.Size(230, 22);
            this.tbComplexStruct_stringVal.TabIndex = 11;
            // 
            // Label3
            // 
            this.Label3.Location = new System.Drawing.Point(10, 175);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(67, 19);
            this.Label3.TabIndex = 10;
            this.Label3.Text = "stringVal:";
            // 
            // tbComplexStruct_boolVal
            // 
            this.tbComplexStruct_boolVal.Location = new System.Drawing.Point(77, 102);
            this.tbComplexStruct_boolVal.Name = "tbComplexStruct_boolVal";
            this.tbComplexStruct_boolVal.Size = new System.Drawing.Size(230, 22);
            this.tbComplexStruct_boolVal.TabIndex = 3;
            // 
            // Label9
            // 
            this.Label9.Location = new System.Drawing.Point(10, 102);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(57, 18);
            this.Label9.TabIndex = 2;
            this.Label9.Text = "boolVal:";
            // 
            // tbComplexStruct_IntVal
            // 
            this.tbComplexStruct_IntVal.Location = new System.Drawing.Point(77, 31);
            this.tbComplexStruct_IntVal.Name = "tbComplexStruct_IntVal";
            this.tbComplexStruct_IntVal.Size = new System.Drawing.Size(230, 22);
            this.tbComplexStruct_IntVal.TabIndex = 1;
            // 
            // Label10
            // 
            this.Label10.Location = new System.Drawing.Point(10, 31);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(48, 19);
            this.Label10.TabIndex = 0;
            this.Label10.Text = "intVal:";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.tbStr2);
            this.GroupBox2.Controls.Add(this.Label7);
            this.GroupBox2.Controls.Add(this.tbStr1);
            this.GroupBox2.Controls.Add(this.Label8);
            this.GroupBox2.Location = new System.Drawing.Point(10, 194);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(201, 101);
            this.GroupBox2.TabIndex = 8;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "String Types";
            // 
            // tbStr2
            // 
            this.tbStr2.Location = new System.Drawing.Point(58, 65);
            this.tbStr2.Name = "tbStr2";
            this.tbStr2.Size = new System.Drawing.Size(124, 22);
            this.tbStr2.TabIndex = 3;
            // 
            // Label7
            // 
            this.Label7.Location = new System.Drawing.Point(10, 67);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(48, 18);
            this.Label7.TabIndex = 2;
            this.Label7.Text = "str2:";
            // 
            // tbStr1
            // 
            this.tbStr1.Location = new System.Drawing.Point(58, 28);
            this.tbStr1.Name = "tbStr1";
            this.tbStr1.Size = new System.Drawing.Size(124, 22);
            this.tbStr1.TabIndex = 1;
            // 
            // Label8
            // 
            this.Label8.Location = new System.Drawing.Point(10, 31);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(48, 19);
            this.Label8.TabIndex = 0;
            this.Label8.Text = "str1:";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.tblreal1);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.tbUsint1);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.tbDint1);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.tbBool1);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Location = new System.Drawing.Point(10, 9);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(201, 176);
            this.GroupBox1.TabIndex = 7;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Primitive Types";
            // 
            // tblreal1
            // 
            this.tblreal1.Location = new System.Drawing.Point(58, 138);
            this.tblreal1.Name = "tblreal1";
            this.tblreal1.Size = new System.Drawing.Size(124, 22);
            this.tblreal1.TabIndex = 11;
            // 
            // Label6
            // 
            this.Label6.Location = new System.Drawing.Point(10, 138);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(48, 19);
            this.Label6.TabIndex = 10;
            this.Label6.Text = "lreal1:";
            // 
            // tbUsint1
            // 
            this.tbUsint1.Location = new System.Drawing.Point(58, 102);
            this.tbUsint1.Name = "tbUsint1";
            this.tbUsint1.Size = new System.Drawing.Size(124, 22);
            this.tbUsint1.TabIndex = 7;
            // 
            // Label4
            // 
            this.Label4.Location = new System.Drawing.Point(10, 102);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(48, 18);
            this.Label4.TabIndex = 6;
            this.Label4.Text = "usint1:";
            // 
            // tbDint1
            // 
            this.tbDint1.Location = new System.Drawing.Point(58, 67);
            this.tbDint1.Name = "tbDint1";
            this.tbDint1.Size = new System.Drawing.Size(124, 22);
            this.tbDint1.TabIndex = 3;
            // 
            // Label2
            // 
            this.Label2.Location = new System.Drawing.Point(10, 67);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(48, 18);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "dint1:";
            // 
            // tbBool1
            // 
            this.tbBool1.Location = new System.Drawing.Point(58, 31);
            this.tbBool1.Name = "tbBool1";
            this.tbBool1.Size = new System.Drawing.Size(124, 22);
            this.tbBool1.TabIndex = 1;
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(10, 31);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(48, 19);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "bool1:";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(707, 340);
            this.Controls.Add(this.btnDeleteNotifications);
            this.Controls.Add(this.btnAddNotifications);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.GroupBox3);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox1);
            this.Name = "Form1";
            this.Text = "Sample14";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{		
			Application.Run(new Form1());
		}
		private void Form1_Load(object sender, System.EventArgs e)
		{
			adsClient = new TcAdsClient();
			notificationHandles = new ArrayList();
			try
			{
				adsClient.Connect(851);
				adsClient.AdsNotificationEx+=new AdsNotificationExEventHandler(adsClient_AdsNotificationEx);
				btnDeleteNotifications.Enabled = false;
				//create handles for the PLC variables;
				hbool1 = adsClient.CreateVariableHandle("MAIN.bool1");
				hdint1 = adsClient.CreateVariableHandle("MAIN.dint1");
				husint1 = adsClient.CreateVariableHandle("MAIN.usint1");
				hlreal1 = adsClient.CreateVariableHandle("MAIN.lreal1");
				hstr1 = adsClient.CreateVariableHandle("MAIN.str1");
				hstr2 = adsClient.CreateVariableHandle("MAIN.str2");
				hcomplexStruct = adsClient.CreateVariableHandle("MAIN.ComplexStruct1");
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

        private void ReadWriteAnyType()
        {


            #region CODE_READWRITEANY

            using (TcAdsClient client = new TcAdsClient())
            {
                client.Connect("1.2.3.4.5.6", 851);

                // Bool value
                bool boolValue = (bool)client.ReadSymbol("MAIN.bool1", typeof(bool), false);
                adsClient.WriteSymbol("MAIN.bool1", boolValue,false);

                // or
                int handle1 = adsClient.CreateVariableHandle("MAIN.bool1"); // BOOL
                boolValue = (bool)client.ReadAny(handle1, typeof(bool));
                adsClient.WriteAny(handle1, boolValue);
                adsClient.DeleteVariableHandle(handle1);

                // RealValue
                int handle2 = adsClient.CreateVariableHandle("MAIN.bool1"); // BOOL
                float realValue = (float)client.ReadAny(handle2, typeof(float)); // REAL
                client.WriteAny(handle2, realValue);
                adsClient.DeleteVariableHandle(handle2);

                // String
                int handle3 = adsClient.CreateVariableHandle("MAIN.string1"); // STRING[80]
                string stringValue = (string)client.ReadAny(handle3, typeof(string), new int[] { 80 }); // Needs additional para for strlen
                adsClient.WriteAny(handle3, stringValue, new int[] { 80 });

                // ushort[]
                int handle4 = adsClient.CreateVariableHandle("MAIN.uint1Arr"); // ARRAY [0..9] OF UINT
                ushort[] ushortArr = (ushort[])client.ReadAny(handle4, typeof(ushort[]), new int[] { 10 });
                adsClient.WriteAny(handle4, ushortArr, new int[] { 10 });
                adsClient.DeleteVariableHandle(handle4);

                // Complex Struct Type
                // Take care the the corresponding .NET Type is blittable / marshallable to the PLC type
                int handle5 = adsClient.CreateVariableHandle("MAIN.struct");
                PlcStruct structValue = (PlcStruct)adsClient.ReadAny(handle5, typeof(PlcStruct));
                adsClient.WriteAny(handle5, typeof(PlcStruct));
                adsClient.DeleteVariableHandle(handle5);

                // ARRAY [0..9] OF STRING[80]
                // args[0] --> Number of Characters
                // args[1] --> Number of Array Elements
                // Needs additional para for strlen and number of Elements in Array
                int handle6 = adsClient.CreateVariableHandle("MAIN.stringArr"); // ARRAY [0..9] OF STRING[80]
                string[] stringArr = (string[])client.ReadAny(handle6, typeof(string[]), new int[] { 80, 10 });
                adsClient.WriteAny(handle6, stringValue, new int[] { 80 });
                adsClient.DeleteVariableHandle(handle6);

            }
            #endregion
        }

        #region CODE_READWRITEANY2

        // Attention: Dependent of the System where the PLC runs, the StructLayout of the exchanged
        // Structures must match. With the ANY_TYPE concept this is realized with 'blittable' objects,
        // that match on .NET and PLC side.

        // Default Pack Modes:

        // TC3 I64/x86: Normal, in this case Pack = 8
        // TC2 x86:     Pack = 1

        // On TC3 PLC side we can force the packing of structures with the attribute
        // {attribute 'pack_mode' := '1'}, see also 'pack_mode' attribute in Beckhoff InfoSystem
        // For TC2 is the Pack setting Pack = 1 the only possible way, because it is not selectable.

        // We have to ensure that the pack mode on both sides is equal!

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct PlcStruct
        {
            // Type must be 'blittable' to the corresponding PLC Struct Type
            // See MSDN for MarshalAs and Default Marshalling.

            [MarshalAs(UnmanagedType.I1)]
            public bool boolVal; // BOOL
            public byte byteVal; // BYTE
            public ushort ushortVal; // UINT
            public short shortVal; // INT
            public uint uintVal; // UDINT
            public int dintVal; // DINT
            public uint udintVal; // UDINT
            public float realVal; // REAL
            public double lrealVal; // LREAL
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
            public string stringVal; // STRING[80]

            [MarshalAs(UnmanagedType.U4)]
            public uint timeVal; // TIME
            [MarshalAs(UnmanagedType.U4)]
            public uint todVal; // TOD
            [MarshalAs(UnmanagedType.U4)]
            public uint dateval; // DATE
            [MarshalAs(UnmanagedType.U4)]
            public uint dtVal; // DT
        }
        #endregion

        void RegisterAnyNotifications()
        {
            #region CODE_READWRITEANYNOT

            using (TcAdsClient client = new TcAdsClient())
            {
                client.AdsNotificationEx += Client_AdsNotificationEx; 
                client.Connect("1.2.3.4.5.6", 851);

                // Add UDINT
                int notificationHandle = client.AddDeviceNotificationEx("MAIN.udint", AdsTransMode.OnChange, 200, 200, null, typeof(uint));
                Thread.Sleep(5000); // ...
                client.DeleteDeviceNotification(notificationHandle); // Unregister Event
            }
            #endregion
        }
        #region CODE_READWRITEANYNOT2
        private void Client_AdsNotificationEx(object sender, AdsNotificationExEventArgs e)
        {
            uint value = (uint)e.Value; // Marshalled value as .NET Type
        }
        #endregion

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			adsClient.Dispose();
		}

        #region CODE_SAMPLE_NOTIFICATIONEX

        //AdsStream readStream = new AdsStream(sizeof(UInt32));

        private void ReceiveNotifications()
        {
            using (TcAdsClient client = new TcAdsClient())
            {
                // Add the Notification event 'Ex' handler
                client.AdsNotificationEx += Client_AdsNotification;

                // Connect to target
                client.Connect(AmsNetId.Local, 851);
                int notificationHandle = 0;

                try
                {
                    // Notification to a ZDINT Type (UINT32)
                    // Check for change every 200 ms
                    notificationHandle = client.AddDeviceNotificationEx("MAIN.nCounter",AdsTransMode.OnChange, 200, 0, null,typeof(uint));
                    Thread.Sleep(5000); // Sleep the main thread to get some (asynchronous Notifications)
                }
                finally
                {
                    // Unregister the Event / Handle
                    client.DeleteDeviceNotification(notificationHandle);
                    client.AdsNotificationEx -= Client_AdsNotification;
                }
            }
        }

        private void Client_AdsNotification(object sender, AdsNotificationExEventArgs e)
        {
            // Or here we know about UDINT type --> can be marshalled as UINT32
            uint nCounter = (uint)e.Value;

            // If Synchronization is needed (e.g. in Windows.Forms or WPF applications)
            // we could synchronize via SynchronizationContext into the UI Thread

            /*SynchronizationContext syncContext = SynchronizationContext.Current;
              _context.Post(status => someLabel.Text = nCounter.ToString(), null); // Non-blocking post */
        }
        #endregion


        private void btnRead_Click(object sender, System.EventArgs e)
		{
			try
			{           
                //read by handle
				//the second parameter specifies the type of the variable
				tbDint1.Text = adsClient.ReadAny(hdint1, typeof(int)).ToString();
				tbUsint1.Text = adsClient.ReadAny(husint1, typeof(Byte)).ToString();
				tbBool1.Text = adsClient.ReadAny(hbool1, typeof(Boolean)).ToString();
				tblreal1.Text = adsClient.ReadAny(hlreal1, typeof(Double)).ToString();
				//with strings one has to additionally pass the number of characters
				//specified in the PLC project(default 80). 
				//This value is passed is an int array.             
				tbStr1.Text = adsClient.ReadAny(hstr1, typeof(String), new int[] {80}).ToString();
				tbStr2.Text = adsClient.ReadAny(hstr2, typeof(String), new int[] {5}).ToString();
				FillStructControls((ComplexStruct)adsClient.ReadAny(hcomplexStruct, typeof(ComplexStruct)));
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnWrite_Click(object sender, System.EventArgs e)
		{
			try
			{
				//write by handle
				//the second parameter is the object to be written to the PLC variable
				adsClient.WriteAny(hdint1, int.Parse(tbDint1.Text));
				adsClient.WriteAny(husint1, Byte.Parse(tbUsint1.Text));
				adsClient.WriteAny(hbool1, Boolean.Parse(tbBool1.Text));
				adsClient.WriteAny(hlreal1, Double.Parse(tblreal1.Text));
				//with strings one has to additionally pass the number of characters
				//the variable has in the PLC(default 80).            
				adsClient.WriteAny(hstr1, tbStr1.Text, new int[] {80});
				adsClient.WriteAny(hstr2, tbStr2.Text, new int[] {5});
				adsClient.WriteAny(hcomplexStruct, GetStructFromControls())	;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnAddNotifications_Click(object sender, System.EventArgs e)
		{
			notificationHandles.Clear();
			try
			{
				//register notification            
				notificationHandles.Add(adsClient.AddDeviceNotificationEx("MAIN.dint1", AdsTransMode.OnChange, 100, 0, tbDint1, typeof(int)));
				notificationHandles.Add(adsClient.AddDeviceNotificationEx("MAIN.usint1", AdsTransMode.OnChange, 100, 0, tbUsint1, typeof(Byte)));
				notificationHandles.Add(adsClient.AddDeviceNotificationEx("MAIN.bool1", AdsTransMode.OnChange, 100, 0, tbBool1, typeof(Boolean)));
				notificationHandles.Add(adsClient.AddDeviceNotificationEx("MAIN.lreal1", AdsTransMode.OnChange, 100, 0, tblreal1, typeof(Double)));
				notificationHandles.Add(adsClient.AddDeviceNotificationEx("MAIN.str1", AdsTransMode.OnChange, 100, 0, tbStr1, typeof(String), new int[] {80}));
				notificationHandles.Add(adsClient.AddDeviceNotificationEx("MAIN.str2", AdsTransMode.OnChange, 100, 0, tbStr2, typeof(String), new int[] {5}));
				notificationHandles.Add(adsClient.AddDeviceNotificationEx("MAIN.complexStruct1", AdsTransMode.OnChange, 100, 0, tbDint1, typeof(ComplexStruct)));
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			btnDeleteNotifications.Enabled = true;
			btnAddNotifications.Enabled = false;
		}

		private void btnDeleteNotifications_Click(object sender, System.EventArgs e)
		{
			//delete registered notifications.
			try
			{
				foreach(int handle in notificationHandles)
					adsClient.DeleteDeviceNotification(handle);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			notificationHandles.Clear();
			btnAddNotifications.Enabled = true;
			btnDeleteNotifications.Enabled = false;
		}

		private void adsClient_AdsNotificationEx(object sender, AdsNotificationExEventArgs e)
		{			 
			TextBox textBox = (TextBox)e.UserData;
			Type type = e.Value.GetType();
			if(type == typeof(string) || type.IsPrimitive)
				textBox.Text = e.Value.ToString();
			else if(type == typeof(ComplexStruct))
				FillStructControls((ComplexStruct)e.Value);        
		}

		private void FillStructControls(ComplexStruct structure)
		{
			tbComplexStruct_IntVal.Text = structure.intVal.ToString();
			tbComplexStruct_dintArr.Text = String.Format("{0:d}, {1:d}, {2:d}, {3:d}", structure.dintArr[0],
				structure.dintArr[1], structure.dintArr[2], structure.dintArr[3]);
			tbComplexStruct_boolVal.Text = structure.boolVal.ToString();
			tbComplexStruct_ByteVal.Text = structure.byteVal.ToString();
			tbComplexStruct_stringVal.Text = structure.stringVal;
			tbComplexStruct_SimpleStruct1_lrealVal.Text = structure.simpleStruct1.lrealVal.ToString();
			tbComplexStruct_SimpleStruct_dintVal.Text = structure.simpleStruct1.dintVal1.ToString();
		}

		private ComplexStruct GetStructFromControls()
		{
			ComplexStruct structure = new ComplexStruct();
			String[] stringArr = tbComplexStruct_dintArr.Text.Split(new char[] {','});
			structure.intVal = short.Parse(tbComplexStruct_IntVal.Text);
			for(int i=0; i<stringArr.Length; i++)
				structure.dintArr[i] = int.Parse(stringArr[i]);
        
			structure.boolVal = Boolean.Parse(tbComplexStruct_boolVal.Text);
			structure.byteVal = Byte.Parse(tbComplexStruct_ByteVal.Text);
			structure.stringVal = tbComplexStruct_stringVal.Text;
			structure.simpleStruct1.dintVal1 = int.Parse(tbComplexStruct_SimpleStruct_dintVal.Text);
			structure.simpleStruct1.lrealVal = double.Parse(tbComplexStruct_SimpleStruct1_lrealVal.Text);
			return structure;
		}		
	}

	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public class SimpleStruct
	{
		public double lrealVal;
		public int dintVal1;
	}

	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public class ComplexStruct
	{
		public short intVal;
		//specifies how .NET should marshal the array
		//SizeConst specifies the number of elements the array has.
		[MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
		public int[] dintArr = new int[4];
		[MarshalAs(UnmanagedType.I1)]
		public bool boolVal;
		public byte byteVal;
		//specifies how .NET should marshal the string
		//SizeConst specifies the number of characters the string has.
		//'(inclusive the terminating null ). 
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst=6)]
		public string stringVal = "";
		public SimpleStruct simpleStruct1 =new SimpleStruct();
	}
}
#endregion

