using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LocalizeText : MonoBehaviour
{
    [SerializeField]
    [ValueDropdown("GetLocalizationChoices")]
    private string id = null;

#if UNITY_EDITOR
    private List<string> cachedChoices = new List<string>();
    private IList<string> GetLocalizationChoices()
    {
        cachedChoices.Clear();
        cachedChoices.AddRange(Localization.localizationTable.Keys);
        return cachedChoices;
    }
#endif

    private void Awake()
    {
        var localizedText = Localization.Localize(id);
        if (string.IsNullOrEmpty(localizedText))
        {
            this.LogWarning($"Localization for string id {id} is missing!");
            return;
        }

        var tmproUgui = GetComponent<TMPro.TextMeshProUGUI>();
        if (tmproUgui != null)
        {
            tmproUgui.text = localizedText;
        }

        var tmpro = GetComponent<TMPro.TextMeshPro>();
        if (tmpro != null)
        {
            tmpro.text = localizedText;
        }

        var ugui = GetComponent<UnityEngine.UI.Text>();
        if (ugui != null)
        {
            ugui.text = localizedText;
        }
    }
}
