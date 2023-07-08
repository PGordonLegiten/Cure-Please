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
        CureI = 101,
        CureII = 102,
        CureIII = 103,
        CureIV = 123,
        CureV = 124,
        CureVI = 125,
        CureVII = 126, //doenst exist only for prio pourposes
        CureVIII = 128, //doenst exist only for prio pourposes
        CuragaI = 130,
        CuragaII = 131,
        CuragaIII = 132,
        CuragaIV = 133,
        CuragaV = 134,
        CuragaVI = 135,//doenst exist only for prio pourposes
        CuragaVII = 137,//doenst exist only for prio pourposes
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
