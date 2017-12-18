using System;
using System.Collections.Generic;
using UnityEngine;
//using Toolbar;
using VesselView;
using KSP.UI.Screens;

namespace VesselView
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class VesselViewPlugin : MonoBehaviour
    {

        public int scrnSizeXpos = 3;
        public int scrnSizeYpos = 3;

        //GUI stuff
        //Toolbar button

        const string appPath = "VesselView/Textures/";
        IButtonBar button;

        protected Rect windowPos;
        protected Rect windowCPos;

        //our screen
        RenderTexture screen;
        private VesselViewer viewer;
        public ViewerSettings settings;

        private int customMode = 0;
        private static List<CustomModeSettings> customModes = new List<CustomModeSettings>();

        public static void registerCustomMode(CustomModeSettings settings)
        {
            customModes.Add(settings);
        }

        /// <summary>
        /// Yoinked from a tutorial.
        /// </summary>
        private void drawGUI()
        {
            GUI.skin = HighLogic.Skin;
            windowPos = GUILayout.Window(1, windowPos, WindowGUI, "Vessel View", GUILayout.MinWidth(screen.width), GUILayout.MaxWidth(screen.width), GUILayout.MinHeight(screen.height), GUILayout.MaxHeight(screen.height));
        }

        /// <summary>
        /// Yoinked from a tutorial.
        /// </summary>
        private void drawGUIC()
        {
            GUI.skin = HighLogic.Skin;
            windowCPos = GUILayout.Window(2, windowCPos, WindowGUI, "Vessel View Config", GUILayout.MinWidth(64));
        }

        /// <summary>
        /// Setup the window for the addon.
        /// </summary>
        /// <param name="windowID"></param>
        private void WindowGUI(int windowID)
        {
            //setup the style
            GUIStyle mySty = new GUIStyle(GUI.skin.button);
            GUIStyle myStyL = new GUIStyle(GUI.skin.label);
            GUIStyle myStyT = new GUIStyle(GUI.skin.toggle);

            int h = 16;

            mySty.fontSize = 12;
            mySty.fixedHeight = h;
            myStyL.fontSize = 12;
            myStyT.fontSize = 12;

            mySty.normal.textColor = mySty.focused.textColor = Color.white;
            mySty.hover.textColor = mySty.active.textColor = Color.yellow;
            mySty.onNormal.textColor = mySty.onFocused.textColor = mySty.onHover.textColor = mySty.onActive.textColor = Color.green;
            mySty.padding = new RectOffset(8, 8, 8, 8);


            if (windowID == 1)
            {
                //draw the texture
                GUILayout.Box(screen);
            }
            if (windowID == 2)
            {
                //and now, buttons. lots of buttons.
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.Label("Display configuration", myStyL, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.Label("Orientation: ", myStyL);
                settings.drawPlane = GUILayout.SelectionGrid(settings.drawPlane, ViewerConstants.PLANES, ViewerConstants.PLANES.Length, mySty);

#if false
                for (int i = 0; i < ViewerConstants.PLANES.Length; i++)
                {
                    bool b = false;
                    if (i == settings.drawPlane)
                        b = true;
                    GUILayout.BeginVertical();
                    b1 = GUILayout.Toggle(b, ViewerConstants.PLANES[i]);
                    GUILayout.EndVertical();
                    if ()
                }
#endif
                GUILayout.EndHorizontal();
#if false
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Orientation:" + ViewerConstants.PLANES[settings.drawPlane], mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.drawPlane++;
                    if (settings.drawPlane == ViewerConstants.PLANES.Length) settings.drawPlane = 0;
                }
#endif
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.BeginVertical(GUILayout.Height(h));
                settings.autoCenter = GUILayout.Toggle(settings.autoCenter, "Autocentering", myStyT);
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.Height(h));
                settings.centerOnRootH = GUILayout.Toggle(settings.centerOnRootH, "Hor. root center", myStyT);
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.Height(h));
                settings.centerOnRootV = GUILayout.Toggle(settings.centerOnRootV, "Ver. root center", myStyT);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(h));
#if false
                if (GUILayout.Button("Autocentering:" + settings.autoCenter, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.autoCenter = !settings.autoCenter;
                }

                if (GUILayout.Button("Hor. root center:" + settings.centerOnRootH, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.centerOnRootH = !settings.centerOnRootH;
                }
                if (GUILayout.Button("Ver. root center:" + settings.centerOnRootV, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.centerOnRootV = !settings.centerOnRootV;
                }
#endif
                GUILayout.Label("Autoscaling: ", myStyL);
                settings.centerRescale = GUILayout.SelectionGrid(settings.centerRescale, ViewerConstants.RESCALEMODES, ViewerConstants.RESCALEMODES.Length, mySty);
#if false
                if (GUILayout.Button("Autoscaling:" + ViewerConstants.RESCALEMODES[settings.centerRescale], mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.centerRescale++;
                    if (settings.centerRescale == ViewerConstants.centerRescaleMAX) settings.centerRescale = 0;
                }
#endif
                /*if (GUILayout.Button("Scale:" + settings.scaleFact, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.scalePos++;
                    if (settings.scalePos == ViewerConstants.SCALE_FACTS.Length) settings.scalePos = 0;
                    //scrOffX = OFFX_DEF + (int)((scrOffX - OFFX_DEF) / (settings.scaleFact / SCALE_FACTS[settings.scalePos]));
                    //scrOffY = OFFY_DEF + (int)((scrOffY - OFFY_DEF) / (settings.scaleFact / SCALE_FACTS[settings.scalePos]));
                    settings.scaleFact = ViewerConstants.SCALE_FACTS[settings.scalePos];
                }*/

                GUILayout.EndHorizontal();

                if (!settings.autoCenter)
                {
                    GUILayout.BeginHorizontal(GUILayout.Height(h));
                    GUILayout.Label("Manual positioning", myStyL, GUILayout.ExpandWidth(true));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(GUILayout.Height(h));
                    if (GUILayout.Button("Up", mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                    {
                        viewer.manuallyOffset(0, ViewerConstants.OFFSET_MODS[settings.scalePos]);
                    }
                    if (GUILayout.Button("Down", mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                    {
                        viewer.manuallyOffset(0, -ViewerConstants.OFFSET_MODS[settings.scalePos]);
                    }
                    if (GUILayout.Button("Left", mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                    {
                        viewer.manuallyOffset(-ViewerConstants.OFFSET_MODS[settings.scalePos], 0);
                    }
                    if (GUILayout.Button("Right", mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                    {
                        viewer.manuallyOffset(ViewerConstants.OFFSET_MODS[settings.scalePos], 0);
                    }
                    if (GUILayout.Button("Center", mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                    {
                        viewer.nilOffset(screen.width, screen.height);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.Label("Autorotation axis:", myStyL);
                settings.spinAxis = GUILayout.SelectionGrid(settings.spinAxis, ViewerConstants.AXES, ViewerConstants.AXES.Length, mySty);
                GUILayout.EndHorizontal();

#if false
                if (GUILayout.Button("Autorotation axis:" + ViewerConstants.AXES[settings.spinAxis], mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.spinAxis++;
                    if (settings.spinAxis == ViewerConstants.spinAxisMAX) settings.spinAxis = 0;
                }
#endif
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.Label("Autorotation speed:", myStyL);
                settings.spinSpeed = GUILayout.SelectionGrid(settings.spinSpeed, ViewerConstants.SPIN_SPEEDS, ViewerConstants.SPIN_SPEEDS.Length, mySty);
                GUILayout.EndHorizontal();
#if false
                if (GUILayout.Button("Autorotation speed:" + ViewerConstants.SPIN_SPEEDS[settings.spinSpeed], mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.spinSpeed++;
                    if (settings.spinSpeed == ViewerConstants.spinSpeedMAX) settings.spinSpeed = 0;
                }
#endif
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                string[] customModeAr = new string[customModes.Count + 1];
                customModeAr[0] = "Inactive";
                int i = 1;
                foreach (var s in customModes)
                {
                    customModeAr[i++] = s.name;
                }
                GUILayout.Label("Custom mode: ", myStyL);
                customMode = GUILayout.SelectionGrid(customMode, customModeAr, customModes.Count + 1, mySty);
                if (customMode != 0)
                    viewer.customMode = (customModes.ToArray())[customMode - 1];
                else
                    viewer.customMode = null;
                GUILayout.EndHorizontal();
#if false
                String customModeName = "Inactive";

                if (customModes.Count > 0 & customMode >= 0) customModeName = (customModes.ToArray())[customMode].name;
                if (GUILayout.Button("Custom mode:" + customModeName, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    customMode++;
                    if (customMode >= customModes.Count | customModes.Count == 0) customMode = -1;
                    if (customMode != -1) viewer.customMode = (customModes.ToArray())[customMode];
                    else viewer.customMode = null;
                }
                GUILayout.EndHorizontal();
#endif
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.Label("Draw modes", myStyL, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.Label("Mesh:", myStyL);
                settings.colorModeFillDull = GUILayout.Toggle(settings.colorModeFillDull, "Dull", myStyT);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                settings.colorModeFill = GUILayout.SelectionGrid(settings.colorModeFill, ViewerConstants.COLORMODES, ViewerConstants.COLORMODES.Length, mySty);
               // GUILayout.EndHorizontal();
               // GUILayout.BeginHorizontal();

                GUILayout.EndHorizontal();
#if false
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Mesh:" + ViewerConstants.COLORMODES[settings.colorModeFill], mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.colorModeFill++;
                    if (settings.colorModeFill == ViewerConstants.COLORMODES.Length) settings.colorModeFill = 0;
                }
#endif

                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.Label("Wire:", myStyL);
                settings.colorModeWireDull = GUILayout.Toggle(settings.colorModeWireDull, "Dull", myStyT);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                settings.colorModeWire = GUILayout.SelectionGrid(settings.colorModeWire, ViewerConstants.COLORMODES, ViewerConstants.COLORMODES.Length, mySty);
               // GUILayout.EndHorizontal();

                GUILayout.EndHorizontal();
#if false
                if (GUILayout.Button("Dull:" + settings.colorModeFillDull, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.colorModeFillDull = !settings.colorModeFillDull;
                }
                if (GUILayout.Button("Wire:" + ViewerConstants.COLORMODES[settings.colorModeWire], mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.colorModeWire++;
                    if (settings.colorModeWire == ViewerConstants.COLORMODES.Length) settings.colorModeWire = 0;
                }

                if (GUILayout.Button("Dull:" + settings.colorModeWireDull, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.colorModeWireDull = !settings.colorModeWireDull;
                }
#endif

                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.Label("Bounds:", myStyL);
                settings.colorModeBoxDull = GUILayout.Toggle(settings.colorModeBoxDull, "Dull", myStyT);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUILayout.Height(h));

                settings.colorModeBox = GUILayout.SelectionGrid(settings.colorModeBox, ViewerConstants.COLORMODES, ViewerConstants.COLORMODES.Length, mySty);
                // GUILayout.EndHorizontal();

                GUILayout.EndHorizontal();

#if false
                if (GUILayout.Button("Bounds:" + ViewerConstants.COLORMODES[settings.colorModeBox], mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.colorModeBox++;
                    if (settings.colorModeBox == ViewerConstants.COLORMODES.Length) settings.colorModeBox = 0;
                }
                if (GUILayout.Button("Dull:" + settings.colorModeBoxDull, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.colorModeBoxDull = !settings.colorModeBoxDull;
                }
                GUILayout.EndHorizontal();
#endif
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.BeginVertical(GUILayout.Height(h));
                settings.displayAxes = GUILayout.Toggle(settings.displayAxes, "Axes", myStyT);
                GUILayout.EndVertical();
                GUILayout.BeginVertical(GUILayout.Height(h));
                settings.displayCOM = GUILayout.Toggle(settings.displayCOM, "Center of mass", myStyT);
                GUILayout.EndVertical();
                GUILayout.BeginVertical(GUILayout.Height(h));
                settings.displayEngines = GUILayout.Toggle(settings.displayEngines, "Engine status", myStyT);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
#if false
                if (GUILayout.Button("Axes:" + settings.displayAxes, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.displayAxes = !settings.displayAxes;
                }
                if (GUILayout.Button("Center of mass:" + settings.displayCOM, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.displayCOM = !settings.displayCOM;
                }
                if (GUILayout.Button("Engine status:" + settings.displayEngines, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.displayEngines = !settings.displayEngines;
                }
#endif

                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.Label("Landing assist:", myStyL);

                settings.displayGround = GUILayout.SelectionGrid(settings.displayGround, ViewerConstants.GROUND_DISPMODES, ViewerConstants.GROUND_DISPMODES.Length, mySty);
                GUILayout.EndHorizontal();

#if false
                GUILayout.BeginHorizontal(GUILayout.Height(h));

                if (GUILayout.Button("Landing assist:" + ViewerConstants.GROUND_DISPMODES[settings.displayGround], mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.displayGround++;
                    if (settings.displayGround == ViewerConstants.displayGroundMAX) settings.displayGround = 0;
                }
                GUILayout.EndHorizontal();
#endif
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                GUILayout.Label("Other configuration", myStyL, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(GUILayout.Height(h));
                if (GUILayout.Button("X size:" + screen.width, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    scrnSizeXpos++;
                    if (scrnSizeXpos == ViewerConstants.SCREEN_SIZES.Length) scrnSizeXpos = 0;
                    setupTexture();
                }
                if (GUILayout.Button("Y size:" + screen.height, mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    scrnSizeYpos++;
                    if (scrnSizeYpos == ViewerConstants.SCREEN_SIZES.Length) scrnSizeYpos = 0;
                    setupTexture();
                }
                if (GUILayout.Button("Margins:" + ViewerConstants.MARGINS[settings.margin], mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.margin++;
                    if (settings.margin == ViewerConstants.marginMAX) settings.margin = 0;
                }
                if (GUILayout.Button("Latency Mode:" + ViewerConstants.LATENCIES[settings.latency], mySty, GUILayout.ExpandWidth(true)))//GUILayout.Button is "true" when clicked
                {
                    settings.latency++;
                    if (settings.latency == ViewerConstants.latencyMAX) settings.latency = 0;
                }
                GUILayout.EndHorizontal();

            }
            GUI.DragWindow(); //  new Rect(0, 0, 10000, 20));
        }

        private void setupTexture()
        {
            if (screen != null)
            {
                screen.DiscardContents();
                screen.Release();
            }
            screen = new RenderTexture(ViewerConstants.SCREEN_SIZES[scrnSizeXpos], ViewerConstants.SCREEN_SIZES[scrnSizeYpos], 24, RenderTextureFormat.ARGB32);
        }

        /// <summary>
        /// Constructor. Apparently not meant to be used.
        /// </summary>
        public VesselViewPlugin()
        {

        }

        /// <summary>
        /// Called after the scene is loaded.
        /// </summary>
        void Awake()
        {
            if (ToolbarManager.ToolbarAvailable)
            {
                button = new Toolbar(appPath, this);
            }
            else
            {
                button = new AppLauncher(appPath, this);
            }
        }

        private void OnGUI()
        {
            if (settings.screenVisible)
                drawGUI();
            if (settings.configScreenVisible)
                drawGUIC();
        }

        /// <summary>
        /// Use for constructor-like stuff.
        /// </summary>
        void Start()
        {
            setupTexture();
            viewer = new VesselViewer();
            settings = viewer.basicSettings;
        }

        /// <summary>
        /// Called every frame.
        /// </summary>
        void FixedUpdate()
        {
            if (settings != null && settings.screenVisible)
            {
                viewer.drawCall(screen);
            }
        }



        /// <summary>
        /// Called at a fixed time interval determined by the physics time step.
        /// </summary>
        //void FixedUpdate()
        //{

        //}

        /// <summary>
        /// Called when the game is leaving the scene (or exiting). Perform any clean up work here.
        /// </summary>
        void OnDestroy()
        {
            if (button != null)
            {
                button.Destroy();
            }
        }


    }
}
