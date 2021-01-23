﻿using Camera2.Behaviours;
using Camera2.Interfaces;
using Camera2.Utils;
using System;
using UnityEngine;

namespace Camera2.Configuration {
	class Settings_NoodleExtensions {
		public bool moveWithMap = true;
		public bool autoOpaqueWalls = false;
	}
}

namespace Camera2.Middlewares {
	class NoodleExtensions : CamMiddleware, IMHandler {
		private bool didAttach = false;
		//public static Transform currentWorldTransform { get; private set; }
		public new bool Pre() {
			// We wanna parent FP cams as well so that the noodle translations are applied instantly and dont get smoothed out by SmoothFollow
			if(enabled && settings.NoodleExtensions.moveWithMap && !SceneUtil.isInMenu && cam.settings.type != Configuration.CameraType.Attached) {
				// If we are not yet attached, and we dont have a parent thats active yet, try to get one!
				if(!didAttach && SceneUtil.songWorldTransform != null) {
					didAttach = true;
					cam.SetParent(SceneUtil.songWorldTransform);
				}
			} else {
				didAttach = false;
				cam.SetParent(null);
			}
			return true;
		}

		/*
		 * This gets called when we are leaving a song because otherwise any game object attached
		 * to the origin would get destroyed in the process of the origin being destroyed
		 */
		public static void ForceDetachTracks() {
			foreach(var cam in CamManager.cams.Values) {
				if(cam.settings.type == Configuration.CameraType.Attached)
					continue;

				cam.SetParent(null);
			}
		}
	}
}
