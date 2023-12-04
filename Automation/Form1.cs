using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Web.WebView2.Core;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using System.Security.Policy;


namespace Automation
{
    public partial class Form1 : Form
    {

        TabPageCustom ScriptEditortab { get; set; }

        Regex BegginingOfUrl = new Regex(@"^(https?:\/\/)");
        string googleSearch = "https://www.google.com/search?q=";
        // Regular expression to extract the domain
        string domainPattern = @"\b(\w+\.\w+)$";


        public Form1()
        {
            InitializeComponent();
            InitializeAsync();
        }

        private async Task Navigate()
        {
            var activeTabPage = tabControl1.SelectedTab as TabPageCustom;

            if (activeTabPage?._scriptBox != null) return;

            var navigatingUrl = this.navigationBar.Text.Trim();

            if (string.IsNullOrEmpty(navigatingUrl)) return;

            var navigateTO = navigatingUrl;

            if (ClassifyInput(navigateTO) == "URL")
            {
                if (!BegginingOfUrl.IsMatch(navigatingUrl))
                {
                    navigateTO = $"https:\\\\{navigatingUrl}";
                }
            }
            else
            {
                navigateTO = Uri.EscapeDataString(navigateTO);
                navigateTO = $"{googleSearch}{navigateTO}";
            }

            try
            {
                activeTabPage._webView2.CoreWebView2.Navigate(navigateTO);
            }
            catch (Exception ex)
            {
                writeLog(ex.Message);
            }
        }

        async void InitializeAsync()
        {
            ScriptEditortab = await CreateNewTab("New Text", ControlType.RichTextBox);
            await CreateNewTab("New Tab", ControlType.WebView2, 0);
        }

        private async void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            if (e.Uri == "https://localhost/")
            {
                e.Handled = true; // Prevent the default behavior

                if (sender is CoreWebView2 coreWebView2)
                {
                    TabPageCustom? theParentTab = FindParentTab(coreWebView2);

                    var newTab = await CreateNewTab("New Tab", ControlType.WebView2, theParentTab.PortNumber);

                    return; //RETURNS HERE
                }

                writeLog($"Tried to open: {e.Uri}");
            }

            if (!(e.Uri.Contains("https://accounts.google.com")))
            {
                e.Handled = true; // Prevent the default behavior
                string pattern = @"https?:\/\/(?:www\.)?([a-zA-Z0-9.-]+)";
                string url = "";

                if (sender is CoreWebView2 coreWebView2)
                {

                    url = coreWebView2.Source;
                    Match match = Regex.Match(url, pattern);

                    if (match.Success)
                    {
                        string domainName = match.Groups[1].Value;

                        url = e.Uri;
                        match = Regex.Match(url, pattern);

                        if (match.Success)
                        {
                            string domainName2 = match.Groups[1].Value;

                            if (string.Equals(domainName.ToLower().Trim(), domainName2.ToLower().Trim()))
                            {
                                TabPageCustom? theParentTab = FindParentTab(coreWebView2);

                                var newTab = await CreateNewTab("New Tab", ControlType.WebView2, theParentTab.PortNumber);
                                newTab._webView2.CoreWebView2.Navigate(url);

                                return; //RETURNS HERE
                            }

                            match = Regex.Match(domainName, domainPattern);
                            var matchSecondDomain = Regex.Match(domainName2, domainPattern);

                            if (match.Success)
                            {
                                if (match.Groups[1].Value == matchSecondDomain.Groups[1].Value)
                                {
                                    TabPageCustom? theParentTab = FindParentTab(coreWebView2);

                                    var newTab = await CreateNewTab("New Tab", ControlType.WebView2, theParentTab.PortNumber);
                                    newTab._webView2.CoreWebView2.Navigate(url);

                                    return; //RETURNS HERE
                                }
                            }


                        }

                    }
                }

                writeLog($"Tried to open: {e.Uri}");
            }

        }

        private string ClassifyInput(string inputString)
        {
            // Regular expression to find domain-like patterns before any slashes
            string pattern = @"\.[a-zA-Z]{2,3}(?=/|$)";

            if (Regex.IsMatch(inputString, pattern))
            {
                return "URL";
            }
            else
            {
                return "Search Query";
            }
        }

        private async void Button1_Click(object sender, EventArgs e)
        {

            await Navigate();
        }


        private async void writeLog(string textToWrite = "")
        {
            string messageToSend = ($"{textToWrite}");

            var activeTabPage = tabControl1.SelectedTab as TabPageCustom;

            string escapedMessage = System.Text.Json.JsonSerializer.Serialize(messageToSend);

            await activeTabPage?._webView2.CoreWebView2.ExecuteScriptAsync($"console.log({escapedMessage});");

        }

        private void webView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {

            var activeTabPage = tabControl1.SelectedTab as TabPageCustom;

            if (e.IsSuccess)
            {
                // Update the URL
                string currentUrl = activeTabPage._webView2?.CoreWebView2.Source ?? "";
                // Update a text box or other UI element with the current URL
                navigationBar.Text = currentUrl;

            }
        }

        private async Task<TabPageCustom> CreateNewTab(string text = "New Tab", ControlType controlType = ControlType.WebView2, int portNumber = 1)
        {
            var newTabPage = new TabPageCustom(text, controlType);

            //Only NavigatorsTab will have the navigation completed
            if (controlType == ControlType.WebView2)
            {
                await newTabPage.InitializeAsync(portNumber);
                newTabPage._webView2.NavigationCompleted += webView21_NavigationCompleted;
                newTabPage._webView2.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
                newTabPage._webView2.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
            }


            tabControl1.TabPages.Add(newTabPage);

            // Set the newly added tab page as the active one
            tabControl1.SelectedTab = newTabPage;


            return newTabPage;
        }

        private void CoreWebView2_DocumentTitleChanged(object? sender, object e)
        {

            if (sender is CoreWebView2 coreWebView2)
            {
                var activeTabPage = FindParentTab(coreWebView2);
                int maxCharacters = (sender as CoreWebView2)?.DocumentTitle.Length > 20 ? 20 : (sender as CoreWebView2).DocumentTitle.Length;
                var title = coreWebView2.DocumentTitle;
                if (title.Length > maxCharacters)
                {
                    title = title.Substring(0, maxCharacters);
                }
                activeTabPage.Text = title;
            }

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            var activeTabPage = tabControl1.SelectedTab as TabPageCustom;

            navigationBar.Text = "about:blank";

            if (activeTabPage?._scriptBox != null)
            {
                navigationBar.Text = "Script";
            }
            else if (activeTabPage?._webView2.CoreWebView2 != null)
            {
                // Update the URL
                string currentUrl = activeTabPage._webView2.CoreWebView2.Source;
                // Update a text box or other UI element with the current URL
                navigationBar.Text = currentUrl;
            }

        }

        private async void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Navigate();
                e.SuppressKeyPress = true;
            }
        }

        private async void btnCloseTab_Click(object sender, EventArgs e)
        {
            await CloseCurrentTab();
        }

        private async Task CloseCurrentTab()
        {
            if (tabControl1.TabCount > 1)
            {
                var selectedTab = tabControl1.SelectedTab as TabPageCustom;

                if (selectedTab._scriptBox != null) return;

                int count = 0;

                foreach (TabPageCustom tab in tabControl1.TabPages)
                {
                    if (tab.PortNumber == selectedTab.PortNumber) count++;
                }

                tabControl1.TabPages.Remove(selectedTab);

                if (count < 2 && selectedTab.PortNumber != 0)
                {
                    TabPageCustom.UsedPorts.Remove(selectedTab.PortNumber);
                    try
                    {
                        selectedTab.DeleteTempFiles();
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show("Execution Error: " + e.Message);
                    }

                }

                selectedTab.Dispose();
            }
        }

        private async void btnExecuteScript_Click(object sender, EventArgs e)
        {
            if (ScriptEditortab._scriptBox != null)
            {
                await ExecutePlaywrightScriptNonBlocking(ScriptEditortab._scriptBox.Text);
            }
        }

        public async Task ExecutePlaywrightScriptNonBlocking(string scriptCode)
        {
            try
            {
                // Setup the script options to reference Playwright
                var scriptOptions = ScriptOptions.Default
                    .AddReferences(typeof(IPlaywright).Assembly)
                    .WithImports("Microsoft.Playwright");

                // Execute the script asynchronously without blocking the UI thread
                var result = await CSharpScript.EvaluateAsync(scriptCode, scriptOptions);

                // Optionally, handle the result
                // If you need to update the UI based on the result, make sure to do it on the UI thread.
                // For example, if using WPF, you might need to use Dispatcher.Invoke to update the UI elements.
            }
            catch (CompilationErrorException e)
            {
                // Handle compilation errors
                // If you're in a UI context, make sure you're on the UI thread when you show the message box.
                // For Windows Forms, you would be okay to directly interact with UI controls here.
                MessageBox.Show("Compilation Error: " + string.Join(Environment.NewLine, e.Diagnostics));
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                MessageBox.Show("Execution Error: " + ex.Message);
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            var selectedTab = tabControl1.SelectedTab as TabPageCustom;
            this.txtBoxPortNumber.Text = selectedTab.PortNumber.ToString();
        }

        private async void newTabBtn_Click(object sender, EventArgs e)
        {
            await CreateNewTab("New Tab", ControlType.WebView2);

            this.navigationBar.Focus();
            this.navigationBar.SelectAll();
        }

        private async void btnCreateUserTab_Click(object sender, EventArgs e)
        {
            await CreateNewTab("New Tab", ControlType.WebView2, 0);

            this.navigationBar.Focus();
            this.navigationBar.SelectAll();
        }

        private void navigationBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private TabPageCustom? FindParentTab(CoreWebView2 coreWebView2)
        {
            TabPageCustom? theParentTab = null;
            foreach (TabPageCustom parentTab in tabControl1.TabPages)
            {
                if (parentTab._webView2?.CoreWebView2 == coreWebView2)
                {
                    theParentTab = parentTab;
                    break;
                }
            }

            return theParentTab;
        }
    }
}