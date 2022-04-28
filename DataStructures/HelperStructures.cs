using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.DataStructures
{
    public class BuffStorage : List<BuffStorage>
    {
        public string CharacterName { get; set; }

        public string CharacterBuffs { get; set; }
    }

    public class CharacterData : List<CharacterData>
    {
        public int TargetIndex { get; set; }

        public int MemberNumber { get; set; }
    }

    public class SongData : List<SongData>
    {
        public string song_type { get; set; }

        public int song_position { get; set; }

        public string song_name { get; set; }

        public int buff_id { get; set; }
    }

    public class SpellsData : List<SpellsData>
    {
        public string Spell_Name { get; set; }

        public int spell_position { get; set; }

        public int type { get; set; }

        public int buffID { get; set; }

        public bool aoe_version { get; set; }
    }

    public class GeoData : List<GeoData>
    {
        public int geo_position { get; set; }

        public string indi_spell { get; set; }

        public string geo_spell { get; set; }
    }

    public class JobTitles : List<JobTitles>
    {
        public int job_number { get; set; }

        public string job_name { get; set; }
    }
}
