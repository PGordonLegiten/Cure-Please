using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.DataStructures
{
    public enum SpellType : byte
    {
        Prio = 0,
        Healing = 1,
        Buff = 2,
        Debuff = 3,
        GEO = 4,
        Raise = 5,
        Action = 6,
    }

    public enum SpellPrio : byte
    {
        Low = 10,
        Middle =11,
        High = 12,
        Higher = 13,
        Top = 14,
    }

    public enum CurePrio : byte
    {
        CureI = 1,
        CureII = 2,
        CureIII = 3,



        CureIV = 23,
        CureV = 24,
        CureVI = 25,
        CureVII = 26, //doenst exist only for prio pourposes
        CureVIII = 28, //doenst exist only for prio pourposes
        CuragaI = 30,
        CuragaII = 31,
        CuragaIII = 32,
        CuragaIV = 33,
        CuragaV = 34,
        CuragaVI = 35,//doenst exist only for prio pourposes
        CuragaVII = 37,//doenst exist only for prio pourposes
    }
    public class CastingAction : IEquatable<CastingAction>
    {
        public string ActionId { get; set; }
        public string SpellName { get; set; }
        public string Target { get; set; }
        public SpellType Type { get; set; }
        public DateTime Invoked { get; set; }
        public int Priority { get; set; }
        public List<JobAbility> JobAbilities { get; set; }

        public CastingAction()
        {
        }
        public CastingAction(SpellType type, string spell, string target, Enum priority, List<JobAbility> jas)
        {
            SpellName = spell;
            Target = target;
            ActionId = target + spell;
            Invoked = DateTime.Now;
            Priority = Convert.ToInt32(priority);
            Type = type;
            JobAbilities = jas;
        }

        public override string ToString()
        {
            return "[" + Invoked.ToString("hh:mm:ss") + "][" + SpellName + "][" + Target + "][" + Priority + "]";
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            CastingAction objAsPart = obj as CastingAction;
            if (objAsPart == null)
            {
                return false;
            }
            else
            {
                return Equals(objAsPart);
            }
        }
        public override int GetHashCode()
        {
            return ActionId.GetHashCode();
        }
        public bool Equals(CastingAction other)
        {
            if (other == null)
            {
                return false;
            }
            return this.SpellName.Equals(other.SpellName) && this.Target.Equals(other.Target);
        }
    }
}
