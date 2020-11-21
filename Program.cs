using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.MailClient.Win;
using DevExpress.MailDemo.Win;
using DevExpress.ProductsDemo.Win.Forms;
using DevExpress.Skins;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;

namespace DevExpress.ProductsDemo.Win {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments) {
            AppDomain.CurrentDomain.AssemblyResolve += OnCurrentDomainAssemblyResolve;
            WindowsFormsSettings.ApplyDemoSettings();

            DataHelper.ApplicationArguments = arguments;
            System.Globalization.CultureInfo enUs = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = enUs;
            System.Threading.Thread.CurrentThread.CurrentUICulture = enUs;
            DevExpress.Utils.LocalizationHelper.SetCurrentCulture(DataHelper.ApplicationArguments);
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Utils.AppearanceObject.DefaultFont = new Font("Segoe UI", 8);
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2016 Colorful");
            SkinManager.EnableFormSkins();
            EnumProcessingHelper.RegisterEnum<TaskStatus>();

            SplashScreenManager.ShowForm(null, typeof(ssMain), true, true, false, 1000);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
        //
        static Assembly OnCurrentDomainAssemblyResolve(object sender, ResolveEventArgs args) {
            string partialName = DevExpress.Utils.AssemblyHelper.GetPartialName(args.Name).ToLower();
            if(partialName == "entityframework" || partialName == "system.data.sqlite") {
                string path = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "Dll", partialName + ".dll");
                return Assembly.LoadFrom(path);
            }
            return null;
        }
    }
}
