using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImmigrationSolution
{
    public class WebBrowserControler
    {
        //private WebBrowser webBrowser1;

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem,
            saveAsToolStripMenuItem, printToolStripMenuItem,
            printPreviewToolStripMenuItem, exitToolStripMenuItem,
            pageSetupToolStripMenuItem, propertiesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1, toolStripSeparator2;

        private ToolStrip toolStrip1, toolStrip2;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripButton goButton, backButton,
            forwardButton, stopButton, refreshButton,
            homeButton, searchButton, printButton;

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;

        public void GoTo(Uri url)
        {
            
            //Tuple<Form,Uri> threadObject=new Tuple<Form, Uri>(form,url);
           // Thread thread=new Thread(() =>
           // {
                WebBrowser webBrowser=new WebBrowser();
                //Thread.Sleep(1000);
                var form = Initilaize(webBrowser);
                webBrowser.Navigate(url);
                //Application.Run(form);
                form.Show();
           // });
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
            
        }
        private Form Initilaize(WebBrowser webBrowser)
        {
            Form form = new Form
            {
                Name = "WebBrowserForm",
                WindowState = FormWindowState.Maximized
            };

            //webBrowser = new WebBrowser();

            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            printToolStripMenuItem = new ToolStripMenuItem();
            printPreviewToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            pageSetupToolStripMenuItem = new ToolStripMenuItem();
            propertiesToolStripMenuItem = new ToolStripMenuItem();

            toolStrip1 = new ToolStrip();
            goButton = new ToolStripButton();
            backButton = new ToolStripButton();
            forwardButton = new ToolStripButton();
            stopButton = new ToolStripButton();
            refreshButton = new ToolStripButton();
            homeButton = new ToolStripButton();
            searchButton = new ToolStripButton();
            printButton = new ToolStripButton();

            toolStrip2 = new ToolStrip();
            toolStripTextBox1 = new ToolStripTextBox();

            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();

            menuStrip1.Items.Add(fileToolStripMenuItem);

            fileToolStripMenuItem.DropDownItems.AddRange(
                new ToolStripItem[] {
                saveAsToolStripMenuItem, toolStripSeparator1,
                pageSetupToolStripMenuItem, printToolStripMenuItem,
                printPreviewToolStripMenuItem, toolStripSeparator2,
                propertiesToolStripMenuItem, exitToolStripMenuItem
                });

            fileToolStripMenuItem.Text = "&File";
            saveAsToolStripMenuItem.Text = "Save &As...";
            pageSetupToolStripMenuItem.Text = "Page Set&up...";
            printToolStripMenuItem.Text = "&Print...";
            printPreviewToolStripMenuItem.Text = "Print Pre&view...";
            propertiesToolStripMenuItem.Text = "Properties";
            exitToolStripMenuItem.Text = "E&xit";

            printToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.P;

            saveAsToolStripMenuItem.Click +=
                new System.EventHandler((sender, args) => saveAsToolStripMenuItem_Click(sender,args,webBrowser));
            pageSetupToolStripMenuItem.Click +=
                new System.EventHandler((sender, args) => pageSetupToolStripMenuItem_Click(sender,args,webBrowser));
            printToolStripMenuItem.Click +=
                new System.EventHandler((sender, args) => printToolStripMenuItem_Click(sender,args,webBrowser));
            printPreviewToolStripMenuItem.Click +=
                new System.EventHandler((sender, args) => printPreviewToolStripMenuItem_Click(sender,args,webBrowser));
            propertiesToolStripMenuItem.Click +=
                new System.EventHandler((sender, args) => propertiesToolStripMenuItem_Click(sender,args,webBrowser));
            exitToolStripMenuItem.Click +=
                new System.EventHandler(exitToolStripMenuItem_Click);
            webBrowser.CanGoBackChanged +=
           new EventHandler((sender, args) => webBrowser_CanGoBackChanged(sender,args,webBrowser));
            webBrowser.CanGoForwardChanged +=
                new EventHandler((sender, args) => webBrowser_CanGoForwardChanged(sender,args,webBrowser));
            webBrowser.DocumentTitleChanged +=
                new EventHandler(webBrowser_DocumentTitleChanged);
            webBrowser.StatusTextChanged +=
                new EventHandler((sender, args) => webBrowser_StatusTextChanged(sender,args,webBrowser));
            webBrowser.AllowNavigation = true;
           

            toolStrip1.Items.AddRange(new ToolStripItem[] {
            goButton, backButton, forwardButton, stopButton,
            refreshButton, homeButton, searchButton, printButton});

            goButton.Text = "Go";
            backButton.Text = "Back";
            forwardButton.Text = "Forward";
            stopButton.Text = "Stop";
            refreshButton.Text = "Refresh";
            homeButton.Text = "Home";
            searchButton.Text = "Search";
            printButton.Text = "Print";

            backButton.Enabled = false;
            forwardButton.Enabled = false;

            goButton.Click += new System.EventHandler((sender, args) => goButton_Click(sender,args,webBrowser));
            backButton.Click += new System.EventHandler((sender, args) => backButton_Click(sender,args,webBrowser));
            forwardButton.Click += new System.EventHandler((sender, args) => forwardButton_Click(sender,args,webBrowser));
            stopButton.Click += new System.EventHandler((sender, args) => stopButton_Click(sender,args,webBrowser));
            refreshButton.Click += new System.EventHandler((sender, args) =>refreshButton_Click(sender,args,webBrowser));
            homeButton.Click += new System.EventHandler((sender, args) => homeButton_Click(sender,args,webBrowser));
            searchButton.Click += new System.EventHandler((sender, args) => searchButton_Click(sender,args,webBrowser));
            printButton.Click += new System.EventHandler((sender, args) => printButton_Click(sender,args,webBrowser));

            toolStrip2.Items.Add(toolStripTextBox1);
            toolStripTextBox1.Size = new System.Drawing.Size(250, 25);
            toolStripTextBox1.KeyDown +=
                new KeyEventHandler((sender, args) => toolStripTextBox1_KeyDown(sender,args,webBrowser));
            toolStripTextBox1.Click +=
                new System.EventHandler(toolStripTextBox1_Click);

            statusStrip1.Items.Add(toolStripStatusLabel1);

            webBrowser.Dock = DockStyle.Fill;
            webBrowser.Navigated +=
                new WebBrowserNavigatedEventHandler((sender, args) => webBrowser_Navigated(sender,args,webBrowser));

            form.Controls.AddRange(new Control[] {
            webBrowser, toolStrip2, toolStrip1,
            menuStrip1, statusStrip1, menuStrip1 });

            return form;
        }
        // Displays the Save dialog box.
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e,WebBrowser webBrowser)
        {
            
            webBrowser.ShowSaveAsDialog();
        }

        // Displays the Page Setup dialog box.
        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e, WebBrowser webBrowser)
        {
            webBrowser.ShowPageSetupDialog();
        }

        // Displays the Print dialog box.
        private void printToolStripMenuItem_Click(object sender, EventArgs e, WebBrowser webBrowser)
        {
            webBrowser.ShowPrintDialog();
        }

        // Displays the Print Preview dialog box.
        private void printPreviewToolStripMenuItem_Click(
            object sender, EventArgs e, WebBrowser webBrowser)
        {
            webBrowser.ShowPrintPreviewDialog();
        }

        // Displays the Properties dialog box.
        private void propertiesToolStripMenuItem_Click(
            object sender, EventArgs e, WebBrowser webBrowser)
        {
            webBrowser.ShowPropertiesDialog();
        }

        // Selects all the text in the text box when the user clicks it. 
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            toolStripTextBox1.SelectAll();
        }

        // Navigates to the URL in the address box when 
        // the ENTER key is pressed while the ToolStripTextBox has focus.
        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e, WebBrowser webBrowser)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Navigate(toolStripTextBox1.Text,webBrowser);
            }
        }

        // Navigates to the URL in the address box when 
        // the Go button is clicked.
        private void goButton_Click(object sender, EventArgs e, WebBrowser webBrowser)
        {
            Navigate(toolStripTextBox1.Text,webBrowser);
        }

        // Navigates to the given URL if it is valid.
        private void Navigate(String address, WebBrowser webBrowser)
        {
            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }
            try
            {
                webBrowser.Navigate(new Uri(address));
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }

        // Updates the URL in TextBoxAddress upon navigation.
        private void webBrowser_Navigated(object sender,
            WebBrowserNavigatedEventArgs e, WebBrowser webBrowser)
        {
            toolStripTextBox1.Text = webBrowser.Url.ToString();
        }

        // Navigates webBrowser1 to the previous page in the history.
        private void backButton_Click(object sender, EventArgs e, WebBrowser webBrowser)
        {
            webBrowser.GoBack();
        }

        // Disables the Back button at the beginning of the navigation history.
        private void webBrowser_CanGoBackChanged(object sender, EventArgs e, WebBrowser webBrowser)
        {
            backButton.Enabled = webBrowser.CanGoBack;
        }

        // Navigates webBrowser1 to the next page in history.
        private void forwardButton_Click(object sender, EventArgs e, WebBrowser webBrowser)
        {
            webBrowser.GoForward();
        }

        // Disables the Forward button at the end of navigation history.
        private void webBrowser_CanGoForwardChanged(object sender, EventArgs e, WebBrowser webBrowser)
        {
            forwardButton.Enabled = webBrowser.CanGoForward;
        }

        // Halts the current navigation and any sounds or animations on 
        // the page.
        private void stopButton_Click(object sender, EventArgs e, WebBrowser webBrowser)
        {
            webBrowser.Stop();
        }

        // Reloads the current page.
        private void refreshButton_Click(object sender, EventArgs e, WebBrowser webBrowser)
        {
            // Skip refresh if about:blank is loaded to avoid removing
            // content specified by the DocumentText property.
            if (!webBrowser.Url.Equals("about:blank"))
            {
                webBrowser.Refresh();
            }
        }

        // Navigates webBrowser1 to the home page of the current user.
        private void homeButton_Click(object sender, EventArgs e, WebBrowser webBrowser)
        {
            webBrowser.GoHome();
        }

        // Navigates webBrowser1 to the search page of the current user.
        private void searchButton_Click(object sender, EventArgs e, WebBrowser webBrowser)
        {
            webBrowser.GoSearch();
        }

        // Prints the current document using the current print settings.
        private void printButton_Click(object sender, EventArgs e, WebBrowser webBrowser)
        {
            webBrowser.Print();
        }

        // Updates the status bar with the current browser status text.
        private void webBrowser_StatusTextChanged(object sender, EventArgs e, WebBrowser webBrowser)
        {
            toolStripStatusLabel1.Text = webBrowser.StatusText;
        }

        // Updates the title bar with the current document title.
        private void webBrowser_DocumentTitleChanged(object sender, EventArgs e)
        {
            
            
        }

        // Exits the application.
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
