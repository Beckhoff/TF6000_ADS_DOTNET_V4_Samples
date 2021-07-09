using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using TwinCAT.Ads;
using TwinCAT.PlcOpen;

namespace Sample11
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnWrite;
		private System.Windows.Forms.Button btnRead;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox2;
		private TcAdsClient adsClient;
		private int[] varHandles;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			InitializeComponent();
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
			this.label1 = new System.Windows.Forms.Label();
			this.btnWrite = new System.Windows.Forms.Button();
			this.btnRead = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 21);
			this.label1.TabIndex = 7;
			this.label1.Text = "MAIN.Time1:";
			// 
			// btnWrite
			// 
			this.btnWrite.Location = new System.Drawing.Point(240, 48);
			this.btnWrite.Name = "btnWrite";
			this.btnWrite.Size = new System.Drawing.Size(72, 24);
			this.btnWrite.TabIndex = 6;
			this.btnWrite.Text = "Write";
			this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
			// 
			// btnRead
			// 
			this.btnRead.Location = new System.Drawing.Point(240, 16);
			this.btnRead.Name = "btnRead";
			this.btnRead.Size = new System.Drawing.Size(72, 24);
			this.btnRead.TabIndex = 5;
			this.btnRead.Text = "Read";
			this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(80, 16);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(144, 20);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 21);
			this.label2.TabIndex = 9;
			this.label2.Text = "MAIN.Date1:";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(80, 48);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(144, 20);
			this.textBox2.TabIndex = 8;
			this.textBox2.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(320, 85);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnWrite);
			this.Controls.Add(this.btnRead);
			this.Controls.Add(this.textBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			this.Load += new System.EventHandler(this.Form1_Load);
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
			try
			{
				adsClient = new TcAdsClient();
				adsClient.Connect(851);
				varHandles = new int[2];
				varHandles[0] = adsClient.CreateVariableHandle("MAIN.Time1");
				varHandles[1] = adsClient.CreateVariableHandle("MAIN.Date1");
			}
			catch( Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				adsClient.Dispose();
			}
			catch( Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

		private void btnRead_Click(object sender, System.EventArgs e)
		{
			try
			{
				AdsStream adsStream = new AdsStream(4);
				AdsBinaryReader reader = new AdsBinaryReader(adsStream);				
				adsClient.Read(varHandles[0], adsStream);
				textBox1.Text = reader.ReadPlcTIME().ToString();
				
				adsStream.Position = 0;
				adsClient.Read(varHandles[1], adsStream);
				textBox2.Text = reader.ReadPlcDATE().ToString();
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
				AdsStream adsStream = new AdsStream(4);
				AdsBinaryWriter writer = new AdsBinaryWriter(adsStream);								
				writer.WritePlcType(TimeSpan.Parse(textBox1.Text));				
				adsClient.Write(varHandles[0], adsStream);
				
				adsStream.Position = 0;
				writer.WritePlcType(DateTime.Parse(textBox2.Text));				
				adsClient.Write(varHandles[1], adsStream);
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}


        private void ReadWritePlcOpenTypes()
        {
            #region CODE_SAMPLE_PLCOPEN_STREAMED
            using (TcAdsClient client = new TcAdsClient())
            {
                client.Connect(851); // Connect to local plc

                int handle1 = 0;
                int handle2 = 0;

                try
                {
                    handle1 = client.CreateVariableHandle("MAIN.time"); // TIME
                    handle2 = client.CreateVariableHandle("MAIN.date"); // DATE

                    AdsStream readStream = new AdsStream(LTIME.MarshalSize); // Largest (8 Bytes)
                    AdsBinaryReader reader = new AdsBinaryReader(readStream);

                    client.Read(handle1, readStream, 0, TIME.MarshalSize);
                    TimeSpan time = reader.ReadPlcTIME();

                    client.Read(handle2, readStream, 0, DATE.MarshalSize);
                    DateTime dateTime = reader.ReadPlcDATE();
                }
                finally
                {
                    client.DeleteVariableHandle(handle1);
                    client.DeleteVariableHandle(handle2);
                }
            }
            #endregion
        }

        private void ReadWritePlcOpenTypesAny()
        {
            #region CODE_SAMPLE_PLCOPEN_ANY
            using (TcAdsClient client = new TcAdsClient())
            {
                client.Connect(851); // Connect to local plc

                int handle1 = 0;
                int handle2 = 0;
                int handle3 = 0;

                try
                {
                    handle1 = client.CreateVariableHandle("MAIN.time"); // TIME
                    handle2 = client.CreateVariableHandle("MAIN.date"); // DATE
                    handle3 = client.CreateVariableHandle("MAIN.ltime"); // LTIME

                    TIME time = (TIME)client.ReadAny(handle1, typeof(TIME)); // TIME
                    TimeSpan timeSpan = time.Time;

                    DATE date = (DATE)client.ReadAny(handle2, typeof(DATE)); // DATE
                    DateTime dateTime = date.Date;

                    LTIME ltime = (LTIME)client.ReadAny(handle3, typeof(LTIME)); // LTIME
                    TimeSpan lTimeSpan = ltime.Time;
                }
                finally
                {
                    client.DeleteVariableHandle(handle1);
                    client.DeleteVariableHandle(handle2);
                    client.DeleteVariableHandle(handle3);
                }
            }
            #endregion
        }
    }
}
