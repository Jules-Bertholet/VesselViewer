using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

#if false

namespace VesselView
{
    // http://forum.kerbalspaceprogram.com/index.php?/topic/147576-modders-notes-for-ksp-12/#comment-2754813
    // search for "Mod integration into Stock Settings

    public class VesselViewerPluginSettings : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "General Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Vessel Viewer Plugin"; } }
        public override string DisplaySection { get { return "Vessel Viewer Plugin"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return false; } }


#if false
        [GameParameters.CustomParameterUI("Use Blizzy toobar if available")]
        public bool useToolbarIfAvailable = true;
#endif

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            return true; //otherwise return true
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {

            return true;
            //            return true; //otherwise return true
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }

    }
}

#endif