using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshMerger))]
public class MeshMergerEditor : Editor
{
    string searchField = "�����...";
    bool useSearchName;
    int selectedMode;
    string[] selectedModeStr = {" ������ �� ������", " ������ �������������� ������� ����� �����������", " ������� �������������� ������� ����� �����������"};

    public override void OnInspectorGUI()
    {
        MeshMerger meshMerger = (MeshMerger)target;

        #region �����
        searchField = GUILayout.TextField(searchField, 25);
        useSearchName = GUILayout.Toggle(useSearchName, "��� ������� ��� ��� ��������� �������");
        if (GUILayout.Button("����� ��� ������� �� ��������"))
        {
            if ((searchField != null) || (searchField != "�����..."))
            {
                meshMerger.FindObjectsByName(searchField,useSearchName);
            }
        }
        #endregion

        #region ���� ����������
        if (DrawDefaultInspector())
        {
            
        }
        #endregion

        #region �������������� ���������
        GUILayout.Label("�������������� ���������", EditorStyles.boldLabel);

        GUILayout.Label("��������� �������������� ��������");

        selectedMode = GUILayout.SelectionGrid(selectedMode, selectedModeStr, 1, EditorStyles.radioButton);
        switch (selectedMode)
        {
            case 0:
                meshMerger.hideMergedObjects = false;
                meshMerger.deleteMergedObjects = false;
                break;

            case 1:
                meshMerger.hideMergedObjects = true;
                meshMerger.deleteMergedObjects = false;
                break;

            case 2:
                meshMerger.hideMergedObjects = false;
                meshMerger.deleteMergedObjects = true;
                break;

            default:
                break;
        }


        #endregion

        if (GUILayout.Button("���������� �������"))
        {
            meshMerger.MergeMeshes();
        }     


    }
   
}
