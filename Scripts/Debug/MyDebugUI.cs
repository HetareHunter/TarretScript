using UnityEngine;
using UnityEngine.UI;

public class MyDebugUI : MonoBehaviour
{
    bool inMenu;

    void Start()
    {
        DebugUIBuilder.instance.AddLabel("Debug Start", DebugUIBuilder.DEBUG_PANE_CENTER);
        DebugUIBuilder.instance.AddLabel("Debug Log", DebugUIBuilder.DEBUG_PANE_LEFT);
        DebugUIBuilder.instance.Show();
        inMenu = true;
    }

    void Update()
    {
        // B�{�^���Ńf�o�b�O�f�B�X�v���C�̕\���E��\��
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            if (inMenu) DebugUIBuilder.instance.Hide();
            else DebugUIBuilder.instance.Show();
            inMenu = !inMenu;
        }

        // A�{�^���Ńf�o�b�O���O���N���A
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            DebugUIBuilder.instance.Clear();
            DebugUIBuilder.instance.AddLabel("Clear");
            DebugUIBuilder.instance.AddDivider();
        }
    }
}