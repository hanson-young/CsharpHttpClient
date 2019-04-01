using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections;

namespace HttpClient
{
    public partial class client_form : Form
    {
        public client_form()
        {
            InitializeComponent();
            //testJson();
            //testb64();
        }
        Process proc = new Process();
        private void send_Click(object sender, EventArgs e)
        {
            //string str = txtMsg.Text.Trim();
            string url = "http://127.0.0.1:8087/particle/heatmap";
            string post_str = SendImage();
            //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            //Console.WriteLine("buffer" + buffer.Count());
            string re_data = HttpPost(url, post_str);
            //this.txtMsg.Text = re_data;

            RecvMessageStructure rms = new RecvMessageStructure();
            rms = (RecvMessageStructure)JsonConvert.DeserializeObject<RecvMessageStructure>(re_data);

            imageBox.Image = base642Bitmap(rms.b64Image);
            this.txtMsg.Text = rms.path;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }


        private static string bitmap2Base64(Bitmap bmp)
        {
            string base64String = null;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            base64String = Convert.ToBase64String(byteImage);
            return base64String;
        }

        private static Bitmap base642Bitmap(string base64String)
        {
            Bitmap bmp = null;
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);
            memoryStream.Position = 0;
            bmp = (Bitmap)Bitmap.FromStream(memoryStream);

            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;
            return bmp;

        }

        private void testb64()
        {
            Bitmap bmp = new Bitmap(this.imageBox.Image);
            string b64String = bitmap2Base64(bmp);
            //Console.WriteLine(b64String);
            this.copyImageBox.Image = base642Bitmap(b64String);
        }

        private string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);

            using (Stream writeStream = request.GetRequestStream())
            {
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(postDataStr);
                writeStream.Write(bytes, 0, bytes.Length);
            }

            //Stream _stream = request.GetRequestStream();
            //StreamWriter _writer = new StreamWriter(_stream, Encoding.ASCII);
            //_writer.Write(postDataStr);
            //_writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream _rstream = response.GetResponseStream();
            StreamReader _sreader = new StreamReader(_rstream, Encoding.GetEncoding("utf-8"));
            string retString = _sreader.ReadToEnd();
            _sreader.Close();
            _rstream.Close();

            return retString;
        }

        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        private string SendImage()
        {
            SendMessage sm = new SendMessage();
            sm.imageName = "test.jpg";
            sm.imageCount = 31;
            sm.detecteItem = "forward";
            sm.detecteType = "static";
            sm.detecteStatus = "prepredict";
            Bitmap bmp = new Bitmap(this.imageBox.Image);
            //Bitmap bmp = (Bitmap)Image.FromFile(@"G:\win_setup\WeChat\WeChat Files\WeChat Files\Don_Not_Panic\Files\20181207012726\20181207012726.bmp");
            //BitmapData _BitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            //byte[] _Value = new byte[_BitmapData.Stride * _BitmapData.Height];
            //string b64String = bitmap2Base64(bmp);
            sm.b64Image = bitmap2Base64(bmp);
            sm.timeStamp = DateTime.Now.ToString();
            string serialStr = JsonConvert.SerializeObject(sm);
            return serialStr;
        }

        public void setEnvPath(string path)
        {
            string pathlist;
            pathlist = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
            string[] list = pathlist.Split(';');
            bool isPathExist = false;

            foreach (string item in list)
            {
                if (item == path)
                    isPathExist = true;

            }
            if (!isPathExist)
            {
                Environment.SetEnvironmentVariable("PATH", pathlist + ";" + path, EnvironmentVariableTarget.Machine);
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            proc.CloseMainWindow();
        }
        private void start_Click(object sender, EventArgs e)
        {
            try
            {
                setEnvPath("C:\\Users\\win10\\Anaconda3");
                setEnvPath("C:\\Users\\win10\\Anaconda3\\Library\\bin");
                setEnvPath("C:\\Users\\win10\\Anaconda3\\Scripts");
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                foreach (DictionaryEntry de in environmentVariables)
                {
                    Console.WriteLine("  {0} = {1}", de.Key, de.Value);
                }
                string str = System.Windows.Forms.Application.StartupPath + "\\start_server.bat";

                string strDirPath = System.IO.Path.GetDirectoryName(str);
                string strFilePath = System.IO.Path.GetFileName(str);

                string targetDir = string.Format(strDirPath);//this is where mybatch.bat lies
                
                proc.StartInfo.WorkingDirectory = targetDir;
                proc.StartInfo.FileName = strFilePath;

                //proc.StartInfo.CreateNoWindow = true;
                //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.Start();
                //proc.WaitForExit();


                //MessageBox.Show("执行成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("执行失败 错误原因:" + ex.Message);
            }
        }
    }

    public class RecvMessageStructure
    {
        /// <summary>
        /// 
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int imageCount;

        public string b64Image;
    }

    public class SendMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public string imageName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int imageCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string detecteStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string detecteType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string detecteItem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string timeStamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string b64Image { get; set; }
    }
}
