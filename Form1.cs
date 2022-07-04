using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft;
using Microsoft.Win32;

namespace ssfnloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            WebClient client0 = new WebClient();
            byte[] buffer0 = client0.DownloadData("https://teiku.moe/notice.txt");
            string quotes = Encoding.GetEncoding("GBK").GetString(buffer0);
            quote.Text = quotes;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ssfn = textBox1.Text;
            RegistryKey hkCu = Microsoft.Win32.Registry.CurrentUser;
            RegistryKey hkSoftware = hkCu.OpenSubKey("Software");
            RegistryKey hkValve = hkSoftware.OpenSubKey("Valve");
            RegistryKey hkSteam = hkValve.OpenSubKey("Steam");
            string steampath = hkSteam.GetValue("SteamPath").ToString()+"\\";
            status.Text = "Get Steam Path at " + steampath;
            string url = "https://ssfnbox.com/download/" + ssfn;
            string save = steampath + ssfn;
            string cfg = steampath + "config\\config.vdf";
            string diacfg = steampath + "config\\dialogconfig.vdf";
            if (ssfn == "") {
                status.Text = "Please type in ssfn!";
            }
            else {
                status.Text = "Delete config.vdf/dialogconfig.vdf...";
                File.Delete(cfg);
                status.Text = "Succeed";
                status.Text = "Delete ssfn...";
                var files = new DirectoryInfo(steampath).GetFiles("ssfn" + "*");
                foreach (var fileInfo in files)
                {
                    if (fileInfo.IsReadOnly)
                    {
                        fileInfo.Attributes = FileAttributes.Normal;
                    }
                    fileInfo.Delete();
                }
                status.Text = "Succeed";
                status.Text = "Fetch ssfn from ssfnbox.com...";
                dld(url, save);
                Thread.Sleep(1000);
                FileInfo checkfile = new FileInfo(save);
                if (checkfile.Length == 0)
                {
                    status.Text = "Fetch failed due to time out!";
                }
                else {
                    status.Text = "Succeed";
                    status.Text = "Finished";
                }
                
            }
        }
        static void dld(string dlurl, string dlpath)
        {
            using (WebClient client1 = new WebClient())
            {
                client1.Headers.Add("a", "a");
                try
                {
                    client1.DownloadFile(dlurl, dlpath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());//idk what is this but paste here together
                }
            }
        }
    }
}
