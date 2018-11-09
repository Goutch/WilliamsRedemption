using UnityEngine;
using UnityEngine.Networking;

namespace Game.Web
{
    public class LeaderBoard : MonoBehaviour
    {
        public bool InsertDataToDatabase(string name, int score, int time)
        {
            WWWForm form = new WWWForm();
            form.AddField("name",name);
            form.AddField("score_value",score);
            form.AddField("time", time);

            using (UnityWebRequest www = UnityWebRequest.Post("http://35.188.160.44/api/insert", form))
            {
                if (www.isNetworkError || www.isHttpError)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}