using UnityEditor;
using UnityEngine;

public class ProceduralTerrainPainter : EditorWindow
{
    private Terrain terrain;

    [Header("Terrain Layers")]
    public TerrainLayer grassLayer;
    public TerrainLayer dirtLayer;
    public TerrainLayer sandLayer;

    [Header("Sand")]
    [Tooltip("Height above the terrain's minimum height where sand can appear.")]
    public float sandHeight = 15f;

    [Tooltip("How much slope sand can tolerate.")]
    [Range(0f, 45f)]
    public float maxSandSlope = 12f;

    [Header("Grass")]
    [Range(0f, 90f)]
    public float grassMaxSlope = 20f;

    [Header("Dirt")]
    [Range(0f, 90f)]
    public float dirtStartSlope = 25f;

    [Range(0f, 90f)]
    public float dirtFullSlope = 45f;

    [Header("Transitions")]
    [Range(0.01f, 1f)]
    public float transitionSoftness = 0.15f;

    [MenuItem("Tools/Procedural Terrain Painter")]
    public static void ShowWindow()
    {
        GetWindow<ProceduralTerrainPainter>("Terrain Painter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Procedural Terrain Painter", EditorStyles.boldLabel);

        terrain = (Terrain)EditorGUILayout.ObjectField(
            "Terrain",
            terrain,
            typeof(Terrain),
            true
        );

        GUILayout.Space(10);

        grassLayer = (TerrainLayer)EditorGUILayout.ObjectField(
            "Grass Layer",
            grassLayer,
            typeof(TerrainLayer),
            false
        );

        dirtLayer = (TerrainLayer)EditorGUILayout.ObjectField(
            "Dirt Layer",
            dirtLayer,
            typeof(TerrainLayer),
            false
        );

        sandLayer = (TerrainLayer)EditorGUILayout.ObjectField(
            "Sand Layer",
            sandLayer,
            typeof(TerrainLayer),
            false
        );

        GUILayout.Space(10);

        sandHeight = EditorGUILayout.FloatField("Sand Height", sandHeight);
        maxSandSlope = EditorGUILayout.Slider("Max Sand Slope", maxSandSlope, 0f, 45f);

        grassMaxSlope = EditorGUILayout.Slider("Grass Max Slope", grassMaxSlope, 0f, 90f);

        dirtStartSlope = EditorGUILayout.Slider("Dirt Start Slope", dirtStartSlope, 0f, 90f);
        dirtFullSlope = EditorGUILayout.Slider("Dirt Full Slope", dirtFullSlope, 0f, 90f);

        transitionSoftness = EditorGUILayout.Slider(
            "Transition Softness",
            transitionSoftness,
            0.01f,
            1f
        );

        GUILayout.Space(15);

        GUI.enabled = terrain != null &&
                      grassLayer != null &&
                      dirtLayer != null &&
                      sandLayer != null;

        if (GUILayout.Button("GENERATE TERRAIN", GUILayout.Height(40)))
        {
            GenerateTerrain();
        }

        GUI.enabled = true;
    }

    private void GenerateTerrain()
    {
        TerrainData data = terrain.terrainData;

        TerrainLayer[] layers =
        {
            grassLayer,
            dirtLayer,
            sandLayer
        };

        Undo.RecordObject(data, "Generate Procedural Terrain");

        data.terrainLayers = layers;

        int resolution = data.alphamapResolution;

        float[,,] splatmap = new float[
            resolution,
            resolution,
            layers.Length
        ];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float normalizedX = x / (float)(resolution - 1);
                float normalizedY = y / (float)(resolution - 1);

                float height01 = data.GetInterpolatedHeight(
                    normalizedX,
                    normalizedY
                ) / data.size.y;

                float height = data.GetInterpolatedHeight(
                    normalizedX,
                    normalizedY
                );

                float slope = data.GetSteepness(
                    normalizedX,
                    normalizedY
                );

                // -------------------------
                // SAND
                // -------------------------

                float sandHeightWeight =
                    1f - Mathf.Clamp01(height / sandHeight);

                float sandSlopeWeight =
                    1f - Mathf.InverseLerp(
                        maxSandSlope * 0.7f,
                        maxSandSlope,
                        slope
                    );

                float sand = sandHeightWeight * sandSlopeWeight;

                // -------------------------
                // DIRT
                // -------------------------

                float dirt =
                    Mathf.InverseLerp(
                        dirtStartSlope,
                        dirtFullSlope,
                        slope
                    );

                // -------------------------
                // GRASS
                // -------------------------

                float grass =
                    1f - Mathf.InverseLerp(
                        grassMaxSlope * 0.7f,
                        grassMaxSlope,
                        slope
                    );

                // Grass should dominate when reasonably flat.
                // Dirt dominates steep terrain.
                // Sand dominates low, gentle terrain.

                float total = grass + dirt + sand;

                if (total <= 0.001f)
                {
                    grass = 1f;
                    dirt = 0f;
                    sand = 0f;
                    total = 1f;
                }

                splatmap[y, x, 0] = grass / total;
                splatmap[y, x, 1] = dirt / total;
                splatmap[y, x, 2] = sand / total;
            }
        }

        data.SetAlphamaps(0, 0, splatmap);

        EditorUtility.SetDirty(data);

        Debug.Log("Procedural terrain generated!");
    }
}