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

namespace Socket通信_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            testJson();
            //testb64();
        }
        Socket socketSend;
        private void btnStart_Click(object sender, EventArgs e)
        {
            //创建负责通信的Socket
            socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(txtServer.Text);
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(txtPort.Text));
            //获得要连接的远程服务器应用程序的IP地址和端口号
            socketSend.Connect(point);
            ShowMsg("连接成功");

            Thread th = new Thread(Recive);
            th.IsBackground = true;
            th.Start();
        }
        void ShowMsg(string str)
        {
            txtLog.AppendText(str + "\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string str = txtMsg.Text.Trim();
            string str = SendImage();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            Console.WriteLine("buffer" + buffer.Count());
            socketSend.Send(buffer);
        }
        /// <summary>
        /// 不停的接收服务器发来的消息
        /// </summary>
        void Recive()
        {
            while (true)
            {
                byte[] buffer = new byte[5120 * 5120 * 4];
                int r = socketSend.Receive(buffer);
                //实际接收到的有效字节数
                if (r > 0)
                {
                    try
                    {
                        string s = Encoding.UTF8.GetString(buffer, 0, r);
                        RecvMessageStructure rms = new RecvMessageStructure();
                        rms = (RecvMessageStructure)JsonConvert.DeserializeObject<RecvMessageStructure>(s);
                        Bitmap heatmap = base642Bitmap(rms.b64Image);
                        copyImageBox.Image = heatmap;
                        txtMsg.Text = rms.path;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }
            }
        }
        /// <summary>
        /// 震动
        /// </summary>
        void ZD()
        {
            Point p = this.Location;
            int x = p.X;
            int y = p.Y;
            int j;
            for (int i = 0; i < 200; i++)
            {
                this.Location = new Point(x, y + 5);
                for (j = 0; j < 10000; j++) ;
                this.Location = new Point(x + 5, y + 5);
                for (j = 0; j < 10000; j++) ;
                this.Location = new Point(x + 5, y);
                for (j = 0; j < 10000; j++) ;
                this.Location = new Point(x, y + 5);
                for (j = 0; j < 10000; j++) ;
            }
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

        private void testJson()
        {
            List<Person> List = new List<Person>();
            Person person = null;

            for(int i = 0; i < 10; i++)
            {
                person = new Person();
                person.Name = string.Format("xxxx{0}", i);
                person.Age = 20 + i;
                person.Birthday = DateTime.Now.AddDays(i);
                person.Six = i % 2 == 0 ? "man" : "woman";
                List.Add(person);
            }

            string serialStr = JsonConvert.SerializeObject(List);
            Console.WriteLine(serialStr);
            List<Person> DeList = new List<Person>();
            DeList = (List<Person>)JsonConvert.DeserializeObject<List<Person>>(serialStr);
            Console.WriteLine(DeList[1].Name + DeList[0].Birthday.ToString());
        }

        private string SendImage()
        {
            SendMessage sm = new SendMessage();
            sm.imageName = "yangjian123.jpg";
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
                Process proc = new Process();
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

    //public class JointPoint
    //{
    //    public List<double> Reye { get; set; }
    //    public List<double> Rsho { get; set; }
    //}

    //public class ReturnAngle
    //{
    //    public double shoulder { get; set; }
    //    public double head { get; set; }
    //    public double eye { get; set; }
    //}

    //public class RecvMessageStructure
    //{
    //    public JointPoint jointPoint { get; set; }
    //    public ReturnAngle returnAngle { get; set; }
    //    public string imageCount { get; set; }
    //}
    public class ReturnRectItem
    {
        /// <summary>
        /// 
        /// </summary>
        public float c { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float s { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float w { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float h { get; set; }
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
        public List<ReturnRectItem> returnRect { get; set; }

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

    public class Person
    {
        public string Name
        {
            get;
            set;
        }

        public int Age
        {
            get;
            set;
        }

        public string Six
        {
            set;
            get;
        }
        public DateTime Birthday
        {
            get;
            set;
        }
    }
}
