namespace CurePlease
{
    partial class CastingMonitorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.castlog_box = new System.Windows.Forms.RichTextBox();
            this.chatlogscan_timer = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.charselect = new System.Windows.Forms.GroupBox();
            this.prioqueue_box = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.healingqueue_box = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.debuffqueue_box = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buffqueue_box = new System.Windows.Forms.RichTextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.debufflist_box = new System.Windows.Forms.RichTextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // castlog_box
            // 
            this.castlog_box.BackColor = System.Drawing.SystemColors.Control;
            this.castlog_box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.castlog_box.Location = new System.Drawing.Point(45, 32);
            this.castlog_box.Name = "castlog_box";
            this.castlog_box.Size = new System.Drawing.Size(408, 424);
            this.castlog_box.TabIndex = 0;
            this.castlog_box.Text = "";
            // 
            // chatlogscan_timer
            // 
            this.chatlogscan_timer.Enabled = true;
            this.chatlogscan_timer.Tick += new System.EventHandler(this.chatlogscan_timer_Tick);
            // 
            // charselect
            // 
            this.charselect.BackColor = System.Drawing.Color.Transparent;
            this.charselect.ForeColor = System.Drawing.SystemColors.GrayText;
            this.charselect.Location = new System.Drawing.Point(26, 12);
            this.charselect.Name = "charselect";
            this.charselect.Size = new System.Drawing.Size(443, 464);
            this.charselect.TabIndex = 17;
            this.charselect.TabStop = false;
            this.charselect.Text = "Log";
            // 
            // prioqueue_box
            // 
            this.prioqueue_box.BackColor = System.Drawing.SystemColors.Control;
            this.prioqueue_box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.prioqueue_box.Location = new System.Drawing.Point(498, 32);
            this.prioqueue_box.Name = "prioqueue_box";
            this.prioqueue_box.Size = new System.Drawing.Size(219, 251);
            this.prioqueue_box.TabIndex = 18;
            this.prioqueue_box.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox1.Location = new System.Drawing.Point(479, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(254, 288);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Prio Queue";
            // 
            // healingqueue_box
            // 
            this.healingqueue_box.BackColor = System.Drawing.SystemColors.Control;
            this.healingqueue_box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.healingqueue_box.Location = new System.Drawing.Point(762, 32);
            this.healingqueue_box.Name = "healingqueue_box";
            this.healingqueue_box.Size = new System.Drawing.Size(219, 251);
            this.healingqueue_box.TabIndex = 20;
            this.healingqueue_box.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox2.Location = new System.Drawing.Point(743, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(254, 288);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Healing Queue";
            // 
            // debuffqueue_box
            // 
            this.debuffqueue_box.BackColor = System.Drawing.SystemColors.Control;
            this.debuffqueue_box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.debuffqueue_box.Location = new System.Drawing.Point(1026, 32);
            this.debuffqueue_box.Name = "debuffqueue_box";
            this.debuffqueue_box.Size = new System.Drawing.Size(219, 251);
            this.debuffqueue_box.TabIndex = 22;
            this.debuffqueue_box.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox3.Location = new System.Drawing.Point(1007, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(254, 288);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Debuffs Queue";
            // 
            // buffqueue_box
            // 
            this.buffqueue_box.BackColor = System.Drawing.SystemColors.Control;
            this.buffqueue_box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.buffqueue_box.Location = new System.Drawing.Point(1291, 32);
            this.buffqueue_box.Name = "buffqueue_box";
            this.buffqueue_box.Size = new System.Drawing.Size(219, 251);
            this.buffqueue_box.TabIndex = 24;
            this.buffqueue_box.Text = "";
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox4.Location = new System.Drawing.Point(1272, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(254, 288);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Buffs Queue";
            // 
            // debufflist_box
            // 
            this.debufflist_box.BackColor = System.Drawing.SystemColors.Control;
            this.debufflist_box.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.debufflist_box.Location = new System.Drawing.Point(498, 325);
            this.debufflist_box.Name = "debufflist_box";
            this.debufflist_box.Size = new System.Drawing.Size(1012, 131);
            this.debufflist_box.TabIndex = 26;
            this.debufflist_box.Text = "";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.groupBox5.Location = new System.Drawing.Point(479, 305);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1047, 171);
            this.groupBox5.TabIndex = 27;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Active Debuffs";
            // 
            // CastingMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 500);
            this.Controls.Add(this.debufflist_box);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.buffqueue_box);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.debuffqueue_box);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.healingqueue_box);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.prioqueue_box);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.castlog_box);
            this.Controls.Add(this.charselect);
            this.Name = "CastingMonitorForm";
            this.Text = "Casting Monitor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox castlog_box;
        private System.Windows.Forms.Timer chatlogscan_timer;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox charselect;
        private System.Windows.Forms.RichTextBox prioqueue_box;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox healingqueue_box;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox debuffqueue_box;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox buffqueue_box;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RichTextBox debufflist_box;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}