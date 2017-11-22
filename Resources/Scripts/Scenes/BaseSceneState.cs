using UnityEngine;
using System.Collections;

namespace Game
{

    public class BaseSceneState
    {
        protected string m_sceneName;
        protected ESceneState m_eState;
        protected SceneController m_controller;
        protected bool m_bRunning = false;
        protected bool m_bLoaded = false;


        public BaseSceneState(SceneController controller)
        {
            m_controller = controller;
            m_eState = ESceneState.E_SCENE_STATE_LOGIN;
        }

        public string GetSceneName()
        {
            return m_sceneName;
        }

        public ESceneState GetESceneState()
        {
            return m_eState;
        }

        public bool isLoaded()
        {
            return m_bLoaded;
        }

        public bool isRunning()
        {
            return m_bRunning;
        }

        public virtual void SceneStateBegin()
        {

        }

        public virtual void SceneStateEnd()
        {

        }

        public virtual void SceneStateUpdate()
        {

        }
    }
}

