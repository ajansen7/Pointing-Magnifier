using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WobbrockLib;
using WobbrockLib.Devices;
using System.Threading;
using System.Net.Mail;

namespace PointingMagnifier
{
    public partial class MainForm : Form
    {

        #region Properties
        private Magnifier _magnifier;
        private LowLevelKeyboardHook kHook;
        private Preferences preferences;
       // private Keys shortcutKeys;
        private Logger _logger;
        private System.Windows.Forms.Timer temp_disable;
        private delegate void formDelegate();
        private CursorWrapper _cursorWrapper;
        private bool _feedbackVisible;

        //Elapsed Time
        private System.Windows.Forms.Timer _updateTime; //must use a timer on the same thread!
        private System.DateTime _start;
        private System.Windows.Forms.Timer _feedbackReminder; //must use a timer on the same thread!
        private static System.TimeSpan STUDY_LENGTH = System.TimeSpan.FromHours(10);
        private static System.TimeSpan BASELINE_DATA = System.TimeSpan.FromHours(0); //use the software for 10 hours without enabling the magnifier.
        #endregion

        #region Constructor
        public MainForm()
        {
            InitializeComponent();
            //Initialize settings
            preferences = new Preferences();
            _cursorWrapper = new CursorWrapper();
            _logger = new Logger(ref preferences); //load preferences firs5t!

            //Add Keyboard Hook
            kHook = new LowLevelKeyboardHook("kHook");
            kHook.OnKeyDown += new EventHandler<KeyEventArgs>(kHook_OnKeyDown);

            //Add the Magnifier
            _magnifier = new Magnifier(ref preferences,ref _logger, ref _cursorWrapper);
            _magnifier.Idle += new EventHandler(_magnifier_Idle);

            menuStart.Enabled = !preferences.Study;

            _start = DateTime.Now;
            _updateTime = new System.Windows.Forms.Timer();
            _updateTime.Interval = 1000;
            _updateTime.Tick+=new EventHandler(_updateTime_Tick);
            _updateTime.Enabled = preferences.Logging;

            temp_disable = new System.Windows.Forms.Timer();
            temp_disable.Interval = 300000; //disable for 5 minutes
            temp_disable.Enabled = !preferences.Baseline && preferences.Logging && preferences.Study;
            temp_disable.Tick += new EventHandler(temp_disable_Tick);

            _feedbackReminder = new System.Windows.Forms.Timer();
            _feedbackReminder.Interval = 3600000; //show feedback reminder every hour. Does not neet to be precise
            _feedbackReminder.Enabled = !preferences.Baseline && preferences.Study;
            _feedbackReminder.Tick += new EventHandler(_feedbackReminder_Tick);

            _magnifier.Active = true; //Need to set to true so that the RIM will get initialized properly
           
            UpdateValues();
        }

        private void _feedbackReminder_Tick(object sender, EventArgs e)
        {
            this.niVMM.ShowBalloonTip(500, "Click here to send feedback now.", "Remember to please send us feedback on how the Pointing Magnifier is working.", ToolTipIcon.Info);
        }


        private void menuStart_Click(object sender, EventArgs e)
        {
            preferences.Study = true;
            preferences.Logging = true;
            preferences.Baseline = true;
            preferences.ElapsedTime = TimeSpan.Zero;
            _logger.StartStudy();
            UpdateValues(); //this is a really hack-y way to make sure the preferences get logged.
            if (_magnifier.Active)
                ToggleActive();
            
            _start = System.DateTime.Now;
            _updateTime.Enabled = true;
            _feedbackReminder.Enabled = true;
            menuStart.Enabled = false;
        }
        
        public void UpdateValues()
        {
            // Reset Form Values
            nudMagnification.Value = preferences.Magnification;
            nudSize.Value = preferences.Size;
            cbGain.Checked = preferences.Gain;
            cbKeyboard.Checked = preferences.Shortcuts;
            cbCrosshairs.Checked = preferences.Crosshairs;
            keyShortcut1.LoadShortcut(preferences.Shortcuts1);
            keyShortcut2.LoadShortcut(preferences.Shortcuts2);
            keyShortcut3.LoadShortcut(preferences.Shortcuts3);
            Invalidate();
            //update Magnifier
            _magnifier.RadiusNM = preferences.Size;
            _magnifier.MagnificationFactor = preferences.Magnification;
            _magnifier.Transparency = preferences.Transparency;
            _magnifier.LowerMouseGain = preferences.Gain;
            _magnifier.Crosshairs = preferences.Crosshairs;
            _magnifier.BackColor = preferences.Color;
            _magnifier.Invalidate();
        }
        #endregion 

        #region Form Events
        private void MainForm_Load(object sender, EventArgs e)
        {
            kHook.Install();
            SetMaximum();
            ToggleActive();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _magnifier.Close();
            _logger.UploadLog(false);
            _logger.Cleanup();
            kHook.Uninstall();
        }

        /// <summary>
        /// When the Pointing Magnifier goes idle, it is logged, and the elapsed time is not updated until the user returns.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _magnifier_Idle(object sender, EventArgs e)
        {
            System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();
            nvc.Add("Elapsed", preferences.ElapsedTime.ToString());
            if (_magnifier.isIdle)
            {
                _updateTime.Enabled = false;
                _logger.LogEvent("Idle", nvc);
            }
            else
            {
                _start = DateTime.Now;
                _updateTime.Enabled = true;
            }

        }

        void _updateTime_Tick(object sender, EventArgs e)
        {
            preferences.ElapsedTime += System.DateTime.Now - _start;
            _start = System.DateTime.Now;
            if (preferences.Baseline)
            {
                if (preferences.ElapsedTime >= BASELINE_DATA)
                {
                    preferences.ElapsedTime = new TimeSpan();
                    preferences.Baseline = false;
                    //MessageBox.Show("The study is starting, please use the Pointing Magnifier for 25 hours. You will be notified once the study is completed.");
                    ToggleActive();
                }
            }
            else if (preferences.Study)
            {
                if (preferences.ElapsedTime >= STUDY_LENGTH)
                {
                    preferences.Study = false;
                    preferences.ElapsedTime = new TimeSpan();
                    formDelegate f = EmailConfirmation;
                    f.BeginInvoke(null, null);
                    //EmailConfirmation();

                    //wahh this is lame
                    MessageBox.Show("Thank you for your participation. You will be contacted shortly about scheduling a time to debrief. In the mean time, you may continue using the Pointing Magnifier if you choose.", "Study Completed");
                    this.Close(); //quit the magnifier!
                }
            }
        }
        
        /// <summary>
        /// Sends an email to myself as a confirmation that the participant is done with the study.
        /// 
        /// this wonderous method is courtesy of http://stackoverflow.com/questions/32260/sending-email-in-net-through-gmail
        /// </summary>
        private void EmailConfirmation()
        {
            var fromAddress = new MailAddress("ajansen1090@gmail.com", "Alex Jansen");
            var toAddress = new MailAddress("ajansen7@uw.edu", "me");
            const string fromPassword = "ade9lie9";
            const string subject = "Study Notification";
            string body = String.Format("Participant {0} has just finished using the Pointing Magnifier for the ammount of time prescribed for the study. Please notify participant and schedule a debreifing session", Properties.Settings.Default.UID);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
#endregion


        #region Preferences
        private void nudMagnification_ValueChanged(object sender, EventArgs e)
        {
            preferences.Magnification = (int)nudMagnification.Value;
            _magnifier.MagnificationFactor = preferences.Magnification;
            SetMaximum();
        }
        private void nudSize_ValueChanged(object sender, EventArgs e)
        {
            preferences.Size = (int)nudSize.Value;
            _magnifier.RadiusNM = preferences.Size;
        }
        private void keyShortcut2_ShortcutChanged(object sender, EventArgs e)
        {
            preferences.Shortcuts2 = keyShortcut2.Shortcut;
            _logger.Preferences("Shorcut_Quit", keyShortcut2.Shortcut.ToString());
        }

        private void keyShortcut1_ShortcutChanged(object sender, EventArgs e)
        {
            preferences.Shortcuts1 = keyShortcut1.Shortcut;
            _logger.Preferences("Shorcut_Toggle", keyShortcut1.Shortcut.ToString());
            updateButtons();
        }
        private void keyShortcut3_ShortcutChanged(object sender, EventArgs e)
        {
            preferences.Shortcuts3 = keyShortcut3.Shortcut;
            _logger.Preferences("Shorcut_Feedback", keyShortcut3.Shortcut.ToString());
        }
        private void cbGain_CheckedChanged(object sender, EventArgs e)
        {
            preferences.Gain = cbGain.Checked;
            _magnifier.LowerMouseGain = preferences.Gain;
        }
        private void cbCrosshairs_CheckedChanged(object sender, EventArgs e)
        {
            preferences.Crosshairs = cbCrosshairs.Checked;
            _magnifier.Crosshairs = preferences.Crosshairs;
        }
        private void btnDefaults_Click(object sender, EventArgs e)
        {
            preferences.Reset();
            UpdateValues();
        }
        #endregion

        #region Menu Strip
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.niVMM.ShowBalloonTip(500, "The Pointing Magnifier is here.", "The Pointing Magnifier is still running and can be accessed via this System Tray icon.", ToolTipIcon.Info);
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.Show();
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            SessionInformation s = new SessionInformation();
            s.Show();
        }
        #endregion

        #region Tray
        /// <summary>
        /// On click, show the options panel if it is not already visible. otherwise, bring it to the front. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsShow_Click(object sender, EventArgs e)
        {
                if (!this.Visible)
                    this.Show();
                this.TopMost = true;
                this.TopMost = false;
                if (_magnifier.Active)
                    _magnifier.BringToFront();
        }

        /// <summary>
        /// Need to disambiguate between click an mouse down in the case of the sys tray icon so that the magnifier behavior can be preserved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void niVMM_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
            if (!this.Visible)
                this.Show();
            this.TopMost = true;
            this.TopMost = false;
            if (_magnifier.Active)
                _magnifier.BringToFront();
            }
        }
        #endregion

        #region Feedback
        /// <summary>
        /// Function to create a modal dialogue for feedback. Because of the mousehooks and toplevel windows, we must use a delegate to call this function.
        /// </summary>
        private void showQuickFeedback()
        {
            if (!_feedbackVisible)
            {
                _feedbackVisible = true;
                Bitmap b = (Bitmap)_logger.CurrPic.Clone(); //gotta make a copy, not a pointer. 
                QuickFeedback f = new QuickFeedback();
                f.SetPicture(b);
                DialogResult dr = new DialogResult();
                dr = f.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    String fname = String.Format("{0}_{1}_{2}.jpg", Properties.Settings.Default.UID, DateTime.Today.DayOfYear, Environment.TickCount);
                    if (f.Checked)
                    {
                        try
                        {
                            b.Save(fname, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    _logger.Feedback(f.Checked, f.Feedback, "QuickFeedback", fname);
                }
                b.Dispose();
                f.Dispose();
                _feedbackVisible = false;
            }
        }

        /// <summary>
        /// Function to create a modal dialogue for feedback. Because of the mousehooks and toplevel windows, we must use a delegate to call this function.
        /// </summary>
        private void showDailySurvey()
        {
            if (!_feedbackVisible)
            {
                _feedbackVisible = true;
                DailySurvey f = new DailySurvey();
                DialogResult dr = new DialogResult();
                dr = f.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    _logger.Feedback(false, f.Values, "DailyLog", "dailyLog.txt");
                }
                f.Dispose();
                _feedbackVisible = false;
            }
        }

        private void Feedback_event(object sender, EventArgs e)
        {
            if (preferences.Logging)
            {
                //Must launch Dialog in separate UI Thread.
                formDelegate f = showQuickFeedback;
                f.BeginInvoke(null, null);

            }
        }

        private void DailySurvey_event(object sender, EventArgs e)
        {
            if (preferences.Logging)
            {
                formDelegate f = showDailySurvey;
                f.BeginInvoke(null, null);
            }
        }

        private void ToggleMagnifier_event(object sender, EventArgs e)
        {
            ToggleActive();
        }

        private void temp_disable_Tick(object sender, EventArgs e)
        {
            temp_disable.Stop();
            if (!_magnifier.Active)
            {
                ToggleActive();

                formDelegate f = showQuickFeedback;
                f.BeginInvoke(null, null);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Switches the magnifier on and off.
        /// 
        /// If logging, will start the timer to force the magnifier to automatically re-appear.
        /// 
        /// Will also update the cursor so that it is appropriately hidden/visible.
        /// </summary>
        /// <param name="temp"></param>
        private void ToggleActive()
        {
            if (!preferences.Baseline)
            {
                if (_magnifier.Active)
                {
                    if (preferences.Logging) //If logging, only temporarily disable the magnifier
                        temp_disable.Start();
                    _magnifier.Active = false;
                }
                else
                {
                    if (temp_disable.Enabled)
                        temp_disable.Stop();
                    _magnifier.Center = Cursor.Position;
                    _magnifier.Active = true;
                }
            }
            else
            {
                if (_magnifier.Active)
                    _magnifier.Active = false;
            }
            // this is a magical event that makes sure the cursor is visible when the magnifier is disabled.
            _cursorWrapper.Visible = !_magnifier.Active;
            updateButtons();
        }

        /// <summary>
        /// Updates the text anywhere the magnifier can be toggled on and off.
        /// </summary>
        private void updateButtons()
        {
            String lbl = " (" + keyShortcut1.Shortcut + ")";
            String label = "Turn Off Pointing Magnifier" + lbl;
            if (!_magnifier.Active)
                label = "Turn On Pointing Magnifier" + lbl;
            tsEnable.Text = label;
            cbEnable.Text = label;
            menuEnable.Text = label;
        }

        /// <summary>
        /// Sets the maximum size of the magnifier based on the screen dimensions.
        /// </summary>
        private void SetMaximum()
        {
            int min = 100000000;
            foreach (Screen s in Screen.AllScreens)
            {
                min = Math.Min(s.Bounds.Width, min);
                min = Math.Min(s.Bounds.Height, min);
            }
            nudSize.Maximum = (int)(min / (preferences.Magnification * 2));
            if (preferences.Size > nudSize.Maximum)
            {
                preferences.Size = (int)nudSize.Maximum;
            }
            else
            {
                nudSize.Value = preferences.Size;
            }
            _magnifier.RadiusNM = preferences.Size;
            Invalidate();
        }
        #endregion

        #region Keyboard Shortcuts

        private void cbKeyboard_CheckedChanged(object sender, EventArgs e)
        {
            preferences.Shortcuts = cbKeyboard.Checked;
            keyShortcut1.Enabled = cbKeyboard.Checked;
            keyShortcut2.Enabled = cbKeyboard.Checked;
            keyShortcut3.Enabled = cbKeyboard.Checked;
        }
        /// <summary>
        /// on key down, check to see if the key pressed matches a keyboard shortcut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kHook_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (cbKeyboard.Checked)
            {
                if (keyShortcut1.Shortcut == e.KeyData)
                {
                    e.SuppressKeyPress = true;
                    ToggleActive();
                }
                if (keyShortcut2.Shortcut == e.KeyData)
                {
                    //Quit
                    e.SuppressKeyPress = true;
                    this.Close();
                }
                if (keyShortcut3.Shortcut == e.KeyData)
                {
                    e.SuppressKeyPress = true;
                    Feedback_event(this, null);
                }
            }
        }
        #endregion

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Win32.WM.SYSCOMMAND && m.WParam == (IntPtr)Win32.SC.CLOSE)
            {
                this.Hide();
                this.niVMM.ShowBalloonTip(500, "The Pointing Magnifier is here.", "The Pointing Magnifier is still running and can be accessed via this System Tray icon.", ToolTipIcon.Info);
            }
            else
            {
                base.WndProc(ref m);
            }
        } 
       
    }
}
