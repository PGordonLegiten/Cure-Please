using CurePlease.DataStructures;
using CurePlease.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CurePlease
{
    internal partial class CastingMonitorForm : Form
    {
        private CastingHelper _Monitor;
        private CurePleaseForm _Form;
        public class BuffList
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }

        public List<BuffList> XMLBuffList = new List<BuffList>();
        public CastingMonitorForm(CurePleaseForm form, CastingHelper mon)
        {
            InitializeComponent();
            _Monitor = mon;
            _Form = form;

            // Read the Buffs file a generate a List to call.
            foreach (XElement BuffElement in XElement.Load("Resources/Buffs.xml").Elements("o"))
            {
                XMLBuffList.Add(new BuffList() { ID = BuffElement.Attribute("id").Value, Name = BuffElement.Attribute("en").Value });
            }
        }

        delegate void AppendLogCallback(DateTime stamp, string text, Color color);
        public void AppendLog(DateTime stamp, string text, Color color)
        {

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.castlog_box.InvokeRequired)
            {
                AppendLogCallback d = new AppendLogCallback(AppendLog);
                this.Invoke(d, new object[] { stamp, text, color });
            }
            else
            {
                castlog_box.AppendText("[" + DateTime.Now.ToString("mm:ss:fff") + "] " + text, color);
                castlog_box.AppendText(Environment.NewLine);
                castlog_box.SelectionStart = castlog_box.Text.Length;
                castlog_box.ScrollToCaret();
            }
        }

        private void chatlogscan_timer_Tick(object sender, EventArgs e)
        {
            if(healingqueue_box == null) {  return; }
            foreach (LogEntry log in _Monitor.GetLog().ToList())
            {
                AppendLog(log.Invoked, log.Text, log.Color);
                _Monitor.GetLog().Remove(log);
            }
            healingqueue_box.Clear();
            foreach (CastingAction action in _Monitor.GetQueue().ToList().OrderByDescending(x => x.Priority))
            {
                healingqueue_box.AppendText(action.ToString());
                healingqueue_box.AppendText(Environment.NewLine);
            }
            ShowPartyBuffs();
        }

        private void ShowPartyBuffs()
        {
            debufflist_box.Text = "";

            // Search through current active party buffs
            foreach (BuffStorage ailment in _Form.GetActiveBuffs().ToList())
            {
                // First add Character name and a Line Break.
                debufflist_box.AppendText(ailment.CharacterName + ": ");
                // Now create a list and loop through each buff and name them
                List<string> named_buffs = ailment.CharacterBuffs.Split(',').ToList();
                int i = 1;
                int count = named_buffs.Count();
                foreach (string acBuff in named_buffs)
                {
                    i++;
                    var found_Buff = XMLBuffList.Find(r => r.ID == acBuff);
                    if (found_Buff != null)
                    {
                        if (i == count)
                        {
                            debufflist_box.AppendText(found_Buff.Name + " (" + acBuff + ") ");
                        }
                        else
                        {
                            debufflist_box.AppendText(found_Buff.Name + " (" + acBuff + "), ");
                        }
                    }
                }
                debufflist_box.AppendText(Environment.NewLine);
            }
        }
    }
}
