using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Xml.Serialization;
using UnityEngine.Events;

public static class Utility
{

    public static bool IsOutsideScreen(Vector3 position)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(position);
        if (viewportPos.x < 0.0f || 1.0f < viewportPos.x ||
            viewportPos.y < 0.0f || 1.0f < viewportPos.y)
        {
            return true;
        }

        return false;
    }

    public static IEnumerator EntityLeftScreen(UnityAction action, Transform entity)
    {
        while (true)
        {
            if(IsOutsideScreen(entity.position))
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        action();
    }
    public static IEnumerator DelaySeconds(UnityAction action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public static float GetMaxDuration(this Component self)
    {
        ParticleSystem[] particles = self.GetComponentsInChildren<ParticleSystem>();
        AudioSource[] sounds = self.GetComponentsInChildren<AudioSource>();

        float maxDuration = float.MinValue; 

        foreach (var particle in particles)
        {
            if(particle.main.duration > maxDuration)
            {
                maxDuration = particle.main.duration;
            }
        }

        foreach (var sound in sounds)
        {
            if(sound.clip.length > maxDuration)
            {
                maxDuration = sound.clip.length;
            }
        }

        return maxDuration;
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        if (min > max)
        {
            if (angle > min || angle < max) return angle;
            return angle > (min + max) / 2f ? min : max;
        }
        if (angle > min && angle < max) return angle;
        return angle < min ? min : max;
    }
    public static void SaveData<T>(T data, string filePath)
    {
        var serializer = new XmlSerializer(typeof(T));
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(stream, data);
        }
    }
    public static T LoadData<T>(string filePath)
    {
        if (File.Exists(filePath))
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
        return default;
    }
    public static Dictionary<string, T> LoadDatabase<T>(string path) where T : Object
    {
        Dictionary<string, T> database = new Dictionary<string, T>();
        T[] collection = Resources.LoadAll<T>(path) as T[];
        foreach (var item in collection)
        {
            database.Add(item.name, item);
        }
        return database;
    }
}