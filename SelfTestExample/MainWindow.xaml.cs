using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Ivi.Visa;
using Ivi.Driver;
using SelfTestExample.Models;

namespace SelfTestExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelfTestButton_Click(object sender, RoutedEventArgs e)
        {
            SelfTestResults.Items.Clear();
            Mouse.OverrideCursor = Cursors.Wait;
            SelfTestButton.IsEnabled = false;
            IEnumerable<string> resources = GlobalResourceManager.Find(); // Get all resource ids
            foreach (string resource in resources)
            {
                try
                {
                    SelfTestIVIDriver(resource); // Attempt IVI Driver First
                }
                catch (Exception)
                {
                    SelfTestMessageBased(resource); // Try SCPI
                }
            }
            SelfTestButton.IsEnabled = true;
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SelfTestIVIDriver(string resource)
        {
            IIviDriver driver = IviDriver.Create(resource);
            SelfTestResult selfTestResult = driver.Utility.SelfTest();
            SelfTestResults.Items.Add(new SelfTestViewModel(driver.Identity.Identifier, selfTestResult.Code, selfTestResult.Message));
            driver.Close();
        }

        private void SelfTestMessageBased(string resource)
        {
            try
            {
                IMessageBasedSession session = GlobalResourceManager.Open(resource) as IMessageBasedSession;
                session.TimeoutMilliseconds = 3000;
                session.FormattedIO.WriteLine("*TST?");
                string result = session.FormattedIO.ReadLine();
                SelfTestResults.Items.Add(new SelfTestViewModel(session.ResourceName, 1, result));
                session.Dispose();
            }
            catch (Exception ex)
            {
                SelfTestResults.Items.Add(new SelfTestViewModel(resource, -1, ex.Message));
            }
        }
    }
}
