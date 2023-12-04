using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automation
{
    internal class TabPageCustom : TabPage
    {
        public WebView2 _webView2 { get; set; }
        public RichTextBox _scriptBox { get; set; }
        private string _userDataDir = null!;
        private const int MinPort = 49152;
        private const int MaxPort = 65535;
        public int PortNumber { get; private set; }
        public static readonly HashSet<int> UsedPorts = new HashSet<int>();
        private static readonly Random Random = new Random();
        private static readonly object PortLock = new object();

        public TabPageCustom() : base() => Initializer();

        public TabPageCustom(string name) : base(name) => Initializer();

        public TabPageCustom(string name, ControlType controlType) : base(name) 
        {
            Initializer(controlType);
        }


        public delegate void NewWindowRequested(string address);

        public delegate void WriteToLog(string message);


        private void Initializer(ControlType controlType = ControlType.WebView2)
        {

            switch (controlType)
            {
                case ControlType.WebView2:
                    _webView2 = new WebView2
                    {
                        Dock = DockStyle.Fill,
                    };

                    this.Controls.Add(_webView2);

                    break;

                case ControlType.RichTextBox:
                    _scriptBox = new RichTextBox
                    {
                        Dock = DockStyle.Fill,
                    };
                    this.Controls.Add(_scriptBox);
                    break;
            }

        }

        public async Task InitializeAsync(int inheritedPortNumber = 1)
        {
            PortNumber = inheritedPortNumber;

            if (PortNumber == 1)
            {
                lock (PortLock)
                {
                    do
                    {
                        PortNumber = Random.Next(MinPort, MaxPort + 1);
                    }
                    while (UsedPorts.Contains(PortNumber));
                    UsedPorts.Add(PortNumber);
                }
            }

            string executablePath = Assembly.GetExecutingAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(executablePath);

            _userDataDir = Path.Join(executableDirectory, $"playwright-webview2-tests/user-data-dir-{PortNumber}");

            if (!Directory.Exists(_userDataDir))
            {
                Directory.CreateDirectory(_userDataDir);
            }

            if (_webView2 != null)
            {
                var envOptions = new CoreWebView2EnvironmentOptions();

                if (inheritedPortNumber != 0)
                {
                    envOptions = new CoreWebView2EnvironmentOptions()
                    {
                        AdditionalBrowserArguments = $"--remote-debugging-port={PortNumber}",
                    };
                }

                var environment = await CoreWebView2Environment.CreateAsync(null, _userDataDir, envOptions);
                await _webView2.EnsureCoreWebView2Async(environment).ConfigureAwait(false);
            }
        }

        public void DeleteTempFiles()
        {
            if (Directory.Exists(_userDataDir))
            {
               Directory.Delete(_userDataDir, true);
            }
        }

    }
}
