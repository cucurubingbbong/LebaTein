using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ZBK.Scene
{
    public class SceneManager : MonoBehaviour
    {
        /// <summary>
        /// fade 이미지
        /// </summary>
        [SerializeField] Image m_fadeImg = null;

        /// <summary>
        /// Fade 중인지
        /// 0 : in , 1 : out
        /// </summary>
        [SerializeField] Color[] m_fadeColorArr = null;

        /// <summary>
        /// �ε� ������Ʈ �迭
        /// 0 : �ؽ�Ʈ , 1 : �ε� �����̴�
        /// </summary>
        [SerializeField] GameObject[] m_loadingObjArr = null;

        /// <summary>
        /// �ε� �����̴�
        /// </summary>
        [SerializeField] Slider m_loadingSlider = null;

        /// <summary>
        /// ������ �÷�
        /// </summary>
        Color m_nowColor = Color.black;

        /// <summary>
        /// �� �̵� �÷���
        /// </summary>
        bool m_changeSceneFlag = false;

        /// <summary>
        /// �� �ε�
        /// </summary>
        /// <param name="argSceneName">�̵��� �� �̸�</param>
        public void Load(string argSceneName)
        {
            if (m_changeSceneFlag) return;

            m_changeSceneFlag = true;

            StartCoroutine(ChangeScene(argSceneName));

        }

        /// <summary>
        /// �� �̵�
        /// </summary>
        /// <param name="argSceneName">�̵��� �� �̸�</param>
        /// <returns></returns>
        IEnumerator ChangeScene(string argSceneName)
        {
            m_fadeImg.raycastTarget = true;
            for (int i = 0; i < m_loadingObjArr.Length; i++) m_loadingObjArr[i].SetActive(true);
            m_loadingSlider.value = 0.0f;
            m_nowColor.a = m_fadeColorArr[0].a;
            while (m_nowColor.a != m_fadeColorArr[1].a)
            {
                m_nowColor.a += Time.deltaTime;
                m_nowColor.a = m_nowColor.a > 1.0f ? 1.0f : m_nowColor.a;
                m_fadeImg.color = m_nowColor;
                yield return null;
            }

            AsyncOperation _async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(argSceneName);
            _async.allowSceneActivation = false;

            while (_async.progress < 0.9f)
            {
                m_loadingSlider.value = _async.progress;
                yield return null;
            }

            _async.allowSceneActivation = true;

            m_loadingSlider.value = 1.0f;

            m_nowColor.a = m_fadeColorArr[1].a;
            while (m_nowColor.a != m_fadeColorArr[0].a)
            {
                m_nowColor.a -= Time.deltaTime;
                m_nowColor.a = m_nowColor.a < 0.0f ? 0.0f : m_nowColor.a;
                m_fadeImg.color = m_nowColor;
                yield return null;
            }

            for (int i = 0; i < m_loadingObjArr.Length; i++) m_loadingObjArr[i].SetActive(false);
            m_fadeImg.raycastTarget = false;

            m_changeSceneFlag = false;
        }

        public bool IsLoad {  get { return m_changeSceneFlag; } } 
    }
}