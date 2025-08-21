using System.Collections.Generic;

[System.Serializable]
public class ActivityWrapper
{
    public string activityName;
    public List<QuestionData> questions = new List<QuestionData>();
}


[System.Serializable]
public class SerializableDictionary
{
    public List<ActivityWrapper> activities = new List<ActivityWrapper>();

    public SerializableDictionary(Dictionary<string, List<QuestionData>> dict)
    {
        foreach (var kvp in dict)
        {
            var wrapper = new ActivityWrapper();
            wrapper.activityName = kvp.Key;
            wrapper.questions = kvp.Value; // make sure this is not null
            activities.Add(wrapper);
        }
    }
}