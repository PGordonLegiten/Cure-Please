using CurePlease.DataStructures;
using EliteMMO.API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    public enum SpellType : byte
    {
        Prio = 0,
        Healing = 1,
        Buff = 2,
        Debuff = 3,
    }
    internal class CastingHelper
    {
        public LinkedList<CastingAction> _Cures = new LinkedList<CastingAction>();
        public LinkedList<CastingAction> _Buffs = new LinkedList<CastingAction>();
        public LinkedList<CastingAction> _Debuffs = new LinkedList<CastingAction>();
        public LinkedList<CastingAction> _Priority = new LinkedList<CastingAction>();

        public List<LogEntry> _Log = new List<LogEntry>();

        private EliteAPI _ELITEAPIPL;
        private EliteAPI _ELITEAPIMonitored;
        private CurePleaseForm _Form;


        private bool CastingBackground_Check = false;
        public DateTime CastingBackground_Check_timer = DateTime.Now;

        public CastingHelper(CurePleaseForm form, EliteAPI pl, EliteAPI monitor)
        {
            _Form = form;
            _ELITEAPIMonitored = monitor;
            _ELITEAPIPL = pl;
        }

        public void Run()
        {
            FailSafe();
            //PRIO
            foreach (CastingAction action in _Priority.ToList())
            {
                if (DeQueueSpell(_Priority, action))
                {
                    return;                
                }
               
            }
            //CURES
            foreach (CastingAction action in _Cures.ToList())
            {
                if (DeQueueSpell(_Cures, action))
                {
                    return;
                }
            }
            //DEBUFFS
            foreach (CastingAction action in _Debuffs.ToList().Where(x => x.Target == "<me>"))
            {
                if (DeQueueSpell(_Debuffs, action))
                {
                    return;
                }
            }
            foreach (CastingAction action in _Debuffs.ToList())
            {
                if (DeQueueSpell(_Debuffs, action))
                {
                    return;
                }
            }

            //BUFFS
            foreach (CastingAction action in _Buffs.ToList().Where(x => x.Target == "<me>"))
            {
                if (DeQueueSpell(_Buffs, action))
                {
                    return;
                }
            }
            foreach (CastingAction action in _Buffs.ToList())
            {
                if (DeQueueSpell(_Buffs, action))
                {
                    return;
                }
            }
        }

        public bool DeQueueSpell(LinkedList<CastingAction> list, CastingAction action)
        {
            if (!HasApi()) { return false; } //this should not be happening
            if (!SpellRecastReady(action.SpellName)) { return false; } //try next spell
            if (CanCast())
            {
                var lockStamp = GetLock();
                list.Remove(action);
                var currentTime = DateTime.Now;
                if (currentTime.Subtract(CastingBackground_Check_timer) <= TimeSpan.FromSeconds(2)) //THIS VALUE NEEDS TO BE TESTED (MAYBE TOO SHORT?)
                {
                    CastSpell(action.Target, action.SpellName, lockStamp, action.DisplayText);
                }
                else
                {
                    FreeLock("Queue Timeout", lockStamp);
                }
            }
            return true;
        }

        public void QueueSpell(SpellType type, string partyMemberName, string spellName, [Optional] string OptionalExtras)
        {
            //doesnt do anything yet, prepraration for a queueing system
            CastingAction action = new CastingAction(spellName, partyMemberName, OptionalExtras);
            switch (type)
            {
                case SpellType.Prio:
                    AddToQueue(_Priority, action);
                    break;
                case SpellType.Healing:
                    AddToQueue(_Cures, action);
                    break;
                case SpellType.Debuff:
                    AddToQueue(_Debuffs, action);
                    break;
                case SpellType.Buff:
                    AddToQueue(_Buffs, action);
                    break;
            }
            //CastSpell(partyMemberName, spellName, OptionalExtras);
        }

        private void AddToQueue(LinkedList<CastingAction> list, CastingAction action)
        {
            if (list.Contains(action))
            {
                list.Remove(action);
            };
            list.AddFirst(action);
        }

        public void CastSpell(string partyMemberName, string spellName, DateTime lockStamp, [Optional] string OptionalExtras)
        {
            if (!HasApi()) { return; }

            _Log.Add(new LogEntry("Attempting to cast ["+ spellName + "] on ["+partyMemberName + "]", Color.LightGray));

            //if (CanCast())
            //{
                //GetLock();

                InternalHelper.setCooldown(spellName, partyMemberName);

                EliteAPI.ISpell magic = _ELITEAPIPL.Resources.GetSpell(spellName.Trim(), 0);

                var castingSpell = magic.Name[0];

                var cast = "/ma \"" + castingSpell + "\" " + partyMemberName;
                _ELITEAPIPL.ThirdParty.SendString(cast);
                _Log.Add(new LogEntry(cast, Color.Black));

                if (OptionalExtras != null)
                {
                    _Form.currentAction.Text = "Casting: " + castingSpell + " [" + OptionalExtras + "]";
                }
                else
                {
                    _Form.currentAction.Text = "Casting: " + castingSpell;
                }

                if (OptionsForm.config.trackCastingPackets == true && OptionsForm.config.EnableAddOn == true)
                {
                    //This is more or less here as failsafe if we dont get a package from the addon back that the cast is done (due to lag for example)
                    //We assume the cast is done after 50 percent of the castbar
                    if (!_Form.ProtectCasting.IsBusy) { _Form.ProtectCasting.RunWorkerAsync(argument: lockStamp); }
                }
                else
                {
                    _Form.castingLockLabel.Text = "Casting is LOCKED";
                    if (!_Form.ProtectCasting.IsBusy) { _Form.ProtectCasting.RunWorkerAsync(argument: lockStamp); }
                }
           // }
        }
        public bool SpellRecastReady(string checked_recastspellName)
        {
            checked_recastspellName = checked_recastspellName.Trim().ToLower();

            if (checked_recastspellName == "honor march")
            {
                return true;
            }

            if (checked_recastspellName != "blank")
            {
                EliteAPI.ISpell magic = _ELITEAPIPL.Resources.GetSpell(checked_recastspellName, 0);

                if (magic == null)
                {
                    throw new Exception("Error detected, please Report Error: #SpellRecastError #" + checked_recastspellName);
                }
                else
                {
                    if (_ELITEAPIPL.Recast.GetSpellRecast(magic.Index) == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private void FailSafe()
        {
            var currentTime = DateTime.Now;
            if (!CanCast()) {
                if (currentTime.Subtract(CastingBackground_Check_timer) > TimeSpan.FromSeconds(5))
                {
                    //_ELITEAPIPL.ThirdParty.SendString("/p Activating super casting powers!");
                    _Log.Add(new LogEntry("Uh-Oh! Failsafe kicked in! ["+currentTime.Subtract(CastingBackground_Check_timer)+"]", Color.Violet));
                    FreeLock("Failsafe", CastingBackground_Check_timer);
                }
            }
        }

        public DateTime GetLock()
        {
            CastingBackground_Check = true;
            CastingBackground_Check_timer = DateTime.Now;
            _Log.Add(new LogEntry("Lock aquired [" + CastingBackground_Check_timer.ToString("mm:ss:fff") + "] ", Color.Red)) ;
            return CastingBackground_Check_timer;
        }

        public void FreeLock(string input, DateTime lockStamp)
        {
            //timestamp is used to make sure you only release the lock of the same cast and not of the new one which might have already started
            if(CastingBackground_Check_timer == lockStamp)
            {
                CastingBackground_Check = false;
                _Log.Add(new LogEntry("[" + input + "] Lock realeased [" + lockStamp.ToString("mm:ss:fff") + "]", Color.Green));
            }
            else
            {
                _Log.Add(new LogEntry("[" + input + "] Lock expired [" + lockStamp.ToString("mm:ss:fff") + "]", Color.Violet));
            }
        }

        public bool CanCast()
        {
            return !CastingBackground_Check;
        }

        private bool HasApi()
        {
            return _ELITEAPIMonitored != null && _ELITEAPIPL != null;
        }
    }
}
