using KSP.Localization;

// from Alshain01/KSP-AdvancedTweakablesButton
namespace VesselView
{
    class Toolbar : IButtonBar
    {
        string baseTexture = "icon", configTexture = "iconC";
        IButton baseButton;
        IButton configButton;
        VesselViewPlugin myvvp;

        public Toolbar(string appPath, VesselViewPlugin vvp)
        {
            myvvp = vvp;
            baseTexture = appPath + baseTexture;
            baseButton = ToolbarManager.Instance.add("VV", "VVbutton");
            baseButton.TexturePath = baseTexture;
            baseButton.ToolTip = "Vessel View";
            baseButton.Visibility = new GameScenesVisibility(GameScenes.FLIGHT);
            baseButton.OnClick += (e) =>
            {
                if (e.MouseButton == 0 && baseButton != null)
                {
                    myvvp.settings.screenVisible = !myvvp.settings.screenVisible;
                }
            };

            configTexture = appPath + configTexture;
            configButton = ToolbarManager.Instance.add("VVC", "VVbuttonC");
            configButton.TexturePath = configTexture;
            configButton.ToolTip = "Vessel View Config";
            configButton.Visibility = new GameScenesVisibility(GameScenes.FLIGHT);
            configButton.OnClick += (e) =>
            {
                if (e.MouseButton == 0 && configButton != null)
                {
                    myvvp.settings.configScreenVisible = !myvvp.settings.configScreenVisible;
                }
            };
        }

        public void Destroy()
        {
            if (baseButton != null)
            {
                baseButton.Destroy();
                baseButton = null;
            }
            if (configButton != null)
            {
                configButton.Destroy();
                configButton = null;
            }
        }
    }
}
