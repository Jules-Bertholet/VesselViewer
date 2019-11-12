
using UnityEngine;
using ToolbarControl_NS;

namespace VesselView
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(VesselViewPlugin.MODID, VesselViewPlugin.MODNAME);
            ToolbarControl.RegisterMod(VesselViewPlugin.MODIDCONFIG, VesselViewPlugin.MODNAMECONFIG);
        }
    }
}