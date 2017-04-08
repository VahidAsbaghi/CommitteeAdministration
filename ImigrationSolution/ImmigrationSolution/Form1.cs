using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ImmigrationSolution
{
    public partial class Form1 : Form
    {
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const int INTERNET_OPTION_REFRESH = 37;
        bool settingsReturn, refreshReturn;
        private  QueueResponse[] _responses;
        private  int[] _elapsedTimes;
        public AppWebProxies _appWebProxies=new AppWebProxies();
        public List<int> _threadsState;
        public List<EventWaitHandle> _threadStateHanlde=new List<EventWaitHandle>();
        public EventWaitHandle _KillThreadWaitHandle = new ManualResetEvent(false);
        private readonly SoftwareValidation _softwareValidation;
        private readonly string _macAddress;
        private static volatile int _progressBarThreadState = 0;
        public  Form1()
        {
            InitializeComponent();
            Thread backgroundThread = new Thread(
                ThreadStart);
            backgroundThread.Start();


            var deviceInfo=new DeviceInfo();
            _macAddress=deviceInfo.GetDeviceId();
            _softwareValidation=new SoftwareValidation();
            //_softwareValidation.SendMacAddress(_macAddress);
            IProxyListProvider proxyListProvider=new ProxyListProviderUs(String.Empty);
            _appWebProxies=proxyListProvider.GetProxyList(true,true);
            //proxyListProvider=new ProxyListProviderHidymy(String.Empty);
            //_appWebProxies.WebProxies.AddRange(proxyListProvider.GetProxyList(true,true).WebProxies);
            if (_appWebProxies.WebProxies.Count < 2)
            {
                _appWebProxies.WebProxies = new List<AppWebProxy>
                {
                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("201.16.140.120", 8080),
                    new AppWebProxy("200.229.194.137", 8080),
                    new AppWebProxy("94.177.254.40", 3128),
                    new AppWebProxy("54.82.233.43", 8083),
                    new AppWebProxy("89.38.149.185", 3128),
                    new AppWebProxy("213.136.77.16", 8080),
                    new AppWebProxy("190.248.128.122", 3128),
                    new AppWebProxy("77.73.66.26", 3128),
                    new AppWebProxy("54.84.109.76", 8083),
                    new AppWebProxy("192.99.98.159", 8080),
                    new AppWebProxy("54.87.138.13", 8083),
                    new AppWebProxy("207.99.118.74", 8080),
                    new AppWebProxy("202.60.195.129", 8080),
                    new AppWebProxy("104.131.30.42", 8080),
                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("198.50.235.188", 8080),
                    new AppWebProxy("200.229.202.147", 8080),
                    new AppWebProxy("54.213.122.80", 8083),
                    new AppWebProxy("185.28.193.95", 8080),

                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("201.16.140.120", 8080),
                    new AppWebProxy("200.229.194.137", 8080),
                    new AppWebProxy("94.177.254.40", 3128),
                    new AppWebProxy("54.82.233.43", 8083),
                    new AppWebProxy("89.38.149.185", 3128),
                    new AppWebProxy("213.136.77.16", 8080),
                    new AppWebProxy("190.248.128.122", 3128),
                    new AppWebProxy("77.73.66.26", 3128),
                    new AppWebProxy("54.84.109.76", 8083),
                    new AppWebProxy("192.99.98.159", 8080),
                    new AppWebProxy("54.87.138.13", 8083),
                    new AppWebProxy("207.99.118.74", 8080),
                    new AppWebProxy("202.60.195.129", 8080),
                    new AppWebProxy("104.131.30.42", 8080),
                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("198.50.235.188", 8080),
                    new AppWebProxy("200.229.202.147", 8080),
                    new AppWebProxy("54.213.122.80", 8083),
                    new AppWebProxy("185.28.193.95", 8080),

                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("201.16.140.120", 8080),
                    new AppWebProxy("200.229.194.137", 8080),
                    new AppWebProxy("94.177.254.40", 3128),
                    new AppWebProxy("54.82.233.43", 8083),
                    new AppWebProxy("89.38.149.185", 3128),
                    new AppWebProxy("213.136.77.16", 8080),
                    new AppWebProxy("190.248.128.122", 3128),
                    new AppWebProxy("77.73.66.26", 3128),
                    new AppWebProxy("54.84.109.76", 8083),
                    new AppWebProxy("192.99.98.159", 8080),
                    new AppWebProxy("54.87.138.13", 8083),
                    new AppWebProxy("207.99.118.74", 8080),
                    new AppWebProxy("202.60.195.129", 8080),
                    new AppWebProxy("104.131.30.42", 8080),
                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("198.50.235.188", 8080),
                    new AppWebProxy("200.229.202.147", 8080),
                    new AppWebProxy("54.213.122.80", 8083),
                    new AppWebProxy("185.28.193.95", 8080),

                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("201.16.140.120", 8080),
                    new AppWebProxy("200.229.194.137", 8080),
                    new AppWebProxy("94.177.254.40", 3128),
                    new AppWebProxy("54.82.233.43", 8083),
                    new AppWebProxy("89.38.149.185", 3128),
                    new AppWebProxy("213.136.77.16", 8080),
                    new AppWebProxy("190.248.128.122", 3128),
                    new AppWebProxy("77.73.66.26", 3128),
                    new AppWebProxy("54.84.109.76", 8083),
                    new AppWebProxy("192.99.98.159", 8080),
                    new AppWebProxy("54.87.138.13", 8083),
                    new AppWebProxy("207.99.118.74", 8080),
                    new AppWebProxy("202.60.195.129", 8080),
                    new AppWebProxy("104.131.30.42", 8080),
                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("198.50.235.188", 8080),
                    new AppWebProxy("200.229.202.147", 8080),
                    new AppWebProxy("54.213.122.80", 8083),
                    new AppWebProxy("185.28.193.95", 8080),

                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("201.16.140.120", 8080),
                    new AppWebProxy("200.229.194.137", 8080),
                    new AppWebProxy("94.177.254.40", 3128),
                    new AppWebProxy("54.82.233.43", 8083),
                    new AppWebProxy("89.38.149.185", 3128),
                    new AppWebProxy("213.136.77.16", 8080),
                    new AppWebProxy("190.248.128.122", 3128),
                    new AppWebProxy("77.73.66.26", 3128),
                    new AppWebProxy("54.84.109.76", 8083),
                    new AppWebProxy("192.99.98.159", 8080),
                    new AppWebProxy("54.87.138.13", 8083),
                    new AppWebProxy("207.99.118.74", 8080),
                    new AppWebProxy("202.60.195.129", 8080),
                    new AppWebProxy("104.131.30.42", 8080),
                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("198.50.235.188", 8080),
                    new AppWebProxy("200.229.202.147", 8080),
                    new AppWebProxy("54.213.122.80", 8083),
                    new AppWebProxy("185.28.193.95", 8080),

                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("201.16.140.120", 8080),
                    new AppWebProxy("200.229.194.137", 8080),
                    new AppWebProxy("94.177.254.40", 3128),
                    new AppWebProxy("54.82.233.43", 8083),
                    new AppWebProxy("89.38.149.185", 3128),
                    new AppWebProxy("213.136.77.16", 8080),
                    new AppWebProxy("190.248.128.122", 3128),
                    new AppWebProxy("77.73.66.26", 3128),
                    new AppWebProxy("54.84.109.76", 8083),
                    new AppWebProxy("192.99.98.159", 8080),
                    new AppWebProxy("54.87.138.13", 8083),
                    new AppWebProxy("207.99.118.74", 8080),
                    new AppWebProxy("202.60.195.129", 8080),
                    new AppWebProxy("104.131.30.42", 8080),
                    new AppWebProxy("103.56.158.196", 3128),
                    new AppWebProxy("198.50.235.188", 8080),
                    new AppWebProxy("200.229.202.147", 8080),
                    new AppWebProxy("54.213.122.80", 8083),
                    new AppWebProxy("185.28.193.95", 8080),
                };
            }

            comboBox1.Items.Add(Request.Method.Post);
            comboBox1.Items.Add(Request.Method.Get);
            _KillThreadWaitHandle.Set();
            //KillTheThread(backgroundThread);
        }

        private void ThreadStart()
        {
            //var message = MessageBox.Show("Please Wait", "",MessageBoxButtons.OK,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification,false) == DialogResult.OK;
            //message = true;
            Form newForm=new Form();
            newForm.Height = 100;
            newForm.Width = 200;
            Label label=new Label();
            label.Name = "labelnX";
            label.Width = 100;
            label.TextAlign=ContentAlignment.MiddleCenter;
            label.Font=new Font(FontFamily.GenericSansSerif, 20,FontStyle.Bold);
            
            label.Text = "Please Wait";
            label.AutoSize = true;
            //label.Location=new Point(10,10);
            newForm.Controls.Add(label);
            //label.Refresh();
            //newForm.Refresh();
            
            newForm.StartPosition=FormStartPosition.CenterScreen;
            newForm.Show();
            newForm.BringToFront();
            newForm.Refresh();
            newForm.Cursor=Cursors.WaitCursor;
               _KillThreadWaitHandle.WaitOne();
            _KillThreadWaitHandle.Reset();
            newForm.Dispose();
             Thread.CurrentThread.Abort();
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        private void KillTheThread(Thread thread)
        {
            thread.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (!_softwareValidation.CheckValidation(_macAddress))
            //{
            //    return;
            //}
            Thread backgroundThread = new Thread(
                () =>
                {
                    Form form=new Form();
                    form.Width = 500;
                    form.Height = 150;
                    ProgressBar progressBar=new ProgressBar();
                    progressBar.Value = 0;
                    progressBar.Width = 500;
                    form.Controls.Add(progressBar);
                    form.Show();
                    form.BringToFront();
                    int n = 0;
                    while (Interlocked.CompareExchange(ref _progressBarThreadState,0,0)==0) 
                    {
                        Thread.Sleep(200);
                        progressBar.Value=n;
                        progressBar.Refresh();
                        form.Refresh();
                        //.BeginInvoke(
                        //    new Action(() =>
                        //    {
                        //        progressBar.Value = n;
                        //    }
                        //        ));
                        if (n == 100)
                        {
                            n = 0;
                        }
                        n++;
                    }
                    progressBar.Value = 0;
                    form.Dispose();
                });
            backgroundThread.Start();
            var defaultProxy=ReadProxySetting();
            Cursor.Current=Cursors.WaitCursor;
            var url = textBox1.Text;
            var requestCount = int.Parse(textBox2.Text);// Math.Min(_appWebProxies.WebProxies.Count, int.Parse(textBox2.Text));
            CompleteProxyList(requestCount);
            _responses =new QueueResponse[requestCount];
            _elapsedTimes=new int[requestCount];
            _threadsState=new List<int>();
            for (var i = 0; i < requestCount; i++)
            {
                _threadStateHanlde.Add(new ManualResetEvent(false));
                _threadsState.Add(0);
            }
            var requesterThreads=new Thread[requestCount];
            for (int i = 0; i < requestCount/2; i++)
            {
                var request=new Request() {Address = url,Type = (Request.Method)comboBox1.SelectedItem, WebProxy = _appWebProxies.WebProxies[i] };
                requesterThreads[i]=new Thread(ThreadStartMethod);
                //Thread.Sleep(1000);
                requesterThreads[i].Start(new Tuple<Request,int>(request, i));
            }
            for (int i = requestCount / 2; i < requestCount; i++)
            {
                var request = new Request() { Address = url, Type = (Request.Method)comboBox1.SelectedItem, WebProxy = _appWebProxies.WebProxies[i] };
                requesterThreads[i] = new Thread(ThreadStartMethod);
                //Thread.Sleep(1000);
                requesterThreads[i].Start(new Tuple<Request, int>(request, i));
            }
            // var killThreadCounter = 0;
            for (int i = 0; i < requestCount; i++)
            {
                //var location1 = _threadsState[i];
                //while (//Interlocked.CompareExchange(ref location1,0,0)==0)//requesterThreads[i].ThreadState==ThreadState.Running)
               // {
                   
               // }
                _threadStateHanlde[i].WaitOne();
                requesterThreads[i].Abort();
            }
            var a =_elapsedTimes;
            var b = _responses;
            TextBox[] textBoxes=new TextBox[requestCount];
            dataGridView1.Rows.Add(requestCount);
            for (int i = 0; i < requestCount; i++)
            {
                textBoxes[i]= new TextBox {Name = "textBox" + i};
                textBoxes[i].AutoSize = false;
                //b[i].Wait(10000);                
                var response = b[i];
                if (response!=null)
                {
                    dataGridView1.Rows[i].SetValues(i, response.QueueNumber,response.ResponseNumber, response.WebResponse.ResponseUri.AbsoluteUri as object);                
                }

            }
            ResetWebProxy(defaultProxy);
            Cursor.Current = Cursors.Default;
            Interlocked.Exchange(ref _progressBarThreadState, 1);
            backgroundThread.Abort();
           
        }

        private void CompleteProxyList(int requestCount)
        {
            if (requestCount <= _appWebProxies.WebProxies.Count) return;
            var tempProxis = _appWebProxies.WebProxies;
            var counter = _appWebProxies.WebProxies.Count;
            var countWebProxy = 0;
            while (counter<requestCount)
            {
                if (countWebProxy == tempProxis.Count - 1)
                {
                    countWebProxy = 0;
                }
                _appWebProxies.WebProxies.Add(_appWebProxies.WebProxies[countWebProxy]);
                counter++;
                countWebProxy++;
            }
        }
        private void ListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public  void ThreadStartMethod(object request)
        {
            var data = (Tuple<Request, int>) request;
            var startCount = Environment.TickCount;
            RequestSender requestSender=new RequestSender();
            var response = requestSender.Send(data.Item1,data.Item2,0);
            var finishCount = Environment.TickCount;
            _elapsedTimes[data.Item2] = finishCount - startCount;
            _responses[data.Item2] = response;
            //var location = (IntPtr)_threadsState[data.Item2];
            //Interlocked.CompareExchange(ref location, 1,0);
            _threadStateHanlde[data.Item2].Set();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ResetWebProxy(AppWebProxy webProxy)
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            if (webProxy==null)
            {
                registry.SetValue("ProxyEnable", 0);
                registry.SetValue("ProxyServer", "0.0.0.0:0");
            }
            else
            {
                registry.SetValue("ProxyEnable", 1);
                registry.SetValue("ProxyServer", webProxy.ProxyAddress+":"+webProxy.PortNumber);
            }
            registry.Dispose();
            settingsReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            refreshReturn = InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        private AppWebProxy ReadProxySetting()
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            var proxyEnable=(int)registry.GetValue("ProxyEnable");
            if (proxyEnable != 1) return null;
            var proxyServer=((string)registry.GetValue("ProxyServer")).Split(':');
            var proxyAddress = proxyServer[0];
            var portNumber = int.Parse(proxyServer[1]);
                registry.Dispose();
            AppWebProxy appWebProxy=new AppWebProxy(proxyAddress,portNumber);
            return appWebProxy;
        }
    }
}
