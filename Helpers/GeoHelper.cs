using EliteMMO.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class GeoHelper
    {
        private EliteAPI _ELITEAPIPL;
        private EliteAPI _ELITEAPIMonitored;
        public GeoHelper(EliteAPI plApi, EliteAPI monitoredApi)
        {
            _ELITEAPIMonitored = monitoredApi;
            _ELITEAPIPL = plApi;
        }
        public void GetTargetOnCast(string target)
        {
            if(target == "<t>")
            {
                int GrabbedTargetID = GrabGEOTargetID();

                if (GrabbedTargetID != 0)
                {
                    _ELITEAPIPL.Target.SetTarget(GrabbedTargetID);
                    Thread.Sleep(TimeSpan.FromSeconds(4));
                    if (OptionsForm.config.DisableTargettingCancel == false)
                    {
                        ClearTarget();
                    }
                }
            }
        }
        public async void ClearTarget()
        {
            await Task.Delay(TimeSpan.FromSeconds((double)OptionsForm.config.TargetRemoval_Delay));
            _ELITEAPIPL.Target.SetTarget(0);
        }

        private int GrabGEOTargetID()
        {
            if (OptionsForm.config.specifiedEngageTarget == true && OptionsForm.config.LuopanSpell_Target != String.Empty)
            {
                for (int x = 0; x < 2048; x++)
                {
                    EliteAPI.XiEntity z = _ELITEAPIPL.Entity.GetEntity(x);

                    if (z.Name != null && z.Name.ToLower() == OptionsForm.config.LuopanSpell_Target.ToLower())
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
    }
}
