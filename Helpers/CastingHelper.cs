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
        public List<CastingAction> _Cures = new List<CastingAction>();
        public List<LogEntry> _Log = new List<LogEntry>();

        private EliteAPI _ELITEAPIPL;
        private EliteAPI _ELITEAPIMonitored;
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
            foreach (CastingAction action in _Cures.OrderByDescending(x => x.Priority))
            {
                FailSafe();
                if (DeQueueSpell(ref _Cures, action))
                {
                    return;
                }
            }
        }

        private void RemoveSpell(ref List<CastingAction> list, CastingAction action, string reason)
        {
            lock (list)
            {
                list.Remove(action);
            }
            if (!string.IsNullOrEmpty(reason))
            {
                AddLog(new LogEntry(reason, Color.Brown));
            }
        }

        public bool DeQueueSpell(ref List<CastingAction> list, CastingAction action)
        {
            
            if (!HasApi()) { return false; } //this should not be happening
            if (DateTime.Now.Subtract(action.Invoked) >= GetQueueExpiration(action.Type)) {
                RemoveSpell(ref list, action, $"Queue Timeout [{action.SpellName}] [{DateTime.Now.Subtract(action.Invoked)}]");
                return false;
            }
            var spellName = action.SpellName;
            if (action.Type != SpellType.Action)
            {
                if (!CanCast()) { return false; }
                if (IsMoving()) { return false; }
               
                if (action.Type == SpellType.Raise)
                {
                    if (_PlayerHelper.IsAlive(action.Target)) { RemoveSpell(ref list, action, $"{action.Target} already alive!"); ; return false; }
                    spellName = _SpellsHelper.GetRaiseSpell();
                }
                else
                {
                    if (!_PlayerHelper.IsAlive(action.Target)) { RemoveSpell(ref list, action, $"{action.Target} already dead :("); return false; }
                }
                if (!_PlayerHelper.CastingPossible(action.Target)) { RemoveSpell(ref list, action, $"{action.Target} out of range."); return false; }

                if (action.Type == SpellType.Healing)
                {
                    //Special Case for cures since when its on recast we check lower tiers
                    var preSpell = spellName;
                    spellName = _CureHelper.GetCureSpell(spellName);
                    if (spellName == "false") { RemoveSpell(ref list, action, $"{preSpell} spell not ready."); return false; }
                }
                else
                {
                    if (!SpellRecastReady(new CastingAction() { SpellName = spellName, JobAbilities = action.JobAbilities })) { return false; } //try next spell
                }
            }
            if (CanAct())
            {
                AddLog(new LogEntry("Attempting to cast [" + spellName + "] on [" + action.Target + "]", Color.DimGray));
                RemoveSpell(ref list, action, "");
                
                if (action.Type == SpellType.GEO) {
                    _GoeHelper.GetTargetOnCast(action.Target);
                }
                if(action.Type == SpellType.Action)
                {
                    new System.Threading.Tasks.Task(() => { CastAction(action.Target, spellName, action.JobAbilities); }).Start();
                }
                else
                {
                    new System.Threading.Tasks.Task(() => { CastSpell(action.Target, spellName, action.JobAbilities); }).Start();
                }
                
                return true;
            }
            return false;
        }

        public void Cleanup()
        {

        }

        public void QueueSpell(SpellType type, string partyMemberName, string spellName)
        {
            QueueSpell(type, partyMemberName, spellName, SpellPrio.Low, new List<JobAbility>());
        }
        public void QueueSpell(SpellType type, string partyMemberName, string spellName, Enum priority)
        {
            QueueSpell(type, partyMemberName, spellName, priority, new List<JobAbility>());
        }
        public void QueueSpell(SpellType type, string partyMemberName, string spellName, Enum priority, List<JobAbility> jas)
        {
            //doesnt do anything yet, prepraration for a queueing system
            CastingAction action = new CastingAction(type, spellName, partyMemberName, priority, jas);
            AddToQueue(_Cures, action);
        }

        private void AddToQueue(List<CastingAction> list, CastingAction action)
        {
            lock (list)
            {
                var existing = list.FirstOrDefault(x => x.SpellName == action.SpellName && x.Target == action.Target);
                if (existing != null)
                {
                    existing.Invoked = action.Invoked;
                    existing.Priority = action.Priority;
                    existing.JobAbilities = action.JobAbilities;
                }
                else
                {
                    list.Add(action);
                }
            }
        }

        public void CastSpell(string partyMemberName, string spellName, List<JobAbility> jas)
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

            var lockStamp = GetLock();

            var cast = "/ma \"" + castingSpell + "\" " + partyMemberName;
            _ELITEAPIPL.ThirdParty.SendString(cast);
            Thread.Sleep(200);
            _ELITEAPIPL.ThirdParty.SendString(cast);
            Thread.Sleep(200);
            _ELITEAPIPL.ThirdParty.SendString(cast);
            Thread.Sleep(200);
            _ELITEAPIPL.ThirdParty.SendString(string.Format("//cpaddon lock {0}", lockStamp));
            AddLog(new LogEntry(cast, Color.Black));

            //_Form.currentAction.Text = "Casting: " + castingSpell+ " → "+ partyMemberName;
            //new System.Threading.Tasks.Task(() => { ProtectCasting(lockStamp); }).Start();
        }
        public void CastAction(string partyMemberName, string spellName, List<JobAbility> jas)
        {
            if (!HasApi()) { return; }
            if (jas.Count > 0)
            {
                foreach (JobAbility ja in jas)
                {
                    _JaHelper.DoAbility(ja);
                }
            }
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
                if (GetUnix() - IsPerformingAction_Timer > 4500)
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
            if ((plStatusCheck(StatusEffect.Paralysis) || plStatusCheck(StatusEffect.Silence)) && OptionsForm.config.plSilenceItemEnabled)
            {
                // Check to make sure we have echo drops
                if (ItemHelper.HasItem(_ELITEAPIPL, ItemHelper.GetSilenaItem()))
                {
                    _JaHelper.Item_Wait(ItemHelper.GetSilenaItem());
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
                    _JaHelper.Item_Wait(ItemHelper.GetCursnaItem());
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
                    return TimeSpan.FromSeconds(2);
                case SpellType.Healing:
                    return TimeSpan.FromSeconds(2);
                case SpellType.Buff:
                    return TimeSpan.FromSeconds(2);
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
                _Log = new List<LogEntry>();
            }
            _Log.Add(entry);    
        }
    }
}
