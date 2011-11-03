using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using System.Timers;

namespace PointingMagnifier
{
    public class Logger
    {
        #region Properties
        private string _fName;
        private string _fPath; // full path and filename without extension
        private XmlTextWriter _writer; // XML writer -- uses _fileNoExt.xml
        private List<Bitmap> _screens;
        private System.Timers.Timer _capture;
        private System.Timers.Timer _sendLogs;
        private bool _state;
        private bool _event;
        private bool _clickthrough;
        private Random _random;
        private static String URI = "https://students.washington.edu/ajansen7/logs/savelog.php";
        private static NameValueCollection NVC = new NameValueCollection(); //parameters passed to upload a file
        private static int SCRAPE_SCREEN = 5000; //how often to scrape the screen ~ every 10 seconds
        private static int SEND_LOGS = 3600000; //how often to send the log files ~ every hour
        private Preferences _preferences;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new logging object. Log will automatically be stored and uploaded periodically.
        /// Log consists of an xml file of movement data, quick feedback, daily feedback, and screenshots.
        /// </summary>
        public Logger(ref Preferences p)
        {
            _preferences = p;
            _screens = new List<Bitmap>();
            _random = new Random();
            NVC.Add("passkey", "alex");
            NVC.Add("id", Properties.Settings.Default.UID.ToString());

            StartStudy();
        }

        public void StartStudy()
        {
            if (_preferences.Logging)
            {
                _screens.Add(ScrapePixels());

                //start a new log file
                InitializeLog();

                _capture = new System.Timers.Timer();
                _capture.Interval = SCRAPE_SCREEN;
                _capture.Elapsed += new ElapsedEventHandler(_capture_Elapsed);
                _capture.Start();

                //Add timer to automatically send logs
                _sendLogs = new System.Timers.Timer();
                _sendLogs.Interval = SEND_LOGS;
                _sendLogs.Elapsed += new ElapsedEventHandler(_sendLogs_Elapsed);
                _sendLogs.Start();
            }
        }
        /// <summary>
        /// Initializes a new XML log file. 
        /// </summary>
        private bool InitializeLog()
        {
            bool success = true;
            _event = false;
            _clickthrough = false;
            // create the XML log for output and write the log header
            _fName = String.Format("{0}_{1}_{2}", Properties.Settings.Default.UID, DateTime.Today.DayOfYear, Environment.TickCount);
            _fPath = String.Format("{0}\\{1}", System.IO.Directory.GetCurrentDirectory(), _fName);
            _writer = new XmlTextWriter(_fPath + ".xml", Encoding.UTF8);
            _writer.Formatting = Formatting.Indented;
            try
            {
                //Required Tags
                _writer.WriteStartDocument(true);
                _writer.WriteStartElement("PointingMagnifier");
                _writer.WriteAttributeString("T", DateTime.Now.ToString("HH:mm:ss:fff"));
                _writer.WriteAttributeString("eT", _preferences.ElapsedTime.ToString());
                _state = true;
                _writer.WriteStartElement("State");
                _writer.WriteAttributeString("Active", "Initialized");
                _writer.WriteAttributeString("Study", XmlConvert.ToString(_preferences.Study));
                _writer.WriteAttributeString("Logging", XmlConvert.ToString(_preferences.Logging));
                _writer.WriteAttributeString("Baseline", XmlConvert.ToString(_preferences.Baseline));
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                _writer.Flush();
            }
            return success;
        }
        #endregion

        #region Form Events
        /// <summary>
        /// On timer tick, close the log file and attempt to write it to the server
        /// 
        /// Not, right now, there is zero redundancy. Need to store local copies! If not uploaded...move to a new folder.
        /// On each upload attempt, try first to upload other failed attempts?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _sendLogs_Elapsed(object sender, ElapsedEventArgs e)
        {
            UploadLog(true);
        }

        /// <summary>
        /// On tick, capture a screenshot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _capture_Elapsed(object sender, ElapsedEventArgs e)
        {
            _screens.Add(ScrapePixels());
            if (_screens.Count > 3)
            {
                
                for (int i = 0; i < _screens.Count - 3; i++)
                {
                    _screens[i].Dispose(); //release bitmap from memory. Woot
                    _screens.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Does any necessary cleaning before closing the logger
        /// </summary>
        public void Cleanup()
        {
            if (_sendLogs != null)
                _sendLogs.Dispose();
            if (_capture != null)
                _capture.Dispose();
        }
        #endregion

        #region File Uploading
        /// <summary>
        /// Attempts to cleanly close the logger so that logs will remain in tact.
        /// Will also attempt to upload the final log file.
        /// </summary>
        public void UploadLog(bool reset)
        {
            if (_preferences.Logging && _writer != null)
            {
                try
                {
                    if (_writer != null)
                        _writer.Close();
                }
                catch (XmlException xex)
                {
                    Console.WriteLine(xex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    UploadFile(new object[5] { URI, _fPath + ".xml", "log", "text/xml", NVC });
                    if (reset)
                        InitializeLog();
                }
            }
        }
        /// <summary>
        /// Starts a new thread and uploads a file
        /// </summary>
        /// <param name="args">An array of 5 parameters</param>
        /// <param name="url">the URI to make a request to</param>
        /// <param name="file">The file to post</param>
        /// <param name="paramName">identifying the name of the content (eg image or log)</param>
        /// <param name="contentType">the type of file being sent</param>
        /// <param name="nvc">any extra parameters which need to be included</param>
        private void UploadFile(object[] args)
        {
            Thread upload = new Thread(new ParameterizedThreadStart(JansenLib.HttpUploadFile));
            upload.Name = "Upload Helper";
            upload.Start(args);
        }
        #endregion

        #region Screen Shots
        /// <summary>
        /// Gets the most recent picture from the list of stored screenshots.
        /// </summary>
        public Bitmap CurrPic
        {
            get
            {
                if (_screens.Count == 0)
                    _screens.Add(ScrapePixels());
                return _screens[_random.Next(_screens.Count)];
            }
        }

        /// <summary>
        /// Updates the stored image with the current screen underneath the PM.
        /// </summary>
        private Bitmap ScrapePixels()
        {
            Rectangle bounds = System.Windows.Forms.Screen.FromPoint(Cursor.Position).Bounds;
            Bitmap bmpCk = new Bitmap(bounds.Width, bounds.Height);
            Graphics gck = Graphics.FromImage(bmpCk);
            gck.CopyFromScreen(new Point(0, 0), new Point(0, 0), bounds.Size);
            gck.Dispose();
            return bmpCk;
        }
        #endregion

        /// <summary>
        /// Adds feedback and the filename of the image (if any) included with the feedback.
        /// </summary>
        /// <param name="image">True if OK to upload image</param>
        /// <param name="feedback">Text to upload</param>
        /// <param name="filename">Filename that is being uploaded</param>
        public bool Feedback(bool image, String feedback, String fbtype, String filename)
        {
            bool success = true;
            if (_preferences.Logging && _writer != null)
            {
                try
                {
                    if (_clickthrough)
                    {
                        _writer.WriteEndElement();
                        _clickthrough = false;
                    }
                    if (_event)
                    {
                        _writer.WriteEndElement();
                        _event = false;
                    }
                    _writer.WriteStartElement("Feedback");
                    _writer.WriteAttributeString("Type", fbtype);
                    _writer.WriteAttributeString("Image", XmlConvert.ToString(image));
                    if (image)
                    {
                        _writer.WriteAttributeString("Filename", filename);
                    }
                    _writer.WriteAttributeString("T", DateTime.Now.ToString("HH:mm:ss:fff"));
                    _writer.WriteAttributeString("eT", _preferences.ElapsedTime.ToString());
                    _writer.WriteString(fbtype+": ");
                    _writer.WriteString(feedback);
                    _writer.WriteEndElement();
                    _writer.Flush();
                    if (image)
                    {
                        UploadFile(new object[5] { URI, @String.Format("{0}\\{1}", System.IO.Directory.GetCurrentDirectory(), filename),
                                "image", "image/jpeg", NVC});
                    }
                }
                catch (XmlException)
                {
                    success = false;
                }
                catch (Exception)
                {
                    success = false;
                }
            }
            return success;
        }

        

        /// <summary>
        /// Writes the updated preferences to the log file.
        /// 
        /// AHHH add the actual preferences perhaps???
        /// </summary>
        /// <returns></returns>
        public bool Preferences(String pref, String val)
        {
            if (_preferences.Logging)
            {
                bool success = true;
                try
                {
                    //Required Tags
                    _writer.WriteStartElement("Preferences");
                    _writer.WriteAttributeString(pref, val);
                    _writer.WriteAttributeString("eT", _preferences.ElapsedTime.ToString());
                    _writer.WriteEndElement();
                }
                catch (XmlException)
                {
                    success = false;
                }
                catch (Exception)
                {
                    success = false;
                }
                finally
                {
                    _writer.Flush();
                }
                return success;
            }
            return _preferences.Logging;
        }

        /// <summary>
        /// Closes the "event" tag associated with a serries of clicks
        /// </summary>
        /// <returns></returns>
        public bool ResetEvent()
        {
            if (_preferences.Logging && _writer != null)
            {
                bool success = true;
                try
                {
                    if (_event)
                    {
                        _writer.WriteEndElement();
                        _event = false;
                    }
                }
                catch (XmlException)
                {
                    success = false;
                }
                catch (Exception)
                {
                    success = false;
                }
                finally
                {
                    _writer.Flush();
                }
                return success;
            }
            return _preferences.Logging;
        }
        /// <summary>
        /// Writes an element including attributes about a mouse down event
        /// -Sender (real vs simulated)
        /// -Button
        /// -Time
        /// -Location
        /// -State
        /// </summary>
        /// <param name="magnified"></param>
        /// <returns></returns>
        public bool ME(MouseEventArgs e, String state, bool magnified, int count, bool synth)
        {
            if (_preferences.Logging && _writer != null)
            {
                bool success = true;
                try
                {
                    if (!_event)
                    {
                        _event = true;
                        _writer.WriteStartElement("Event");
                    }
                    //Required Tags
                    _writer.WriteStartElement("ME");
                    _writer.WriteAttributeString("Btn", e.Button.ToString());
                    _writer.WriteAttributeString("Btn_St", state);
                    _writer.WriteAttributeString("Btn_cnt", XmlConvert.ToString(count));
                    _writer.WriteAttributeString("X", XmlConvert.ToString(e.X));
                    _writer.WriteAttributeString("Y", XmlConvert.ToString(e.Y));
                    _writer.WriteAttributeString("Synth", XmlConvert.ToString(synth));
                    _writer.WriteAttributeString("Mag", XmlConvert.ToString(magnified));
                    _writer.WriteAttributeString("T", DateTime.Now.ToString("HH:mm:ss:fff"));
                    _writer.WriteAttributeString("eT", _preferences.ElapsedTime.ToString());
                    _writer.WriteEndElement();
                }
                catch (XmlException)
                {
                    success = false;
                }
                catch (Exception)
                {
                    success = false;
                }
                finally
                {
                    _writer.Flush();
                }
                return success;
            }
            return _preferences.Logging;
        }

        /// <summary>
        /// Writes a mouse movement event to the log file. 
        /// 
        /// Includes:
        /// position (x,y)
        /// time
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool MouseMove(MouseEventArgs e)
        {
            if (_preferences.Logging && _writer != null)
            {
                bool success = true;
                try
                {
                    //Required Tags
                    _writer.WriteStartElement("Move");
                    _writer.WriteAttributeString("X", XmlConvert.ToString(e.X));
                    _writer.WriteAttributeString("Y", XmlConvert.ToString(e.Y));
                    _writer.WriteAttributeString("T", DateTime.Now.ToString("HH:mm:ss:fff"));
                    _writer.WriteAttributeString("eT", _preferences.ElapsedTime.ToString());
                    _writer.WriteEndElement();
                }
                catch (XmlException)
                {
                    success = false;
                }
                catch (Exception)
                {
                    success = false;
                }
                finally
                {
                    _writer.Flush();
                }
                return success;
            }
            return _preferences.Logging;
        }

        /// <summary>
        /// Logs magnifier events:
        /// 
        /// -Reset
        /// -Magnify/demagnify
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public bool LogEvent(String evnt, NameValueCollection nvc)
        {
            if (_preferences.Logging && _writer != null)
            {
                bool success = true;
                try
                {
                    //Required Tags
                    _writer.WriteStartElement(evnt);
                    if (nvc != null)
                    {
                        foreach (String key in nvc.Keys)
                        {
                            _writer.WriteAttributeString(key, nvc[key]);
                        }
                    }
                    _writer.WriteAttributeString("T", DateTime.Now.ToString("HH:mm:ss:fff"));
                    _writer.WriteAttributeString("eT", _preferences.ElapsedTime.ToString());
                    _writer.WriteEndElement();
                }
                catch (XmlException)
                {
                    success = false;
                }
                catch (Exception)
                {
                    success = false;
                }
                finally
                {
                    _writer.Flush();
                }
                return success;
            }
            return _preferences.Logging;
        }

        /// <summary>
        /// Encapsulates all events that happen while the user can click through to the underlying application.
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public bool ClickThrough(bool transparent)
        {
            if (_preferences.Logging && _writer != null)
            {
                bool success = true;
                try
                {
                    //Required Tags
                    if (_clickthrough != transparent)
                    {
                        if (transparent)
                        {
                            _writer.WriteStartElement("Clickthrough");
                            _writer.WriteAttributeString("T", DateTime.Now.ToString("HH:mm:ss:fff"));
                            _writer.WriteAttributeString("eT", _preferences.ElapsedTime.ToString());
                        }
                        else
                        {
                            _writer.WriteEndElement();
                        }
                        _clickthrough = transparent;
                    }
                }
                catch (XmlException)
                {
                    success = false;
                }
                catch (Exception)
                {
                    success = false;
                }
                finally
                {
                    _writer.Flush();
                }
                return success;
            }
            return _preferences.Logging;
        }
        /// <summary>
        /// Logs changes in the magnifier state (active/deactive)
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        public bool ChangeState(bool active)
        {
            if (_preferences.Logging && _writer != null)
            {
                bool success = true;
                try
                {
                    if (_clickthrough)
                    {
                        _writer.WriteEndElement();
                        _clickthrough = false;
                    }
                    if (_event)
                    {
                        _writer.WriteEndElement();
                        _event = false;
                    }
                    if (_state)
                    {
                        _writer.WriteEndElement();
                    }
                    _state = true;
                    _writer.WriteStartElement("State");
                    _writer.WriteAttributeString("Study", XmlConvert.ToString(_preferences.Study));
                    _writer.WriteAttributeString("Logging", XmlConvert.ToString(_preferences.Logging));
                    _writer.WriteAttributeString("Baseline", XmlConvert.ToString(_preferences.Baseline));
                    _writer.WriteAttributeString("Active", XmlConvert.ToString(active));
                    _writer.WriteAttributeString("T", DateTime.Now.ToString("HH:mm:ss:fff"));
                    _writer.WriteAttributeString("eT", _preferences.ElapsedTime.ToString());
                }
                catch (XmlException)
                {
                    success = false;
                }
                catch (Exception)
                {
                    success = false;
                }
                finally
                {
                    _writer.Flush();
                }
                return success;
            }
            return _preferences.Logging;
        }
    }
}
