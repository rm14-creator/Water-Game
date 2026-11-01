using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaterGame.Showcase
{
    // Greybox Character Showcase prototype. All character assignments are design proposals, not canon.
    public class CharacterShowcase : MonoBehaviour
    {
        [Serializable] public class Activity
        {
            public string label;
            public string prop;
            public Activity(string label, string prop) { this.label = label; this.prop = prop; }
        }
        [Serializable] public class Profile
        {
            public string name;
            public string role;
            public Activity[] activities;
            public Profile(string name, string role, params Activity[] activities)
            { this.name = name; this.role = role; this.activities = activities; }
        }

        public static readonly Profile[] Roster =
        {
            new Profile("Kaia Ren", "Survival / exploration (proposed)", new Activity("Clean equipment","Tool"),new Activity("Study holo-map","Screen"),new Activity("Check respirator","Device")),
            new Profile("Meera","Medic",new Activity("Prepare medical tools","Tool"),new Activity("Review diagnostics","Screen"),new Activity("Sort supplies","Crate")),
            new Profile("Nyx","Technical / reconnaissance (proposed)",new Activity("Repair electronics","Tool"),new Activity("Work at terminal","Screen"),new Activity("Calibrate drone","Device")),
            new Profile("Dagna Hallsson","Engineering",new Activity("Weld panel","Tool"),new Activity("Repair defense module","Device"),new Activity("Tune turbine","Device")),
            new Profile("Vesper","Synthetic systems (proposed)",new Activity("Self-diagnostics","Device"),new Activity("Inspect robotics","Tool"),new Activity("Program interface","Screen")),
            new Profile("Nerai","Aquatic research (proposed)",new Activity("Analyze water sample","Device"),new Activity("Tune water tank","Device"),new Activity("Inspect specimen","Tool")),
            new Profile("Thyra","Security (proposed)",new Activity("Inspect armor","Tool"),new Activity("Equipment exercise","Device"),new Activity("Check gear","Crate")),
            new Profile("Onyxa","Energy systems (proposed)",new Activity("Calibrate conductor","Device"),new Activity("Monitor electrical charge","Screen"),new Activity("Tune energy coil","Device")),
            new Profile("Sael","Crystal research (proposed)",new Activity("Examine crystal","Device"),new Activity("Adjust optical sensor","Tool"),new Activity("Tune resonance","Screen")),
            new Profile("SYN-01","Colony administrator",new Activity("Check duty roster","Screen"),new Activity("Run self-maintenance","Device"),new Activity("Review colony terminal","Screen"))
        };

        private int character, activity, outfit, cameraMode;
        private GameObject actor, prop, stage;
        private Camera sceneCamera;
        private float elapsed, orbitAngle;
        private bool playing = true;
        private static readonly string[] Outfits = { "Work", "Casual", "Special" };
        private static readonly string[] Cameras = { "Fixed", "Orbit", "Close-up" };
        private Material actorMaterial;
        private readonly List<GameObject> pieces = new List<GameObject>();
        private GUIStyle titleStyle;

        private void Start()
        {
            character = Mathf.Clamp(PlayerPrefs.GetInt("WGShowcase.Character", 2),0,Roster.Length-1);
            activity = Mathf.Clamp(PlayerPrefs.GetInt("WGShowcase.Activity",0),0,2);
            outfit = Mathf.Clamp(PlayerPrefs.GetInt("WGShowcase.Outfit",0),0,2);
            cameraMode = Mathf.Clamp(PlayerPrefs.GetInt("WGShowcase.Camera",1),0,2);
            playing = PlayerPrefs.GetInt("WGShowcase.Play",1)==1;
            BuildStage();
            RefreshCharacter();
        }

        private Material NewMaterial(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (!shader) shader = Shader.Find("Standard");
            if (!shader) shader = Shader.Find("Sprites/Default");
            var material = new Material(shader);
            material.color = color;
            return material;
        }
        private GameObject Shape(PrimitiveType type, string name, Transform parent, Vector3 pos, Vector3 scale, Color color)
        {
            var obj = GameObject.CreatePrimitive(type);
            obj.name = name;
            obj.transform.SetParent(parent,false);
            obj.transform.localPosition = pos;
            obj.transform.localScale = scale;
            var renderer = obj.GetComponent<Renderer>();
            if (renderer) renderer.material = NewMaterial(color);
            pieces.Add(obj);
            return obj;
        }
        private void BuildStage()
        {
            stage = new GameObject("Showcase Stage (generated)");
            Shape(PrimitiveType.Cube,"Floor",stage.transform,new Vector3(0,-.11f,0),new Vector3(8,.2f,6),new Color(.13f,.18f,.21f));
            Shape(PrimitiveType.Cube,"Back Wall",stage.transform,new Vector3(0,2,2.4f),new Vector3(8,4,.15f),new Color(.09f,.13f,.18f));
            Shape(PrimitiveType.Cube,"Workbench",stage.transform,new Vector3(1,.72f,.5f),new Vector3(1.5f,.12f,.8f),new Color(.31f,.3f,.27f));
            var lightObj = new GameObject("Demo Key Light");
            lightObj.transform.SetParent(stage.transform);
            lightObj.transform.rotation = Quaternion.Euler(48,-35,0);
            var light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional; light.intensity = 1.4f; light.color = new Color(1f,.78f,.57f);
            var existing = Camera.main;
            sceneCamera = existing ? existing : new GameObject("Showcase Camera").AddComponent<Camera>();
            sceneCamera.clearFlags = CameraClearFlags.SolidColor;
            sceneCamera.backgroundColor = new Color(.06f,.09f,.14f);
            sceneCamera.fieldOfView = 45;
        }
        private void RefreshCharacter()
        {
            if (actor) Destroy(actor);
            actor = new GameObject(Roster[character].name + " (GREYBOX — REPLACE WITH CANON PREFAB)");
            actor.transform.position = new Vector3(-.8f,0,0);
            // NOT a visual representation of the approved character identities.
            // Replace the entire actor with each character's approved skinned-mesh prefab.
            var skin = new Color(.68f,.66f,.62f);
            Shape(PrimitiveType.Capsule,"Body placeholder",actor.transform,new Vector3(0,1.05f,0),new Vector3(.62f,1.12f,.38f),skin);
            Shape(PrimitiveType.Sphere,"Head placeholder",actor.transform,new Vector3(0,1.9f,0),new Vector3(.42f,.45f,.40f),skin);
            Shape(PrimitiveType.Capsule,"Left arm",actor.transform,new Vector3(-.42f,1.12f,0),new Vector3(.18f,.85f,.2f),skin);
            Shape(PrimitiveType.Capsule,"Right arm",actor.transform,new Vector3(.42f,1.12f,0),new Vector3(.18f,.85f,.2f),skin);
            Shape(PrimitiveType.Capsule,"Left leg",actor.transform,new Vector3(-.18f,.4f,0),new Vector3(.24f,.80f,.25f),skin);
            Shape(PrimitiveType.Capsule,"Right leg",actor.transform,new Vector3(.18f,.4f,0),new Vector3(.24f,.80f,.25f),skin);
            actorMaterial = Shape(PrimitiveType.Cube,"Outfit placeholder",actor.transform,new Vector3(0,1.10f,-.22f),new Vector3(.65f,.8f,.1f),Color.white).GetComponent<Renderer>().material;
            RefreshOutfit(); RefreshProp();
            elapsed = 0;
        }
        private void RefreshOutfit()
        {
            if (actorMaterial) actorMaterial.color = outfit==0 ? new Color(.75f,.32f,.18f) : outfit==1 ? new Color(.2f,.48f,.62f) : new Color(.58f,.32f,.72f);
        }
        private void RefreshProp()
        {
            if (prop) Destroy(prop);
            prop = new GameObject("Activity prop: "+Roster[character].activities[activity].prop);
            prop.transform.position = new Vector3(.50f,1.06f,-.08f);
            string p = Roster[character].activities[activity].prop;
            Shape(p=="Screen"?PrimitiveType.Cube:p=="Crate"?PrimitiveType.Cube:PrimitiveType.Cylinder,"Greybox prop",prop.transform,Vector3.zero,p=="Screen"?new Vector3(.52f,.38f,.07f):new Vector3(.3f,.21f,.3f),new Color(.28f,.73f,.78f));
            elapsed=0;
        }
        private void Update()
        {
            if (playing) elapsed += Time.deltaTime;
            // Three visually distinct PLACEHOLDER movement loops; final mocap/animation clips not included.
            if (actor)
            {
                float t = elapsed * (activity==0?2f:activity==1?1.2f:1.65f);
                actor.transform.rotation=Quaternion.Euler(0,activity==1?12f*Mathf.Sin(t*.5f):0,activity==2?4f*Mathf.Sin(t):0);
                actor.transform.position=new Vector3(-.8f,(activity==0?.03f:activity==1?.015f:.05f)*Mathf.Sin(t),0);
                if (prop) prop.transform.rotation = Quaternion.Euler(0,activity==2?t*26f:0,0);
            }
            if (!sceneCamera) return;
            if (cameraMode==1 && playing) orbitAngle+=Time.deltaTime*12f;
            float angle=cameraMode==1?orbitAngle:0;
            Vector3 target = new Vector3(-.25f,cameraMode==2?1.55f:1.1f,0);
            float radius = cameraMode==2?2.7f:5.3f;
            sceneCamera.transform.position=target + new Vector3(Mathf.Sin(angle*Mathf.Deg2Rad)*radius, cameraMode==2?.35f:1.3f,-Mathf.Cos(angle*Mathf.Deg2Rad)*radius);
            sceneCamera.transform.LookAt(target);
        }
        private void Persist()
        {
            PlayerPrefs.SetInt("WGShowcase.Character",character);
            PlayerPrefs.SetInt("WGShowcase.Activity",activity);
            PlayerPrefs.SetInt("WGShowcase.Outfit",outfit);
            PlayerPrefs.SetInt("WGShowcase.Camera",cameraMode);
            PlayerPrefs.SetInt("WGShowcase.Play",playing?1:0);
            PlayerPrefs.Save();
        }
        private void OnGUI()
        {
            var rect = new Rect(16,16,Mathf.Min(365,Screen.width-32),Mathf.Min(480,Screen.height-32));
            GUI.Box(rect,"WATER GAME | CHARACTER SHOWCASE");
            GUILayout.BeginArea(new Rect(rect.x+12,rect.y+32,rect.width-24,rect.height-44));
            GUILayout.Label("Greybox demo — NOT approved character art");
            GUILayout.Label("Character: "+Roster[character].name);
            if (GUILayout.Button("Previous / Next Character")) { character=(character+1)%Roster.Length;activity=0;RefreshCharacter();Persist(); }
            GUILayout.Label("Role: "+Roster[character].role);
            GUILayout.Space(9);
            GUILayout.Label("ACTIVITY LOOP");
            for(int i=0;i<3;i++) if(GUILayout.Toggle(activity==i, (i+1)+". "+Roster[character].activities[i].label,"Button") && activity!=i) {activity=i;RefreshProp();Persist();}
            GUILayout.Space(8); GUILayout.Label("OUTFIT (greybox material placeholders)");
            GUILayout.BeginHorizontal();
            for(int i=0;i<3;i++) if(GUILayout.Toggle(outfit==i,Outfits[i],"Button") && outfit!=i) {outfit=i;RefreshOutfit();Persist();}
            GUILayout.EndHorizontal();
            GUILayout.Space(8); GUILayout.Label("CAMERA");
            GUILayout.BeginHorizontal();
            for(int i=0;i<3;i++) if(GUILayout.Toggle(cameraMode==i,Cameras[i],"Button") && cameraMode!=i) {cameraMode=i;Persist();}
            GUILayout.EndHorizontal();
            if(GUILayout.Button(playing?"Pause":"Play")) {playing=!playing;Persist();}
            GUILayout.Label("Replace demo primitives with canonical prefabs + real animation clips.");
            GUILayout.EndArea();
        }
        private void OnDestroy()
        {
            Persist();
            if(actor) Destroy(actor);
            if(prop) Destroy(prop);
            if(stage) Destroy(stage);
        }
    }
}