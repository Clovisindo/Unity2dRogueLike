using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utilities 
{
    public static GameObject FindObjectWithTag(Transform transform, string _tag)
    {
        Transform parent = transform;
       return GetChildObject(parent, _tag);
    }

    public static GameObject GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                return child.gameObject;
            }
            if (child.childCount > 0)
            {
                return GetChildObject(child, _tag);
            }
        }
        return null;
    }

    public static GameObject[] GetChildsObject(Transform parent, string _tag)
    {
        List<GameObject> listChilds = new List<GameObject>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                listChilds.Add(child.gameObject);
            }
        }
        return listChilds.ToArray();
    }

    public static  List<T> getAllChildsObject<T> (Transform parent) where T : Component
    {
        List<T> listChilds = new List<T>();
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            listChilds.Add(child.GetComponent<T>());
        }
        if (listChilds.Count != 0)
        {
            return listChilds;
        }
        else
        {
            return null;
        }
        
    }

    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}

public static class Helper 
{
    /// <summary>
    /// Devuelve el componente deseado del objeto etiquetado 
    /// </summary>
    /// <typeparam name="T">Tipo de componentes que se quiere devolver</typeparam>
    /// <param name="parent"> objeto padre</param>
    /// <param name="tag">patron de busqueda</param>
    /// <returns>Componente especificado</returns>
    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }

    public static T[] FindComponentsInChildWithTag<T>(this GameObject parent, string tag) where T : Component
    {
        List<T> arrayChildItems = new List<T>();
        Transform t = parent.transform;
        foreach (Transform tr in t)
        {
            if (tr.tag == tag)
            {
                arrayChildItems.Add(tr.GetComponent<T>());
            }
        }
        return arrayChildItems.ToArray();
    }
}
