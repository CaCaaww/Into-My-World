using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class MeshCombiner : EditorWindow
{
    [MenuItem("Window/Mesh Combiner")]
    static void Init()
    {
        MeshCombiner window = (MeshCombiner)EditorWindow.GetWindow(typeof(MeshCombiner));
        window.Show();
    }

    DefaultAsset targetFolder = null;
    public Renderer[] renderers;

    public int maxVertexesPerMesh;
    public bool instantiateInScene;
    SerializedProperty renderersProp;

    SerializedObject serialObj;

    int counterSaved = 0;

    GUIStyle titleNameStyle;

    private void OnEnable()
    {
        ScriptableObject scriptableObj = this;
        serialObj = new SerializedObject(scriptableObj);

        renderersProp = serialObj.FindProperty("renderers");

        maxVertexesPerMesh = 60000;
        counterSaved = 0;
    }

    private void OnGUI()
    {
        if(titleNameStyle == null)
            titleNameStyle = new GUIStyle(EditorStyles.largeLabel);

        serialObj.Update();

        float width = position.width;

        titleNameStyle.fontStyle = FontStyle.Bold;
        titleNameStyle.fontSize = ((24 * (int)width) / 354); //354

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("     Mesh Combiner v1.0", titleNameStyle, GUILayout.Height( (32 * (int)width) / 354 ));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical(GUI.skin.box);
       
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            targetFolder = (DefaultAsset)EditorGUILayout.ObjectField(
                 "Save Folder",
                 targetFolder,
                 typeof(DefaultAsset),
                 false);

            maxVertexesPerMesh = EditorGUILayout.IntField("Max Vertexes Per Mesh", maxVertexesPerMesh);
            instantiateInScene = EditorGUILayout.Toggle("Instantiate In Scene", instantiateInScene);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.PropertyField(renderersProp, true);
        }
        EditorGUILayout.EndVertical();

        serialObj.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if (GUILayout.Button("Clear"))
        {
            renderers = new Renderer[0];
            renderersProp = serialObj.FindProperty("renderers");
        }
        if (GUILayout.Button("Bake"))
        {
            Dictionary<Material, List<MeshFilter>> meshesPerMaterial = new Dictionary<Material, List<MeshFilter>>();

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer r = renderers[i];

                if (r != null)
                {
                    Material mainMaterial = r.sharedMaterial;

                    if (r.GetType() == typeof(MeshRenderer))
                    {
                        var meshFilter = ((MeshRenderer)r).GetComponent<MeshFilter>();
                        if (meshFilter)
                        {
                            if (!meshesPerMaterial.ContainsKey(mainMaterial))
                                meshesPerMaterial.Add(mainMaterial, new List<MeshFilter>());

                            meshesPerMaterial[mainMaterial].Add(meshFilter);
                        }
                    }
                }
            }

            if(meshesPerMaterial.Count > 0)
            {
                foreach(var meshes in meshesPerMaterial)
                {
                    int vertexCount = 0;

                    List<CombineInstance> combineInstances = new List<CombineInstance>();
                    for (int ci = 0; ci < meshes.Value.Count; ci++)
                    {
                        if(vertexCount >= maxVertexesPerMesh)
                        {
                            GenerateMesh(combineInstances.ToArray(), meshes.Key);
                            combineInstances.Clear();

                            vertexCount = 0;
                        }

                        Mesh mesh = meshes.Value[ci].sharedMesh;
                        vertexCount += mesh.vertexCount;

                        CombineInstance combinedInstance = new CombineInstance();

                        combinedInstance.mesh = mesh;
                        combinedInstance.transform = meshes.Value[ci].transform.localToWorldMatrix;

                        combineInstances.Add(combinedInstance);
                    }

                    if (combineInstances.Count > 0)
                        GenerateMesh(combineInstances.ToArray(), meshes.Key);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        EditorGUILayout.EndVertical();
    }

    void GenerateMesh(CombineInstance[] instances, Material material)
    {
        Mesh r = new Mesh();
        r.CombineMeshes(instances);

        string path = "";
        string key = counterSaved.ToString() + DateTime.Now.ToString("MMddyyyy_hhmmsstt");
        if(targetFolder != null)
        {
            path = AssetDatabase.GetAssetPath(targetFolder.GetInstanceID());
        }
        else
        {
            path = Application.dataPath + "/GeneratedMeshes/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        //AssetDatabase.CreateAsset(r, path + "/" + key + ".asset");
        r.name = key + "_mesh";

        GameObject newObject = new GameObject();

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(newObject, path + "/" + key + ".prefab");
        DestroyImmediate(newObject);

        MeshFilter meshFilter = prefab.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = r;

        MeshRenderer meshRenderer = prefab.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = material;

        AssetDatabase.AddObjectToAsset(r, AssetDatabase.GetAssetPath(prefab));

        if(instantiateInScene)
            PrefabUtility.InstantiatePrefab(prefab);
        //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(prefab));

        counterSaved++;
    }
}
