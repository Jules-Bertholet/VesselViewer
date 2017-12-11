using UnityEngine;
using KSP.UI.Screens;

// from Alshain01/KSP-AdvancedTweakablesButton
namespace VesselView
{
    class AppLauncher : IButtonBar
    {
        ApplicationLauncherButton launcherButton = null;
        static Texture2D baseTexture;
        VesselViewPlugin myvvp;


        public AppLauncher(string appPath, VesselViewPlugin vvp)
        {
            myvvp = vvp;
            baseTexture = GameDatabase.Instance.GetTexture(appPath + "icon38", false);

            GameEvents.onGUIApplicationLauncherReady.Add(Add);
            GameEvents.onGUIApplicationLauncherDestroyed.Add(Destroy);
            GameEvents.onGUIApplicationLauncherUnreadifying.Add(Destroy);
        }

        private void Add()
        {
            if (launcherButton == null)
            {
                launcherButton = ApplicationLauncher.Instance.AddModApplication(OnClick, OnClick, null, null, null, null, ApplicationLauncher.AppScenes.FLIGHT, baseTexture);
                launcherButton.onRightClick = OnRightClick;
            }
        }

        private void Destroy(GameScenes scene)
        {
            if (scene != GameScenes.FLIGHT)
                Destroy();
        }

        public void Destroy()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(Add);
            GameEvents.onGUIApplicationLauncherUnreadifying.Remove(Destroy);
            if (launcherButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(launcherButton);
                launcherButton = null;
            }
        }

        private void OnClick()
        {
            if (launcherButton != null)
            {
                myvvp.settings.screenVisible = !myvvp.settings.screenVisible;
            }
        }

        private void OnRightClick()
        {
            myvvp.settings.configScreenVisible = !myvvp.settings.configScreenVisible;
        }
    }
}
