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

namespace CurePlease
{
    internal partial class CastingMonitorForm : Form
    {
        private CastingHelper _Monitor;
        public CastingMonitorForm(CastingHelper mon)
        {
            InitializeComponent();
            _Monitor = mon;
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
            }
        }

        private void chatlogscan_timer_Tick(object sender, EventArgs e)
        {
            if(_Monitor == null) {  return; }

            foreach (LogEntry log in _Monitor._Log.ToList())
            {
                AppendLog(log.Invoked, log.Text, log.Color);
                _Monitor._Log.Remove(log);
            }
            prioqueue_box.Clear();
            healingqueue_box.Clear();
            debuffqueue_box.Clear();
            buffqueue_box.Clear();
            foreach (CastingAction action in _Monitor._Priority.ToList())
            {
                prioqueue_box.AppendText(action.ToString());
                prioqueue_box.AppendText(Environment.NewLine);
            }
            foreach (CastingAction action in _Monitor._Cures.ToList())
            {
                healingqueue_box.AppendText(action.ToString());
                healingqueue_box.AppendText(Environment.NewLine);
            }
            foreach (CastingAction action in _Monitor._Debuffs.ToList())
            {
                debuffqueue_box.AppendText(action.ToString());
                debuffqueue_box.AppendText(Environment.NewLine);
            }
            foreach (CastingAction action in _Monitor._Buffs.ToList())
            {
                buffqueue_box.AppendText(action.ToString());
                buffqueue_box.AppendText(Environment.NewLine);
            }
        }
    }
}
