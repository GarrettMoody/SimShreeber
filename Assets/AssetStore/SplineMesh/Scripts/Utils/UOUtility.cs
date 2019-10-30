using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace SplineMesh {
    public static class UOUtility {
        public static GameObject Create(string name, UnityEngine.GameObject parent, params Type[] components) {
            var res = new UnityEngine.GameObject(name, components);
            res.transform.parent = parent.transform;
            res.transform.localPosition = Vector3.zero;
            res.transform.localScale = Vector3.one;
            res.transform.localRotation = Quaternion.identity;
            return res;
        }

        public static GameObject Instantiate(UnityEngine.GameObject prefab, Transform parent) {
            var res = UnityEngine.Object.Instantiate(prefab, parent);
            res.transform.localPosition = Vector3.zero;
            res.transform.localRotation = Quaternion.identity;
            res.transform.localScale = Vector3.one;
            return res;
        }

        public static void Destroy(UnityEngine.GameObject go) {
            if (Application.isPlaying) {
                UnityEngine.Object.Destroy(go);
            } else {
                UnityEngine.Object.DestroyImmediate(go);
            }
        }

        public static void Destroy(Component comp) {
            if (Application.isPlaying) {
                UnityEngine.Object.Destroy(comp);
            } else {
                UnityEngine.Object.DestroyImmediate(comp);
            }
        }

        public static void DestroyChildren(UnityEngine.GameObject go) {
            var childList = go.transform.Cast<Transform>().ToList();
            foreach (Transform childTransform in childList) {
                Destroy(childTransform.gameObject);
            }
        }
    }
}
