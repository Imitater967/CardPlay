using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script {
    public class SkillPreview : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler {
        public Image IconImg;
        public TMP_Text DescriptionTxt;
        public GameObject DescriptionObject;
        public void Initialize(Skill modelSkill) {
            IconImg.sprite = modelSkill.Icon;
            DescriptionTxt.text = modelSkill.Description;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            DescriptionObject.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData) {
            DescriptionObject.gameObject.SetActive(false);
        }
    }
}