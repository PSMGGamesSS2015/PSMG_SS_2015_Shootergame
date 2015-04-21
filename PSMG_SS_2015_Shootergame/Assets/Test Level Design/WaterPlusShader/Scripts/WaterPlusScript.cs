using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public enum WaterMovementType {
	directional = 0,
	island,
	flowmap,
	still
}
/*
public enum WaterQualityLevel {
	fastest = 0,
	fast,
	simple,
	good,
	beautiful,
	fantastic,
	automatic
}*/

public class WaterPlusScript : MonoBehaviour {
	//public WaterQualityLevel waterQualityLevel = WaterQualityLevel.automatic;
	public WaterMovementType movementType = WaterMovementType.directional;
	
	public Vector2 velocity;	//In world units
	public float speed;	//In world units
	
	//public float tideAnimationSpeed;
	//public float tideAmplitude;
	
	public Transform target;
	
	//public bool isTargetingMobile;
	
	//private float yAnimationValue;
	private float animationValue;

	//private float initialTransformY;
	
	private Vector3 waterCenter;
	private Material waterMaterial;
	
	private Vector3 projectedLightDir;
	
	private Vector2 anisoDirAnimationOffset;
	
	void Reset() {
		speed = 3.0f;
		velocity = new Vector2(0.7f, 0.0f);
		//tideAnimationSpeed = 0.5f;
		//tideAmplitude = 0.5f;
		
		//isTargetingMobile = false;
	}
	
	private Light FindTheBrightestDirectionalLight() {
		//Find the brightest directional light
		Light resultLight = null;
		
		Light[] lights = GameObject.FindObjectsOfType( typeof(Light) ) as Light[];
		List<Light> directionalLights = new List<Light>();
		
		foreach (Light light in lights) {
			if (light.type == LightType.Directional)
				directionalLights.Add( light );
		}
		
		if (directionalLights.Count <= 0)
			return null;
		
		resultLight = directionalLights[0];
		
		foreach (Light light in directionalLights) {
			if (light.intensity > resultLight.intensity)
				resultLight = light;
		}
		
		return resultLight;
	}
	
	// Use this for initialization
	void Start () {
		//QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
		
		//Make sure that the editor's updater won't run at runtime
		//WaterPlusScriptEditor editorScript = gameObject.GetComponent<WaterPlusScriptEditor>();
		//if (editorScript)
		//	Destroy(editorScript);
		
		//yAnimationValue = 0.0f;
		//initialTransformY = transform.position.y - tideAmplitude;
		
		waterCenter = GetComponent<Renderer>().bounds.center;
		
		if (movementType == WaterMovementType.directional)
			speed = velocity.magnitude;
		
		//Convert to tiles/second from world_units/second
		float tileWidth = GetComponent<Renderer>().bounds.size.x / gameObject.GetComponent<Renderer>().material.GetTextureScale("_MainTex").x;
		speed = speed / tileWidth;
		
		waterMaterial = GetComponent<Renderer>().material;
		
		//Pro only
//		if (Camera.main)
//				Camera.main.depthTextureMode |= DepthTextureMode.Depth;
		
		/*if (waterQualityLevel == WaterQualityLevel.automatic) {
			if (isTargetingMobile)
				waterQualityLevel = WaterQualityLevel.simple;
			else
				waterQualityLevel = WaterQualityLevel.beautiful;	
		}*/
		
		
		Shader.DisableKeyword("WATER_EDGEBLEND_OFF");	Shader.EnableKeyword("WATER_EDGEBLEND_ON");
		
		if (movementType == WaterMovementType.flowmap) {
			Shader.DisableKeyword("FLOWMAP_ANIMATION_OFF");	Shader.EnableKeyword("FLOWMAP_ANIMATION_ON");
			//speed *= 10.0f;	//Account for flowmap strength
			FlowmapAnimator fmAnimatior = gameObject.AddComponent<FlowmapAnimator>();
			fmAnimatior.flowSpeed = speed;
		} else {
			Shader.DisableKeyword("FLOWMAP_ANIMATION_ON");	Shader.EnableKeyword("FLOWMAP_ANIMATION_OFF");
		}
		
		Light dirLight = FindTheBrightestDirectionalLight();
		
		/*if (dirLight != null) {
			Vector3 anisoLightPos = dirLight.transform.position;
			
			renderer.material.SetVector("anisoLightPos", new Vector4(anisoLightPos.x, anisoLightPos.y, anisoLightPos.z, 0.0f) );
		}*/
		
		projectedLightDir = dirLight.transform.forward - transform.up * Vector3.Dot( transform.up, dirLight.transform.forward );
		projectedLightDir.Normalize();
		
		anisoDirAnimationOffset = Vector2.zero;
	}
	
	void OnDestroy() {
		Shader.DisableKeyword("WATER_EDGEBLEND_ON");	Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
	}
	
	private int causticsAnimationFrame = 0;
	private float causticsAnimationTime = 0.0f;
	
	// Update is called once per frame
	void Update () {
		switch (movementType) {
			case WaterMovementType.island: {
				Vector3 tempDirection = (waterCenter - target.position);
				velocity.x = tempDirection.x; velocity.y = tempDirection.z;
				velocity = velocity.normalized * speed;
				break;
			}
			
			case WaterMovementType.still:
				velocity = Vector3.zero;
				break;
			
			default: case WaterMovementType.directional:
				
				break;
		}
		
		if (movementType == WaterMovementType.directional | movementType == WaterMovementType.island) {
			//
			//Animation in XZ-plane
			Vector2 previousOffset = waterMaterial.GetTextureOffset("_MainTex");
			Vector2 currentOffset = previousOffset + velocity * Time.deltaTime;
			
			//Limit speed - this is to account for an awkward water movement when moving at large speeds.
			if ( (velocity * Time.deltaTime).sqrMagnitude > 1.0f ) {
				//Debug.Log("Offset is bigger than tile: " + (currentOffset - previousOffset).magnitude + " tile: " + 1.0f);
				Vector2 deltaOffset = velocity * Time.deltaTime;
				
				Vector2 offsetDirection = deltaOffset.normalized;
				
				while ( deltaOffset.sqrMagnitude > 1.0f ) {
					deltaOffset -= offsetDirection;
				}
				
				currentOffset = previousOffset + deltaOffset;
			}
			
			waterMaterial.SetTextureOffset("_MainTex", currentOffset );
			waterMaterial.SetTextureOffset("_Normalmap", currentOffset );
			
			//Y-axis animation
			//Water goes upwards faster than downwards.
			
			/*float deltaAnimation;
			//Derivative, are we going upwards or downwards?
			if ( Mathf.Cos(animationValue) >= 0.0f) {
				animationValue += Time.deltaTime * 1.5f * tideAnimationSpeed;
				deltaAnimation = Mathf.Cos(animationValue) * Time.deltaTime * 1.5f * tideAnimationSpeed;
			} else {
				animationValue += Time.deltaTime * tideAnimationSpeed;
				deltaAnimation = Mathf.Cos(animationValue) * Time.deltaTime * tideAnimationSpeed;
			}
			
			yAnimationValue += deltaAnimation;*/
			
			//Vector3 position = transform.position;
			
			//transform.position = new Vector3(position.x, initialTransformY + yAnimationValue * tideAmplitude, position.z);
			
			//Debug.Log("yAnimationValue * yAnimationThreshold: " + yAnimationValue * yAmplitude);
			//waterMaterial.SetFloat("_yOffset", yAnimationValue * tideAmplitude * 0.5f / 25.0f);	//Normalized, 0..1
		}
		
		anisoDirAnimationOffset += ( new Vector2(projectedLightDir.x, projectedLightDir.z) ) * Time.deltaTime * .01f;
		
		/*while (anisoDirAnimationOffset.x > 1.0f) {
			anisoDirAnimationOffset.x -= 1.0f;
		}
		
		while (anisoDirAnimationOffset.x < 0.0f) {
			anisoDirAnimationOffset.x += 1.0f;
		}
		
		while (anisoDirAnimationOffset.y > 1.0f) {
			anisoDirAnimationOffset.y -= 1.0f;
		}
		
		while (anisoDirAnimationOffset.y < 0.0f) {
			anisoDirAnimationOffset.y += 1.0f;
		}*/
		
		Vector4 anisoDirAnimation = new Vector4(anisoDirAnimationOffset.x, anisoDirAnimationOffset.y, 0.0f, 0.0f);
		
		//Debug.Log("anisoDirAnimation: " + anisoDirAnimation);
		
		waterMaterial.SetVector( "anisoDirAnimationOffset", anisoDirAnimation );
		
		//
		//Caustics animation
		
		//i = z * 16 + y * 4 + x;
		int causticsColorChannel = causticsAnimationFrame / 16;
		float causticsYOffset = (float) ( (causticsAnimationFrame % 16) / 4 ) * .25f;
		float causticsXOffset = (float) ( (causticsAnimationFrame % 16) % 4 ) * .25f;
		
		
		Vector4 causticsAnimation = new Vector4(causticsXOffset, causticsYOffset, 0.25f, 0.25f);
		Vector4 causticsAnimationColorChannel;
		
		switch (causticsColorChannel) {
		default: case 0:
			causticsAnimationColorChannel = new Vector4(1.0f, 0.0f, 0.0f, 0.0f); 
			break;
			
		case 1:
			causticsAnimationColorChannel = new Vector4(0.0f, 1.0f, 0.0f, 0.0f); 
			break;
			
		case 2:
			causticsAnimationColorChannel = new Vector4(0.0f, 0.0f, 1.0f, 0.0f); 
			break;
		}
		
		waterMaterial.SetVector( "causticsOffsetAndScale", causticsAnimation );
		waterMaterial.SetVector( "causticsAnimationColorChannel", causticsAnimationColorChannel );
		
		causticsAnimationTime += Time.deltaTime;
		if (causticsAnimationTime >= .04f) {
			//Debug.Log("caustics animation frame: " + causticsAnimationFrame + "; (" + causticsXOffset + "; " + causticsYOffset + "; " + causticsColorChannel + ")" );
			causticsAnimationFrame++;
			causticsAnimationTime = .0f;
			
			if (causticsAnimationFrame >= 48)
				causticsAnimationFrame = 0;
		}
		
		
		
		//float refractiveIndex = 1.333f;
		
		//float fresnel0 = (1.0f - refractiveIndex) * (1.0f - refractiveIndex) / ( (1.0f + refractiveIndex) );
		
		//waterMaterial.SetFloat("_fresnel0", fresnel0);
	}
	
	/*int framesCounted = 0;
	float totalDeltaTime = 0.0f;
	
	
	void LateUpdate() {
		if (framesCounted >= 100) {
			//Debug.Log("FPS: " + ( 1.0f / (totalDeltaTime / (float) framesCounted) ) );
			framesCounted = 0;
			totalDeltaTime = 0.0f;
		}
		
		framesCounted++;
		totalDeltaTime += Time.deltaTime;
	}*/
}
