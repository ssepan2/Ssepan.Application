using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ssepan.Utility;
using System.Diagnostics;
using System.Reflection;

namespace Ssepan.Application.WinForms
{
    public static class ClickOnce
    {
        public static Boolean CheckForUpdates()
        {
            Boolean returnValue = default(Boolean);
            try
            {
                // First check to see if we are running in a ClickOnce context
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    // Get an instance of the deployment
                    ApplicationDeployment deployment = ApplicationDeployment.CurrentDeployment;

                    // Check to see if updates are available
                    if (deployment.CheckForUpdate())
                    {
                        DialogResult res = MessageBox.Show("A new version of the application is available, do you want to update?", "Application Updater", MessageBoxButtons.YesNo);
                        if (res == DialogResult.Yes)
                        {
                            // Do the update
                            deployment.Update();
                            DialogResult res2 = MessageBox.Show("Update complete, do you want to restart the application to apply the update?", "Application Updater", MessageBoxButtons.YesNo);
                            if (res2 == DialogResult.Yes)
                            {
                                // Restart the application to apply the update
                                System.Windows.Forms.Application.Restart();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No updates available.", "Application Updater");
                    }
                }
                else
                {
                    MessageBox.Show("Updates not allowed unless you are launched through ClickOnce.");
                }

                returnValue = true;
            }
            catch (Exception ex)
            {
                Log.Write(
                    ex,
                    MethodBase.GetCurrentMethod(),
                    EventLogEntryType.Error);
                        
            }
            return returnValue;
        }
    }
}
