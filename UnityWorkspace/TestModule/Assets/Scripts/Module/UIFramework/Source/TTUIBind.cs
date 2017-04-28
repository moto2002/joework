namespace TinyTeam.UI
{
    using UnityEngine;
    using System.Collections;

    /// <summary>
    /// Bind Some Delegate Func For Yours.
    /// </summary>
    public class TTUIBind
    {
        static bool isBind = false;

        public static void Bind()
        {
            if (!isBind)
            {
                isBind = true;

                //bind for your loader api to load UI.
				TTUIPage.delegateSyncLoadUI = SceneLoadManager.Instance.GetCurrentSceneCacheObj;
                TTUIPage.delegateAsyncLoadUI = ResourcesManager.Instance.LoadAssetBundle;

            }
        }
    }
}