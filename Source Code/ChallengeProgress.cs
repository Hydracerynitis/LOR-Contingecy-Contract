using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSave;
using System.Threading.Tasks;

namespace Contingecy_Contract
{
    public class ChallengeProgress: Savable
    {
        public int Philiph_Risk;
        public int Eileen_Risk;
        public int Jaeheon_Risk;
        public int Elena_Risk;
        public int Orange_Path;
        public int Pluto_Risk;
        public int Ensemble_Complete;
        public SaveData GetSaveData() 
        {
            SaveData save = new SaveData();
            save.AddData("Philiph_Risk", new SaveData(Philiph_Risk));
            save.AddData("Eileen_Risk", new SaveData(Eileen_Risk));
            save.AddData("Jaeheon_Risk", new SaveData(Jaeheon_Risk));
            save.AddData(" Elena_Risk", new SaveData(Elena_Risk));
            save.AddData("Orange_Path", new SaveData(Orange_Path));
            save.AddData("Pluto_Risk", new SaveData(Pluto_Risk));
            save.AddData("Ensemble_Complete", new SaveData(Ensemble_Complete));
            return save;
        }
        public void LoadFromSaveData(SaveData data)
        {
            try
            {
                if (data == null)
                {
                    Philiph_Risk = 0;
                    Eileen_Risk = 0;
                    Jaeheon_Risk = 0;
                    Elena_Risk = 0;
                    Orange_Path = 0;
                    Pluto_Risk = 0;
                    Ensemble_Complete = 0;
                    return;
                }
                Philiph_Risk = data.GetInt("Philiph_Risk");                
                Debug.Log("Philiph_Risk: {0}", Philiph_Risk.ToString());
                Eileen_Risk = data.GetInt("Eileen_Risk");
                Debug.Log("Eileen_Risk: {0}", Eileen_Risk.ToString());
                Jaeheon_Risk = data.GetInt("Jaeheon_Risk");
                Debug.Log("Jaeheon_Risk: {0}", Jaeheon_Risk.ToString());
                Elena_Risk = data.GetInt("Elena_Risk");
                Debug.Log("Elena_Risk: {0}", Elena_Risk.ToString());
                Orange_Path = data.GetInt("Orange_Path");
                Debug.Log("Orange_Path: {0}", Orange_Path.ToString());
                Pluto_Risk = data.GetInt("Pluto_Risk");
                Debug.Log("Pluto_Risk: {0}", Pluto_Risk.ToString());
                Ensemble_Complete = data.GetInt("Ensemble_Complete");
                Debug.Log("Ensemble_Complete: {0}", Ensemble_Complete.ToString());
                Debug.SaveDebug();
            }
            catch(Exception ex)
            {
                Debug.Error("LoadErrorInDepth", ex);
            }

        }
    }
}
