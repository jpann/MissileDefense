using UnityEngine;
using System.Collections;

[AddComponentMenu("2D Toolkit/Sprite/tk2dClippedSpriteSample")]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class tk2dClippedSpriteSample : tk2dBaseSprite
{
	Mesh mesh;
	Vector2[] meshUvs;
	Vector3[] meshVertices;
	Color[] meshColors;
	int[] meshIndices;
	
	public Vector2 _clipBottomLeft = new Vector2(0, 0);
	public Vector2 _clipTopRight = new Vector2(1, 1);
	
	/// <summary>
	/// Sets the bottom left clip area.
	/// 0, 0 = display full sprite
	/// </summary>
	public Vector2 clipBottomLeft
	{
		get { return _clipBottomLeft; }
		set 
		{ 
			if (value != _clipBottomLeft) 
			{
				_clipBottomLeft = new Vector2(Mathf.Clamp01(value.x), Mathf.Clamp01(value.y));
				Build();
			}
		}
	}
	
	/// <summary>
	/// Sets the top right clip area
	/// 1, 1 = display full sprite
	/// </summary>
	public Vector2 clipTopRight
	{
		get { return _clipTopRight; }
		set 
		{ 
			if (value != _clipTopRight) 
			{
				_clipTopRight = new Vector2(Mathf.Clamp01(value.x), Mathf.Clamp01(value.y));
				Build();
			}
		}
	}
	
	new void Awake()
	{
		base.Awake();
		
		// Create mesh, independently to everything else
		mesh = new Mesh();
		mesh.hideFlags = HideFlags.DontSave;
		GetComponent<MeshFilter>().mesh = mesh;

		// This will not be set when instantiating in code
		// In that case, Build will need to be called
		if (Collection)
		{
			// reset spriteId if outside bounds
			// this is when the sprite collection data is corrupt
			if (_spriteId < 0 || _spriteId >= Collection.Count)
				_spriteId = 0;
			
			Build();
		}
	}
	
	protected void OnDestroy()
	{
		if (mesh)
		{
#if UNITY_EDITOR
			DestroyImmediate(mesh);
#else
			Destroy(mesh);
#endif
		}
	}
	
	new protected void SetColors(Color[] dest)
	{
		Color c = _color;
        if (collectionInst.premultipliedAlpha) { c.r *= c.a; c.g *= c.a; c.b *= c.a; }
		for (int i = 0; i < dest.Length; ++i)
			dest[i] = c;
	}	
	
	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		var sprite = collectionInst.spriteDefinitions[spriteId];
		
		// Only do this when there are exactly 4 polys to a sprite (i.e. the sprite isn't diced, and isnt a more complex mesh)
		if (sprite.positions.Length == 4)
		{
			// This is how the default quad is set up
			// Indices are 0, 3, 1, 2, 3, 0
			
			// 2--------3
			// |        |
			// |        |
			// |        |
			// |        |
			// 0--------1
			
			// index 0 = top left
			// index 3 = bottom right
			
			// clipBottomLeft is the fraction to start from the bottom left (0,0 - full sprite)
			// clipTopRight is the fraction to start from the top right (1,1 - full sprite)
			
			// find the fraction of positions, but fold in the scale multiply as well
			Vector3 bottomLeft = new Vector3(Mathf.Lerp(sprite.positions[0].x, sprite.positions[3].x, _clipBottomLeft.x) * _scale.x,
										  	 Mathf.Lerp(sprite.positions[0].y, sprite.positions[3].y, _clipBottomLeft.y) * _scale.y,
										 	 sprite.positions[0].z * _scale.z);
			Vector3 topRight = new Vector3(Mathf.Lerp(sprite.positions[0].x, sprite.positions[3].x, _clipTopRight.x) * _scale.x,
										   Mathf.Lerp(sprite.positions[0].y, sprite.positions[3].y, _clipTopRight.y) * _scale.y,
										   sprite.positions[0].z * _scale.z);
			
			
			// The z component only needs to be consistent
			meshVertices[0] = new Vector3(bottomLeft.x, bottomLeft.y, bottomLeft.z);
			meshVertices[1] = new Vector3(topRight.x, bottomLeft.y, bottomLeft.z);
			meshVertices[2] = new Vector3(bottomLeft.x, topRight.y, bottomLeft.z);
			meshVertices[3] = new Vector3(topRight.x, topRight.y, bottomLeft.z);
			
			// find the fraction of UV
			// This can be done without a branch, but will end up with loads of unnecessary interpolations
			if (sprite.flipped)
			{
				Vector2 v0 = new Vector2(Mathf.Lerp(sprite.uvs[0].x, sprite.uvs[3].x, _clipBottomLeft.y),
  									     Mathf.Lerp(sprite.uvs[0].y, sprite.uvs[3].y, _clipBottomLeft.x));
				Vector2 v1 = new Vector2(Mathf.Lerp(sprite.uvs[0].x, sprite.uvs[3].x, _clipTopRight.y),
										 Mathf.Lerp(sprite.uvs[0].y, sprite.uvs[3].y, _clipTopRight.x));
				
				meshUvs[0] = new Vector2(v0.x, v0.y);
				meshUvs[1] = new Vector2(v0.x, v1.y);
				meshUvs[2] = new Vector2(v1.x, v0.y);
				meshUvs[3] = new Vector2(v1.x, v1.y);
			}
			else
			{
				Vector2 v0 = new Vector2(Mathf.Lerp(sprite.uvs[0].x, sprite.uvs[3].x, _clipBottomLeft.x),
										 Mathf.Lerp(sprite.uvs[0].y, sprite.uvs[3].y, _clipBottomLeft.y));
				Vector2 v1 = new Vector2(Mathf.Lerp(sprite.uvs[0].x, sprite.uvs[3].x, _clipTopRight.x),
										 Mathf.Lerp(sprite.uvs[0].y, sprite.uvs[3].y, _clipTopRight.y));
	
				meshUvs[0] = new Vector2(v0.x, v0.y);
				meshUvs[1] = new Vector2(v1.x, v0.y);
				meshUvs[2] = new Vector2(v0.x, v1.y);
				meshUvs[3] = new Vector2(v1.x, v1.y);
			}
		}
		else
		{
			// Only supports normal sprites
			for (int i = 0; i < vertices.Length; ++i)
				vertices[i] = Vector3.zero;
		}
	}
	
	public override void Build()
	{
		meshUvs = new Vector2[4];
		meshVertices = new Vector3[4];
		meshColors = new Color[4];
		
		SetGeometry(meshVertices, meshUvs);
		SetColors(meshColors);
		
		if (mesh == null)
		{
			mesh = new Mesh();
			mesh.hideFlags = HideFlags.DontSave;
		}
		else
		{
			mesh.Clear();
		}
		
		mesh.vertices = meshVertices;
		mesh.colors = meshColors;
		mesh.uv = meshUvs;
		mesh.triangles = new int[6] { 0, 3, 1, 2, 3, 0 };
		mesh.RecalculateBounds();
		
		GetComponent<MeshFilter>().mesh = mesh;
		
		UpdateMaterial();
	}
	
	protected override void UpdateGeometry() { UpdateGeometryImpl(); }
	protected override void UpdateColors() { UpdateColorsImpl(); }
	protected override void UpdateVertices() { UpdateGeometryImpl(); }
	
	
	protected void UpdateColorsImpl()
	{
#if UNITY_EDITOR
		// This can happen with prefabs in the inspector
		if (meshColors == null || meshColors.Length == 0)
			return;
#endif
		
		SetColors(meshColors);
		mesh.colors = meshColors;
	}

	protected void UpdateGeometryImpl()
	{
#if UNITY_EDITOR
		// This can happen with prefabs in the inspector
		if (mesh == null)
			return;
#endif
		SetGeometry(meshVertices, meshUvs);
		mesh.vertices = meshVertices;
		mesh.uv = meshUvs;
		mesh.RecalculateBounds();
	}
	
	protected override void UpdateMaterial()
	{
		if (renderer.sharedMaterial != collectionInst.spriteDefinitions[spriteId].material)
			renderer.material = collectionInst.spriteDefinitions[spriteId].material;
	}
	
	protected override int GetCurrentVertexCount()
	{
#if UNITY_EDITOR
		if (meshVertices == null)
			return 0;
#endif
		return 4;
	}
}
