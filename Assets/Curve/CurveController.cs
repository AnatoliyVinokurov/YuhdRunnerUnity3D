//http://blog.onebyonedesign.com/games/unity3d-endless-runner-part-i-curved-worlds/
// CurveController.cs
//
// Author:
//       Devon O. <devon.o@onebyonedesign.com>
//
// Copyright (c) 2016 Devon O. Wolfgang
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.


using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CurveController : MonoBehaviour
{

	public Transform CurveOrigin;

	[Range(-500f, 500f)]
	[SerializeField]
	float x = 0f;

	[Range(-500f, 500f)]
	[SerializeField]
	float y = 0f;

	[Range(0f, 90f)]
	[SerializeField]
	float falloff = 0f;

	private Vector2 bendAmount = Vector2.zero;

	// Global shader property ids
	private int bendAmountId;
	private int bendOriginId;
	private int bendFalloffId;

	void Start ()
	{
		bendAmountId = Shader.PropertyToID("_BendAmount");
		bendOriginId = Shader.PropertyToID("_BendOrigin");
		bendFalloffId = Shader.PropertyToID("_BendFalloff");
	}

	void Update ()
	{
		bendAmount.x=x;
		bendAmount.y=y;

		Shader.SetGlobalVector(bendAmountId, bendAmount);
		Shader.SetGlobalVector(bendOriginId, CurveOrigin.position);
		Shader.SetGlobalFloat(bendFalloffId, falloff);
	}

}