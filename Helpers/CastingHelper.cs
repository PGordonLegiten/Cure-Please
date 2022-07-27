using CurePlease.DataStructures;
using EliteMMO.API;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    
    internal class CastingHelper
    {
        public LinkedList<CastingAction> _Cures = new LinkedList<CastingAction>();
        public LinkedList<CastingAction> _Buffs = new LinkedList<CastingAction>();
        public LinkedList<CastingAction> _Debuffs = new LinkedList<CastingAction>();
        public LinkedList<CastingAction> _Priority = new LinkedList<CastingAction>();

        public LinkedList<LogEntry> _Log = new LinkedList<LogEntry>();

        private EliteAPI _ELITEAPIPL;
        private EliteAPI _ELITEAPIMonitored;
        private CurePleaseForm _Form;
        private CureHelper _CureHelper;
        private GeoHelper _GoeHelper;
        private PlayerHelper _PlayerHelper;
        private JaHelper _JaHelper;
        private SpellsHelper _SpellsHelper;
        public float plX;
        public float plY;
        public float plZ;


        private bool IsPerformingAction = false;
        public long IsPerformingAction_Timer = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public CastingHelper(CurePleaseForm form, EliteAPI pl, EliteAPI monitor)
        {
            _Form = form;
            _ELITEAPIMonitored = monitor;
            _ELITEAPIPL = pl;
            _CureHelper = new CureHelper(form, pl, monitor, this);
            _JaHelper = new JaHelper(form, pl, monitor, this);
            _PlayerHelper = new PlayerHelper(pl, monitor);
            _SpellsHelper = new SpellsHelper(_PlayerHelper);
            _GoeHelper = new GeoHelper(pl, monitor);
        }

        public async void Run()
        {
            FailSafe();
            //PRIO
            foreach (CastingAction action in _Priority.ToList().OrderByDescending(x => x.Priority))
            {
                if (DeQueueSpell(_Priority, action))
                {
                    return;                
                }
            }
            //CURES
            foreach (CastingAction action in _Cures.ToList().OrderByDescending(x => x.Priority))
            {
                if (OptionsForm.config.PrioritiseOverLowerTier == true && action.Priority < Convert.ToInt32(CurePrio.CureIV) && (_Debuffs.Count > 0))
                {
                    break; //skip all the lower cures if debuffs
                }
                if (DeQueueSpell(_Cures, action))
                {
                    return;
                }
            }
            //DEBUFFS
            foreach (CastingAction action in _Debuffs.ToList().OrderByDescending(x => x.Priority))
            {
                if (DeQueueSpell(_Debuffs, action))
                {
                    return;
                }
            }
            //BUFFS
            if(_Cures.Where(x => x.Priority >= Convert.ToInt32(CurePrio.CureIV)).Count() > 0) { return; }
            foreach (CastingAction action in _Buffs.ToList().OrderByDescending(x => x.Priority))
            {
                if (DeQueueSpell(_Buffs, action))
                {
                    return;
                }
            }
        }
        private void RemoveSpell(LinkedList<CastingAction> list, CastingAction action, string reason)
        {
            list.Remove(action);
            if (!string.IsNullOrEmpty(reason))
            {
                AddLog(new LogEntry(reason, Color.Brown));
            }
        }

        public bool DeQueueSpell(LinkedList<CastingAction> list, CastingAction action)
        {
            if (!HasApi()) { return false; } //this should not be happening
            if (DateTime.Now.Subtract(action.Invoked) >= GetQueueExpiration(action.Type)) {
                RemoveSpell(list, action, $"Queue Timeout [{action.SpellName}] [{DateTime.Now.Subtract(action.Invoked)}]");
                return false;
            }
            if (IsMoving()) { return false; } 
            if (!CanCast()) { return false; }
            var spellName = action.SpellName;
            if (action.Type == SpellType.Raise)
            {
                if (_PlayerHelper.IsAlive(action.Target)) { RemoveSpell(list, action, $"{action.Target} already alive!"); ; return false; }
                spellName = _SpellsHelper.GetRaiseSpell();
            }
            else 
            {
                if (!_PlayerHelper.IsAlive(action.Target)) { RemoveSpell(list, action, $"{action.Target} already dead :("); return false; }
                
            }
            if (!_PlayerHelper.CastingPossible(action.Target)) { RemoveSpell(list, action, $"{action.Target} out of range."); return false; }

            if (action.Type == SpellType.Healing)
            {
                //Special Case for cures since when its on recast we check lower tiers
                var preSpell = spellName;
                spellName = _CureHelper.GetCureSpell(spellName);
                if(spellName == "false") { RemoveSpell(list, action, $"{preSpell} spell not ready."); return false; }
            }
            else
            {
                if (!SpellRecastReady(new CastingAction() { SpellName = spellName, JobAbilities = action.JobAbilities })) { return false; } //try next spell
            }
            if (CanAct())
            {
                AddLog(new LogEntry("Attempting to cast [" + spellName + "] on [" + action.Target + "]", Color.DimGray));
                var lockStamp = GetLock();
                RemoveSpell(list, action, "");
                
                if (action.Type == SpellType.GEO) {
                    _GoeHelper.GetTargetOnCast(action.Target);
                }
                CastSpell(action.Target, spellName, lockStamp, action.JobAbilities);
                return true;
            }
            return false;
        }

        public void Cleanup()
        {

        }

        public void QueueSpell(SpellType type, string partyMemberName, string spellName)
        {
            QueueSpell(type, partyMemberName, spellName, SpellPrio.Low);
        }
        public void QueueSpell(SpellType type, string partyMemberName, string spellName, Enum priority)
        {
            QueueSpell(type, partyMemberName, spellName, priority, new List<JobAbility>());
        }
        public void QueueSpell(SpellType type, string partyMemberName, string spellName, Enum priority, List<JobAbility> jas)
        {
            //doesnt do anything yet, prepraration for a queueing system
            CastingAction action = new CastingAction(type, spellName, partyMemberName, priority, jas);
            switch (type)
            {
                case SpellType.Prio:
                case SpellType.Raise:
                    AddToQueue(_Priority, action);
                    break;
                case SpellType.Healing:
                    AddToQueue(_Cures, action);
                    break;
                case SpellType.Debuff:
                    AddToQueue(_Debuffs, action);
                    break;
                case SpellType.Buff:
                case SpellType.GEO:
                    AddToQueue(_Buffs, action);
                    break;
            }
        }

        private void AddToQueue(LinkedList<CastingAction> list, CastingAction action)
        {
            if (list.Contains(action))
            {
                list.Remove(action);
            };
            list.AddFirst(action);
        }

        public void CastSpell(string partyMemberName, string spellName, long lockStamp, List<JobAbility> jas)
        {
            if (!HasApi()) { return; }

            InternalHelper.setCooldown(spellName, partyMemberName);

            EliteAPI.ISpell magic = _ELITEAPIPL.Resources.GetSpell(spellName.Trim(), 0);

            var castingSpell = magic.Name[0];

            if(jas.Count > 0)
            {
                foreach(JobAbility ja in jas)
                {
                    _JaHelper.DoAbility(ja);
                }
            }
            var cast = "/ma \"" + castingSpell + "\" " + partyMemberName;
            _ELITEAPIPL.ThirdParty.SendString(cast);
            _ELITEAPIPL.ThirdParty.SendString(string.Format("//cpaddon lock {0}", lockStamp));
            AddLog(new LogEntry(cast, Color.Black));

            _Form.currentAction.Text = "Casting: " + castingSpell+ " → "+ partyMemberName;

            new System.Threading.Tasks.Task(() => { ProtectCasting(lockStamp); }).Start();
        }
        private void ProtectCasting(long lockStamp)
        {
            Thread.Sleep(TimeSpan.FromSeconds(3.0));
            int count = 0;
            float lastPercent = 0;
            float castPercent = _ELITEAPIPL.CastBar.Percent;
            while (castPercent < 1)
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

            Thread.Sleep(TimeSpan.FromSeconds(1.0));
            FreeLock("ProtectCasting_DoWork", lockStamp);
        }



        public bool SpellRecastReady(CastingAction action)
        {
            if(action == null || action.SpellName == null) { return false; }
            var checked_recastspellName = action.SpellName.Trim().ToLower();

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
                        if (action.JobAbilities.Count > 0)
                        {
                            var ja_recasts = true;
                            foreach (JobAbility ja in action.JobAbilities)
                            {
                                if (!_PlayerHelper.plStatusCheck(ja.Status) && !_JaHelper.IsJaReady(ja))
                                {
                                    ja_recasts = false;
                                }
                            }
                            if (!_JaHelper.HasTotalStratagems(action.JobAbilities)) //check there is enough stratagems for all sch ja's
                            {
                                ja_recasts = false;
                            }
                            return ja_recasts;
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
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsMoving()
        {
            return (_ELITEAPIPL.Player.X != plX) || (_ELITEAPIPL.Player.Y != plY) || (_ELITEAPIPL.Player.Z != plZ);
        }
        private void FailSafe()
        {
            if (!CanAct()) {
                if (GetUnix() - IsPerformingAction_Timer > 6000)
                {
                    //_ELITEAPIPL.ThirdParty.SendString("/p Activating super casting powers!");
                    AddLog(new LogEntry("Uh-Oh! Failsafe kicked in! ["+ (GetUnix() - IsPerformingAction_Timer) + "]", Color.Violet));
                    FreeLock("Failsafe", IsPerformingAction_Timer);
                }
            }
        }

        public long GetLock()
        {
            IsPerformingAction = true;
            IsPerformingAction_Timer = GetUnix();
            AddLog(new LogEntry("Lock aquired [" + IsPerformingAction_Timer + "] ", Color.Red)) ;
            return IsPerformingAction_Timer;
        }

        public void FreeLock(string input, long lockStamp)
        {
            //timestamp is used to make sure you only release the lock of the same cast and not of the new one which might have already started
            if(IsPerformingAction_Timer == lockStamp)
            {
                IsPerformingAction = false;
                AddLog(new LogEntry("[" + input + "] Lock realeased [" + lockStamp + "]", Color.Green));
            }
            else
            {
                AddLog(new LogEntry("[" + input + "] Lock expired [" + lockStamp + "]", Color.Blue));
            }
        }

        private long GetUnix()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public bool CanAct()
        {
            return !IsPerformingAction;
        }

        public bool CanCast()
        {
            if (plStatusCheck(StatusEffect.Silence) && OptionsForm.config.plSilenceItemEnabled)
            {
                // Check to make sure we have echo drops
                if (ItemHelper.HasItem(_ELITEAPIPL, ItemHelper.GetSilenaItem()))
                {
                    _Form.Item_Wait(ItemHelper.GetSilenaItem());
                    return false;
                }
                else
                {
                    _ELITEAPIPL.ThirdParty.SendString("//get \"" + ItemHelper.GetSilenaItem() + "\""); //try to get it
                }
            }
            else if ((plStatusCheck(StatusEffect.Doom) && OptionsForm.config.plDoomEnabled) /* Add more options from UI HERE*/)
            {
                // Check to make sure we have holy water
                if (ItemHelper.HasItem(_ELITEAPIPL, ItemHelper.GetCursnaItem()))
                {
                    _Form.Item_Wait(ItemHelper.GetCursnaItem());
                    return false;
                }
                else
                {
                    _ELITEAPIPL.ThirdParty.SendString("//get \"" + ItemHelper.GetCursnaItem() + "\""); //try to get it
                }
            }
            return !plStatusCheck(StatusEffect.Silence) && !plStatusCheck(StatusEffect.Terror) && !plStatusCheck(StatusEffect.Petrification) && !plStatusCheck(StatusEffect.Stun);
        }

        private bool HasApi()
        {
            return _ELITEAPIMonitored != null && _ELITEAPIPL != null;
        }


        private TimeSpan GetQueueExpiration(SpellType type)
        {
            //THIS VALUES NEEDS TO BE TESTED (MAYBE TOO SHORT?)
            switch (type)
            {
                case SpellType.Prio:
                    return TimeSpan.FromSeconds(10);
                case SpellType.Healing:
                    return TimeSpan.FromSeconds(3);
                case SpellType.Buff:
                    return TimeSpan.FromSeconds(10);
                case SpellType.Debuff:
                    return TimeSpan.FromSeconds(2);
                default:
                    return TimeSpan.FromSeconds(2);
            }
        }

        private bool plStatusCheck(StatusEffect requestedStatus)
        {
            bool statusFound = false;
            foreach (StatusEffect status in _ELITEAPIPL.Player.Buffs.Cast<StatusEffect>().Where(status => requestedStatus == status))
            {
                statusFound = true;
            }
            return statusFound;
        }

        public void AddLog(LogEntry entry)
        {
            if(_Log.Count() > 500)
            {
                _Log.RemoveFirst();
            }
            _Log.AddLast(entry);    
        }
    }
}
