using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using TwinCAT.Ads;
using System.Text;

namespace Sample10
{
	public class Form1 : System.Windows.Forms.Form
	{
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnRead;
		private System.Windows.Forms.Button btnWrite;
		private System.Windows.Forms.Label label1;
		private TcAdsClient adsClient;
		private int varHandle;

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
            this.btnWrite = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(86, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(183, 22);
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
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(288, 55);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(86, 28);
            this.btnWrite.TabIndex = 2;
            this.btnWrite.Text = "Write";
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "MAIN.text:";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(377, 89);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Sample10";
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
				varHandle = adsClient.CreateVariableHandle("MAIN.text");
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

		private void btnRead_Click(object sender, System.EventArgs e)
		{
			try
			{
				AdsStream adsStream = new AdsStream(30);
				AdsBinaryReader reader = new AdsBinaryReader(adsStream);				
				adsClient.Read(varHandle, adsStream);
				textBox1.Text = reader.ReadPlcAnsiString(30);
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
				AdsStream adsStream = new AdsStream(30);
				AdsBinaryWriter writer = new AdsBinaryWriter(adsStream);								
				writer.WritePlcAnsiString(textBox1.Text, 30);				
				adsClient.Write(varHandle, adsStream);
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

        private void CodeSampleReadString()
        {
            #region CODE_SAMPLE_STRING
            using (TcAdsClient client = new TcAdsClient())
            {
                client.Connect(851); // Connect to local port 851 (PLC)

                int handle = client.CreateVariableHandle("MAIN.string"); // Symbol "string" in MAIN defined as string

                try
                {
                    // Read ANSI String string[80]
                    int byteSize = 81; // Size of 80 ANSI chars + /0 (STRING[80])
                    AdsStream readStream = new AdsStream(byteSize); // Size of 80 ANSI chars + /0 (STRING[80])
                    AdsBinaryReader reader = new AdsBinaryReader(readStream);
                    client.Read(handle, readStream, 0, byteSize); // Read 81 bytes
                    string value = reader.ReadPlcString(byteSize,Encoding.Default);

                    // Write ANSI String string[80]
                    AdsStream writeStream = new AdsStream(byteSize);
                    AdsBinaryWriter writer = new AdsBinaryWriter(writeStream);
                    value = "Changed";
                    writer.WritePlcString(value, 80,Encoding.Default); // Max 80 characters!
                    client.Write(handle, writeStream, 0, byteSize);
                }
                finally
                {
                    client.DeleteVariableHandle(handle);
                }
            }
            #endregion
        }

        private void CodeSampleReadWString()
        {
            #region CODE_SAMPLE_WSTRING
            using (TcAdsClient client = new TcAdsClient())
            {
                client.Connect(851); // Connect to local port 851 (PLC)

                int handle = client.CreateVariableHandle("MAIN.wstring"); // Symbol "wstring" defined in MAIN as WSTRING

                try
                {
                    // Read UNICODE String wstring[80]
                    int byteSize = 2 * 81; // Size of 80 UNICODE chars + /0 (WSTRING[80])
                    AdsStream readStream = new AdsStream(byteSize); 
                    AdsBinaryReader reader = new AdsBinaryReader(readStream);
                    client.Read(handle, readStream, 0, byteSize); // Read 2*81 bytes
                    string value = reader.ReadPlcString(byteSize,Encoding.Unicode);

                    // Write ANSI String string[80]
                    AdsStream writeStream = new AdsStream(byteSize);
                    AdsBinaryWriter writer = new AdsBinaryWriter(writeStream);
                    value = "Changed";
                    writer.WritePlcString(value, 80,Encoding.Unicode); 
                    client.Write(handle, writeStream, 0, byteSize);
                }
                finally
                {
                    client.DeleteVariableHandle(handle);
                }
            }
            #endregion
        }

        private void CodeSampleStringAny()
        {
            #region CODE_SAMPLE_ANYSTRING
            using (TcAdsClient client = new TcAdsClient())
            {
                client.Connect(851); // Connect to local port 851 (PLC)

                int stringHandle = client.CreateVariableHandle("MAIN.string"); // Symbol "string" defined in MAIN as STRING
                int wStringHandle = client.CreateVariableHandle("MAIN.wstring"); // Symbol "string" defined in MAIN as WSTRING

                try
                {
                    string str = client.ReadAnyString(stringHandle, 80, Encoding.Default);
                    string wStr = client.ReadAnyString(wStringHandle, 80, Encoding.Unicode);

                    string changedValue = "Changed";

                    // Attention, take care that the memory of the string in the process image is not exceeded!
                    client.WriteAnyString(stringHandle, changedValue, 80, Encoding.Default);
                    client.WriteAnyString(wStringHandle, changedValue, 80, Encoding.Unicode);
                }
                finally
                {
                    client.DeleteVariableHandle(stringHandle);
                    client.DeleteVariableHandle(wStringHandle);
                }
            }
            #endregion
        }
    }
}
