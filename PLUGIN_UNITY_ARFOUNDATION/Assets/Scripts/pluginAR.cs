#if (UNITY_EDITOR) 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;
using UnityEngine.XR.ARSubsystems;
using UnityEditor.XR.ARFoundation;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SpatialTracking;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class pluginAR : MonoBehaviour
{

    public static bool arPlaneDetection;
    public static bool arCloudDetection;

    private GameObject _openButton;
    private GameObject _exitButton;
    private Sprite _backgroundSprite;
    private Sprite _iconSprite;
    private Sprite _exitSprite;
    private Texture _indicatorTexture;
    private Texture _particleTexture;
    private Material _customLine;
    private Material _customPlane;
    private Material _customPoint;
    private Material _customTracker;
    private GameObject refCanvas;
    private string _Path;
    private ARSessionOrigin m_arSess;
    private ARPlaneManager m_arPlaneManager;
    private ARPointCloudManager m_arPointCloud;


    public void CreateFolder()
    {

        if (AssetDatabase.IsValidFolder("Assets/Editor"))
        {
            Debug.Log("Your Folders already exists !"); ;
        }
        else
        {
            AssetDatabase.CreateFolder("Assets", "Editor");
            AssetDatabase.CreateFolder("Assets", "Scripts");
            AssetDatabase.CreateFolder("Assets/Editor", "Resources");
            AssetDatabase.CreateFolder("Assets/Editor/Resources", "3D");
            AssetDatabase.CreateFolder("Assets/Editor/Resources", "2D");
            AssetDatabase.CreateFolder("Assets/Editor/Resources", "ANIMATIONS");
            AssetDatabase.CreateFolder("Assets/Editor/Resources", "MATERIALS");

            AssetDatabase.MoveAsset("Assets/pluginAR.cs", "Assets/Scripts/pluginAR.cs");
            AssetDatabase.MoveAsset("Assets/pluginAR.meta", "Assets/Scripts/pluginAR.meta");
            AssetDatabase.MoveAsset("Assets/pluginAREditor.cs", "Assets/Scripts/pluginAREditor.cs");
            AssetDatabase.MoveAsset("Assets/pluginAREditor.meta", "Assets/Scripts/pluginAREditor.meta");
            AssetDatabase.MoveAsset("Assets/PlacementIndicator.cs", "Assets/Scripts/PlacementIndicator.cs");
            AssetDatabase.MoveAsset("Assets/PlacementIndicator.meta", "Assets/Scripts/PlacementIndicator.meta");
            AssetDatabase.MoveAsset("Assets/TapToPlace.cs", "Assets/Scripts/TapToPlace.cs");
            AssetDatabase.MoveAsset("Assets/TapToPlace.meta", "Assets/Scripts/TapToPlace.meta");
            AssetDatabase.MoveAsset("Assets/InteractManager.cs", "Assets/Scripts/InteractManager.cs");
            AssetDatabase.MoveAsset("Assets/InteractManager.meta", "Assets/Scripts/InteractManager.meta");

            AssetDatabase.MoveAsset("Assets/_TapToPlace.png", "Assets/Editor/Resources/2D/_TapToPlace.png");
            AssetDatabase.MoveAsset("Assets/_Particle.png", "Assets/Editor/Resources/2D/_Particle.png");
            AssetDatabase.MoveAsset("Assets/_Background.png", "Assets/Editor/Resources/2D/_Background.png");
            AssetDatabase.MoveAsset("Assets/_Icon.png", "Assets/Editor/Resources/2D/_Icon.png");

            AssetDatabase.MoveAsset("Assets/_3DModel.obj", "Assets/Editor/Resources/3D/_3DModel.obj");

            AssetDatabase.Refresh();

        }
    }

    public void SceneMenu()
    {
     
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
    
        EditorSceneManager.SaveScene(newScene,"Assets/Scenes/Menu.unity", true);
        Scene sceneRef = EditorSceneManager.OpenScene("Assets/Scenes/Menu.unity", OpenSceneMode.Single);

        GameObject scriptManager = new GameObject("ScriptManager");
        scriptManager.AddComponent<pluginAR>();
        scriptManager.AddComponent<InteractManager>();
     
        EditorSceneManager.SaveScene(sceneRef);

        GameObject _eventSys = new GameObject("EventSystem");
        _eventSys.AddComponent<EventSystem>();
        _eventSys.AddComponent<StandaloneInputModule>();
        _eventSys.tag = "GameController";
    }

    public void CreateBackground()
    {
        refCanvas = new GameObject("MainCanvas");
        Canvas c = refCanvas.AddComponent<Canvas>();

        c.renderMode = RenderMode.ScreenSpaceOverlay;

        refCanvas.AddComponent<CanvasScaler>();
        refCanvas.AddComponent<GraphicRaycaster>();

        GameObject _background = new GameObject("background");
        _background.AddComponent<CanvasRenderer>();
        _background.AddComponent<Image>();

        _background.transform.SetParent(refCanvas.transform, false);

        _background.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        _background.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        _background.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        _background.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

        _backgroundSprite = Resources.Load<Sprite>("2D/_Background");
        _background.GetComponent<Image>().sprite = _backgroundSprite;

       
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(),"Assets/Scenes/Menu.unity", true);

    }

    public void CreateOpenButton()
    {
        GameObject button = new GameObject("OpenScene");

        button.AddComponent<Image>();
        button.AddComponent<Button>();
        button.tag = "Player";

        button.GetComponent<Image>().preserveAspect = true;
        button.transform.SetParent(refCanvas.transform, false);


        button.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        button.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        button.GetComponent<RectTransform>().anchorMin = new Vector2(0.437f, 0.404f);
        button.GetComponent<RectTransform>().anchorMax = new Vector2(0.56f, 0.604f);

        _iconSprite = Resources.Load<Sprite>("2D/_Icon");
        button.GetComponent<Image>().sprite = _iconSprite;


        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/Scenes/Menu.unity", true);

    }

    public void ExitButton()
    {
        GameObject button = new GameObject("ExitApp");
        button.AddComponent<Image>();
        button.AddComponent<Button>();
        button.tag = "Respawn";

        button.GetComponent<Image>().preserveAspect = true;
        button.transform.SetParent(refCanvas.transform, false);

        button.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        button.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        button.GetComponent<RectTransform>().anchorMin = new Vector2(0.437f, 0.104f);
        button.GetComponent<RectTransform>().anchorMax = new Vector2(0.56f, 0.288f);

        _exitSprite = Resources.Load<Sprite>("2D/_TapToPlace");
        button.GetComponent<Image>().sprite = _exitSprite;
        button.GetComponent<Image>().color = new Color32(255,0,90,255);

        Button btn = _openButton.GetComponent<Button>();
       

        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/Scenes/Menu.unity", true);
    }

    public void ARScene()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/Scenes/Menu.unity", true);

        if (arCloudDetection == false && arPlaneDetection == false) 
        {
            Debug.Log("Choose Tracking Methode !");
        }
        else
        {
          
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            EditorSceneManager.SaveScene(newScene,"Assets/Scenes/ARScene.unity", true);

            Scene sceneRef = EditorSceneManager.OpenScene("Assets/Scenes/ARScene.unity", OpenSceneMode.Single);

            GameObject _directionLight = new GameObject("Directional light");
            _directionLight.AddComponent<Light>();
            _directionLight.GetComponent<Light>().type = LightType.Directional;
            _directionLight.transform.Rotate(50, -30, 0);
            GameObject scriptManager = new GameObject("ScriptManager");
            scriptManager.AddComponent<pluginAR>();

            GameObject _ARsession = new GameObject("ARsession");
            _ARsession.AddComponent<ARSession>();
            _ARsession.AddComponent<ARInputManager>();

            GameObject _ARsessionOrigin = new GameObject("AR Session Origin");
            _ARsessionOrigin.tag = "GameController";
            _ARsessionOrigin.AddComponent<ARSessionOrigin>();
            _ARsessionOrigin.AddComponent<ARPlaneManager>();
            _ARsessionOrigin.AddComponent<ARPointCloudManager>();
            _ARsessionOrigin.AddComponent<TapToPlace>();
            _ARsessionOrigin.AddComponent<InteractManager>();

            _ARsessionOrigin.GetComponent<TapToPlace>()._objectToPlace = Resources.Load("3D/_3DModel") as GameObject;

            GameObject _ARcamera = new GameObject("AR Camera");
            _ARcamera.tag = "MainCamera";
            _ARcamera.AddComponent<Camera>();
            _ARcamera.AddComponent<TrackedPoseDriver>();
            _ARcamera.AddComponent<ARCameraManager>();
            _ARcamera.AddComponent<ARCameraBackground>();
            _ARcamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
            _ARcamera.GetComponent<Camera>().farClipPlane = 20;
            _ARcamera.GetComponent<Camera>().nearClipPlane = 0.1f;
            _ARcamera.GetComponent<TrackedPoseDriver>().SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRDevice, TrackedPoseDriver.TrackedPose.ColorCamera);
            _ARcamera.GetComponent<Camera>().backgroundColor = Color.black;

            _ARcamera.transform.SetParent(_ARsessionOrigin.transform, false);

            Material _customDebugBorder = new Material(Shader.Find("Standard"));
            Material _customDebugPlane = new Material(Shader.Find("Standard"));
            Material _customDebugPoint = new Material(Shader.Find("Standard"));

            AssetDatabase.CreateAsset(_customDebugBorder, "Assets/Editor/Resources/MATERIALS/customDebugBorder.mat");
            AssetDatabase.CreateAsset(_customDebugPlane, "Assets/Editor/Resources/MATERIALS/PlaneRenderer.mat");
            AssetDatabase.CreateAsset(_customDebugPoint, "Assets/Editor/Resources/MATERIALS/PointRenderer.mat");

            _customLine = Resources.Load("MATERIALS/customDebugBorder") as Material;
            _customLine.color = new Color32(80, 80, 80, 255);
            _customLine.SetFloat("_Mode", 3);

            _customPlane = Resources.Load("MATERIALS/PlaneRenderer") as Material;
            _customPlane.SetFloat("_Mode", 3);
            _customPlane.color = new Color32(5, 5, 5, 5);

            _customPoint = Resources.Load("MATERIALS/PointRenderer") as Material;
            _customPoint.SetFloat("_Mode", 1);
            _customPoint.color = new Color(255, 210, 0, 50);
            _particleTexture = Resources.Load("2D/_Particle") as Texture;
            _customPoint.mainTexture = _particleTexture;

            if (arPlaneDetection == true)
            {

                GameObject _ARplane = new GameObject("AR Default Plane");
           
                _ARplane.AddComponent<ARPlane>();
                _ARplane.AddComponent<ARPlaneMeshVisualizer>();
                _ARplane.AddComponent<MeshCollider>();
                _ARplane.AddComponent<MeshFilter>();
                _ARplane.AddComponent<MeshRenderer>();
                _ARplane.AddComponent<LineRenderer>();
                _ARplane.GetComponent<LineRenderer>().startWidth = 0.010F;
                _ARplane.GetComponent<LineRenderer>().receiveShadows = false;
                _ARplane.GetComponent<LineRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                _ARplane.GetComponent<LineRenderer>().material = _customLine;
                _ARplane.GetComponent<LineRenderer>().loop = true;
                _ARplane.GetComponent<LineRenderer>().numCornerVertices = 4;
                _ARplane.GetComponent<LineRenderer>().numCapVertices = 4;
                _ARplane.GetComponent<LineRenderer>().useWorldSpace = false;
                _ARplane.GetComponent<MeshRenderer>().material = _customPlane;

                m_arPlaneManager = FindObjectOfType<ARPlaneManager>();
                m_arPlaneManager.planePrefab = FindObjectOfType<ARPlane>().gameObject;

            }

            if (arCloudDetection == true)
            {
               

                GameObject _ARPointCloud = new GameObject("AR Default Point Cloud");
             
                _ARPointCloud.AddComponent<ARPointCloud>();
                _ARPointCloud.AddComponent<ARPointCloudParticleVisualizer>();
                _ARPointCloud.AddComponent<ParticleSystem>();
                _ARPointCloud.GetComponent<ParticleSystem>().startSize = 0.01f;
                _ARPointCloud.GetComponent<ParticleSystem>().playOnAwake = false;
                _ARPointCloud.GetComponent<ParticleSystem>().loop = false;
                _ARPointCloud.GetComponent<ParticleSystem>().scalingMode = ParticleSystemScalingMode.Hierarchy;
                _ARPointCloud.GetComponent<ParticleSystem>().startColor = new Color(253, 184, 19, 255);
                _ARPointCloud.GetComponent<ParticleSystem>().enableEmission = false;
                var _paramShape = _ARPointCloud.GetComponent<ParticleSystem>().shape;
                var _paramEmission = _ARPointCloud.GetComponent<ParticleSystem>().emission;
                _paramEmission.enabled = false;
                _paramShape.enabled = false;
                _ARPointCloud.GetComponent<ParticleSystemRenderer>().material = _customPoint;

                m_arPointCloud = FindObjectOfType<ARPointCloudManager>();
                m_arPointCloud.pointCloudPrefab = FindObjectOfType<ARPointCloud>().gameObject;

            }


            GameObject _placementIndicator = new GameObject("Placement Indicator");
            GameObject _placementRef = GameObject.CreatePrimitive(PrimitiveType.Quad);
            _placementIndicator.AddComponent<PlacementIndicator>();

            _placementRef.transform.Rotate(90.0f, 0f, 0f);
            _placementRef.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);


            Material _imageRef = new Material(Shader.Find("Standard"));
            AssetDatabase.CreateAsset(_imageRef, "Assets/Editor/Resources/MATERIALS/TrackingMaterial.mat");
            _customTracker = Resources.Load("MATERIALS/TrackingMaterial") as Material;
            _indicatorTexture = Resources.Load("2D/_TapToPlace") as Texture;
            _customTracker.mainTexture = _indicatorTexture;
            _customTracker.SetFloat("_Mode", 1);
            _customTracker.color = new Color(255, 255, 0, 255);

            _placementRef.GetComponent<Renderer>().material = _imageRef;
            _placementRef.transform.SetParent(_placementIndicator.transform, false);

            m_arSess = FindObjectOfType<ARSessionOrigin>();
            m_arSess.camera = FindObjectOfType<Camera>();

            AssetDatabase.Refresh();
            EditorSceneManager.SaveScene(sceneRef);
        }
    }


    public void AnchorButton()
    {
        GameObject _eventSys = new GameObject("EventSystem");
        _eventSys.AddComponent<EventSystem>();
        _eventSys.AddComponent<StandaloneInputModule>();

        refCanvas = new GameObject("MainCanvas");
        Canvas c = refCanvas.AddComponent<Canvas>();

        c.renderMode = RenderMode.ScreenSpaceOverlay;

        refCanvas.AddComponent<CanvasScaler>();
        refCanvas.AddComponent<GraphicRaycaster>();

        GameObject button = new GameObject("AnchorAR");
        button.AddComponent<Image>();
        button.AddComponent<Button>();
        button.tag = "Finish";

        button.transform.SetParent(refCanvas.transform, false);

        button.GetComponent<Image>().preserveAspect = true;
        button.transform.SetParent(refCanvas.transform, false);

        button.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        button.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        button.GetComponent<RectTransform>().anchorMin = new Vector2(0.437f, 0.104f);
        button.GetComponent<RectTransform>().anchorMax = new Vector2(0.56f, 0.288f);

        _exitSprite = Resources.Load<Sprite>("2D/_TapToPlace");
        button.GetComponent<Image>().sprite = _exitSprite;
        button.GetComponent<Image>().color = new Color32(255, 0, 90, 255);

 


        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/Scenes/ARScene.unity", true);
    }
}

#endif