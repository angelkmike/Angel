using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraNavBar;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon.Gallery;
using DevExpress.XtraRichEdit;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using DevExpress.Utils.About;
using DevExpress.MailDemo.Win;
using DevExpress.MailClient.Win;
using DevExpress.MailClient.Win.Forms;
using DevExpress.Demos;
using DevExpress.Description.Controls;
using DevExpress.XtraEditors.ColorWheel;
using DevExpress.LookAndFeel;
using DevExpress.Utils.Taskbar.Core;
using DevExpress.Utils.Taskbar;

namespace DevExpress.ProductsDemo.Win {
    public partial class frmMain : RibbonForm {
        ModulesNavigator modulesNavigator;
        ZoomManager zoomManager;
        List<BarItem> AllowCustomizationMenuList = new List<BarItem>();
        GuideGenerator guideGenerator;
        public frmMain() {
            TaskbarHelper.InitDemoJumpList(TaskbarAssistant.Default, this);
            InitializeComponent();
            RibbonButtonsInitialize();
            modulesNavigator = new ModulesNavigator(ribbonControl1, pcMain);
            zoomManager = new ZoomManager(ribbonControl1, modulesNavigator, beiZoom);
            InitNavBarItemLinks();
            NavigationInitialize();
            SetPageLayoutStyle();
            guideGenerator = new GuideGenerator();
            guideGenerator.CreateWhatsThisItem(ribbonControl1, () => { return this; });
        }
        void NavigationInitialize() {
            foreach(NavBarItemLink link in nbgModules.ItemLinks) {
                BarButtonItem item = new BarButtonItem(ribbonControl1.Manager, link.Item.Caption);
                item.Tag = link;
                item.Glyph = link.Item.SmallImage;
                item.ItemClick += new ItemClickEventHandler(item_ItemClick);
                bsiNavigation.ItemLinks.Add(item);
            }
        }

        void item_ItemClick(object sender, ItemClickEventArgs e) {
            nbgModules.SelectedLink = (NavBarItemLink)e.Item.Tag;
        }
        void RibbonButtonsInitialize() {
            InitBarButtonItem(bbiNewTask, TagResources.TaskNew, Properties.Resources.NewTaskDescription);
            InitBarButtonItem(bbiEditTask, TagResources.TaskEdit, Properties.Resources.EditTaskDescription);
            InitBarButtonItem(bbiDeleteTask, TagResources.TaskDelete, Properties.Resources.DeleteTaskDescription);
            InitBarButtonItem(bbiTodayFlag, FlagStatus.Today, Properties.Resources.FlagTodayDescription);
            InitBarButtonItem(bbiTomorrowFlag, FlagStatus.Tomorrow, Properties.Resources.FlagTomorrowDescription);
            InitBarButtonItem(bbiThisWeekFlag, FlagStatus.ThisWeek, Properties.Resources.FlagThisWeekDescription);
            InitBarButtonItem(bbiNextWeekFlag, FlagStatus.NextWeek, Properties.Resources.FlagNextWeekDescription);
            InitBarButtonItem(bbiNoDateFlag, FlagStatus.NoDate, Properties.Resources.FlagNoDatekDescription);
            InitBarButtonItem(bbiCustomFlag, FlagStatus.Custom, Properties.Resources.FlagCustomDescription);
            InitBarButtonItem(bbiNewContact, TagResources.ContactNew, Properties.Resources.NewContactDescription);
            InitBarButtonItem(bbiEditContact, TagResources.ContactEdit, Properties.Resources.EditContactDescription);
            InitBarButtonItem(bbiDeleteContact, TagResources.ContactDelete, Properties.Resources.DeleteContactDescription);
            InitBarButtonItem(bbiFlipLayout, TagResources.FlipLayout, Properties.Resources.FlipLayoutDescription);
            InitGalleryItem(rgbiCurrentViewTasks.Gallery.Groups[0].Items[0], TagResources.TaskList, Properties.Resources.TaskListDescription);
            InitGalleryItem(rgbiCurrentViewTasks.Gallery.Groups[0].Items[1], TagResources.TaskToDoList, Properties.Resources.TaskToDoListDescription);
            InitGalleryItem(rgbiCurrentViewTasks.Gallery.Groups[0].Items[2], TagResources.TaskCompleted, Properties.Resources.TaskCompletedDescription);
            InitGalleryItem(rgbiCurrentViewTasks.Gallery.Groups[0].Items[3], TagResources.TaskToday, Properties.Resources.TaskTodayDescription);
            InitGalleryItem(rgbiCurrentViewTasks.Gallery.Groups[0].Items[4], TagResources.TaskPrioritized, Properties.Resources.TaskPrioritizedDescription);
            InitGalleryItem(rgbiCurrentViewTasks.Gallery.Groups[0].Items[5], TagResources.TaskOverdue, Properties.Resources.TaskOverdueDescription);
            InitGalleryItem(rgbiCurrentViewTasks.Gallery.Groups[0].Items[6], TagResources.TaskSimpleList, Properties.Resources.TaskSimpleListDescription);
            InitGalleryItem(rgbiCurrentViewTasks.Gallery.Groups[0].Items[7], TagResources.TaskDeferred, Properties.Resources.TaskDeferredDescription);
            InitGalleryItem(rgbiCurrentView.Gallery.Groups[0].Items[0], TagResources.ContactList, Properties.Resources.ContactListDescription);
            InitGalleryItem(rgbiCurrentView.Gallery.Groups[0].Items[1], TagResources.ContactAlphabetical, Properties.Resources.ContactAlphabeticalDescription);
            InitGalleryItem(rgbiCurrentView.Gallery.Groups[0].Items[2], TagResources.ContactByState, Properties.Resources.ContactByStateDescription);
            InitGalleryItem(rgbiCurrentView.Gallery.Groups[0].Items[3], TagResources.ContactCard, Properties.Resources.ContactCardDescription);
            bvbiSaveAs.Tag = TagResources.MenuSaveAs;
            bvbiSaveAttachment.Tag = TagResources.MenuSaveAttachment;
            bsiNavigation.Hint = Properties.Resources.NavigationDescription;
            
            AllowCustomizationMenuList.Add(bsiNavigation);
            AllowCustomizationMenuList.Add(skinDropDownButtonItem1);
            ribbonControl1.Toolbar.ItemLinks.Add(skinDropDownButtonItem1);
            AllowCustomizationMenuList.Add(skinPaletteRibbonGalleryBarItem1);
            ribbonControl1.Toolbar.ItemLinks.Add(skinPaletteRibbonGalleryBarItem1);
        }

        void InitGalleryItem(GalleryItem galleryItem, string tag, string description) {
            galleryItem.Tag = tag;
            galleryItem.Hint = description;
        }
        internal ZoomManager ZoomManager { get { return zoomManager; } }
        internal BackstageViewButtonItem SaveAsMenuItem { get { return bvbiSaveAs; } }
        internal BackstageViewButtonItem SaveAttachmentMenuItem { get { return bvbiSaveAttachment; } }
        internal InRibbonGallery TaskGallery { get { return rgbiCurrentViewTasks.Gallery; } }
        internal PopupMenu FlagStatusMenu { get { return pmFlagStatus; } }
        void InitBarButtonItem(DevExpress.XtraBars.BarButtonItem buttonItem, object tag) {
            InitBarButtonItem(buttonItem, tag, string.Empty);
        }
        void InitBarButtonItem(DevExpress.XtraBars.BarButtonItem buttonItem, object tag, string description) {
            buttonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(bbi_ItemClick);
            buttonItem.Hint = description;
            buttonItem.Tag = tag;
        }
        void InitNavBarItemLinks() {
            nbiGrid.Tag = new NavBarGroupTagObject("Tasks", typeof(DevExpress.ProductsDemo.Win.Modules.GridModule));
            nbiGridCardView.Tag = new NavBarGroupTagObject("Contacts", typeof(DevExpress.ProductsDemo.Win.Modules.Contacts));
            nbiSpreadsheet.Tag = new NavBarGroupTagObject("Spreadsheet", typeof(DevExpress.ProductsDemo.Win.Modules.SpreadsheetModule), RibbonControlColorScheme.Green);
            nbiWord.Tag = new NavBarGroupTagObject("Word", typeof(DevExpress.ProductsDemo.Win.Modules.WordModule), RibbonControlColorScheme.DarkBlue);
#if !DXCORE3
            nbiSnap.Tag = new NavBarGroupTagObject("Snap", typeof(DevExpress.ProductsDemo.Win.Modules.SnapModule), RibbonControlColorScheme.Teal);
            nbiSnap.Visible = false;
            nbiReports.Tag = new NavBarGroupTagObject("Reports", typeof(DevExpress.ProductsDemo.Win.Modules.ReportsModule), RibbonControlColorScheme.Teal);
            nbiPivot.Tag = new NavBarGroupTagObject("Pivot", typeof(DevExpress.ProductsDemo.Win.Modules.PivotModuleNew));
#else
            nbiSnap.Visible = false;
            nbiReports.Visible = false;
            nbiPivot.Visible = false;
#endif
            nbiCharts.Tag = new NavBarGroupTagObject("Charts", typeof(DevExpress.ProductsDemo.Win.Modules.AnalyticsModule));
            nbiScheduler.Tag = new NavBarGroupTagObject("Scheduler", typeof(DevExpress.ProductsDemo.Win.Modules.SchedulerModule), RibbonControlColorScheme.Purple);
            nbiPdf.Tag = new NavBarGroupTagObject("PdfViewer", typeof(DevExpress.ProductsDemo.Win.Modules.PdfViewerModule), RibbonControlColorScheme.Orange);
            nbiMaps.Tag = new NavBarGroupTagObject("Maps", typeof(DevExpress.ProductsDemo.Win.Modules.MapsModule), RibbonControlColorScheme.Red);
            nbgModules.SelectedLinkIndex = 0;
        }
        internal void EnableLayoutButtons(bool enabled) {
            bbiFlipLayout.Enabled = enabled;
        }
        internal void EnableEditContact(bool enabled) {
            bbiDeleteContact.Enabled = enabled;
            bbiEditContact.Enabled = enabled;
        }
        internal void EnabledFlagButtons(bool enabledCurrentTask, bool enabledEdit, Task task) {
            List<BarButtonItem> list = new List<BarButtonItem> { bbiTodayFlag, bbiTomorrowFlag, bbiThisWeekFlag, 
                bbiNextWeekFlag, bbiNoDateFlag, bbiCustomFlag };
            foreach(BarButtonItem item in list) {
                item.Enabled = enabledCurrentTask;
                if(task != null)
                    item.Down = task.FlagStatus.Equals(item.Tag);
                else item.Down = false;
            }
            bbiDeleteTask.Enabled = enabledCurrentTask;
            bbiEditTask.Enabled = enabledEdit;
        }
        internal void EnableZoomControl(bool enabled) {
            beiZoom.Visibility = enabled ? BarItemVisibility.Always : BarItemVisibility.Never;
        }
        internal void ShowInfo(int? count) {
            if(count == null) bsiInfo.Caption = string.Empty;
            else
                bsiInfo.Caption = string.Format(Properties.Resources.InfoText, count.Value);
            HtmlText = "Build Your Own Office"; // string.Format("{0}{1}", GetModuleName(), GetModulePartName());
        }
        string GetModuleName() {
            if(string.IsNullOrEmpty(modulesNavigator.CurrentModule.PartName)) return CurrentModuleName;
            return string.Format("<b>{0}</b>", CurrentModuleName);
        }
        string GetModulePartName() {
            if(string.IsNullOrEmpty(modulesNavigator.CurrentModule.PartName)) return null;
            return string.Format(" - {0}", modulesNavigator.CurrentModule.PartName);
        }
        private void navBarControl1_SelectedLinkChanged(object sender, XtraNavBar.ViewInfo.NavBarSelectedLinkChangedEventArgs e) {
            if(e.Link != null)
                modulesNavigator.ChangeSelectedItem(e.Link, null);
        }
        private void bbi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            modulesNavigator.CurrentModule.ButtonClick(string.Format("{0}", e.Item.Tag));
        }
        private void frmMain_KeyDown(object sender, KeyEventArgs e) {
            modulesNavigator.CurrentModule.SendKeyDown(e);
        }
        private void navBarControl1_NavPaneStateChanged(object sender, EventArgs e) {
            SetPageLayoutStyle();
        }
        private void bvbiExit_ItemClick(object sender, BackstageViewItemEventArgs e) {
            this.Close();
        }

        private void galleryControlGallery1_ItemClick(object sender, GalleryItemClickEventArgs e) {
            modulesNavigator.CurrentModule.ButtonClick(string.Format("{0}", e.Item.Tag));
        }

        private void backstageViewControl1_ItemClick(object sender, BackstageViewItemEventArgs e) {
            if(modulesNavigator.CurrentModule == null) return;
            modulesNavigator.CurrentModule.ButtonClick(string.Format("{0}", e.Item.Tag));
        }
        void SetPageLayoutStyle() {
            bbiNormal.Down = navBarControl1.OptionsNavPane.NavPaneState == NavPaneState.Expanded;
            bbiReading.Down = navBarControl1.OptionsNavPane.NavPaneState == NavPaneState.Collapsed;
        }

        private void bbiNormal_ItemClick(object sender, ItemClickEventArgs e) {
            if(bbiNormal.Down) navBarControl1.OptionsNavPane.NavPaneState = NavPaneState.Expanded;
            else
                bbiNormal.Down = true;
        }

        private void bbiReading_ItemClick(object sender, ItemClickEventArgs e) {
            if(bbiReading.Down) navBarControl1.OptionsNavPane.NavPaneState = NavPaneState.Collapsed;
            else
                bbiReading.Down = true;
        }

        private void rgbiCurrentView_GalleryInitDropDownGallery(object sender, InplaceGalleryEventArgs e) {
            e.PopupGallery.GalleryDropDown.ItemLinks.Add(bbiManageView);
            e.PopupGallery.GalleryDropDown.ItemLinks.Add(bbiSaveCurrentView);
            e.PopupGallery.SynchWithInRibbonGallery = true;
        }

        private void rgbiCurrentViewTasks_GalleryItemClick(object sender, GalleryItemClickEventArgs e) {
            modulesNavigator.CurrentModule.ButtonClick(string.Format("{0}", e.Item.Tag));
        }

        private void bvtiPrint_SelectedChanged(object sender, BackstageViewItemEventArgs e) {
            if(backstageViewControl1.SelectedTab == bvtiPrint)
                this.printControl1.InitPrintingSystem();
        }
        private void ribbonControl1_BeforeApplicationButtonContentControlShow(object sender, EventArgs e) {
            if(backstageViewControl1.SelectedTab == bvtiPrint) backstageViewControl1.SelectedTab = bvtiInfo;
            bvtiPrint.Enabled = CurrentRichEdit != null || CurrentPrintableComponent != null;
            bvtiExport.Enabled = CurrentExportComponent != null;
        }
        public IPrintable CurrentPrintableComponent { get { return modulesNavigator.CurrentModule.PrintableComponent; } }
        public IPrintable CurrentExportComponent { get { return modulesNavigator.CurrentModule.ExportComponent; } }
        public RichEditControl CurrentRichEdit { get { return modulesNavigator.CurrentModule.CurrentRichEdit; } }
        public string CurrentModuleName { get { return modulesNavigator.CurrentModule.ModuleName; } }

        private void ribbonControl1_ShowCustomizationMenu(object sender, RibbonCustomizationMenuEventArgs e) {
            e.CustomizationMenu.InitializeMenu();
            if(e.Link == null || !AllowCustomizationMenuList.Contains(e.Link.Item))
                e.CustomizationMenu.RemoveLink(e.CustomizationMenu.ItemLinks[0]);
        }
        public RibbonStatusBar RibbonStatusBar { get { return ribbonStatusBar1; } }
        internal void ShowReminder(List<Task> reminders) {
            bool allowReminders = reminders != null && reminders.Count > 0;
            bbiReminder.Visibility = allowReminders ? BarItemVisibility.Always : BarItemVisibility.Never;
            bsiTemp.Visibility = allowReminders ? BarItemVisibility.Never : BarItemVisibility.Always;
            if(allowReminders) {
                bbiReminder.Caption = string.Format(Properties.Resources.ReminderText, reminders.Count);
                bbiReminder.Tag = reminders;
            }
        }
        public void ShowInfo(bool visible) {
            bsiInfo.Visibility = bsiTemp.Visibility = visible ? BarItemVisibility.Always : BarItemVisibility.Never;
        }

        private void biPrintPreview_ItemClick(object sender, ItemClickEventArgs e) {
            ShowPrintPreview();
        }
        protected void ShowPrintPreview() {
            if(CurrentPrintableComponent == null) return;
            PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
            if(modulesNavigator.CurrentModule.AllowRtfTitle) {
                link.RtfReportHeader = @"{\rtf1\ansi\ansicpg1251\deff0\deflang1049{\fonttbl{\f0\fnil\fcharset0 Segoe UI;}}
{\*\generator Msftedit 5.41.21.2510;}\viewkind4\uc1\pard\sa200\sl276\slmult1\qc\lang9\f0\fs32 " + CurrentModuleName + @"\par
}";
            } 
            link.Component = CurrentPrintableComponent;
            link.CreateDocument();
            link.ShowRibbonPreviewDialog(this.LookAndFeel);
        }

        internal void OnModuleShown(BaseModule baseModule) {
            rpgPrint.Visible = CurrentPrintableComponent != null;
        }

        private void bbiReminder_ItemClick(object sender, ItemClickEventArgs e) {
            using(frmReminders frm = new frmReminders()) {
                frm.InitData(bbiReminder.Tag as List<Task>);
                if(frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
                    modulesNavigator.CurrentModule.FocusObject(frm.CurrentTask);
                    modulesNavigator.CurrentModule.ButtonClick(TagResources.TaskEdit);
                }
            }

        }
    }
}
