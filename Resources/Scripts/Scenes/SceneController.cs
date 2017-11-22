using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public enum ESceneState
    {
        E_SCENE_STATE_LOGIN,
        E_SCENE_STATE_SELECT_TEAM,
        // E_SCENE_STATE_MAIN_MENU,
        E_SCENE_STATE_GAME
    }

    public class SceneController
    {
        private bool m_bSceneLoaded = false;
        private BaseSceneState m_state;

        public SceneController()
        {
            //初始状态为Login状态
            m_state = new LoginSceneState(this);
            m_state.SceneStateBegin();

        }

        public void SetSceneState(BaseSceneState sceneState)
        {
            if (m_state.GetESceneState() != sceneState.GetESceneState())
            {
                //开始准备切换场景
                m_bSceneLoaded = false;
                //清理当前场景
                m_state.SceneStateEnd();
                //设置新场景
                m_state = sceneState;
                //加载场景
                LoadScene(m_state.GetSceneName());
            }
        }



        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }


        public void UpdateSceneState()
        {
            if (m_state == null || m_state.isLoaded() == false)
            {
                return;
            }

            if (m_state.isRunning() == false)
            {
                m_state.SceneStateBegin();
            }

            m_state.SceneStateUpdate();
        }

    }
}


 