using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshMerger))]
public class MeshMergerEditor : Editor
{
    string searchField = "Поиск...";
    bool useSearchName;
    int selectedMode;
    string[] selectedModeStr = {" Ничего не делать", " Скрыть первоначальные обьекты после объединения", " Удалить первоначальные обьекты после объединения"};

    public override void OnInspectorGUI()
    {
        MeshMerger meshMerger = (MeshMerger)target;

        #region Поиск
        searchField = GUILayout.TextField(searchField, 25);
        useSearchName = GUILayout.Toggle(useSearchName, "Имя запроса как имя выходного объекта");
        if (GUILayout.Button("Найти все объекты по названию"))
        {
            if ((searchField != null) || (searchField != "Поиск..."))
            {
                meshMerger.FindObjectsByName(searchField,useSearchName);
            }
        }
        #endregion

        #region Тело Инспектора
        if (DrawDefaultInspector())
        {
            
        }
        #endregion

        #region Дополнительные Параметры
        GUILayout.Label("Дополнительные параметры", EditorStyles.boldLabel);

        GUILayout.Label("Настройки первоначальных обьектов");

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

        if (GUILayout.Button("Объединить объекты"))
        {
            meshMerger.MergeMeshes();
        }     


    }
   
}
