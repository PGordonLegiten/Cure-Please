namespace CurePlease
{
    using CurePlease.Helpers;
    using CurePlease.Properties;
    using CurePlease.DataStructures;
    using EliteMMO.API;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using CurePlease.Data;

    public partial class CurePleaseForm : Form
    {

        private OptionsForm Form2 = new CurePlease.OptionsForm();

        private CastingHelper CastingManager;
        private CureHelper CureManager;
        private JaHelper JaManager;


        public int currentSCHCharges = 0;

        private string debug_MSG_show = string.Empty;

        private int lastCommand = 0;

        private int lastKnownEstablisherTarget = 0;

        // BARD SONG VARIABLES
        private int song_casting = 0;

        private int PL_BRDCount = 0;
        private bool ForceSongRecast = false;
        private string Last_Song_Cast = string.Empty;


        private uint PL_Index = 0;
        private uint Monitored_Index = 0;


        //  private int song_casting = 0;
        //  private string LastSongCast = String.Empty;


        // private bool ForceSongRecast = false;
        //  private string Last_Song_Cast = String.Empty;


        // GEO ENGAGED CHECK
        public bool targetEngaged = false;

        //public bool EclipticStillUp = false;

        public string JobAbilityCMD = String.Empty;

        private DateTime DefaultTime = new DateTime(1970, 1, 1);

        private bool curePlease_autofollow = false;

        private List<string> characterNames_naRemoval = new List<string>();

        public enum LoginStatus
        {
            CharacterLoginScreen = 0,
            Loading = 1,
            LoggedIn = 2
        }

        public enum Status : byte
        {
            Standing = 0,
            Fighting = 1,
            Dead1 = 2,
            Dead2 = 3,
            Event = 4,
            Chocobo = 5,
            Healing = 33,
            Synthing = 44,
            Sitting = 47,
            Fishing = 56,
            FishBite = 57,
            Obtained = 58,
            RodBreak = 59,
            LineBreak = 60,
            CatchMonster = 61,
            LostCatch = 62,
            Unknown
        }

        public string WindowerMode = "Windower";

        public static EliteAPI _ELITEAPIPL;

        public EliteAPI _ELITEAPIMonitored;
        PlayerHelper playerHelper;

        public ListBox processids = new ListBox();

        public ListBox activeprocessids = new ListBox();

        public double last_percent = 1;

        public string castingSpell = string.Empty;

        public int max_count = 10;
        public int spell_delay_count = 0;

        public int geo_step = 0;

        public int followWarning = 0;

        public bool stuckWarning = false;
        public int stuckCount = 0;

        public int protectionCount = 0;

        public float lastZ;
        public float lastX;
        public float lastY;

        // Stores the previously-colored button, if any
        public List<BuffStorage> ActiveBuffs = new List<BuffStorage>();

        public List<string> TemporaryItem_Zones = new List<string> { "Escha Ru'Aun", "Escha Zi'Tah", "Reisenjima", "Abyssea - La Theine", "Abyssea - Konschtat", "Abyssea - Tahrongi",
                                                                        "Abyssea - Attohwa", "Abyssea - Misareaux", "Abyssea - Vunkerl", "Abyssea - Altepa", "Abyssea - Uleguerand", "Abyssea - Grauberg", "Walk of Echoes" };

        public string wakeSleepSpellName = "Cure";

        private float plX;

        private float plY;

        private float plZ;

        private string playerOptionsSelected;

        private string autoOptionsSelected;

        private bool pauseActions;

        private bool islowmp;

        public int LUA_Plugin_Loaded = 0;

        public int firstTime_Pause = 0;


           // SPELL CHECKER CODE: (playerHelper.CheckSpellRecast("") && (playerHelper.HasSpell(""))
        // ABILITY CHECKER CODE: JaManager.IsJaReady("") && (playerHelper.playerHelper.HasAbility(""))
        // PIANISSIMO TIME FORMAT
        // SONGNUMBER_SONGSET (Example: 1_2 = Song #1 in Set #2

        #region "datastructures"

        private DateTime currentTime = DateTime.Now;


        private DateTime[] playerSong1 = new DateTime[]
      {
            new DateTime(1970, 1, 1, 0, 0, 0)
      };

        private DateTime[] playerSong2 = new DateTime[]
      {
            new DateTime(1970, 1, 1, 0, 0, 0)
      };

        private DateTime[] playerSong3 = new DateTime[]
      {
            new DateTime(1970, 1, 1, 0, 0, 0)
      };

        private DateTime[] playerSong4 = new DateTime[]
      {
            new DateTime(1970, 1, 1, 0, 0, 0)
      };

        private DateTime[] Last_SongCast_Timer = new DateTime[]
      {
            new DateTime(1970, 1, 1, 0, 0, 0)
      };

        private DateTime[] playerPianissimo1_1 = new DateTime[]
      {
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0)
      };

        private DateTime[] playerPianissimo2_1 = new DateTime[]
      {
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0)
      };

        private DateTime[] playerPianissimo1_2 = new DateTime[]
      {
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0)
      };

        private DateTime[] playerPianissimo2_2 = new DateTime[]
      {
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0),
            new DateTime(1970, 1, 1, 0, 0, 0)
      };

        private TimeSpan[] playerSong1_Span = new TimeSpan[]
      {
            new TimeSpan()
      };

        private TimeSpan[] playerSong2_Span = new TimeSpan[]
      {
            new TimeSpan()
      };

        private TimeSpan[] playerSong3_Span = new TimeSpan[]
      {
            new TimeSpan()
      };

        private TimeSpan[] playerSong4_Span = new TimeSpan[]
     {
            new TimeSpan()
     };

        private TimeSpan[] Last_SongCast_Timer_Span = new TimeSpan[]
     {
            new TimeSpan()
     };

        private TimeSpan[] pianissimo1_1_Span = new TimeSpan[]
      {
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
      };

        private TimeSpan[] pianissimo2_1_Span = new TimeSpan[]
      {
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
      };

        private TimeSpan[] pianissimo1_2_Span = new TimeSpan[]
      {
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
      };

        private TimeSpan[] pianissimo2_2_Span = new TimeSpan[]
      {
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
            new TimeSpan(),
      };
        #endregion

        private void PaintBorderlessGroupBox(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            DrawGroupBox(box, e.Graphics, Color.Black, Color.Gray);
        }

        private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor)
        {
            if (box != null)
            {
                Brush textBrush = new SolidBrush(textColor);
                Brush borderBrush = new SolidBrush(borderColor);
                Pen borderPen = new Pen(borderBrush);
                SizeF strSize = g.MeasureString(box.Text, box.Font);
                Rectangle rect = new Rectangle(box.ClientRectangle.X,
                                           box.ClientRectangle.Y + (int)(strSize.Height / 2),
                                           box.ClientRectangle.Width - 1,
                                           box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);

                // Clear text and border
                g.Clear(BackColor);

                // Draw text
                g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);

                // Drawing Border
                //Left
                g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
                //Right
                g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Bottom
                g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
                //Top1
                g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
                //Top2
                g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
            }
        }

        private void PaintButton(object sender, PaintEventArgs e)
        {
            Button button = sender as Button;

            button.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
        }


        public CurePleaseForm()
        {
            StartPosition = FormStartPosition.CenterScreen;

            InitializeComponent();

            currentAction.Text = string.Empty;

            if (System.IO.File.Exists("debug"))
            {
                debug.Visible = true;
            }
            IEnumerable<Process> pol = Process.GetProcessesByName("pol").Union(Process.GetProcessesByName("xiloader")).Union(Process.GetProcessesByName("edenxi"));

            if (pol.Count() < 1)
            {
                MessageBox.Show("FFXI not found");
            }
            else
            {
                for (int i = 0; i < pol.Count(); i++)
                {
                    POLID.Items.Add(pol.ElementAt(i).MainWindowTitle);
                    POLID2.Items.Add(pol.ElementAt(i).MainWindowTitle);
                    processids.Items.Add(pol.ElementAt(i).Id);
                    activeprocessids.Items.Add(pol.ElementAt(i).Id);
                }
                POLID.SelectedIndex = 0;
                POLID2.SelectedIndex = 0;
                processids.SelectedIndex = 0;
                activeprocessids.SelectedIndex = 0;
            }
            // Show the current version number..
            Text = notifyIcon1.Text = "Cure Please v" + Application.ProductVersion;

            notifyIcon1.BalloonTipTitle = "Cure Please v" + Application.ProductVersion;
            notifyIcon1.BalloonTipText = "CurePlease has been minimized.";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
        }

        private void setinstance_Click(object sender, EventArgs e)
        {
            if (!CheckForDLLFiles())
            {
                MessageBox.Show(
                    "Unable to locate EliteAPI.dll or EliteMMO.API.dll\nMake sure both files are in the same directory as the application",
                    "Error");
                return;
            }

            processids.SelectedIndex = POLID.SelectedIndex;
            activeprocessids.SelectedIndex = POLID.SelectedIndex;
            _ELITEAPIPL = new EliteAPI((int)processids.SelectedItem);
            plLabel.Text = "Selected PL: " + _ELITEAPIPL.Player.Name;
            Text = notifyIcon1.Text = _ELITEAPIPL.Player.Name + " - " + "Cure Please v" + Application.ProductVersion;

            plLabel.ForeColor = Color.Green;
            POLID.BackColor = Color.White;
            plPosition.Enabled = true;
            setinstance2.Enabled = true;
            OptionsForm.config.autoFollowName = string.Empty;

            ForceSongRecast = true;

            foreach (Process dats in Process.GetProcessesByName("pol").Union(Process.GetProcessesByName("xiloader")).Union(Process.GetProcessesByName("edenxi")).Where(dats => POLID.Text == dats.MainWindowTitle))
            {
                for (int i = 0; i < dats.Modules.Count; i++)
                {
                    if (dats.Modules[i].FileName.Contains("Ashita.dll"))
                    {
                        WindowerMode = "Ashita";
                    }
                    else if (dats.Modules[i].FileName.Contains("Hook.dll"))
                    {
                        WindowerMode = "Windower";
                    }
                }
            }

            if (firstTime_Pause == 0)
            {
                Follow_BGW.RunWorkerAsync();
                AddonReader.RunWorkerAsync();
                firstTime_Pause = 1;
            }

            // LOAD AUTOMATIC SETTINGS
            string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Settings");
            if (System.IO.File.Exists(path + "/loadSettings"))
            {
                if (_ELITEAPIPL.Player.MainJob != 0)
                {
                    if (_ELITEAPIPL.Player.SubJob != 0)
                    {
                        JobTitles mainJob = Jobs.JobNames.Where(c => c.job_number == _ELITEAPIPL.Player.MainJob).FirstOrDefault();
                        JobTitles subJob = Jobs.JobNames.Where(c => c.job_number == _ELITEAPIPL.Player.SubJob).FirstOrDefault();

                        string filename = path + "\\" + _ELITEAPIPL.Player.Name + "_" + mainJob.job_name + "_" + subJob.job_name + ".xml";
                        string filename2 = path + "\\" + mainJob.job_name + "_" + subJob.job_name + ".xml";


                        if (System.IO.File.Exists(filename))
                        {
                            OptionsForm.MySettings config = new OptionsForm.MySettings();

                            XmlSerializer mySerializer = new XmlSerializer(typeof(OptionsForm.MySettings));

                            StreamReader reader = new StreamReader(filename);
                            config = (OptionsForm.MySettings)mySerializer.Deserialize(reader);

                            reader.Close();
                            reader.Dispose();
                            Form2.updateForm(config);
                            Form2.button4_Click(sender, e);
                        }
                        else if (System.IO.File.Exists(filename2))
                        {
                            OptionsForm.MySettings config = new OptionsForm.MySettings();

                            XmlSerializer mySerializer = new XmlSerializer(typeof(OptionsForm.MySettings));

                            StreamReader reader = new StreamReader(filename2);
                            config = (OptionsForm.MySettings)mySerializer.Deserialize(reader);

                            reader.Close();
                            reader.Dispose();
                            Form2.updateForm(config);
                            Form2.button4_Click(sender, e);
                        }
                    }
                }
            }

            if (LUA_Plugin_Loaded == 0 && !OptionsForm.config.pauseOnStartBox && _ELITEAPIMonitored != null)
            {
                // Wait a milisecond and then load and set the config.
                Thread.Sleep(500);

                if (WindowerMode == "Windower")
                {
                    _ELITEAPIPL.ThirdParty.SendString("//lua load CurePlease_addon");
                    Thread.Sleep(1500);
                    _ELITEAPIPL.ThirdParty.SendString("//cpaddon settings " + OptionsForm.config.ipAddress + " " + OptionsForm.config.listeningPort);
                    Thread.Sleep(100);
                    _ELITEAPIPL.ThirdParty.SendString("//cpaddon verify");
                    if (OptionsForm.config.enableHotKeys)
                    {
                        _ELITEAPIPL.ThirdParty.SendString("//bind ^!F1 cureplease toggle");
                        _ELITEAPIPL.ThirdParty.SendString("//bind ^!F2 cureplease start");
                        _ELITEAPIPL.ThirdParty.SendString("//bind ^!F3 cureplease pause");
                    }
                }
                else if (WindowerMode == "Ashita")
                {
                    _ELITEAPIPL.ThirdParty.SendString("/addon load CurePlease_addon");
                    Thread.Sleep(1500);
                    _ELITEAPIPL.ThirdParty.SendString("/cpaddon settings " + OptionsForm.config.ipAddress + " " + OptionsForm.config.listeningPort);
                    Thread.Sleep(100);

                    _ELITEAPIPL.ThirdParty.SendString("/cpaddon verify");
                    if (OptionsForm.config.enableHotKeys)
                    {
                        _ELITEAPIPL.ThirdParty.SendString("/bind ^!F1 /cureplease toggle");
                        _ELITEAPIPL.ThirdParty.SendString("/bind ^!F2 /cureplease start");
                        _ELITEAPIPL.ThirdParty.SendString("/bind ^!F3 /cureplease pause");
                    }
                }

                AddOnStatus_Click(sender, e);

                currentAction.Text = "LUA Addon loaded. ( " + OptionsForm.config.ipAddress + " - " + OptionsForm.config.listeningPort + " )";

                LUA_Plugin_Loaded = 1;
            }
        }

        private void setinstance2_Click(object sender, EventArgs e)
        {
            if (!CheckForDLLFiles())
            {
                MessageBox.Show(
                    "Unable to locate EliteAPI.dll or EliteMMO.API.dll\nMake sure both files are in the same directory as the application",
                    "Error");
                return;
            }
            processids.SelectedIndex = POLID2.SelectedIndex;
            _ELITEAPIMonitored = new EliteAPI((int)processids.SelectedItem);
            playerHelper = new PlayerHelper(_ELITEAPIPL, _ELITEAPIMonitored);
            CastingManager = new CastingHelper(this, _ELITEAPIPL, _ELITEAPIMonitored); //TODO MIGHT HAVE TO INJECT ON PL SELECTION WHEN CHANGING PL TBS
            CureManager = new CureHelper(this, _ELITEAPIPL, _ELITEAPIMonitored, CastingManager); //TODO MIGHT HAVE TO INJECT ON PL SELECTION WHEN CHANGING PL TBS
            JaManager = new JaHelper(this, _ELITEAPIPL, _ELITEAPIMonitored, CastingManager); //TODO MIGHT HAVE TO INJECT ON PL SELECTION WHEN CHANGING PL TBS

            monitoredLabel.Text = "Monitoring: " + _ELITEAPIMonitored.Player.Name;
            monitoredLabel.ForeColor = Color.Green;
            POLID2.BackColor = Color.White;
            pauseButton.Enabled = true;
            EnableTickers();

            if (OptionsForm.config.pauseOnStartBox)
            {
                pauseActions = true;
                pauseButton.Text = "Loaded, Paused!";
                pauseButton.ForeColor = Color.Red;
                DisableTickers();
            }
            else
            {
                if (OptionsForm.config.MinimiseonStart == true && WindowState != FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Minimized;
                }
            }

            if (LUA_Plugin_Loaded == 0 && !OptionsForm.config.pauseOnStartBox && _ELITEAPIPL != null)
            {
                // Wait a milisecond and then load and set the config.
                Thread.Sleep(500);
                if (WindowerMode == "Windower")
                {
                    _ELITEAPIPL.ThirdParty.SendString("//lua load CurePlease_addon");
                    Thread.Sleep(1500);
                    _ELITEAPIPL.ThirdParty.SendString("//cpaddon settings " + OptionsForm.config.ipAddress + " " + OptionsForm.config.listeningPort);
                    Thread.Sleep(100);
                    _ELITEAPIPL.ThirdParty.SendString("//cpaddon verify");

                    if (OptionsForm.config.enableHotKeys)
                    {
                        _ELITEAPIPL.ThirdParty.SendString("//bind ^!F1 cureplease toggle");
                        _ELITEAPIPL.ThirdParty.SendString("//bind ^!F2 cureplease start");
                        _ELITEAPIPL.ThirdParty.SendString("//bind ^!F3 cureplease pause");
                    }
                }
                else if (WindowerMode == "Ashita")
                {
                    _ELITEAPIPL.ThirdParty.SendString("/addon load CurePlease_addon");
                    Thread.Sleep(1500);
                    _ELITEAPIPL.ThirdParty.SendString("/cpaddon settings " + OptionsForm.config.ipAddress + " " + OptionsForm.config.listeningPort);
                    Thread.Sleep(100);
                    _ELITEAPIPL.ThirdParty.SendString("/cpaddon verify");
                    if (OptionsForm.config.enableHotKeys)
                    {
                        _ELITEAPIPL.ThirdParty.SendString("/bind ^!F1 /cureplease toggle");
                        _ELITEAPIPL.ThirdParty.SendString("/bind ^!F2 /cureplease start");
                        _ELITEAPIPL.ThirdParty.SendString("/bind ^!F3 /cureplease pause");
                    }
                }

                currentAction.Text = "LUA Addon loaded. ( " + OptionsForm.config.ipAddress + " - " + OptionsForm.config.listeningPort + " )";

                LUA_Plugin_Loaded = 1;

                AddOnStatus_Click(sender, e);

                lastCommand = _ELITEAPIMonitored.ThirdParty.ConsoleIsNewCommand();
            }
        }

        private bool CheckForDLLFiles()
        {
            if (!File.Exists("eliteapi.dll") || !File.Exists("elitemmo.api.dll"))
            {
                return false;
            }
            return true;
        }


        public void EnableTickers()
        {
            cureTimer.Enabled = true;
            partyMembersUpdate.Enabled = true;
            actionTimer.Enabled = true;
            castingTimer.Enabled = true;
            hpUpdates.Enabled = true;
        }
        public void DisableTickers()
        {
            cureTimer.Enabled = false;
            partyMembersUpdate.Enabled = false;
            actionTimer.Enabled = false;
            castingTimer.Enabled = false;
            hpUpdates.Enabled = false;
        }

        private bool partyMemberUpdateMethod(byte partyMemberId)
        {
            if (_ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].Active >= 1)
            {
                if (_ELITEAPIPL.Player.ZoneId == _ELITEAPIMonitored.Party.GetPartyMembers()[partyMemberId].Zone)
                {
                    return true;
                }

                return false;
            }
            return false;
        }

        private void UpdateRaiseButton(Button raiseButton, Button autoButton, byte pos)
        {
            EliteAPI.PartyMember member = _ELITEAPIMonitored.Party.GetPartyMember(pos);
            if (raiseButton != null)
            {
                if (member.Active == 1 && member.CurrentHP <= 0 && member.Zone == _ELITEAPIPL.Player.ZoneId) 
                {

                    raiseButton.Visible = true;
                    raiseButton.Enabled = false;
                    if (playerHelper.CastingPossible(member.Name) && !playerHelper.IsAlive(member.Name))
                    {
                        raiseButton.Enabled = true;
                    }
                    autoButton.Enabled = false;
                    autoButton.Visible = false;
                }
                else
                {
                    raiseButton.Visible = false;
                    raiseButton.Enabled = false;
                    autoButton.Visible = true;
                    autoButton.Enabled = true;
                }
            }
        }

        private async void partyMembersUpdate_TickAsync(object sender, EventArgs e)
        {
            if (_ELITEAPIPL == null || _ELITEAPIMonitored == null)
            {
                return;
            }

            if (_ELITEAPIPL.Player.LoginStatus == (int)LoginStatus.Loading || _ELITEAPIMonitored.Player.LoginStatus == (int)LoginStatus.Loading)
            {
                if (OptionsForm.config.pauseOnZoneBox == true)
                {
                    song_casting = 0;
                    ForceSongRecast = true;
                    if (pauseActions != true)
                    {
                        pauseButton.Text = "Zoned, paused.";
                        pauseButton.ForeColor = Color.Red;
                        pauseActions = true;
                        DisableTickers();
                    }
                }
                else
                {
                    song_casting = 0;
                    ForceSongRecast = true;

                    if (pauseActions != true)
                    {
                        pauseButton.Text = "Zoned, waiting.";
                        pauseButton.ForeColor = Color.Red;
                        await Task.Delay(100);
                        Thread.Sleep(17000);
                        pauseButton.Text = "Pause";
                        pauseButton.ForeColor = Color.Black;
                    }
                }
                ActiveBuffs.Clear();
                InternalHelper.resetAllCooldowns();
            }

            if (_ELITEAPIPL.Player.LoginStatus != (int)LoginStatus.LoggedIn || _ELITEAPIMonitored.Player.LoginStatus != (int)LoginStatus.LoggedIn)
            {
                return;
            }

            if (partyMemberUpdateMethod(0))
            {
                player0.Text = _ELITEAPIMonitored.Party.GetPartyMember(0).Name;
                player0.Enabled = true;
                player0optionsButton.Enabled = true;
                player0buffsButton.Enabled = true;
                UpdateRaiseButton(player0raiseButton, player0buffsButton, 0);
            }
            else
            {
                player0.Text = "Inactive or out of zone";
                player0.Enabled = false;
                player0HP.Value = 0;
                player0optionsButton.Enabled = false;
                player0buffsButton.Enabled = false;
                player0raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(1))
            {
                player1.Text = _ELITEAPIMonitored.Party.GetPartyMember(1).Name;
                player1.Enabled = true;
                player1optionsButton.Enabled = true;
                player1buffsButton.Enabled = true;
                UpdateRaiseButton(player1raiseButton, player1buffsButton, 1);
            }
            else
            {
                player1.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player1.Enabled = false;
                player1HP.Value = 0;
                player1optionsButton.Enabled = false;
                player1buffsButton.Enabled = false;
                player1raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(2))
            {
                player2.Text = _ELITEAPIMonitored.Party.GetPartyMember(2).Name;
                player2.Enabled = true;
                player2optionsButton.Enabled = true;
                player2buffsButton.Enabled = true;
                UpdateRaiseButton(player2raiseButton, player2buffsButton, 2);
            }
            else
            {
                player2.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player2.Enabled = false;
                player2HP.Value = 0;
                player2optionsButton.Enabled = false;
                player2buffsButton.Enabled = false;
                player2raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(3))
            {
                player3.Text = _ELITEAPIMonitored.Party.GetPartyMember(3).Name;
                player3.Enabled = true;
                player3optionsButton.Enabled = true;
                player3buffsButton.Enabled = true;
                UpdateRaiseButton(player3raiseButton, player3buffsButton, 3);
            }
            else
            {
                player3.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player3.Enabled = false;
                player3HP.Value = 0;
                player3optionsButton.Enabled = false;
                player3buffsButton.Enabled = false;
                player3raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(4))
            {
                player4.Text = _ELITEAPIMonitored.Party.GetPartyMember(4).Name;
                player4.Enabled = true;
                player4optionsButton.Enabled = true;
                player4buffsButton.Enabled = true;
                UpdateRaiseButton(player4raiseButton, player4buffsButton, 4);
            }
            else
            {
                player4.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player4.Enabled = false;
                player4HP.Value = 0;
                player4optionsButton.Enabled = false;
                player4buffsButton.Enabled = false;
                player4raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(5))
            {
                player5.Text = _ELITEAPIMonitored.Party.GetPartyMember(5).Name;
                player5.Enabled = true;
                player5optionsButton.Enabled = true;
                player5buffsButton.Enabled = true;
                UpdateRaiseButton(player5raiseButton, player5buffsButton, 5);
            }
            else
            {
                player5.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player5.Enabled = false;
                player5HP.Value = 0;
                player5optionsButton.Enabled = false;
                player5buffsButton.Enabled = false;
                player5raiseButton.Visible = false;
            }
            if (partyMemberUpdateMethod(6))
            {
                player6.Text = _ELITEAPIMonitored.Party.GetPartyMember(6).Name;
                player6.Enabled = true;
                player6optionsButton.Enabled = true;
                UpdateRaiseButton(player6raiseButton, player6optionsButton, 6);
            }
            else
            {
                player6.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player6.Enabled = false;
                player6HP.Value = 0;
                player6optionsButton.Enabled = false;
                player6raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(7))
            {
                player7.Text = _ELITEAPIMonitored.Party.GetPartyMember(7).Name;
                player7.Enabled = true;
                player7optionsButton.Enabled = true;
                UpdateRaiseButton(player7raiseButton, player7optionsButton, 7);
            }
            else
            {
                player7.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player7.Enabled = false;
                player7HP.Value = 0;
                player7optionsButton.Enabled = false;
                player7raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(8))
            {
                player8.Text = _ELITEAPIMonitored.Party.GetPartyMember(8).Name;
                player8.Enabled = true;
                player8optionsButton.Enabled = true;
                UpdateRaiseButton(player8raiseButton, player8optionsButton, 8);
            }
            else
            {
                player8.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player8.Enabled = false;
                player8HP.Value = 0;
                player8optionsButton.Enabled = false;
                player8raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(9))
            {
                player9.Text = _ELITEAPIMonitored.Party.GetPartyMember(9).Name;
                player9.Enabled = true;
                player9optionsButton.Enabled = true;
                UpdateRaiseButton(player9raiseButton, player9optionsButton, 9);
            }
            else
            {
                player9.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player9.Enabled = false;
                player9HP.Value = 0;
                player9optionsButton.Enabled = false;
                player9raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(10))
            {
                player10.Text = _ELITEAPIMonitored.Party.GetPartyMember(10).Name;
                player10.Enabled = true;
                player10optionsButton.Enabled = true;
                UpdateRaiseButton(player10raiseButton, player10optionsButton, 10);
            }
            else
            {
                player10.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player10.Enabled = false;
                player10HP.Value = 0;
                player10optionsButton.Enabled = false;
                player10raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(11))
            {
                player11.Text = _ELITEAPIMonitored.Party.GetPartyMember(11).Name;
                player11.Enabled = true;
                player11optionsButton.Enabled = true;
                UpdateRaiseButton(player11raiseButton, player11optionsButton, 11);
            }
            else
            {
                player11.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player11.Enabled = false;
                player11HP.Value = 0;
                player11optionsButton.Enabled = false;
                player11raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(12))
            {
                player12.Text = _ELITEAPIMonitored.Party.GetPartyMember(12).Name;
                player12.Enabled = true;
                player12optionsButton.Enabled = true;
                UpdateRaiseButton(player12raiseButton, player12optionsButton, 12);
            }
            else
            {
                player12.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player12.Enabled = false;
                player12HP.Value = 0;
                player12optionsButton.Enabled = false;
                player12raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(13))
            {
                player13.Text = _ELITEAPIMonitored.Party.GetPartyMember(13).Name;
                player13.Enabled = true;
                player13optionsButton.Enabled = true;
                UpdateRaiseButton(player13raiseButton, player13optionsButton, 13);
            }
            else
            {
                player13.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player13.Enabled = false;
                player13HP.Value = 0;
                player13optionsButton.Enabled = false;
                player13raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(14))
            {
                player14.Text = _ELITEAPIMonitored.Party.GetPartyMember(14).Name;
                player14.Enabled = true;
                player14optionsButton.Enabled = true;
                UpdateRaiseButton(player14raiseButton, player14optionsButton, 14);
            }
            else
            {
                player14.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player14.Enabled = false;
                player14HP.Value = 0;
                player14optionsButton.Enabled = false;
                player14raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(15))
            {
                player15.Text = _ELITEAPIMonitored.Party.GetPartyMember(15).Name;
                player15.Enabled = true;
                player15optionsButton.Enabled = true;
                UpdateRaiseButton(player15raiseButton, player15optionsButton, 15);
            }
            else
            {
                player15.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player15.Enabled = false;
                player15HP.Value = 0;
                player15optionsButton.Enabled = false;
                player15raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(16))
            {
                player16.Text = _ELITEAPIMonitored.Party.GetPartyMember(16).Name;
                player16.Enabled = true;
                player16optionsButton.Enabled = true;
                UpdateRaiseButton(player16raiseButton, player16optionsButton, 16);
            }
            else
            {
                player16.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player16.Enabled = false;
                player16HP.Value = 0;
                player16optionsButton.Enabled = false;
                player16raiseButton.Visible = false;
            }

            if (partyMemberUpdateMethod(17))
            {
                player17.Text = _ELITEAPIMonitored.Party.GetPartyMember(17).Name;
                player17.Enabled = true;
                player17optionsButton.Enabled = true;
                UpdateRaiseButton(player17raiseButton, player17optionsButton, 17);
            }
            else
            {
                player17.Text = Resources.CurePleaseForm_partyMembersUpdate_Tick_Inactive;
                player17.Enabled = false;
                player17HP.Value = 0;
                player17optionsButton.Enabled = false;
                player17raiseButton.Visible = false;
            }
        }

        private void hpUpdates_Tick(object sender, EventArgs e)
        {
            if (_ELITEAPIPL == null || _ELITEAPIMonitored == null)
            {
                return;
            }

            if (_ELITEAPIPL.Player.LoginStatus != (int)LoginStatus.LoggedIn || _ELITEAPIMonitored.Player.LoginStatus != (int)LoginStatus.LoggedIn)
            {
                return;
            }

            if (player0.Enabled)
            {
                UpdateHPProgressBar(player0HP, _ELITEAPIMonitored.Party.GetPartyMember(0).CurrentHPP);
            }

            if (player0.Enabled)
            {
                UpdateHPProgressBar(player0HP, _ELITEAPIMonitored.Party.GetPartyMember(0).CurrentHPP);
            }

            if (player1.Enabled)
            {
                UpdateHPProgressBar(player1HP, _ELITEAPIMonitored.Party.GetPartyMember(1).CurrentHPP);
            }

            if (player2.Enabled)
            {
                UpdateHPProgressBar(player2HP, _ELITEAPIMonitored.Party.GetPartyMember(2).CurrentHPP);
            }

            if (player3.Enabled)
            {
                UpdateHPProgressBar(player3HP, _ELITEAPIMonitored.Party.GetPartyMember(3).CurrentHPP);
            }

            if (player4.Enabled)
            {
                UpdateHPProgressBar(player4HP, _ELITEAPIMonitored.Party.GetPartyMember(4).CurrentHPP);
            }

            if (player5.Enabled)
            {
                UpdateHPProgressBar(player5HP, _ELITEAPIMonitored.Party.GetPartyMember(5).CurrentHPP);
            }

            if (player6.Enabled)
            {
                UpdateHPProgressBar(player6HP, _ELITEAPIMonitored.Party.GetPartyMember(6).CurrentHPP);
            }

            if (player7.Enabled)
            {
                UpdateHPProgressBar(player7HP, _ELITEAPIMonitored.Party.GetPartyMember(7).CurrentHPP);
            }

            if (player8.Enabled)
            {
                UpdateHPProgressBar(player8HP, _ELITEAPIMonitored.Party.GetPartyMember(8).CurrentHPP);
            }

            if (player9.Enabled)
            {
                UpdateHPProgressBar(player9HP, _ELITEAPIMonitored.Party.GetPartyMember(9).CurrentHPP);
            }

            if (player10.Enabled)
            {
                UpdateHPProgressBar(player10HP, _ELITEAPIMonitored.Party.GetPartyMember(10).CurrentHPP);
            }

            if (player11.Enabled)
            {
                UpdateHPProgressBar(player11HP, _ELITEAPIMonitored.Party.GetPartyMember(11).CurrentHPP);
            }

            if (player12.Enabled)
            {
                UpdateHPProgressBar(player12HP, _ELITEAPIMonitored.Party.GetPartyMember(12).CurrentHPP);
            }

            if (player13.Enabled)
            {
                UpdateHPProgressBar(player13HP, _ELITEAPIMonitored.Party.GetPartyMember(13).CurrentHPP);
            }

            if (player14.Enabled)
            {
                UpdateHPProgressBar(player14HP, _ELITEAPIMonitored.Party.GetPartyMember(14).CurrentHPP);
            }

            if (player15.Enabled)
            {
                UpdateHPProgressBar(player15HP, _ELITEAPIMonitored.Party.GetPartyMember(15).CurrentHPP);
            }

            if (player16.Enabled)
            {
                UpdateHPProgressBar(player16HP, _ELITEAPIMonitored.Party.GetPartyMember(16).CurrentHPP);
            }

            if (player17.Enabled)
            {
                UpdateHPProgressBar(player17HP, _ELITEAPIMonitored.Party.GetPartyMember(17).CurrentHPP);
            }
        }

        private void UpdateHPProgressBar(ProgressBar playerHP, int CurrentHPP)
        {
            playerHP.Value = CurrentHPP;
            if (CurrentHPP >= 75)
            {
                playerHP.ForeColor = Color.DarkGreen;
            }
            else if (CurrentHPP > 50 && CurrentHPP < 75)
            {
                playerHP.ForeColor = Color.Yellow;
            }
            else if (CurrentHPP > 25 && CurrentHPP < 50)
            {
                playerHP.ForeColor = Color.Orange;
            }
            else if (CurrentHPP < 25)
            {
                playerHP.ForeColor = Color.Red;
            }
        }

        private void plPosition_Tick(object sender, EventArgs e)
        {
            if (_ELITEAPIPL == null || _ELITEAPIMonitored == null)
            {
                return;
            }

            if (_ELITEAPIPL.Player.LoginStatus != (int)LoginStatus.LoggedIn || _ELITEAPIMonitored.Player.LoginStatus != (int)LoginStatus.LoggedIn)
            {
                return;
            }

            plX = _ELITEAPIPL.Player.X;
            plY = _ELITEAPIPL.Player.Y;
            plZ = _ELITEAPIPL.Player.Z;
            CastingManager.plX = _ELITEAPIPL.Player.X;
            CastingManager.plY = _ELITEAPIPL.Player.Y;
            CastingManager.plZ = _ELITEAPIPL.Player.Z;
        }        

        private void RunDebuffChecker()
        {
            // PL and Monitored Player Debuff Removal Starting with PL
            
            if (OptionsForm.config.wakeSleepSpell == 0)
            {
                wakeSleepSpellName = "Cure";
            }
            else if (OptionsForm.config.wakeSleepSpell == 1)
            {
                wakeSleepSpellName = "Cura";
            }
            else if (OptionsForm.config.wakeSleepSpell == 2)
            {
                wakeSleepSpellName = "Curaga";
            }

            foreach (StatusEffect plEffect in _ELITEAPIPL.Player.Buffs)
            {
                #region "pl_debuffs"
                if ((plEffect == StatusEffect.Doom) && (OptionsForm.config.plDoom) && playerHelper.IsAbleToCastSpell("Cursna"))
                {
                    CastingManager.QueueSpell(SpellType.Prio, _ELITEAPIPL.Player.Name, "Cursna");
                }
                if ((plEffect == StatusEffect.Paralysis) && (OptionsForm.config.plParalysis) && playerHelper.IsAbleToCastSpell("Paralyna"))
                {
                    CastingManager.QueueSpell(SpellType.Prio, _ELITEAPIPL.Player.Name, "Paralyna");
                }
                if ((plEffect == StatusEffect.Amnesia) && (OptionsForm.config.plAmnesia) && playerHelper.IsAbleToCastSpell("Esuna") && BuffChecker(0, 418))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Esuna");
                }
                if ((plEffect == StatusEffect.Poison) && (OptionsForm.config.plPoison) && playerHelper.IsAbleToCastSpell("Poisona"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Poisona");
                }
                if ((plEffect == StatusEffect.Attack_Down) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                if ((plEffect == StatusEffect.Blindness) && (OptionsForm.config.plBlindness) && playerHelper.IsAbleToCastSpell("Blindna"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Blindna");
                }
                if ((plEffect == StatusEffect.Bind) && (OptionsForm.config.plBind) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                if ((plEffect == StatusEffect.Weight) && (OptionsForm.config.plWeight) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                if ((plEffect == StatusEffect.Slow) && (OptionsForm.config.plSlow) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                if ((plEffect == StatusEffect.Curse) && (OptionsForm.config.plCurse) && playerHelper.IsAbleToCastSpell("Cursna"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Cursna", SpellPrio.High);
                }
                if ((plEffect == StatusEffect.Curse2) && (OptionsForm.config.plCurse2) && playerHelper.IsAbleToCastSpell("Cursna"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Cursna", SpellPrio.High);
                }
                if ((plEffect == StatusEffect.Addle) && (OptionsForm.config.plAddle) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                if ((plEffect == StatusEffect.Bane) && (OptionsForm.config.plBane) && playerHelper.IsAbleToCastSpell("Cursna"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Cursna");
                }
                if ((plEffect == StatusEffect.Plague) && (OptionsForm.config.plPlague) && playerHelper.IsAbleToCastSpell("Viruna"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Viruna");
                }
                if ((plEffect == StatusEffect.Disease) && (OptionsForm.config.plDisease) && playerHelper.IsAbleToCastSpell("Viruna"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Viruna");
                }
                if ((plEffect == StatusEffect.Burn) && (OptionsForm.config.plBurn) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Frost) && (OptionsForm.config.plFrost) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Choke) && (OptionsForm.config.plChoke) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Rasp) && (OptionsForm.config.plRasp) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Shock) && (OptionsForm.config.plShock) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Drown) && (OptionsForm.config.plDrown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Dia) && (OptionsForm.config.plDia) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Bio) && (OptionsForm.config.plBio) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.STR_Down) && (OptionsForm.config.plStrDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.DEX_Down) && (OptionsForm.config.plDexDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.VIT_Down) && (OptionsForm.config.plVitDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.AGI_Down) && (OptionsForm.config.plAgiDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.INT_Down) && (OptionsForm.config.plIntDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.MND_Down) && (OptionsForm.config.plMndDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.CHR_Down) && (OptionsForm.config.plChrDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Max_HP_Down) && (OptionsForm.config.plMaxHpDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Max_MP_Down) && (OptionsForm.config.plMaxMpDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Accuracy_Down) && (OptionsForm.config.plAccuracyDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Evasion_Down) && (OptionsForm.config.plEvasionDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Defense_Down) && (OptionsForm.config.plDefenseDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Flash) && (OptionsForm.config.plFlash) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Magic_Acc_Down) && (OptionsForm.config.plMagicAccDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Magic_Atk_Down) && (OptionsForm.config.plMagicAtkDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Helix) && (OptionsForm.config.plHelix) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Max_TP_Down) && (OptionsForm.config.plMaxTpDown) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Requiem) && (OptionsForm.config.plRequiem) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Elegy) && (OptionsForm.config.plElegy) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                else if ((plEffect == StatusEffect.Threnody) && (OptionsForm.config.plThrenody) && (OptionsForm.config.plAttackDown) && playerHelper.IsAbleToCastSpell("Erase"))
                {
                    CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIPL.Player.Name, "Erase");
                }
                #endregion
            }
            #region "monitored_player"
            // Next, we check monitored player
            if (playerHelper.CastingPossible(_ELITEAPIMonitored.Player.Name))
            {
                foreach (StatusEffect monitoredEffect in _ELITEAPIMonitored.Player.Buffs)
                {
                    if ((monitoredEffect == StatusEffect.Doom) && (OptionsForm.config.monitoredDoom) && playerHelper.IsAbleToCastSpell("Cursna"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Cursna", SpellPrio.Top);
                    }
                    if ((monitoredEffect == StatusEffect.Sleep) && (OptionsForm.config.monitoredSleep) && (OptionsForm.config.wakeSleepEnabled))
                    {
                        CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIMonitored.Player.Name, wakeSleepSpellName, CurePrio.CureIV);
                    }
                    if ((monitoredEffect == StatusEffect.Sleep2) && (OptionsForm.config.monitoredSleep2) && (OptionsForm.config.wakeSleepEnabled))
                    {
                        CastingManager.QueueSpell(SpellType.Healing, _ELITEAPIMonitored.Player.Name, wakeSleepSpellName, CurePrio.CureIV);
                    }
                    if ((monitoredEffect == StatusEffect.Silence) && (OptionsForm.config.monitoredSilence) && playerHelper.IsAbleToCastSpell("Silena"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Silena");
                    }
                    if ((monitoredEffect == StatusEffect.Petrification) && (OptionsForm.config.monitoredPetrification) && playerHelper.IsAbleToCastSpell("Stona"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Stona", SpellPrio.High);
                    }
                    if ((monitoredEffect == StatusEffect.Paralysis) && (OptionsForm.config.monitoredParalysis) && playerHelper.IsAbleToCastSpell("Paralyna"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Paralyna");
                    }
                    if ((monitoredEffect == StatusEffect.Amnesia) && (OptionsForm.config.monitoredAmnesia) && playerHelper.IsAbleToCastSpell("Esuna") && BuffChecker(0, 418))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Esuna");
                    }
                    if ((monitoredEffect == StatusEffect.Poison) && (OptionsForm.config.monitoredPoison) && playerHelper.IsAbleToCastSpell("Poisona"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Poisona");
                    }
                    if ((monitoredEffect == StatusEffect.Attack_Down) && (OptionsForm.config.monitoredAttackDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    if ((monitoredEffect == StatusEffect.Blindness) && (OptionsForm.config.monitoredBlindness) && playerHelper.IsAbleToCastSpell("Blindna"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Blindna");
                    }
                    if ((monitoredEffect == StatusEffect.Bind) && (OptionsForm.config.monitoredBind) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    if ((monitoredEffect == StatusEffect.Weight) && (OptionsForm.config.monitoredWeight) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    if ((monitoredEffect == StatusEffect.Slow) && (OptionsForm.config.monitoredSlow) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    if ((monitoredEffect == StatusEffect.Curse) && (OptionsForm.config.monitoredCurse) && playerHelper.IsAbleToCastSpell("Cursna"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Cursna", SpellPrio.High);
                    }
                    if ((monitoredEffect == StatusEffect.Curse2) && (OptionsForm.config.monitoredCurse2) && playerHelper.IsAbleToCastSpell("Cursna"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Cursna", SpellPrio.High);
                    }
                    if ((monitoredEffect == StatusEffect.Addle) && (OptionsForm.config.monitoredAddle) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    if ((monitoredEffect == StatusEffect.Bane) && (OptionsForm.config.monitoredBane) && playerHelper.IsAbleToCastSpell("Cursna"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Cursna");
                    }
                    if ((monitoredEffect == StatusEffect.Plague) && (OptionsForm.config.monitoredPlague) && playerHelper.IsAbleToCastSpell("Viruna"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Viruna");
                    }
                    if ((monitoredEffect == StatusEffect.Disease) && (OptionsForm.config.monitoredDisease) && playerHelper.IsAbleToCastSpell("Viruna"))
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Viruna");
                    }
                    if ((monitoredEffect == StatusEffect.Burn) && (OptionsForm.config.monitoredBurn) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Frost) && (OptionsForm.config.monitoredFrost) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Choke) && (OptionsForm.config.monitoredChoke) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Rasp) && (OptionsForm.config.monitoredRasp) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Shock) && (OptionsForm.config.monitoredShock) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Drown) && (OptionsForm.config.monitoredDrown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Dia) && (OptionsForm.config.monitoredDia) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Bio) && (OptionsForm.config.monitoredBio) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.STR_Down) && (OptionsForm.config.monitoredStrDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.DEX_Down) && (OptionsForm.config.monitoredDexDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.VIT_Down) && (OptionsForm.config.monitoredVitDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.AGI_Down) && (OptionsForm.config.monitoredAgiDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.INT_Down) && (OptionsForm.config.monitoredIntDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.MND_Down) && (OptionsForm.config.monitoredMndDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.CHR_Down) && (OptionsForm.config.monitoredChrDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Max_HP_Down) && (OptionsForm.config.monitoredMaxHpDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Max_MP_Down) && (OptionsForm.config.monitoredMaxMpDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Accuracy_Down) && (OptionsForm.config.monitoredAccuracyDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Evasion_Down) && (OptionsForm.config.monitoredEvasionDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Defense_Down) && (OptionsForm.config.monitoredDefenseDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Flash) && (OptionsForm.config.monitoredFlash) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Magic_Acc_Down) && (OptionsForm.config.monitoredMagicAccDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Magic_Atk_Down) && (OptionsForm.config.monitoredMagicAtkDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Helix) && (OptionsForm.config.monitoredHelix) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Max_TP_Down) && (OptionsForm.config.monitoredMaxTpDown) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Requiem) && (OptionsForm.config.monitoredRequiem) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Elegy) && (OptionsForm.config.monitoredElegy) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                    else if ((monitoredEffect == StatusEffect.Threnody) && (OptionsForm.config.monitoredThrenody) && playerHelper.IsAbleToCastSpell("Erase") && plMonitoredSameParty() == true)
                    {
                        CastingManager.QueueSpell(SpellType.Debuff, _ELITEAPIMonitored.Player.Name, "Erase");
                    }
                }
            }
            // End MONITORED Debuff Removal
            #endregion

            if (OptionsForm.config.EnableAddOn)
            {
                List<EliteAPI.PartyMember> partyMembers = _ELITEAPIPL.Party.GetPartyMembers();

                foreach (BuffStorage ailment in ActiveBuffs.ToList())
                {
                    if (ailment != null)
                    {
                        if (ailment.CharacterName != null)
                        {
                            var charName = ailment.CharacterName;
                            if (ailment.CharacterBuffs != null)
                            {
                                List<string> named_Debuffs = ailment.CharacterBuffs.Split(',').ToList();

                                if (named_Debuffs != null && named_Debuffs.Count() != 0)
                                {
                                    named_Debuffs = named_Debuffs.Select(t => t.Trim()).ToList();

                                    #region "resetting_debuff_timers"

                                    // IF SLOW IS NOT ACTIVE, YET NEITHER IS HASTE / FLURRY DESPITE BEING ENABLED
                                    // RESET THE TIMER TO FORCE IT TO BE CAST
                                    // 13 slow
                                    // 33 haste
                                    // 580 haste - twice in buff table
                                    if (!DebuffContains(named_Debuffs, "13") && !DebuffContains(named_Debuffs, "33") && !DebuffContains(named_Debuffs, "580"))
                                    {
                                        InternalHelper.resetCooldown("Haste", charName);
                                        InternalHelper.resetCooldown("Haste II", charName);
                                    }
                                    //slow 13
                                    //flurry 265
                                    //flurry 581 - twice in buff table
                                    if (!DebuffContains(named_Debuffs, "13") && !DebuffContains(named_Debuffs, "265") && !DebuffContains(named_Debuffs, "581"))
                                    {
                                        InternalHelper.resetCooldown("Flurry", charName);
                                        InternalHelper.resetCooldown("Flurry II", charName);
                                    }
                                    // IF SUBLIMATION IS NOT ACTIVE, YET NEITHER IS REFRESH DESPITE BEING
                                    // ENABLED RESET THE TIMER TO FORCE IT TO BE CAST
                                    if (!DebuffContains(named_Debuffs, "187") && !DebuffContains(named_Debuffs, "188") && !DebuffContains(named_Debuffs, "43") && !DebuffContains(named_Debuffs, "541"))
                                    {
                                        InternalHelper.resetCooldown(SpellsHelper.GetRefreshSpell(), charName);
                                    }
                                    // IF REGEN IS NOT ACTIVE DESPITE BEING ENABLED RESET THE TIMER TO
                                    // FORCE IT TO BE CAST
                                    if (!DebuffContains(named_Debuffs, "42"))
                                    {
                                        InternalHelper.resetCooldown(SpellsHelper.GetRegenSpell(), charName);
                                    }
                                    // IF PROTECT IS NOT ACTIVE DESPITE BEING ENABLED RESET THE TIMER TO
                                    // FORCE IT TO BE CAST
                                    if (!DebuffContains(named_Debuffs, "40"))
                                    {
                                        InternalHelper.resetCooldown(SpellsHelper.GetProtectSpell(), charName);
                                    }

                                    // IF SHELL IS NOT ACTIVE DESPITE BEING ENABLED RESET THE TIMER TO
                                    // FORCE IT TO BE CAST
                                    if (!DebuffContains(named_Debuffs, "41"))
                                    {

                                        InternalHelper.resetCooldown(SpellsHelper.GetShellSpell(), charName);
                                    }
                                    // IF PHALANX II IS NOT ACTIVE DESPITE BEING ENABLED RESET THE TIMER
                                    // TO FORCE IT TO BE CAST
                                    if (!DebuffContains(named_Debuffs, "116"))
                                    {
                                        InternalHelper.resetCooldown("Phalanx II", charName);

                                    }
                                    // IF NO STORM SPELL IS ACTIVE DESPITE BEING ENABLED RESET THE TIMER
                                    // TO FORCE IT TO BE CAST
                                    if (!DebuffContains(named_Debuffs, "178") && !DebuffContains(named_Debuffs, "179") && !DebuffContains(named_Debuffs, "180") && !DebuffContains(named_Debuffs, "181") &&
                                        !DebuffContains(named_Debuffs, "182") && !DebuffContains(named_Debuffs, "183") && !DebuffContains(named_Debuffs, "184") && !DebuffContains(named_Debuffs, "185") &&
                                        !DebuffContains(named_Debuffs, "589") && !DebuffContains(named_Debuffs, "590") && !DebuffContains(named_Debuffs, "591") && !DebuffContains(named_Debuffs, "592") &&
                                        !DebuffContains(named_Debuffs, "593") && !DebuffContains(named_Debuffs, "594") && !DebuffContains(named_Debuffs, "595") && !DebuffContains(named_Debuffs, "596"))
                                    {
                                        InternalHelper.resetCooldown(SpellsHelper.GetStormVersion(SpellsHelper.GetEnabledStormSpell(charName)), charName);
                                    }
                                    #endregion

                                    // ==============================================================================================================================================================================
                                    // PARTY DEBUFF REMOVAL
                                    #region "party_debuff_removal"

                                    string character_name = ailment.CharacterName;

                                    if (!playerHelper.IsAlive(character_name)) { continue; }; //no love for the dead

                                    if (OptionsForm.config.enablePartyDebuffRemoval && !string.IsNullOrEmpty(character_name) && (characterNames_naRemoval.Contains(character_name) || OptionsForm.config.SpecifiednaSpellsenable == false))
                                    {
                                        //DOOM
                                        if (OptionsForm.config.naCurse && DebuffContains(named_Debuffs, "15") && playerHelper.IsAbleToCastSpell("Cursna"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Cursna", SpellPrio.Top);
                                        }
                                        //SLEEP
                                        if (DebuffContains(named_Debuffs, "2") && playerHelper.HasSpell(wakeSleepSpellName))
                                        {
                                            CastingManager.QueueSpell(SpellType.Healing, charName, wakeSleepSpellName, CurePrio.CureIV);
                                            //removeDebuff(charName, 2);
                                        }
                                        //SLEEP 2
                                        if (DebuffContains(named_Debuffs, "19") && playerHelper.HasSpell(wakeSleepSpellName))
                                        {
                                            CastingManager.QueueSpell(SpellType.Healing, charName, wakeSleepSpellName, CurePrio.CureIV);
                                            //removeDebuff(charName, 19);
                                        }
                                        //PETRIFICATION
                                        if (OptionsForm.config.naPetrification && DebuffContains(named_Debuffs, "7") && playerHelper.IsAbleToCastSpell("Stona"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Stona", SpellPrio.High);
                                            //removeDebuff(charName, 7);
                                        }
                                        //SILENCE
                                        if (OptionsForm.config.naSilence && DebuffContains(named_Debuffs, "6") && playerHelper.IsAbleToCastSpell("Silena"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Silena");
                                            //removeDebuff(charName, 6);
                                        }
                                        //PARALYSIS
                                        if (OptionsForm.config.naParalysis && DebuffContains(named_Debuffs, "4") && playerHelper.IsAbleToCastSpell("Paralyna"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Paralyna");
                                            //removeDebuff(charName, 4);
                                        }
                                        // PLAGUE
                                        if (OptionsForm.config.naDisease && DebuffContains(named_Debuffs, "31") && playerHelper.IsAbleToCastSpell("Viruna"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Viruna");
                                            //removeDebuff(charName, 31);
                                        }
                                        //DISEASE
                                        if (OptionsForm.config.naDisease && DebuffContains(named_Debuffs, "8") && playerHelper.IsAbleToCastSpell("Viruna"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Viruna");
                                            //removeDebuff(charName, 8);
                                        }
                                        // AMNESIA
                                        if (OptionsForm.config.Esuna && DebuffContains(named_Debuffs, "16") && playerHelper.IsAbleToCastSpell("Esuna") && BuffChecker(1, 418))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Esuna");
                                            //removeDebuff(charName, 16);
                                        }
                                        //CURSE
                                        if (OptionsForm.config.naCurse && DebuffContains(named_Debuffs, "9") && playerHelper.IsAbleToCastSpell("Cursna"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Cursna", SpellPrio.High);
                                            //removeDebuff(charName, 9);
                                        }
                                        //BLINDNESS
                                        if (OptionsForm.config.naBlindness && DebuffContains(named_Debuffs, "5") && playerHelper.IsAbleToCastSpell("Blindna"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Blindna");
                                            //removeDebuff(charName, 5);
                                        }
                                        //POISON
                                        if (OptionsForm.config.naPoison && DebuffContains(named_Debuffs, "3") && playerHelper.IsAbleToCastSpell("Poisona"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Poisona");
                                            //removeDebuff(charName, 3);
                                        }

                                                
                                        // SLOW
                                        if (OptionsForm.config.naErase == true && OptionsForm.config.na_Slow && DebuffContains(named_Debuffs, "13") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 13);
                                        }
                                        // BIO
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Bio && DebuffContains(named_Debuffs, "135") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 135);
                                        }
                                        // BIND
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Bind && DebuffContains(named_Debuffs, "11") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 11);
                                        }
                                        // GRAVITY
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Weight && DebuffContains(named_Debuffs, "12") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 12);
                                        }
                                        // ACCURACY DOWN
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_AccuracyDown && DebuffContains(named_Debuffs, "146") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 146);
                                        }
                                        // DEFENSE DOWN
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_DefenseDown && DebuffContains(named_Debuffs, "149") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 149);
                                        }
                                        // MAGIC DEF DOWN
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MagicDefenseDown && DebuffContains(named_Debuffs, "167") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 167);
                                        }
                                        // ATTACK DOWN
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_AttackDown && DebuffContains(named_Debuffs, "147") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 147);
                                        }
                                        // HP DOWN
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MaxHpDown && DebuffContains(named_Debuffs, "144") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 144);
                                        }
                                        // VIT Down
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_VitDown && DebuffContains(named_Debuffs, "138") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 138);
                                                    
                                        }
                                        // Threnody
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Threnody && DebuffContains(named_Debuffs, "217") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 217);
                                                    
                                        }
                                        // Shock
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Shock && DebuffContains(named_Debuffs, "132") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 132);
                                                    
                                        }
                                        // StrDown
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_StrDown && DebuffContains(named_Debuffs, "136") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 136);
                                                    
                                        }
                                        // Requiem
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Requiem && DebuffContains(named_Debuffs, "192") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 192);
                                                    
                                        }
                                        // Rasp
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Rasp && DebuffContains(named_Debuffs, "131") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 131);
                                                    
                                        }
                                        // Max TP Down
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MaxTpDown && DebuffContains(named_Debuffs, "189") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 189);
                                                    
                                        }
                                        // Max MP Down
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MaxMpDown && DebuffContains(named_Debuffs, "145") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 145);
                                                    
                                        }
                                        // Magic Attack Down
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MagicAttackDown && DebuffContains(named_Debuffs, "175") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 175);
                                                    
                                        }
                                        // Magic Acc Down
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MagicAccDown && DebuffContains(named_Debuffs, "174") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 174);
                                                    
                                        }
                                        // Mind Down
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_MndDown && DebuffContains(named_Debuffs, "141") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 141);
                                                    
                                        }
                                        // Int Down
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_IntDown && DebuffContains(named_Debuffs, "140") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 140);
                                                    
                                        }
                                        // Helix
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Helix && DebuffContains(named_Debuffs, "186") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 186);
                                                    
                                        }
                                        // Frost
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Frost && DebuffContains(named_Debuffs, "129") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 129);
                                                    
                                        }
                                        // EvasionDown
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_EvasionDown && DebuffContains(named_Debuffs, "148") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 148);
                                                    
                                        }
                                        // ELEGY
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Elegy && DebuffContains(named_Debuffs, "194") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 194);
                                                    
                                        }
                                        // Drown
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Drown && DebuffContains(named_Debuffs, "133") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 133);
                                                    
                                        }
                                        // Dia
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Dia && DebuffContains(named_Debuffs, "134") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 134);
                                                    
                                        }
                                        // DexDown
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_DexDown && DebuffContains(named_Debuffs, "137") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 137);
                                                    
                                        }
                                        // Choke
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Choke && DebuffContains(named_Debuffs, "130") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 130);
                                                    
                                        }
                                        // ChrDown
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_ChrDown && DebuffContains(named_Debuffs, "142") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 142);
                                                    
                                        }
                                        // Burn
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Burn && DebuffContains(named_Debuffs, "128") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 128);
                                                    
                                        }
                                        // Addle
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_Addle && DebuffContains(named_Debuffs, "21") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 21);
                                        }
                                        // AGI Down
                                        else if (OptionsForm.config.naErase == true && OptionsForm.config.na_AgiDown && DebuffContains(named_Debuffs, "139") && playerHelper.IsAbleToCastSpell("Erase"))
                                        {
                                            CastingManager.QueueSpell(SpellType.Debuff, charName, "Erase");
                                            //removeDebuff(charName, 139);
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                } // Closing FOREACH base_list
            }
        }

        private bool DebuffContains(List<string> Debuff_list, string Checked_id)
        {
            if (Debuff_list != null)
            {
                if (Debuff_list.Any(x => x == Checked_id))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private bool monitoredStatusCheck(StatusEffect requestedStatus)
        {
            bool statusFound = false;
            foreach (StatusEffect status in _ELITEAPIMonitored.Player.Buffs.Cast<StatusEffect>().Where(status => requestedStatus == status))
            {
                statusFound = true;
            }
            return statusFound;
        }

        public bool BuffChecker(int buffID, int checkedPlayer)
        {
            if (checkedPlayer == 1)
            {
                if (_ELITEAPIMonitored.Player.GetPlayerInfo().Buffs.Any(b => b == buffID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (_ELITEAPIPL.Player.GetPlayerInfo().Buffs.Any(b => b == buffID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        private bool ActiveSpikes()
        {
            if ((OptionsForm.config.plSpikes_Spell == 0) && playerHelper.plStatusCheck(StatusEffect.Blaze_Spikes))
            {
                return true;
            }
            else if ((OptionsForm.config.plSpikes_Spell == 1) && playerHelper.plStatusCheck(StatusEffect.Ice_Spikes))
            {
                return true;
            }
            else if ((OptionsForm.config.plSpikes_Spell == 2) && playerHelper.plStatusCheck(StatusEffect.Shock_Spikes))
            {
                return true;
            }
            return false;
        }


        private void GrabPlayerMonitoredData()
        {
            for (int x = 0; x < 2048; x++)
            {
                EliteAPI.XiEntity entity = _ELITEAPIPL.Entity.GetEntity(x);

                if (entity.Name != null && entity.Name == _ELITEAPIMonitored.Player.Name)
                {
                    Monitored_Index = entity.TargetID;
                }
                else if (entity.Name != null && entity.Name == _ELITEAPIPL.Player.Name)
                {
                    PL_Index = entity.TargetID;
                }
            }
        }

        private void castingTimer_TickAsync(object sender, EventArgs e)
        {
            if (CastingManager != null)
            {
                CastingManager.Run();
            }

        }

        private void cureTimer_TickAsync(object sender, EventArgs e)
        {
            if (CureManager != null)
            {
                CureManager.Run();
            }

        }

        private async void actionTimer_TickAsync(object sender, EventArgs e)
        {


            if (_ELITEAPIPL == null || _ELITEAPIMonitored == null)
            {
                return;
            }

            if (_ELITEAPIPL.Player.LoginStatus != (int)LoginStatus.LoggedIn || _ELITEAPIMonitored.Player.LoginStatus != (int)LoginStatus.LoggedIn)
            {
                return;
            }

            GrabPlayerMonitoredData();

            // Grab current time for calculations below

            currentTime = DateTime.Now;

            // Calculate time since Songs were cast on particular player
            playerSong1_Span[0] = currentTime.Subtract(playerSong1[0]);
            playerSong2_Span[0] = currentTime.Subtract(playerSong2[0]);
            playerSong3_Span[0] = currentTime.Subtract(playerSong3[0]);
            playerSong4_Span[0] = currentTime.Subtract(playerSong4[0]);

            Last_SongCast_Timer_Span[0] = currentTime.Subtract(Last_SongCast_Timer[0]);

            // Calculate time since Piannisimo Songs were cast on particular player
            pianissimo1_1_Span[0] = currentTime.Subtract(playerPianissimo1_1[0]);
            pianissimo2_1_Span[0] = currentTime.Subtract(playerPianissimo2_1[0]);
            pianissimo1_2_Span[0] = currentTime.Subtract(playerPianissimo1_2[0]);
            pianissimo2_2_Span[0] = currentTime.Subtract(playerPianissimo2_2[0]);

  

            int songs_currently_up1 = _ELITEAPIMonitored.Player.GetPlayerInfo().Buffs.Where(b => b == 197 || b == 198 || b == 195 || b == 199 || b == 200 || b == 215 || b == 196 || b == 214 || b == 216 || b == 218 || b == 222).Count();

            // IF ENABLED PAUSE ON KO
            if (OptionsForm.config.pauseOnKO && (_ELITEAPIPL.Player.Status == 2 || _ELITEAPIPL.Player.Status == 3))
            {
                pauseButton.Text = "Paused!";
                pauseButton.ForeColor = Color.Red;
                DisableTickers();
                ActiveBuffs.Clear();
                pauseActions = true;
                if (OptionsForm.config.FFXIDefaultAutoFollow == false)
                {
                    _ELITEAPIPL.AutoFollow.IsAutoFollowing = false;
                }
            }

            // IF YOU ARE DEAD BUT RERAISE IS AVAILABLE THEN ACCEPT RAISE
            if (OptionsForm.config.AcceptRaise == true && (_ELITEAPIPL.Player.Status == 2 || _ELITEAPIPL.Player.Status == 3))
            {
                if (_ELITEAPIPL.Menu.IsMenuOpen && _ELITEAPIPL.Menu.HelpName == "Revival" && _ELITEAPIPL.Menu.MenuIndex == 1 && ((OptionsForm.config.AcceptRaiseOnlyWhenNotInCombat == true && _ELITEAPIMonitored.Player.Status != 1) || OptionsForm.config.AcceptRaiseOnlyWhenNotInCombat == false))
                {
                    await Task.Delay(2000);
                    currentAction.Text = "Accepting Raise or Reraise.";
                    _ELITEAPIPL.ThirdParty.KeyPress(EliteMMO.API.Keys.NUMPADENTER);
                    await Task.Delay(5000);
                    currentAction.Text = string.Empty;
                }
            }

            // If CastingLock is not FALSE and you're not Terrorized, Petrified, or Stunned run the actions
            if (CastingManager.CanAct() && !playerHelper.plStatusCheck(StatusEffect.Terror) && !playerHelper.plStatusCheck(StatusEffect.Petrification) && !playerHelper.plStatusCheck(StatusEffect.Stun))
            {
                if (OptionsForm.config.DivineSeal && _ELITEAPIPL.Player.MPP <= 11 && JaManager.IsJaReady("Divine Seal") && !_ELITEAPIPL.Player.Buffs.Contains((short)StatusEffect.Weakness))
                {
                    JaManager.JobAbility_Wait("Divine Seal", "Divine Seal");
                    return;
                }
                else if (OptionsForm.config.Convert && (_ELITEAPIPL.Player.MP <= OptionsForm.config.convertMP) && JaManager.IsJaReady("Convert") && !_ELITEAPIPL.Player.Buffs.Contains((short)StatusEffect.Weakness))
                {
                    JaManager.JobAbility_Wait("Convert");
                    return;
                }
                else if (OptionsForm.config.RadialArcana && (_ELITEAPIPL.Player.MP <= OptionsForm.config.RadialArcanaMP) && JaManager.IsJaReady("Radial Arcana") && !_ELITEAPIPL.Player.Buffs.Contains((short)StatusEffect.Weakness))
                {
                    // Check if a pet is already active
                    if (_ELITEAPIPL.Player.Pet.HealthPercent >= 1 && _ELITEAPIPL.Player.Pet.Distance <= 9)
                    {
                        JaManager.JobAbility_Wait("Radial Arcana", "Radial Arcana");
                        return;
                    }
                    else if (_ELITEAPIPL.Player.Pet.HealthPercent >= 1 && _ELITEAPIPL.Player.Pet.Distance >= 9 && JaManager.IsJaReady("Full Circle"))
                    {
                        JaManager.JobAbility_Wait("Full Circle");
                        string SpellCheckedResult = ReturnGeoSpell(OptionsForm.config.RadialArcana_Spell, 2);
                        CastingManager.QueueSpell(SpellType.GEO, "<me>", SpellCheckedResult);
                    }
                    else
                    {
                        string SpellCheckedResult = ReturnGeoSpell(OptionsForm.config.RadialArcana_Spell, 2);
                        CastingManager.QueueSpell(SpellType.GEO, "<me>", SpellCheckedResult);
                    }
                }
                else if (OptionsForm.config.FullCircle)
                {
                    // When out of range Distance is 59 Yalms regardless, Must be within 15 yalms to gain
                    // the effect

                    #region "full_circle"
                    //Check if "pet" is active and out of range of the monitored player
                    if (_ELITEAPIPL.Player.Pet.HealthPercent >= 1)
                    {
                        if (OptionsForm.config.Fullcircle_GEOTarget == true && OptionsForm.config.LuopanSpell_Target != "")
                        {

                            ushort PetsIndex = _ELITEAPIPL.Player.PetIndex;

                            EliteAPI.XiEntity PetsEntity = _ELITEAPIPL.Entity.GetEntity(PetsIndex);

                            int FullCircle_CharID = 0;

                            for (int x = 0; x < 2048; x++)
                            {
                                EliteAPI.XiEntity entity = _ELITEAPIPL.Entity.GetEntity(x);

                                if (entity.Name != null && entity.Name.ToLower().Equals(OptionsForm.config.LuopanSpell_Target.ToLower()))
                                {
                                    FullCircle_CharID = Convert.ToInt32(entity.TargetID);
                                    break;
                                }
                            }

                            if (FullCircle_CharID != 0)
                            {
                                EliteAPI.XiEntity FullCircleEntity = _ELITEAPIPL.Entity.GetEntity(FullCircle_CharID);

                                float fX = PetsEntity.X - FullCircleEntity.X;
                                float fY = PetsEntity.Y - FullCircleEntity.Y;
                                float fZ = PetsEntity.Z - FullCircleEntity.Z;

                                float generatedDistance = (float)Math.Sqrt((fX * fX) + (fY * fY) + (fZ * fZ));

                                if (generatedDistance >= 10)
                                {
                                    FullCircle_Timer.Enabled = true;
                                }
                            }

                        }
                        else if (OptionsForm.config.Fullcircle_GEOTarget == false && _ELITEAPIMonitored.Player.Status == 1)
                        {
                            ushort PetsIndex = _ELITEAPIPL.Player.PetIndex;

                            EliteAPI.XiEntity PetsEntity = _ELITEAPIMonitored.Entity.GetEntity(PetsIndex);

                            if (PetsEntity.Distance >= 10)
                            {
                                FullCircle_Timer.Enabled = true;
                            }
                        }
                    }
                    #endregion
                }
                else if ((OptionsForm.config.Troubadour) && JaManager.IsJaReady("Troubadour") && (playerHelper.HasAbility("Troubadour")) && songs_currently_up1 == 0)
                {
                    JaManager.JobAbility_Wait("Troubadour", "Troubadour");
                    return;
                }
                else if ((OptionsForm.config.Nightingale) && JaManager.IsJaReady("Nightingale") && (playerHelper.HasAbility("Nightingale")) && songs_currently_up1 == 0)
                {
                    JaManager.JobAbility_Wait("Nightingale", "Nightingale");
                    return;
                }

                if (_ELITEAPIPL.Player.MP <= (int)OptionsForm.config.mpMinCastValue && _ELITEAPIPL.Player.MP != 0)
                {
                    if (OptionsForm.config.lowMPcheckBox && !islowmp && !OptionsForm.config.healLowMP)
                    {
                        _ELITEAPIPL.ThirdParty.SendString("/tell " + _ELITEAPIMonitored.Player.Name + " MP is low!");
                        islowmp = true;
                        return;
                    }
                    islowmp = true;
                    return;
                }
                if (_ELITEAPIPL.Player.MP > (int)OptionsForm.config.mpMinCastValue && _ELITEAPIPL.Player.MP != 0)
                {
                    if (OptionsForm.config.lowMPcheckBox && islowmp && !OptionsForm.config.healLowMP)
                    {
                        _ELITEAPIPL.ThirdParty.SendString("/tell " + _ELITEAPIMonitored.Player.Name + " MP OK!");
                        islowmp = false;
                    }
                }

                if (OptionsForm.config.healLowMP == true && _ELITEAPIPL.Player.MP <= OptionsForm.config.healWhenMPBelow && _ELITEAPIPL.Player.Status == (uint)Status.Standing && _ELITEAPIPL.Player.Status != (uint)Status.Healing)
                {
                    if (OptionsForm.config.lowMPcheckBox && !islowmp)
                    {
                        _ELITEAPIPL.ThirdParty.SendString("/tell " + _ELITEAPIMonitored.Player.Name + " MP is seriously low, /healing.");
                        islowmp = true;
                    }
                    _ELITEAPIPL.ThirdParty.SendString("/heal");
                }
                else if (OptionsForm.config.standAtMP == true && _ELITEAPIPL.Player.MPP >= OptionsForm.config.standAtMP_Percentage && _ELITEAPIPL.Player.Status == (uint)Status.Healing)
                {
                    if (OptionsForm.config.lowMPcheckBox && !islowmp)
                    {
                        _ELITEAPIPL.ThirdParty.SendString("/tell " + _ELITEAPIMonitored.Player.Name + " MP has recovered.");
                        islowmp = false;
                    }
                    _ELITEAPIPL.ThirdParty.SendString("/heal");
                }

                if (_ELITEAPIPL.Player.Status == (uint)Status.Healing) { 
                    return; 
                } //Healing do nothing 

                // Only perform actions if PL is stationary PAUSE GOES HERE
                if (_ELITEAPIPL.Player.LoginStatus == (int)LoginStatus.LoggedIn && curePlease_autofollow == false && (_ELITEAPIPL.Player.Status == (uint)Status.Standing || _ELITEAPIPL.Player.Status == (uint)Status.Fighting))
                {

                    // RUN DEBUFF REMOVAL - CONVERTED TO FUNCTION SO CAN BE RUN IN MULTIPLE AREAS
                    RunDebuffChecker();

                    // PL Auto Buffs



                    string BarspellName = string.Empty;
                    int BarspellBuffID = 0;
                    bool BarSpell_AOE = false;

                    if (OptionsForm.config.AOE_Barelemental == false)
                    {
                        SpellsData barspell = BarSpells.barspells.Where(c => c.spell_position == OptionsForm.config.plBarElement_Spell && c.type == 1 && c.aoe_version != true).SingleOrDefault();

                        BarspellName = barspell.Spell_Name;
                        BarspellBuffID = barspell.buffID;
                        BarSpell_AOE = false;
                    }
                    else
                    {
                        SpellsData barspell = BarSpells.barspells.Where(c => c.spell_position == OptionsForm.config.plBarElement_Spell && c.type == 1 && c.aoe_version == true).SingleOrDefault();

                        BarspellName = barspell.Spell_Name;
                        BarspellBuffID = barspell.buffID;
                        BarSpell_AOE = true;
                    }

                    string BarstatusName = string.Empty;
                    int BarstatusBuffID = 0;
                    bool BarStatus_AOE = false;

                    if (OptionsForm.config.AOE_Barstatus == false)
                    {
                        SpellsData barstatus = BarSpells.barspells.Where(c => c.spell_position == OptionsForm.config.plBarStatus_Spell && c.type == 2 && c.aoe_version != true).SingleOrDefault();

                        BarstatusName = barstatus.Spell_Name;
                        BarstatusBuffID = barstatus.buffID;
                        BarStatus_AOE = false;
                    }
                    else
                    {
                        SpellsData barstatus = BarSpells.barspells.Where(c => c.spell_position == OptionsForm.config.plBarStatus_Spell && c.type == 2 && c.aoe_version == true).SingleOrDefault();

                        BarstatusName = barstatus.Spell_Name;
                        BarstatusBuffID = barstatus.buffID;
                        BarStatus_AOE = true;
                    }

                    SpellsData enspell = EnSpells.enspells.Where(c => c.spell_position == OptionsForm.config.plEnspell_Spell && c.type == 1).SingleOrDefault();
                    SpellsData stormspell = StormSpells.stormspells.Where(c => c.spell_position == OptionsForm.config.plStormSpell_Spell).SingleOrDefault();

                    if (_ELITEAPIPL.Player.LoginStatus == (int)LoginStatus.LoggedIn && CastingManager.CanAct())
                    {
                        #region "self_buffs"
                        if ((OptionsForm.config.Composure) && (!playerHelper.plStatusCheck(StatusEffect.Composure)) && JaManager.IsJaReady("Composure") && (playerHelper.HasAbility("Composure")))
                        {

                            JaManager.JobAbility_Wait("Composure", "Composure");
                        }
                        else if ((OptionsForm.config.LightArts) && (!playerHelper.plStatusCheck(StatusEffect.Light_Arts)) && (!playerHelper.plStatusCheck(StatusEffect.Addendum_White)) && JaManager.IsJaReady("Light Arts") && (playerHelper.HasAbility("Light Arts")))
                        {
                            JaManager.JobAbility_Wait("Light Arts", "Light Arts");
                        }
                        else if ((OptionsForm.config.AddendumWhite) && (!playerHelper.plStatusCheck(StatusEffect.Addendum_White)) && (playerHelper.plStatusCheck(StatusEffect.Light_Arts)) && JaManager.IsJaReady("Stratagems") && (playerHelper.HasAbility("Stratagems")))
                        {
                            JaManager.JobAbility_Wait("Addendum: White", "Addendum: White");
                        }
                        else if ((OptionsForm.config.DarkArts) && (!playerHelper.plStatusCheck(StatusEffect.Dark_Arts)) && (!playerHelper.plStatusCheck(StatusEffect.Addendum_Black)) && JaManager.IsJaReady("Dark Arts") && playerHelper.HasAbility("Dark Arts"))
                        {
                            JaManager.JobAbility_Wait("Dark Arts", "Dark Arts");
                        }
                        else if ((OptionsForm.config.AddendumBlack) && (playerHelper.plStatusCheck(StatusEffect.Dark_Arts)) && (!playerHelper.plStatusCheck(StatusEffect.Addendum_Black)) && JaManager.IsJaReady("Stratagems") && (playerHelper.HasAbility("Stratagems")))
                        {
                            JaManager.JobAbility_Wait("Addendum: Black", "Addendum: Black");
                        }
                        else if ((OptionsForm.config.plReraise) && (OptionsForm.config.EnlightenmentReraise) && (!playerHelper.plStatusCheck(StatusEffect.Reraise)) && _ELITEAPIPL.Player.MainJob == 20 && !BuffChecker(401, 0) && playerHelper.HasAbility("Enlightenment"))
                        {


                            if (!playerHelper.plStatusCheck(StatusEffect.Enlightenment) && JaManager.IsJaReady("Enlightenment"))
                            {
                                JaManager.JobAbility_Wait("Reraise, Enlightenment", "Enlightenment");
                            }


                            if ((OptionsForm.config.plReraise_Level == 1) && _ELITEAPIPL.Player.HasSpell(_ELITEAPIPL.Resources.GetSpell("Reraise", 0).Index) && _ELITEAPIPL.Player.MP > 150)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Reraise", SpellPrio.Top);
                            }
                            else if ((OptionsForm.config.plReraise_Level == 2) && _ELITEAPIPL.Player.HasSpell(_ELITEAPIPL.Resources.GetSpell("Reraise II", 0).Index) && _ELITEAPIPL.Player.MP > 150)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Reraise II", SpellPrio.Top);
                            }
                            else if ((OptionsForm.config.plReraise_Level == 3) && _ELITEAPIPL.Player.HasSpell(_ELITEAPIPL.Resources.GetSpell("Reraise III", 0).Index) && _ELITEAPIPL.Player.MP > 150)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Reraise III", SpellPrio.Top);
                            }
                            else if ((OptionsForm.config.plReraise_Level == 4) && _ELITEAPIPL.Player.HasSpell(_ELITEAPIPL.Resources.GetSpell("Reraise III", 0).Index) && _ELITEAPIPL.Player.MP > 150)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Reraise III", SpellPrio.Top);
                            }

                        }
                        if ((OptionsForm.config.plReraise) && (!playerHelper.plStatusCheck(StatusEffect.Reraise)) && CheckReraiseLevelPossession() == true)
                        {
                            if ((OptionsForm.config.plReraise_Level == 1) && _ELITEAPIPL.Player.MP > 150)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Reraise", SpellPrio.Top);
                            }
                            else if ((OptionsForm.config.plReraise_Level == 2) && _ELITEAPIPL.Player.MP > 150)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Reraise II", SpellPrio.Top);
                            }
                            else if ((OptionsForm.config.plReraise_Level == 3) && _ELITEAPIPL.Player.MP > 150)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Reraise III", SpellPrio.Top);
                            }
                            else if ((OptionsForm.config.plReraise_Level == 4) && _ELITEAPIPL.Player.MP > 150)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Reraise IV", SpellPrio.Top);
                            }
                        }
                        if ((OptionsForm.config.plUtsusemi) && (BuffChecker(444, 0) != true && BuffChecker(445, 0) != true && BuffChecker(446, 0) != true))
                        {
                            if (playerHelper.IsAbleToCastSpell("Utsusemi: Ni") && ItemHelper.HasItem(_ELITEAPIPL, "Shihei"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Utsusemi: Ni", SpellPrio.Top);
                            }
                            else if (playerHelper.IsAbleToCastSpell("Utsusemi: Ichi") && (BuffChecker(62, 0) != true && BuffChecker(444, 0) != true && BuffChecker(445, 0) != true && BuffChecker(446, 0) != true) && ItemHelper.HasItem(_ELITEAPIPL, "Shihei"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Utsusemi: Ichi", SpellPrio.Top);
                            }
                        }
                        if ((OptionsForm.config.plProtect) && (!playerHelper.plStatusCheck(StatusEffect.Protect)))
                        {
                            string protectSpell = string.Empty;
                            if (OptionsForm.config.autoProtect_Spell == 0)
                            {
                                protectSpell = "Protect";
                            }
                            else if (OptionsForm.config.autoProtect_Spell == 1)
                            {
                                protectSpell = "Protect II";
                            }
                            else if (OptionsForm.config.autoProtect_Spell == 2)
                            {
                                protectSpell = "Protect III";
                            }
                            else if (OptionsForm.config.autoProtect_Spell == 3)
                            {
                                protectSpell = "Protect IV";
                            }
                            else if (OptionsForm.config.autoProtect_Spell == 4)
                            {
                                protectSpell = "Protect V";
                            }

                            if (protectSpell != string.Empty && playerHelper.IsAbleToCastSpell(protectSpell))
                            {
                                if (OptionsForm.config.Accession && OptionsForm.config.accessionProShell && _ELITEAPIPL.Party.GetPartyMembers().Count() > 2 && ((_ELITEAPIPL.Player.MainJob == 5 && _ELITEAPIPL.Player.SubJob == 20) || _ELITEAPIPL.Player.MainJob == 20) && playerHelper.HasAbility("Accession"))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, "<me>", protectSpell, SpellPrio.Higher, new List<JobAbility>() { new JobAbility("Accession", "<me>", StatusEffect.Accession) });
                                }
                                else
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, "<me>", protectSpell, SpellPrio.Higher);
                                }
                            }
                        }
                        if ((OptionsForm.config.plShell) && (!playerHelper.plStatusCheck(StatusEffect.Shell)))
                        {
                            string shellSpell = string.Empty;
                            if (OptionsForm.config.autoShell_Spell == 0)
                            {
                                shellSpell = "Shell";
                            }
                            else if (OptionsForm.config.autoShell_Spell == 1)
                            {
                                shellSpell = "Shell II";
                            }
                            else if (OptionsForm.config.autoShell_Spell == 2)
                            {
                                shellSpell = "Shell III";
                            }
                            else if (OptionsForm.config.autoShell_Spell == 3)
                            {
                                shellSpell = "Shell IV";
                            }
                            else if (OptionsForm.config.autoShell_Spell == 4)
                            {
                                shellSpell = "Shell V";
                            }

                            if (shellSpell != string.Empty && playerHelper.IsAbleToCastSpell(shellSpell))
                            {
                                if (OptionsForm.config.Accession && OptionsForm.config.accessionProShell && _ELITEAPIPL.Party.GetPartyMembers().Count() > 2 && ((_ELITEAPIPL.Player.MainJob == 5 && _ELITEAPIPL.Player.SubJob == 20) || _ELITEAPIPL.Player.MainJob == 20) && (playerHelper.HasAbility("Accession")))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, "<me>", shellSpell, SpellPrio.Higher, new List<JobAbility>() { new JobAbility("Accession", "<me>", StatusEffect.Accession) });
                                }
                                else
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, "<me>", shellSpell, SpellPrio.Higher);
                                }
                            }
                        }
                        var jaList = new List<JobAbility>();
                        if ((OptionsForm.config.plBlink) && (!playerHelper.plStatusCheck(StatusEffect.Blink)) && playerHelper.IsAbleToCastSpell("Blink"))
                        {
                            jaList = new List<JobAbility>();
                            if (OptionsForm.config.Accession && OptionsForm.config.blinkAccession && playerHelper.HasAbility("Accession"))
                            {
                                jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                            }

                            if (OptionsForm.config.Perpetuance && OptionsForm.config.blinkPerpetuance && playerHelper.HasAbility("Perpetuance"))
                            {
                                jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                            }

                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Blink", SpellPrio.High, jaList);
                        }
                        jaList = new List<JobAbility>();
                        if ((OptionsForm.config.plPhalanx) && (!playerHelper.plStatusCheck(StatusEffect.Phalanx)) && playerHelper.IsAbleToCastSpell("Phalanx"))
                        {
                            if (OptionsForm.config.Accession && OptionsForm.config.phalanxAccession && playerHelper.HasAbility("Accession"))
                            {
                                jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                            }

                            if (OptionsForm.config.Perpetuance && OptionsForm.config.phalanxPerpetuance && playerHelper.HasAbility("Perpetuance"))
                            {
                                jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                            }

                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Phalanx", SpellPrio.High, jaList);
                        }
                        jaList = new List<JobAbility>();
                        if ((OptionsForm.config.plRefresh) && (!playerHelper.plStatusCheck(StatusEffect.Refresh)) && CheckRefreshLevelPossession())
                        {
                            if ((OptionsForm.config.plRefresh_Level == 1) && playerHelper.IsAbleToCastSpell("Refresh"))
                            {
                                if (OptionsForm.config.Accession && OptionsForm.config.refreshAccession && playerHelper.HasAbility("Accession"))
                                {
                                    jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                                }

                                if (OptionsForm.config.Perpetuance && OptionsForm.config.refreshPerpetuance && playerHelper.HasAbility("Perpetuance"))
                                {
                                    jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                                }

                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Refresh", SpellPrio.High, jaList);
                            }
                            else if ((OptionsForm.config.plRefresh_Level == 2) && playerHelper.IsAbleToCastSpell("Refresh II"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Refresh II", SpellPrio.High, jaList);
                            }
                            else if ((OptionsForm.config.plRefresh_Level == 3) && playerHelper.IsAbleToCastSpell("Refresh III"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Refresh III", SpellPrio.High, jaList);
                            }
                        }
                        jaList = new List<JobAbility>();
                        if ((OptionsForm.config.plRegen) && (!playerHelper.plStatusCheck(StatusEffect.Regen)) && CheckRegenLevelPossession() == true)
                        {
                            if (OptionsForm.config.Accession && OptionsForm.config.regenAccession && playerHelper.HasAbility("Accession"))
                            {
                                jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                            }

                            if (OptionsForm.config.Perpetuance && OptionsForm.config.regenPerpetuance && playerHelper.HasAbility("Perpetuance"))
                            {
                                jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                            }

                            if ((OptionsForm.config.plRegen_Level == 1) && _ELITEAPIPL.Player.MP > 15)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Regen", SpellPrio.High, jaList);
                            }
                            else if ((OptionsForm.config.plRegen_Level == 2) && _ELITEAPIPL.Player.MP > 36)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Regen II", SpellPrio.High, jaList);
                            }
                            else if ((OptionsForm.config.plRegen_Level == 3) && _ELITEAPIPL.Player.MP > 64)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Regen III", SpellPrio.High, jaList);
                            }
                            else if ((OptionsForm.config.plRegen_Level == 4) && _ELITEAPIPL.Player.MP > 82)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Regen IV", SpellPrio.High, jaList);
                            }
                            else if ((OptionsForm.config.plRegen_Level == 5) && _ELITEAPIPL.Player.MP > 100)
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Regen V", SpellPrio.High, jaList);
                            }
                        }
                        jaList = new List<JobAbility>();
                        if ((OptionsForm.config.plAdloquium) && (!playerHelper.plStatusCheck(StatusEffect.Regain)) && playerHelper.IsAbleToCastSpell("Adloquium"))
                        {
                            if (OptionsForm.config.Accession && OptionsForm.config.adloquiumAccession && playerHelper.HasAbility("Accession"))
                            {
                                jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                            }

                            if (OptionsForm.config.Perpetuance && OptionsForm.config.adloquiumPerpetuance && playerHelper.HasAbility("Perpetuance"))
                            {
                                jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                            }

                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Adloquium", SpellPrio.High, jaList);
                        }
                        jaList = new List<JobAbility>();
                        if ((OptionsForm.config.plStoneskin) && (!playerHelper.plStatusCheck(StatusEffect.Stoneskin)) && playerHelper.IsAbleToCastSpell("Stoneskin"))
                        {
                            if (OptionsForm.config.Accession && OptionsForm.config.stoneskinAccession && playerHelper.HasAbility("Accession"))
                            {
                                jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                            }

                            if (OptionsForm.config.Perpetuance && OptionsForm.config.stoneskinPerpetuance && playerHelper.HasAbility("Perpetuance"))
                            {
                                jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                            }

                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Stoneskin", SpellPrio.High, jaList);
                        }
                        jaList = new List<JobAbility>();
                        if ((OptionsForm.config.plAquaveil) && (!playerHelper.plStatusCheck(StatusEffect.Aquaveil)) && playerHelper.IsAbleToCastSpell("Aquaveil"))
                        {
                            if (OptionsForm.config.Accession && OptionsForm.config.aquaveilAccession && playerHelper.HasAbility("Accession"))
                            {
                                jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                            }

                            if (OptionsForm.config.Perpetuance && OptionsForm.config.aquaveilPerpetuance && playerHelper.HasAbility("Perpetuance") && playerHelper.plStatusCheck(StatusEffect.Perpetuance))
                            {
                                jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                            }

                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Aquaveil", SpellPrio.High, jaList);
                        }
                        if ((OptionsForm.config.plShellra) && (!playerHelper.plStatusCheck(StatusEffect.Shell)) && CheckShellraLevelPossession() == true)
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", GetShellraLevel(OptionsForm.config.plShellra_Level), SpellPrio.High);
                        }
                        if ((OptionsForm.config.plProtectra) && (!playerHelper.plStatusCheck(StatusEffect.Protect)) && CheckProtectraLevelPossession() == true)
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", GetProtectraLevel(OptionsForm.config.plProtectra_Level), SpellPrio.High);
                        }
                        jaList = new List<JobAbility>();
                        if ((OptionsForm.config.plBarElement) && !BuffChecker(BarspellBuffID, 0) && playerHelper.IsAbleToCastSpell(BarspellName))
                        {
                            if (OptionsForm.config.Accession && OptionsForm.config.barspellAccession && playerHelper.HasAbility("Accession") && BarSpell_AOE == false)
                            {
                                jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                            }

                            if (OptionsForm.config.Perpetuance && OptionsForm.config.barspellPerpetuance && playerHelper.HasAbility("Perpetuance"))
                            {
                                jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                            }

                            CastingManager.QueueSpell(SpellType.Buff, "<me>", BarspellName, SpellPrio.High, jaList);
                        }
                        jaList = new List<JobAbility>();
                        if ((OptionsForm.config.plBarStatus) && !BuffChecker(BarstatusBuffID, 0) && playerHelper.IsAbleToCastSpell(BarstatusName))
                        {
                            if (OptionsForm.config.Accession && OptionsForm.config.barstatusAccession && playerHelper.HasAbility("Accession") && BarStatus_AOE == false)
                            {
                                jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                            }

                            if (OptionsForm.config.Perpetuance && OptionsForm.config.barstatusPerpetuance && playerHelper.HasAbility("Perpetuance"))
                            {
                                jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                            }

                            CastingManager.QueueSpell(SpellType.Buff, "<me>", BarstatusName, SpellPrio.High, jaList);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 0) && !playerHelper.plStatusCheck(StatusEffect.STR_Boost2) && playerHelper.IsAbleToCastSpell("Gain-STR"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Gain-STR", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 1) && !playerHelper.plStatusCheck(StatusEffect.DEX_Boost2) && playerHelper.IsAbleToCastSpell("Gain-DEX"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Gain-DEX", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 2) && !playerHelper.plStatusCheck(StatusEffect.VIT_Boost2) && playerHelper.IsAbleToCastSpell("Gain-VIT"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Gain-VIT", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 3) && !playerHelper.plStatusCheck(StatusEffect.AGI_Boost2) && playerHelper.IsAbleToCastSpell("Gain-AGI"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Gain-AGI", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 4) && !playerHelper.plStatusCheck(StatusEffect.INT_Boost2) && playerHelper.IsAbleToCastSpell("Gain-INT"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Gain-INT", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 5) && !playerHelper.plStatusCheck(StatusEffect.MND_Boost2) && playerHelper.IsAbleToCastSpell("Gain-MND"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Gain-MND", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 6) && !playerHelper.plStatusCheck(StatusEffect.CHR_Boost2) && playerHelper.IsAbleToCastSpell("Gain-CHR"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Gain-CHR", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 7) && !playerHelper.plStatusCheck(StatusEffect.STR_Boost2) && playerHelper.IsAbleToCastSpell("Boost-STR"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Boost-STR", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 8) && !playerHelper.plStatusCheck(StatusEffect.DEX_Boost2) && playerHelper.IsAbleToCastSpell("Boost-DEX"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Boost-DEX", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 9) && !playerHelper.plStatusCheck(StatusEffect.VIT_Boost2) && playerHelper.IsAbleToCastSpell("Boost-VIT"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Boost-VIT", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 10) && !playerHelper.plStatusCheck(StatusEffect.AGI_Boost2) && playerHelper.IsAbleToCastSpell("Boost-AGI"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Boost-AGI", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 11) && !playerHelper.plStatusCheck(StatusEffect.INT_Boost2) && playerHelper.IsAbleToCastSpell("Boost-INT"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Boost-INT", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 12) && !playerHelper.plStatusCheck(StatusEffect.MND_Boost2) && playerHelper.IsAbleToCastSpell("Boost-MND"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Boost-MND", SpellPrio.High);
                        }
                        if (OptionsForm.config.plGainBoost && (OptionsForm.config.plGainBoost_Spell == 13) && !playerHelper.plStatusCheck(StatusEffect.CHR_Boost2) && playerHelper.IsAbleToCastSpell("Boost-CHR"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Boost-CHR", SpellPrio.High);
                        }
                        jaList = new List<JobAbility>();
                        if (OptionsForm.config.plStormSpell && !BuffChecker(stormspell.buffID, 0) && playerHelper.IsAbleToCastSpell(stormspell.Spell_Name))
                        {
                            if (OptionsForm.config.Accession && OptionsForm.config.stormspellAccession && playerHelper.HasAbility("Accession"))
                            {
                                jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                            }

                            if (OptionsForm.config.Perpetuance && OptionsForm.config.stormspellPerpetuance && playerHelper.HasAbility("Perpetuance"))
                            {
                                jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                            }

                            CastingManager.QueueSpell(SpellType.Buff, "<me>", stormspell.Spell_Name, SpellPrio.High, jaList);
                        }
                        if ((OptionsForm.config.plKlimaform) && !playerHelper.plStatusCheck(StatusEffect.Klimaform))
                        {
                            if (playerHelper.IsAbleToCastSpell("Klimaform"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Klimaform", SpellPrio.Top);
                            }
                        }
                        if ((OptionsForm.config.plTemper) && (!playerHelper.plStatusCheck(StatusEffect.Multi_Strikes)))
                        {
                            if ((OptionsForm.config.plTemper_Level == 1) && playerHelper.IsAbleToCastSpell("Temper"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Temper", SpellPrio.Top);
                            }
                            else if ((OptionsForm.config.plTemper_Level == 2) && playerHelper.IsAbleToCastSpell("Temper II"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Temper II", SpellPrio.Top);
                            }
                        }
                        if ((OptionsForm.config.plHaste) && (!playerHelper.plStatusCheck(StatusEffect.Haste)))
                        {
                            if ((OptionsForm.config.plHaste_Level == 1) && playerHelper.IsAbleToCastSpell("Haste"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Haste", SpellPrio.Top);
                            }
                            else if ((OptionsForm.config.plHaste_Level == 2) && playerHelper.IsAbleToCastSpell("Haste II"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Haste II", SpellPrio.Top);
                            }
                        }
                        if ((OptionsForm.config.plSpikes) && ActiveSpikes() == false)
                        {
                            if ((OptionsForm.config.plSpikes_Spell == 0) && playerHelper.IsAbleToCastSpell("Blaze Spikes"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Blaze Spikes", SpellPrio.Top);
                            }
                            else if ((OptionsForm.config.plSpikes_Spell == 1) && playerHelper.IsAbleToCastSpell("Ice Spikes"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Ice Spikes", SpellPrio.Top);
                            }
                            else if ((OptionsForm.config.plSpikes_Spell == 2) && playerHelper.IsAbleToCastSpell("Shock Spikes"))
                            {
                                CastingManager.QueueSpell(SpellType.Buff, "<me>", "Shock Spikes", SpellPrio.Top);
                            }
                        }
                        jaList = new List<JobAbility>();
                        if ((OptionsForm.config.plEnspell) && !BuffChecker(enspell.buffID, 0) && playerHelper.IsAbleToCastSpell(enspell.Spell_Name))
                        {
                            if (OptionsForm.config.Accession && OptionsForm.config.enspellAccession && playerHelper.HasAbility("Accession") && enspell.spell_position < 6)
                            {
                                jaList.Add(new JobAbility("Accession", "<me>", StatusEffect.Accession));
                            }

                            if (OptionsForm.config.Perpetuance && OptionsForm.config.enspellPerpetuance && playerHelper.HasAbility("Perpetuance") && enspell.spell_position < 6)
                            {
                                jaList.Add(new JobAbility("Perpetuance", "<me>", StatusEffect.Perpetuance));
                            }

                            CastingManager.QueueSpell(SpellType.Buff, "<me>", enspell.Spell_Name, SpellPrio.Top, jaList);
                        }
                        if ((OptionsForm.config.plAuspice) && (!playerHelper.plStatusCheck(StatusEffect.Auspice)) && playerHelper.IsAbleToCastSpell("Auspice"))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", "Auspice", SpellPrio.Top);
                        }

                        // ENTRUSTED INDI SPELL CASTING, WILL BE CAST SO LONG AS ENTRUST IS ACTIVE
                        if (OptionsForm.config.Entrust && OptionsForm.config.EnableGeoSpells && JaManager.IsJaReady("Entrust") && playerHelper.HasAbility("Entrust"))
                        {
                            string SpellCheckedResult = ReturnGeoSpell(OptionsForm.config.EntrustedSpell_Spell, 1);
                            if (SpellCheckedResult == "SpellError_Cancel")
                            {
                                OptionsForm.config.EnableGeoSpells = false;
                                MessageBox.Show("An error has occurred with Entrusted INDI spell casting, please report what spell was active at the time.");
                            }
                            else if (SpellCheckedResult == "SpellRecast" || SpellCheckedResult == "SpellUnknown")
                            {
                            }
                            else
                            {
                                if (OptionsForm.config.EntrustedSpell_Target == string.Empty)
                                {
                                    CastingManager.QueueSpell(SpellType.GEO, _ELITEAPIMonitored.Player.Name, SpellCheckedResult, SpellPrio.Top, new List<JobAbility>() { new JobAbility("Entrust", "<me>", StatusEffect.Unknown) });
                                }
                                else
                                {
                                    CastingManager.QueueSpell(SpellType.GEO, OptionsForm.config.EntrustedSpell_Target, SpellCheckedResult, SpellPrio.Top, new List<JobAbility>() { new JobAbility("Entrust", "<me>", StatusEffect.Unknown) });
                                }
                            }
                        }
                        
                        // CAST NON ENTRUSTED INDI SPELL
                        if (OptionsForm.config.EnableGeoSpells && !BuffChecker(612, 0) && (CheckEngagedStatus() == true || !OptionsForm.config.IndiWhenEngaged))
                        {
                            string SpellCheckedResult = ReturnGeoSpell(OptionsForm.config.IndiSpell_Spell, 1);

                            if (SpellCheckedResult == "SpellError_Cancel")
                            {
                                OptionsForm.config.EnableGeoSpells = false;
                                MessageBox.Show("An error has occurred with INDI spell casting, please report what spell was active at the time.");
                            }
                            else if (SpellCheckedResult == "SpellRecast" || SpellCheckedResult == "SpellUnknown")
                            {
                            }
                            else
                            {
                                CastingManager.QueueSpell(SpellType.GEO, "<me>", SpellCheckedResult, SpellPrio.Top);
                            }

                        }
                        #endregion
                        #region "geo"

                        // GEO SPELL CASTING 
                        if ((OptionsForm.config.EnableLuopanSpells) && (_ELITEAPIPL.Player.Pet.HealthPercent < 1) && (CheckEngagedStatus() == true))
                        {
                            var abList = new List<JobAbility>() { };
                            // Use BLAZE OF GLORY if ENABLED
                            if (OptionsForm.config.BlazeOfGlory && JaManager.IsJaReady("Blaze of Glory") && playerHelper.HasAbility("Blaze of Glory") && CheckEngagedStatus() == true && GEO_EnemyCheck() == true)
                            {
                                abList.Add(new JobAbility("Blaze of Glory", "<me>", StatusEffect.Unknown));
                            }

                            // Grab GEO spell name
                            string SpellCheckedResult = ReturnGeoSpell(OptionsForm.config.GeoSpell_Spell, 2);

                            if (SpellCheckedResult == "SpellError_Cancel")
                            {
                                OptionsForm.config.EnableGeoSpells = false;
                                MessageBox.Show("An error has occurred with GEO spell casting, please report what spell was active at the time.");
                            }
                            else if (SpellCheckedResult == "SpellRecast" || SpellCheckedResult == "SpellUnknown")
                            {
                                // Do nothing and continue on with the program
                            }
                            else
                            {
                                if (_ELITEAPIPL.Resources.GetSpell(SpellCheckedResult, 0).ValidTargets == 5)
                                { // PLAYER CHARACTER TARGET
                                    if (OptionsForm.config.LuopanSpell_Target == string.Empty)
                                    {
                                        CastingManager.QueueSpell(SpellType.GEO, _ELITEAPIMonitored.Player.Name, SpellCheckedResult, SpellPrio.Top, abList);
                                    }
                                    else
                                    {
                                        CastingManager.QueueSpell(SpellType.GEO, OptionsForm.config.LuopanSpell_Target, SpellCheckedResult, SpellPrio.Top, abList);
                                    }
                                }
                                else
                                { // ENEMY BASED TARGET NEED TO ASSURE PLAYER IS ENGAGED
                                    if (CheckEngagedStatus() == true)
                                    {
                                        CastingManager.QueueSpell(SpellType.GEO, "<t>", SpellCheckedResult, SpellPrio.Top, abList);
                                    }
                                }
                            }
                        }
                        #endregion
                        if ((OptionsForm.config.autoTarget == true) && playerHelper.IsAbleToCastSpell(OptionsForm.config.autoTargetSpell))
                        {
                            if (OptionsForm.config.Hate_SpellType == 1) // PARTY BASED HATE SPELL
                            {
                                int enemyID = CheckEngagedStatus_Hate();

                                if (enemyID != 0 && enemyID != lastKnownEstablisherTarget)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, OptionsForm.config.autoTarget_Target, OptionsForm.config.autoTargetSpell);
                                    lastKnownEstablisherTarget = enemyID;
                                }
                            }
                            else // ENEMY BASED TARGET
                            {
                                int enemyID = CheckEngagedStatus_Hate();

                                if (enemyID != 0 && enemyID != lastKnownEstablisherTarget)
                                {
                                    CastingManager.QueueSpell(SpellType.GEO, "<t>", OptionsForm.config.autoTargetSpell, SpellPrio.Top);
                                    lastKnownEstablisherTarget = enemyID;
                                }
                            }
                        }

                        // BARD SONGS

                        if (OptionsForm.config.enableSinging && !playerHelper.plStatusCheck(StatusEffect.Silence) && (_ELITEAPIPL.Player.Status == 1 || _ELITEAPIPL.Player.Status == 0))
                        {
                            Run_BardSongs();

                        }

                        #region "pl_jas"
                        // so PL job abilities are in order
                        if (!playerHelper.plStatusCheck(StatusEffect.Amnesia) && (_ELITEAPIPL.Player.Status == 1 || _ELITEAPIPL.Player.Status == 0))
                        {
                            if ((OptionsForm.config.AfflatusSolace) && (!playerHelper.plStatusCheck(StatusEffect.Afflatus_Solace)) && JaManager.IsJaReady("Afflatus Solace") && (playerHelper.HasAbility("Afflatus Solace")))
                            {
                                JaManager.JobAbility_Wait("Afflatus Solace", "Afflatus Solace");
                                return;
                            }
                            if ((OptionsForm.config.AfflatusMisery) && (!playerHelper.plStatusCheck(StatusEffect.Afflatus_Misery)) && JaManager.IsJaReady("Afflatus Misery") && (playerHelper.HasAbility("Afflatus Misery")))
                            {
                                JaManager.JobAbility_Wait("Afflatus Misery", "Afflatus Misery");
                                return;
                            }
                            if ((OptionsForm.config.Composure) && (!playerHelper.plStatusCheck(StatusEffect.Composure)) && JaManager.IsJaReady("Composure") && (playerHelper.HasAbility("Composure")))
                            {
                                JaManager.JobAbility_Wait("Composure #2", "Composure");
                                return;
                            }
                            if ((OptionsForm.config.LightArts) && (!playerHelper.plStatusCheck(StatusEffect.Light_Arts)) && (!playerHelper.plStatusCheck(StatusEffect.Addendum_White)) && JaManager.IsJaReady("Light Arts") && (playerHelper.HasAbility("Light Arts")))
                            {
                                JaManager.JobAbility_Wait("Light Arts #2", "Light Arts");
                            }
                            if ((OptionsForm.config.AddendumWhite) && !playerHelper.plStatusCheck(StatusEffect.Addendum_White) && JaManager.IsJaReady("Stratagems") && playerHelper.HasAbility("Stratagems"))
                            {
                                JaManager.JobAbility_Wait("Addendum: White", "Addendum: White");
                                return;
                            }
                            if ((OptionsForm.config.Sublimation) && (!playerHelper.plStatusCheck(StatusEffect.Sublimation_Activated)) && (!playerHelper.plStatusCheck(StatusEffect.Sublimation_Complete)) && (!playerHelper.plStatusCheck(StatusEffect.Refresh)) && JaManager.IsJaReady("Sublimation") && (playerHelper.HasAbility("Sublimation")))
                            {
                                JaManager.JobAbility_Wait("Sublimation, Charging", "Sublimation");
                                return;
                            }
                            if ((OptionsForm.config.Sublimation) && ((_ELITEAPIPL.Player.MPMax - _ELITEAPIPL.Player.MP) > OptionsForm.config.sublimationMP) && (playerHelper.plStatusCheck(StatusEffect.Sublimation_Complete)) && JaManager.IsJaReady("Sublimation") && (playerHelper.HasAbility("Sublimation")))
                            {
                                JaManager.JobAbility_Wait("Sublimation, Recovery", "Sublimation");
                                return;
                            }
                            if ((OptionsForm.config.DivineCaress) && (OptionsForm.config.plDebuffEnabled || OptionsForm.config.monitoredDebuffEnabled || OptionsForm.config.enablePartyDebuffRemoval) && JaManager.IsJaReady("Divine Caress") && (playerHelper.HasAbility("Divine Caress")))
                            {
                                JaManager.JobAbility_Wait("Divine Caress", "Divine Caress");
                                return;
                            }
                           
                            if (OptionsForm.config.Dematerialize && CheckEngagedStatus() == true && _ELITEAPIPL.Player.Pet.HealthPercent >= 90 && JaManager.IsJaReady("Dematerialize") && playerHelper.HasAbility("Dematerialize"))
                            {
                                JaManager.JobAbility_Wait("Dematerialize", "Dematerialize");
                                return;
                            }
                            if (OptionsForm.config.EclipticAttrition && CheckEngagedStatus() == true && _ELITEAPIPL.Player.Pet.HealthPercent >= 90 && JaManager.IsJaReady("Ecliptic Attrition") && playerHelper.HasAbility("Ecliptic Attrition"))
                            {
                                JaManager.JobAbility_Wait("Ecliptic Attrition", "Ecliptic Attrition");
                                return;
                            }
                            if (OptionsForm.config.LifeCycle && CheckEngagedStatus() == true && _ELITEAPIPL.Player.Pet.HealthPercent <= 30 && _ELITEAPIPL.Player.Pet.HealthPercent >= 5 && _ELITEAPIPL.Player.HPP >= 90 && JaManager.IsJaReady("Life Cycle") && playerHelper.HasAbility("Life Cycle"))
                            {
                                JaManager.JobAbility_Wait("Life Cycle", "Life Cycle");
                                return;
                            }
                            if ((OptionsForm.config.Devotion) && JaManager.IsJaReady("Devotion") && (playerHelper.HasAbility("Devotion")) && _ELITEAPIPL.Player.HPP > 80 && (!OptionsForm.config.DevotionWhenEngaged || (_ELITEAPIMonitored.Player.Status == 1)))
                            {
                                // First Generate the current party number, this will be used
                                // regardless of the type
                                int memberOF = CureManager.GetPLPartyNumber();

                                // Now generate the party
                                IEnumerable<EliteAPI.PartyMember> cParty = _ELITEAPIMonitored.Party.GetPartyMembers().Where(p => p.Active != 0 && p.Zone == _ELITEAPIPL.Player.ZoneId);

                                // Make sure member number is not 0 (null) or 4 (void)
                                if (memberOF != 0 && memberOF != 4)
                                {
                                    // Run through Each party member as we're looking for either a specifc name or if set otherwise anyone with the MP criteria in the current party.
                                    foreach (EliteAPI.PartyMember pData in cParty)
                                    {
                                        // If party of party v1
                                        if (memberOF == 1 && pData.MemberNumber >= 0 && pData.MemberNumber <= 5)
                                        {
                                            if (!string.IsNullOrEmpty(pData.Name) && pData.Name != _ELITEAPIPL.Player.Name)
                                            {
                                                if ((OptionsForm.config.DevotionTargetType == 0))
                                                {
                                                    if (pData.Name == OptionsForm.config.DevotionTargetName)
                                                    {
                                                        EliteAPI.XiEntity playerInfo = _ELITEAPIPL.Entity.GetEntity((int)pData.TargetIndex);
                                                        if (playerInfo.Distance < 10 && playerInfo.Distance > 0 && pData.CurrentMP <= OptionsForm.config.DevotionMP && pData.CurrentMPP <= 30 && pData.CurrentHP > 0)
                                                        {
                                                            JaManager.JobAbility_Wait("Devotion", "Devotion", OptionsForm.config.DevotionTargetName);
                                                            return;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    EliteAPI.XiEntity playerInfo = _ELITEAPIPL.Entity.GetEntity((int)pData.TargetIndex);

                                                    if ((pData.CurrentMP <= OptionsForm.config.DevotionMP) && (playerInfo.Distance < 10) && pData.CurrentMPP <= 30 && pData.CurrentHP > 0)
                                                    {
                                                        JaManager.JobAbility_Wait("Devotion", "Devotion", pData.Name);
                                                        return;
                                                    }
                                                }
                                            }
                                        } // If part of party 2
                                        else if (memberOF == 2 && pData.MemberNumber >= 6 && pData.MemberNumber <= 11)
                                        {
                                            if (!string.IsNullOrEmpty(pData.Name) && pData.Name != _ELITEAPIPL.Player.Name)
                                            {
                                                if ((OptionsForm.config.DevotionTargetType == 0))
                                                {
                                                    if (pData.Name == OptionsForm.config.DevotionTargetName)
                                                    {
                                                        EliteAPI.XiEntity playerInfo = _ELITEAPIPL.Entity.GetEntity((int)pData.TargetIndex);
                                                        if (playerInfo.Distance < 10 && playerInfo.Distance > 0 && pData.CurrentMP <= OptionsForm.config.DevotionMP && pData.CurrentHP > 0)
                                                        {
                                                            JaManager.JobAbility_Wait("Devotion", "Devotion", OptionsForm.config.DevotionTargetName);
                                                            return;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    EliteAPI.XiEntity playerInfo = _ELITEAPIPL.Entity.GetEntity((int)pData.TargetIndex);

                                                    if ((pData.CurrentMP <= OptionsForm.config.DevotionMP) && (playerInfo.Distance < 10) && pData.CurrentMPP <= 50 && pData.CurrentHP > 0)
                                                    {
                                                        JaManager.JobAbility_Wait("Devotion", "Devotion", pData.Name);
                                                        return;
                                                    }
                                                }
                                            }
                                        } // If part of party 3
                                        else if (memberOF == 3 && pData.MemberNumber >= 12 && pData.MemberNumber <= 17)
                                        {
                                            if (!string.IsNullOrEmpty(pData.Name) && pData.Name != _ELITEAPIPL.Player.Name)
                                            {
                                                if ((OptionsForm.config.DevotionTargetType == 0))
                                                {
                                                    if (pData.Name == OptionsForm.config.DevotionTargetName)
                                                    {
                                                        EliteAPI.XiEntity playerInfo = _ELITEAPIPL.Entity.GetEntity((int)pData.TargetIndex);
                                                        if (playerInfo.Distance < 10 && playerInfo.Distance > 0 && pData.CurrentMP <= OptionsForm.config.DevotionMP && pData.CurrentHP > 0)
                                                        {
                                                            JaManager.JobAbility_Wait("Devotion", "Devotion", OptionsForm.config.DevotionTargetName);
                                                            return;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    EliteAPI.XiEntity playerInfo = _ELITEAPIPL.Entity.GetEntity((int)pData.TargetIndex);

                                                    if ((pData.CurrentMP <= OptionsForm.config.DevotionMP) && (playerInfo.Distance < 10) && pData.CurrentMPP <= 50 && pData.CurrentHP > 0)
                                                    {
                                                        JaManager.JobAbility_Wait("Devotion", "Devotion", pData.Name);
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        var playerBuffOrder = _ELITEAPIMonitored.Party.GetPartyMembers().OrderBy(p => p.MemberNumber).Where(p => p.Active == 1);


                        // Auto Casting
                        foreach (var charDATA in playerBuffOrder)
                        {
                            // Grab the Storm Spells name to perform checks.
                            string StormSpell_Enabled = SpellsHelper.GetEnabledStormSpell(charDATA.Name);

                            // Grab storm spell Data for Buff ID etc...
                            SpellsData PTstormspell = StormSpells.stormspells.Where(c => c.Spell_Name == StormSpell_Enabled).SingleOrDefault();

                            if (!playerHelper.IsAlive(charDATA.Name)) { continue; } //no love for the dead

                            // PL BASED BUFFS
                            if (_ELITEAPIPL.Player.Name == charDATA.Name)
                            {
                                #region "pl"
                                var spell = "Haste";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !playerHelper.plStatusCheck(StatusEffect.Haste) && !playerHelper.plStatusCheck(StatusEffect.Slow))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Haste II";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !playerHelper.plStatusCheck(StatusEffect.Haste) && !playerHelper.plStatusCheck(StatusEffect.Slow))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Adloquium";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !BuffChecker(170, 0))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Flurry";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !BuffChecker(581, 0) && !playerHelper.plStatusCheck(StatusEffect.Slow))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Flurry II";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !BuffChecker(581, 0) && !playerHelper.plStatusCheck(StatusEffect.Slow))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetShellSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !playerHelper.plStatusCheck(StatusEffect.Shell))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetProtectSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !playerHelper.plStatusCheck(StatusEffect.Protect))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Phalanx II";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !playerHelper.plStatusCheck(StatusEffect.Phalanx))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetRegenSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !playerHelper.plStatusCheck(StatusEffect.Regen))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetRefreshSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !playerHelper.plStatusCheck(StatusEffect.Refresh))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetEnabledStormSpell(charDATA.Name);
                                if(spell != "Dummy") { 
                                    if (CheckIfAutoStormspellEnabled(charDATA.Name) && (_ELITEAPIPL.Player.MP > OptionsForm.config.mpMinCastValue) && !BuffChecker(PTstormspell.buffID, 0) && playerHelper.IsAbleToCastSpell(spell))
                                    {
                                        CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                    }
                                }
                                #endregion
                            }
                            // MONITORED PLAYER BASED BUFFS
                            else if (_ELITEAPIMonitored.Player.Name == charDATA.Name && playerHelper.CastingPossible(charDATA.Name))
                            {
                                var spell = "Haste";
                                #region "monitored"
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !monitoredStatusCheck(StatusEffect.Haste) && !monitoredStatusCheck(StatusEffect.Slow))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Haste II";

                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !monitoredStatusCheck(StatusEffect.Haste) && !monitoredStatusCheck(StatusEffect.Slow))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Adloquium";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !BuffChecker(170, 1))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);

                                }
                                spell = "Flurry";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !BuffChecker(581, 1) && !monitoredStatusCheck(StatusEffect.Slow))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Flurry II";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !BuffChecker(581, 1) && !monitoredStatusCheck(StatusEffect.Slow))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetShellSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !monitoredStatusCheck(StatusEffect.Shell))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetProtectSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !monitoredStatusCheck(StatusEffect.Protect))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Phalanx II";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !monitoredStatusCheck(StatusEffect.Phalanx))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetRegenSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !monitoredStatusCheck(StatusEffect.Regen))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetRefreshSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && !monitoredStatusCheck(StatusEffect.Refresh))
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetEnabledStormSpell(charDATA.Name);
                                if (spell != "Dummy")
                                {
                                    if (CheckIfAutoStormspellEnabled(charDATA.Name) && (_ELITEAPIPL.Player.MP > OptionsForm.config.mpMinCastValue) && playerHelper.IsAbleToCastSpell(spell) && !BuffChecker(PTstormspell.buffID, 1))
                                    {
                                        CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                    }
                                }
                                #endregion
                            }
                            else if (playerHelper.CastingPossible(charDATA.Name))
                            {
                                #region "thirdparty"
                                var spell = "Haste";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoHasteMinutes)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Haste II";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoHasteMinutes)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Adloquium";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoAdloquiumMinutes)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Flurry";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoHasteMinutes)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Flurry II";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoHasteMinutes)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetShellSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoShellMinutes)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetProtectSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoProtect_Minutes)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = "Phalanx II";
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoPhalanxIIMinutes)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetRegenSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoRegen_Minutes)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetRefreshSpell();
                                if (playerHelper.isAbleToBuff(spell, charDATA.Name) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoRefresh_Minutes)
                                {
                                    CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                }
                                spell = SpellsHelper.GetEnabledStormSpell(charDATA.Name);
                                if (spell != "Dummy")
                                {
                                    if (CheckIfAutoStormspellEnabled(charDATA.Name) && (_ELITEAPIPL.Player.MP > OptionsForm.config.mpMinCastValue) && playerHelper.IsAbleToCastSpell(spell) && InternalHelper.getTimeSpanInMinutes(spell, charDATA.Name) >= OptionsForm.config.autoStormspellMinutes)
                                    {
                                        CastingManager.QueueSpell(SpellType.Buff, charDATA.Name, spell);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
        }


        private bool CheckIfAutoStormspellEnabled(string name)
        { 
            return SpellsHelper.GetEnabledStormSpell(name) != "Dummy";
        }

        private string GetShellraLevel(decimal p)
        {
            switch ((int)p)
            {
                case 1:
                    return "Shellra";

                case 2:
                    return "Shellra II";

                case 3:
                    return "Shellra III";

                case 4:
                    return "Shellra IV";

                case 5:
                    return "Shellra V";

                default:
                    return "Shellra";
            }
        }

        private string GetProtectraLevel(decimal p)
        {
            switch ((int)p)
            {
                case 1:
                    return "Protectra";

                case 2:
                    return "Protectra II";

                case 3:
                    return "Protectra III";

                case 4:
                    return "Protectra IV";

                case 5:
                    return "Protectra V";

                default:
                    return "Protectra";
            }
        }

        private string ReturnGeoSpell(int GEOSpell_ID, int GeoSpell_Type)
        {
            // GRAB THE SPELL FROM THE CUSTOM LIST
            var GeoSpells = new GeoSpells();
            GeoData GeoSpell = GeoSpells.GeomancerInfo.Where(c => c.geo_position == GEOSpell_ID).FirstOrDefault();

            if (GeoSpell_Type == 1)
            {
                if (playerHelper.IsAbleToCastSpell(GeoSpell.indi_spell))
                {
                    if (playerHelper.IsSpellRecastReady(GeoSpell.indi_spell))
                    {
                        return GeoSpell.indi_spell;
                    }
                    else
                    {
                        return "SpellRecast";
                    }
                }
                else
                {
                    return "SpellNA";
                }
            }
            else if (GeoSpell_Type == 2)
            {
                if (playerHelper.IsAbleToCastSpell(GeoSpell.geo_spell))
                {
                    if (playerHelper.IsSpellRecastReady(GeoSpell.geo_spell))
                    {
                        return GeoSpell.geo_spell;
                    }
                    else
                    {
                        return "SpellRecast";
                    }
                }
                else
                {
                    return "SpellNA";
                }
            }
            else
            {
                return "SpellError_Cancel";
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm settings = new OptionsForm();
            settings.Show();
        }


        private bool isIncharacterNames_naRemoval(int pos)
        {
            string generated_name = _ELITEAPIMonitored.Party.GetPartyMembers()[pos].Name;
            return characterNames_naRemoval.Contains(generated_name);
        }

        private void optionsButtonClick(GroupBox box, byte pos)
        {
            string generated_name = _ELITEAPIMonitored.Party.GetPartyMembers()[pos].Name;
            playerOptionsSelected = generated_name;
            autoHasteToolStripMenuItem.Checked = InternalHelper.getAutoEnable("Haste", generated_name);
            autoHasteIIToolStripMenuItem.Checked = InternalHelper.getAutoEnable("Haste II", generated_name);
            autoAdloquiumToolStripMenuItem.Checked = InternalHelper.getAutoEnable("Adloqium", generated_name);
            autoFlurryToolStripMenuItem.Checked = InternalHelper.getAutoEnable("Flurry", generated_name);
            autoFlurryIIToolStripMenuItem.Checked = InternalHelper.getAutoEnable("Flurry II", generated_name);
            autoProtectToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetProtectSpell(), generated_name);
            autoShellToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetShellSpell(), generated_name);
            enableDebuffRemovalToolStripMenuItem.Checked = isIncharacterNames_naRemoval(pos);

            playerOptions.Show(box, new Point(0, 0));
        }

        private void player0optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party0, 0);

        }

        private void player1optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party0, 1);
        }

        private void player2optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party0, 2);
        }

        private void player3optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party0, 3);
        }

        private void player4optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party0, 4);
        }

        private void player5optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party0, 5);
        }

        private void player6optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party1, 6);
        }

        private void player7optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party1, 7);
        }

        private void player8optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party1, 8);
        }

        private void player9optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party1, 9);
        }

        private void player10optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party1, 10);
        }

        private void player11optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party1, 11);
        }

        private void player12optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party2, 12);
        }

        private void player13optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party2, 13);
        }

        private void player14optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party2, 14);
        }

        private void player15optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party2, 15);
        }

        private void player16optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party2, 16);
        }

        private void player17optionsButton_Click(object sender, EventArgs e)
        {
            optionsButtonClick(party2, 17);
        }

        private void playerbuffsButtonClick(GroupBox box, byte pos)
        {
            string generated_name = _ELITEAPIMonitored.Party.GetPartyMembers()[pos].Name;
            autoOptionsSelected = generated_name;
            autoPhalanxIIToolStripMenuItem1.Checked = InternalHelper.getAutoEnable("Phalanx II", generated_name);
            autoRegenVToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetRegenSpell(), generated_name);
            autoRefreshIIToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetRefreshSpell(), generated_name);
            SandstormToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetStormVersion("Sandstorm"), generated_name);
            RainstormToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetStormVersion("Rainstorm"), generated_name);
            WindstormToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetStormVersion("Windstorm"), generated_name);
            FirestormToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetStormVersion("Firestorm"), generated_name);
            HailstormToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetStormVersion("Hailstorm"), generated_name);
            ThunderstormToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetStormVersion("Thunderstorm"), generated_name);
            VoidstormToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetStormVersion("Voidstorm"), generated_name);
            AurorastormToolStripMenuItem.Checked = InternalHelper.getAutoEnable(SpellsHelper.GetStormVersion("Aurorastorm"), generated_name);
            autoOptions.Show(box, new Point(0, 0));
        }

        private void player0buffsButton_Click(object sender, EventArgs e)
        {
            playerbuffsButtonClick(party0, 0);
        }

        private void player1buffsButton_Click(object sender, EventArgs e)
        {
            playerbuffsButtonClick(party0, 1);
        }

        private void player2buffsButton_Click(object sender, EventArgs e)
        {
            playerbuffsButtonClick(party0, 2);
        }

        private void player3buffsButton_Click(object sender, EventArgs e)
        {
            playerbuffsButtonClick(party0, 3);
        }

        private void player4buffsButton_Click(object sender, EventArgs e)
        {
            playerbuffsButtonClick(party0, 4);
        }

        private void player5buffsButton_Click(object sender, EventArgs e)
        {
            playerbuffsButtonClick(party0, 5);
        }

        private void player6buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[6].Name;
            autoOptions.Show(party1, new Point(0, 0));
        }

        private void player7buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[7].Name;
            autoOptions.Show(party1, new Point(0, 0));
        }

        private void player8buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[8].Name;
            autoOptions.Show(party1, new Point(0, 0));
        }

        private void player9buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[9].Name;
            autoOptions.Show(party1, new Point(0, 0));
        }

        private void player10buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[10].Name;
            autoOptions.Show(party1, new Point(0, 0));
        }

        private void player11buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[11].Name;
            autoOptions.Show(party1, new Point(0, 0));
        }

        private void player12buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[12].Name;
            autoOptions.Show(party2, new Point(0, 0));
        }

        private void player13buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[13].Name;
            autoOptions.Show(party2, new Point(0, 0));
        }

        private void player14buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[14].Name;
            autoOptions.Show(party2, new Point(0, 0));
        }

        private void player15buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[15].Name;
            autoOptions.Show(party2, new Point(0, 0));
        }

        private void player16buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[16].Name; 
            autoOptions.Show(party2, new Point(0, 0));
        }

        private void player17buffsButton_Click(object sender, EventArgs e)
        {
            autoOptionsSelected = _ELITEAPIMonitored.Party.GetPartyMembers()[17].Name;
            autoOptions.Show(party2, new Point(0, 0));
        }

        public void Item_Wait(string ItemName)
        {
            if (CastingManager.CanAct())
            {
                Invoke((MethodInvoker)(async () =>
                {
                    var time = CastingManager.GetLock();
                    castingLockLabel.Text = "Casting is LOCKED for ITEM Use.";
                    currentAction.Text = "Using an Item: " + ItemName;
                    _ELITEAPIPL.ThirdParty.SendString("/item \"" + ItemName + "\" <me>");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    castingLockLabel.Text = "Casting is UNLOCKED";
                    currentAction.Text = string.Empty;
                    castingSpell = string.Empty;
                    CastingManager.FreeLock("Item_Wait", time);
                }));
            }
        }

        private void autoHasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternalHelper.setAutoEnable("Haste", playerOptionsSelected, !InternalHelper.getAutoEnable("Haste", playerOptionsSelected));
            InternalHelper.setAutoEnable("Haste II", playerOptionsSelected, false);
            InternalHelper.setAutoEnable("Flurry", playerOptionsSelected, false);
            InternalHelper.setAutoEnable("Flurry II", playerOptionsSelected, false);
        }

        private void autoHasteIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternalHelper.setAutoEnable("Haste II", playerOptionsSelected, !InternalHelper.getAutoEnable("Haste II", playerOptionsSelected));
            InternalHelper.setAutoEnable("Haste", playerOptionsSelected, false);
            InternalHelper.setAutoEnable("Flurry", playerOptionsSelected, false);
            InternalHelper.setAutoEnable("Flurry II", playerOptionsSelected, false);
        }

        private void autoAdloquiumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternalHelper.setAutoEnable("Adloquium", playerOptionsSelected, !InternalHelper.getAutoEnable("Adloquium", playerOptionsSelected));
        }

        private void autoFlurryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternalHelper.setAutoEnable("Flurry", playerOptionsSelected, !InternalHelper.getAutoEnable("Flurry", playerOptionsSelected));
            InternalHelper.setAutoEnable("Haste", playerOptionsSelected, false);
            InternalHelper.setAutoEnable("Haste II", playerOptionsSelected, false);
            InternalHelper.setAutoEnable("Flurry II", playerOptionsSelected, false);
        }

        private void autoFlurryIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternalHelper.setAutoEnable("Flurry II", playerOptionsSelected, !InternalHelper.getAutoEnable("Flurry II", playerOptionsSelected));
            InternalHelper.setAutoEnable("Haste", playerOptionsSelected, false);
            InternalHelper.setAutoEnable("Flurry", playerOptionsSelected, false);
            InternalHelper.setAutoEnable("Haste II", playerOptionsSelected, false);
        }

        private void autoProtectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternalHelper.setAutoEnable(SpellsHelper.GetProtectSpell(), playerOptionsSelected, !InternalHelper.getAutoEnable(SpellsHelper.GetProtectSpell(), playerOptionsSelected));
        }

        private void enableDebuffRemovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string generated_name = playerOptionsSelected;
            if (characterNames_naRemoval.Contains(generated_name))
            {
                characterNames_naRemoval.Remove(generated_name);
            }
            else
            {
                characterNames_naRemoval.Add(generated_name);
            }
        }

        private void autoShellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            InternalHelper.setAutoEnable(SpellsHelper.GetShellSpell(), playerOptionsSelected, !InternalHelper.getAutoEnable(SpellsHelper.GetShellSpell(), playerOptionsSelected));
        }

        private void autoPhalanxIIToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InternalHelper.setAutoEnable("Phalanx II", autoOptionsSelected, !InternalHelper.getAutoEnable("Phalanx II", autoOptionsSelected));
        }

        private void autoRegenVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternalHelper.setAutoEnable(SpellsHelper.GetRegenSpell(), autoOptionsSelected, !InternalHelper.getAutoEnable(SpellsHelper.GetRegenSpell(), autoOptionsSelected));
        }

        private void autoRefreshIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternalHelper.setAutoEnable(SpellsHelper.GetRefreshSpell(), autoOptionsSelected, !InternalHelper.getAutoEnable(SpellsHelper.GetRefreshSpell(), autoOptionsSelected));
        }

        private void hasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, "Haste", playerOptionsSelected);
        }

        private void followToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm.config.autoFollowName = playerOptionsSelected;
        }

        private void stopfollowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm.config.autoFollowName = string.Empty;
        }

        private void EntrustTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm.config.EntrustedSpell_Target = playerOptionsSelected;
        }

        private void GeoTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm.config.LuopanSpell_Target = playerOptionsSelected;
        }

        private void DevotionTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm.config.DevotionTargetName = playerOptionsSelected;
        }

        private void HateEstablisherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm.config.autoTarget_Target = playerOptionsSelected;
        }

        /* NOT USED ATM COMMENTED FOR LATER USE
         * private void phalanxIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Phalanx II");
        }

        private void invisibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Invisible");
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Refresh");
        }

        private void refreshIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Refresh II");
        }

        private void refreshIIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Refresh III");
        }

        private void sneakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Sneak");
        }

        private void regenIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Regen II");
        }

        private void regenIIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Regen III");
        }

        private void regenIVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Regen IV");
        }

        private void eraseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Erase");
        }

        private void sacrificeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Sacrifice");
        }

        private void blindnaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Blindna");
        }

        private void cursnaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Cursna");
        }

        private void paralynaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Paralyna");
        }

        private void poisonaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Poisona");
        }

        private void stonaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Stona");
        }

        private void silenaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Silena");
        }

        private void virunaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Viruna");
        }*/

        private void setAllStormsFalse(string player)
        {
            // MessageBox.Show("SONG DATA: " + activeStorm + " " + autoOptionsSelected);
            InternalHelper.setAutoEnable(SpellsHelper.GetStormVersion("Sandstorm"), player, false);
            InternalHelper.setAutoEnable(SpellsHelper.GetStormVersion("Rainstorm"), player, false);
            InternalHelper.setAutoEnable(SpellsHelper.GetStormVersion("Windstorm"), player, false);
            InternalHelper.setAutoEnable(SpellsHelper.GetStormVersion("Firestorm"), player, false);
            InternalHelper.setAutoEnable(SpellsHelper.GetStormVersion("Hailstorm"), player, false);
            InternalHelper.setAutoEnable(SpellsHelper.GetStormVersion("Thunderstorm"), player, false);
            InternalHelper.setAutoEnable(SpellsHelper.GetStormVersion("Voidstorm"), player, false);
            InternalHelper.setAutoEnable(SpellsHelper.GetStormVersion("Aurorastorm"), player, false);
        }

        private void toggleStormSpell(string storm, string player)
        {
            bool currentStatus = InternalHelper.getAutoEnable(SpellsHelper.GetStormVersion(storm), player);
            setAllStormsFalse(player);
            InternalHelper.setAutoEnable(SpellsHelper.GetStormVersion(storm), player, !currentStatus);
        }

        private void SandstormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleStormSpell("Sandstorm", autoOptionsSelected);
        }

        private void RainstormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleStormSpell("Rainstorm", autoOptionsSelected);
        }

        private void WindstormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleStormSpell("Windstorm", autoOptionsSelected);
        }

        private void FirestormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleStormSpell("Firestorm", autoOptionsSelected);
        }

        private void HailstormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleStormSpell("Hailstorm", autoOptionsSelected);
        }

        private void ThunderstormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleStormSpell("Thunderstorm", autoOptionsSelected);
        }

        private void VoidstormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleStormSpell("Voidstorm", autoOptionsSelected);
        }

        private void AurorastormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toggleStormSpell("Aurorastorm", autoOptionsSelected);
        }

        /* NOT USED COMMENTED FOR LATER USE
         * private void protectIVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Protect IV");
        }

        private void protectVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Protect V");
        }

        private void shellIVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Shell IV");
        }

        private void shellVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CastingManager.QueueSpell(SpellType.Buff, playerOptionsSelected, "Shell V");
        }*/

        private void button3_Click(object sender, EventArgs e)
        {


            song_casting = 0;
            ForceSongRecast = true;

            if (pauseActions == false)
            {
                pauseButton.Text = "Paused!";
                pauseButton.ForeColor = Color.Red;
                DisableTickers();
                ActiveBuffs.Clear();
                pauseActions = true;
                if (OptionsForm.config.FFXIDefaultAutoFollow == false)
                {
                    _ELITEAPIPL.AutoFollow.IsAutoFollowing = false;
                }
            }
            else
            {
                pauseButton.Text = "Pause";
                pauseButton.ForeColor = Color.Black;
                EnableTickers();
                pauseActions = false;

                if (OptionsForm.config.MinimiseonStart == true && WindowState != FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Minimized;
                }

                if (OptionsForm.config.EnableAddOn && LUA_Plugin_Loaded == 0)
                {
                    if (WindowerMode == "Windower")
                    {
                        _ELITEAPIPL.ThirdParty.SendString("//lua load CurePlease_addon");
                        Thread.Sleep(1500);
                        _ELITEAPIPL.ThirdParty.SendString("//cpaddon settings " + OptionsForm.config.ipAddress + " " + OptionsForm.config.listeningPort);
                        Thread.Sleep(100);
                        if (OptionsForm.config.enableHotKeys)
                        {
                            _ELITEAPIPL.ThirdParty.SendString("//bind ^!F1 cureplease toggle");
                            _ELITEAPIPL.ThirdParty.SendString("//bind ^!F2 cureplease start");
                            _ELITEAPIPL.ThirdParty.SendString("//bind ^!F3 cureplease pause");
                        }
                    }
                    else if (WindowerMode == "Ashita")
                    {
                        _ELITEAPIPL.ThirdParty.SendString("/addon load CurePlease_addon");
                        Thread.Sleep(1500);
                        _ELITEAPIPL.ThirdParty.SendString("/cpaddon settings " + OptionsForm.config.ipAddress + " " + OptionsForm.config.listeningPort);
                        Thread.Sleep(100);
                        if (OptionsForm.config.enableHotKeys)
                        {
                            _ELITEAPIPL.ThirdParty.SendString("/bind ^!F1 /cureplease toggle");
                            _ELITEAPIPL.ThirdParty.SendString("/bind ^!F2 /cureplease start");
                            _ELITEAPIPL.ThirdParty.SendString("/bind ^!F3 /cureplease pause");
                        }
                    }

                    AddOnStatus_Click(sender, e);


                    LUA_Plugin_Loaded = 1;


                }
            }
        }

        private void Debug_Click(object sender, EventArgs e)
        {
            if (CastingManager == null) { MessageBox.Show("Attach to process before pressing this button", "Error"); return; }
            new CastingMonitorForm(this, CastingManager).Show();

            //MessageBox.Show(debug_MSG_show);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (TopMost)
            {
                TopMost = false;
            }
            else
            {
                TopMost = true;
            }
        }

        private void MouseClickTray(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && Visible == false)
            {
                Show();
                WindowState = FormWindowState.Normal;
            }
            else
            {
                Hide();
                WindowState = FormWindowState.Minimized;
            }
        }

        private bool CheckShellraLevelPossession()
        {
            switch ((int)OptionsForm.config.plShellra_Level)
            {
                case 1:
                    if (playerHelper.JobChecker("Shellra") == true && playerHelper.IsAbleToCastSpell("Shellra"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case 2:
                    if (playerHelper.JobChecker("Shellra II") == true && playerHelper.IsAbleToCastSpell("Shellra II"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case 3:
                    if (playerHelper.JobChecker("Shellra III") == true && playerHelper.IsAbleToCastSpell("Shellra III"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case 4:
                    if (playerHelper.JobChecker("Shellra IV") == true && playerHelper.IsAbleToCastSpell("Shellra IV"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case 5:
                    if (playerHelper.JobChecker("Shellra V") == true && playerHelper.IsAbleToCastSpell("Shellra V"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        private bool CheckProtectraLevelPossession()
        {
            switch ((int)OptionsForm.config.plProtectra_Level)
            {
                case 1:
                    if (playerHelper.IsAbleToCastSpell("Protectra"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case 2:
                    if (playerHelper.IsAbleToCastSpell("Protectra II"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case 3:
                    if (playerHelper.IsAbleToCastSpell("Protectra III"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case 4:
                    if (playerHelper.IsAbleToCastSpell("Protectra IV"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case 5:
                    if (playerHelper.IsAbleToCastSpell("Protectra V"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        private bool CheckReraiseLevelPossession()
        {
            switch (OptionsForm.config.plReraise_Level)
            {
                case 1:
                    if (playerHelper.IsAbleToCastSpell("Reraise"))
                    {
                        // Check SCH possiblity
                        if (_ELITEAPIPL.Player.MainJob == 20 && _ELITEAPIPL.Player.SubJob != 3 && !BuffChecker(401, 0))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }

                case 2:

                    if (playerHelper.IsAbleToCastSpell("Reraise II"))
                    {
                        if (_ELITEAPIPL.Player.MainJob == 20 && !BuffChecker(401, 0))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }

                case 3:

                    if (playerHelper.IsAbleToCastSpell("Reraise III"))
                    {
                        if (_ELITEAPIPL.Player.MainJob == 20 && !BuffChecker(401, 0))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }

                case 4:
                    if (playerHelper.IsAbleToCastSpell("Reraise IV"))
                    {
                        if (_ELITEAPIPL.Player.MainJob == 20 && !BuffChecker(401, 0))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }

                default:
                    return false;
            }
        }

        private bool CheckRefreshLevelPossession()
        {
            switch (OptionsForm.config.plRefresh_Level)
            {
                case 1:
                    return playerHelper.HasSpell("Refresh");

                case 2:
                    return playerHelper.HasSpell("Refresh II");

                case 3:
                    return playerHelper.HasSpell("Refresh III");

                default:
                    return false;
            }
        }



        private bool CheckRegenLevelPossession()
        {
            switch (OptionsForm.config.plRegen_Level)
            {
                case 1:
                    return playerHelper.HasSpell("Regen");

                case 2:
                    return playerHelper.HasSpell("Regen II");

                case 3:
                    return playerHelper.HasSpell("Regen III");

                case 4:
                    return playerHelper.HasSpell("Regen IV");

                case 5:
                    return playerHelper.HasSpell("Regen V");

                default:
                    return false;
            }
        }

        private void chatLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChatLogForm form4 = new ChatLogForm(this);
            form4.Show();
        }

        private void partyBuffsdebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PartyBuffs PartyBuffs = new PartyBuffs(this);
            PartyBuffs.Show();
        }

        private void refreshCharactersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<Process> pol = Process.GetProcessesByName("pol").Union(Process.GetProcessesByName("xiloader")).Union(Process.GetProcessesByName("edenxi"));

            if (_ELITEAPIPL.Player.LoginStatus == (int)LoginStatus.Loading || _ELITEAPIMonitored.Player.LoginStatus == (int)LoginStatus.Loading)
            {
            }
            else
            {
                if (pol.Count() < 1)
                {
                    MessageBox.Show("FFXI not found");
                }
                else
                {
                    POLID.Items.Clear();
                    POLID2.Items.Clear();
                    processids.Items.Clear();

                    for (int i = 0; i < pol.Count(); i++)
                    {
                        POLID.Items.Add(pol.ElementAt(i).MainWindowTitle);
                        POLID2.Items.Add(pol.ElementAt(i).MainWindowTitle);
                        processids.Items.Add(pol.ElementAt(i).Id);
                    }

                    POLID.SelectedIndex = 0;
                    POLID2.SelectedIndex = 0;
                }
            }
        }


        private void CurePleaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Dispose();

            if (_ELITEAPIPL != null)
            {
                if (WindowerMode == "Ashita")
                {
                    _ELITEAPIPL.ThirdParty.SendString("/addon unload CurePlease_addon");
                    if (OptionsForm.config.enableHotKeys)
                    {
                        _ELITEAPIPL.ThirdParty.SendString("/unbind ^!F1");
                        _ELITEAPIPL.ThirdParty.SendString("/unbind ^!F2");
                        _ELITEAPIPL.ThirdParty.SendString("/unbind ^!F3");
                    }
                }
                else if (WindowerMode == "Windower")
                {
                    _ELITEAPIPL.ThirdParty.SendString("//lua unload CurePlease_addon");

                    if (OptionsForm.config.enableHotKeys)
                    {
                        _ELITEAPIPL.ThirdParty.SendString("//unbind ^!F1");
                        _ELITEAPIPL.ThirdParty.SendString("//unbind ^!F2");
                        _ELITEAPIPL.ThirdParty.SendString("//unbind ^!F3");
                    }

                }
            }

        }

        private int followID()
        {
            if ((setinstance2.Enabled == true) && !string.IsNullOrEmpty(OptionsForm.config.autoFollowName) && !pauseActions)
            {
                for (int x = 0; x < 2048; x++)
                {
                    EliteAPI.XiEntity entity = _ELITEAPIPL.Entity.GetEntity(x);

                    if (entity.Name != null && entity.Name.ToLower().Equals(OptionsForm.config.autoFollowName.ToLower()))
                    {
                        return Convert.ToInt32(entity.TargetID);
                    }
                }
                return -1;
            }
            else
            {
                return -1;
            }
        }

        public bool plMonitoredSameParty()
        {
            int PT_Structutre_NO = CureManager.GetPLPartyNumber();

            // Now generate the party
            IEnumerable<EliteAPI.PartyMember> cParty = _ELITEAPIMonitored.Party.GetPartyMembers().Where(p => p.Active != 0 && p.Zone == _ELITEAPIPL.Player.ZoneId);

            // Make sure member number is not 0 (null) or 4 (void)
            if (PT_Structutre_NO != 0 && PT_Structutre_NO != 4)
            {
                // Run through Each party member as we're looking for either a specific name or if set
                // otherwise anyone with the MP criteria in the current party.
                foreach (EliteAPI.PartyMember pData in cParty)
                {
                    if (PT_Structutre_NO == 1 && pData.MemberNumber >= 0 && pData.MemberNumber <= 5 && pData.Name == _ELITEAPIMonitored.Player.Name)
                    {
                        return true;
                    }
                    else if (PT_Structutre_NO == 2 && pData.MemberNumber >= 6 && pData.MemberNumber <= 11 && pData.Name == _ELITEAPIMonitored.Player.Name)
                    {
                        return true;
                    }
                    else if (PT_Structutre_NO == 3 && pData.MemberNumber >= 12 && pData.MemberNumber <= 17 && pData.Name == _ELITEAPIMonitored.Player.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        

        private void resetSongTimer_Tick(object sender, EventArgs e)
        {
            song_casting = 0;
        }

        private void checkSCHCharges_Tick(object sender, EventArgs e)
        {
            if (_ELITEAPIPL != null && _ELITEAPIMonitored != null)
            {
                int MainJob = _ELITEAPIPL.Player.MainJob;
                int SubJob = _ELITEAPIPL.Player.SubJob;

                if (MainJob == 20 || SubJob == 20)
                {
                    if (playerHelper.plStatusCheck(StatusEffect.Light_Arts) || playerHelper.plStatusCheck(StatusEffect.Addendum_White))
                    {
                        decimal currentRecastTimer = JaManager.GetRecast(231);

                        int SpentPoints = _ELITEAPIPL.Player.GetJobPoints(20).SpentJobPoints;

                        int MainLevel = _ELITEAPIPL.Player.MainJobLevel;
                        int SubLevel = _ELITEAPIPL.Player.SubJobLevel;

                        decimal baseTimer = 240;
                        decimal baseCharges = 1;

                        // Generate the correct timer between charges depending on level / Job Points
                        if (MainLevel == 99 && SpentPoints > 550 && MainJob == 20)
                        {
                            baseTimer = 33;
                            baseCharges = 5;
                        }
                        else if (MainLevel >= 90 && SpentPoints < 550 && MainJob == 20)
                        {
                            baseTimer = 48;
                            baseCharges = 5;
                        }
                        else if (MainLevel >= 70 && MainLevel < 90 && MainJob == 20)
                        {
                            baseTimer = 60;
                            baseCharges = 4;
                        }
                        else if (MainLevel >= 50 && MainLevel < 70 && MainJob == 20)
                        {
                            baseTimer = 80;
                            baseCharges = 3;
                        }
                        else if ((MainLevel >= 30 && MainLevel < 50 && MainJob == 20) || (SubLevel >= 30 && SubLevel < 50 && SubJob == 20))
                        {
                            baseTimer = 120;
                            baseCharges = 2;
                        }
                        else if ((MainLevel >= 10 && MainLevel < 30 && MainJob == 20) || (SubLevel >= 10 && SubLevel < 30 && SubJob == 20))
                        {
                            baseTimer = 240;
                            baseCharges = 1;
                        }

                        // Now knowing what the time between charges is lets calculate how many
                        // charges are available

                        int charges = 0;

                        if (currentRecastTimer == 0)
                        {
                            charges = (int)baseCharges;
                        }
                        else
                        {
      
                            var stratsUsed = Math.Ceiling((currentRecastTimer / 60) / baseTimer);
                            charges  = (int)baseCharges - (int)stratsUsed;
                            
                        }
                        if (charges != currentSCHCharges)
                        {
                            CastingManager.AddLog(new LogEntry($"Stratagem Count: {charges}", Color.Aqua));
                        }
                        currentSCHCharges = charges;
                    }
                }
            }
        }

        private bool CheckEngagedStatus()
        {
            if (_ELITEAPIMonitored == null || _ELITEAPIPL == null) { return false; }

            if (OptionsForm.config.GeoWhenEngaged == false)
            {
                return true;
            }
            else if (OptionsForm.config.specifiedEngageTarget == true && !string.IsNullOrEmpty(OptionsForm.config.LuopanSpell_Target))
            {
                for (int x = 0; x < 2048; x++)
                {
                    EliteAPI.XiEntity z = _ELITEAPIPL.Entity.GetEntity(x);
                    if (z.Name != string.Empty && z.Name != null)
                    {
                        if (z.Name.ToLower() == OptionsForm.config.LuopanSpell_Target.ToLower()) // A match was located so use this entity as a check.
                        {
                            if (z.Status == 1)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                return false;
            }
            else
            {
                if (_ELITEAPIMonitored.Player.Status == 1)
                {
                    return true;


                }
                else
                {
                    return false;
                }
            }
        }

        /*private void EclipticTimer_Tick(object sender, EventArgs e)
        {
            if (_ELITEAPIMonitored == null || _ELITEAPIPL == null) { return; }

            if (_ELITEAPIPL.Player.Pet.HealthPercent >= 1)
            {
                EclipticStillUp = true;
            }
            else
            {
                EclipticStillUp = false;
            }
        }*/

        private bool GEO_EnemyCheck()
        {
            if (_ELITEAPIMonitored == null || _ELITEAPIPL == null) { return false; }

            // Grab GEO spell name
            string SpellCheckedResult = ReturnGeoSpell(OptionsForm.config.GeoSpell_Spell, 2);

            if (SpellCheckedResult == "SpellError_Cancel" || SpellCheckedResult == "SpellRecast" || SpellCheckedResult == "SpellUnknown")
            {
                // Do nothing and continue on with the program
                return true;
            }
            else
            {
                if (_ELITEAPIPL.Resources.GetSpell(SpellCheckedResult, 0).ValidTargets == 5)
                {
                    return true; // SPELL TARGET IS PLAYER THEREFORE ONLY THE DEFAULT CHECK IS REQUIRED SO JUST RETURN TRUE TO VOID THIS CHECK
                }
                else
                {
                    if (OptionsForm.config.specifiedEngageTarget == true && !string.IsNullOrEmpty(OptionsForm.config.LuopanSpell_Target))
                    {
                        for (int x = 0; x < 2048; x++)
                        {
                            EliteAPI.XiEntity z = _ELITEAPIPL.Entity.GetEntity(x);
                            if (z.Name != string.Empty && z.Name != null)
                            {
                                if (z.Name.ToLower() == OptionsForm.config.LuopanSpell_Target.ToLower()) // A match was located so use this entity as a check.
                                {
                                    if (z.Status == 1)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        return false;
                    }
                    else
                    {
                        if (_ELITEAPIMonitored.Player.Status == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        private int CheckEngagedStatus_Hate()
        {
            if (OptionsForm.config.AssistSpecifiedTarget == true && OptionsForm.config.autoTarget_Target != String.Empty)
            {
                for (int x = 0; x < 2048; x++)
                {
                    EliteAPI.XiEntity z = _ELITEAPIPL.Entity.GetEntity(x);

                    if (z.Name != null && z.Name.ToLower() == OptionsForm.config.autoTarget_Target.ToLower())
                    {
                        if (z.Status == 1)
                        {
                            return z.TargetingIndex;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                return 0;
            }
            else
            {
                if (_ELITEAPIMonitored.Player.Status == 1)
                {
                    EliteAPI.TargetInfo target = _ELITEAPIMonitored.Target.GetTargetInfo();
                    EliteAPI.XiEntity entity = _ELITEAPIMonitored.Entity.GetEntity(Convert.ToInt32(target.TargetIndex));
                    return Convert.ToInt32(entity.TargetID);

                }
                else
                {
                    return 0;
                }
            }
        }


        private void updateInstances_Tick(object sender, EventArgs e)
        {
            if ((_ELITEAPIPL != null && _ELITEAPIPL.Player.LoginStatus == (int)LoginStatus.Loading) || (_ELITEAPIMonitored != null && _ELITEAPIMonitored.Player.LoginStatus == (int)LoginStatus.Loading))
            {
                return;
            }

            IEnumerable<Process> pol = Process.GetProcessesByName("pol").Union(Process.GetProcessesByName("xiloader")).Union(Process.GetProcessesByName("edenxi"));

            if (pol.Count() < 1)
            {
            }
            else
            {
                POLID.Items.Clear();
                POLID2.Items.Clear();
                processids.Items.Clear();

                int selectedPOLID = 0;
                int selectedPOLID2 = 0;

                for (int i = 0; i < pol.Count(); i++)
                {
                    POLID.Items.Add(pol.ElementAt(i).MainWindowTitle);
                    POLID2.Items.Add(pol.ElementAt(i).MainWindowTitle);
                    processids.Items.Add(pol.ElementAt(i).Id);

                    if (_ELITEAPIPL != null && _ELITEAPIPL.Player.Name != null)
                    {
                        if (pol.ElementAt(i).MainWindowTitle.ToLower() == _ELITEAPIPL.Player.Name.ToLower())
                        {
                            selectedPOLID = i;
                            plLabel.Text = "Selected PL: " + _ELITEAPIPL.Player.Name;
                            Text = notifyIcon1.Text = _ELITEAPIPL.Player.Name + " - " + "Cure Please v" + Application.ProductVersion;
                        }
                    }

                    if (_ELITEAPIMonitored != null && _ELITEAPIMonitored.Player.Name != null)
                    {
                        if (pol.ElementAt(i).MainWindowTitle == _ELITEAPIMonitored.Player.Name)
                        {
                            selectedPOLID2 = i;
                            monitoredLabel.Text = "Monitored Player: " + _ELITEAPIMonitored.Player.Name;
                        }
                    }
                }
                POLID.SelectedIndex = selectedPOLID;
                POLID2.SelectedIndex = selectedPOLID2;
            }
        }

        private void CurePleaseForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                Hide();
            }
            else if (FormWindowState.Normal == WindowState)
            {
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void CheckCustomActions_TickAsync(object sender, EventArgs e)
        {
            if (_ELITEAPIPL != null && _ELITEAPIMonitored != null)
            {

                int cmdTime = _ELITEAPIMonitored.ThirdParty.ConsoleIsNewCommand();

                if (lastCommand != cmdTime)
                {
                    lastCommand = cmdTime;

                    if (_ELITEAPIMonitored.ThirdParty.ConsoleGetArg(0) == "cureplease")
                    {
                        int argCount = _ELITEAPIMonitored.ThirdParty.ConsoleGetArgCount();

                        // 0 = cureplease or cp so ignore
                        // 1 = command to run
                        // 2 = (if set) PL's name

                        if (argCount >= 3)
                        {
                            if ((_ELITEAPIMonitored.ThirdParty.ConsoleGetArg(1) == "stop" || _ELITEAPIMonitored.ThirdParty.ConsoleGetArg(1) == "pause") && _ELITEAPIPL.Player.Name == _ELITEAPIMonitored.ThirdParty.ConsoleGetArg(2))
                            {
                                pauseButton.Text = "Paused!";
                                pauseButton.ForeColor = Color.Red;
                                DisableTickers();
                                ActiveBuffs.Clear();
                                pauseActions = true;
                                song_casting = 0;
                                ForceSongRecast = true;
                                if (OptionsForm.config.FFXIDefaultAutoFollow == false)
                                {
                                    _ELITEAPIPL.AutoFollow.IsAutoFollowing = false;
                                }
                            }
                            else if ((_ELITEAPIMonitored.ThirdParty.ConsoleGetArg(1) == "unpause" || _ELITEAPIMonitored.ThirdParty.ConsoleGetArg(1) == "start") && _ELITEAPIPL.Player.Name.ToLower() == _ELITEAPIMonitored.ThirdParty.ConsoleGetArg(2).ToLower())
                            {
                                pauseButton.Text = "Pause";
                                pauseButton.ForeColor = Color.Black;
                                EnableTickers();
                                pauseActions = false;
                                song_casting = 0;
                                ForceSongRecast = true;
                            }
                            else if ((_ELITEAPIMonitored.ThirdParty.ConsoleGetArg(1) == "toggle") && _ELITEAPIPL.Player.Name.ToLower() == _ELITEAPIMonitored.ThirdParty.ConsoleGetArg(2).ToLower())
                            {
                                pauseButton.PerformClick();
                            }
                            else
                            {

                            }
                        }
                        else if (argCount < 3)
                        {
                            if (_ELITEAPIMonitored.ThirdParty.ConsoleGetArg(1) == "stop" || _ELITEAPIMonitored.ThirdParty.ConsoleGetArg(1) == "pause")
                            {
                                pauseButton.Text = "Paused!";
                                pauseButton.ForeColor = Color.Red;
                                DisableTickers();
                                ActiveBuffs.Clear();
                                pauseActions = true;
                                song_casting = 0;
                                ForceSongRecast = true;
                                if (OptionsForm.config.FFXIDefaultAutoFollow == false)
                                {
                                    _ELITEAPIPL.AutoFollow.IsAutoFollowing = false;
                                }
                            }
                            else if (_ELITEAPIMonitored.ThirdParty.ConsoleGetArg(1) == "unpause" || _ELITEAPIMonitored.ThirdParty.ConsoleGetArg(1) == "start")
                            {
                                pauseButton.Text = "Pause";
                                pauseButton.ForeColor = Color.Black;
                                EnableTickers();
                                pauseActions = false;
                                song_casting = 0;
                                ForceSongRecast = true;
                            }
                            else if (_ELITEAPIMonitored.ThirdParty.ConsoleGetArg(1) == "toggle")
                            {
                                pauseButton.PerformClick();
                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            // DO NOTHING
                        }
                    }
                }
            }
        }

        public void Run_BardSongs()
        {
            PL_BRDCount = _ELITEAPIPL.Player.GetPlayerInfo().Buffs.Where(b => b == 195 || b == 196 || b == 197 || b == 198 || b == 199 || b == 200 || b == 201 || b == 214 || b == 215 || b == 216 || b == 218 || b == 219 || b == 222).Count();



            if ((OptionsForm.config.enableSinging) && _ELITEAPIPL.Player.Status != (uint)Status.Healing)
            {

                debug_MSG_show = "ORDER: " + song_casting;

                SongSpells songs = new SongSpells();

                SongData song_1 = songs.SongInfo.Where(c => c.song_position == OptionsForm.config.song1).FirstOrDefault();
                SongData song_2 = songs.SongInfo.Where(c => c.song_position == OptionsForm.config.song2).FirstOrDefault();
                SongData song_3 = songs.SongInfo.Where(c => c.song_position == OptionsForm.config.song3).FirstOrDefault();
                SongData song_4 = songs.SongInfo.Where(c => c.song_position == OptionsForm.config.song4).FirstOrDefault();

                SongData dummy1_song = songs.SongInfo.Where(c => c.song_position == OptionsForm.config.dummy1).FirstOrDefault();
                SongData dummy2_song = songs.SongInfo.Where(c => c.song_position == OptionsForm.config.dummy2).FirstOrDefault();

                // Check the distance of the Monitored player
                int Monitoreddistance = 50;


                EliteAPI.XiEntity monitoredTarget = _ELITEAPIPL.Entity.GetEntity((int)_ELITEAPIMonitored.Player.TargetID);
                Monitoreddistance = (int)monitoredTarget.Distance;

                int Songs_Possible = 0;

                if (song_1.song_name.ToLower() != "blank")
                {
                    Songs_Possible++;
                }
                if (song_2.song_name.ToLower() != "blank")
                {
                    Songs_Possible++;
                }
                if (dummy1_song != null && dummy1_song.song_name.ToLower() != "blank")
                {
                    Songs_Possible++;
                }
                if (dummy2_song != null && dummy2_song.song_name.ToLower() != "blank")
                {
                    Songs_Possible++;
                }

                // List to make it easy to check how many of each buff is needed.
                List<int> SongDataMax = new List<int> { song_1.buff_id, song_2.buff_id, song_3.buff_id, song_4.buff_id };

                // Check Whether e have the songs Currently Up
                int count1_type = _ELITEAPIPL.Player.GetPlayerInfo().Buffs.Where(b => b == song_1.buff_id).Count();
                int count2_type = _ELITEAPIPL.Player.GetPlayerInfo().Buffs.Where(b => b == song_2.buff_id).Count();
                int count3_type = _ELITEAPIPL.Player.GetPlayerInfo().Buffs.Where(b => b == dummy1_song.buff_id).Count();
                int count4_type = _ELITEAPIPL.Player.GetPlayerInfo().Buffs.Where(b => b == song_3.buff_id).Count();
                int count5_type = _ELITEAPIPL.Player.GetPlayerInfo().Buffs.Where(b => b == dummy2_song.buff_id).Count();
                int count6_type = _ELITEAPIPL.Player.GetPlayerInfo().Buffs.Where(b => b == song_4.buff_id).Count();

                int MON_count1_type = _ELITEAPIMonitored.Player.GetPlayerInfo().Buffs.Where(b => b == song_1.buff_id).Count();
                int MON_count2_type = _ELITEAPIMonitored.Player.GetPlayerInfo().Buffs.Where(b => b == song_2.buff_id).Count();
                int MON_count3_type = _ELITEAPIMonitored.Player.GetPlayerInfo().Buffs.Where(b => b == dummy1_song.buff_id).Count();
                int MON_count4_type = _ELITEAPIMonitored.Player.GetPlayerInfo().Buffs.Where(b => b == song_3.buff_id).Count();
                int MON_count5_type = _ELITEAPIMonitored.Player.GetPlayerInfo().Buffs.Where(b => b == dummy2_song.buff_id).Count();
                int MON_count6_type = _ELITEAPIMonitored.Player.GetPlayerInfo().Buffs.Where(b => b == song_4.buff_id).Count();


                if (ForceSongRecast == true) { song_casting = 0; ForceSongRecast = false; }


                // SONG NUMBER #4
                if (song_casting == 3 && PL_BRDCount >= 3 && song_4.song_name.ToLower() != "blank" && count6_type < SongDataMax.Where(c => c == song_4.buff_id).Count() && Last_Song_Cast != song_4.song_name)
                {
                    if (PL_BRDCount == 3)
                    {
                        if (playerHelper.IsAbleToCastSpell(dummy2_song.song_name))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", dummy2_song.song_name);
                        }
                    }
                    else
                    {
                        if (playerHelper.IsAbleToCastSpell(song_4.song_name))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", song_4.song_name);
                            Last_Song_Cast = song_4.song_name;
                            Last_SongCast_Timer[0] = DateTime.Now;
                            playerSong4[0] = DateTime.Now;
                            song_casting = 0;
                        }
                    }

                }
                else if (song_casting == 3 && song_4.song_name.ToLower() != "blank" && count6_type >= SongDataMax.Where(c => c == song_4.buff_id).Count())
                {
                    song_casting = 0;
                }


                // SONG NUMBER #3
                else if (song_casting == 2 && PL_BRDCount >= 2 && song_3.song_name.ToLower() != "blank" && count4_type < SongDataMax.Where(c => c == song_3.buff_id).Count() && Last_Song_Cast != song_3.song_name)
                {
                    if (PL_BRDCount == 2)
                    {
                        if (playerHelper.IsAbleToCastSpell(dummy1_song.song_name))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", dummy1_song.song_name);
                        }
                    }
                    else
                    {
                        if (playerHelper.IsAbleToCastSpell(song_3.song_name))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", song_3.song_name);
                            Last_Song_Cast = song_3.song_name;
                            Last_SongCast_Timer[0] = DateTime.Now;
                            playerSong3[0] = DateTime.Now;
                            song_casting = 3;
                        }
                    }
                }
                else if (song_casting == 2 && song_3.song_name.ToLower() != "blank" && count4_type >= SongDataMax.Where(c => c == song_3.buff_id).Count())
                {
                    song_casting = 3;
                }


                // SONG NUMBER #2
                else if (song_casting == 1 && song_2.song_name.ToLower() != "blank" && count2_type < SongDataMax.Where(c => c == song_2.buff_id).Count() && Last_Song_Cast != song_4.song_name)
                {
                    if (playerHelper.IsAbleToCastSpell(song_2.song_name))
                    {
                        CastingManager.QueueSpell(SpellType.Buff, "<me>", song_2.song_name);
                        Last_Song_Cast = song_2.song_name;
                        Last_SongCast_Timer[0] = DateTime.Now;
                        playerSong2[0] = DateTime.Now;
                        song_casting = 2;
                    }
                }
                else if (song_casting == 1 && song_2.song_name.ToLower() != "blank" && count2_type >= SongDataMax.Where(c => c == song_2.buff_id).Count())
                {
                    song_casting = 2;
                }

                // SONG NUMBER #1
                else if ((song_casting == 0) && song_1.song_name.ToLower() != "blank" && count1_type < SongDataMax.Where(c => c == song_1.buff_id).Count() && Last_Song_Cast != song_4.song_name)
                {
                    if (playerHelper.IsAbleToCastSpell(song_1.song_name))
                    {
                        CastingManager.QueueSpell(SpellType.Buff, "<me>", song_1.song_name);
                        Last_Song_Cast = song_1.song_name;
                        Last_SongCast_Timer[0] = DateTime.Now;
                        playerSong1[0] = DateTime.Now;
                        song_casting = 1;
                    }

                }
                else if (song_casting == 0 && song_2.song_name.ToLower() != "blank" && count1_type >= SongDataMax.Where(c => c == song_1.buff_id).Count())
                {
                    song_casting = 1;
                }


                // ONCE ALL SONGS HAVE BEEN CAST ONLY RECAST THEM WHEN THEY MEET THE THRESHOLD SET ON SONG RECAST AND BLOCK IF IT'S SET AT LAUNCH DEFAULTS
                if (playerSong1[0] != DefaultTime && playerSong1_Span[0].Minutes >= OptionsForm.config.recastSongTime)
                {
                    if ((OptionsForm.config.SongsOnlyWhenNear && Monitoreddistance < 10) || OptionsForm.config.SongsOnlyWhenNear == false)
                    {
                        if (playerHelper.IsAbleToCastSpell(song_1.song_name))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", song_1.song_name);
                            playerSong1[0] = DateTime.Now;
                            song_casting = 0;
                        }
                    }
                }
                else if (playerSong2[0] != DefaultTime && playerSong2_Span[0].Minutes >= OptionsForm.config.recastSongTime)
                {
                    if ((OptionsForm.config.SongsOnlyWhenNear && Monitoreddistance < 10) || OptionsForm.config.SongsOnlyWhenNear == false)
                    {
                        if (playerHelper.IsAbleToCastSpell(song_2.song_name))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", song_2.song_name);
                            playerSong2[0] = DateTime.Now;
                            song_casting = 0;
                        }
                    }
                }
                else if (playerSong3[0] != DefaultTime && playerSong3_Span[0].Minutes >= OptionsForm.config.recastSongTime)
                {
                    if ((OptionsForm.config.SongsOnlyWhenNear && Monitoreddistance < 10) || OptionsForm.config.SongsOnlyWhenNear == false)
                    {
                        if (playerHelper.IsAbleToCastSpell(song_3.song_name))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", song_3.song_name);
                            playerSong3[0] = DateTime.Now;
                            song_casting = 0;
                        }
                    }
                }
                else if (playerSong4[0] != DefaultTime && playerSong4_Span[0].Minutes >= OptionsForm.config.recastSongTime)
                {
                    if ((OptionsForm.config.SongsOnlyWhenNear && Monitoreddistance < 10) || OptionsForm.config.SongsOnlyWhenNear == false)
                    {
                        if (playerHelper.IsAbleToCastSpell(song_4.song_name))
                        {
                            CastingManager.QueueSpell(SpellType.Buff, "<me>", song_4.song_name);
                            playerSong4[0] = DateTime.Now;
                            song_casting = 0;
                        }
                    }
                }


            }
        }

        private void Follow_BGW_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            // MAKE SURE BOTH ELITEAPI INSTANCES ARE ACTIVE, THE BOT ISN'T PAUSED, AND THERE IS AN AUTOFOLLOWTARGET NAMED
            if (_ELITEAPIPL != null && _ELITEAPIMonitored != null && !string.IsNullOrEmpty(OptionsForm.config.autoFollowName) && !pauseActions)
            {

                if (OptionsForm.config.FFXIDefaultAutoFollow != true)
                {
                    // CANCEL ALL PREVIOUS FOLLOW ACTIONS
                    _ELITEAPIPL.AutoFollow.IsAutoFollowing = false;
                    curePlease_autofollow = false;
                    stuckWarning = false;
                    stuckCount = 0;
                }

                // RUN THE FUNCTION TO GRAB THE ID OF THE FOLLOW TARGET THIS ALSO MAKES SURE THEY ARE IN RANGE TO FOLLOW
                int followersTargetID = followID();

                // If the FOLLOWER'S ID is NOT -1 THEN THEY WERE LOCATED SO CONTINUE THE CHECKS
                if (followersTargetID != -1)
                {
                    // GRAB THE FOLLOW TARGETS ENTITY TABLE TO CHECK DISTANCE ETC
                    EliteAPI.XiEntity followTarget = _ELITEAPIPL.Entity.GetEntity(followersTargetID);

                    if (Math.Truncate(followTarget.Distance) >= (int)OptionsForm.config.autoFollowDistance && curePlease_autofollow == false)
                    {
                        // THE DISTANCE IS GREATER THAN REQUIRED SO IF AUTOFOLLOW IS NOT ACTIVE THEN DEPENDING ON THE TYPE, FOLLOW

                        // SQUARE ENIX FINAL FANTASY XI DEFAULT AUTO FOLLOW
                        if (OptionsForm.config.FFXIDefaultAutoFollow == true && _ELITEAPIPL.AutoFollow.IsAutoFollowing != true)
                        {
                            // IF THE CURRENT TARGET IS NOT THE FOLLOWERS TARGET ID THEN CHANGE THAT NOW
                            if (_ELITEAPIPL.Target.GetTargetInfo().TargetIndex != followersTargetID)
                            {
                                // FIRST REMOVE THE CURRENT TARGET
                                _ELITEAPIPL.Target.SetTarget(0);
                                // NOW SET THE NEXT TARGET AFTER A WAIT
                                Thread.Sleep(TimeSpan.FromSeconds(0.1));
                                _ELITEAPIPL.Target.SetTarget(followersTargetID);
                            }
                            // IF THE TARGET IS CORRECT BUT YOU'RE NOT LOCKED ON THEN DO SO NOW
                            else if (_ELITEAPIPL.Target.GetTargetInfo().TargetIndex == followersTargetID && !_ELITEAPIPL.Target.GetTargetInfo().LockedOn)
                            {
                                _ELITEAPIPL.ThirdParty.SendString("/lockon <t>");
                            }
                            // EVERYTHING SHOULD BE FINE SO FOLLOW THEM
                            else
                            {
                                Thread.Sleep(TimeSpan.FromSeconds(0.1));
                                _ELITEAPIPL.ThirdParty.SendString("/follow");
                            }
                        }
                        // ELITEAPI'S IMPROVED AUTO FOLLOW
                        else if (OptionsForm.config.FFXIDefaultAutoFollow != true && _ELITEAPIPL.AutoFollow.IsAutoFollowing != true)
                        {
                            // IF YOU ARE TOO FAR TO FOLLOW THEN STOP AND IF ENABLED WARN THE MONITORED PLAYER
                            if (OptionsForm.config.autoFollow_Warning == true && Math.Truncate(followTarget.Distance) >= 40 && _ELITEAPIMonitored.Player.Name != _ELITEAPIPL.Player.Name && followWarning == 0)
                            {
                                string createdTell = "/tell " + _ELITEAPIMonitored.Player.Name + " " + "You're too far to follow.";
                                _ELITEAPIPL.ThirdParty.SendString(createdTell);
                                followWarning = 1;
                                Thread.Sleep(TimeSpan.FromSeconds(0.3));
                            }
                            else if (Math.Truncate(followTarget.Distance) <= 40)
                            {
                                // ONLY TARGET AND BEGIN FOLLOW IF TARGET IS AT THE DEFINED DISTANCE
                                if (Math.Truncate(followTarget.Distance) >= (int)OptionsForm.config.autoFollowDistance && Math.Truncate(followTarget.Distance) <= 48)
                                {
                                    followWarning = 0;

                                    // Cancel current target this is to make sure the character is not locked
                                    // on and therefore unable to move freely. Wait 5ms just to allow it to work

                                    _ELITEAPIPL.Target.SetTarget(0);
                                    Thread.Sleep(TimeSpan.FromSeconds(0.1));

                                    float Target_X;
                                    float Target_Y;
                                    float Target_Z;

                                    EliteAPI.XiEntity FollowerTargetEntity = _ELITEAPIPL.Entity.GetEntity(followersTargetID);

                                    if (!string.IsNullOrEmpty(FollowerTargetEntity.Name))
                                    {
                                        while (Math.Truncate(followTarget.Distance) >= (int)OptionsForm.config.autoFollowDistance)
                                        {

                                            float Player_X = _ELITEAPIPL.Player.X;
                                            float Player_Y = _ELITEAPIPL.Player.Y;
                                            float Player_Z = _ELITEAPIPL.Player.Z;


                                            if (FollowerTargetEntity.Name == _ELITEAPIMonitored.Player.Name)
                                            {
                                                Target_X = _ELITEAPIMonitored.Player.X;
                                                Target_Y = _ELITEAPIMonitored.Player.Y;
                                                Target_Z = _ELITEAPIMonitored.Player.Z;
                                                float dX = Target_X - Player_X;
                                                float dY = Target_Y - Player_Y;
                                                float dZ = Target_Z - Player_Z;

                                                _ELITEAPIPL.AutoFollow.SetAutoFollowCoords(dX, dY, dZ);

                                                _ELITEAPIPL.AutoFollow.IsAutoFollowing = true;
                                                curePlease_autofollow = true;


                                                lastX = _ELITEAPIPL.Player.X;
                                                lastY = _ELITEAPIPL.Player.Y;
                                                lastZ = _ELITEAPIPL.Player.Z;

                                                Thread.Sleep(TimeSpan.FromSeconds(0.1));
                                            }
                                            else
                                            {
                                                Target_X = FollowerTargetEntity.X;
                                                Target_Y = FollowerTargetEntity.Y;
                                                Target_Z = FollowerTargetEntity.Z;

                                                float dX = Target_X - Player_X;
                                                float dY = Target_Y - Player_Y;
                                                float dZ = Target_Z - Player_Z;


                                                _ELITEAPIPL.AutoFollow.SetAutoFollowCoords(dX, dY, dZ);

                                                _ELITEAPIPL.AutoFollow.IsAutoFollowing = true;
                                                curePlease_autofollow = true;


                                                lastX = _ELITEAPIPL.Player.X;
                                                lastY = _ELITEAPIPL.Player.Y;
                                                lastZ = _ELITEAPIPL.Player.Z;

                                                Thread.Sleep(TimeSpan.FromSeconds(0.1));
                                            }

                                            // STUCK CHECKER
                                            float genX = lastX - _ELITEAPIPL.Player.X;
                                            float genY = lastY - _ELITEAPIPL.Player.Y;
                                            float genZ = lastZ - _ELITEAPIPL.Player.Z;

                                            double distance = Math.Sqrt(genX * genX + genY * genY + genZ * genZ);

                                            if (distance < .1)
                                            {
                                                stuckCount = stuckCount + 1;
                                                if (OptionsForm.config.autoFollow_Warning == true && stuckWarning != true && FollowerTargetEntity.Name == _ELITEAPIMonitored.Player.Name && stuckCount == 10)
                                                {
                                                    string createdTell = "/tell " + _ELITEAPIMonitored.Player.Name + " " + "I appear to be stuck.";
                                                    _ELITEAPIPL.ThirdParty.SendString(createdTell);
                                                    stuckWarning = true;
                                                }
                                            }
                                        }

                                        _ELITEAPIPL.AutoFollow.IsAutoFollowing = false;
                                        curePlease_autofollow = false;
                                        stuckWarning = false;
                                        stuckCount = 0;
                                    }
                                }
                            }
                            else
                            {
                                // YOU ARE NOT AT NOR FURTHER THAN THE DISTANCE REQUIRED SO CANCEL ELITEAPI AUTOFOLLOW
                                curePlease_autofollow = false;
                            }
                        }
                    }
                }
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));

        }

        private void Follow_BGW_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Follow_BGW.RunWorkerAsync();
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Opacity = trackBar1.Value * 0.01;
        }

        private Form settings;

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            if ((settings == null) || (settings.IsDisposed))
            {
                settings = new OptionsForm();
            }
            settings.Show();

        }

        private void ChatLogButton_Click(object sender, EventArgs e)
        {
            ChatLogForm form4 = new ChatLogForm(this);

            if (_ELITEAPIPL != null)
            {
                form4.Show();
            }
        }

        private void PartyBuffsButton_Click(object sender, EventArgs e)
        {
            PartyBuffs PartyBuffs = new PartyBuffs(this);
            if (_ELITEAPIPL != null)
            {
                PartyBuffs.Show();
            }
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            new AboutForm().Show();
        }

        private Int32 GetFreePort()
        {
            var startingAtPort = Convert.ToInt32(OptionsForm.config.listeningPort);
            var maxNumberOfPortsToCheck = 500;
            var range = Enumerable.Range(startingAtPort, maxNumberOfPortsToCheck);
            var portsInUse =
                from p in range
                join used in System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners()
                on p equals used.Port
                select p;

            return range.Except(portsInUse).FirstOrDefault();
        }

        private void AddonReader_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (OptionsForm.config.EnableAddOn == true && pauseActions == false && _ELITEAPIMonitored != null && _ELITEAPIPL != null)
            {

                bool done = false;

                var freePort = GetFreePort();

                UdpClient listener = new UdpClient(freePort);
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(OptionsForm.config.ipAddress), freePort);

                OptionsForm.config.listeningPort = freePort.ToString();

                _ELITEAPIPL.ThirdParty.SendString("//cpaddon settings " + OptionsForm.config.ipAddress + " " + freePort);
                Thread.Sleep(100);
                _ELITEAPIPL.ThirdParty.SendString("//cpaddon verify");

                string received_data;
                byte[] receive_byte_array;
                try
                {
                    while (!done)
                    {

                        receive_byte_array = listener.Receive(ref groupEP);

                        received_data = Encoding.ASCII.GetString(receive_byte_array, 0, receive_byte_array.Length);
                        

                        string[] commands = received_data.Split('_');

                        // MessageBox.Show(commands[1] + " " + commands[2]);
                        if (commands[1] == "casting" && commands.Count() == 4 && OptionsForm.config.trackCastingPackets == true)
                        {
                            if (commands[2] == "blocked")
                            {
                                Invoke((MethodInvoker)(() =>
                                {
                                    // most likely cause for deadlock, when this is activated but never unlocked (happens when lag is involved)
                                    // when spells are cast from cureplease this is useless since we should already have lock 
                                    // if a spell is cast manually the worst that can happen is cureplease atempting to cast when its already casting
                                    // since this is the lesser evil i comment this in order to have less deadlocks
                                    //CastingManager.GetLock(); 
                                    castingLockLabel.Text = "PACKET: Casting is LOCKED";
                                }));

                                //if (!ProtectCasting.IsBusy) { ProtectCasting.RunWorkerAsync(); }
                            }
                            else if (commands[2] == "interrupted")
                            {
                                Invoke((MethodInvoker)(async () =>
                                  {
                                      //ProtectCasting.CancelAsync();
                                      castingLockLabel.Text = "PACKET: Casting is INTERRUPTED";
                                      await Task.Delay(TimeSpan.FromSeconds(3));
                                      castingLockLabel.Text = "Casting is UNLOCKED";
                                      if (long.TryParse(commands[3], out long lockstamp))
                                      {
                                          CastingManager.FreeLock("ADDON: Interrupt", lockstamp);
                                      }
                                  }));
                            }
                            else if (commands[2] == "finished")
                            {

                                Invoke((MethodInvoker)(async () =>
                                  {
                                      //ProtectCasting.CancelAsync();
                                      castingLockLabel.Text = "PACKET: Casting is soon to be AVAILABLE!";
                                      await Task.Delay(TimeSpan.FromSeconds(3));
                                      castingLockLabel.Text = "Casting is UNLOCKED";
                                      currentAction.Text = string.Empty;
                                      castingSpell = string.Empty;
                                      if (long.TryParse(commands[3], out long lockstamp))
                                      {
                                          CastingManager.FreeLock("ADDON: Finished", lockstamp);
                                      }
                                  }));
                            }
                        }
                        else if (commands[1] == "confirmed")
                        {
                            AddOnStatus.BackColor = Color.ForestGreen;
                        }
                        else if (commands[1] == "command")
                        {
                            // MessageBox.Show(commands[2]);
                            if (commands[2] == "start" || commands[2] == "unpause")
                            {
                                Invoke((MethodInvoker)(() =>
                                {
                                    pauseButton.Text = "Pause";
                                    pauseButton.ForeColor = Color.Black;
                                    EnableTickers();
                                    pauseActions = false;
                                    song_casting = 0;
                                    ForceSongRecast = true;
                                }));
                            }
                            if (commands[2] == "stop" || commands[2] == "pause")
                            {
                                Invoke((MethodInvoker)(() =>
                                {

                                    pauseButton.Text = "Paused!";
                                    pauseButton.ForeColor = Color.Red;
                                    DisableTickers();
                                    ActiveBuffs.Clear();
                                    pauseActions = true;
                                    if (OptionsForm.config.FFXIDefaultAutoFollow == false)    
                                    {
                                        _ELITEAPIPL.AutoFollow.IsAutoFollowing = false;
                                    }

                                }));
                            }
                            if (commands[2] == "toggle")
                            {
                                Invoke((MethodInvoker)(() =>
                                {
                                    pauseButton.PerformClick();
                                }));
                            }
                        }
                        else if (commands[1] == "buffs" && commands.Count() == 4)
                        {
                            ActiveBuffs.RemoveAll(buf => buf.CharacterName == commands[2]);

                            ActiveBuffs.Add(new BuffStorage
                            {
                                CharacterName = commands[2],
                                CharacterBuffs = commands[3]
                            });
                        }
                        else if (commands[1] == "spell" && commands.Count() == 4)
                        {
                            EliteAPI.ISpell magic = _ELITEAPIPL.Resources.GetSpell(commands[2].Trim(), 0);
                            if(magic != null)
                            {
                                var spell = commands[2];
                                var target = commands[3];
                                if(spell.ToLower().Contains("raise") || spell.ToLower().Contains("arise"))
                                {
                                    CastingManager.QueueSpell(SpellType.Raise, target, spell);
                                }
                                else
                                {
                                    CastingManager.QueueSpell(SpellType.Prio, target, spell);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Spell not found: " + commands[2]);
                            }
                            
                        }
                    }
                }
                catch (Exception error1)
                {
                    Console.WriteLine(error1.ToString());
                }

                listener.Close();

            }

            Thread.Sleep(TimeSpan.FromSeconds(0.3));
        }

        private void AddonReader_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            AddonReader.RunWorkerAsync();
        }


        private void FullCircle_Timer_Tick(object sender, EventArgs e)
        {

            if (_ELITEAPIPL.Player.Pet.HealthPercent >= 1)
            {
                ushort PetsIndex = _ELITEAPIPL.Player.PetIndex;

                if (OptionsForm.config.Fullcircle_GEOTarget == true && OptionsForm.config.LuopanSpell_Target != "")
                {
                    EliteAPI.XiEntity PetsEntity = _ELITEAPIPL.Entity.GetEntity(PetsIndex);

                    int FullCircle_CharID = 0;

                    for (int x = 0; x < 2048; x++)
                    {
                        EliteAPI.XiEntity entity = _ELITEAPIPL.Entity.GetEntity(x);

                        if (entity.Name != null && entity.Name.ToLower().Equals(OptionsForm.config.LuopanSpell_Target.ToLower()))
                        {
                            FullCircle_CharID = Convert.ToInt32(entity.TargetID);
                            break;
                        }
                    }

                    if (FullCircle_CharID != 0)
                    {
                        EliteAPI.XiEntity FullCircleEntity = _ELITEAPIPL.Entity.GetEntity(FullCircle_CharID);

                        float fX = PetsEntity.X - FullCircleEntity.X;
                        float fY = PetsEntity.Y - FullCircleEntity.Y;
                        float fZ = PetsEntity.Z - FullCircleEntity.Z;

                        float generatedDistance = (float)Math.Sqrt((fX * fX) + (fY * fY) + (fZ * fZ));

                        if (generatedDistance >= 10)
                        {
                            JaManager.JobAbility_Wait("Full Circle", "Full Circle");
                            return;
                        }
                    }

                }
                else if (OptionsForm.config.Fullcircle_GEOTarget == false && _ELITEAPIMonitored.Player.Status == 1)
                {


                    string SpellCheckedResult = ReturnGeoSpell(OptionsForm.config.GeoSpell_Spell, 2);



                    if (OptionsForm.config.Fullcircle_DisableEnemy != true || (OptionsForm.config.Fullcircle_DisableEnemy == true && _ELITEAPIPL.Resources.GetSpell(SpellCheckedResult, 0).ValidTargets == 32))
                    {
                        EliteAPI.XiEntity PetsEntity = _ELITEAPIMonitored.Entity.GetEntity(PetsIndex);

                        if (PetsEntity.Distance >= 10 && PetsEntity.Distance != 0 && JaManager.IsJaReady("Full Circle"))
                        {
                            JaManager.JobAbility_Wait("Full Circle", "Full Circle");
                            return;
                        }
                    }
                }
            }

            FullCircle_Timer.Enabled = false;
        }

        private void AddOnStatus_Click(object sender, EventArgs e)
        {
            if (_ELITEAPIMonitored != null && _ELITEAPIPL != null)
            {
                if (WindowerMode == "Ashita")
                {
                    _ELITEAPIPL.ThirdParty.SendString(string.Format("/cpaddon verify"));
                }
                else if (WindowerMode == "Windower")
                {
                    _ELITEAPIPL.ThirdParty.SendString(string.Format("//cpaddon verify"));
                }
            }
        }

        /*private void CastingCheck_BackgroundTask_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DateTime lockStamp = (DateTime)e.Argument;

            if (_ELITEAPIMonitored != null && _ELITEAPIPL != null)
            {
                if (_ELITEAPIPL.Player.LoginStatus == (int)LoginStatus.Loading || _ELITEAPIMonitored.Player.LoginStatus == (int)LoginStatus.Loading)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                var count = 0;
                float lastPercent = 0;
                var castPercent = _ELITEAPIPL.CastBar.Percent;
                while (castPercent < 1 && !CastingManager.CanCast())
                {
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                    castPercent = _ELITEAPIPL.CastBar.Percent;
                    if (lastPercent != castPercent)
                    {
                        count = 0;
                        lastPercent = castPercent;
                    }
                    else if (count == 10)
                    {
                        break;
                    }
                    else
                    {
                        count++;
                        lastPercent = castPercent;
                    }
                }
                Thread.Sleep(TimeSpan.FromSeconds(2));
                CastingManager.FreeLock("CastingCheck_BackgroundTask_DoWork", lockStamp);
            }
            else {
                CastingManager.FreeLock("CastingCheck_BackgroundTask_DoWork", lockStamp);
                return; 
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(500));
        }*/


        private void JobAbility_Delay_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Invoke((MethodInvoker)(() =>
          {
              var time = CastingManager.GetLock();
              castingLockLabel.Text = "Casting is LOCKED for a JA.";
              currentAction.Text = "Using a Job Ability: " + JobAbilityCMD;
              Thread.Sleep(TimeSpan.FromSeconds(2));
              castingLockLabel.Text = "Casting is UNLOCKED";
              currentAction.Text = string.Empty;
              castingSpell = string.Empty;
              CastingManager.FreeLock("JobAbility_Delay_DoWork", time);
              JobAbilityCMD = String.Empty;
          }));
        }


        private void CustomCommand_Tracker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

        }

        private void CustomCommand_Tracker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            CustomCommand_Tracker.RunWorkerAsync();
        }

        private void RaiseButtonClick(byte pos)
        {
            string generated_name = _ELITEAPIMonitored.Party.GetPartyMembers()[pos].Name;
            
            currentAction.Text = "Queued -> Raise on -> " + generated_name;
            CastingManager.QueueSpell(SpellType.Raise, generated_name, "Arise");
        }

        private void player0raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(0);
        }

        private void player1raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(1);
        }

        private void player2raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(2);
        }

        private void player3raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(3);
        }

        private void player4raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(4);
        }

        private void player5raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(5);
        }

        private void player6raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(6);
        }

        private void player7raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(7);
        }

        private void player8raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(8);
        }

        private void player9raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(9);
        }

        private void player10raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(10);

        }

        private void player11raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(11);
        }

        private void player12raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(12);
        }

        private void player13raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(13);
        }

        private void player14raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(14);
        }

        private void player15raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(15);
        }

        private void player16raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(16);
        }

        private void player17raiseButton_Click(object sender, EventArgs e)
        {
            RaiseButtonClick(17);
        }

        private void DebugButton_Click(object sender, EventArgs e)
        {
           
        }
    }

    // END OF THE FORM SCRIPT

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
