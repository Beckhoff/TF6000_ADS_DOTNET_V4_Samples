﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwinCAT.Ads;

namespace _30_ADS.NET_MethodCall
{

    #region CODE_SAMPLE
    public partial class Form1 : Form
    {
        TcAdsClient tcClient;
        AdsStream readStream, writeStream;
        AdsBinaryReader binReader;
        AdsBinaryWriter binWriter;
        int hMethod = 0;

        public Form1()
        {
            //Create a new instance of class TcAdsClient
            tcClient = new TcAdsClient();
            //Allocate memory [2 bytes] for read parameter (sum in this case)
            readStream = new AdsStream(sizeof(int));
            binReader = new AdsBinaryReader(readStream);
            //Allocate memory [4 bytes] for write parameter (summands A and B in this case)
            writeStream = new AdsStream(2 * sizeof(int));
            binWriter = new AdsBinaryWriter(writeStream);
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //Connect to local PLC - Port 851
                tcClient.Connect(851);

                //Get the handle of the PLC method "Addition" of POU MAIN.fbMath
                hMethod = tcClient.CreateVariableHandle("MAIN.fbMath#Addition");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnMethodCall_Click(object sender, EventArgs e)
        {
            try
            {
                //Write value A to stream
                binWriter.Write(Convert.ToInt32(tbValueA.Text));
                //Set stream position for next value
                writeStream.Position = sizeof(int);
                //Write value B to stream
                binWriter.Write(Convert.ToInt32(tbValueB.Text));
                
                //Call PLC method via handle
                tcClient.ReadWrite((uint)AdsReservedIndexGroups.SymbolValueByHandle, (uint)hMethod, readStream, writeStream);
                
                //Read sum
                tbSumAB.Text = Convert.ToString(binReader.ReadInt32());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Reset stream position
                readStream.Position = 0;
                writeStream.Position = 0;
            }
        }
        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            try
            {
                tcClient.DeleteVariableHandle(hMethod);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                tcClient.Dispose();
            }
        }
    }
    #endregion
}
