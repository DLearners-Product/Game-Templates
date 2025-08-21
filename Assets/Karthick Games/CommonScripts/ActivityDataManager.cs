using System.Collections.Generic;
using UnityEngine;
using System;



public class ActivityDataManager : MonoGenericSingleton<ActivityDataManager>
{

    // Dictionary: key = activity name, value = list of questions
    private Dictionary<string, List<QuestionData>> activityData = new Dictionary<string, List<QuestionData>>();


    // Record a question answer for a specific activity
    public void RecordAnswer(string activityName, string question, string selectedOption, bool isCorrect, int attempts)
    {
        if (!activityData.TryGetValue(activityName, out var list))
        {
            list = new List<QuestionData>();
            activityData[activityName] = list;
        }

        list.Add(new QuestionData
        {
            question = question,
            selectedOption = selectedOption,
            isCorrect = isCorrect,
            attempts = attempts
        });
    }

    // Save all activities to JSON and download
    public void SaveAllActivitiesJSON()
    {
        // Convert dictionary to a serializable object
        var jsonString = JsonUtility.ToJson(new SerializableDictionary(activityData), true);
        WebGLDownloader.SaveJSON(jsonString, "AllActivities.json");
    }


    public void SaveToLocalWebStorage()
    {
        // Convert dictionary to a serializable object
        var jsonString = JsonUtility.ToJson(new SerializableDictionary(activityData), true);
        LocalStorageManager.Instance.Save(jsonString);
    }
    
}